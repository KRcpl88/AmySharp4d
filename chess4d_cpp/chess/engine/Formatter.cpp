/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/Formatter.h"
#include <cmath>
#include <sstream>
#include <string>

namespace tgreiner::amy::chess::engine {

// ---------------------------------------------------------------------------
// Searcher constants used only by Formatter (avoiding a circular dependency
// on Searcher_Fields.h).  These values mirror Searcher_Fields in the C# source.
// ---------------------------------------------------------------------------
namespace {
    constexpr int MATE       = 32768;
    constexpr int MATE_LIMIT = 32668;
}

std::string Formatter::timeToString(int time)
{
    int seconds = time / 1000;
    int tenths  = (time - seconds * 1000) / 100;
    int minutes = seconds / 60;
    seconds -= minutes * 60;

    std::ostringstream buf;
    if (minutes == 0) {
        buf << seconds << '.' << tenths;
    } else {
        buf << minutes << ':';
        if (seconds < 10) buf << '0';
        buf << seconds << '.' << tenths;
    }
    return buf.str();
}

std::string Formatter::scoreToString(int score)
{
    if (score > MATE_LIMIT) {
        int plies = (MATE - score + 1) / 2;
        return "+M" + std::to_string(plies);
    } else if (score < -MATE_LIMIT) {
        int plies = (MATE + score + 1) / 2;
        return "-M" + std::to_string(plies);
    }

    bool neg      = score < 0;
    int  absScore = std::abs(score);
    int  pawns    = absScore / 100;
    int  centi    = absScore - 100 * pawns;

    std::ostringstream buf;
    if (neg) buf << '-';
    buf << pawns << '.';
    if (centi < 10) buf << '0';
    buf << centi;
    return buf.str();
}

} // namespace tgreiner::amy::chess::engine
