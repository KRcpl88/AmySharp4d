#pragma once

#include <memory>

namespace tgreiner::amy::common::timer {
class IChessTimer;
}

namespace tgreiner::amy::chess::engine {

class ChessBoard;
class EvaluatorImpl;
class ISearchOutput;
class ISearcher;
class ITransTable;
class PVSaver;
class TransTableImpl2;

class Driver {
public:
    Driver(ChessBoard& board, ITransTable& ttable, tgreiner::amy::common::timer::IChessTimer& timer);
    Driver(ChessBoard& board, tgreiner::amy::common::timer::IChessTimer& timer, int hashBits, ISearchOutput* output = nullptr);
    ~Driver();

    void setSearchOutput(ISearchOutput* output);
    int getRootScore() const;
    int getPonderMove() const;
    int search(int maxDepth = 64);

private:
    void init();
    int researchFailLow(int move, int bound, int depth);
    int researchFailHigh(int move, int bound, int depth);

    ChessBoard& board_;
    tgreiner::amy::common::timer::IChessTimer& timer_;
    ITransTable* transTable_ = nullptr;
    int hashBits_ = 0;
    std::unique_ptr<ITransTable> ownedTransTable_;
    std::unique_ptr<EvaluatorImpl> evaluator_;
    std::unique_ptr<PVSaver> pvsaver_;
    std::unique_ptr<ISearcher> searcher_;
    std::unique_ptr<ISearchOutput> ownedOutput_;
    ISearchOutput* output_ = nullptr;
    int window_ = 33;
    int researchWindow1_ = 150;
    int ponderMove_ = 0;
    int bestScore_ = 0;
};

} // namespace tgreiner::amy::chess::engine
