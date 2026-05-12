#include "chess/engine/CheckEvasionMoveGenerator.h"

#include <utility>

#include "bitboard/BoardConstants.h"
#include "chess/engine/Geometry.h"
#include "chess/engine/HistoryTable.h"
#include "chess/engine/ITransTable.h"
#include "chess/engine/Move.h"
#include "chess/engine/TTEntry.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BoardConstants::HA7;
using tgreiner::amy::bitboard::BoardConstants::HA8;
using tgreiner::amy::bitboard::BoardConstants::HH1;
using tgreiner::amy::bitboard::BoardConstants::HH2;

CheckEvasionMoveGenerator::CheckEvasionMoveGenerator(ChessBoard& theBoard, ITransTable& theTtable, HistoryTable& theHistory)
    : ttable_(theTtable)
    , history_(theHistory)
    , board_(theBoard) {
    reset();
}

int CheckEvasionMoveGenerator::nextMove() {
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
            const int kingPos = board_.getKingPos(board_.getWhiteToMove());
            auto victims = board_.getMask(!board_.getWhiteToMove()) & (board_.getAttackFrom(kingPos) | board_.getAttackTo(kingPos));
            while (!victims.IsEmpty()) {
                const int square = victims.findFirstOne();
                victims.ClearBit(square);
                board_.generateTo(square, captures_);
            }
            board_.generateEnPassant(captures_);
            nCaptures_ = captures_.size();
            phase_ = CAPTURES;
            break;
        }

        case CAPTURES:
            while (nCaptures_ > 0) {
                --nCaptures_;
                const int move = captures_.get_Renamed(nCaptures_);
                if (move != hashMove_) {
                    phase_ = CAPTURES;
                    return move;
                }
            }
            phase_ = KILLER1;
            break;

        case KILLER1:
            phase_ = KILLER2;
            if (killer1_ != hashMove_ && board_.isPseudoLegalMove(killer1_)) {
                return killer1_;
            }
            break;

        case KILLER2:
            phase_ = GENERATE;
            if (killer2_ != hashMove_ && board_.isPseudoLegalMove(killer2_)) {
                return killer2_;
            }
            break;

        case GENERATE:
            moves_.setSize(0);
            generateEvasions(moves_);
            idx_ = moves_.size();
            phase_ = REST;
            break;

        case REST:
            while (idx_ > 0) {
                const int move = history_.select(moves_, idx_);
                --idx_;
                if (move != hashMove_ && move != killer1_ && move != killer2_) {
                    return move;
                }
            }
            return -1;
        }
    }
}

void CheckEvasionMoveGenerator::generateEvasions(tgreiner::amy::common::engine::IMoveList& theMoves) {
    const int kingPos = board_.getKingPos(board_.getWhiteToMove());
    const auto oppPieces = board_.getMask(!board_.getWhiteToMove());
    auto kingSqs = board_.getAttackTo(kingPos) & ~(board_.getMask(true) | board_.getMask(false));

    while (!kingSqs.IsEmpty()) {
        const int to = kingSqs.findFirstOne();
        kingSqs.ClearBit(to);
        if ((board_.getAttackFrom(to) & oppPieces).IsEmpty()) {
            theMoves.add(Move::makeMove(kingPos, to));
        }
    }

    const auto attackers = board_.getAttackFrom(kingPos) & oppPieces;
    if (attackers.countBits() == 1) {
        const auto rayAttackers = attackers &
            (board_.getMask(!board_.getWhiteToMove(), ChessConstants_Fields::BISHOP) |
             board_.getMask(!board_.getWhiteToMove(), ChessConstants_Fields::ROOK) |
             board_.getMask(!board_.getWhiteToMove(), ChessConstants_Fields::QUEEN));
        if (!rayAttackers.IsEmpty()) {
            const int attackerPos = rayAttackers.findFirstOne();
            const auto validToSquares = Geometry::INTER_PATH[attackerPos][kingPos];

            const auto defenders = board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::KNIGHT) |
                board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::BISHOP) |
                board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::ROOK) |
                board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::QUEEN);

            auto tmp = validToSquares;
            while (!tmp.IsEmpty()) {
                const int to = tmp.findFirstOne();
                tmp.ClearBit(to);

                auto tmp2 = board_.getAttackFrom(to) & defenders;
                while (!tmp2.IsEmpty()) {
                    const int from = tmp2.findFirstOne();
                    tmp2.ClearBit(from);
                    theMoves.add(Move::makeMove(from, to));
                }
            }

            auto pawns = board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::PAWN);
            while (!pawns.IsEmpty()) {
                const int from = pawns.findFirstOne();
                pawns.ClearBit(from);

                if (board_.getWhiteToMove()) {
                    int to = from + 8;
                    if (board_.getPieceAt(to) != 0) {
                        continue;
                    }
                    if (validToSquares.GetBit(to) != 0) {
                        if (to >= HA8) {
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_QUEEN);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_ROOK);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_BISHOP);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_KNIGHT);
                        } else {
                            theMoves.add(Move::makeMove(from, to));
                        }
                    }
                    if (from <= HH2) {
                        to = from + 16;
                        if (validToSquares.GetBit(to) != 0) {
                            theMoves.add(Move::makeMove(from, to) | Move::PAWN_DOUBLE);
                        }
                    }
                } else {
                    int to = from - 8;
                    if (board_.getPieceAt(to) != 0) {
                        continue;
                    }
                    if (validToSquares.GetBit(to) != 0) {
                        if (to <= HH1) {
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_QUEEN);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_ROOK);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_BISHOP);
                            theMoves.add(Move::makeMove(from, to) | Move::PROMO_KNIGHT);
                        } else {
                            theMoves.add(Move::makeMove(from, to));
                        }
                    }
                    if (from >= HA7) {
                        to = from - 16;
                        if (validToSquares.GetBit(to) != 0) {
                            theMoves.add(Move::makeMove(from, to) | Move::PAWN_DOUBLE);
                        }
                    }
                }
            }
        }
    }
}

void CheckEvasionMoveGenerator::reset() {
    phase_ = HASHMOVE;
    hashMove_ = 0;
}

void CheckEvasionMoveGenerator::failHigh(int move, int) {
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
