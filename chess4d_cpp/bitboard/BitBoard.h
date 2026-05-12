#pragma once

#include <array>
#include <bit>
#include <cstdint>
#include <limits>
#include <string>
#include <vector>

#include "bitboard\BoardConstants.h"

namespace tgreiner::amy::bitboard {

class BitBoard final {
public:
    inline static constexpr std::array<int, 15> LEVEL_SIZE{1, 4, 9, 16, 25, 36, 49, 64, 49, 36, 25, 16, 9, 4, 1};
    inline static constexpr std::array<int, 15> LEVEL_WIDTH{1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1};
    inline static constexpr std::array<int, 15> LEVEL_OFFSET{
        BoardConstants::LA,
        BoardConstants::LB,
        BoardConstants::LC,
        BoardConstants::LD,
        BoardConstants::LE,
        BoardConstants::LF,
        BoardConstants::LG,
        BoardConstants::LH,
        BoardConstants::LI,
        BoardConstants::LJ,
        BoardConstants::LK,
        BoardConstants::LL,
        BoardConstants::LM,
        BoardConstants::LN,
        BoardConstants::LO};

    static constexpr int NUM_LEVELS = 15;
    static constexpr int MAX_LEVEL_WIDTH = 8;
    static constexpr int SIZE = 344;
    static constexpr int ULONG_SIZE_BITS = static_cast<int>(sizeof(std::uint64_t) * 8);
    static constexpr int SIZE_LONG = (SIZE / ULONG_SIZE_BITS) + 1;

    BitBoard();
    BitBoard(const BitBoard& other) = default;
    explicit BitBoard(const std::vector<int>& offsets);

    static std::vector<BitBoard> CreateArray(int size);

    bool operator==(const BitBoard& other) const;
    bool operator!=(const BitBoard& other) const;
    BitBoard operator&(const BitBoard& other) const;
    BitBoard operator|(const BitBoard& other) const;
    BitBoard operator~() const;
    operator std::string() const;

    bool IsEmpty() const;
    int findFirstOne() const;
    int GetBit(int offset) const;
    int GetBit(int level, int file, int rank) const;
    void SetBit(int offset);
    void SetBit(int level, int file, int rank);
    void SetBits(const std::vector<int>& offsets);
    void ClearBit(int offset);
    void ClearBit(int level, int file, int rank);
    static int BitOffset(int level, int file, int rank);
    int countBits() const;
    std::string ToString() const;
    void Clear();

private:
    inline static constexpr int VALID_BITS_IN_LAST_WORD = SIZE % ULONG_SIZE_BITS;
    inline static constexpr std::uint64_t INVALID_SQUARE_MASK =
        VALID_BITS_IN_LAST_WORD == 0 ? std::numeric_limits<std::uint64_t>::max() : ((std::uint64_t{1} << VALID_BITS_IN_LAST_WORD) - 1U);

    std::array<std::uint64_t, SIZE_LONG> data_{};

    void ClearInvalidBits();
    static void ValidateOffset(int offset);
    static constexpr int UlongOffset2BitOffset(int countUlongOffset) {
        return countUlongOffset * ULONG_SIZE_BITS;
    }
};

} // namespace tgreiner::amy::bitboard
