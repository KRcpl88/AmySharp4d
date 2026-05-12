#include "chess/engine/Swapper.h"

#include "chess/engine/ChessBoard.h"
#include "chess/engine/Geometry.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;

BitBoard Swapper::swapReRay(ChessBoard& board, const BitBoard& atks, int from, int to) {
    BitBoard result;
    if (atks.GetBit(from) != 0) {
        result.SetBit(from);
    }
    result = result | ((Geometry::RAY[to][from] & board.getSlidingPieces()) & board.getAttackFrom(from));
    return result;
}

int Swapper::swap(ChessBoard& board, int move) {
    const int from = Move::getFrom(move);
    const int to = Move::getTo(move);
    int swapval = PIECE_VALUES[board.getPieceAt(from)];
    swaplist_[0] = PIECE_VALUES[board.getPieceAt(to)];

    BitBoard atks = board.getAttackFrom(to);
    int swapcnt = 0;
    bool swapwtm = board.getSideAt(from) != Player::black;
    int swapsign = -1;

    atks = swapReRay(board, atks, from, to);

    while (!atks.IsEmpty()) {
        BitBoard tmp;
        int square = -1;

        tmp = atks & board.getMask(swapwtm, ChessConstants_Fields::PAWN);
        if (!tmp.IsEmpty()) {
            square = tmp.findFirstOne();
        } else {
            tmp = atks & (board.getMask(swapwtm, ChessConstants_Fields::KNIGHT) | board.getMask(swapwtm, ChessConstants_Fields::BISHOP));
            if (!tmp.IsEmpty()) {
                square = tmp.findFirstOne();
            } else {
                tmp = atks & board.getMask(swapwtm, ChessConstants_Fields::ROOK);
                if (!tmp.IsEmpty()) {
                    square = tmp.findFirstOne();
                } else {
                    tmp = atks & board.getMask(swapwtm, ChessConstants_Fields::QUEEN);
                    if (!tmp.IsEmpty()) {
                        square = tmp.findFirstOne();
                    } else {
                        tmp = atks & board.getMask(swapwtm, ChessConstants_Fields::KING);
                        if (!tmp.IsEmpty()) {
                            square = tmp.findFirstOne();
                        } else {
                            break;
                        }
                    }
                }
            }
        }

        ++swapcnt;
        swaplist_[swapcnt] = swaplist_[swapcnt - 1] + swapsign * swapval;
        swapval = PIECE_VALUES[board.getPieceAt(square)];
        swapsign = -swapsign;
        swapwtm = !swapwtm;
        atks = swapReRay(board, atks, square, to);
    }

    swapsign = ((swapcnt & 1) != 0) ? -1 : 1;
    while (swapcnt != 0) {
        if (swapsign < 0) {
            if (swaplist_[swapcnt] <= swaplist_[swapcnt - 1]) {
                swaplist_[swapcnt - 1] = swaplist_[swapcnt];
            }
        } else {
            if (swaplist_[swapcnt] >= swaplist_[swapcnt - 1]) {
                swaplist_[swapcnt - 1] = swaplist_[swapcnt];
            }
        }
        --swapcnt;
        swapsign = -swapsign;
    }

    return swaplist_[0];
}

} // namespace tgreiner::amy::chess::engine
