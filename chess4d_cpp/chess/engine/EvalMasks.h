/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "bitboard/BitBoard.h"
#include <array>

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;

/// Contains masks used by the Evaluator.
/// All arrays are indexed by board square (0..BitBoard::SIZE-1).
/// NOTE: As in the original source, these masks are only meaningful for
/// level H (the standard 8×8 board, offsets 140..203).  The comments in
/// the C# source acknowledge this as a BUGBUG.  They are faithfully ported
/// as-is.
class EvalMasks {
public:
    EvalMasks() = delete;

    /// Masks for white backward pawns.
    static std::vector<BitBoard> WHITE_BACKWARD;

    /// Masks for black backward pawns.
    static std::vector<BitBoard> BLACK_BACKWARD;

    /// Masks for isolated pawns.
    static std::vector<BitBoard> ISOLATED;

    /// Masks for white doubled pawns (squares below i on same file).
    static std::vector<BitBoard> WHITE_DOUBLED;

    /// Masks for black doubled pawns (squares above i on same file).
    static std::vector<BitBoard> BLACK_DOUBLED;

    /// Masks for white passed pawns (squares ahead of white pawn on same/adj file).
    static std::vector<BitBoard> WHITE_PASSED;

    /// Masks for black passed pawns (squares ahead of black pawn on same/adj file).
    static std::vector<BitBoard> BLACK_PASSED;

    /// File masks (file 0..7 → all squares on that file of level H).
    static std::array<BitBoard, 8> FILE_MASK;

    /// Rank masks (rank 0..7 → all squares on that rank of level H).
    static std::array<BitBoard, 8> RANK_MASK;

    /// All black (dark) squares on level H.
    static BitBoard BLACK_SQUARES;

    /// All white (light) squares on level H.
    static BitBoard WHITE_SQUARES;

    /// Squares where white king is considered "in the centre" (e1,e2,d1,d2).
    static BitBoard WHITE_KING_IN_CENTER;

    /// Squares where black king is considered "in the centre" (e8,e7,d8,d7).
    static BitBoard BLACK_KING_IN_CENTER;

private:
    static void initMasks();

    struct StaticInit {
        StaticInit();
    };
    static StaticInit staticInit_;
};

} // namespace tgreiner::amy::chess::engine
