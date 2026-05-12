#include "chess/engine/MoveGenerator2.h"

#include <utility>

#include "chess/engine/HistoryTable.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

MoveGenerator2::MoveGenerator2(ChessBoard& theBoard, ITransTable& theTtable, HistoryTable& theHistory)
    : ttable_(theTtable)
    , history_(theHistory)
    , board_(theBoard) {
    reset();
}

int MoveGenerator2::getKiller1() const {
    return killer1_;
}

int MoveGenerator2::getKiller2() const {
    return killer2_;
}

void MoveGenerator2::setTwoPliesBelow(MoveGenerator2* gen) {
    twoPliesBelow_ = gen;
}

int MoveGenerator2::nextMove() {
    for (;;) {
        switch (phase_) {
        case HASHMOVE: {
            phase_ = GENERATE_CAPTURES;
            TTEntry* entry = ttable_.get_Renamed(board_.getPosHash());
            if (entry != nullptr && entry->move != 0 && board_.isPseudoLegalMove(entry->move)) {
                hashMove_ = entry->move;
                return hashMove_;
            }
            break;
        }

        case GENERATE_CAPTURES: {
            captures_.setSize(0);
            swapOffs_.setSize(0);
            auto victims = board_.getMask(!board_.getWhiteToMove());
            if (inCheck_) {
                const int kingPos = board_.getKingPos(board_.getWhiteToMove());
                victims = victims & (board_.getAttackFrom(kingPos) | board_.getAttackTo(kingPos));
            }

            while (!victims.IsEmpty()) {
                const int square = victims.findFirstOne();
                victims.ClearBit(square);
                board_.generateTo(square, captures_);
            }

            nCaptures_ = captures_.size();
            for (int i = 0; i < nCaptures_; ++i) {
                swapOffs_.add(swapper_.swap(board_, captures_.get_Renamed(i)));
            }

            board_.generateEnPassant(captures_);
            while (swapOffs_.size() < captures_.size()) {
                swapOffs_.add(0);
                ++nCaptures_;
            }

            phase_ = GAINING_CAPTURES;
            break;
        }

        case GAINING_CAPTURES:
            while (nCaptures_ > 0) {
                int bestIdx = 0;
                int bestSwap = swapOffs_.get_Renamed(0);
                for (int i = 1; i < nCaptures_; ++i) {
                    if (swapOffs_.get_Renamed(i) > bestSwap) {
                        bestSwap = swapOffs_.get_Renamed(i);
                        bestIdx = i;
                    }
                }

                if (bestSwap < 0) {
                    phase_ = KILLER1;
                    break;
                }

                --nCaptures_;
                const int move = captures_.get_Renamed(bestIdx);
                captures_.swap(bestIdx, nCaptures_);
                swapOffs_.swap(bestIdx, nCaptures_);

                if (move != hashMove_) {
                    phase_ = GAINING_CAPTURES;
                    return move;
                }
            }
            if (phase_ == GAINING_CAPTURES) {
                phase_ = KILLER1;
            }
            break;

        case KILLER1:
            phase_ = KILLER2;
            if (killer1_ != hashMove_ && board_.isPseudoLegalMove(killer1_)) {
                return killer1_;
            }
            break;

        case KILLER2:
            phase_ = KILLER3;
            if (killer2_ != hashMove_ && board_.isPseudoLegalMove(killer2_)) {
                return killer2_;
            }
            break;

        case KILLER3:
            phase_ = LOOSING_CAPTURES;
            if (twoPliesBelow_ != nullptr) {
                killer3_ = twoPliesBelow_->getKiller1();
                if (killer3_ != hashMove_ && killer3_ != killer1_ && killer3_ != killer2_ && board_.isPseudoLegalMove(killer3_)) {
                    return killer3_;
                }
                killer3_ = twoPliesBelow_->getKiller2();
                if (killer3_ != hashMove_ && killer3_ != killer1_ && killer3_ != killer2_ && board_.isPseudoLegalMove(killer3_)) {
                    return killer3_;
                }
            }
            break;

        case LOOSING_CAPTURES:
            while (nCaptures_ > 0) {
                int bestIdx = 0;
                int bestSwap = swapOffs_.get_Renamed(0);
                for (int i = 1; i < nCaptures_; ++i) {
                    if (swapOffs_.get_Renamed(i) > bestSwap) {
                        bestSwap = swapOffs_.get_Renamed(i);
                        bestIdx = i;
                    }
                }

                --nCaptures_;
                const int move = captures_.get_Renamed(bestIdx);
                captures_.swap(bestIdx, nCaptures_);
                swapOffs_.swap(bestIdx, nCaptures_);

                if (move != hashMove_) {
                    phase_ = LOOSING_CAPTURES;
                    return move;
                }
            }
            phase_ = GENERATE;
            break;

        case GENERATE: {
            moves_.setSize(0);
            auto all = board_.getMask(board_.getWhiteToMove());
            while (!all.IsEmpty()) {
                const int square = all.findFirstOne();
                all.ClearBit(square);
                board_.generateFrom(square, moves_);
            }
            idx_ = moves_.size();
            phase_ = REST;
            break;
        }

        case REST:
            while (idx_ > 0) {
                const int move = history_.select(moves_, idx_);
                --idx_;
                if (move != hashMove_ && move != killer1_ && move != killer2_ && move != killer3_) {
                    return move;
                }
            }
            return -1;
        }
    }
}

void MoveGenerator2::reset() {
    phase_ = HASHMOVE;
    hashMove_ = 0;
    killer3_ = 0;
    inCheck_ = board_.getInCheck();
}

void MoveGenerator2::failHigh(int move, int) {
    if ((move & (Move::CAPTURE | Move::ENPASSANT)) == 0) {
        if (killer1_ == 0) {
            killer1_ = move;
            killer1cnt_ = 1;
        } else if (move == killer1_) {
            ++killer1cnt_;
        } else if (move == killer2_) {
            ++killer2cnt_;
            if (killer1cnt_ > 1) {
                --killer1cnt_;
            }
            if (killer2cnt_ > killer1cnt_) {
                std::swap(killer1_, killer2_);
                std::swap(killer1cnt_, killer2cnt_);
            }
        } else {
            killer2_ = move;
            killer2cnt_ = 1;
            if (killer1cnt_ > 1) {
                --killer1cnt_;
            }
        }
    }
}

} // namespace tgreiner::amy::chess::engine
