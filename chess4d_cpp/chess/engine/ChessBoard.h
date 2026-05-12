/*-
 * Copyright (c) 2003, 2004 Thorsten Greiner
 * All rights reserved.
 * (C++ port)
 */
#pragma once

#include <array>
#include <cstdint>
#include <memory>
#include <string>
#include <vector>

#include "bitboard/BitBoard.h"
#include "bitboard/BoardConstants.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/Position.h"
#include "common/engine/IMoveList.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;
using tgreiner::amy::common::engine::IMoveList;

/// An implementation of the 4-D chess board using bitboards.
class ChessBoard {
public:
    // ------------------------------------------------------------------
    // Static data
    // ------------------------------------------------------------------
    static const std::array<char, 7> PIECE_NAME; // ' ','P','N','B','R','Q','K'

    // ------------------------------------------------------------------
    // Constructors
    // ------------------------------------------------------------------
    ChessBoard();                           ///< Start from initial position
    explicit ChessBoard(const std::string& epd); ///< Parse EPD string
    explicit ChessBoard(const IPosition&   pos); ///< Set position directly
    ChessBoard(const ChessBoard& other);    ///< Deep copy

    // ------------------------------------------------------------------
    // Properties (getters matching C# property names)
    // ------------------------------------------------------------------
    bool getWhiteToMove()      const { return whiteToMove_; }
    bool getInCheck()          const;
    bool getOppInCheck()       const;
    bool getDrawByRepetition() const { return checkDraw(2); }
    bool getRepeated()         const { return checkDraw(1); }
    bool getCheckMate()        const;
    bool getInsufficientMaterial() const;
    bool getFiftyMoveRuleDraw()    const;
    bool canUndo()             const { return player_ > 0; }

    int  getEnPassant()        const { return enPassant_; }
    int64_t getPosHash()       const { return positionHash_; }
    int64_t getPawnHash()      const { return pawnHash_; }
    int  getLastMove()         const;
    int  getLastCaptured()     const;

    BitBoard getSlidingPieces()const { return slidingPieces_; }
    BitBoard getMaskNonPawn()  const;

    // ------------------------------------------------------------------
    // Position get/set
    // ------------------------------------------------------------------
    std::unique_ptr<IPosition> getPosition() const;
    void setPosition(const IPosition& pos);

    // ------------------------------------------------------------------
    // Evaluator
    // ------------------------------------------------------------------
    IEvaluator* getEvaluator() const { return evaluator_; }
    void setEvaluator(IEvaluator* ev);

    // ------------------------------------------------------------------
    // Move making / unmaking
    // ------------------------------------------------------------------
    virtual void doMove  (int move);
    virtual void undoMove();
    virtual void doNull  ();

    // ------------------------------------------------------------------
    // Move generation
    // ------------------------------------------------------------------
    virtual void generatePseudoLegalMoves(IMoveList& mvs);
    virtual void generateLegalMoves      (IMoveList& mvs);

    // ------------------------------------------------------------------
    // Move-generation helpers (accessible by derived generators)
    // ------------------------------------------------------------------
    virtual void generateTo       (int to,   IMoveList& mvs);
    virtual void generateFrom     (int from, IMoveList& mvs);
    virtual void generateEnPassant(IMoveList& mvs);

    // ------------------------------------------------------------------
    // Queries
    // ------------------------------------------------------------------
    int       getPieceAt(int square) const;
    int       getPieceAt(int level, int file, int rank) const;
    Player    getSideAt (int square) const;
    bool      isWhiteAt (int level, int file, int rank) const;

    bool      isCastleLegal   (int move) const;
    bool      isPseudoLegalMove(int move) const;
    bool      isLegalMove      (int move) const;
    bool      isCheckingMove   (int move) const;

    // Castling rights
    bool canWhiteCastleKingSide()  const;
    bool canWhiteCastleQueenSide() const;
    bool canBlackCastleKingSide()  const;
    bool canBlackCastleQueenSide() const;

