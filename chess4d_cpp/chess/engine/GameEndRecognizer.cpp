#include "chess/engine/GameEndRecognizer.h"

#include "chess/engine/ChessBoard.h"

namespace tgreiner::amy::chess::engine {

const std::string& GameEndRecognizer::getResult() const
{
    return result_;
}

const std::string& GameEndRecognizer::getComment() const
{
    return comment_;
}

bool GameEndRecognizer::isGameEnded(ChessBoard& board)
{
    legalMoves_.setSize(0);
    board.generateLegalMoves(legalMoves_);
    if (legalMoves_.size() == 0) {
        if (board.getInCheck()) {
            if (board.getWhiteToMove()) {
                result_ = BLACK_WINS;
                comment_ = "Black mates";
            } else {
                result_ = WHITE_WINS;
                comment_ = "White mates";
            }
        } else {
            result_ = DRAW;
            comment_ = "Stalemate";
        }
        return true;
    }

    if (board.getDrawByRepetition()) {
        result_ = DRAW;
        comment_ = "Draw by repetition";
        return true;
    }

    if (board.getFiftyMoveRuleDraw()) {
        result_ = DRAW;
        comment_ = "Draw by fifty move rule";
        return true;
    }

    if (board.getInsufficientMaterial()) {
        result_ = DRAW;
        comment_ = "Insufficient material";
        return true;
    }

    return false;
}

} // namespace tgreiner::amy::chess::engine
