#include "chess/engine/SelectivityImpl.h"

#include "bitboard/BoardConstants.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/Futility.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

using namespace tgreiner::amy::bitboard;

int SelectivityImpl::extendPreDoMove(ChessBoard& board, int move)
{
    return extendReCapture(board, move) + extendPassedPawnPush(board, move);
}

int SelectivityImpl::extendAfterDoMove(ChessBoard& board)
{
    if (board.getInCheck()) {
        return EXTEND_CHECK;
    }
    return 0;
}

int SelectivityImpl::extendReCapture(ChessBoard& board, int move)
{
    if ((move & Move::CAPTURE) != 0) {
        const int lmove = board.getLastMove();
        if ((lmove & Move::CAPTURE) != 0 && Move::getTo(move) == Move::getTo(lmove)) {
            const int captured = board.getPieceAt(Move::getTo(move));
            switch (board.getLastCaptured()) {
            case ChessConstants_Fields::PAWN:
                if (captured == ChessConstants_Fields::PAWN) {
                    return EXTEND_RECAPTURE_PAWN;
                }
                break;
            case ChessConstants_Fields::KNIGHT:
            case ChessConstants_Fields::BISHOP:
                if (captured == ChessConstants_Fields::KNIGHT || captured == ChessConstants_Fields::BISHOP) {
                    return EXTEND_RECAPTURE_MINOR;
                }
                break;
            case ChessConstants_Fields::ROOK:
                if (captured == ChessConstants_Fields::ROOK) {
                    return EXTEND_RECAPTURE_ROOK;
                }
                break;
            case ChessConstants_Fields::QUEEN:
                if (captured == ChessConstants_Fields::QUEEN) {
                    return EXTEND_RECAPTURE_QUEEN;
                }
                break;
            default:
                break;
            }
        }
    }
    return 0;
}

int SelectivityImpl::extendPassedPawnPush(ChessBoard& board, int move)
{
    if (board.getPieceAt(Move::getFrom(move)) == ChessConstants_Fields::PAWN) {
        const int to = Move::getTo(move);
        if ((board.getWhiteToMove() && to >= BoardConstants::HA7 && to <= BoardConstants::HH7) ||
            (!board.getWhiteToMove() && to >= BoardConstants::HA2 && to <= BoardConstants::HH2)) {
            if (swapper_.swap(board, move) >= 0) {
                return EXTEND_PASSED_PAWN;
            }
        }
    }
    return 0;
}

bool SelectivityImpl::isFutile(ChessBoard& board, int nextDepth, int move, int alpha)
{
    constexpr int DEPTH_STEP = 16;

    if (nextDepth <= 0 && Futility::isFutile(board, move, alpha)) {
        return true;
    }
    if (nextDepth <= DEPTH_STEP && nextDepth > 0 && Futility::isFutile2(board, move, alpha)) {
        return true;
    }

    return false;
}

} // namespace tgreiner::amy::chess::engine
