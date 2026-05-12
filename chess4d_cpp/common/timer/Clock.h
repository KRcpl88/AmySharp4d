#pragma once

namespace tgreiner::amy::common::timer
{
    class Clock
    {
    public:
        static constexpr int WHITE = 0;
        static constexpr int BLACK = 1;

        virtual ~Clock() = default;

        virtual bool isRunning() const = 0;
        virtual void start(int side) = 0;
        virtual void stop() = 0;
        virtual int getWallTime(int side) const = 0;
        virtual void reset() = 0;
    };
}
