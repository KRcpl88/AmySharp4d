/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <array>
#include <vector>
#include "bitboard/BitBoard.h"
#include "bitboard/LevelRankFile.h"
#include "bitboard/UniversalCoordinate.h"
#include "chess/engine/ChessConstants.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;
using tgreiner::amy::bitboard::Lfr;
using tgreiner::amy::bitboard::UCoord;

/// Encapsulates the geometry of the chess board.
/// All tables are initialised once at program start via the static initialiser.
class Geometry final {
public:
    // Re-export piece-type indices used inside Geometry tables.
    // WHITE_PAWN = 1, KNIGHT = 2, BISHOP = 3, ROOK = 4, QUEEN = 5, KING = 6
    // BLACK_PAWN is placed at index 7 (KING + 1)
    static constexpr int WHITE_PAWN = ChessConstants::PAWN;
    static constexpr int KNIGHT     = ChessConstants::KNIGHT;
    static constexpr int BISHOP     = ChessConstants::BISHOP;
    static constexpr int ROOK       = ChessConstants::ROOK;
    static constexpr int QUEEN      = ChessConstants::QUEEN;
    static constexpr int KING       = ChessConstants::KING;
    static constexpr int BLACK_PAWN = KING + 1;   // index 7
    static constexpr int LAST_PIECE = BLACK_PAWN + 1; // 8 (one past end)

    static constexpr int SIZE = BitBoard::SIZE; // 344

    // -----------------------------------------------------------------------
    // Main geometry tables
    // -----------------------------------------------------------------------

    /// NEXT_POS[piece][startSquare][currentSquare] → next square to visit
    /// (short; -1 = end)
    static std::vector<std::vector<std::vector<short>>> NEXT_POS;

    /// NEXT_DIR[piece][startSquare][currentSquare] → first square of next dir
    static std::vector<std::vector<std::vector<short>>> NEXT_DIR;

    /// NEXT_SQ[from][current] → next square on same ray (queen dirs only)
    static std::vector<std::vector<short>> NEXT_SQ;

    /// RAY[a][b] = bitboard of squares beyond b on the ray from a through b
    static std::vector<std::vector<BitBoard>> RAY;

    /// INTER_PATH[a][b] = bitboard of squares strictly between a and b
    static std::vector<std::vector<BitBoard>> INTER_PATH;

    /// Ever-possible-move bitboards
    static std::vector<BitBoard> WHITE_PAWN_EPM;
    static std::vector<BitBoard> BLACK_PAWN_EPM;
    static std::vector<BitBoard> KNIGHT_EPM;
    static std::vector<BitBoard> BISHOP_EPM;
    static std::vector<BitBoard> ROOK_EPM;
    static std::vector<BitBoard> QUEEN_EPM;
    static std::vector<BitBoard> KING_EPM;

    // -----------------------------------------------------------------------
    // Utility
    // -----------------------------------------------------------------------
    /// Invert rank for a square (a1↔a8, etc.)
    static int invertRank(int square);

    // Non-instantiable
    Geometry() = delete;

private:
    // attack deltas for the 3-D board (used to build NEXT_POS/NEXT_DIR)
    static const std::vector<std::vector<UCoord>> ATTACK_DELTA;

    static void initMoves();
    static void initNextPos(int piece);
    static void initSlidingNextPos(int piece);
    static void initGeometry();

    // Static-init helper (called once)
    struct StaticInit {
        StaticInit();
    };
    static StaticInit staticInit_;
};

} // namespace tgreiner::amy::chess::engine
