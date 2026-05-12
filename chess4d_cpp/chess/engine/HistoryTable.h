/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <array>

#include "common/engine/IntVector.h"

namespace tgreiner::amy::chess::engine {

class HistoryTable {
public:
    static constexpr int MASK = 4095;

    HistoryTable();

    void addHistory(int move, int depth);
    void reset();
    int select(tgreiner::amy::common::engine::IntVector& moves, int size);

private:
    std::array<int, 4096> table_{};
};

} // namespace tgreiner::amy::chess::engine
