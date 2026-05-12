/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#include "chess/engine/ChessBoard.h"
#include "chess/engine/BoardPosition.h"
#include "chess/engine/Geometry.h"
#include "chess/engine/Hashing.h"
#include "chess/engine/Move.h"
#include "bitboard/BitBoard.h"
#include "bitboard/BoardConstants.h"
#include "bitboard/LevelRankFile.h"
#include "bitboard/HexLevelRankFile.h"
#include "common/engine/IntVector.h"

#include <cassert>
#include <cctype>
#include <cmath>
#include <sstream>
#include <stdexcept>

using namespace tgreiner::amy::bitboard;
using namespace tgreiner::amy::common::engine;

namespace tgreiner::amy::chess::engine {

// ===========================================================================
// Static member definitions
// ===========================================================================
const std::array<char, 7> ChessBoard::PIECE_NAME = {' ','P','N','B','R','Q','K'};

const bool ChessBoard::IS_SLIDING[7] = {
    false,  // 0 – unused
    false,  // 1 – PAWN
    false,  // 2 – KNIGHT
    true,   // 3 – BISHOP
    true,   // 4 – ROOK
    true,   // 5 – QUEEN
    false   // 6 – KING
};

const short ChessBoard::MATERIAL_SIGNATURE_BITS[ChessConstants::LAST_PIECE] = {
    0,     // 0 – unused
    0x01,  // PAWN
    0x02,  // KNIGHT
    0x04,  // BISHOP
    0x08,  // ROOK
    0x10,  // QUEEN
    0x00   // KING – not counted
};

// ---------------------------------------------------------------------------
// EN_PASSANT_TRANSLATE – only level H (index 7) is populated for rank 3↔4
// and rank 5↔6.  All other entries are 0 (unused).
// ---------------------------------------------------------------------------
// The C# array has SIZE=344 entries.  We reproduce the same layout:
// squares on level H at ranks 3 (HA4..HH4) map to rank 2 (HA3..HH3), and
// rank 4 (HA5..HH5) maps to rank 5 (HA6..HH6).  (En passant square is the
// square "behind" the double-pushed pawn.)
static int buildEnPassantTranslate()
{
    return 0; // helper – the real array is a compile-time literal below
}

// Full translation table (SIZE entries).
// Only the level-H double-pawn-step target squares are non-zero.
// rank 3 (white double-push lands here) → rank 2 (the passed-through square)
// rank 5 (black double-push lands here) → rank 6 (the passed-through square)
const int ChessBoard::EN_PASSANT_TRANSLATE[BitBoard::SIZE] = {
    // All levels other than H are 0.  Level H (offset 140) has 64 entries
    // indexed file*8+rank inside the level.  We fill only the ones we need.
    // Rather than repeat 344 values inline, we use a generated layout:
    // entries 0..139: 0 (levels A-G)
    // entries 140..203: level H (8×8)
    //   rank index within level = offset - 140
    //   file = (offset-140) % 8 ,  rank = (offset-140) / 8
    //   white double-push: from rank 1 lands at rank 3 → EP sq = rank 2
    //   black double-push: from rank 6 lands at rank 4 → EP sq = rank 5
    // entries 204..343: 0 (levels I-O)

    // Levels A-G (0..139) – all 0
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,

    // Level H (140..203): local index = rank*8 + file  (rank 0-indexed)
    //
    // The array is used in TWO directions:
    //   1. After a double pawn push: translate landing square → ep-capture square
    //      white lands at rank 3 (0-indexed) → ep sq = rank 2
    //      black lands at rank 4 (0-indexed) → ep sq = rank 5
    //   2. During an en passant capture: translate ep square → captured pawn square
    //      white captures TO rank 2 (0-indexed) → captured pawn = rank 3
    //      black captures TO rank 5 (0-indexed) → captured pawn = rank 4
    // All four active ranks must therefore be populated.

    // local index 0..7   = rank 0 → 0
    0,0,0,0,0,0,0,0,
    // local index 8..15  = rank 1 → 0
    0,0,0,0,0,0,0,0,
    // local index 16..23 = rank 2 (ep capture square for black capturing white pawn)
    // captured white pawn is at rank 3: LH + 24..31
    BoardConstants::LH+24, BoardConstants::LH+25, BoardConstants::LH+26, BoardConstants::LH+27,
    BoardConstants::LH+28, BoardConstants::LH+29, BoardConstants::LH+30, BoardConstants::LH+31,
    // local index 24..31 = rank 3 (white double-push landing square)
    // ep square = rank 2: LH + 16..23
    BoardConstants::LH+16, BoardConstants::LH+17, BoardConstants::LH+18, BoardConstants::LH+19,
    BoardConstants::LH+20, BoardConstants::LH+21, BoardConstants::LH+22, BoardConstants::LH+23,
    // local index 32..39 = rank 4 (black double-push landing square)
    // ep square = rank 5: LH + 40..47
    BoardConstants::LH+40, BoardConstants::LH+41, BoardConstants::LH+42, BoardConstants::LH+43,
    BoardConstants::LH+44, BoardConstants::LH+45, BoardConstants::LH+46, BoardConstants::LH+47,
    // local index 40..47 = rank 5 (ep capture square for white capturing black pawn)
    // captured black pawn is at rank 4: LH + 32..39
    BoardConstants::LH+32, BoardConstants::LH+33, BoardConstants::LH+34, BoardConstants::LH+35,
    BoardConstants::LH+36, BoardConstants::LH+37, BoardConstants::LH+38, BoardConstants::LH+39,
    // local index 48..55 = rank 6 → 0
    0,0,0,0,0,0,0,0,
    // local index 56..63 = rank 7 → 0
    0,0,0,0,0,0,0,0,

    // Levels I-O (204..343) – all 0
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
    0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,
};

// ===========================================================================
// Construction helpers
// ===========================================================================
static void initBoardArrays(
    std::vector<BitBoard>& attackTo,
    std::vector<BitBoard>& attackFrom,
    BitBoard pieceMask[][ChessConstants::LAST_PIECE])
{
    attackTo   = BitBoard::CreateArray(BitBoard::SIZE);
    attackFrom = BitBoard::CreateArray(BitBoard::SIZE);
    for (int s = 0; s < 2; ++s) {
        for (int p = 0; p < ChessConstants::LAST_PIECE; ++p) {
            pieceMask[s][p] = BitBoard();
        }
    }
}

// ===========================================================================
// Constructors
// ===========================================================================
ChessBoard::ChessBoard()
{
    InitialPosition ip;
    board_.assign(BitBoard::SIZE, 0);
    initBoardArrays(attackTo_, attackFrom_, pieceMask_);
    setPosition(ip);
}

ChessBoard::ChessBoard(const std::string& epd)
    : ChessBoard()
{
    // Minimal EPD parser (FEN-like: piece placement / side / castling / ep)
    // Format: <pieces> <side> <castling> <ep> [halfmove] [fullmove]
    // We re-use the full parser via a helper.
    board_.assign(BitBoard::SIZE, 0);
    initBoardArrays(attackTo_, attackFrom_, pieceMask_);

    // Parse using helper function defined later in this file.
    // (Implemented inline here to avoid needing EpdParser.h)
    auto parsedPos = parseEpd(epd);
    setPosition(*parsedPos);
}

ChessBoard::ChessBoard(const IPosition& pos)
{
    board_.assign(BitBoard::SIZE, 0);
    initBoardArrays(attackTo_, attackFrom_, pieceMask_);
    setPosition(pos);
}

ChessBoard::ChessBoard(const ChessBoard& other)
{
    board_.assign(BitBoard::SIZE, 0);
    initBoardArrays(attackTo_, attackFrom_, pieceMask_);

    for (int i = 0; i < BitBoard::SIZE; ++i) {
        attackTo_  [i] = other.attackTo_  [i];
        attackFrom_[i] = other.attackFrom_[i];
        board_     [i] = other.board_     [i];
    }
    for (int s = 0; s < 2; ++s) {
        for (int p = 0; p < ChessConstants::LAST_PIECE; ++p) {
            pieceMask_[s][p] = other.pieceMask_[s][p];
        }
        kingPos_[s] = other.kingPos_[s];
    }
    slidingPieces_    = other.slidingPieces_;
    enPassant_        = other.enPassant_;
    castle_           = other.castle_;
    whiteToMove_      = other.whiteToMove_;
    player_           = other.player_;
    positionHash_     = other.positionHash_;
    pawnHash_         = other.pawnHash_;
    materialSignature_[0] = other.materialSignature_[0];
    materialSignature_[1] = other.materialSignature_[1];
    evaluator_        = other.evaluator_;
    history_          = other.history_;
}

// ===========================================================================
// setPosition / getPosition
// ===========================================================================
void ChessBoard::setPosition(const IPosition& pos)
{
    const auto& src = pos.getBoard();
    board_.assign(src.begin(), src.end());

    whiteToMove_ = pos.getWtm();
    castle_      = 0;

    if (pos.canWhiteCastleKingSide())  castle_ |= WHITE_CASTLE_KINGSIDE;
    if (pos.canWhiteCastleQueenSide()) castle_ |= WHITE_CASTLE_QUEENSIDE;
    if (pos.canBlackCastleKingSide())  castle_ |= BLACK_CASTLE_KINGSIDE;
    if (pos.canBlackCastleQueenSide()) castle_ |= BLACK_CASTLE_QUEENSIDE;

    enPassant_ = pos.getEnPassantSquare();

    recalcAttacks();
}

std::unique_ptr<IPosition> ChessBoard::getPosition() const
{
    return std::make_unique<BoardPosition>(
        board_, whiteToMove_, enPassant_,
        canWhiteCastleKingSide(),  canWhiteCastleQueenSide(),
        canBlackCastleKingSide(),  canBlackCastleQueenSide());
}

void ChessBoard::setEvaluator(IEvaluator* ev)
{
    evaluator_ = ev;
    if (ev) ev->init();
}

// ===========================================================================
// Properties
// ===========================================================================
bool ChessBoard::getInCheck() const
{
    if (whiteToMove_) {
        return !(attackFrom_[kingPos_[0]] & pieceMask_[1][0]).IsEmpty();
    } else {
        return !(attackFrom_[kingPos_[1]] & pieceMask_[0][0]).IsEmpty();
    }
}

bool ChessBoard::getOppInCheck() const
{
    if (!whiteToMove_) {
        return !(attackFrom_[kingPos_[0]] & pieceMask_[1][0]).IsEmpty();
    } else {
        return !(attackFrom_[kingPos_[1]] & pieceMask_[0][0]).IsEmpty();
    }
}

bool ChessBoard::getCheckMate() const
{
    if (!getInCheck()) return false;
    IntVector tmp;
    const_cast<ChessBoard*>(this)->generateLegalMoves(tmp);
    return tmp.size() == 0;
}

bool ChessBoard::getInsufficientMaterial() const
{
    if ((materialSignature_[0] & MAJOR_PIECES_OR_PAWNS) != 0 ||
        (materialSignature_[1] & MAJOR_PIECES_OR_PAWNS) != 0) {
        return false;
    }
    BitBoard minors =
        pieceMask_[0][ChessConstants::BISHOP] |
        pieceMask_[1][ChessConstants::BISHOP] |
        pieceMask_[0][ChessConstants::KNIGHT] |
        pieceMask_[1][ChessConstants::KNIGHT];
    return minors.countBits() < 2;
}

bool ChessBoard::getFiftyMoveRuleDraw() const
{
    int bound = player_ - 100;
    if (bound < 0) return false;
    for (int p = player_ - 1; p >= bound; --p) {
        const History& h = history_[p];
        if ((h.move & (Move::CAPTURE | Move::CASTLE | Move::PROMOTION)) != 0 ||
            h.pawnHash != pawnHash_) {
            return false;
        }
    }
    return true;
}

int ChessBoard::getLastMove() const
{
    return history_[player_ - 1].move;
}

int ChessBoard::getLastCaptured() const
{
    return history_[player_ - 1].capturedPiece;
}

BitBoard ChessBoard::getMaskNonPawn() const
{
    return pieceMask_[0][ChessConstants::KNIGHT] | pieceMask_[1][ChessConstants::KNIGHT] |
           pieceMask_[0][ChessConstants::BISHOP] | pieceMask_[1][ChessConstants::BISHOP] |
           pieceMask_[0][ChessConstants::ROOK]   | pieceMask_[1][ChessConstants::ROOK]   |
           pieceMask_[0][ChessConstants::QUEEN]  | pieceMask_[1][ChessConstants::QUEEN];
}

// ===========================================================================
// Attack maintenance helpers
// ===========================================================================
void ChessBoard::setReciprocalAttacks(int from, int to)
{
    attackTo_  [from].SetBit(to);
    attackFrom_[to]  .SetBit(from);
}

void ChessBoard::attackSet(int type, bool theWtm, int square)
{
    const short* nd;
    const short* np;
    int nsq;

    if (type == ChessConstants::PAWN) {
        nd = theWtm
            ? Geometry::NEXT_DIR[Geometry::WHITE_PAWN][square].data()
            : Geometry::NEXT_DIR[Geometry::BLACK_PAWN][square].data();

        nsq = nd[square];
        if (nsq >= 0) {
            setReciprocalAttacks(square, nsq);
            nsq = nd[nsq];
            if (nsq >= 0) setReciprocalAttacks(square, nsq);
        }
    } else {
        np = Geometry::NEXT_POS[type][square].data();
        nd = Geometry::NEXT_DIR[type][square].data();

        nsq = np[square];
        while (nsq >= 0) {
            setReciprocalAttacks(square, nsq);
            if (board_[nsq] != 0) nsq = nd[nsq];
            else                  nsq = np[nsq];
        }
    }
}

void ChessBoard::attackClr(int type, bool theWtm, int square)
{
    attackTo_[square].Clear();

    const short* nd;
    const short* np;
    int nsq;

    if (type == ChessConstants::PAWN) {
        nd = theWtm
            ? Geometry::NEXT_DIR[Geometry::WHITE_PAWN][square].data()
            : Geometry::NEXT_DIR[Geometry::BLACK_PAWN][square].data();

        nsq = nd[square];
        if (nsq >= 0) {
            attackFrom_[nsq].ClearBit(square);
            nsq = nd[nsq];
            if (nsq >= 0) attackFrom_[nsq].ClearBit(square);
        }
    } else {
        np = Geometry::NEXT_POS[type][square].data();
        nd = Geometry::NEXT_DIR[type][square].data();

        nsq = np[square];
        while (nsq >= 0) {
            attackFrom_[nsq].ClearBit(square);
            if (board_[nsq] != 0) nsq = nd[nsq];
            else                  nsq = np[nsq];
        }
    }
}

void ChessBoard::gainAttack(int from, int to)
{
    const short* nsq = Geometry::NEXT_SQ[from].data();
    int square = to;
    for (;;) {
        square = nsq[square];
        if (square < 0) break;
        setReciprocalAttacks(from, square);
        if (board_[square] != 0) break;
    }
}

void ChessBoard::looseAttack(int from, int to)
{
    const short* nsq = Geometry::NEXT_SQ[from].data();
    int square = to;
    for (;;) {
        square = nsq[square];
        if (square < 0) break;
        attackTo_  [from].ClearBit(square);
        attackFrom_[square].ClearBit(from);
        if (board_[square] != 0) break;
    }
}

void ChessBoard::gainAttacks(int to)
{
    BitBoard tmp = attackFrom_[to] & slidingPieces_;
    while (!tmp.IsEmpty()) {
        int i = tmp.findFirstOne();
        tmp.ClearBit(i);
        gainAttack(i, to);
    }
}

void ChessBoard::looseAttacks(int to)
{
    BitBoard tmp = attackFrom_[to] & slidingPieces_;
    while (!tmp.IsEmpty()) {
        int i = tmp.findFirstOne();
        tmp.ClearBit(i);
        looseAttack(i, to);
    }
}

// ===========================================================================
// recalcAttacks
// ===========================================================================
void ChessBoard::recalcAttacks()
{
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        attackTo_  [i] = BitBoard();
        attackFrom_[i] = BitBoard();
    }
    for (int s = 0; s < 2; ++s) {
        for (int p = 0; p <= ChessConstants::KING; ++p) {
            pieceMask_[s][p] = BitBoard();
        }
    }
    slidingPieces_ = BitBoard();
    positionHash_  = 0;
    pawnHash_      = 0;
    materialSignature_[0] = 0;
    materialSignature_[1] = 0;

