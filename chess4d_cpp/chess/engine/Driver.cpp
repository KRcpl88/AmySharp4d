#include "chess/engine/Driver.h"

#include <iostream>
#include <string>

#include "chess/engine/ChessBoard.h"
#include "chess/engine/EvaluatorImpl.h"
#include "chess/engine/ISearchOutput.h"
#include "chess/engine/ISearcher.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/NegaScout.h"
#include "chess/engine/PVSaver.h"
#include "chess/engine/RootMoveList.h"
#include "chess/engine/SearchOutputTextUI.h"
#include "chess/engine/TransTableImpl2.h"
#include "common/engine/IntVector.h"
#include "common/engine/NodeType.h"
#include "common/timer/IChessTimer.h"
#include "common/timer/TimeOutException.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::common::engine::IntVector;
using tgreiner::amy::common::engine::NodeType;

Driver::Driver(ChessBoard& board, ITransTable& ttable, tgreiner::amy::common::timer::IChessTimer& timer)
    : board_(board)
    , timer_(timer)
    , transTable_(&ttable)
{
    init();
}

Driver::Driver(ChessBoard& board, tgreiner::amy::common::timer::IChessTimer& timer, int hashBits, ISearchOutput* output)
    : board_(board)
    , timer_(timer)
    , hashBits_(hashBits)
    , output_(output)
{
    ownedTransTable_ = std::make_unique<TransTableImpl2>(hashBits_);
    transTable_ = ownedTransTable_.get();
    init();
}

Driver::~Driver() = default;

void Driver::init()
{
    evaluator_ = std::make_unique<EvaluatorImpl>(board_);
    board_.setEvaluator(evaluator_.get());

    pvsaver_ = std::make_unique<PVSaver>();

    if (output_ == nullptr) {
        ownedOutput_ = std::make_unique<SearchOutputTextUI>(std::cout);
        output_ = ownedOutput_.get();
    }

    searcher_ = std::make_unique<NegaScout>(board_, *transTable_, *pvsaver_, timer_);
}

void Driver::setSearchOutput(ISearchOutput* output)
{
    output_ = output;
    if (output_ != nullptr) {
        ownedOutput_.reset();
    } else {
        ownedOutput_ = std::make_unique<SearchOutputTextUI>(std::cout);
        output_ = ownedOutput_.get();
    }
}

int Driver::getRootScore() const
{
    return bestScore_;
}

int Driver::getPonderMove() const
{
    return ponderMove_;
}

int Driver::researchFailLow(int move, int bound, int depth)
{
    board_.doMove(move);
    try {
        int beta = bound;
        int alpha = beta - researchWindow1_;

        int tmp = -searcher_->search(-beta, -alpha, depth - 1, NodeType::PV);
        if (tmp <= alpha) {
            beta = tmp;
            alpha = -Searcher_Fields::MATE;
            tmp = -searcher_->search(-beta, -alpha, depth - 1, NodeType::PV);
        }

        board_.undoMove();
        return tmp;
    } catch (...) {
        board_.undoMove();
        throw;
    }
}

int Driver::researchFailHigh(int move, int bound, int depth)
{
    board_.doMove(move);
    try {
        int alpha = bound;
        int beta = alpha + researchWindow1_;

        int tmp = -searcher_->search(-beta, -alpha, depth - 1, NodeType::PV);
        if (tmp >= beta) {
            alpha = tmp;
            beta = Searcher_Fields::MATE;
            tmp = -searcher_->search(-beta, -alpha, depth - 1, NodeType::PV);
        }

        board_.undoMove();
        return tmp;
    } catch (...) {
        board_.undoMove();
        throw;
    }
}

int Driver::search(int maxDepth)
{
    IntVector moves;
    board_.generateLegalMoves(moves);

    bestScore_ = 0;

    if (moves.size() == 0) {
        return 0;
    }
    if (moves.size() == 1) {
        return moves.get_Renamed(0);
    }

    searcher_->reset();
    transTable_->clear();
    timer_.start();

    int alpha = 0;
    int beta = 0;
    int mateFound = 0;
    RootMoveList rml(moves);
    std::string pv;

    output_->header();

    try {
        for (int depth = 1; depth < maxDepth; ++depth) {
            int move = rml.next();
            int nodes = searcher_->getNodes();

            output_->move(depth, timer_.getTime(), Move::toSAN(board_, move), 0, moves.size());

            alpha = bestScore_ - window_;
            beta = bestScore_ + window_;

            board_.doMove(move);
            try {
                bestScore_ = -searcher_->search(-beta, -alpha, depth - 1, NodeType::PV);
            } catch (...) {
                board_.undoMove();
                throw;
            }
            board_.undoMove();

            if (bestScore_ <= alpha) {
                timer_.failLow();
                output_->failLow(depth, timer_.getTime(), Move::toSAN(board_, move));
                bestScore_ = researchFailLow(move, bestScore_, depth);
            } else if (bestScore_ >= beta) {
                output_->failHigh(depth, timer_.getTime(), Move::toSAN(board_, move));
                bestScore_ = researchFailHigh(move, bestScore_, depth);
            }

            pvsaver_->move(0, move);
            pv = pvsaver_->getPV(board_);
            ponderMove_ = pvsaver_->getPonderMove();

            output_->pv(depth, timer_.getTime(), bestScore_, pv, searcher_->getNodes());

            alpha = bestScore_;
            beta = bestScore_ + 1;

            nodes = searcher_->getNodes() - nodes;
            rml.setNodes(nodes);

            int i = 0;
            while (rml.hasNext()) {
                move = rml.next();
                ++i;

                output_->move(depth, timer_.getTime(), Move::toSAN(board_, move), i, moves.size());

                nodes = searcher_->getNodes();
                board_.doMove(move);
                int tmp = 0;
                try {
                    tmp = -searcher_->search(-beta, -alpha, depth - 1, NodeType::CUT);
                } catch (...) {
                    board_.undoMove();
                    throw;
                }
                board_.undoMove();

                if (tmp > bestScore_) {
                    output_->failHigh(depth, timer_.getTime(), Move::toSAN(board_, move));
                    rml.failHigh();

                    bestScore_ = researchFailHigh(move, tmp, depth);
                    pvsaver_->move(0, move);
                    pv = pvsaver_->getPV(board_);
                    ponderMove_ = pvsaver_->getPonderMove();
                    alpha = bestScore_;
                    beta = bestScore_ + 1;

                    output_->pv(depth, timer_.getTime(), bestScore_, pv, searcher_->getNodes());

                    nodes = searcher_->getNodes() - nodes;
                    rml.setNodes(nodes);
                } else {
                    nodes = searcher_->getNodes() - nodes;
                    rml.setNodes(nodes);
                }
            }

            rml.resort();
            output_->pv(depth, timer_.getTime(), bestScore_, pv, searcher_->getNodes());

            if (bestScore_ < -Searcher_Fields::MATE_LIMIT || bestScore_ > Searcher_Fields::MATE_LIMIT) {
                ++mateFound;
                if (mateFound == 3) {
                    break;
                }
            } else {
                mateFound = 0;
            }

            timer_.iterationFinished(depth);
        }
    } catch (const tgreiner::amy::common::timer::TimeOutException&) {
    }

    return rml.Best(0);
}

} // namespace tgreiner::amy::chess::engine
