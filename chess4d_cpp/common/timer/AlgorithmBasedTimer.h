#pragma once

#include "common\timer\AbstractTimer.h"
#include "common\timer\TimerAlgorithm.h"

#include <chrono>

namespace tgreiner::amy::common::timer
{
    class AlgorithmBasedTimer : public AbstractTimer
    {
    public:
        explicit AlgorithmBasedTimer(TimerAlgorithm& theAlgorithm);
        ~AlgorithmBasedTimer() override = default;

        void check() override;
        void start() override;
        void iterationFinished(int iteration) override;
        void failLow() override;

    private:
        static constexpr int CHECK_CALLS = 10000;

        std::chrono::steady_clock::time_point lastCheckTime_;
        int callsBetweenChecks_ = CHECK_CALLS;
        int calls_ = 0;
        TimerAlgorithm& algorithm_;
    };
}
