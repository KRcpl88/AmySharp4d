#pragma once

#include <cstdint>
#include <memory>

#include "bitboard/BitBoard.h"
#include "common/engine/AbstractCache.h"

namespace tgreiner::amy::chess::engine {

class PawnEvalCache : public tgreiner::amy::common::engine::AbstractCache {
public:
    class Entry : public tgreiner::amy::common::engine::AbstractCache::Entry {
    public:
        tgreiner::amy::bitboard::BitBoard whitePassedPawns;
        tgreiner::amy::bitboard::BitBoard blackPassedPawns;
    };

    const tgreiner::amy::bitboard::BitBoard& getWhitePassedPawns() const;
    const tgreiner::amy::bitboard::BitBoard& getBlackPassedPawns() const;
    void store(std::int64_t key, int value, const tgreiner::amy::bitboard::BitBoard& whitePP, const tgreiner::amy::bitboard::BitBoard& blackPP);

protected:
    std::unique_ptr<tgreiner::amy::common::engine::AbstractCache::Entry> createEntry() override;
};

} // namespace tgreiner::amy::chess::engine
