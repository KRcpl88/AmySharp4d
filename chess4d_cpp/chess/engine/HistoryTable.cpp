#include "chess/engine/HistoryTable.h"

#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

HistoryTable::HistoryTable() {
    reset();
}

void HistoryTable::addHistory(int move, int depth) {
    if ((move & (Move::CAPTURE | Move::ENPASSANT)) == 0) {
        table_[move & MASK] += depth * depth;
    }
}

void HistoryTable::reset() {
    table_.fill(0);
}

int HistoryTable::select(tgreiner::amy::common::engine::IntVector& moves, int size) {
    int bestidx = 0;
    int move = moves.get_Renamed(bestidx);
    int best = table_[move & MASK];

    for (int i = 1; i < size; ++i) {
        move = moves.get_Renamed(i);
        const int value = table_[move & MASK];
        if (value > best) {
            best = value;
            bestidx = i;
        }
    }

    move = moves.get_Renamed(bestidx);
    if (bestidx != (size - 1)) {
        moves.swap(bestidx, size - 1);
    }

    return move;
}

} // namespace tgreiner::amy::chess::engine
