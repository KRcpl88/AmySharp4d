#pragma once

#include "common\timer\TimerAlgorithm.h"

namespace tgreiner::amy::common::timer
{
    class LowerBoundTimerAlgorithm : public TimerAlgorithm
    {
    public:
        explicit LowerBoundTimerAlgorithm(int theTime);
        ~LowerBoundTimerAlgorithm() override = default;

        void setMaxDepth(int depth);
        void setDuration(int theTime);
        void check(int currentTime) override;
        void iterationFinished(int currentDepth) override;

    private:
        int time_ = 0;
        int hard_ = 0;
        bool terminate_ = false;
        int maxDepth_ = 0;
    };
}
