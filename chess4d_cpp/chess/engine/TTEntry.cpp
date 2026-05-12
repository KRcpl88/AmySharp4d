#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

int TTEntry::getDepth() const {
    return depth_ & DEPTH_MASK;
}

bool TTEntry::getExact() const {
    return (static_cast<unsigned short>(depth_) & FLAG_MASK) == EXACT;
}

bool TTEntry::getLower() const {
    return (static_cast<unsigned short>(depth_) & FLAG_MASK) == LOWER;
}

bool TTEntry::getUpper() const {
    return (static_cast<unsigned short>(depth_) & FLAG_MASK) == UPPER;
}

void TTEntry::set_Renamed(std::int64_t newHashkey, int newMove, int newDepth, int newScore, int alpha, int beta) {
    hashkey = newHashkey;
    move = newMove;
    score = static_cast<short>(newScore);
    depth_ = static_cast<short>(newDepth & DEPTH_MASK);

    if (newScore <= alpha) {
        depth_ = static_cast<short>(depth_ | UPPER);
    } else if (newScore >= beta) {
        depth_ = static_cast<short>(depth_ | LOWER);
    } else {
        depth_ = static_cast<short>(depth_ | EXACT);
    }
}

} // namespace tgreiner::amy::chess::engine
