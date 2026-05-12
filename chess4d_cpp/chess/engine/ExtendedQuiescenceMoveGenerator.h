/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "common/engine/Generator.h"
#include "common/engine/IntVector.h"
#include "chess/engine/CheckingMoveGenerator.h"
#include "chess/engine/Swapper.h"

namespace tgreiner::amy::chess::engine {

class ITransTable;

class ExtendedQuiescenceMoveGenerator : public tgreiner::amy::common::engine::Generator {
public:
    ExtendedQuiescenceMoveGenerator(ChessBoard& theBoard, ITransTable& theTransTable);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

private:
    static constexpr int HASH_MOVE = 0;
    static constexpr int GENERATE_CAPTURES = HASH_MOVE + 1;
    static constexpr int GAINING_CAPTURES = GENERATE_CAPTURES + 1;
    static constexpr int GENERATE_CHECKS = GAINING_CAPTURES + 1;
    static constexpr int CHECKS = GENERATE_CHECKS + 1;
    static constexpr int LOOSING_CAPTURES = CHECKS + 1;

    ChessBoard& board_;
    tgreiner::amy::common::engine::IntVector moves_;
    tgreiner::amy::common::engine::IntVector checkingMoves_;
    tgreiner::amy::common::engine::IntVector swapOffs_;
    int nMoves_{0};
    int phase_{HASH_MOVE};
    Swapper swapper_;
    CheckingMoveGenerator checkGenerator_;
    int nChecks_{0};
    ITransTable& transTable_;
    int hashMove_{0};
};

} // namespace tgreiner::amy::chess::engine
