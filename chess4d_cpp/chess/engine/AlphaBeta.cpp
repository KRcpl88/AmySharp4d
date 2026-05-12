#include "chess/engine/AlphaBeta.h"

#include <algorithm>

#include "bitboard/BitBoard.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/HistoryTable.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/MoveGenerator.h"
#include "chess/engine/PVSaver.h"
#include "chess/engine/QuiescenceSearch.h"
#include "chess/engine/TTEntry.h"
#include "common/engine/NodeType.h"

namespace tgreiner::amy::chess::engine {

AlphaBeta::AlphaBeta(ChessBoard& board, ITransTable& ttable, HistoryTable& history)
    : board_(board)
    , ttable_(ttable)
    , history_(&history)
    , ownedPVSaver_(std::make_unique<PVSaver>())
    , pvsaver_(ownedPVSaver_.get())
{
    qsearch_ = std::make_unique<QuiescenceSearch>(board_, *pvsaver_);
    initGenerators();
}

AlphaBeta::AlphaBeta(ChessBoard& board, ITransTable& ttable, PVSaver& pvsaver)
    : board_(board)
    , ttable_(ttable)
    , ownedHistory_(std::make_unique<HistoryTable>())
    , history_(ownedHistory_.get())
    , pvsaver_(&pvsaver)
{
    qsearch_ = std::make_unique<QuiescenceSearch>(board_, *pvsaver_);
    initGenerators();
}

AlphaBeta::~AlphaBeta() = default;

void AlphaBeta::initGenerators()
{
    generators_.reserve(MAX_DEPTH);
    for (int i = 0; i < MAX_DEPTH; ++i) {
        generators_.push_back(std::make_unique<MoveGenerator>(board_, ttable_, *history_));
    }
}

int AlphaBeta::getNodes() const
{
    return static_cast<int>(nodes_ + qsearch_->getNodes());
}

int AlphaBeta::search(int alpha, int beta, int depth, const tgreiner::amy::common::engine::NodeType&)
{
    return search(alpha, beta, depth, 1);
}

int AlphaBeta::search(int alpha, int beta, int depth, int ply)
{
    if (depth <= 0) {
        return qsearch_->search(alpha, beta, 0, ply);
    }

    ++nodes_;

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
    }

    if (!board_.getInCheck() && !board_.getMaskNonPawn().IsEmpty()) {
        board_.doNull();
        const int tmp = -search(-beta, -beta + 1, depth - 3, ply + 1);
        board_.undoMove();
        if (tmp >= beta) {
            return tmp;
        }
    }

    MoveGenerator& gen = *generators_[static_cast<std::size_t>(ply)];
    gen.reset();

    int move = 0;
    int bestMove = 0;
    int best = -Searcher_Fields::MATE + ply;

    while ((move = gen.nextMove()) != -1) {
        if ((move & Move::CASTLE) != 0 && !board_.isCastleLegal(move)) {
            continue;
        }

        board_.doMove(move);
        if (board_.getOppInCheck()) {
            board_.undoMove();
            continue;
        }

        int depthSub = 1;
        if (board_.getInCheck()) {
            depthSub = 0;
        }

        const int tmp = -search(-beta, -std::max(alpha, best), depth - depthSub, ply + 1);
        board_.undoMove();

        if (tmp > best) {
            best = tmp;
            bestMove = move;
            if (tmp < beta) {
                pvsaver_->move(ply, bestMove);
            }
        }
        if (best >= beta) {
            gen.failHigh(move, depth);
            break;
        }
    }

    if (bestMove != 0) {
        ttable_.store(board_.getPosHash(), bestMove, depth, best, alpha, beta);
    } else {
        if (!board_.getInCheck()) {
            best = 0;
        }
        if (alpha < best && best < beta) {
            pvsaver_->terminal(ply);
        }
    }

    return best;
}

void AlphaBeta::reset()
{
    qsearch_->resetStats();
    nodes_ = 0;
}

PVSaver& AlphaBeta::getPVSaver()
{
    return *pvsaver_;
}

} // namespace tgreiner::amy::chess::engine
