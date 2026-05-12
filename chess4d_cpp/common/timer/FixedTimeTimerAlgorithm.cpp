#include "common\timer\FixedTimeTimerAlgorithm.h"
#include "common\timer\TimeOutException.h"

namespace tgreiner::amy::common::timer
{
    FixedTimeTimerAlgorithm::FixedTimeTimerAlgorithm(int theTime)
        : time_(theTime)
    {
    }

    void FixedTimeTimerAlgorithm::setDuration(int theTime)
    {
        time_ = theTime;
    }

    void FixedTimeTimerAlgorithm::check(int currentTime)
    {
        if (currentTime >= time_)
        {
            throw TimeOutException();
        }
    }
}
