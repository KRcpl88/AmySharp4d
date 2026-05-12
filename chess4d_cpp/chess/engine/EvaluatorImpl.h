#pragma once

#include <array>

#include "bitboard/BitBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/GamePhase.h"
#include "chess/engine/IEvaluator.h"
#include "chess/engine/OutsidePassedPawnIdentifier.h"
#include "chess/engine/PawnEvalCache.h"
#include "common/engine/EvalCache.h"

namespace tgreiner::amy::chess::engine {

class ChessBoard;

class EvaluatorImpl : public IEvaluator {
public:
    explicit EvaluatorImpl(ChessBoard& theBoard);

    int getWhiteMaterial() const override;
    int getBlackMaterial() const override;
    int evaluate(int alpha, int beta) override;
    void init() override;
    void move(int from, int to, int type, bool whiteToMove) override;
    void capture(int square, int type, bool whiteToMove) override;
    void add(int square, int type, bool whiteToMove) override;
    int getMaterialValue(int piece) const override;

private:
    using Table344 = std::array<int, tgreiner::amy::bitboard::BitBoard::SIZE>;
    using Table64 = std::array<int, 64>;

    int evaluateInternal(int alpha, int beta);
    int evalPawns();
    int evalPassedPawns();
    int evalKnights();
    int evalBishops();
    int evalRooks();
    int evalQueens();
    int evalMaterialImbalance();
    bool oppositeColoredBishopsAndPawns();
    void determineGamePhase();
    const GamePhase& determineGamePhase(bool whiteToMove) const;
    int getScale(bool whiteToMove) const;

    static Table344 createPieceSquareTable(const Table64& levelHValues);

    static const Table344 ZERO_POS;
    static const Table344 pawnPos;
    static const Table344 knightPos;
    static const Table344 bishopPos;
    static const Table344 rookPos;
    static const Table344 queenPos;
    static const Table344 kingPosOpening;
    static const Table344 kingPosEndgame;

    ChessBoard& board_;
    OutsidePassedPawnIdentifier outsidePassedPawnId_;

    int pawnValue_ = 100;
    int knightValue_ = 325;
    int bishopValue_ = 325;
    int rookValue_ = 500;
    int queenValue_ = 975;

    std::array<int, ChessConstants::LAST_PIECE> pieceValues_{};
    int whiteMaterial_ = 0;
    int blackMaterial_ = 0;
    int posScore_ = 0;
    const GamePhase* gamePhase_ = &GamePhase::OPENING;

    tgreiner::amy::common::engine::EvalCache cache_;
    PawnEvalCache pawnCache_;
    int maxPos_ = 150;

    tgreiner::amy::bitboard::BitBoard whitePassedPawns_;
    tgreiner::amy::bitboard::BitBoard blackPassedPawns_;

    std::array<const Table344*, ChessConstants::LAST_PIECE> pieceSquare_{};

    int bishopMobility_ = 3;
    int bishopPair_ = 40;
    int rookMobility_ = 1;
    int rookOnOpenFile_ = 10;
    int rookOnSemiOpenFile_ = 3;
    std::array<int, 8> isolatedPawn_{-7, -8, -9, -10, -10, -9, -8, -7};
    int backwardPawn_ = -10;
    int doubledPawn_ = -7;
    int outsidePassedPawn_ = 150;
};

} // namespace tgreiner::amy::chess::engine