    for (int sq = 0; sq < BitBoard::SIZE; ++sq) {
        int pc = board_[sq];
        if (pc > 0) {
            int s = 0;
            pieceMask_[s][0].SetBit(sq);
            pieceMask_[s][pc].SetBit(sq);
            materialSignature_[s] |= MATERIAL_SIGNATURE_BITS[pc];
            if (IS_SLIDING[pc]) slidingPieces_.SetBit(sq);
            positionHash_ ^= Hashing::HASH_KEYS[s][pc][sq];
            if (pc == ChessConstants::PAWN)
                pawnHash_ ^= Hashing::HASH_KEYS[s][ChessConstants::PAWN][sq];
            if (pc == ChessConstants::KING)
                kingPos_[s] = sq;
        } else if (pc < 0) {
            int s  = 1;
            int ap = -pc;
            pieceMask_[s][0].SetBit(sq);
            pieceMask_[s][ap].SetBit(sq);
            materialSignature_[s] |= MATERIAL_SIGNATURE_BITS[ap];
            if (IS_SLIDING[ap]) slidingPieces_.SetBit(sq);
            positionHash_ ^= Hashing::HASH_KEYS[s][ap][sq];
            if (ap == ChessConstants::PAWN)
                pawnHash_ ^= Hashing::HASH_KEYS[s][ChessConstants::PAWN][sq];
            if (ap == ChessConstants::KING)
                kingPos_[s] = sq;
        }
    }