    // Piece/position queries
    int  getPositions(bool whiteToMove, int type, std::vector<int>& squares) const;
    int  getMaterialSignature(bool whiteToMove) const;
    int  getKingPos(bool whiteToMove) const;

    BitBoard getMask(bool whiteToMove)           const;
    BitBoard getMask(bool whiteToMove, int type) const;

    BitBoard getAttackFrom(int square) const { return attackFrom_[square]; }
    BitBoard getAttackTo  (int square) const { return attackTo_[square]; }

    // ------------------------------------------------------------------
    // Diagnostics / display
    // ------------------------------------------------------------------
    std::string toString()               const { return toString(-1); }
    std::string toString(int showMoves)  const;
    std::string toStringHex(int showMoves) const;

    void debug()       const;
    void checkSanity() const;

    // ------------------------------------------------------------------
    // Internal constants (used by generators / evaluator)
    // ------------------------------------------------------------------
    static constexpr int WHITE_CASTLE_KINGSIDE  = 0x01;
    static constexpr int WHITE_CASTLE_QUEENSIDE = 0x02;
    static constexpr int BLACK_CASTLE_KINGSIDE  = 0x04;
    static constexpr int BLACK_CASTLE_QUEENSIDE = 0x08;

    static constexpr short MAJOR_PIECES_OR_PAWNS = 0x19;

protected:
    // ------------------------------------------------------------------
    // Internal helpers
    // ------------------------------------------------------------------
    static int side(bool theWtm) { return theWtm ? 0 : 1; }

    void attackSet(int type, bool theWtm, int square);
    void attackClr(int type, bool theWtm, int square);
    void gainAttack (int from, int to);
    void looseAttack(int from, int to);
    void gainAttacks (int to);
    void looseAttacks(int to);
    void recalcAttacks();

    void generateFromPawn      (int from, IMoveList& mvs, bool isWhite);
    void generateFromKing      (int from, IMoveList& mvs, bool isWhite);
    void generateFromOtherPieces(int from, IMoveList& mvs);

    bool checkDraw(int count) const;

    bool testDirectCheck(const std::vector<BitBoard>& epm, int to, int oppKing) const;

    void checkMaterialSignatures() const;
    std::unique_ptr<IPosition> parseEpd(const std::string& fen) const;

    void setReciprocalAttacks(int from, int to);

    // ------------------------------------------------------------------
    // Board state
    // ------------------------------------------------------------------
    std::vector<BitBoard> attackTo_;    ///< attackTo_[sq]   = squares attacked FROM sq
    std::vector<BitBoard> attackFrom_;  ///< attackFrom_[sq] = squares that ATTACK sq

    BitBoard pieceMask_[2][ChessConstants::LAST_PIECE]; ///< [side][type]
    BitBoard slidingPieces_;

    std::vector<int> board_;  ///< +ve = white, -ve = black, 0 = empty
    int kingPos_[2]{};

    int  enPassant_{ 0 };
    int  castle_   { 0 };
    bool whiteToMove_{ true };
    int  player_   { 0 };

    int64_t positionHash_{ 0 };
    int64_t pawnHash_    { 0 };

    int materialSignature_[2]{};

    IEvaluator* evaluator_{ nullptr };

    // ------------------------------------------------------------------
    // History record
    // ------------------------------------------------------------------
    struct History {
        int     move         { 0 };
        int     enPassant    { 0 };
        int     capturedPiece{ 0 };
        int     castle       { 0 };
        int64_t posHash      { 0 };
        int64_t pawnHash     { 0 };
    };
    std::vector<History> history_;

    // ------------------------------------------------------------------
    // Static tables
    // ------------------------------------------------------------------
    static const bool  IS_SLIDING[7];
    static const int   EN_PASSANT_TRANSLATE[];
    static const short MATERIAL_SIGNATURE_BITS[ChessConstants::LAST_PIECE];
};

} // namespace tgreiner::amy::chess::engine
