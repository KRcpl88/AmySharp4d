#include "chess/engine/ExtendedQuiescenceSearch.h"

#include <algorithm>

#include "chess/engine/CheckEvasionMoveGenerator.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/ExtendedQuiescenceMoveGenerator.h"
#include "chess/engine/HistoryTable.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/NonLoosingCaptureMoveGenerator.h"
#include "chess/engine/PVSaver.h"
#include "chess/engine/ISearcher.h"

namespace tgreiner::amy::chess::engine {

ExtendedQuiescenceSearch::ExtendedQuiescenceSearch(ChessBoard& board, ITransTable& transTable, HistoryTable& history)
    : board_(board)
    , evaluator_(board.getEvaluator())
    , ownedPVSaver_(std::make_unique<PVSaver>())
    , pvsaver_(ownedPVSaver_.get())
    , transTable_(transTable)
{
    initGenerators(history);
}

ExtendedQuiescenceSearch::ExtendedQuiescenceSearch(ChessBoard& board, PVSaver& pvsaver, ITransTable& transTable, HistoryTable& history)
    : board_(board)
    , evaluator_(board.getEvaluator())
    , pvsaver_(&pvsaver)
    , transTable_(transTable)
{
    initGenerators(history);
}

ExtendedQuiescenceSearch::~ExtendedQuiescenceSearch() = default;

void ExtendedQuiescenceSearch::initGenerators(HistoryTable& history)
{
    generators_.reserve(MAX_DEPTH);
    for (int i = 0; i < MAX_DEPTH; ++i) {
        generators_.push_back(std::make_unique<NonLoosingCaptureMoveGenerator>(board_));
    }
    for (auto& generator : checkEvasionGenerators_) {
        generator = std::make_unique<CheckEvasionMoveGenerator>(board_, transTable_, history);
    }
    extendedGenerator_ = std::make_unique<ExtendedQuiescenceMoveGenerator>(board_, transTable_);
}

std::int64_t ExtendedQuiescenceSearch::getNodes() const
{
    return nodes_;
}

int ExtendedQuiescenceSearch::search(int alpha, int beta, int depth, int ply)
{
    ++nodes_;

    if (ply >= MAX_DEPTH) {
        return 0;
    }

    if (board_.getRepeated() || board_.getInsufficientMaterial()) {
        if (alpha < 0 && beta > 0) {
            pvsaver_->terminal(ply);
        }
        return 0;
    }

    if (depth < 2 && board_.getInCheck()) {
        return evadeCheck(alpha, beta, depth, ply);
    }

    if (depth == 0) {
        return extendedQ(alpha, beta, ply);
    }

    evaluator_ = board_.getEvaluator();
    int best = evaluator_->evaluate(alpha, beta);
    const int staticEval = best;

    if (best >= beta) {
        return best;
    }
    if (best > alpha) {
        pvsaver_->terminal(ply);
    }

    NonLoosingCaptureMoveGenerator& gen = *generators_[static_cast<std::size_t>(ply)];
    gen.reset();

    int move = 0;
    while ((move = gen.nextMove()) != -1) {
        const int promoGain = (move & Move::PROMO_QUEEN) != 0
            ? (evaluator_->getMaterialValue(ChessConstants_Fields::QUEEN) - evaluator_->getMaterialValue(ChessConstants_Fields::PAWN))
            : 0;
        if (staticEval + promoGain + evaluator_->getMaterialValue(board_.getPieceAt(Move::getTo(move))) + evaluator_->getMaterialValue(ChessConstants_Fields::PAWN) < alpha) {
            continue;
        }

        board_.doMove(move);
        if (board_.getOppInCheck()) {
            board_.undoMove();
            continue;
        }

        const int tmp = -search(-beta, -std::max(alpha, best), depth + 1, ply + 1);
        board_.undoMove();

        if (tmp > best) {
            best = tmp;
            if (best >= beta) {
                break;
            }
            if (best > alpha) {
                pvsaver_->move(ply, move);
            }
        }
    }

    return best;
}

int ExtendedQuiescenceSearch::search(int alpha, int beta, int depth, int ply, const tgreiner::amy::common::engine::NodeType&)
{
    return search(alpha, beta, depth, ply);
}

int ExtendedQuiescenceSearch::evadeCheck(int alpha, int beta, int depth, int ply)
{
    int best = -Searcher_Fields::MATE + ply;
    int bestMove = 0;

    if (best >= beta) {
        return best;
    }
    if (best > alpha) {
        pvsaver_->terminal(ply);
    }

    CheckEvasionMoveGenerator& gen = *checkEvasionGenerators_[static_cast<std::size_t>(depth)];
    gen.reset();

    int move = 0;
    while ((move = gen.nextMove()) != -1) {
        if ((move & Move::CASTLE) != 0 && !board_.isCastleLegal(move)) {
            continue;
        }

        board_.doMove(move);
        if (board_.getOppInCheck()) {
            board_.undoMove();
            continue;
        }

        const int tmp = -search(-beta, -std::max(alpha, best), depth + 1, ply + 1);
        board_.undoMove();

        if (tmp > best) {
            best = tmp;
            bestMove = move;
            if (best >= beta) {
                gen.failHigh(move, depth);
                break;
            }
            if (best > alpha) {
                pvsaver_->move(ply, move);
            }
        }
    }

    transTable_.store(board_.getPosHash(), bestMove, 0, best, alpha, beta);
    return best;
}

int ExtendedQuiescenceSearch::extendedQ(int alpha, int beta, int ply)
{
    evaluator_ = board_.getEvaluator();
    int best = evaluator_->evaluate(alpha, beta);
    int bestMove = 0;

    if (best >= beta) {
        return best;
    }
    if (best > alpha) {
        pvsaver_->terminal(ply);
    }

    extendedGenerator_->reset();

    int move = 0;
    while ((move = extendedGenerator_->nextMove()) != -1) {
        board_.doMove(move);
        if (board_.getOppInCheck()) {
            board_.undoMove();
            continue;
        }

        const int tmp = -search(-beta, -std::max(alpha, best), 1, ply + 1);
        board_.undoMove();

        if (tmp > best) {
            best = tmp;
            bestMove = move;
            if (best >= beta) {
                extendedGenerator_->failHigh(move, 1);
                break;
            }
            if (best > alpha) {
                pvsaver_->move(ply, move);
            }
        }
    }

    transTable_.store(board_.getPosHash(), bestMove, 0, best, alpha, beta);
    return best;
}

void ExtendedQuiescenceSearch::reset()
{
    resetStats();
}

void ExtendedQuiescenceSearch::resetStats()
{
    nodes_ = 0;
}

PVSaver& ExtendedQuiescenceSearch::getPVSaver()
{
    return *pvsaver_;
}

} // namespace tgreiner::amy::chess::engine
