#pragma once

namespace tgreiner::amy::common::timer
{
    class IChessTimer
    {
    public:
        virtual ~IChessTimer() = default;

        virtual int getTime() const = 0;
        virtual void check() = 0;
        virtual void start() = 0;
        virtual void iterationFinished(int iteration) = 0;
        virtual void failLow() = 0;
    };
}
