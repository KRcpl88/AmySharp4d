#pragma once

namespace tgreiner::amy::common::engine
{
    class Generator
    {
    public:
        virtual ~Generator() = default;

        virtual int nextMove() = 0;
        virtual void reset() = 0;
        virtual void failHigh(int move, int depth) = 0;
    };
}
