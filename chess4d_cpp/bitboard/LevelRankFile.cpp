#include "bitboard\LevelRankFile.h"

#include <stdexcept>

#include "bitboard\BitBoard.h"

namespace tgreiner::amy::bitboard {

Lfr::Lfr(int level, int file, int rank)
    : Level(level), Rank(rank), File(file) {
    Validate();
}

Lfr::Lfr(int offset) {
    ValidateOffset(offset);

    Level = BitBoard::NUM_LEVELS - 1;
    while (offset < BitBoard::LEVEL_OFFSET[Level]) {
        --Level;
    }

    Rank = (offset - BitBoard::LEVEL_OFFSET[Level]) / BitBoard::LEVEL_WIDTH[Level];
    File = (offset - BitBoard::LEVEL_OFFSET[Level]) % BitBoard::LEVEL_WIDTH[Level];
}

void Lfr::Validate() const {
    if (!IsValid()) {
        throw std::out_of_range("Lfr::Validate()");
    }
}

void Lfr::ValidateOffset(int offset) {
    if (!IsValid(offset)) {
        throw std::out_of_range("Lfr::ValidateOffset(offset) offset");
    }
}

bool Lfr::IsValid() const {
    return IsValid(Level, File, Rank);
}

bool Lfr::IsValid(int level, int file, int rank) {
    if ((level < 0) || (level >= BitBoard::NUM_LEVELS)) {
        return false;
    }

    if ((rank < 0) || (rank >= BitBoard::LEVEL_WIDTH[level]) || (file < 0) || (file >= BitBoard::LEVEL_WIDTH[level])) {
        return false;
    }

    return true;
}

bool Lfr::IsValid(int offset) {
    return (offset < BitBoard::SIZE) && (offset >= 0);
}

Lfr::operator int() const {
    Validate();
    return BitBoard::BitOffset(Level, File, Rank);
}

} // namespace tgreiner::amy::bitboard