    // Set attacks for all white pieces
    {
        BitBoard tmp = pieceMask_[0][0];
        while (!tmp.IsEmpty()) {
            int sq = tmp.findFirstOne();
            tmp.ClearBit(sq);
            attackSet(board_[sq], true, sq);
        }
    }
    // Set attacks for all black pieces
    {
        BitBoard tmp = pieceMask_[1][0];
        while (!tmp.IsEmpty()) {
            int sq = tmp.findFirstOne();
            tmp.ClearBit(sq);
            attackSet(-board_[sq], false, sq);
        }
    }

    // Validate en passant
    if (enPassant_ != 0 &&
        (getMask(whiteToMove_, ChessConstants::PAWN) & attackFrom_[enPassant_]).IsEmpty()) {
        enPassant_ = 0;
    }
    if (enPassant_ != 0)
        positionHash_ ^= Hashing::EN_PASSANT_HASH_KEYS[enPassant_];
    positionHash_ ^= Hashing::CASTLE_HASH_KEYS[castle_];
}

// ===========================================================================
// doMove
// ===========================================================================
void ChessBoard::doMove(int move)
{
    if ((int)history_.size() <= player_) history_.emplace_back();
    History& hist = history_[player_];

    hist.enPassant     = enPassant_;
    hist.castle        = castle_;
    hist.move          = move;
    hist.posHash       = positionHash_;
    hist.pawnHash      = pawnHash_;
    hist.capturedPiece = 0;

    int from = Move::getFrom(move);
    int to   = Move::getTo  (move);
    int type = getPieceAt(from);
    int s    = side(whiteToMove_);

    // Remove piece from 'from'
    attackClr(type, whiteToMove_, from);
    board_[from] = 0;
    pieceMask_[s][0]   .ClearBit(from);
    pieceMask_[s][type].ClearBit(from);
    if (IS_SLIDING[type]) slidingPieces_.ClearBit(from);
    gainAttacks(from);

    positionHash_ ^= Hashing::HASH_KEYS[s][type][from];
    if (type == ChessConstants::PAWN)
        pawnHash_ ^= Hashing::HASH_KEYS[s][ChessConstants::PAWN][from];

    if (move & Move::CAPTURE) {
        // ---- Capture ----
        int captured = getPieceAt(to);
        if (captured == ChessConstants::KING)
            throw std::runtime_error("Captured a king!");

        attackClr(captured, !whiteToMove_, to);
        hist.capturedPiece = captured;
        pieceMask_[1^s][0]       .ClearBit(to);
        pieceMask_[1^s][captured].ClearBit(to);
        if (IS_SLIDING[captured]) slidingPieces_.ClearBit(to);

        // Update castling if rook captured
        if (whiteToMove_) {
            if (to == BoardConstants::HH8) castle_ &= ~BLACK_CASTLE_KINGSIDE;
            if (to == BoardConstants::HA8) castle_ &= ~BLACK_CASTLE_QUEENSIDE;
        } else {
            if (to == BoardConstants::HH1) castle_ &= ~WHITE_CASTLE_KINGSIDE;
            if (to == BoardConstants::HA1) castle_ &= ~WHITE_CASTLE_QUEENSIDE;
        }

        positionHash_ ^= Hashing::HASH_KEYS[1^s][captured][to];
        if (captured == ChessConstants::PAWN)
            pawnHash_ ^= Hashing::HASH_KEYS[1^s][ChessConstants::PAWN][to];

        if (evaluator_) {
            evaluator_->move(from, to, type, whiteToMove_);
            evaluator_->capture(to, captured, !whiteToMove_);
        }
        if (pieceMask_[1^s][captured].IsEmpty())
            materialSignature_[1^s] &= ~MATERIAL_SIGNATURE_BITS[captured];

    } else if (move & Move::ENPASSANT) {
        // ---- En passant ----
        int epto = EN_PASSANT_TRANSLATE[to];
        attackClr(ChessConstants::PAWN, !whiteToMove_, epto);
        hist.capturedPiece = ChessConstants::PAWN;
        pieceMask_[1^s][0]                 .ClearBit(epto);
        pieceMask_[1^s][ChessConstants::PAWN].ClearBit(epto);
        board_[epto] = 0;
        gainAttacks(epto);
        looseAttacks(to);

        positionHash_ ^= Hashing::HASH_KEYS[1^s][ChessConstants::PAWN][epto];
        pawnHash_     ^= Hashing::HASH_KEYS[1^s][ChessConstants::PAWN][epto];

        if (evaluator_) {
            evaluator_->move(from, to, ChessConstants::PAWN, whiteToMove_);
            evaluator_->capture(epto, ChessConstants::PAWN, !whiteToMove_);
        }
        if (pieceMask_[1^s][ChessConstants::PAWN].IsEmpty())
            materialSignature_[1^s] &= ~MATERIAL_SIGNATURE_BITS[ChessConstants::PAWN];

    } else if (Move::isCastle(move)) {
        // ---- Castle ----
        int rookFrom, rookTo;
        if (Move::isKingSideCastle(move)) {
            rookFrom = from + 3;
            rookTo   = from + 1;
        } else {
            rookFrom = from - 4;
            rookTo   = from - 1;
        }

        attackClr(ChessConstants::ROOK, whiteToMove_, rookFrom);
        board_[rookTo] = board_[rookFrom];
        board_[rookFrom] = 0;

        pieceMask_[s][0]                .ClearBit(rookFrom);
        pieceMask_[s][ChessConstants::ROOK].ClearBit(rookFrom);
        slidingPieces_.ClearBit(rookFrom);

        pieceMask_[s][0]                .SetBit(rookTo);
        pieceMask_[s][ChessConstants::ROOK].SetBit(rookTo);
        slidingPieces_.SetBit(rookTo);

        attackSet(ChessConstants::ROOK, whiteToMove_, rookTo);
        looseAttacks(rookTo);
        looseAttacks(to);

        if (whiteToMove_) castle_ &= ~(WHITE_CASTLE_QUEENSIDE | WHITE_CASTLE_KINGSIDE);
        else              castle_ &= ~(BLACK_CASTLE_QUEENSIDE | BLACK_CASTLE_KINGSIDE);

        positionHash_ ^= (Hashing::HASH_KEYS[s][ChessConstants::ROOK][rookFrom] ^
                          Hashing::HASH_KEYS[s][ChessConstants::ROOK][rookTo]);

        if (evaluator_) {
            evaluator_->move(from, to, ChessConstants::KING, whiteToMove_);
            evaluator_->move(rookFrom, rookTo, ChessConstants::ROOK, whiteToMove_);
        }
    } else {
        // ---- Quiet move ----
        looseAttacks(to);
        if (evaluator_) evaluator_->move(from, to, type, whiteToMove_);
    }

    // Update castling rights if rook or king moved
    if (whiteToMove_) {
        if (from == BoardConstants::HH1) castle_ &= ~WHITE_CASTLE_KINGSIDE;
        if (from == BoardConstants::HA1) castle_ &= ~WHITE_CASTLE_QUEENSIDE;
        if (from == BoardConstants::HE1) castle_ &= ~(WHITE_CASTLE_QUEENSIDE | WHITE_CASTLE_KINGSIDE);
    } else {
        if (from == BoardConstants::HH8) castle_ &= ~BLACK_CASTLE_KINGSIDE;
        if (from == BoardConstants::HA8) castle_ &= ~BLACK_CASTLE_QUEENSIDE;
        if (from == BoardConstants::HE8) castle_ &= ~(BLACK_CASTLE_QUEENSIDE | BLACK_CASTLE_KINGSIDE);
    }

    // Promotion
    if (Move::isPromotion(move)) {
        int promotedTo = Move::getPromoPiece(move);
        if (evaluator_) {
            evaluator_->capture(to, ChessConstants::PAWN, whiteToMove_);
            evaluator_->add(to, promotedTo, whiteToMove_);
        }
        materialSignature_[s] |= MATERIAL_SIGNATURE_BITS[promotedTo];
        if (pieceMask_[s][ChessConstants::PAWN].IsEmpty())
            materialSignature_[s] &= ~MATERIAL_SIGNATURE_BITS[ChessConstants::PAWN];
        type = promotedTo;
    }

    // Place piece on 'to'
    board_[to] = whiteToMove_ ? type : -type;
    pieceMask_[s][0]   .SetBit(to);
    pieceMask_[s][type].SetBit(to);
    if (IS_SLIDING[type]) slidingPieces_.SetBit(to);
    if (type == ChessConstants::KING) kingPos_[s] = to;

    attackSet(type, whiteToMove_, to);

    positionHash_ ^= Hashing::HASH_KEYS[s][type][to];
    if (type == ChessConstants::PAWN)
        pawnHash_ ^= Hashing::HASH_KEYS[s][ChessConstants::PAWN][to];

    // Double pawn push → set en passant
    if (move & Move::PAWN_DOUBLE) {
        int etmp = EN_PASSANT_TRANSLATE[to];
        if (!(pieceMask_[1^s][ChessConstants::PAWN] & attackFrom_[etmp]).IsEmpty())
            enPassant_ = etmp;
        else
            enPassant_ = 0;
    } else {
        enPassant_ = 0;
    }

    if (castle_ != hist.castle)
        positionHash_ ^= (Hashing::CASTLE_HASH_KEYS[castle_] ^
                          Hashing::CASTLE_HASH_KEYS[hist.castle]);
    if (enPassant_ != hist.enPassant)
        positionHash_ ^= (Hashing::EN_PASSANT_HASH_KEYS[enPassant_] ^
                          Hashing::EN_PASSANT_HASH_KEYS[hist.enPassant]);

    positionHash_ ^= Hashing::WTM_HASH;
    ++player_;
    whiteToMove_ = !whiteToMove_;
}

