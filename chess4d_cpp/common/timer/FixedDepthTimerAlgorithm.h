#pragma once

#include "common\timer\TimerAlgorithm.h"

namespace tgreiner::amy::common::timer
{
    class FixedDepthTimerAlgorithm : public TimerAlgorithm
    {
    public:
        explicit FixedDepthTimerAlgorithm(int theDepth);
        ~FixedDepthTimerAlgorithm() override = default;

        void iterationFinished(int currentDepth) override;

    private:
        int depth_ = 0;
    };
}
