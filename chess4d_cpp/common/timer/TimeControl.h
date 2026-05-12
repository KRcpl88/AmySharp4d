#pragma once

namespace tgreiner::amy::common::timer
{
    class TimeControl
    {
    public:
        virtual ~TimeControl() = default;

        virtual int getSoftLimit() const = 0;
        virtual int getHardLimit() const = 0;
        virtual void setRemainingTime(int remaining) = 0;
    };
}
