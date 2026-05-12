#include "common\timer\ExtendOnFailLowTimerAlgorithm.h"
#include "common\timer\TimeOutException.h"

namespace tgreiner::amy::common::timer
{
    ExtendOnFailLowTimerAlgorithm::ExtendOnFailLowTimerAlgorithm(int theTime, int theExtended)
    {
        setDuration(theTime, theExtended);
    }

    void ExtendOnFailLowTimerAlgorithm::check(int currentTime)
    {
        if (currentTime >= threshold_)
        {
            throw TimeOutException();
        }
    }

    void ExtendOnFailLowTimerAlgorithm::setDuration(int theTime, int theExtended)
    {
        time_ = theTime;
        extended_ = theExtended;
        threshold_ = time_;
    }

    void ExtendOnFailLowTimerAlgorithm::failLow()
    {
        threshold_ = extended_;
    }

    void ExtendOnFailLowTimerAlgorithm::iterationFinished(int iteration)
    {
        (void)iteration;
        threshold_ = time_;
    }
}
