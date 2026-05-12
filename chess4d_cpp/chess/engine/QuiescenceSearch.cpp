#include "chess/engine/QuiescenceSearch.h"

#include <algorithm>

#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/Move.h"
#include "chess/engine/NonLoosingCaptureMoveGenerator.h"
#include "chess/engine/PVSaver.h"

namespace tgreiner::amy::chess::engine {

QuiescenceSearch::QuiescenceSearch(ChessBoard& board)
    : board_(board)
    , evaluator_(board.getEvaluator())
    , ownedPVSaver_(std::make_unique<PVSaver>())
    , pvsaver_(ownedPVSaver_.get())
{
    initGenerators();
}

QuiescenceSearch::QuiescenceSearch(ChessBoard& board, PVSaver& pvsaver)
    : board_(board)
    , evaluator_(board.getEvaluator())
    , pvsaver_(&pvsaver)
{
    initGenerators();
}

QuiescenceSearch::~QuiescenceSearch() = default;

void QuiescenceSearch::initGenerators()
{
    generators_.reserve(MAX_DEPTH);
    for (int i = 0; i < MAX_DEPTH; ++i) {
        generators_.push_back(std::make_unique<NonLoosingCaptureMoveGenerator>(board_));
    }
}

std::int64_t QuiescenceSearch::getNodes() const
{
    return nodes_;
}

int QuiescenceSearch::search(int alpha, int beta, int depth, int ply)
{
    ++nodes_;

    if (ply >= MAX_DEPTH) {
        return 0;
    }

    if (board_.getInsufficientMaterial()) {
        return 0;
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

        const int tmp = -search(-beta, -std::max(alpha, best), depth - 1, ply + 1);
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

void QuiescenceSearch::reset()
{
    resetStats();
}

void QuiescenceSearch::resetStats()
{
    nodes_ = 0;
}

PVSaver& QuiescenceSearch::getPVSaver()
{
    return *pvsaver_;
}

} // namespace tgreiner::amy::chess::engine
