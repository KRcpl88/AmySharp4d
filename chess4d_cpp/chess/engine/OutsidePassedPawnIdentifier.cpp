#include "chess/engine/OutsidePassedPawnIdentifier.h"

#include <algorithm>

#include "chess/engine/EvalMasks.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;

namespace {
std::array<BitBoard, 8> createFilesLeftQueenSide()
{
    std::array<BitBoard, 8> result{};
    for (int file = 0; file < 8; ++file) {
        for (int f2 = 0; f2 < file + 1; ++f2) {
            result[file] = result[file] | EvalMasks::FILE_MASK[f2];
        }
    }
    return result;
}

std::array<BitBoard, 8> createFilesRightQueenSide()
{
    std::array<BitBoard, 8> result{};
    for (int file = 0; file < 8; ++file) {
        for (int f3 = file + 2; f3 < 8; ++f3) {
            result[file] = result[file] | EvalMasks::FILE_MASK[f3];
        }
    }
    return result;
}

std::array<BitBoard, 8> createFilesLeftKingSide()
{
    std::array<BitBoard, 8> result{};
    for (int file = 0; file < 8; ++file) {
        for (int f4 = 0; f4 < file - 1; ++f4) {
            result[file] = result[file] | EvalMasks::FILE_MASK[f4];
        }
    }
    return result;
}

std::array<BitBoard, 8> createFilesRightKingSide()
{
    std::array<BitBoard, 8> result{};
    for (int file = 0; file < 8; ++file) {
        for (int f5 = std::max(file - 1, 0); f5 < 8; ++f5) {
            result[file] = result[file] | EvalMasks::FILE_MASK[f5];
        }
    }
    return result;
}
} // namespace

std::array<BitBoard, 8> OutsidePassedPawnIdentifier::FILES_LEFT_QUEEN_SIDE = createFilesLeftQueenSide();
std::array<BitBoard, 8> OutsidePassedPawnIdentifier::FILES_RIGHT_QUEEN_SIDE = createFilesRightQueenSide();
std::array<BitBoard, 8> OutsidePassedPawnIdentifier::FILES_LEFT_KING_SIDE = createFilesLeftKingSide();
std::array<BitBoard, 8> OutsidePassedPawnIdentifier::FILES_RIGHT_KING_SIDE = createFilesRightKingSide();

void OutsidePassedPawnIdentifier::probe(BitBoard whitePawns, BitBoard blackPawns)
{
    whiteOutsidePassedPawns_ = BitBoard();
    blackOutsidePassedPawns_ = BitBoard();

    for (int file = 0; file < 4; ++file) {
        if ((whitePawns & EvalMasks::FILE_MASK[file]).IsEmpty()) {
            continue;
        }

        if ((blackPawns & FILES_LEFT_QUEEN_SIDE[file]).IsEmpty()) {
            break;
        }

        if (!(whitePawns & FILES_RIGHT_QUEEN_SIDE[file]).IsEmpty() && !(blackPawns & FILES_RIGHT_QUEEN_SIDE[file]).IsEmpty()) {
            whiteOutsidePassedPawns_ = whiteOutsidePassedPawns_ | (whitePawns & EvalMasks::FILE_MASK[file]);
        }

        break;
    }

    for (int file = 0; file < 4; ++file) {
        if ((blackPawns & EvalMasks::FILE_MASK[file]).IsEmpty()) {
            continue;
        }

        if (!(whitePawns & FILES_LEFT_QUEEN_SIDE[file]).IsEmpty()) {
            break;
        }

        if (!(whitePawns & FILES_RIGHT_QUEEN_SIDE[file]).IsEmpty() && !(blackPawns & FILES_RIGHT_QUEEN_SIDE[file]).IsEmpty()) {
            blackOutsidePassedPawns_ = blackOutsidePassedPawns_ | (blackPawns & EvalMasks::FILE_MASK[file]);
        }

        break;
    }

    for (int file = 7; file >= 4; --file) {
        if ((whitePawns & EvalMasks::FILE_MASK[file]).IsEmpty()) {
            continue;
        }

        if (!(blackPawns & FILES_RIGHT_KING_SIDE[file]).IsEmpty()) {
            break;
        }

        if (!(whitePawns & FILES_LEFT_KING_SIDE[file]).IsEmpty() && !(blackPawns & FILES_LEFT_KING_SIDE[file]).IsEmpty()) {
            whiteOutsidePassedPawns_ = whiteOutsidePassedPawns_ | (whitePawns & EvalMasks::FILE_MASK[file]);
        }

        break;
    }

    for (int file = 7; file >= 4; --file) {
        if ((blackPawns & EvalMasks::FILE_MASK[file]).IsEmpty()) {
            continue;
        }

        if (!(whitePawns & FILES_RIGHT_KING_SIDE[file]).IsEmpty()) {
            break;
        }

        if (!(whitePawns & FILES_LEFT_KING_SIDE[file]).IsEmpty() && !(blackPawns & FILES_LEFT_KING_SIDE[file]).IsEmpty()) {
            blackOutsidePassedPawns_ = blackOutsidePassedPawns_ | (blackPawns & EvalMasks::FILE_MASK[file]);
        }

        break;
    }
}

const BitBoard& OutsidePassedPawnIdentifier::getWhiteOutsidePassedPawns() const
{
    return whiteOutsidePassedPawns_;
}

const BitBoard& OutsidePassedPawnIdentifier::getBlackOutsidePassedPawns() const
{
    return blackOutsidePassedPawns_;
}

} // namespace tgreiner::amy::chess::engine
