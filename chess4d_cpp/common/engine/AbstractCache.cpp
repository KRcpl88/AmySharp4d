#include "common\engine\AbstractCache.h"

#include <cstddef>
#include <cstdint>

namespace tgreiner::amy::common::engine
{
    namespace
    {
        constexpr int kCacheSize = 1 << 15;
    }

    AbstractCache::AbstractCache()
        : entries_(static_cast<std::size_t>(kCacheSize)), mask_(kCacheSize - 1)
    {
    }

    int AbstractCache::getValue() const
    {
        return probed->value;
    }

    bool AbstractCache::probe(std::int64_t key)
    {
        const auto index = static_cast<std::size_t>(static_cast<std::uint64_t>(key) & static_cast<std::uint64_t>(mask_));
        probed = entries_[index].get();

        return probed != nullptr && probed->key == key;
    }

    AbstractCache::Entry* AbstractCache::getEntry(std::int64_t key)
    {
        const auto index = static_cast<std::size_t>(static_cast<std::uint64_t>(key) & static_cast<std::uint64_t>(mask_));

        if (!entries_[index])
        {
            entries_[index] = createEntry();
        }

        return entries_[index].get();
    }
}
