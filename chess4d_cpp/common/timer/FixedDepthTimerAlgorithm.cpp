#include "common\timer\FixedDepthTimerAlgorithm.h"
#include "common\timer\TimeOutException.h"

namespace tgreiner::amy::common::timer
{
    FixedDepthTimerAlgorithm::FixedDepthTimerAlgorithm(int theDepth)
        : depth_(theDepth)
    {
    }

    void FixedDepthTimerAlgorithm::iterationFinished(int currentDepth)
    {
        if (currentDepth >= depth_)
        {
            throw TimeOutException();
        }
    }
}
