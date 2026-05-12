/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <string>

namespace tgreiner::amy::chess::engine {

/// Utility class to format search engine output.
class Formatter {
public:
    Formatter() = delete;

    /// Format a time value (milliseconds) as "m:ss.t" or "s.t".
    static std::string timeToString(int time);

    /// Format a centipawn score as "+M<n>", "-M<n>", or "<p>.<cc>".
    static std::string scoreToString(int score);
};

} // namespace tgreiner::amy::chess::engine
