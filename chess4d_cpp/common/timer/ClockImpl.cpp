#include "common\timer\ClockImpl.h"

#include <chrono>

namespace tgreiner::amy::common::timer
{
    bool ClockImpl::isRunning() const
    {
        return isRunning_;
    }

    void ClockImpl::start(int side)
    {
        startTime_ = std::chrono::steady_clock::now();
        clockTickingFor_ = side;
        isRunning_ = true;
    }

    void ClockImpl::stop()
    {
        if (isRunning_)
        {
            wallTime_[static_cast<std::size_t>(clockTickingFor_)] +=
                std::chrono::duration_cast<std::chrono::milliseconds>(
                    std::chrono::steady_clock::now() - startTime_).count();
            isRunning_ = false;
        }
    }

    int ClockImpl::getWallTime(int side) const
    {
        long long time = wallTime_[static_cast<std::size_t>(side)];
        if (isRunning_ && side == clockTickingFor_)
        {
            time += std::chrono::duration_cast<std::chrono::milliseconds>(
                std::chrono::steady_clock::now() - startTime_).count();
        }

        return static_cast<int>(time / 1000);
    }

    void ClockImpl::reset()
    {
        wallTime_[WHITE] = 0;
        wallTime_[BLACK] = 0;
        isRunning_ = false;
        clockTickingFor_ = WHITE;
        startTime_ = std::chrono::steady_clock::time_point{};
    }
}
