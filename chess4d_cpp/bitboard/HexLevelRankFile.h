#pragma once

#include <array>

#include "bitboard\LevelRankFile.h"

namespace tgreiner::amy::bitboard {

class HexLfr {
public:
    inline static constexpr std::array<int, 15> Relu16{0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7};
    inline static constexpr std::array<int, 15> NegRelu16{7, 6, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0};

    int Level{0};
    int Rank{0};
    int File{0};

    HexLfr() = default;
    HexLfr(int level, int file, int rank);
    explicit HexLfr(const Lfr& lfr);

    bool IsValid() const;

    static bool IsValid(int level, int file, int rank);
    static int RankWidth(int level, int rank);
    static bool IsValid(int offset);

    explicit operator int() const;
    explicit operator Lfr() const;

private:
    void Validate() const;
};

} // namespace tgreiner::amy::bitboard
