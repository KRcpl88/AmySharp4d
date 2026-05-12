#include "chess/engine/PawnEvalCache.h"

namespace tgreiner::amy::chess::engine {

const tgreiner::amy::bitboard::BitBoard& PawnEvalCache::getWhitePassedPawns() const
{
    return static_cast<Entry*>(probed)->whitePassedPawns;
}

const tgreiner::amy::bitboard::BitBoard& PawnEvalCache::getBlackPassedPawns() const
{
    return static_cast<Entry*>(probed)->blackPassedPawns;
}

std::unique_ptr<tgreiner::amy::common::engine::AbstractCache::Entry> PawnEvalCache::createEntry()
{
    return std::make_unique<Entry>();
}

void PawnEvalCache::store(std::int64_t key, int value, const tgreiner::amy::bitboard::BitBoard& whitePP, const tgreiner::amy::bitboard::BitBoard& blackPP)
{
    Entry* entry = static_cast<Entry*>(getEntry(key));
    entry->key = key;
    entry->value = value;
    entry->whitePassedPawns = whitePP;
    entry->blackPassedPawns = blackPP;
}

} // namespace tgreiner::amy::chess::engine
