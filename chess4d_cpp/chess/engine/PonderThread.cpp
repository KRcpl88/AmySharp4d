#include "chess/engine/PonderThread.h"

#include <exception>

#include "chess/engine/Driver.h"
#include "chess/engine/ISearchOutput.h"
#include "chess/engine/ITransTable.h"
#include "common/timer/IChessTimer.h"
#include "common/timer/PonderingTimerDecorator.h"

namespace tgreiner::amy::chess::engine {

PonderThread::PonderThread(const ChessBoard& theBoard,
                           int thePonderMove,
                           ITransTable& theTransTable,
                           tgreiner::amy::common::timer::IChessTimer& theTimer,
                           ISearchOutput& theSearchOutput,
                           int maxDepth)
    : board_(theBoard),
      transTable_(theTransTable),
      searchOutput_(theSearchOutput),
      ponderTimer_(std::make_unique<tgreiner::amy::common::timer::PonderingTimerDecorator>(theTimer)),
      ponderMove_(thePonderMove),
      maxDepth_(maxDepth)
{
    board_.doMove(thePonderMove);
    ponderThread_ = std::thread(&PonderThread::Run, this);
}

PonderThread::~PonderThread()
{
    abort();
}

int PonderThread::getPonderMove() const
{
    return ponderMove_.load();
}

int PonderThread::getBestMove() const
{
    return bestMove_.load();
}

int PonderThread::getNextPonderMove() const
{
    return nextPonderMove_.load();
}

void PonderThread::Run()
{
    try {
        Driver driver(board_, transTable_, *ponderTimer_);
        driver.setSearchOutput(&searchOutput_);
        bestMove_.store(driver.search(maxDepth_));
        nextPonderMove_.store(driver.getPonderMove());
    } catch (const std::exception&) {
        bestMove_.store(0);
        nextPonderMove_.store(0);
    }
}

void PonderThread::abort()
{
    if (ponderTimer_) {
        ponderTimer_->abort();
    }
    joinThread();
}

void PonderThread::ponderHit()
{
    if (ponderTimer_) {
        ponderTimer_->stopPondering();
    }
    joinThread();
}

void PonderThread::joinThread()
{
    std::lock_guard<std::mutex> lock(joinMutex_);
    if (ponderThread_.joinable()) {
        ponderThread_.join();
    }
}

} // namespace tgreiner::amy::chess::engine
