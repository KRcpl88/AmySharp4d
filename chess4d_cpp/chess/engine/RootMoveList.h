#pragma once

#include <vector>

#include "common/engine/IntVector.h"

namespace tgreiner::amy::chess::engine {

class RootMoveList {
public:
    explicit RootMoveList(tgreiner::amy::common::engine::IntVector& list);

    bool hasNext() const;
    int next();
    void failHigh();
    void resort();
    int Best(int n) const;
    void setNodes(int nodes);

private:
    struct Entry {
        int move = 0;
        int nodes = 0;
    };

    int ptr_ = -1;
    Entry* current_ = nullptr;
    std::vector<Entry> entries_;
};

} // namespace tgreiner::amy::chess::engine
