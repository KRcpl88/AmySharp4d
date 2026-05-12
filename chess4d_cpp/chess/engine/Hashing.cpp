/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/Hashing.h"

namespace tgreiner::amy::chess::engine {

// ---------------------------------------------------------------------------
// Static member storage
// ---------------------------------------------------------------------------
std::vector<std::vector<std::vector<int64_t>>> Hashing::HASH_KEYS;
std::vector<int64_t>                           Hashing::EN_PASSANT_HASH_KEYS;
std::vector<int64_t>                           Hashing::CASTLE_HASH_KEYS;

// ---------------------------------------------------------------------------
// JavaRandom – faithful reproduction of java.util.Random (seed constructor
// and nextLong) so that hash keys are identical to the C# port.
// ---------------------------------------------------------------------------
Hashing::JavaRandom::JavaRandom(int64_t seed)
    // Java initialises with (seed ^ 0x5DEECE66DL) & ((1<<48)-1)
    : seed_((seed ^ 0x5DEECE66DLL) & ((1LL << 48) - 1))
{}

int64_t Hashing::JavaRandom::next(int bits)
{
    seed_ = (seed_ * 0x5DEECE66DLL + 0xBLL) & ((1LL << 48) - 1);
    // Java arithmetic shift right
    return static_cast<int64_t>(
        static_cast<uint64_t>(seed_) >> (48 - bits));
}

int64_t Hashing::JavaRandom::nextLong()
{
    // Java nextLong: (((long)(next(32)) << 32) + next(32))
    int64_t hi = next(32);
    int64_t lo = next(32);
    return (hi << 32) + lo;
}

// ---------------------------------------------------------------------------
// Static initialiser
// ---------------------------------------------------------------------------
Hashing::StaticInit::StaticInit()
{
    using BB = tgreiner::amy::bitboard::BitBoard;

    JavaRandom rng(4711LL);

    const int numPieces = ChessConstants::KING + 1; // indices 0..6
    const int sz        = BB::SIZE;

    // Allocate HASH_KEYS[2][numPieces][sz]
    HASH_KEYS.assign(2,
        std::vector<std::vector<int64_t>>(numPieces,
            std::vector<int64_t>(sz, 0)));

    EN_PASSANT_HASH_KEYS.assign(sz, 0);

    for (int pc = 0; pc <= ChessConstants::KING; ++pc) {
        for (int square = 0; square < sz; ++square) {
            HASH_KEYS[0][pc][square] = rng.nextLong();
            HASH_KEYS[1][pc][square] = rng.nextLong();
            EN_PASSANT_HASH_KEYS[square] = rng.nextLong();
        }
    }
    EN_PASSANT_HASH_KEYS[0] = 0;

    CASTLE_HASH_KEYS.assign(16, 0);
    for (int castle = 0; castle < 16; ++castle) {
        CASTLE_HASH_KEYS[castle] = rng.nextLong();
    }
}

Hashing::StaticInit Hashing::staticInit_;

} // namespace tgreiner::amy::chess::engine