// ===========================================================================
// doNull
// ===========================================================================
void ChessBoard::doNull()
{
    if ((int)history_.size() <= player_) history_.emplace_back();
    History& hist = history_[player_];

    hist.enPassant = enPassant_;
    hist.castle    = castle_;
    hist.move      = 0;
    hist.posHash   = positionHash_;
    hist.pawnHash  = pawnHash_;

    if (enPassant_ != 0) {
        positionHash_ ^= (Hashing::EN_PASSANT_HASH_KEYS[0] ^
                          Hashing::EN_PASSANT_HASH_KEYS[enPassant_]);
        enPassant_ = 0;
    }
    positionHash_ ^= Hashing::WTM_HASH;
    ++player_;
    whiteToMove_ = !whiteToMove_;
}

// ===========================================================================
// undoMove
// ===========================================================================
void ChessBoard::undoMove()
{
    --player_;
    whiteToMove_ = !whiteToMove_;
    const History& hist = history_[player_];
    int move = hist.move;

    if (move != 0) {
        int from  = Move::getFrom(move);
        int to    = Move::getTo  (move);
        int type  = getPieceAt(to);
        int s     = side(whiteToMove_);

        attackClr(type, whiteToMove_, to);
        pieceMask_[s][0]   .ClearBit(to);
        pieceMask_[s][type].ClearBit(to);
        if (IS_SLIDING[type]) slidingPieces_.ClearBit(to);

        if (move & Move::CAPTURE) {
            int captured = hist.capturedPiece;
            attackSet(captured, !whiteToMove_, to);
            pieceMask_[1^s][0]       .SetBit(to);
            pieceMask_[1^s][captured].SetBit(to);
            if (IS_SLIDING[captured]) slidingPieces_.SetBit(to);
            board_[to] = whiteToMove_ ? -captured : captured;
            if (evaluator_) {
                evaluator_->add(to, captured, !whiteToMove_);
                evaluator_->move(to, from, type, whiteToMove_);
            }
            materialSignature_[1^s] |= MATERIAL_SIGNATURE_BITS[captured];

        } else if (move & Move::ENPASSANT) {
            int enPassantTo = EN_PASSANT_TRANSLATE[to];
            attackSet(ChessConstants::PAWN, !whiteToMove_, enPassantTo);
            pieceMask_[1^s][0]                 .SetBit(enPassantTo);
            pieceMask_[1^s][ChessConstants::PAWN].SetBit(enPassantTo);
            board_[enPassantTo] = whiteToMove_
                ? ChessConstants::BLACK_PAWN
                : ChessConstants::WHITE_PAWN;
            looseAttacks(enPassantTo);
            board_[to] = 0;
            gainAttacks(to);
            if (evaluator_) {
                evaluator_->add(enPassantTo, ChessConstants::PAWN, !whiteToMove_);
                evaluator_->move(to, from, ChessConstants::PAWN, whiteToMove_);
            }
            materialSignature_[1^s] |= MATERIAL_SIGNATURE_BITS[ChessConstants::PAWN];

        } else if (Move::isCastle(move)) {
            int rookFrom, rookTo;
            if (Move::isKingSideCastle(move)) {
                rookFrom = from + 3; rookTo = from + 1;
            } else {
                rookFrom = from - 4; rookTo = from - 1;
            }
            attackClr(ChessConstants::ROOK, whiteToMove_, rookTo);
            board_[rookFrom] = board_[rookTo];
            board_[rookTo]   = 0;
            board_[to]       = 0;
            pieceMask_[s][0]                .ClearBit(rookTo);
            pieceMask_[s][ChessConstants::ROOK].ClearBit(rookTo);
            slidingPieces_.ClearBit(rookTo);
            pieceMask_[s][0]                .SetBit(rookFrom);
            pieceMask_[s][ChessConstants::ROOK].SetBit(rookFrom);
            slidingPieces_.SetBit(rookFrom);
            attackSet(ChessConstants::ROOK, whiteToMove_, rookFrom);
            gainAttacks(rookTo);
            gainAttacks(to);
            if (evaluator_) {
                evaluator_->move(to, from, ChessConstants::KING, whiteToMove_);
                evaluator_->move(rookTo, rookFrom, ChessConstants::ROOK, whiteToMove_);
            }
        } else {
            board_[to] = 0;
            gainAttacks(to);
            if (evaluator_) evaluator_->move(to, from, type, whiteToMove_);
        }

        if (Move::isPromotion(move)) {
            if (evaluator_) {
                evaluator_->capture(from, type, whiteToMove_);
                evaluator_->add(from, ChessConstants::PAWN, whiteToMove_);
            }
            materialSignature_[s] |= MATERIAL_SIGNATURE_BITS[ChessConstants::PAWN];
            if (pieceMask_[s][type].IsEmpty())
                materialSignature_[s] &= ~MATERIAL_SIGNATURE_BITS[type];
            type = ChessConstants::PAWN;
        }

        board_[from] = whiteToMove_ ? type : -type;
        pieceMask_[s][0]   .SetBit(from);
        pieceMask_[s][type].SetBit(from);
        if (IS_SLIDING[type]) slidingPieces_.SetBit(from);
        attackSet(type, whiteToMove_, from);
        looseAttacks(from);
        if (type == ChessConstants::KING) kingPos_[s] = from;
    }

    enPassant_    = hist.enPassant;
    castle_       = hist.castle;
    positionHash_ = hist.posHash;
    pawnHash_     = hist.pawnHash;
}

