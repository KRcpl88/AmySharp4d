#include "chess/engine/MVVLVAGenerator.h"

#include "bitboard/BoardConstants.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BoardConstants::HA8;
using tgreiner::amy::bitboard::BoardConstants::HH1;

MVVLVAGenerator::MVVLVAGenerator(ChessBoard& theBoard)
    : board(theBoard) {
    reset();
}

int MVVLVAGenerator::nextMove() {
    while (victimSq_ != -1) {
        const int attSq = nextAttacker();
        if (attSq == -1) {
            nextVictim();
            attacker_ = 0;
            attackers_.Clear();
            continue;
        }

        int move = Move::makeMove(attSq, victimSq_) | Move::CAPTURE;
        if (board.getPieceAt(attSq) == ChessConstants_Fields::PAWN) {
            if (board.getWhiteToMove() && victimSq_ >= HA8) {
                move |= Move::PROMO_QUEEN;
            }
            if (!board.getWhiteToMove() && victimSq_ <= HH1) {
                move |= Move::PROMO_QUEEN;
            }
        }
        return move;
    }

    return -1;
}

void MVVLVAGenerator::nextVictim() {
    while (victim_ >= ChessConstants_Fields::PAWN) {
        if (victims_.IsEmpty()) {
            --victim_;
            if (victim_ >= ChessConstants_Fields::PAWN) {
                victims_ = board.getMask(!whiteToMove_, victim_);
            }
            continue;
        }

        victimSq_ = victims_.findFirstOne();
        victims_.ClearBit(victimSq_);
        return;
    }

    victimSq_ = -1;
}

int MVVLVAGenerator::nextAttacker() {
    while (attacker_ <= ChessConstants_Fields::KING) {
        if (attackers_.IsEmpty()) {
            ++attacker_;
            if (attacker_ > ChessConstants_Fields::KING) {
                break;
            }
            attackers_ = board.getMask(whiteToMove_, attacker_) & board.getAttackFrom(victimSq_);
            continue;
        }

        const int attackSquare = attackers_.findFirstOne();
        attackers_.ClearBit(attackSquare);
        return attackSquare;
    }

    return -1;
}

void MVVLVAGenerator::reset() {
    whiteToMove_ = board.getWhiteToMove();
    victim_ = ChessConstants_Fields::QUEEN;
    attacker_ = 0;
    victims_ = board.getMask(!whiteToMove_, victim_);
    attackers_.Clear();
    nextVictim();
}

void MVVLVAGenerator::failHigh(int, int) {
}

} // namespace tgreiner::amy::chess::engine
