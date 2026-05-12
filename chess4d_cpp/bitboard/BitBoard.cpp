#include "bitboard\BitBoard.h"

#include <sstream>
#include <stdexcept>

namespace tgreiner::amy::bitboard {

BitBoard::BitBoard() = default;

BitBoard::BitBoard(const std::vector<int>& offsets) {
    SetBits(offsets);
}

std::vector<BitBoard> BitBoard::CreateArray(int size) {
    if (size < 0) {
        throw std::out_of_range("BitBoard::CreateArray(size) size");
    }

    return std::vector<BitBoard>(static_cast<std::size_t>(size));
}

bool BitBoard::operator==(const BitBoard& other) const {
    return data_ == other.data_;
}

bool BitBoard::operator!=(const BitBoard& other) const {
    return !(*this == other);
}

BitBoard BitBoard::operator&(const BitBoard& other) const {
    BitBoard result;
    for (std::size_t i = 0; i < data_.size(); ++i) {
        result.data_[i] = data_[i] & other.data_[i];
    }
    return result;
}

BitBoard BitBoard::operator|(const BitBoard& other) const {
    BitBoard result;
    for (std::size_t i = 0; i < data_.size(); ++i) {
        result.data_[i] = data_[i] | other.data_[i];
    }
    return result;
}

BitBoard BitBoard::operator~() const {
    BitBoard result;
    for (std::size_t i = 0; i < data_.size(); ++i) {
        result.data_[i] = ~data_[i];
    }
    result.ClearInvalidBits();
    return result;
}

BitBoard::operator std::string() const {
    return ToString();
}

void BitBoard::ClearInvalidBits() {
    data_.back() &= INVALID_SQUARE_MASK;
}

bool BitBoard::IsEmpty() const {
    for (const auto value : data_) {
        if (value != 0U) {
            return false;
        }
    }
    return true;
}

int BitBoard::findFirstOne() const {
    for (std::size_t i = 0; i < data_.size(); ++i) {
        if (data_[i] != 0U) {
            return static_cast<int>(i * ULONG_SIZE_BITS + std::countr_zero(data_[i]));
        }
    }
    return -1;
}

int BitBoard::GetBit(int offset) const {
    ValidateOffset(offset);
    const int countUlongOffset = offset / ULONG_SIZE_BITS;
    return ((data_[countUlongOffset] >> (offset - UlongOffset2BitOffset(countUlongOffset))) & 1U) == 0U ? 0 : 1;
}

void BitBoard::ValidateOffset(int offset) {
    if ((offset >= SIZE) || (offset < 0)) {
        throw std::out_of_range("BitBoard::GetBit(offset) offset");
    }
}

int BitBoard::GetBit(int level, int file, int rank) const {
    return GetBit(BitOffset(level, file, rank));
}

void BitBoard::SetBit(int offset) {
    ValidateOffset(offset);
    const int countUlongOffset = offset / ULONG_SIZE_BITS;
    data_[countUlongOffset] |= std::uint64_t{1} << (offset - UlongOffset2BitOffset(countUlongOffset));
}

void BitBoard::SetBit(int level, int file, int rank) {
    SetBit(BitOffset(level, file, rank));
}

void BitBoard::SetBits(const std::vector<int>& offsets) {
    for (const int offset : offsets) {
        SetBit(offset);
    }
}

void BitBoard::ClearBit(int offset) {
    ValidateOffset(offset);
    const int countUlongOffset = offset / ULONG_SIZE_BITS;
    data_[countUlongOffset] &= ~(std::uint64_t{1} << (offset - UlongOffset2BitOffset(countUlongOffset)));
}

void BitBoard::ClearBit(int level, int file, int rank) {
    ClearBit(BitOffset(level, file, rank));
}

int BitBoard::BitOffset(int level, int file, int rank) {
    if ((level < 0) || (level >= NUM_LEVELS)) {
        throw std::out_of_range("BitBoard::BitOffset(level, file, rank) level");
    }

    if ((file < 0) || (file >= LEVEL_WIDTH[level]) || (rank < 0) || (rank >= LEVEL_WIDTH[level])) {
        throw std::out_of_range("BitBoard::BitOffset(level, file, rank) coordinate");
    }

    return LEVEL_OFFSET[level] + rank * LEVEL_WIDTH[level] + file;
}

int BitBoard::countBits() const {
    int count = 0;
    for (const auto value : data_) {
        count += static_cast<int>(std::popcount(value));
    }
    return count;
}

std::string BitBoard::ToString() const {
    std::ostringstream buf;

    for (int level = static_cast<int>(LEVEL_WIDTH.size()) - 1; level >= 0; --level) {
        for (int rank = LEVEL_WIDTH[level] - 1; rank >= 0; --rank) {
            for (int file = 0; file < LEVEL_WIDTH[level]; ++file) {
                buf << (GetBit(level, file, rank) != 0 ? 'x' : '.');
            }
            buf << '\n';
        }
    }

    return buf.str();
}

void BitBoard::Clear() {
    data_.fill(0U);
}

} // namespace tgreiner::amy::bitboard
