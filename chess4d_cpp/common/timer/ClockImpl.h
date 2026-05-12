#pragma once

#include "common\timer\Clock.h"

#include <array>
#include <chrono>

namespace tgreiner::amy::common::timer
{
    class ClockImpl : public Clock
    {
    public:
        ClockImpl() = default;
        ~ClockImpl() override = default;

        bool isRunning() const override;
        void start(int side) override;
        void stop() override;
        int getWallTime(int side) const override;
        void reset() override;

    private:
        std::array<long long, 2> wallTime_{};
        std::chrono::steady_clock::time_point startTime_{};
        int clockTickingFor_ = WHITE;
        bool isRunning_ = false;
    };
}
