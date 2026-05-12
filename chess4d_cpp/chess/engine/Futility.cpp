#include "chess/engine/Futility.h"

#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

int Futility::estimateMove(ChessBoard& board, int move)
{
    IEvaluator* eval = board.getEvaluator();
    int matBalance = eval->getWhiteMaterial() - eval->getBlackMaterial();
    if (!board.getWhiteToMove()) {
        matBalance = -matBalance;
    }

    if ((move & Move::ENPASSANT) != 0) {
        matBalance += eval->getMaterialValue(ChessConstants_Fields::PAWN);
    } else if ((move & Move::CAPTURE) != 0) {
        matBalance += eval->getMaterialValue(board.getPieceAt(Move::getTo(move)));
    }

    if ((move & Move::PROMOTION) != 0) {
        matBalance += eval->getMaterialValue(Move::getPromoPiece(move));
    }

    return matBalance;
}

bool Futility::isFutile(ChessBoard& board, int move, int alpha)
{
    IEvaluator* eval = board.getEvaluator();
    const int matBalance = estimateMove(board, move);

    if ((matBalance + 2 * eval->getMaterialValue(ChessConstants_Fields::PAWN)) >= alpha) {
        return false;
    }

    if (board.isCheckingMove(move)) {
        return false;
    }

    return true;
}

bool Futility::isFutile2(ChessBoard& board, int move, int alpha)
{
    IEvaluator* eval = board.getEvaluator();
    const int matBalance = estimateMove(board, move);

    if ((matBalance + eval->getMaterialValue(ChessConstants_Fields::BISHOP)) >= alpha) {
        return false;
    }

    if (board.isCheckingMove(move)) {
        return false;
    }

    return true;
}

} // namespace tgreiner::amy::chess::engine
