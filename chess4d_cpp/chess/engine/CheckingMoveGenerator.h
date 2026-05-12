/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include "chess/engine/ChessBoard.h"
#include "common/engine/IMoveList.h"

namespace tgreiner::amy::chess::engine {

class CheckingMoveGenerator {
public:
    explicit CheckingMoveGenerator(ChessBoard& theBoard);

    void generateBRQChecks(tgreiner::amy::common::engine::IMoveList& moves);
    void generateNChecks(tgreiner::amy::common::engine::IMoveList& moves);

private:
    void generateChecks(tgreiner::amy::common::engine::IMoveList& moves,
                        tgreiner::amy::bitboard::BitBoard pcs,
                        tgreiner::amy::bitboard::BitBoard allPieces,
                        tgreiner::amy::bitboard::BitBoard toSquares,
                        int oppKing,
                        bool ignoreInterPath);

    ChessBoard& board_;
};

} // namespace tgreiner::amy::chess::engine
