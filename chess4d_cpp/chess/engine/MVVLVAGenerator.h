/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "bitboard/BitBoard.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/Move.h"
#include "common/engine/Generator.h"

namespace tgreiner::amy::chess::engine {

class MVVLVAGenerator : public tgreiner::amy::common::engine::Generator {
public:
    explicit MVVLVAGenerator(ChessBoard& theBoard);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

protected:
    ChessBoard& board;

private:
    void nextVictim();
    int nextAttacker();

    int victim_{0};
    int attacker_{0};
    int victimSq_{-1};
    tgreiner::amy::bitboard::BitBoard victims_;
    tgreiner::amy::bitboard::BitBoard attackers_;
    bool whiteToMove_{true};
};

} // namespace tgreiner::amy::chess::engine
