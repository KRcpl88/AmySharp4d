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

class HistoryTable;
class ITransTable;

class MoveGenerator2 : public tgreiner::amy::common::engine::Generator {
public:
    MoveGenerator2(ChessBoard& theBoard, ITransTable& theTtable, HistoryTable& theHistory);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

    int getKiller1() const;
    int getKiller2() const;
    void setTwoPliesBelow(MoveGenerator2* gen);

private:
    static constexpr int HASHMOVE = 0;
    static constexpr int GENERATE_CAPTURES = HASHMOVE + 1;
    static constexpr int GAINING_CAPTURES = GENERATE_CAPTURES + 1;
    static constexpr int KILLER1 = GAINING_CAPTURES + 1;
    static constexpr int KILLER2 = KILLER1 + 1;
    static constexpr int KILLER3 = KILLER2 + 1;
    static constexpr int LOOSING_CAPTURES = KILLER3 + 1;
    static constexpr int GENERATE = LOOSING_CAPTURES + 1;
    static constexpr int REST = GENERATE + 1;

    int phase_{HASHMOVE};
    int idx_{0};
    int hashMove_{0};
    int killer1_{0};
    int killer1cnt_{0};
    int killer2_{0};
    int killer2cnt_{0};
    int killer3_{0};
    tgreiner::amy::common::engine::IntVector moves_;
    tgreiner::amy::common::engine::IntVector captures_;
    tgreiner::amy::common::engine::IntVector swapOffs_;
    int nCaptures_{0};
    ITransTable& ttable_;
    HistoryTable& history_;
    ChessBoard& board_;
    Swapper swapper_;
    MoveGenerator2* twoPliesBelow_{nullptr};
    bool inCheck_{false};
};

} // namespace tgreiner::amy::chess::engine
