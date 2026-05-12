#include "chess/engine/NegaScout.h"

#include <algorithm>

#include "chess/engine/CheckEvasionMoveGenerator.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/ExtendedQuiescenceSearch.h"
#include "chess/engine/HistoryTable.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/ISelectivity.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/MoveGenerator2.h"
#include "chess/engine/PVSaver.h"
#include "chess/engine/QuiescenceSearch.h"
#include "chess/engine/SelectivityImpl.h"
#include "chess/engine/TTEntry.h"
#include "chess/engine/recognizer/IRecognizer.h"
#include "chess/engine/recognizer/RecognizerMap.h"
#include "common/engine/NodeType.h"
#include "common/timer/IChessTimer.h"
#include "common/timer/TimeOutException.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::chess::engine::recognizer::IRecognizer;
using tgreiner::amy::common::engine::NodeType;

NegaScout::NegaScout(ChessBoard& board, ITransTable& ttable, PVSaver& pvsaver, tgreiner::amy::common::timer::IChessTimer& timer)
    : board_(board)
    , ttable_(ttable)
    , ownedHistory_(std::make_unique<HistoryTable>())
    , history_(ownedHistory_.get())
    , pvsaver_(&pvsaver)
    , ownedSelectivity_(std::make_unique<SelectivityImpl>())
    , selectivity_(ownedSelectivity_.get())
    , ownedRecognizerMap_(std::make_unique<recognizer::RecognizerMap>())
    , recognizerMap_(ownedRecognizerMap_.get())
    , timer_(timer)
{
    initGenerators();
    reset();
}

NegaScout::NegaScout(ChessBoard& board, ITransTable& ttable, HistoryTable& history, ISelectivity& selectivity, recognizer::RecognizerMap* recognizerMap, tgreiner::amy::common::timer::IChessTimer& timer)
    : board_(board)
    , ttable_(ttable)
    , history_(&history)
    , ownedPVSaver_(std::make_unique<PVSaver>())
    , pvsaver_(ownedPVSaver_.get())
    , selectivity_(&selectivity)
    , recognizerMap_(recognizerMap)
    , timer_(timer)
{
    initGenerators();
    reset();
}

NegaScout::~NegaScout() = default;

void NegaScout::initGenerators()
{
    generators_.reserve(MAX_DEPTH);
    checkEvasionGenerators_.reserve(MAX_DEPTH);

    for (int i = 0; i < MAX_DEPTH; ++i) {
        generators_.push_back(std::make_unique<MoveGenerator2>(board_, ttable_, *history_));
        checkEvasionGenerators_.push_back(std::make_unique<CheckEvasionMoveGenerator>(board_, ttable_, *history_));
    }

    for (int i = 2; i < MAX_DEPTH; ++i) {
        generators_[static_cast<std::size_t>(i)]->setTwoPliesBelow(generators_[static_cast<std::size_t>(i - 2)].get());
    }

    qsearch_ = std::make_unique<QuiescenceSearch>(board_, *pvsaver_);
    extQsearch_ = std::make_unique<ExtendedQuiescenceSearch>(board_, *pvsaver_, ttable_, *history_);
}

int NegaScout::getNodes() const
{
    return static_cast<int>(nodes_ + qsearch_->getNodes() + extQsearch_->getNodes());
}

int NegaScout::search(int alpha, int beta, int depth, const NodeType& nodeType)
{
    return search(alpha, beta, depth * DEPTH_STEP, 1, nodeType);
}

