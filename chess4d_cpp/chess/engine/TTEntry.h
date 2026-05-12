/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <cstdint>

namespace tgreiner::amy::chess::engine {

class TTEntry {
public:
    static constexpr int DEPTH_MASK = 0x3fff;
    static constexpr int FLAG_MASK = 0xc000;
    static constexpr int EXACT = 0xc000;
    static constexpr int LOWER = 0x8000;
    static constexpr int UPPER = 0x4000;

    std::int64_t hashkey{0};
    int move{0};
    short score{0};

    int getDepth() const;
    bool getExact() const;
    bool getLower() const;
    bool getUpper() const;

    void set_Renamed(std::int64_t hashkey, int move, int depth, int score, int alpha, int beta);

private:
    short depth_{0};
};

} // namespace tgreiner::amy::chess::engine
