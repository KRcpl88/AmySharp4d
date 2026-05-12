/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess\engine\recognizer\KBPKPRecognizer.h"

#include "chess\engine\ChessBoard.h"
#include "chess\engine\ChessConstants.h"
#include "chess\engine\EvalMasks.h"

namespace tgreiner::amy::chess::engine::recognizer {

int KBPKPRecognizer::probe(const tgreiner::amy::chess::engine::ChessBoard& board)
{
    if (board.getMaterialSignature(false) == 1) {
        const auto blackPawns = board.getMask(false, ChessConstants::PAWN);
        if ((blackKingDefendsH8(board) && (blackPawns & EvalMasks::FILE_MASK[6]).IsEmpty()) ||
            (blackKingDefendsA8(board) && (blackPawns & EvalMasks::FILE_MASK[1]).IsEmpty())) {
            return board.getWhiteToMove() ? UPPER_BOUND : LOWER_BOUND;
        }
    }

    if (board.getMaterialSignature(true) == 1) {
        const auto whitePawns = board.getMask(true, ChessConstants::PAWN);
        if ((whiteKingDefendsH1(board) && (whitePawns & EvalMasks::FILE_MASK[6]).IsEmpty()) ||
            (whiteKingDefendsA1(board) && (whitePawns & EvalMasks::FILE_MASK[1]).IsEmpty())) {
            return board.getWhiteToMove() ? LOWER_BOUND : UPPER_BOUND;
        }
    }

    return USELESS;
}

} // namespace tgreiner::amy::chess::engine::recognizer
