#include "common\timer\PonderingTimerDecorator.h"
#include "common\timer\TimeOutException.h"

namespace tgreiner::amy::common::timer
{
    PonderingTimerDecorator::PonderingTimerDecorator(IChessTimer& theDecorated)
        : decorated_(theDecorated)
    {
    }

    int PonderingTimerDecorator::getTime() const
    {
        return decorated_.getTime();
    }

    void PonderingTimerDecorator::check()
    {
        if (aborted_)
        {
            throw TimeOutException();
        }
        if (!pondering_)
        {
            decorated_.check();
        }
    }

    void PonderingTimerDecorator::start()
    {
        decorated_.start();
    }

    void PonderingTimerDecorator::iterationFinished(int iteration)
    {
        if (aborted_)
        {
            throw TimeOutException();
        }
        if (!pondering_)
        {
            decorated_.iterationFinished(iteration);
        }
    }

    void PonderingTimerDecorator::failLow()
    {
        decorated_.failLow();
    }

    void PonderingTimerDecorator::stopPondering()
    {
        pondering_ = false;
    }

    void PonderingTimerDecorator::abort()
    {
        aborted_ = true;
    }
}
