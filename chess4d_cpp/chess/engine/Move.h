/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <string>
#include "chess/engine/ChessConstants.h"

// Forward declaration to avoid circular includes
namespace tgreiner::amy::chess::engine { class ChessBoard; }
namespace tgreiner::amy::common::engine { class IMoveList; }

namespace tgreiner::amy::chess::engine {

/// Utility class for encoding/decoding move integers.
///
/// Move integer layout:
///   bits  0- 9  : from square  (9 bits)
///   bits 10-19  : to square    (9 bits, shifted by SHIFT_TO=10)
///   bit  21     : CAPTURE flag
///   bit  22     : CASTLE_KSIDE flag
///   bit  23     : CASTLE_QSIDE flag
///   bit  24     : PROMO_KNIGHT
///   bit  25     : PROMO_BISHOP
///   bit  26     : PROMO_ROOK
///   bit  27     : PROMO_QUEEN
///   bit  28     : PAWN_DOUBLE
///   bit  29     : ENPASSANT
class Move final {
public:
    static constexpr int CAPTURE      = (1 << 21);
    static constexpr int CASTLE_KSIDE = (1 << 22);
    static constexpr int CASTLE_QSIDE = (1 << 23);
    static constexpr int CASTLE       = CASTLE_KSIDE | CASTLE_QSIDE;

    static constexpr int PROMO_KNIGHT = (1 << 24);
    static constexpr int PROMO_BISHOP = (1 << 25);
    static constexpr int PROMO_ROOK   = (1 << 26);
    static constexpr int PROMO_QUEEN  = (1 << 27);
    static constexpr int PROMOTION    = PROMO_KNIGHT | PROMO_BISHOP | PROMO_ROOK | PROMO_QUEEN;

    static constexpr int PAWN_DOUBLE  = (1 << 28);
    static constexpr int ENPASSANT    = (1 << 29);

    static const std::string KSIDE_CASTLE_SAN;  // "O-O"
    static const std::string QSIDE_CASTLE_SAN;  // "O-O-O"

    // ------------------------------------------------------------------
    // Accessors
    // ------------------------------------------------------------------
    static int  getFrom(int move);
    static int  getTo  (int move);
    static int  makeMove(int from, int to);

    static bool isPromotion    (int move);
    static bool isCastle       (int move);
    static bool isKingSideCastle(int move);

    static int  getPromoPiece  (int move);

    static char file(int square);
    static char rank(int square);

    // ------------------------------------------------------------------
    // String conversion
    // ------------------------------------------------------------------
    static std::string toString(int move);
    static std::string toSAN   (ChessBoard& board, int move);
    static std::string toSAN1  (ChessBoard& board, int move);

    // ------------------------------------------------------------------
    // Parsing
    // ------------------------------------------------------------------
    /// Parse a move from SAN or coordinate notation.
    static int parseSAN(ChessBoard& board, const std::string& san);

    /// Parse a simple 4/5-character coordinate move string ("e2e4", "e7e8q").
    static int getMove(ChessBoard& board, const std::string& move);

    Move() = delete;

private:
    static constexpr int MOVE_SQUARE_MASK = 511;  // 9 bits
    static constexpr int SHIFT_TO         = 10;
};

} // namespace tgreiner::amy::chess::engine
