#include "chess/engine/ExtendedQuiescenceMoveGenerator.h"

#include "chess/engine/EvalMasks.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

ExtendedQuiescenceMoveGenerator::ExtendedQuiescenceMoveGenerator(ChessBoard& theBoard, ITransTable& theTransTable)
    : board_(theBoard)
    , checkGenerator_(theBoard)
    , transTable_(theTransTable) {
    reset();
}

int ExtendedQuiescenceMoveGenerator::nextMove() {
    for (;;) {
        switch (phase_) {
        case HASH_MOVE: {
            phase_ = GENERATE_CAPTURES;
            TTEntry* entry = transTable_.get_Renamed(board_.getPosHash());
            if (entry != nullptr && entry->move != 0 && board_.isPseudoLegalMove(entry->move)) {
                hashMove_ = entry->move;
                return hashMove_;
            }
            break;
        }

        case GENERATE_CAPTURES: {
            moves_.setSize(0);
            swapOffs_.setSize(0);
            auto victims = board_.getMask(!board_.getWhiteToMove());
            while (!victims.IsEmpty()) {
                const int square = victims.findFirstOne();
                victims.ClearBit(square);
                board_.generateTo(square, moves_);
            }

            auto pawnOn7th = board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::PAWN) &
                EvalMasks::RANK_MASK[board_.getWhiteToMove() ? 6 : 1];
            while (!pawnOn7th.IsEmpty()) {
                const int square = pawnOn7th.findFirstOne();
                pawnOn7th.ClearBit(square);
                const int to = board_.getWhiteToMove() ? square + 8 : square - 8;
                if (board_.getPieceAt(to) != 0) {
                    continue;
                }
                moves_.add(Move::makeMove(square, to) | Move::PROMO_QUEEN);
            }

            nMoves_ = moves_.size();
            for (int i = 0; i < nMoves_; ++i) {
                swapOffs_.add(swapper_.swap(board_, moves_.get_Renamed(i)));
            }

            board_.generateEnPassant(moves_);
            while (swapOffs_.size() < moves_.size()) {
                swapOffs_.add(0);
            }
            nMoves_ = moves_.size();
            phase_ = GAINING_CAPTURES;
            break;
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
                    phase_ = GENERATE_CHECKS;
                    break;
                }

                --nMoves_;
                const int move = moves_.get_Renamed(bestIdx);
                moves_.swap(bestIdx, nMoves_);
                swapOffs_.swap(bestIdx, nMoves_);

                phase_ = GAINING_CAPTURES;
                if (move != hashMove_) {
                    return move;
                }
            }
            if (phase_ == GAINING_CAPTURES) {
                phase_ = GENERATE_CHECKS;
            }
            break;

        case GENERATE_CHECKS:
            checkingMoves_.setSize(0);
            checkGenerator_.generateNChecks(checkingMoves_);
            checkGenerator_.generateBRQChecks(checkingMoves_);
            nChecks_ = checkingMoves_.size();
            phase_ = CHECKS;
            break;

        case CHECKS:
            while (nChecks_ > 0) {
                --nChecks_;
                phase_ = CHECKS;
                const int move = checkingMoves_.get_Renamed(nChecks_);
                if (move != hashMove_ && swapper_.swap(board_, move) >= 0) {
                    return move;
                }
            }
            phase_ = LOOSING_CAPTURES;
            break;

        case LOOSING_CAPTURES:
            return -1;
        }
    }
}

void ExtendedQuiescenceMoveGenerator::reset() {
    phase_ = HASH_MOVE;
    hashMove_ = 0;
}

void ExtendedQuiescenceMoveGenerator::failHigh(int, int) {
}

} // namespace tgreiner::amy::chess::engine
