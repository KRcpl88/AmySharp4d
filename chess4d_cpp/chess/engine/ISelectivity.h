#pragma once

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class ISelectivity {
public:
    virtual ~ISelectivity() = default;

    virtual int extendPreDoMove(ChessBoard& board, int move) = 0;
    virtual int extendAfterDoMove(ChessBoard& board) = 0;
    virtual bool isFutile(ChessBoard& board, int nextDepth, int move, int alpha) = 0;
};

} // namespace tgreiner::amy::chess::engine
