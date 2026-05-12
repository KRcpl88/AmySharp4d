#pragma once

#include "AbstractCache.h"

namespace tgreiner::amy::common::engine
{
    class EvalCache : public AbstractCache
    {
    public:
        EvalCache() = default;
        ~EvalCache() override = default;

        void store(std::int64_t key, int value);

    protected:
        std::unique_ptr<Entry> createEntry() override;
    };
}
