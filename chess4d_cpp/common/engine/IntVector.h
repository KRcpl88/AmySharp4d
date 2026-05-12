#pragma once

#include "IMoveList.h"

#include <vector>

namespace tgreiner::amy::common::engine
{
    class IntVector : public IMoveList
    {
    public:
        IntVector();
        ~IntVector() override = default;

        void setSize(int newSize);

        int get(int idx) const;
        int get_Renamed(int idx) const;

        void set(int element, int idx);
        void set_Renamed(int element, int idx);

        void add(int move) override;
        void swap(int idx1, int idx2);
        int size() const noexcept;
        int pop();
        bool contains(int x) const;

    private:
        void ensureCapacity(int minCapacity);

        std::vector<int> storage_;
        int size_;
    };
}
