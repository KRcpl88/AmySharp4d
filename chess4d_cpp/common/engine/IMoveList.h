#pragma once

namespace tgreiner::amy::common::engine
{
    class IMoveList
    {
    public:
        virtual ~IMoveList() = default;

        virtual void add(int move) = 0;
    };
}
