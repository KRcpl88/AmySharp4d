/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <vector>
#include "chess/engine/Position.h"

namespace tgreiner::amy::chess::engine {

/// Concrete IPosition built from explicit parameters (used by EPD parser etc.).
class BoardPosition final : public IPosition {
public:
    BoardPosition(std::vector<int> board,
                  bool   whiteToMove,
                  int    enPassant,
                  bool   canWhiteCastleKingSide,
                  bool   canWhiteCastleQueenSide,
                  bool   canBlackCastleKingSide,
                  bool   canBlackCastleQueenSide);

    const std::vector<int>& getBoard()           const override { return board_; }
    bool getWtm()                                const override { return whiteToMove_; }
    int  getEnPassantSquare()                    const override { return enPassant_; }
    bool canWhiteCastleKingSide()                const override { return canWhiteCastleKingSide_; }
    bool canWhiteCastleQueenSide()               const override { return canWhiteCastleQueenSide_; }
    bool canBlackCastleKingSide()                const override { return canBlackCastleKingSide_; }
    bool canBlackCastleQueenSide()               const override { return canBlackCastleQueenSide_; }

private:
    std::vector<int> board_;
    bool whiteToMove_;
    int  enPassant_;
    bool canWhiteCastleKingSide_;
    bool canWhiteCastleQueenSide_;
    bool canBlackCastleKingSide_;
    bool canBlackCastleQueenSide_;
};

} // namespace tgreiner::amy::chess::engine
