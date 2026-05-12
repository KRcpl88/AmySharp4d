#pragma once

#include <array>

#include "bitboard\BitBoard.h"

namespace tgreiner::amy::chess::engine {

enum class Player { none, black, white };

struct ChessConstants {
    static constexpr int PAWN = 1;
    static constexpr int KNIGHT = 2;
    static constexpr int BISHOP = 3;
    static constexpr int ROOK = 4;
    static constexpr int QUEEN = 5;
    static constexpr int KING = 6;
    static constexpr int LAST_PIECE = 7;

    static constexpr int WHITE_PAWN = PAWN;
    static constexpr int WHITE_KNIGHT = KNIGHT;
    static constexpr int WHITE_BISHOP = BISHOP;
    static constexpr int WHITE_ROOK = ROOK;
    static constexpr int WHITE_QUEEN = QUEEN;
    static constexpr int WHITE_KING = KING;

    static constexpr int BLACK_PAWN = -PAWN;
    static constexpr int BLACK_KNIGHT = -KNIGHT;
    static constexpr int BLACK_BISHOP = -BISHOP;
    static constexpr int BLACK_ROOK = -ROOK;
    static constexpr int BLACK_QUEEN = -QUEEN;
    static constexpr int BLACK_KING = -KING;

    inline static constexpr std::array<int, tgreiner::amy::bitboard::BitBoard::SIZE> INITIAL_BOARD{
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::WHITE_ROOK, ChessConstants::WHITE_KNIGHT,
        ChessConstants::WHITE_BISHOP, ChessConstants::WHITE_QUEEN, ChessConstants::WHITE_KING,
        ChessConstants::WHITE_BISHOP, ChessConstants::WHITE_KNIGHT, ChessConstants::WHITE_ROOK,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_ROOK, ChessConstants::BLACK_KNIGHT,
        ChessConstants::BLACK_BISHOP, ChessConstants::BLACK_QUEEN, ChessConstants::BLACK_KING,
        ChessConstants::BLACK_BISHOP, ChessConstants::BLACK_KNIGHT, ChessConstants::BLACK_ROOK,
        ChessConstants::WHITE_ROOK, ChessConstants::WHITE_KNIGHT, ChessConstants::WHITE_BISHOP,
        ChessConstants::WHITE_QUEEN, ChessConstants::WHITE_KNIGHT, ChessConstants::WHITE_BISHOP,
        ChessConstants::WHITE_ROOK, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_ROOK, ChessConstants::BLACK_KNIGHT,
        ChessConstants::BLACK_BISHOP, ChessConstants::BLACK_QUEEN, ChessConstants::BLACK_KNIGHT,
        ChessConstants::BLACK_BISHOP, ChessConstants::BLACK_ROOK, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN,
        ChessConstants::WHITE_PAWN, ChessConstants::WHITE_PAWN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN, ChessConstants::BLACK_PAWN,
        ChessConstants::BLACK_PAWN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };
};

using ChessConstants_Fields = ChessConstants;

} // namespace tgreiner::amy::chess::engine
