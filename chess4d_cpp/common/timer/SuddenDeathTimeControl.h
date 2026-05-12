#pragma once

#include "common\timer\TimeControl.h"

namespace tgreiner::amy::common::timer
{
    class SuddenDeathTimeControl : public TimeControl
    {
    public:
        explicit SuddenDeathTimeControl(int theTime);
        ~SuddenDeathTimeControl() override = default;

        int getSoftLimit() const override;
        int getHardLimit() const override;
        void setRemainingTime(int remaining) override;

    private:
        static constexpr int MICROS_PER_SECOND = 1000;
        static constexpr int MOVES = 60;

        int time_ = 0;
        int remainingTime_ = 0;
    };
}
