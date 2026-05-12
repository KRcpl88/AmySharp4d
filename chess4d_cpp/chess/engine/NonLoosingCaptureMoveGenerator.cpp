#include "chess/engine/NonLoosingCaptureMoveGenerator.h"

#include "chess/engine/EvalMasks.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

NonLoosingCaptureMoveGenerator::NonLoosingCaptureMoveGenerator(ChessBoard& theBoard)
    : board(theBoard) {
    reset();
}

int NonLoosingCaptureMoveGenerator::nextMove() {
    switch (phase_) {
    case GENERATE_CAPTURES: {
        moves_.setSize(0);
        swapOffs_.setSize(0);
        auto victims = board.getMask(!board.getWhiteToMove());
        while (!victims.IsEmpty()) {
            const int square = victims.findFirstOne();
            victims.ClearBit(square);
            board.generateTo(square, moves_);
        }

        auto pawnOn7th = board.getMask(board.getWhiteToMove(), ChessConstants_Fields::PAWN) &
            EvalMasks::RANK_MASK[board.getWhiteToMove() ? 6 : 1];
        while (!pawnOn7th.IsEmpty()) {
            const int square = pawnOn7th.findFirstOne();
            pawnOn7th.ClearBit(square);
            const int to = board.getWhiteToMove() ? square + 8 : square - 8;
            if (board.getPieceAt(to) != 0) {
                continue;
            }
            moves_.add(Move::makeMove(square, to) | Move::PROMO_QUEEN);
        }

        nMoves_ = moves_.size();
        for (int i = 0; i < nMoves_; ++i) {
            swapOffs_.add(swapper_.swap(board, moves_.get_Renamed(i)));
        }

        board.generateEnPassant(moves_);
        while (swapOffs_.size() < moves_.size()) {
            swapOffs_.add(0);
        }
        nMoves_ = moves_.size();
        phase_ = GAINING_CAPTURES;
        [[fallthrough]];
    }

    case GAINING_CAPTURES:
        while (nMoves_ > 0) {
            int bestIdx = 0;
            int bestSwap = swapOffs_.get_Renamed(0);
            for (int i = 1; i < nMoves_; ++i) {
                if (swapOffs_.get_Renamed(i) > bestSwap) {
                    bestSwap = swapOffs_.get_Renamed(i);
                    bestIdx = i;
                }
            }

            if (bestSwap < 0) {
                break;
            }

            --nMoves_;
            const int move = moves_.get_Renamed(bestIdx);
            moves_.swap(bestIdx, nMoves_);
            swapOffs_.swap(bestIdx, nMoves_);

            phase_ = GAINING_CAPTURES;
            return move;
        }
        break;
    }

    return -1;
}

void NonLoosingCaptureMoveGenerator::reset() {
    phase_ = GENERATE_CAPTURES;
}

void NonLoosingCaptureMoveGenerator::failHigh(int, int) {
}

} // namespace tgreiner::amy::chess::engine
