#pragma once

#include <cstdint>
#include <memory>
#include <vector>

namespace tgreiner::amy::common::engine
{
    class AbstractCache
    {
    public:
        class Entry
        {
        public:
            virtual ~Entry() = default;

            std::int64_t key = 0;
            int value = 0;
        };

        AbstractCache();
        virtual ~AbstractCache() = default;

        int getValue() const;
        bool probe(std::int64_t key);

    protected:
        virtual std::unique_ptr<Entry> createEntry() = 0;
        Entry* getEntry(std::int64_t key);

        Entry* probed = nullptr;

    private:
        std::vector<std::unique_ptr<Entry>> entries_;
        int mask_;
    };
}