// ===========================================================================
// Piece queries
// ===========================================================================
int ChessBoard::getPieceAt(int square) const
{
    return std::abs(board_[square]);
}

int ChessBoard::getPieceAt(int level, int file, int rank) const
{
    return std::abs(board_[BitBoard::BitOffset(level, file, rank)]);
}

Player ChessBoard::getSideAt(int square) const
{
    if (board_[square] == 0) return Player::none;
    return board_[square] < 0 ? Player::black : Player::white;
}

bool ChessBoard::isWhiteAt(int level, int file, int rank) const
{
    return board_[BitBoard::BitOffset(level, file, rank)] > 0;
}

// ===========================================================================
// Castling
// ===========================================================================
bool ChessBoard::canWhiteCastleKingSide()  const { return (castle_ & WHITE_CASTLE_KINGSIDE)  != 0; }
bool ChessBoard::canWhiteCastleQueenSide() const { return (castle_ & WHITE_CASTLE_QUEENSIDE) != 0; }
bool ChessBoard::canBlackCastleKingSide()  const { return (castle_ & BLACK_CASTLE_KINGSIDE)  != 0; }
bool ChessBoard::canBlackCastleQueenSide() const { return (castle_ & BLACK_CASTLE_QUEENSIDE) != 0; }

bool ChessBoard::isCastleLegal(int move) const
{
    int from  = Move::getFrom(move);
    int oside = whiteToMove_ ? 1 : 0;

    if (move & Move::CASTLE_KSIDE) {
        if (whiteToMove_  && !(castle_ & WHITE_CASTLE_KINGSIDE))  return false;
        if (!whiteToMove_ && !(castle_ & BLACK_CASTLE_KINGSIDE))  return false;
        if (board_[from+1] != 0 || board_[from+2] != 0)          return false;
        return (attackFrom_[from] | attackFrom_[from+1] | attackFrom_[from+2])
                   .IsEmpty() == false
               ? ((attackFrom_[from]|attackFrom_[from+1]|attackFrom_[from+2]) & pieceMask_[oside][0]).IsEmpty()
               : true;
    } else {
        if (whiteToMove_  && !(castle_ & WHITE_CASTLE_QUEENSIDE)) return false;
        if (!whiteToMove_ && !(castle_ & BLACK_CASTLE_QUEENSIDE)) return false;
        if (board_[from-1]!=0 || board_[from-2]!=0 || board_[from-3]!=0) return false;
        return ((attackFrom_[from]|attackFrom_[from-1]|attackFrom_[from-2]) & pieceMask_[oside][0]).IsEmpty();
    }
}

// ===========================================================================
// Move generation
// ===========================================================================
void ChessBoard::generatePseudoLegalMoves(IMoveList& mvs)
{
    // Captures
    BitBoard tmp = getMask(!whiteToMove_);
    while (!tmp.IsEmpty()) {
        int to = tmp.findFirstOne();
        tmp.ClearBit(to);
        generateTo(to, mvs);
    }
    // Non-captures
    tmp = getMask(whiteToMove_);
    while (!tmp.IsEmpty()) {
        int from = tmp.findFirstOne();
        tmp.ClearBit(from);
        generateFrom(from, mvs);
    }
    generateEnPassant(mvs);
}

void ChessBoard::generateLegalMoves(IMoveList& mvs)
{
    IntVector tmvs;
    generatePseudoLegalMoves(tmvs);
    for (int i = 0; i < tmvs.size(); ++i) {
        int m = tmvs.get_Renamed(i);
        if ((m & Move::CASTLE) && !isCastleLegal(m)) continue;
        doMove(m);
        bool inCheck = getOppInCheck();
        undoMove();
        if (!inCheck) mvs.add(m);
    }
}

void ChessBoard::generateTo(int to, IMoveList& mvs)
{
    BitBoard attacks = attackFrom_[to] & getMask(whiteToMove_);
    while (!attacks.IsEmpty()) {
        int from = attacks.findFirstOne();
        attacks.ClearBit(from);
        if (getPieceAt(from) == ChessConstants::PAWN &&
            (to <= BoardConstants::HH1 || to >= BoardConstants::HA8)) {
            int mv = Move::makeMove(from, to) | Move::CAPTURE;
            mvs.add(mv | Move::PROMO_QUEEN);
            mvs.add(mv | Move::PROMO_ROOK);
            mvs.add(mv | Move::PROMO_BISHOP);
            mvs.add(mv | Move::PROMO_KNIGHT);
        } else {
            mvs.add(Move::makeMove(from, to) | Move::CAPTURE);
        }
    }
}

