#pragma once

#include <array>

#include "bitboard/BitBoard.h"

namespace tgreiner::amy::chess::engine {

class OutsidePassedPawnIdentifier {
public:
    void probe(tgreiner::amy::bitboard::BitBoard whitePawns, tgreiner::amy::bitboard::BitBoard blackPawns);

    const tgreiner::amy::bitboard::BitBoard& getWhiteOutsidePassedPawns() const;
    const tgreiner::amy::bitboard::BitBoard& getBlackOutsidePassedPawns() const;

private:
    static std::array<tgreiner::amy::bitboard::BitBoard, 8> FILES_LEFT_QUEEN_SIDE;
    static std::array<tgreiner::amy::bitboard::BitBoard, 8> FILES_RIGHT_QUEEN_SIDE;
    static std::array<tgreiner::amy::bitboard::BitBoard, 8> FILES_LEFT_KING_SIDE;
    static std::array<tgreiner::amy::bitboard::BitBoard, 8> FILES_RIGHT_KING_SIDE;

    tgreiner::amy::bitboard::BitBoard whiteOutsidePassedPawns_;
    tgreiner::amy::bitboard::BitBoard blackOutsidePassedPawns_;
};

} // namespace tgreiner::amy::chess::engine
