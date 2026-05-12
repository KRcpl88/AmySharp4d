#include "common\timer\QuotaTimeControl.h"

namespace tgreiner::amy::common::timer
{
    QuotaTimeControl::QuotaTimeControl(int theMoves, int theTime)
        : moves_(theMoves), time_(theTime), remainingTime_(MICROS_PER_SECOND * theTime)
    {
    }

    int QuotaTimeControl::getSoftLimit() const
    {
        int limit = (MICROS_PER_SECOND * time_) / moves_;
        if (limit > remainingTime_)
        {
            limit = remainingTime_ / 2;
        }

        return limit;
    }

    int QuotaTimeControl::getHardLimit() const
    {
        int limit = 4 * getSoftLimit();
        if (limit > remainingTime_)
        {
            limit = (3 * remainingTime_) / 4;
        }

        return limit;
    }

    void QuotaTimeControl::setRemainingTime(int remaining)
    {
        remainingTime_ = remaining;
    }
}
