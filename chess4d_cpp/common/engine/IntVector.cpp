#include "common\engine\IntVector.h"

#include <cstddef>
#include <utility>

namespace tgreiner::amy::common::engine
{
    IntVector::IntVector()
        : storage_(16), size_(0)
    {
    }

    void IntVector::setSize(int newSize)
    {
        ensureCapacity(newSize);
        size_ = newSize;
    }

    int IntVector::get(int idx) const
    {
        return storage_[static_cast<std::size_t>(idx)];
    }

    int IntVector::get_Renamed(int idx) const
    {
        return get(idx);
    }

    void IntVector::set(int element, int idx)
    {
        ensureCapacity(idx + 1);
        storage_[static_cast<std::size_t>(idx)] = element;
    }

    void IntVector::set_Renamed(int element, int idx)
    {
        set(element, idx);
    }

    void IntVector::add(int move)
    {
        ensureCapacity(size_ + 1);
        storage_[static_cast<std::size_t>(size_)] = move;
        ++size_;
    }

    void IntVector::swap(int idx1, int idx2)
    {
        std::swap(storage_[static_cast<std::size_t>(idx1)], storage_[static_cast<std::size_t>(idx2)]);
    }

    int IntVector::size() const noexcept
    {
        return size_;
    }

    int IntVector::pop()
    {
        --size_;
        return storage_[static_cast<std::size_t>(size_)];
    }

    bool IntVector::contains(int x) const
    {
        for (int i = size_ - 1; i >= 0; --i)
        {
            if (x == storage_[static_cast<std::size_t>(i)])
            {
                return true;
            }
        }

        return false;
    }

    void IntVector::ensureCapacity(int minCapacity)
    {
        if (minCapacity <= static_cast<int>(storage_.size()))
        {
            return;
        }

        int newCapacity = static_cast<int>(storage_.size());
        while (newCapacity < minCapacity)
        {
            newCapacity *= 2;
        }

        storage_.resize(static_cast<std::size_t>(newCapacity));
    }
}
