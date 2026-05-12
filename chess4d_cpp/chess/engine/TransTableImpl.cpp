#include "chess/engine/TransTableImpl.h"

namespace tgreiner::amy::chess::engine {

TransTableImpl::TransTableImpl(int bits)
    : size_(1 << bits), mask_(size_ - 1), table_(static_cast<std::size_t>(size_))
{
}

TTEntry* TransTableImpl::get_Renamed(std::int64_t hashkey)
{
    const int idx = static_cast<int>(hashkey & mask_);
    TTEntry& entry = table_[static_cast<std::size_t>(idx)];
    if (entry.hashkey != hashkey) {
        return nullptr;
    }
    return &entry;
}

void TransTableImpl::store(std::int64_t hashkey, int move, int depth, int score, int alpha, int beta)
{
    const int idx = static_cast<int>(hashkey & mask_);
    TTEntry& entry = table_[static_cast<std::size_t>(idx)];
    if (depth > entry.getDepth()) {
        entry.set_Renamed(hashkey, move, depth, score, alpha, beta);
    }
}

void TransTableImpl::clear()
{
    for (TTEntry& entry : table_) {
        entry.set_Renamed(0, 0, 0, 0, 0, 0);
    }
}

} // namespace tgreiner::amy::chess::engine