void ChessBoard::generateFrom(int from, IMoveList& mvs)
{
    int pc  = getPieceAt(from);
    bool iw = (getSideAt(from) == Player::white);
    if      (pc == ChessConstants::PAWN) generateFromPawn(from, mvs, iw);
    else if (pc == ChessConstants::KING) generateFromKing(from, mvs, iw);
    else                                 generateFromOtherPieces(from, mvs);
}

void ChessBoard::generateFromOtherPieces(int from, IMoveList& mvs)
{
    BitBoard occupied = pieceMask_[0][0] | pieceMask_[1][0];
    BitBoard attacks  = attackTo_[from] & ~occupied;
    while (!attacks.IsEmpty()) {
        int to = attacks.findFirstOne();
        attacks.ClearBit(to);
        mvs.add(Move::makeMove(from, to));
    }
}

void ChessBoard::generateFromKing(int from, IMoveList& mvs, bool isWhite)
{
    BitBoard occupied  = pieceMask_[0][0] | pieceMask_[1][0];
    BitBoard attack    = attackTo_[from] & ~occupied;
    BitBoard opponent  = getMask(!isWhite);

    while (!attack.IsEmpty()) {
        int to = attack.findFirstOne();
        attack.ClearBit(to);
        if ((attackFrom_[to] & opponent).IsEmpty()) {
            mvs.add(Move::makeMove(from, to));
        }
    }

    if (isWhite) {
        if (castle_ & WHITE_CASTLE_KINGSIDE)
            mvs.add(Move::makeMove(BoardConstants::HE1, BoardConstants::HG1) | Move::CASTLE_KSIDE);
        if (castle_ & WHITE_CASTLE_QUEENSIDE)
            mvs.add(Move::makeMove(BoardConstants::HE1, BoardConstants::HC1) | Move::CASTLE_QSIDE);
    } else {
        if (castle_ & BLACK_CASTLE_KINGSIDE)
            mvs.add(Move::makeMove(BoardConstants::HE8, BoardConstants::HG8) | Move::CASTLE_KSIDE);
        if (castle_ & BLACK_CASTLE_QUEENSIDE)
            mvs.add(Move::makeMove(BoardConstants::HE8, BoardConstants::HC8) | Move::CASTLE_QSIDE);
    }
}

void ChessBoard::generateFromPawn(int from, IMoveList& mvs, bool isWhite)
{
    Lfr next(from);
    if (isWhite) {
        ++(next.Rank);
        if (!Lfr::IsValid(next.Level, next.File, next.Rank)) return;
        if (board_[static_cast<int>(next)] == 0) {
            int nextSq = static_cast<int>(next);
            int levelWidth = BitBoard::LEVEL_WIDTH[next.Level];
            if (next.Rank == levelWidth - 1) {
                int mv = Move::makeMove(from, nextSq);
                mvs.add(mv | Move::PROMO_QUEEN);
                mvs.add(mv | Move::PROMO_ROOK);
                mvs.add(mv | Move::PROMO_BISHOP);
                mvs.add(mv | Move::PROMO_KNIGHT);
            } else {
                mvs.add(Move::makeMove(from, nextSq));
                if (next.Rank == 2) {
                    ++(next.Rank);
                    if (Lfr::IsValid(next.Level, next.File, next.Rank) &&
                        board_[static_cast<int>(next)] == 0) {
                        mvs.add(Move::makeMove(from, static_cast<int>(next)) | Move::PAWN_DOUBLE);
                    }
                }
            }
        }
    } else {
        --(next.Rank);
        if (!Lfr::IsValid(next.Level, next.File, next.Rank)) return;
        if (board_[static_cast<int>(next)] == 0) {
            int nextSq = static_cast<int>(next);
            if (next.Rank == 0) {
                int mv = Move::makeMove(from, nextSq);
                mvs.add(mv | Move::PROMO_QUEEN);
                mvs.add(mv | Move::PROMO_ROOK);
                mvs.add(mv | Move::PROMO_BISHOP);
                mvs.add(mv | Move::PROMO_KNIGHT);
            } else {
                mvs.add(Move::makeMove(from, nextSq));
                if (next.Rank == 5) {
                    --(next.Rank);
                    if (Lfr::IsValid(next.Level, next.File, next.Rank) &&
                        board_[static_cast<int>(next)] == 0) {
                        mvs.add(Move::makeMove(from, static_cast<int>(next)) | Move::PAWN_DOUBLE);
                    }
                }
            }
        }
    }
}

void ChessBoard::generateEnPassant(IMoveList& mvs)
{
    if (enPassant_ == 0) return;
    BitBoard attacks = attackFrom_[enPassant_] & getMask(whiteToMove_, ChessConstants::PAWN);
    while (!attacks.IsEmpty()) {
        int from = attacks.findFirstOne();
        attacks.ClearBit(from);
        mvs.add(Move::makeMove(from, enPassant_) | Move::ENPASSANT);
    }
}

// ===========================================================================
// isPseudoLegalMove
// ===========================================================================
bool ChessBoard::isPseudoLegalMove(int move) const
{
    int from = Move::getFrom(move);
    if (whiteToMove_  && board_[from] <= 0) return false;
    if (!whiteToMove_ && board_[from] >= 0) return false;
    if (Move::isPromotion(move) && getPieceAt(from) != ChessConstants::PAWN) return false;

    int to = Move::getTo(move);
    if (move & Move::CAPTURE) {
        if (whiteToMove_  && board_[to] >= 0) return false;
        if (!whiteToMove_ && board_[to] <= 0) return false;
        return attackTo_[from].GetBit(to) != 0;
    } else if (move & Move::ENPASSANT) {
        if (enPassant_ == 0 || to != enPassant_) return false;
        if (getPieceAt(from) != ChessConstants::PAWN) return false;
        return attackTo_[from].GetBit(to) != 0;
    } else if (move & Move::CASTLE) {
        return isCastleLegal(move);
    } else {
        if (board_[to] != 0) return false;
        if (getPieceAt(from) == ChessConstants::PAWN) {
            Lfr next(from);
            next.Rank = whiteToMove_ ? next.Rank + 1 : next.Rank - 1;
            if (move & Move::PAWN_DOUBLE) {
                if (board_[static_cast<int>(next)] != 0) return false;
                next.Rank = whiteToMove_ ? next.Rank + 1 : next.Rank - 1;
            }
            if (static_cast<int>(next) != to) return false;
            if (whiteToMove_ && !Move::isPromotion(move) && to >= BoardConstants::HA8) return false;
            if (!whiteToMove_ && !Move::isPromotion(move) && to <= BoardConstants::HH1) return false;
        } else {
            if (attackTo_[from].GetBit(to) == 0) return false;
            if (move & Move::PAWN_DOUBLE) return false;
        }
    }
    return true;
}

bool ChessBoard::isLegalMove(int move) const
{
    IntVector tmp;
    const_cast<ChessBoard*>(this)->generateLegalMoves(tmp);
    return tmp.contains(move);
}

// ===========================================================================
// isCheckingMove
// ===========================================================================
bool ChessBoard::testDirectCheck(const std::vector<BitBoard>& epm, int to, int oppKing) const
{
    return epm[oppKing].GetBit(to) != 0;
}

