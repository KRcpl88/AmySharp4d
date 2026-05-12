#pragma once

#include "chess/engine/ISelectivity.h"
#include "chess/engine/Swapper.h"

namespace tgreiner::amy::chess::engine {

class SelectivityImpl : public ISelectivity {
public:
    int extendPreDoMove(ChessBoard& board, int move) override;
    int extendAfterDoMove(ChessBoard& board) override;
    bool isFutile(ChessBoard& board, int nextDepth, int move, int alpha) override;

private:
    int extendReCapture(ChessBoard& board, int move);
    int extendPassedPawnPush(ChessBoard& board, int move);

    Swapper swapper_;

    static constexpr int EXTEND_RECAPTURE_PAWN = 6;
    static constexpr int EXTEND_RECAPTURE_MINOR = 8;
    static constexpr int EXTEND_RECAPTURE_ROOK = 10;
    static constexpr int EXTEND_RECAPTURE_QUEEN = 12;
    static constexpr int EXTEND_PASSED_PAWN = 16;
    static constexpr int EXTEND_CHECK = 16;
};

} // namespace tgreiner::amy::chess::engine
