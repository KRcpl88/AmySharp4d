#include "bitboard\UniversalCoordinate.h"

#include "bitboard\BitBoard.h"

namespace tgreiner::amy::bitboard {

UCoord::UCoord(int x, int y, int z) {
    setX(x);
    setY(y);
    setZ(z);
}

UCoord::UCoord(const Lfr& lfr) {
    lfr.Validate();

    setZ(lfr.Level);
    const int levelOffset = BitBoard::MAX_LEVEL_WIDTH - BitBoard::LEVEL_WIDTH[lfr.Level];
    const int fileOffset = BitBoard::LEVEL_WIDTH[lfr.Level] - 1;
    setX(levelOffset + lfr.File + lfr.Rank);
    setY(levelOffset + lfr.Rank - lfr.File + fileOffset);
}

int UCoord::getX() const {
    return data[0];
}

void UCoord::setX(int value) {
    data[0] = value;
}

int UCoord::getY() const {
    return data[1];
}

void UCoord::setY(int value) {
    data[1] = value;
}

int UCoord::getZ() const {
    return data[2];
}

void UCoord::setZ(int value) {
    data[2] = value;
}

UCoord::operator Lfr() const {
    Lfr lfr;
    lfr.Level = getZ();

    int levelOffset;
    int fileOffset;
    if ((lfr.Level >= 0) && (lfr.Level < static_cast<int>(BitBoard::LEVEL_WIDTH.size()))) {
        levelOffset = BitBoard::MAX_LEVEL_WIDTH - BitBoard::LEVEL_WIDTH[lfr.Level];
        fileOffset = BitBoard::LEVEL_WIDTH[lfr.Level] - 1;
    } else {
        levelOffset = BitBoard::MAX_LEVEL_WIDTH;
        fileOffset = 0;
    }

    const int doubleFile = getX() - getY() + fileOffset;
    if ((doubleFile & 1) != 0) {
        lfr.File = -1;
    } else {
        lfr.File = doubleFile >> 1;
    }

    const int doubleRank = getX() + getY() - fileOffset;
    if ((doubleRank & 1) != 0) {
        lfr.Rank = -1;
    } else {
        lfr.Rank = (doubleRank >> 1) - levelOffset;
    }

    return lfr;
}

bool UCoord::operator==(const UCoord& other) const {
    for (std::size_t i = 0; i < data.size(); ++i) {
        if (data[i] != other.data[i]) {
            return false;
        }
    }

    return true;
}

bool UCoord::operator!=(const UCoord& other) const {
    return !(*this == other);
}

UCoord UCoord::operator+(const UCoord& other) const {
    UCoord result;
    for (std::size_t i = 0; i < data.size(); ++i) {
        result.data[i] = data[i] + other.data[i];
    }
    return result;
}

UCoord UCoord::operator-(const UCoord& other) const {
    UCoord result;
    for (std::size_t i = 0; i < data.size(); ++i) {
        result.data[i] = data[i] - other.data[i];
    }
    return result;
}

UCoord UCoord::operator-() const {
    UCoord result;
    for (std::size_t i = 0; i < data.size(); ++i) {
        result.data[i] = -data[i];
    }
    return result;
}

} // namespace tgreiner::amy::bitboard
