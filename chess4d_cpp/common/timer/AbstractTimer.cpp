#include "common\timer\AbstractTimer.h"

#include <chrono>

namespace tgreiner::amy::common::timer
{
    AbstractTimer::AbstractTimer()
        : startTime_(std::chrono::steady_clock::now())
    {
    }

    int AbstractTimer::getTime() const
    {
        return static_cast<int>(std::chrono::duration_cast<std::chrono::milliseconds>(
            std::chrono::steady_clock::now() - startTime_).count());
    }

    void AbstractTimer::start()
    {
        startTime_ = std::chrono::steady_clock::now();
    }
}
