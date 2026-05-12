#pragma once

#include "common\timer\TimerAlgorithm.h"

namespace tgreiner::amy::common::timer
{
    class ExtendOnFailLowTimerAlgorithm : public TimerAlgorithm
    {
    public:
        ExtendOnFailLowTimerAlgorithm(int theTime, int theExtended);
        ~ExtendOnFailLowTimerAlgorithm() override = default;

        void check(int currentTime) override;
        void setDuration(int theTime, int theExtended);
        void failLow() override;
        void iterationFinished(int iteration) override;

    private:
        int threshold_ = 0;
        int time_ = 0;
        int extended_ = 0;
    };
}
