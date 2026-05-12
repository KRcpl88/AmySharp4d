#pragma once

namespace tgreiner::amy::common::timer
{
    class TimerAlgorithm
    {
    public:
        virtual ~TimerAlgorithm() = 0;

        virtual void check(int time)
        {
        }

        virtual void iterationFinished(int depth)
        {
        }

        virtual void failLow()
        {
        }

    protected:
        TimerAlgorithm() = default;
    };

    inline TimerAlgorithm::~TimerAlgorithm() = default;
}
