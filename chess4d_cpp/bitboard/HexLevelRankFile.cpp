#include "bitboard\HexLevelRankFile.h"

#include <stdexcept>

#include "bitboard\BitBoard.h"

namespace tgreiner::amy::bitboard {

HexLfr::HexLfr(int level, int file, int rank)
    : Level(level), Rank(rank), File(file) {
}

HexLfr::HexLfr(const Lfr& lfr) {
    lfr.Validate();

    Level = lfr.Rank + Relu16[lfr.Level];
    Rank = lfr.Rank + NegRelu16[lfr.Level];
    File = lfr.File + Relu16[lfr.Level];
}

void HexLfr::Validate() const {
    if (!IsValid()) {
        throw std::out_of_range("HexLfr::Validate()");
    }
}

bool HexLfr::IsValid() const {
    return IsValid(Level, File, Rank);
}

bool HexLfr::IsValid(int level, int file, int rank) {
    if ((level < 0) || (level >= BitBoard::MAX_LEVEL_WIDTH)) {
        return false;
    }

    if ((rank < 0) || (rank >= BitBoard::MAX_LEVEL_WIDTH)) {
        return false;
    }

    if (rank < level) {
        if ((file < (BitBoard::MAX_LEVEL_WIDTH - RankWidth(level, rank))) || (file >= BitBoard::MAX_LEVEL_WIDTH)) {
            return false;
        }
    } else {
        if ((file < 0) || (file >= (BitBoard::MAX_LEVEL_WIDTH - RankWidth(level, rank)))) {
            return false;
        }
    }

    return true;
}

int HexLfr::RankWidth(int level, int rank) {
    return BitBoard::MAX_LEVEL_WIDTH - Relu16[7 + rank - level] - NegRelu16[7 + rank - level];
}

bool HexLfr::IsValid(int offset) {
    return (offset < BitBoard::SIZE) && (offset >= 0);
}

HexLfr::operator int() const {
    Validate();
    return BitBoard::BitOffset(Level, File, Rank);
}

HexLfr::operator Lfr() const {
    Lfr ret;
    ret.Level = 7 + Level - Rank;
    ret.File = File - NegRelu16[Rank + 7 - Level];
    ret.Rank = Level - NegRelu16[Rank + 7 - Level];
    return ret;
}

} // namespace tgreiner::amy::bitboard
