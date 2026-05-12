#pragma once

#include "common\timer\IChessTimer.h"

#include <chrono>

namespace tgreiner::amy::common::timer
{
    class AbstractTimer : public IChessTimer
    {
    public:
        AbstractTimer();
        ~AbstractTimer() override = default;

        int getTime() const override;
        void start() override;
        virtual void check() override = 0;
        virtual void failLow() override = 0;
        virtual void iterationFinished(int iteration) override = 0;

    protected:
        std::chrono::steady_clock::time_point startTime_;
    };
}
