/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/Move.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "bitboard/BoardConstants.h"
#include "bitboard/LevelRankFile.h"
#include "common/engine/IntVector.h"

#include <stdexcept>
#include <string>
#include <sstream>

using namespace tgreiner::amy::bitboard;
using namespace tgreiner::amy::common::engine;

namespace tgreiner::amy::chess::engine {

const std::string Move::KSIDE_CASTLE_SAN = "O-O";
const std::string Move::QSIDE_CASTLE_SAN = "O-O-O";

// ---------------------------------------------------------------------------
// Accessors
// ---------------------------------------------------------------------------
int Move::getFrom(int move)  { return move & MOVE_SQUARE_MASK; }
int Move::getTo  (int move)  { return (move >> SHIFT_TO) & MOVE_SQUARE_MASK; }
int Move::makeMove(int from, int to) { return from | (to << SHIFT_TO); }

bool Move::isPromotion    (int move) { return (move & PROMOTION)    != 0; }
bool Move::isCastle       (int move) { return (move & CASTLE)       != 0; }
bool Move::isKingSideCastle(int move){ return (move & CASTLE_KSIDE) != 0; }

int Move::getPromoPiece(int move)
{
    switch (move & PROMOTION) {
        case PROMO_KNIGHT: return ChessConstants::KNIGHT;
        case PROMO_BISHOP: return ChessConstants::BISHOP;
        case PROMO_ROOK:   return ChessConstants::ROOK;
        case PROMO_QUEEN:  return ChessConstants::QUEEN;
        default: throw std::runtime_error("getPromoPiece: not a promotion");
    }
}

char Move::file(int square) { return static_cast<char>('a' + (square & 7)); }
char Move::rank(int square) { return static_cast<char>('1' + (square >> 3)); }

// ---------------------------------------------------------------------------
// String conversion
// ---------------------------------------------------------------------------
std::string Move::toString(int move)
{
    std::string result;
    int from = getFrom(move);
    int to   = getTo  (move);

    result += file(from);
    result += rank(from);
    result += file(to);
    result += rank(to);

    if (isPromotion(move)) {
        result += ChessBoard::PIECE_NAME[getPromoPiece(move)];
    }
    return result;
}

std::string Move::toSAN(ChessBoard& /*board*/, int move)
{
    return toString(move);
}

std::string Move::toSAN1(ChessBoard& board, int move)
{
    std::string result;
    int from = getFrom(move);
    int to   = getTo  (move);
    int type = board.getPieceAt(from);

    if (type == ChessConstants::PAWN) {
        if ((move & (CAPTURE | ENPASSANT)) != 0) {
            result += file(from);
            result += 'x';
        }
        result += file(to);
        result += rank(to);
        if ((move & PROMOTION) != 0) {
            result += '=';
            result += ChessBoard::PIECE_NAME[getPromoPiece(move)];
        }
    } else if ((move & CASTLE_KSIDE) != 0) {
        result = KSIDE_CASTLE_SAN;
    } else if ((move & CASTLE_QSIDE) != 0) {
        result = QSIDE_CASTLE_SAN;
    } else {
        result += ChessBoard::PIECE_NAME[type];

        IntVector legal;
        board.generateLegalMoves(legal);

        bool ambiguous     = false;
        bool rankAmbiguous = false;
        bool fileAmbiguous = false;

        for (int i = 0; i < legal.size(); ++i) {
            int move2 = legal.get_Renamed(i);
            if (move2 == move) continue;
            int from2 = getFrom(move2);
            int to2   = getTo  (move2);
            if (to2 == to && board.getPieceAt(from2) == type) {
                ambiguous = true;
                if ((from2 & 7) == (from & 7)) fileAmbiguous = true;
                if ((from2 >> 3) == (from >> 3)) rankAmbiguous = true;
            }
        }
        if (ambiguous) {
            if (!fileAmbiguous) {
                result += file(from);
            } else if (!rankAmbiguous) {
                result += rank(from);
            } else {
                result += file(from);
                result += rank(from);
            }
        }
        if ((move & CAPTURE) != 0) result += 'x';
        result += file(to);
        result += rank(to);
    }

    board.doMove(move);
    bool inCheck = board.getInCheck();
    if (inCheck) {
        if (board.getCheckMate()) result += '#';
        else                      result += '+';
    }
    board.undoMove();
    return result;
}

// ---------------------------------------------------------------------------
// parseSAN
// ---------------------------------------------------------------------------
int Move::parseSAN(ChessBoard& board, const std::string& san)
{
    int toRank  = -1, toFile  = -1;
    int fromRank= -1, fromFile= -1;
    int fromLevel = -1, toLevel = -1;
    int type      = 0;
    int promotion = 0;
    int castle    = 0;

    for (size_t i = 0; i < san.size(); ++i) {
        char c = san[i];
        switch (c) {
            case 'a': case 'b': case 'c': case 'd':
            case 'e': case 'f': case 'g': case 'h':
                if (toLevel == -1) {
                    toLevel = c - 'a';
                } else if (toFile >= 0 && fromLevel == -1) {
                    fromLevel = toLevel;
                    toLevel   = c - 'a';
                } else {
                    fromFile = toFile;
                    toFile   = c - 'a';
                }
                break;
            case 'i': case 'j': case 'k': case 'l':
            case 'm': case 'n': case 'o':
                if (fromLevel == -1)      fromLevel = c - 'a';
                else if (toLevel == -1)   toLevel   = c - 'a';
                break;
            case '1': case '2': case '3': case '4':
            case '5': case '6': case '7': case '8':
                fromRank = toRank;
                toRank   = c - '1';
                break;
            case 'N': type = ChessConstants::KNIGHT; break;
            case 'B': type = ChessConstants::BISHOP; break;
            case 'R': type = ChessConstants::ROOK;   break;
            case 'Q': type = ChessConstants::QUEEN;  break;
            case 'K': type = ChessConstants::KING;   break;
            case '=':
                ++i;
                if (i < san.size()) {
                    switch (san[i]) {
                        case 'Q': promotion = PROMO_QUEEN;  break;
                        case 'R': promotion = PROMO_ROOK;   break;
                        case 'B': promotion = PROMO_BISHOP; break;
                        case 'N': promotion = PROMO_KNIGHT; break;
                        default:  throw std::runtime_error("Illegal promotion");
                    }
                }
                break;
            case 'O': case '0': ++castle; break;
            case 'x': case '+': case '#': case '-': break;
            default: throw std::runtime_error(std::string("Illegal character in SAN: ") + c);
        }
    }

    if (castle != 0) {
        if (castle < 2 || castle > 3)
            throw std::runtime_error("Illegal castle notation");
        type = ChessConstants::KING;
    }
    if (type == 0) type = ChessConstants::PAWN;

    IntVector mvs;
    board.generatePseudoLegalMoves(mvs);

    int found = 0;
    for (int idx = 0; idx < mvs.size(); ++idx) {
        int m    = mvs.get_Renamed(idx);
        int from = getFrom(m);
        int to   = getTo(m);

        if (board.getPieceAt(from) != type) continue;

        Lfr fromLfr(from);
        if (fromLevel != -1 && fromLfr.Level != fromLevel) continue;
        if (fromRank  != -1 && fromLfr.Rank  != fromRank)  continue;
        if (fromFile  != -1 && fromLfr.File  != fromFile)  continue;

        Lfr toLfr(to);
        if (toLevel != -1 && toLfr.Level != toLevel) continue;
        if (toRank  != -1 && toLfr.Rank  != toRank)  continue;
        if (toFile  != -1 && toLfr.File  != toFile)  continue;

        if (promotion != 0 && (m & PROMOTION) != promotion) continue;
        if (castle == 2 && !(m & CASTLE_KSIDE)) continue;
        if (castle == 3 && !(m & CASTLE_QSIDE)) continue;
        if ((m & CASTLE) != 0 && !board.isCastleLegal(m)) continue;

        board.doMove(m);
        bool inCheck = board.getOppInCheck();
        board.undoMove();
        if (inCheck) continue;

        if (found == 0) found = m;
        else throw std::runtime_error("Ambiguous move");
    }

    if (found == 0)
        throw std::runtime_error("No matching move for SAN: " + san);
    return found;
}

// ---------------------------------------------------------------------------
// getMove – coordinate-notation parser ("e2e4", "e7e8q")
// ---------------------------------------------------------------------------
int Move::getMove(ChessBoard& board, const std::string& move)
{
    if (move.size() < 4)
        throw std::runtime_error("Move string too short: " + move);

    // Simple 2-D parse (level H only, file 'a'-'h', rank '1'-'8')
    int fromFile = move[0] - 'a';
    int fromRank = move[1] - '1';
    int toFile   = move[2] - 'a';
    int toRank   = move[3] - '1';

    int from = tgreiner::amy::bitboard::BoardConstants::LH + 8 * fromRank + fromFile;
    int to   = tgreiner::amy::bitboard::BoardConstants::LH + 8 * toRank + toFile;

    IntVector movesList;
    board.generateLegalMoves(movesList);

    for (int i = 0; i < movesList.size(); ++i) {
        int m = movesList.get_Renamed(i);
        if (getFrom(m) != from || getTo(m) != to) continue;

        if (move.size() == 4) return m;

        // Promotion character
        int promo = 0;
        switch (move[4]) {
            case 'q': case 'Q': promo = PROMO_QUEEN;  break;
            case 'r': case 'R': promo = PROMO_ROOK;   break;
            case 'b': case 'B': promo = PROMO_BISHOP; break;
            case 'n': case 'N': promo = PROMO_KNIGHT; break;
            default: throw std::runtime_error("Illegal promotion char");
        }
        if ((m & PROMOTION) == promo) return m;
    }
    throw std::runtime_error("Illegal move: " + move);
}

} // namespace tgreiner::amy::chess::engine
