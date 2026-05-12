#include "common\engine\EvalCache.h"

#include <memory>

namespace tgreiner::amy::common::engine
{
    std::unique_ptr<AbstractCache::Entry> EvalCache::createEntry()
    {
        return std::make_unique<Entry>();
    }

    void EvalCache::store(std::int64_t key, int value)
    {
        Entry* entry = getEntry(key);
        entry->key = key;
        entry->value = value;
    }
}
