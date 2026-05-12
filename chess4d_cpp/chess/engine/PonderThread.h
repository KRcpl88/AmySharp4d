#pragma once

#include <atomic>
#include <memory>
#include <mutex>
#include <thread>

#include "chess/engine/ChessBoard.h"

namespace tgreiner::amy::chess::engine {

class ITransTable;
class ISearchOutput;

} // namespace tgreiner::amy::chess::engine

namespace tgreiner::amy::common::timer {
class IChessTimer;
class PonderingTimerDecorator;
}

namespace tgreiner::amy::chess::engine {

class PonderThread {
public:
    PonderThread(const ChessBoard& theBoard,
                 int thePonderMove,
                 ITransTable& theTransTable,
                 tgreiner::amy::common::timer::IChessTimer& theTimer,
                 ISearchOutput& theSearchOutput,
                 int maxDepth);
    ~PonderThread();

    int getPonderMove() const;
    int getBestMove() const;
    int getNextPonderMove() const;

    void Run();
    void abort();
    void ponderHit();

private:
    void joinThread();

    ChessBoard board_;
    ITransTable& transTable_;
    ISearchOutput& searchOutput_;
    std::unique_ptr<tgreiner::amy::common::timer::PonderingTimerDecorator> ponderTimer_;
    std::thread ponderThread_;
    std::mutex joinMutex_;
    std::atomic<int> ponderMove_{0};
    std::atomic<int> bestMove_{0};
    std::atomic<int> nextPonderMove_{0};
    int maxDepth_ = 0;
};

} // namespace tgreiner::amy::chess::engine
