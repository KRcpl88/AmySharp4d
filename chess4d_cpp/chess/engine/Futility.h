#pragma once

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class Futility final {
public:
    Futility() = delete;

    static int estimateMove(ChessBoard& board, int move);
    static bool isFutile(ChessBoard& board, int move, int alpha);
    static bool isFutile2(ChessBoard& board, int move, int alpha);
};

} // namespace tgreiner::amy::chess::engine
