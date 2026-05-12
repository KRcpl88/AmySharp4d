/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "chess/engine/MVVLVAGenerator.h"
#include "common/engine/IntVector.h"

namespace tgreiner::amy::chess::engine {

class HistoryTable;
class ITransTable;

class MoveGenerator : public MVVLVAGenerator {
public:
    MoveGenerator(ChessBoard& board, ITransTable& theTransTable, HistoryTable& theHistory);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

private:
    static constexpr int HASHMOVE = 0;
    static constexpr int CAPTURES = 1;
    static constexpr int KILLER1 = 2;
    static constexpr int KILLER2 = 3;
    static constexpr int GENERATE = 4;
    static constexpr int REST = 5;

    int phase_{HASHMOVE};
    int idx_{0};
    int hashmove_{0};
    int killer1_{0};
    int killer1cnt_{0};
    int killer2_{0};
    int killer2cnt_{0};
    tgreiner::amy::common::engine::IntVector moves_;
    ITransTable& ttable_;
    HistoryTable& history_;
};

} // namespace tgreiner::amy::chess::engine
