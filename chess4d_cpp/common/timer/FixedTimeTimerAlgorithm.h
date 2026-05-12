#pragma once

#include "common\timer\TimerAlgorithm.h"

namespace tgreiner::amy::common::timer
{
    class FixedTimeTimerAlgorithm : public TimerAlgorithm
    {
    public:
        explicit FixedTimeTimerAlgorithm(int theTime);
        ~FixedTimeTimerAlgorithm() override = default;

        void setDuration(int theTime);
        void check(int currentTime) override;

    private:
        int time_ = 0;
    };
}
