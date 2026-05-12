/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <cstdint>

namespace tgreiner::amy::chess::engine {

class TTEntry;

class ITransTable {
public:
    virtual ~ITransTable() = default;

    virtual TTEntry* get_Renamed(std::int64_t hashkey) = 0;
    virtual void store(std::int64_t hashkey, int move, int depth, int score, int alpha, int beta) = 0;
    virtual void clear() = 0;
};

} // namespace tgreiner::amy::chess::engine
