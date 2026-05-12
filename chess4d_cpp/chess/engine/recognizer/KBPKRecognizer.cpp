/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess\engine\recognizer\KBPKRecognizer.h"

#include "chess\engine\ChessBoard.h"
#include "chess\engine\ChessConstants.h"
#include "chess\engine\EvalMasks.h"

namespace tgreiner::amy::chess::engine::recognizer {

int KBPKRecognizer::getValue() const
{
    return 0;
}

int KBPKRecognizer::probe(const tgreiner::amy::chess::engine::ChessBoard& board)
{
    if (board.getMaterialSignature(false) == 0) {
        if (blackKingDefendsH8(board) || blackKingDefendsA8(board)) {
            return EXACT;
        }
    }

    if (board.getMaterialSignature(true) == 0) {
        if (whiteKingDefendsH1(board) || whiteKingDefendsA1(board)) {
            return EXACT;
        }
    }

    return USELESS;
}

bool KBPKRecognizer::blackKingDefendsH8(const tgreiner::amy::chess::engine::ChessBoard& board) const
{
    if (!(board.getMask(true, ChessConstants::BISHOP) & EvalMasks::BLACK_SQUARES).IsEmpty()) {
        return false;
    }

    if (!(board.getMask(true, ChessConstants::PAWN) & ~EvalMasks::FILE_MASK[7]).IsEmpty()) {
        return false;
    }

    const int square = board.getKingPos(false);
    return (square >> 3) >= 6 && (square & 7) >= 6;
}

bool KBPKRecognizer::blackKingDefendsA8(const tgreiner::amy::chess::engine::ChessBoard& board) const
{
    if (!(board.getMask(true, ChessConstants::BISHOP) & EvalMasks::WHITE_SQUARES).IsEmpty()) {
        return false;
    }

    if (!(board.getMask(true, ChessConstants::PAWN) & ~EvalMasks::FILE_MASK[0]).IsEmpty()) {
        return false;
    }

    const int square = board.getKingPos(false);
    return (square >> 3) >= 6 && (square & 7) <= 1;
}

bool KBPKRecognizer::whiteKingDefendsH1(const tgreiner::amy::chess::engine::ChessBoard& board) const
{
    if (!(board.getMask(false, ChessConstants::BISHOP) & EvalMasks::WHITE_SQUARES).IsEmpty()) {
        return false;
    }

    if (!(board.getMask(false, ChessConstants::PAWN) & ~EvalMasks::FILE_MASK[7]).IsEmpty()) {
        return false;
    }

    const int square = board.getKingPos(true);
    return (square >> 3) <= 1 && (square & 7) >= 6;
}

bool KBPKRecognizer::whiteKingDefendsA1(const tgreiner::amy::chess::engine::ChessBoard& board) const
{
    if (!(board.getMask(false, ChessConstants::BISHOP) & EvalMasks::BLACK_SQUARES).IsEmpty()) {
        return false;
    }

    if (!(board.getMask(false, ChessConstants::PAWN) & ~EvalMasks::FILE_MASK[0]).IsEmpty()) {
        return false;
    }

    const int square = board.getKingPos(true);
    return (square >> 3) <= 1 && (square & 7) <= 1;
}

} // namespace tgreiner::amy::chess::engine::recognizer
