#include "common\timer\SuddenDeathTimeControl.h"

namespace tgreiner::amy::common::timer
{
    SuddenDeathTimeControl::SuddenDeathTimeControl(int theTime)
        : time_(theTime), remainingTime_(MICROS_PER_SECOND * theTime)
    {
    }

    int SuddenDeathTimeControl::getSoftLimit() const
    {
        int limit = (MICROS_PER_SECOND * time_) / MOVES;
        if (limit > remainingTime_)
        {
            limit = remainingTime_ / 2;
        }

        return limit;
    }

    int SuddenDeathTimeControl::getHardLimit() const
    {
        int limit = 4 * getSoftLimit();
        if (limit > remainingTime_)
        {
            limit = (3 * remainingTime_) / 4;
        }

        return limit;
    }

    void SuddenDeathTimeControl::setRemainingTime(int remaining)
    {
        remainingTime_ = remaining;
    }
}