bool ChessBoard::isCheckingMove(int move) const
{
    int from    = Move::getFrom(move);
    int to      = Move::getTo  (move);
    int oppKing = kingPos_[whiteToMove_ ? 1 : 0];

    switch (board_[from]) {
        case  ChessConstants::PAWN:
            if (Geometry::BLACK_PAWN_EPM[oppKing].GetBit(to)) return true; break;
        case -ChessConstants::PAWN:
            if (Geometry::WHITE_PAWN_EPM[oppKing].GetBit(to)) return true; break;
        case  ChessConstants::KNIGHT:
        case -ChessConstants::KNIGHT:
            if (Geometry::KNIGHT_EPM[oppKing].GetBit(to)) return true; break;
        case  ChessConstants::BISHOP:
        case -ChessConstants::BISHOP:
            if (testDirectCheck(Geometry::BISHOP_EPM, to, oppKing)) return true; break;
        case  ChessConstants::ROOK:
        case -ChessConstants::ROOK:
            if (testDirectCheck(Geometry::ROOK_EPM, to, oppKing)) return true; break;
        case  ChessConstants::QUEEN:
        case -ChessConstants::QUEEN:
            if (testDirectCheck(Geometry::QUEEN_EPM, to, oppKing)) return true; break;
    }

    // Discovered check via rook/queen
    if (Geometry::ROOK_EPM[oppKing].GetBit(from)) {
        BitBoard rookOrQueen = Geometry::RAY[oppKing][from] &
            (getMask(whiteToMove_, ChessConstants::ROOK) | getMask(whiteToMove_, ChessConstants::QUEEN));
        while (!rookOrQueen.IsEmpty()) {
            int idx = rookOrQueen.findFirstOne();
            rookOrQueen.ClearBit(idx);
            BitBoard tmp = (pieceMask_[0][0] | pieceMask_[1][0]) & Geometry::INTER_PATH[idx][oppKing];
            if (tmp.countBits() == 1) return true;
        }
    }
    // Discovered check via bishop/queen
    if (Geometry::BISHOP_EPM[oppKing].GetBit(from)) {
        BitBoard bq = Geometry::RAY[oppKing][from] &
            (getMask(whiteToMove_, ChessConstants::BISHOP) | getMask(whiteToMove_, ChessConstants::QUEEN));
        while (!bq.IsEmpty()) {
            int idx = bq.findFirstOne();
            bq.ClearBit(idx);
            BitBoard tmp = (pieceMask_[0][0] | pieceMask_[1][0]) & Geometry::INTER_PATH[idx][oppKing];
            if (tmp.countBits() == 1) return true;
        }
    }
    return false;
}

// ===========================================================================
// Misc queries
// ===========================================================================
int ChessBoard::getPositions(bool theWtm, int type, std::vector<int>& squares) const
{
    BitBoard pieces = getMask(theWtm, type);
    int count = 0;
    squares.clear();
    while (!pieces.IsEmpty()) {
        int sq = pieces.findFirstOne();
        pieces.ClearBit(sq);
        squares.push_back(sq);
        ++count;
    }
    return count;
}

int ChessBoard::getMaterialSignature(bool theWtm) const
{
    return materialSignature_[side(theWtm)];
}

int ChessBoard::getKingPos(bool theWtm) const
{
    return kingPos_[side(theWtm)];
}

BitBoard ChessBoard::getMask(bool theWtm) const
{
    return pieceMask_[side(theWtm)][0];
}

BitBoard ChessBoard::getMask(bool theWtm, int type) const
{
    return pieceMask_[side(theWtm)][type];
}

// ===========================================================================
// checkDraw
// ===========================================================================
bool ChessBoard::checkDraw(int count) const
{
    int reps = count;
    for (int p = player_ - 1; p >= 0; --p) {
        const History& h = history_[p];
        if ((h.move & (Move::CAPTURE | Move::CASTLE | Move::PROMOTION)) != 0 ||
            h.pawnHash != pawnHash_) {
            return false;
        }
        if (h.posHash == positionHash_) {
            --reps;
            if (reps == 0) return true;
        }
    }
    return false;
}

// ===========================================================================
// checkMaterialSignatures (debug)
// ===========================================================================
void ChessBoard::checkMaterialSignatures() const
{
    int tw = 0, tb = 0;
    for (int i = ChessConstants::PAWN; i <= ChessConstants::QUEEN; ++i) {
        if (!getMask(true,  i).IsEmpty()) tw |= MATERIAL_SIGNATURE_BITS[i];
        if (!getMask(false, i).IsEmpty()) tb |= MATERIAL_SIGNATURE_BITS[i];
    }
    if (tw != materialSignature_[0] || tb != materialSignature_[1])
        throw std::runtime_error("Material signature mismatch");
}

void ChessBoard::checkSanity() const
{
    // Verify reciprocal attack tables
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        BitBoard tmp = attackTo_[i];
        while (!tmp.IsEmpty()) {
            int idx = tmp.findFirstOne();
            tmp.ClearBit(idx);
            if (attackFrom_[idx].GetBit(i) == 0)
                throw std::runtime_error("Attack table sanity check failed (attackTo)");
        }
    }
    for (int i = 0; i < BitBoard::SIZE; ++i) {
        BitBoard tmp = attackFrom_[i];
        while (!tmp.IsEmpty()) {
            int idx = tmp.findFirstOne();
            tmp.ClearBit(idx);
            if (attackTo_[idx].GetBit(i) == 0)
                throw std::runtime_error("Attack table sanity check failed (attackFrom)");
        }
    }
    checkMaterialSignatures();
}

void ChessBoard::debug() const
{
    // Minimal stub – just print last few moves
    for (int i = 0; i < player_; ++i) {
        // history is available but we avoid a heavy logging dependency
    }
}

// ===========================================================================
// toString (ASCII board)
// ===========================================================================
std::string ChessBoard::toString(int showMoves) const
{
    std::ostringstream buf;

    for (int level = (int)BitBoard::LEVEL_WIDTH.size() - 1; level >= 0; --level) {
        int lw = BitBoard::LEVEL_WIDTH[level];

        // Indent
        auto indent = [&]() {
            int spaces = 8 - lw;
            while (spaces-- > 0) buf << "  ";
        };

        indent();
        buf << "   +";
        for (int f = 0; f < lw; ++f) buf << "---+";
        buf << '\n';

        for (int rank = lw - 1; rank >= 0; --rank) {
            indent();
            buf << ' ' << static_cast<char>('1' + rank) << ' ';
            for (int file = 0; file < lw; ++file) {
                int i = BitBoard::BitOffset(level, file, rank);
                buf << '|';
                if (enPassant_ != 0 && i == enPassant_) {
                    buf << "<E>";
                } else {
                    Player ps = getSideAt(i);
                    if (ps == Player::black) buf << '*';
                    else if (showMoves >= 0 && attackTo_[showMoves].GetBit(i)) buf << '.';
                    else buf << ' ';
                    buf << PIECE_NAME[getPieceAt(i)];
                    if (ps == Player::black) buf << '*';
                    else if (showMoves >= 0 && attackTo_[showMoves].GetBit(i)) buf << '.';
                    else buf << ' ';
                }
            }
            buf << '|';
            if (level == 7) {
                if (rank == 4) {
                    std::ostringstream hex;
                    hex << std::hex << positionHash_;
                    buf << "  Hashkey: " << hex.str();
                }
                if ((rank == lw - 1 && !whiteToMove_) || (rank == 0 && whiteToMove_))
                    buf << " *";
            }
            buf << "\n   ";
            indent();
            buf << '+';
            for (int f = 0; f < lw; ++f) buf << "---+";
            buf << '\n';
        }
        buf << ' ';
        indent();
        buf << static_cast<char>('a' + level);
        for (int f = 0; f < lw; ++f) {
            buf << "   " << static_cast<char>('a' + f);
        }
        buf << '\n';
    }
    return buf.str();
}

