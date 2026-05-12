#pragma once

#include "common\timer\TimeControl.h"

namespace tgreiner::amy::common::timer
{
    class QuotaTimeControl : public TimeControl
    {
    public:
        QuotaTimeControl(int theMoves, int theTime);
        ~QuotaTimeControl() override = default;

        int getSoftLimit() const override;
        int getHardLimit() const override;
        void setRemainingTime(int remaining) override;

    private:
        static constexpr int MICROS_PER_SECOND = 1000;

        int moves_ = 0;
        int time_ = 0;
        int remainingTime_ = 0;
    };
}
