/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "common/engine/Generator.h"
#include "common/engine/IntVector.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/Swapper.h"

namespace tgreiner::amy::chess::engine {

class NonLoosingCaptureMoveGenerator : public tgreiner::amy::common::engine::Generator {
public:
    explicit NonLoosingCaptureMoveGenerator(ChessBoard& theBoard);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

protected:
    ChessBoard& board;

private:
    static constexpr int GENERATE_CAPTURES = 0;
    static constexpr int GAINING_CAPTURES = GENERATE_CAPTURES + 1;

    tgreiner::amy::common::engine::IntVector moves_;
    tgreiner::amy::common::engine::IntVector swapOffs_;
    int nMoves_{0};
    int phase_{GENERATE_CAPTURES};
    Swapper swapper_;
};

} // namespace tgreiner::amy::chess::engine
