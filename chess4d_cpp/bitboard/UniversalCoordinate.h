#pragma once

#include <array>

#include "bitboard\LevelRankFile.h"

namespace tgreiner::amy::bitboard {

class UCoord {
public:
    std::array<int, 3> data{};

    UCoord() = default;
    UCoord(int x, int y, int z);
    explicit UCoord(const Lfr& lfr);

    int getX() const;
    void setX(int value);
    int getY() const;
    void setY(int value);
    int getZ() const;
    void setZ(int value);

    explicit operator Lfr() const;

    bool operator==(const UCoord& other) const;
    bool operator!=(const UCoord& other) const;
    UCoord operator+(const UCoord& other) const;
    UCoord operator-(const UCoord& other) const;
    UCoord operator-() const;
};

} // namespace tgreiner::amy::bitboard
