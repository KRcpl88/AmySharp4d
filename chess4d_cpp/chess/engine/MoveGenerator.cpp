#include "chess/engine/MoveGenerator.h"

#include <utility>

#include "chess/engine/HistoryTable.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

MoveGenerator::MoveGenerator(ChessBoard& theBoard, ITransTable& theTransTable, HistoryTable& theHistory)
    : MVVLVAGenerator(theBoard)
    , ttable_(theTransTable)
    , history_(theHistory) {
    reset();
}

int MoveGenerator::nextMove() {
    for (;;) {
        switch (phase_) {
        case HASHMOVE: {
            phase_ = CAPTURES;
            TTEntry* entry = ttable_.get_Renamed(board.getPosHash());
            if (entry != nullptr && board.isPseudoLegalMove(entry->move)) {
                hashmove_ = entry->move;
                return hashmove_;
            }
            break;
        }

        case CAPTURES: {
            int move;
            do {
                move = MVVLVAGenerator::nextMove();
            } while (move == hashmove_);
            if (move != -1) {
                return move;
            }
            phase_ = KILLER1;
            break;
        }

        case KILLER1:
            phase_ = KILLER2;
            if (board.isPseudoLegalMove(killer1_)) {
                return killer1_;
            }
            break;

        case KILLER2:
            phase_ = GENERATE;
            if (board.isPseudoLegalMove(killer2_)) {
                return killer2_;
            }
            break;

        case GENERATE: {
            moves_.setSize(0);
            auto all = board.getMask(board.getWhiteToMove());
            while (!all.IsEmpty()) {
                const int square = all.findFirstOne();
                all.ClearBit(square);
                board.generateFrom(square, moves_);
            }
            idx_ = moves_.size();
            phase_ = REST;
            break;
        }

        case REST:
            while (idx_ > 0) {
                const int move = history_.select(moves_, idx_);
                --idx_;
                if (move != hashmove_ && move != killer1_ && move != killer2_) {
                    return move;
                }
            }
            return -1;
        }
    }
}

void MoveGenerator::reset() {
    MVVLVAGenerator::reset();
    phase_ = HASHMOVE;
    hashmove_ = 0;
}

void MoveGenerator::failHigh(int move, int) {
    if ((move & (Move::CAPTURE | Move::ENPASSANT)) == 0) {
        if (killer1_ == 0) {
            killer1_ = move;
            killer1cnt_ = 1;
        } else if (move == killer1_) {
            ++killer1cnt_;
        } else if (move == killer2_) {
            ++killer2cnt_;
            if (killer2cnt_ > killer1cnt_) {
                std::swap(killer1_, killer2_);
                std::swap(killer1cnt_, killer2cnt_);
            }
        } else {
            killer2_ = move;
            killer2cnt_ = 1;
        }
    }
}

} // namespace tgreiner::amy::chess::engine
