/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <array>

#include "bitboard/BitBoard.h"

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class Swapper {
public:
    Swapper() = default;

    int swap(ChessBoard& board, int move);

private:
    tgreiner::amy::bitboard::BitBoard swapReRay(ChessBoard& board, const tgreiner::amy::bitboard::BitBoard& atks, int from, int to);

    std::array<int, 32> swaplist_{};
    inline static constexpr std::array<int, 7> PIECE_VALUES{0, 1, 3, 3, 5, 9, 100};
};

} // namespace tgreiner::amy::chess::engine
