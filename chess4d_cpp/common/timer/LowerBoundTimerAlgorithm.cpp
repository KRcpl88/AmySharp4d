#include "common\timer\LowerBoundTimerAlgorithm.h"
#include "common\timer\TimeOutException.h"

namespace tgreiner::amy::common::timer
{
    LowerBoundTimerAlgorithm::LowerBoundTimerAlgorithm(int theTime)
    {
        setDuration(theTime);
    }

    void LowerBoundTimerAlgorithm::setMaxDepth(int depth)
    {
        maxDepth_ = depth;
    }

    void LowerBoundTimerAlgorithm::setDuration(int theTime)
    {
        time_ = theTime;
        hard_ = 6 * time_;
    }

    void LowerBoundTimerAlgorithm::check(int currentTime)
    {
        if (currentTime >= time_)
        {
            if (currentTime > hard_)
            {
                throw TimeOutException();
            }

            terminate_ = true;
        }
    }

    void LowerBoundTimerAlgorithm::iterationFinished(int currentDepth)
    {
        if (currentDepth == 1)
        {
            terminate_ = false;
        }
        if (maxDepth_ != 0 && currentDepth >= maxDepth_)
        {
            terminate_ = true;
        }
        if (terminate_)
        {
            throw TimeOutException();
        }
    }
}
