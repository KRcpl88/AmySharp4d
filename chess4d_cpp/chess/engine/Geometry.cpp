/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/Geometry.h"
#include "bitboard/BitBoard.h"
#include "bitboard/LevelRankFile.h"
#include "bitboard/UniversalCoordinate.h"

#include <cassert>
#include <stdexcept>

using namespace tgreiner::amy::bitboard;

namespace tgreiner::amy::chess::engine {

// ---------------------------------------------------------------------------
// Static member storage
// ---------------------------------------------------------------------------
std::vector<std::vector<std::vector<short>>> Geometry::NEXT_POS;
std::vector<std::vector<std::vector<short>>> Geometry::NEXT_DIR;
std::vector<std::vector<short>>              Geometry::NEXT_SQ;
std::vector<std::vector<BitBoard>>           Geometry::RAY;
std::vector<std::vector<BitBoard>>           Geometry::INTER_PATH;
std::vector<BitBoard>                        Geometry::WHITE_PAWN_EPM;
std::vector<BitBoard>                        Geometry::BLACK_PAWN_EPM;
std::vector<BitBoard>                        Geometry::KNIGHT_EPM;
std::vector<BitBoard>                        Geometry::BISHOP_EPM;
std::vector<BitBoard>                        Geometry::ROOK_EPM;
std::vector<BitBoard>                        Geometry::QUEEN_EPM;
std::vector<BitBoard>                        Geometry::KING_EPM;

// ---------------------------------------------------------------------------
// Attack deltas  (3-D board, UCoord(dx, dy, dz))
// index 0 unused; indices 1-7 match piece-type constants; index 7 = BLACK_PAWN
// ---------------------------------------------------------------------------
const std::vector<std::vector<UCoord>> Geometry::ATTACK_DELTA = {
    // [0] – unused
    {},
    // [1] WHITE_PAWN – attacks forward (rank+) on the same level and diagonally to adjacent levels
    { UCoord(2,0,0), UCoord(0,2,0), UCoord(0,0,2), UCoord(0,0,-2) },
    // [2] KNIGHT
    {
        UCoord( 0, 1, 3), UCoord(-1, 0, 3), UCoord( 0,-1, 3), UCoord( 1, 0, 3),
        UCoord( 0, 3, 1), UCoord(-3, 0, 1), UCoord( 0,-3, 1), UCoord( 3, 0, 1),
        UCoord( 3, 1, 0), UCoord( 3,-1, 0), UCoord( 1,-3, 0), UCoord(-1,-3, 0),
        UCoord(-3, 1, 0), UCoord(-3,-1, 0), UCoord( 1, 3, 0), UCoord(-1, 3, 0),
        UCoord( 0, 1,-3), UCoord(-1, 0,-3), UCoord( 0,-1,-3), UCoord( 1, 0,-3),
        UCoord( 0, 3,-1), UCoord(-3, 0,-1), UCoord( 0,-3,-1), UCoord( 3, 0,-1),
    },
    // [3] BISHOP  (diagonal axes in 3-D – pairs of axes)
    {
        UCoord( 0, 0, 2), UCoord( 2, 0, 0), UCoord( 0,-2, 0),
        UCoord(-2, 0, 0), UCoord( 0, 2, 0), UCoord( 0, 0,-2)
    },
    // [4] ROOK  (axis-aligned directions)
    {
        UCoord( 0, 1, 1), UCoord(-1, 0, 1), UCoord( 0,-1, 1), UCoord( 1, 0, 1),
        UCoord( 1, 1, 0), UCoord( 1,-1, 0), UCoord(-1,-1, 0), UCoord(-1, 1, 0),
        UCoord( 0, 1,-1), UCoord(-1, 0,-1), UCoord( 0,-1,-1), UCoord( 1, 0,-1)
    },
    // [5] QUEEN  = BISHOP dirs + ROOK dirs
    {
        UCoord( 0, 0, 2), UCoord( 2, 0, 0), UCoord( 0,-2, 0),
        UCoord(-2, 0, 0), UCoord( 0, 2, 0), UCoord( 0, 0,-2),
        UCoord( 0, 1, 1), UCoord(-1, 0, 1), UCoord( 0,-1, 1), UCoord( 1, 0, 1),
        UCoord( 1, 1, 0), UCoord( 1,-1, 0), UCoord(-1,-1, 0), UCoord(-1, 1, 0),
        UCoord( 0, 1,-1), UCoord(-1, 0,-1), UCoord( 0,-1,-1), UCoord( 1, 0,-1)
    },
    // [6] KING  (one step in every queen direction)
    {
        UCoord( 0, 0, 2), UCoord( 2, 0, 0), UCoord( 0,-2, 0),
        UCoord(-2, 0, 0), UCoord( 0, 2, 0), UCoord( 0, 0,-2),
        UCoord( 0, 1, 1), UCoord(-1, 0, 1), UCoord( 0,-1, 1), UCoord( 1, 0, 1),
        UCoord( 1, 1, 0), UCoord( 1,-1, 0), UCoord(-1,-1, 0), UCoord(-1, 1, 0),
        UCoord( 0, 1,-1), UCoord(-1, 0,-1), UCoord( 0,-1,-1), UCoord( 1, 0,-1)
    },
    // [7] BLACK_PAWN – attacks backward (rank-) on the same level and diagonally
    { UCoord(0,-2,0), UCoord(-2,0,0), UCoord(0,0,2), UCoord(0,0,-2) }
};

// ---------------------------------------------------------------------------
// Helper directions on the 10×10 sentinel board (for 2-D INTER_PATH / RAY)
// ---------------------------------------------------------------------------
static const int DIRS_10[]          = { 1, -1, 10, -10, 9, -9, 11, -11 };
static const int DIRS_BISHOP_10[]   = { 9, -9, 11, -11 };
static const int DIRS_ROOK_10[]     = { 1, -1, 10, -10 };
static const int OFFSETS_KNIGHT_10[]= { 19, 21, -19, -21, 12, 8, -12, -8 };
static const int OFFSETS_KING_10[]  = { -11, -10, -9, -1, 1, 9, 10, 11 };

// ---------------------------------------------------------------------------
// initNextPos – for non-sliding pieces (pawns, knights, kings)
// ---------------------------------------------------------------------------
void Geometry::initNextPos(int piece)
{
    for (int square = 0; square < SIZE; ++square) {
        Lfr levelFileRank(square);
        int prevSquare = square;

        for (const UCoord& delta : ATTACK_DELTA[piece]) {
            UCoord temp = UCoord(levelFileRank) + delta;
            Lfr nextLfr = static_cast<Lfr>(temp);
            if (nextLfr.IsValid()) {
                int ns = static_cast<int>(nextLfr);
                NEXT_POS[piece][square][prevSquare] = static_cast<short>(ns);
                NEXT_DIR[piece][square][prevSquare] = static_cast<short>(ns);
                prevSquare = ns;
            }
        }
    }
}

// ---------------------------------------------------------------------------
// initSlidingNextPos – for sliding pieces (bishop, rook, queen)
// ---------------------------------------------------------------------------
void Geometry::initSlidingNextPos(int piece)
{
    const auto& deltas = ATTACK_DELTA[piece];

    for (int square = 0; square < SIZE; ++square) {
        long long nextDirection     = -1;
        long long nextNextDirection = -1;

        NEXT_DIR[piece][square][square] = -1;

        // Enumerate directions in reverse so that NEXT_DIR[square][square]
        // ends up pointing to the first direction's start (same as C# code).
        for (int idxDir = (int)deltas.size() - 1; idxDir >= 0; --idxDir) {
            const UCoord& delta = deltas[idxDir];
            UCoord nextCoord    = UCoord(Lfr(square)) + delta;
            Lfr    nextLfr      = static_cast<Lfr>(nextCoord);
            int    prevSquare   = square;

            if (nextLfr.IsValid()) {
                int ns = static_cast<int>(nextLfr);
                NEXT_POS[piece][square][square] = static_cast<short>(ns);
                nextNextDirection               = ns;
            }

            if (nextDirection != -1) {
                NEXT_DIR[piece][square][square] =
                    static_cast<short>(nextDirection);
            }

            // Walk along this direction
            while (nextLfr.IsValid()) {
                int ns = static_cast<int>(nextLfr);
                NEXT_POS[piece][square][prevSquare] = static_cast<short>(ns);

                if (piece == QUEEN && square != prevSquare) {
                    NEXT_SQ[square][prevSquare] =
                        NEXT_POS[piece][square][prevSquare];
                }

                prevSquare = ns;
                nextCoord  = nextCoord + delta;
                nextLfr    = static_cast<Lfr>(nextCoord);
                NEXT_DIR[piece][square][prevSquare] =
                    static_cast<short>(nextDirection);
            }

            // End of ray: both NEXT_POS and NEXT_DIR point to start of next dir
            NEXT_POS[piece][square][prevSquare] =
                static_cast<short>(nextDirection);
            NEXT_DIR[piece][square][prevSquare] =
                static_cast<short>(nextDirection);

            nextDirection = nextNextDirection;
        }
    }
}

// ---------------------------------------------------------------------------
// initMoves
// ---------------------------------------------------------------------------
void Geometry::initMoves()
{
    // Initialise all tables to -1
    for (int piece = WHITE_PAWN; piece <= BLACK_PAWN; ++piece) {
        for (int sq = 0; sq < SIZE; ++sq) {
            for (int sq2 = 0; sq2 < SIZE; ++sq2) {
                NEXT_POS[piece][sq][sq2] = -1;
                NEXT_DIR[piece][sq][sq2] = -1;
            }
        }
    }
    for (int sq = 0; sq < SIZE; ++sq) {
        for (int sq2 = 0; sq2 < SIZE; ++sq2) {
            NEXT_SQ[sq][sq2] = -1;
        }
    }

    initNextPos(WHITE_PAWN);
    initNextPos(BLACK_PAWN);
    initNextPos(KNIGHT);
    initNextPos(KING);
    initSlidingNextPos(BISHOP);
    initSlidingNextPos(ROOK);
    initSlidingNextPos(QUEEN);
}

// ---------------------------------------------------------------------------
// initGeometry – INTER_PATH, RAY, and EPM tables
// (uses a 10×10 sentinel board for the 2-D 8×8 level-H subset)
// ---------------------------------------------------------------------------
void Geometry::initGeometry()
{
    // 10×10 sentinel board for the H-level 8×8 squares
    bool edge[100]{};
    int  trto[100]{};   // 10×10 → bitboard index
    int  trfr[SIZE]{};  // bitboard index → 10×10

    for (int i = 0; i < 10; ++i) {
        edge[i]      = true;
        edge[90 + i] = true;
        edge[10 * i]     = true;
        edge[10 * i + 9] = true;

        for (int j = 0; j < 10; ++j) {
            int x = i - 1;
            int y = j - 1;
            if (x >= 0 && y >= 0 && x < 8 && y < 8) {
                // Map to level-H bitboard offset: rank=y, file=x
                // BitBoard::BitOffset(level=7, file=x, rank=y)
                int bbIdx = BitBoard::BitOffset(7, x, y);
                trto[i + 10 * j] = bbIdx;
                trfr[bbIdx]      = i + 10 * j;
            }
        }
    }

    for (int j = 0; j < 100; ++j) {
        if (edge[j]) continue;
        int x = trto[j];

        // INTER_PATH and RAY (all 8 directions)
        for (int d : DIRS_10) {
            for (int k = j + d; !edge[k]; k += d) {
                int y = trto[k];
                for (int l = j + d; l != k; l += d) {
                    INTER_PATH[x][y].SetBit(trto[l]);
                }
                for (int l = k + d; !edge[l]; l += d) {
                    RAY[x][y].SetBit(trto[l]);
                }
            }
        }

        // BISHOP_EPM / QUEEN_EPM
        for (int d : DIRS_BISHOP_10) {
            for (int k = j + d; !edge[k]; k += d) {
                BISHOP_EPM[x].SetBit(trto[k]);
                QUEEN_EPM[x].SetBit(trto[k]);
            }
        }

        // ROOK_EPM / QUEEN_EPM
        for (int d : DIRS_ROOK_10) {
            for (int k = j + d; !edge[k]; k += d) {
                ROOK_EPM[x].SetBit(trto[k]);
                QUEEN_EPM[x].SetBit(trto[k]);
            }
        }

        // KNIGHT_EPM
        for (int off : OFFSETS_KNIGHT_10) {
            int k = j + off;
            if (k >= 0 && k < 100 && !edge[k]) {
                KNIGHT_EPM[x].SetBit(trto[k]);
            }
        }

        // KING_EPM
        for (int off : OFFSETS_KING_10) {
            int k = j + off;
            if (k >= 0 && k < 100 && !edge[k]) {
                KING_EPM[x].SetBit(trto[k]);
            }
        }

        // WHITE_PAWN_EPM (captures: +9, +11 on 10×10)
        if (!edge[j + 9])  WHITE_PAWN_EPM[x].SetBit(trto[j + 9]);
        if (!edge[j + 11]) WHITE_PAWN_EPM[x].SetBit(trto[j + 11]);

        // BLACK_PAWN_EPM (captures: -9, -11 on 10×10)
        if (!edge[j - 9])  BLACK_PAWN_EPM[x].SetBit(trto[j - 9]);
        if (!edge[j - 11]) BLACK_PAWN_EPM[x].SetBit(trto[j - 11]);
    }
}

// ---------------------------------------------------------------------------
// invertRank
// ---------------------------------------------------------------------------
int Geometry::invertRank(int square)
{
    Lfr lfr(square);
    int level = lfr.Level;
    lfr.Rank = BitBoard::LEVEL_WIDTH[level] - lfr.Rank - 1;
    return static_cast<int>(lfr);
}

// ---------------------------------------------------------------------------
// Static initialiser
// ---------------------------------------------------------------------------
Geometry::StaticInit::StaticInit()
{
    // Allocate NEXT_POS [LAST_PIECE][SIZE][SIZE]
    NEXT_POS.assign(LAST_PIECE,
        std::vector<std::vector<short>>(SIZE,
            std::vector<short>(SIZE, -1)));

    // Allocate NEXT_DIR [LAST_PIECE][SIZE][SIZE]
    NEXT_DIR.assign(LAST_PIECE,
        std::vector<std::vector<short>>(SIZE,
            std::vector<short>(SIZE, -1)));

    // Allocate NEXT_SQ [SIZE][SIZE]
    NEXT_SQ.assign(SIZE, std::vector<short>(SIZE, -1));

    // Allocate RAY [SIZE][SIZE]
    RAY.assign(SIZE, BitBoard::CreateArray(SIZE));

    // Allocate INTER_PATH [SIZE][SIZE]
    INTER_PATH.assign(SIZE, BitBoard::CreateArray(SIZE));

    // EPM arrays [SIZE]
    WHITE_PAWN_EPM = BitBoard::CreateArray(SIZE);
    BLACK_PAWN_EPM = BitBoard::CreateArray(SIZE);
    KNIGHT_EPM     = BitBoard::CreateArray(SIZE);
    BISHOP_EPM     = BitBoard::CreateArray(SIZE);
    ROOK_EPM       = BitBoard::CreateArray(SIZE);
    QUEEN_EPM      = BitBoard::CreateArray(SIZE);
    KING_EPM       = BitBoard::CreateArray(SIZE);

    initMoves();
    initGeometry();
}

Geometry::StaticInit Geometry::staticInit_;

} // namespace tgreiner::amy::chess::engine
