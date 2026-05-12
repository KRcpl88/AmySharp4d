#include "chess/engine/TransTableImpl2.h"

namespace tgreiner::amy::chess::engine {

TransTableImpl2::TransTableImpl2(int bits)
    : size_(1 << (bits - 1)),
      mask_(size_ - 1),
      table_(static_cast<std::size_t>(size_)),
      table2_(static_cast<std::size_t>(size_))
{
}

TTEntry* TransTableImpl2::get_Renamed(std::int64_t hashkey)
{
    const int idx = static_cast<int>(hashkey & mask_);
    TTEntry& entry = table_[static_cast<std::size_t>(idx)];
    if (entry.hashkey == hashkey) {
        return &entry;
    }

    TTEntry& entry2 = table2_[static_cast<std::size_t>(idx)];
    if (entry2.hashkey == hashkey) {
        return &entry2;
    }

    return nullptr;
}

void TransTableImpl2::store(std::int64_t hashkey, int move, int depth, int score, int alpha, int beta)
{
    const int idx = static_cast<int>(hashkey & mask_);

    TTEntry& entry = table_[static_cast<std::size_t>(idx)];
    if (entry.hashkey == hashkey) {
        entry.set_Renamed(hashkey, move, depth, score, alpha, beta);
        return;
    }

    TTEntry& entry2 = table2_[static_cast<std::size_t>(idx)];
    if (entry2.hashkey == hashkey) {
        entry2.set_Renamed(hashkey, move, depth, score, alpha, beta);
        return;
    }

    if (entry.getDepth() < entry2.getDepth()) {
        if (entry.getDepth() < depth) {
            entry.set_Renamed(hashkey, move, depth, score, alpha, beta);
        }
    } else {
        if (entry2.getDepth() < depth) {
            entry2.set_Renamed(hashkey, move, depth, score, alpha, beta);
        }
    }
}

void TransTableImpl2::clear()
{
    for (int i = 0; i < size_; ++i) {
        table_[static_cast<std::size_t>(i)].set_Renamed(0, 0, 0, 0, 0, 0);
        table2_[static_cast<std::size_t>(i)].set_Renamed(0, 0, 0, 0, 0, 0);
    }
}

} // namespace tgreiner::amy::chess::engine
