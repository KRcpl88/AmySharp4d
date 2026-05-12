/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/EvalMasks.h"
#include "bitboard/BoardConstants.h"

using namespace tgreiner::amy::bitboard;

namespace tgreiner::amy::chess::engine {

// Static member definitions
std::vector<BitBoard> EvalMasks::WHITE_BACKWARD;
std::vector<BitBoard> EvalMasks::BLACK_BACKWARD;
std::vector<BitBoard> EvalMasks::ISOLATED;
std::vector<BitBoard> EvalMasks::WHITE_DOUBLED;
std::vector<BitBoard> EvalMasks::BLACK_DOUBLED;
std::vector<BitBoard> EvalMasks::WHITE_PASSED;
std::vector<BitBoard> EvalMasks::BLACK_PASSED;
std::array<BitBoard, 8> EvalMasks::FILE_MASK;
std::array<BitBoard, 8> EvalMasks::RANK_MASK;
BitBoard EvalMasks::BLACK_SQUARES;
BitBoard EvalMasks::WHITE_SQUARES;
BitBoard EvalMasks::WHITE_KING_IN_CENTER;
BitBoard EvalMasks::BLACK_KING_IN_CENTER;

EvalMasks::StaticInit EvalMasks::staticInit_;

void EvalMasks::initMasks()
{
    // -----------------------------------------------------------------------
    // White backward pawn masks.
    // For square i: set all squares on adjacent files (file-1 and file+1)
    // with rank LESS than i's rank (i.e. squares behind a white pawn).
    // The C# uses "i & 7" as rank and "i / 8" as file for the flat layout.
    // -----------------------------------------------------------------------
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        if ((i & 7) > 0) {
            for (int j = i - 1; j >= 0; j -= 8) {
                WHITE_BACKWARD[i].SetBit(j);
            }
        }
        if ((i & 7) < 7) {
            for (int j = i + 1; j >= 0; j -= 8) {
                WHITE_BACKWARD[i].SetBit(j);
            }
        }
    }

    // Black backward pawn masks (squares ahead of black pawn on adjacent files).
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        if ((i & 7) > 0) {
            for (int j = i - 1; j < BitBoard::SIZE; j += 8) {
                BLACK_BACKWARD[i].SetBit(j);
            }
        }
        if ((i & 7) < 7) {
            for (int j = i + 1; j < BitBoard::SIZE; j += 8) {
                BLACK_BACKWARD[i].SetBit(j);
            }
        }
    }

    // Isolated pawn masks: all squares on adjacent files.
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        if ((i & 7) > 0) {
            for (int j = (i & 7) - 1; j < BitBoard::SIZE; j += 8) {
                ISOLATED[i].SetBit(j);
            }
        }
        if ((i & 7) < 7) {
            for (int j = (i & 7) + 1; j < BitBoard::SIZE; j += 8) {
                ISOLATED[i].SetBit(j);
            }
        }
    }

    // White doubled pawn masks: all squares on same file below square i.
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        for (int j = i - 8; j >= 0; j -= 8) {
            WHITE_DOUBLED[i].SetBit(j);
        }
    }

    // Black doubled pawn masks: all squares on same file above square i.
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        for (int j = i + 8; j < BitBoard::SIZE; j += 8) {
            BLACK_DOUBLED[i].SetBit(j);
        }
    }

    // File masks (index 0..7, covers full board column).
    for (int i = 0; i < 8; ++i) {
        for (int j = i; j < BitBoard::SIZE; j += 8) {
            FILE_MASK[i].SetBit(j);
        }
    }

    // Rank masks (index 0..7, covers 8 consecutive squares).
    for (int i = 0; i < 8; ++i) {
        for (int j = 0; j < 8; ++j) {
            RANK_MASK[i].SetBit(8 * i + j);
        }
    }

    // White passed pawn masks: squares ahead on same and adjacent files.
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        for (int j = i + 8; j < BitBoard::SIZE; j += 8) {
            WHITE_PASSED[i].SetBit(j);
            if ((j & 7) > 0) WHITE_PASSED[i].SetBit(j - 1);
            if ((j & 7) < 7) WHITE_PASSED[i].SetBit(j + 1);
        }
    }

    // Black passed pawn masks: squares behind (lower rank) on same and adjacent files.
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        for (int j = i - 8; j >= 0; j -= 8) {
            BLACK_PASSED[i].SetBit(j);
            if ((j & 7) > 0) BLACK_PASSED[i].SetBit(j - 1);
            if ((j & 7) < 7) BLACK_PASSED[i].SetBit(j + 1);
        }
    }
}

EvalMasks::StaticInit::StaticInit()
{
    WHITE_BACKWARD = BitBoard::CreateArray(BitBoard::SIZE);
    BLACK_BACKWARD = BitBoard::CreateArray(BitBoard::SIZE);
    ISOLATED       = BitBoard::CreateArray(BitBoard::SIZE);
    WHITE_DOUBLED  = BitBoard::CreateArray(BitBoard::SIZE);
    BLACK_DOUBLED  = BitBoard::CreateArray(BitBoard::SIZE);
    WHITE_PASSED   = BitBoard::CreateArray(BitBoard::SIZE);
    BLACK_PASSED   = BitBoard::CreateArray(BitBoard::SIZE);

    // King-in-centre masks: squares near the centre of level H.
    WHITE_KING_IN_CENTER = BitBoard({
        BoardConstants::HE1,
        BoardConstants::HE2,
        BoardConstants::HD1,
        BoardConstants::HD2
    });

    BLACK_KING_IN_CENTER = BitBoard({
        BoardConstants::HE8,
        BoardConstants::HE7,
        BoardConstants::HD8,
        BoardConstants::HD7
    });

    // Black/white square colouring (level H only; left as empty in C# BUGBUG).
    // We populate them faithfully for level H using the standard chessboard
    // colouring: a square (file f, rank r) is "dark" if (f+r) is odd.
    BLACK_SQUARES = BitBoard();
    WHITE_SQUARES = BitBoard();
    for (int file = 0; file < 8; ++file) {
        for (int rank = 0; rank < 8; ++rank) {
            int sq = BoardConstants::LH + rank * 8 + file;
            if ((file + rank) % 2 == 1) {
                BLACK_SQUARES.SetBit(sq);
            } else {
                WHITE_SQUARES.SetBit(sq);
            }
        }
    }

    initMasks();
}

} // namespace tgreiner::amy::chess::engine