int NegaScout::search(int theAlpha, int theBeta, int depth, int ply, const NodeType& nodeType)
{
    if (ply >= MAX_DEPTH) {
        throw tgreiner::amy::common::timer::TimeOutException();
    }

    int alpha = theAlpha;
    int beta = theBeta;

    if (depth <= 0) {
        if (ply >= 3 && inCheckTab_[static_cast<std::size_t>(ply - 1)] && inCheckTab_[static_cast<std::size_t>(ply - 3)]) {
            return extQsearch_->search(alpha, beta, 0, ply);
        }
        return qsearch_->search(alpha, beta, 0, ply);
    }

    timer_.check();

    bool tryNullMove = true;

    ++nodes_;

    if (board_.getRepeated() || board_.getInsufficientMaterial()) {
        if (alpha < 0 && beta > 0) {
            pvsaver_->terminal(ply);
        }
        return 0;
    }

    TTEntry* entry = ttable_.get_Renamed(board_.getPosHash());
    if (entry != nullptr) {
        if (entry->getDepth() >= depth) {
            if (entry->getExact()) {
                pvsaver_->terminal(ply);
                return entry->score;
            }
            if (entry->getLower()) {
                if (entry->score >= beta) {
                    return entry->score;
                }
            }
            if (entry->getUpper()) {
                if (entry->score <= alpha) {
                    return entry->score;
                }
            }
        }
        if (entry->getUpper()) {
            tryNullMove = false;
        }
    }

    if (recognizerMap_ != nullptr) {
        IRecognizer* recog = recognizerMap_->getRecognizer(board_);
        if (recog != nullptr) {
            const int result = recog->probe(board_);
            if (result == IRecognizer::EXACT) {
                return recog->getValue();
            }
            if (result == IRecognizer::LOWER_BOUND) {
                if (recog->getValue() >= beta) {
                    return recog->getValue();
                }
                if (recog->getValue() > alpha) {
                    alpha = recog->getValue();
                }
            } else if (result == IRecognizer::UPPER_BOUND) {
                if (recog->getValue() <= alpha) {
                    return recog->getValue();
                }
                if (recog->getValue() < beta) {
                    beta = recog->getValue();
                }
            }
        }
    }

    const bool inCheck = board_.getInCheck();
    inCheckTab_[static_cast<std::size_t>(ply)] = inCheck;

    if (tryNullMove && board_.getLastMove() != 0 && !inCheck && board_.getMaskNonPawn().countBits() > 1) {
        ++nullTried_;
        board_.doNull();
        int tmp = 0;

        const int reduction = REDUCE_NULLMOVE;
        extensions_[static_cast<std::size_t>(ply)] = 0;

        try {
            const int nextDepth = depth - reduction;
            if (nextDepth <= 0) {
                tmp = -extQsearch_->search(-beta, -beta + 1, 0, ply + 1);
            } else {
                tmp = -search(-beta, -beta + 1, nextDepth, ply + 1, NodeType::ALL);
            }
        } catch (...) {
            board_.undoMove();
            throw;
        }
        board_.undoMove();

        if (tmp >= beta) {
            ++nullCutOffs_;
            ttable_.store(board_.getPosHash(), 0, depth, tmp, alpha, beta);
            return tmp;
        }
    }

    if (entry == nullptr && (nodeType == NodeType::CUT || nodeType == NodeType::PV) && (alpha + 1 != beta) && depth >= 3 * DEPTH_STEP) {
        search(alpha, beta, depth - 2 * DEPTH_STEP, ply, nodeType);
    }

    auto* gen = inCheck ? static_cast<tgreiner::amy::common::engine::Generator*>(checkEvasionGenerators_[static_cast<std::size_t>(ply)].get())
                        : static_cast<tgreiner::amy::common::engine::Generator*>(generators_[static_cast<std::size_t>(ply)].get());
    gen->reset();

    int move = 0;
    int bestMove = 0;
    int best = -Searcher_Fields::MATE;
    NodeType nextNodeType = nodeType.getSiblingType();

    while ((move = gen->nextMove()) != -1) {
        if ((move & Move::CASTLE) != 0 && !board_.isCastleLegal(move)) {
            continue;
        }

        int nextDepth = depth - DEPTH_STEP;
        int extend = selectivity_->extendPreDoMove(board_, move);

        if (!inCheck && bestMove != 0) {
            if (selectivity_->isFutile(board_, nextDepth + extend, move, alpha)) {
                continue;
            }
        }

        board_.doMove(move);
        if (board_.getOppInCheck()) {
            board_.undoMove();
            continue;
        }

        extend += selectivity_->extendAfterDoMove(board_);
        if (ply > 0 && (extend + extensions_[static_cast<std::size_t>(ply - 1)]) > 2 * DEPTH_STEP) {
            extend = 2 * DEPTH_STEP - extensions_[static_cast<std::size_t>(ply - 1)];
        }

        nextDepth += extend;
        extensions_[static_cast<std::size_t>(ply)] = extend;

        int tmp = 0;
        try {
            if (bestMove == 0) {
                tmp = -search(-beta, -std::max(alpha, best), nextDepth, ply + 1, nextNodeType);
            } else {
                const int talpha = std::max(alpha, best);
                tmp = -search(-(talpha + 1), -talpha, nextDepth, ply + 1, nextNodeType);
                if (tmp > talpha && tmp < beta) {
                    nextNodeType = nodeType.getSiblingType();
                    tmp = -search(-beta, -talpha, nextDepth, ply + 1, NodeType::PV);
                }
            }
            nextNodeType = NodeType::CUT;
        } catch (...) {
            board_.undoMove();
            throw;
        }
        board_.undoMove();

        if (tmp > best) {
            best = tmp;
            bestMove = move;
            if (best >= beta) {
                gen->failHigh(move, depth);
                break;
            }
            if (best > alpha) {
                pvsaver_->move(ply, move);
            }
        }
    }

    if (bestMove != 0) {
        if (best > alpha) {
            history_->addHistory(bestMove, depth / DEPTH_STEP);
        }
    } else {
        if (!inCheck) {
            best = 0;
        } else {
            best = -Searcher_Fields::MATE + ply;
        }

        if (alpha < best && best < beta) {
            pvsaver_->terminal(ply);
        }
    }

    ttable_.store(board_.getPosHash(), bestMove, depth, best, alpha, beta);
    return best;
}

void NegaScout::reset()
{
    history_->reset();
    qsearch_->resetStats();
    extQsearch_->resetStats();
    nodes_ = 0;
    nullTried_ = 0;
    nullCutOffs_ = 0;
    extensions_.fill(0);
    inCheckTab_.fill(false);
    evaluator_ = board_.getEvaluator();
    if (evaluator_ != nullptr) {
        evaluator_->init();
    }
}

PVSaver& NegaScout::getPVSaver()
{
    return *pvsaver_;
}

} // namespace tgreiner::amy::chess::engine
