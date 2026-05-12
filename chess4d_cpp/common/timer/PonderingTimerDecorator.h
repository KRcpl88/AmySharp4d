#pragma once

#include "common\timer\IChessTimer.h"

namespace tgreiner::amy::common::timer
{
    class PonderingTimerDecorator : public IChessTimer
    {
    public:
        explicit PonderingTimerDecorator(IChessTimer& theDecorated);
        ~PonderingTimerDecorator() override = default;

        int getTime() const override;
        void check() override;
        void start() override;
        void iterationFinished(int iteration) override;
        void failLow() override;
        void stopPondering();
        void abort();

    private:
        bool pondering_ = true;
        bool aborted_ = false;
        IChessTimer& decorated_;
    };
}
