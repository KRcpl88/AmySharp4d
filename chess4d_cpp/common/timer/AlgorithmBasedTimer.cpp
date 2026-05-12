#include "common\timer\AlgorithmBasedTimer.h"

#include <chrono>

namespace tgreiner::amy::common::timer
{
    AlgorithmBasedTimer::AlgorithmBasedTimer(TimerAlgorithm& theAlgorithm)
        : lastCheckTime_(std::chrono::steady_clock::now()), algorithm_(theAlgorithm)
    {
    }

    void AlgorithmBasedTimer::check()
    {
        if (--calls_ <= 0)
        {
            const auto now = std::chrono::steady_clock::now();
            const auto elapsedSinceLastCheck = std::chrono::duration_cast<std::chrono::milliseconds>(
                now - lastCheckTime_).count();
            if (elapsedSinceLastCheck > 0)
            {
                algorithm_.check(static_cast<int>(std::chrono::duration_cast<std::chrono::milliseconds>(
                    now - startTime_).count()));

                const double factor = 100.0 / static_cast<double>(elapsedSinceLastCheck);
                callsBetweenChecks_ = static_cast<int>(callsBetweenChecks_ * factor);
                calls_ = callsBetweenChecks_;
                lastCheckTime_ = now;
            }
        }
    }

    void AlgorithmBasedTimer::start()
    {
        AbstractTimer::start();
        lastCheckTime_ = startTime_;
        calls_ = callsBetweenChecks_;
    }

    void AlgorithmBasedTimer::iterationFinished(int iteration)
    {
        algorithm_.iterationFinished(iteration);
    }

    void AlgorithmBasedTimer::failLow()
    {
        algorithm_.failLow();
    }
}
