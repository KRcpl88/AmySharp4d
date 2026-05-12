/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "common/engine/Generator.h"
#include "common/engine/IMoveList.h"
#include "common/engine/IntVector.h"
#include "chess/engine/ChessBoard.h"

namespace tgreiner::amy::chess::engine {

class HistoryTable;
class ITransTable;

class CheckEvasionMoveGenerator : public tgreiner::amy::common::engine::Generator {
public:
    CheckEvasionMoveGenerator(ChessBoard& theBoard, ITransTable& theTtable, HistoryTable& theHistory);

    int nextMove() override;
    void reset() override;
    void failHigh(int move, int depth) override;

    void generateEvasions(tgreiner::amy::common::engine::IMoveList& theMoves);

private:
    static constexpr int HASHMOVE = 0;
    static constexpr int GENERATE_CAPTURES = HASHMOVE + 1;
    static constexpr int CAPTURES = GENERATE_CAPTURES + 1;
    static constexpr int KILLER1 = CAPTURES + 1;
    static constexpr int KILLER2 = KILLER1 + 1;
    static constexpr int GENERATE = KILLER2 + 1;
    static constexpr int REST = GENERATE + 1;

    int phase_{HASHMOVE};
    int idx_{0};
    int hashMove_{0};
    int killer1_{0};
    int killer1cnt_{0};
    int killer2_{0};
    int killer2cnt_{0};
    tgreiner::amy::common::engine::IntVector moves_;
    tgreiner::amy::common::engine::IntVector captures_;
    int nCaptures_{0};
    ITransTable& ttable_;
    HistoryTable& history_;
    ChessBoard& board_;
};

} // namespace tgreiner::amy::chess::engine
