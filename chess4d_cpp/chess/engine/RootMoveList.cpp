#include "chess/engine/RootMoveList.h"

#include <algorithm>

namespace tgreiner::amy::chess::engine {

RootMoveList::RootMoveList(tgreiner::amy::common::engine::IntVector& list)
{
    entries_.resize(static_cast<std::size_t>(list.size()));
    for (int i = 0; i < list.size(); ++i) {
        entries_[static_cast<std::size_t>(i)].move = list.get_Renamed(i);
    }
}

bool RootMoveList::hasNext() const
{
    return (ptr_ + 1) < static_cast<int>(entries_.size());
}

int RootMoveList::next()
{
    ++ptr_;
    current_ = &entries_[static_cast<std::size_t>(ptr_)];
    return current_->move;
}

void RootMoveList::failHigh()
{
    std::swap(entries_.front(), entries_[static_cast<std::size_t>(ptr_)]);
}

void RootMoveList::resort()
{
    ptr_ = -1;
    current_ = nullptr;

    for (std::size_t i = 1; i < entries_.size(); ++i) {
        std::size_t bestidx = i;
        int nodes = entries_[i].nodes;
        for (std::size_t j = i + 1; j < entries_.size(); ++j) {
            if (entries_[j].nodes > nodes) {
                nodes = entries_[j].nodes;
                bestidx = j;
            }
        }
        if (bestidx != i) {
            std::swap(entries_[i], entries_[bestidx]);
        }
    }
}

int RootMoveList::Best(int n) const
{
    while (static_cast<int>(entries_.size()) <= n) {
        --n;
    }
    return entries_[static_cast<std::size_t>(n)].move;
}

void RootMoveList::setNodes(int nodes)
{
    current_->nodes = nodes;
}

} // namespace tgreiner::amy::chess::engine
