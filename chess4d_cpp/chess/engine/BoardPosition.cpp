/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/BoardPosition.h"
#include <utility>

namespace tgreiner::amy::chess::engine {

BoardPosition::BoardPosition(std::vector<int> board,
                             bool   whiteToMove,
                             int    enPassant,
                             bool   canWhiteCastleKingSide,
                             bool   canWhiteCastleQueenSide,
                             bool   canBlackCastleKingSide,
                             bool   canBlackCastleQueenSide)
    : board_(std::move(board))
    , whiteToMove_(whiteToMove)
    , enPassant_(enPassant)
    , canWhiteCastleKingSide_(canWhiteCastleKingSide)
    , canWhiteCastleQueenSide_(canWhiteCastleQueenSide)
    , canBlackCastleKingSide_(canBlackCastleKingSide)
    , canBlackCastleQueenSide_(canBlackCastleQueenSide)
{}

} // namespace tgreiner::amy::chess::engine
