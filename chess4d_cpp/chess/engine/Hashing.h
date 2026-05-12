/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <cstdint>
#include <vector>
#include "bitboard/BitBoard.h"
#include "chess/engine/ChessConstants.h"

namespace tgreiner::amy::chess::engine {

/// Zobrist hash keys for ChessBoard position hashing.
/// All tables are initialised once at program start using a fixed-seed PRNG
/// that exactly reproduces the Java/C# sequence (seed 4711, nextLong algorithm).
class Hashing final {
public:
    /// HASH_KEYS[side][piece][square]  (side: 0=white, 1=black)
    static std::vector<std::vector<std::vector<int64_t>>> HASH_KEYS;

    /// EN_PASSANT_HASH_KEYS[square]
    static std::vector<int64_t> EN_PASSANT_HASH_KEYS;

    /// CASTLE_HASH_KEYS[castleFlags]  (16 entries)
    static std::vector<int64_t> CASTLE_HASH_KEYS;

    /// Hash key XORed whenever the side-to-move changes.
    static constexpr int64_t WTM_HASH =
        static_cast<int64_t>(0xaa55aa55aa55aa55ULL);  // = -6172840429334713771

    Hashing() = delete;

private:
    /// Reproduces Java's java.util.Random nextLong() with seed 4711.
    /// The Java Linear Congruential Generator state advances by
    ///   state = (state * 0x5DEECE66DLL + 0xBLL) & ((1LL<<48)-1)
    /// and nextLong() returns two successive 32-bit halves combined.
    class JavaRandom {
    public:
        explicit JavaRandom(int64_t seed);
        int64_t nextLong();
    private:
        int64_t seed_;
        int64_t next(int bits);
    };

    struct StaticInit {
        StaticInit();
    };
    static StaticInit staticInit_;
};

} // namespace tgreiner::amy::chess::engine