std::string ChessBoard::toStringHex(int showMoves) const
{
    std::ostringstream buffer;

    auto indentRowHex = [&buffer](int level, int rank)
    {
        int file = BitBoard::MAX_LEVEL_WIDTH - HexLfr::RankWidth(level, rank);
        while (file > 0) {
            buffer << "  ";
            --file;
        }
    };

    auto markPieceSide = [this, &buffer, showMoves](int square)
    {
        if (getSideAt(square) == Player::black) {
            if (showMoves >= 0 && attackTo_[showMoves].GetBit(square) == 1) {
                buffer << 'x';
            } else {
                buffer << '*';
            }
        } else {
            if (showMoves >= 0 && attackTo_[showMoves].GetBit(square) == 1) {
                buffer << '.';
            } else {
                buffer << ' ';
            }
        }
    };

    for (int level = BitBoard::MAX_LEVEL_WIDTH - 1; level >= 0; --level) {
        for (int rank = BitBoard::MAX_LEVEL_WIDTH - 1; rank >= 0; --rank) {
            indentRowHex(level, rank);
            if (rank == BitBoard::MAX_LEVEL_WIDTH - 1 || HexLfr::RankWidth(level, rank) >= HexLfr::RankWidth(level, rank + 1)) {
                buffer << "  ";
            } else {
                buffer << "\\ ";
            }

            for (int file = (level > rank ? (8 - HexLfr::RankWidth(level, rank)) : 0);
                 file < (level > rank ? 8 : HexLfr::RankWidth(level, rank));
                 ++file) {
                buffer << '/' << static_cast<char>('a' + file) << "\\ ";
            }

            if (rank == BitBoard::MAX_LEVEL_WIDTH - 1 || HexLfr::RankWidth(level, rank) >= HexLfr::RankWidth(level, rank + 1)) {
                buffer << '\n';
            } else {
                buffer << "/\n";
            }

            buffer << static_cast<char>('1' + rank);
            indentRowHex(level, rank);

            for (int file = (level > rank ? (8 - HexLfr::RankWidth(level, rank)) : 0);
                 file < (level > rank ? 8 : HexLfr::RankWidth(level, rank));
                 ++file) {
                HexLfr hexLfr(level, file, rank);
                Lfr lfr = static_cast<Lfr>(hexLfr);
                int square = static_cast<int>(lfr);

                buffer << '|';
                if (enPassant_ != 0 && square == enPassant_) {
                    if (showMoves >= 0 && attackTo_[showMoves].GetBit(square) == 1) {
                        buffer << ".E.";
                    } else {
                        buffer << "<E>";
                    }
                } else {
                    markPieceSide(square);
                    buffer << PIECE_NAME[getPieceAt(square)];
                    markPieceSide(square);
                }
            }

            buffer << '|';
            if (level == 4) {
                if (rank == 4) {
                    buffer << "  Hashkey: " << std::hex << positionHash_ << std::dec;
                }
                if ((rank == (BitBoard::LEVEL_WIDTH[level] - 1) && !whiteToMove_) || (rank == 0 && whiteToMove_)) {
                    buffer << " *";
                }
            }
            buffer << '\n';
        }

        buffer << static_cast<char>('a' + level);
        indentRowHex(level, 0);
        buffer << ' ';
        for (int file = 0; file < HexLfr::RankWidth(level, 0); ++file) {
            buffer << "\\ / ";
        }
        buffer << "\n\n";
    }

    return buffer.str();
}

// ===========================================================================
// EPD parser helper
// ===========================================================================
std::unique_ptr<IPosition> ChessBoard::parseEpd(const std::string& epd) const
{
    std::vector<int> board(BitBoard::SIZE, 0);

    std::istringstream stream(epd);
    std::vector<std::string> fenParts;
    for (std::string part; stream >> part;) {
        fenParts.push_back(part);
    }
    if (fenParts.size() < 4) {
        throw std::runtime_error("Invalid EPD");
    }

    int level = BitBoard::NUM_LEVELS - 1;
    int rank = BitBoard::LEVEL_WIDTH[level] - 1;
    int file = 0;

    for (char ch : fenParts[0]) {
        if (ch != '/') {
            if (file >= BitBoard::LEVEL_WIDTH[level]) {
                throw std::runtime_error("EPD file out of bounds");
            }
            if (!Lfr::IsValid(level, file, rank)) {
                throw std::runtime_error("EPD contains invalid position");
            }
        }

        int square = 0;
        if (ch != '/') {
            square = BitBoard::BitOffset(level, file, rank);
        }

        switch (ch) {
            case '1': case '2': case '3': case '4':
            case '5': case '6': case '7': case '8':
                file += ch - '0';
                break;
            case 'P': board[square] = ChessConstants::PAWN; ++file; break;
            case 'N': board[square] = ChessConstants::KNIGHT; ++file; break;
            case 'B': board[square] = ChessConstants::BISHOP; ++file; break;
            case 'R': board[square] = ChessConstants::ROOK; ++file; break;
            case 'Q': board[square] = ChessConstants::QUEEN; ++file; break;
            case 'K': board[square] = ChessConstants::KING; ++file; break;
            case 'p': board[square] = -ChessConstants::PAWN; ++file; break;
            case 'n': board[square] = -ChessConstants::KNIGHT; ++file; break;
            case 'b': board[square] = -ChessConstants::BISHOP; ++file; break;
            case 'r': board[square] = -ChessConstants::ROOK; ++file; break;
            case 'q': board[square] = -ChessConstants::QUEEN; ++file; break;
            case 'k': board[square] = -ChessConstants::KING; ++file; break;
            case '/':
                --rank;
                file = 0;
                if (rank < 0) {
                    --level;
                    if (level < 0) {
                        throw std::runtime_error("EPD contains too many ranks");
                    }
                    rank = BitBoard::LEVEL_WIDTH[level] - 1;
                }
                break;
            default:
                throw std::runtime_error("EPD contains illegal character");
        }
    }

    const bool whiteToMove = !fenParts[1].empty() && std::toupper(static_cast<unsigned char>(fenParts[1][0])) == 'W';

    bool wCastleK = false;
    bool wCastleQ = false;
    bool bCastleK = false;
    bool bCastleQ = false;
    if (fenParts[2] != "-") {
        wCastleK = fenParts[2].find('K') != std::string::npos;
        wCastleQ = fenParts[2].find('Q') != std::string::npos;
        bCastleK = fenParts[2].find('k') != std::string::npos;
        bCastleQ = fenParts[2].find('q') != std::string::npos;
    }

    int enPassant = 0;
    if (fenParts[3] != "-") {
        if (fenParts[3].size() < 3) {
            throw std::runtime_error("Illegal en passant square");
        }
        const int epLevel = fenParts[3][0] - 'a';
        const int epFile = fenParts[3][1] - 'a';
        const int epRank = fenParts[3][2] - '1';
        if (!Lfr::IsValid(epLevel, epFile, epRank)) {
            throw std::runtime_error("Illegal en passant square");
        }
        if (epFile < 0 || epFile > (BitBoard::LEVEL_WIDTH[epLevel] - 1) ||
            (epRank != 2 && epRank != (BitBoard::LEVEL_WIDTH[epLevel] - 3))) {
            throw std::runtime_error("Illegal en passant square");
        }
        enPassant = BitBoard::BitOffset(epLevel, epFile, epRank);
    }

    return std::make_unique<BoardPosition>(board, whiteToMove, enPassant, wCastleK, wCastleQ, bCastleK, bCastleQ);
}

} // namespace tgreiner::amy::chess::engine
