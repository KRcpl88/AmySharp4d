#include "chess/engine/EvaluatorImpl.h"

#include <algorithm>
#include <cmath>

#include "bitboard/BoardConstants.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/EvalMasks.h"
#include "chess/engine/Geometry.h"

namespace tgreiner::amy::chess::engine {

using tgreiner::amy::bitboard::BitBoard;
using namespace tgreiner::amy::bitboard;

EvaluatorImpl::Table344 EvaluatorImpl::createPieceSquareTable(const Table64& levelHValues)
{
    Table344 result{};
    std::copy(levelHValues.begin(), levelHValues.end(), result.begin() + BoardConstants::LH);
    return result;
}

const EvaluatorImpl::Table344 EvaluatorImpl::ZERO_POS{};

const EvaluatorImpl::Table344 EvaluatorImpl::pawnPos = createPieceSquareTable(Table64{
    0, 0, 0, 0, 0, 0, 0, 0,
    6, 6, 6, -9, -9, 9, 13, 13,
    6, 6, 6, 0, 0, 6, 9, 9,
    0, 0, 13, 19, 19, 13, 0, 0,
    0, 0, 16, 22, 22, 16, 0, 0,
    0, 0, 19, 25, 25, 19, 0, 0,
    16, 19, 22, 28, 28, 22, 19, 16,
    0, 0, 0, 0, 0, 0, 0, 0
});

const EvaluatorImpl::Table344 EvaluatorImpl::knightPos = createPieceSquareTable(Table64{
    -16, -16, -16, -16, -16, -16, -16, -16,
    -16, -3, 6, 6, 6, 6, -3, -16,
    -16, 6, 13, 13, 13, 13, 6, -16,
    -16, 13, 19, 19, 19, 19, 13, -16,
    -13, 13, 19, 25, 25, 19, 13, -13,
    -13, 19, 25, 25, 25, 25, 19, -13,
    -13, 9, 16, 16, 16, 16, 9, -13,
    -13, -13, -13, -13, -13, -13, -13, -13
});

const EvaluatorImpl::Table344 EvaluatorImpl::bishopPos = createPieceSquareTable(Table64{
    6, 6, 6, 6, 6, 6, 6, 6,
    6, 25, 6, 6, 6, 6, 25, 6,
    6, 16, 16, 16, 16, 16, 16, 6,
    16, 25, 28, 34, 34, 28, 25, 16,
    16, 25, 28, 34, 34, 28, 25, 16,
    16, 25, 28, 28, 28, 28, 25, 16,
    16, 25, 25, 25, 25, 25, 25, 16,
    16, 16, 16, 16, 16, 16, 16, 16
});

const EvaluatorImpl::Table344 EvaluatorImpl::rookPos = createPieceSquareTable(Table64{
    0, 9, 13, 22, 22, 13, 9, 0,
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 0, 0, 0, 0,
    13, 13, 13, 13, 13, 13, 13, 13,
    20, 20, 20, 20, 20, 20, 20, 20,
    20, 20, 20, 20, 20, 20, 20, 20
});

const EvaluatorImpl::Table344 EvaluatorImpl::queenPos = createPieceSquareTable(Table64{
    0, 0, 0, 0, 0, 0, 0, 0,
    0, 3, 3, 6, 6, 3, 3, 0,
    0, -10, 6, 6, 6, 6, 3, 0,
    -10, 3, 6, 9, 9, 6, 3, 0,
    0, 3, 6, 9, 9, 6, 3, 0,
    0, 3, 6, 6, 6, 6, 3, 0,
    0, 3, 3, 3, 3, 3, 3, 0,
    0, 0, 0, 0, 0, 0, 0, 0
});

const EvaluatorImpl::Table344 EvaluatorImpl::kingPosOpening = createPieceSquareTable(Table64{
    0, 22, 9, -9, -9, 9, 28, 16,
    -9, -9, -9, -9, -9, -9, -9, -9,
    -22, -22, -22, -22, -22, -22, -22, -22,
    -22, -22, -22, -22, -22, -22, -22, -22,
    -22, -22, -22, -22, -22, -22, -22, -22,
    -28, -28, -28, -28, -28, -28, -28, -28,
    -34, -34, -34, -34, -34, -34, -34, -34,
    -41, -41, -41, -41, -41, -41, -41, -41
});

const EvaluatorImpl::Table344 EvaluatorImpl::kingPosEndgame = createPieceSquareTable(Table64{
    -10, -10, -10, -10, -10, -10, -10, -10,
    -10, 0, 0, 0, 0, 0, 0, -10,
    -10, 0, 10, 10, 10, 10, 0, -10,
    -10, 0, 10, 25, 25, 10, 0, -10,
    -10, 0, 10, 25, 25, 10, 0, -10,
    -10, 0, 10, 10, 10, 10, 0, -10,
    -10, 0, 0, 0, 0, 0, 0, -10,
    -10, -10, -10, -10, -10, -10, -10, -10
});

EvaluatorImpl::EvaluatorImpl(ChessBoard& theBoard)
    : board_(theBoard),
      pieceValues_{0, pawnValue_, knightValue_, bishopValue_, rookValue_, queenValue_, 0},
      pieceSquare_{nullptr, &pawnPos, &knightPos, &bishopPos, &rookPos, &queenPos, &ZERO_POS}
{
}

int EvaluatorImpl::getWhiteMaterial() const
{
    return whiteMaterial_;
}

int EvaluatorImpl::getBlackMaterial() const
{
    return blackMaterial_;
}

void EvaluatorImpl::init()
{
    whiteMaterial_ = pawnValue_ * board_.getMask(true, ChessConstants_Fields::PAWN).countBits()
        + knightValue_ * board_.getMask(true, ChessConstants_Fields::KNIGHT).countBits()
        + bishopValue_ * board_.getMask(true, ChessConstants_Fields::BISHOP).countBits()
        + rookValue_ * board_.getMask(true, ChessConstants_Fields::ROOK).countBits()
        + queenValue_ * board_.getMask(true, ChessConstants_Fields::QUEEN).countBits();

    blackMaterial_ = pawnValue_ * board_.getMask(false, ChessConstants_Fields::PAWN).countBits()
        + knightValue_ * board_.getMask(false, ChessConstants_Fields::KNIGHT).countBits()
        + bishopValue_ * board_.getMask(false, ChessConstants_Fields::BISHOP).countBits()
        + rookValue_ * board_.getMask(false, ChessConstants_Fields::ROOK).countBits()
        + queenValue_ * board_.getMask(false, ChessConstants_Fields::QUEEN).countBits();

    posScore_ = 0;

    determineGamePhase();
    if (*gamePhase_ == GamePhase::OPENING || *gamePhase_ == GamePhase::MIDDLEGAME) {
        pieceSquare_[ChessConstants_Fields::KING] = &kingPosOpening;
    } else {
        pieceSquare_[ChessConstants_Fields::KING] = &kingPosEndgame;
    }

    for (int type = ChessConstants_Fields::PAWN; type <= ChessConstants_Fields::KING; ++type) {
        BitBoard all = board_.getMask(true, type);
        while (!all.IsEmpty()) {
            const int square = all.findFirstOne();
            all.ClearBit(square);
            posScore_ += (*pieceSquare_[type])[square];
        }

        all = board_.getMask(false, type);
        while (!all.IsEmpty()) {
            const int square = all.findFirstOne();
            all.ClearBit(square);
            posScore_ -= (*pieceSquare_[type])[Geometry::invertRank(square)];
        }
    }
}

void EvaluatorImpl::move(int from, int to, int type, bool whiteToMove)
{
    const Table344* pq = pieceSquare_[type];
    if (pq == nullptr) {
        return;
    }

    if (whiteToMove) {
        posScore_ += (*pq)[to] - (*pq)[from];
    } else {
        posScore_ -= ((*pq)[Geometry::invertRank(to)] - (*pq)[Geometry::invertRank(from)]);
    }
}

void EvaluatorImpl::capture(int square, int type, bool whiteToMove)
{
    const Table344* pq = pieceSquare_[type];
    if (whiteToMove) {
        whiteMaterial_ -= pieceValues_[type];
        if (pq != nullptr) {
            posScore_ -= (*pq)[square];
        }
    } else {
        blackMaterial_ -= pieceValues_[type];
        if (pq != nullptr) {
            posScore_ += (*pq)[Geometry::invertRank(square)];
        }
    }
}

void EvaluatorImpl::add(int square, int type, bool whiteToMove)
{
    const Table344* pq = pieceSquare_[type];
    if (whiteToMove) {
        whiteMaterial_ += pieceValues_[type];
        if (pq != nullptr) {
            posScore_ += (*pq)[square];
        }
    } else {
        blackMaterial_ += pieceValues_[type];
        if (pq != nullptr) {
            posScore_ -= (*pq)[Geometry::invertRank(square)];
        }
    }
}

int EvaluatorImpl::evaluate(int alpha, int beta)
{
    if (board_.getWhiteToMove()) {
        return evaluateInternal(alpha, beta);
    }
    return -evaluateInternal(-beta, -alpha);
}

int EvaluatorImpl::evaluateInternal(int alpha, int beta)
{
    if (cache_.probe(board_.getPosHash())) {
        return cache_.getValue();
    }

    int score = whiteMaterial_ - blackMaterial_;
    score += posScore_;

    if ((score + maxPos_) < alpha) {
        return score + maxPos_;
    }
    if ((score - maxPos_) > beta) {
        return score - maxPos_;
    }

    const int fastScore = score;

    score += evalPawns();
    score += evalPassedPawns();
    score += evalKnights();
    score += evalBishops();
    score += evalRooks();
    score += evalQueens();
    score += evalMaterialImbalance();

    if (oppositeColoredBishopsAndPawns()) {
        score = (3 * score) / 4;
    }

    const int delta = std::abs(score - fastScore);
    if (delta > maxPos_) {
        maxPos_ = delta;
    }

    cache_.store(board_.getPosHash(), score);
    return score;
}

int EvaluatorImpl::evalPawns()
{
    if (pawnCache_.probe(board_.getPawnHash())) {
        whitePassedPawns_ = pawnCache_.getWhitePassedPawns();
        blackPassedPawns_ = pawnCache_.getBlackPassedPawns();
        return pawnCache_.getValue();
    }

    int score = 0;

    const BitBoard whitePawns = board_.getMask(true, ChessConstants_Fields::PAWN);
    const BitBoard blackPawns = board_.getMask(false, ChessConstants_Fields::PAWN);

    whitePassedPawns_ = BitBoard();
    blackPassedPawns_ = BitBoard();

    BitBoard mask = whitePawns;
    while (!mask.IsEmpty()) {
        const int square = mask.findFirstOne();
        mask.ClearBit(square);

        if ((whitePawns & EvalMasks::ISOLATED[square]).IsEmpty()) {
            score += isolatedPawn_[static_cast<std::size_t>(square & 7)];
        } else if ((whitePawns & EvalMasks::WHITE_BACKWARD[square]).IsEmpty()) {
            score += backwardPawn_;
        }

        if (!(whitePawns & EvalMasks::WHITE_DOUBLED[square]).IsEmpty()) {
            score += doubledPawn_;
        }

        if ((blackPawns & EvalMasks::WHITE_PASSED[square]).IsEmpty()) {
            whitePassedPawns_.SetBit(square);
        }
    }

    mask = blackPawns;
    while (!mask.IsEmpty()) {
        const int square = mask.findFirstOne();
        mask.ClearBit(square);

        if ((blackPawns & EvalMasks::ISOLATED[square]).IsEmpty()) {
            score -= isolatedPawn_[static_cast<std::size_t>(square & 7)];
        } else if ((blackPawns & EvalMasks::BLACK_BACKWARD[square]).IsEmpty()) {
            score -= backwardPawn_;
        }

        if ((blackPawns & EvalMasks::BLACK_DOUBLED[square]).IsEmpty()) {
            score -= doubledPawn_;
        }

        if ((whitePawns & EvalMasks::BLACK_PASSED[square]).IsEmpty()) {
            blackPassedPawns_.SetBit(square);
        }
    }

    pawnCache_.store(board_.getPawnHash(), score, whitePassedPawns_, blackPassedPawns_);
    return score;
}

int EvaluatorImpl::getScale(bool whiteToMove) const
{
    const int scale = (board_.getMask(whiteToMove, ChessConstants_Fields::KNIGHT) |
        board_.getMask(whiteToMove, ChessConstants_Fields::BISHOP)).countBits()
        + 2 * board_.getMask(whiteToMove, ChessConstants_Fields::ROOK).countBits()
        + 4 * board_.getMask(whiteToMove, ChessConstants_Fields::QUEEN).countBits();

    return 12 - std::min(12, scale);
}

int EvaluatorImpl::evalPassedPawns()
{
    const int wscale = getScale(false);
    const int bscale = getScale(true);

    int score = 0;

    BitBoard mask = whitePassedPawns_;
    while (!mask.IsEmpty()) {
        const int square = mask.findFirstOne();
        mask.ClearBit(square);
        const int rank = (square >> 3) - 1;
        score += rank * wscale;
    }

    mask = blackPassedPawns_;
    while (!mask.IsEmpty()) {
        const int square = mask.findFirstOne();
        mask.ClearBit(square);
        const int rank = 6 - (square >> 3);
        score -= rank * bscale;
    }

    if (board_.getMaskNonPawn().IsEmpty()) {
        outsidePassedPawnId_.probe(board_.getMask(true, ChessConstants_Fields::PAWN), board_.getMask(false, ChessConstants_Fields::PAWN));

        if (!outsidePassedPawnId_.getWhiteOutsidePassedPawns().IsEmpty() && outsidePassedPawnId_.getBlackOutsidePassedPawns().IsEmpty()) {
            score += outsidePassedPawn_;
        }

        if (outsidePassedPawnId_.getWhiteOutsidePassedPawns().IsEmpty() && !outsidePassedPawnId_.getBlackOutsidePassedPawns().IsEmpty()) {
            score -= outsidePassedPawn_;
        }
    }

    return score;
}

int EvaluatorImpl::evalKnights()
{
    return 0;
}

int EvaluatorImpl::evalBishops()
{
    int score = 0;

    BitBoard whiteBishops = board_.getMask(true, ChessConstants_Fields::BISHOP);
    BitBoard blackBishops = board_.getMask(false, ChessConstants_Fields::BISHOP);

    int count = 0;
    while (!whiteBishops.IsEmpty()) {
        const int square = whiteBishops.findFirstOne();
        whiteBishops.ClearBit(square);
        score += bishopMobility_ * (board_.getAttackTo(square).countBits() - 6);
        ++count;
    }
    if (count > 1) {
        score += bishopPair_;
    }

    count = 0;
    while (!blackBishops.IsEmpty()) {
        const int square = blackBishops.findFirstOne();
        blackBishops.ClearBit(square);
        score -= bishopMobility_ * (board_.getAttackTo(square).countBits() - 6);
        ++count;
    }
    if (count > 1) {
        score -= bishopPair_;
    }

    return score;
}

int EvaluatorImpl::evalRooks()
{
    int score = 0;

    BitBoard whiteRooks = board_.getMask(true, ChessConstants_Fields::ROOK);
    BitBoard blackRooks = board_.getMask(false, ChessConstants_Fields::ROOK);

    const BitBoard whitePawns = board_.getMask(true, ChessConstants_Fields::PAWN);
    const BitBoard blackPawns = board_.getMask(false, ChessConstants_Fields::PAWN);

    while (!whiteRooks.IsEmpty()) {
        const int square = whiteRooks.findFirstOne();
        whiteRooks.ClearBit(square);

        score += rookMobility_ * (board_.getAttackTo(square).countBits() - 7);

        const BitBoard& fileMask = EvalMasks::FILE_MASK[static_cast<std::size_t>(square & 7)];
        if ((whitePawns & fileMask).IsEmpty()) {
            if ((blackPawns & fileMask).IsEmpty()) {
                score += rookOnOpenFile_;
            } else {
                score += rookOnSemiOpenFile_;
            }
        }
    }

    while (!blackRooks.IsEmpty()) {
        const int square = blackRooks.findFirstOne();
        blackRooks.ClearBit(square);

        score -= rookMobility_ * (board_.getAttackTo(square).countBits() - 7);

        const BitBoard& fileMask = EvalMasks::FILE_MASK[static_cast<std::size_t>(square & 7)];
        if ((blackPawns & fileMask).IsEmpty()) {
            if ((whitePawns & fileMask).IsEmpty()) {
                score -= rookOnOpenFile_;
            } else {
                score -= rookOnSemiOpenFile_;
            }
        }
    }

    return score;
}

int EvaluatorImpl::evalQueens()
{
    return 0;
}

int EvaluatorImpl::evalMaterialImbalance()
{
    const int wrookCnt = board_.getMask(true, ChessConstants_Fields::ROOK).countBits();
    const int brookCnt = board_.getMask(false, ChessConstants_Fields::ROOK).countBits();
    const int wknightCnt = board_.getMask(true, ChessConstants_Fields::KNIGHT).countBits();
    const int bknightCnt = board_.getMask(false, ChessConstants_Fields::KNIGHT).countBits();
    const int wpawnCnt = board_.getMask(true, ChessConstants_Fields::PAWN).countBits();
    const int bpawnCnt = board_.getMask(false, ChessConstants_Fields::PAWN).countBits();

    return ((wknightCnt * (wpawnCnt - 5) * pawnValue_) >> 4)
        - ((bknightCnt * (bpawnCnt - 5) * pawnValue_) >> 4)
        + ((wrookCnt * (5 - wpawnCnt) * pawnValue_) >> 3)
        - ((brookCnt * (5 - bpawnCnt) * pawnValue_) >> 3);
}

bool EvaluatorImpl::oppositeColoredBishopsAndPawns()
{
    const int matSigWhite = board_.getMaterialSignature(true);
    const int matSigBlack = board_.getMaterialSignature(false);

    if (((matSigWhite | matSigBlack) == 0x05) && ((matSigWhite & 0x04) != 0) && ((matSigBlack & 0x04) != 0)) {
        const BitBoard whiteBishops = board_.getMask(true, ChessConstants_Fields::BISHOP);
        const BitBoard blackBishops = board_.getMask(false, ChessConstants_Fields::BISHOP);

        const int wwb = (whiteBishops & EvalMasks::WHITE_SQUARES).countBits();
        const int wbb = (whiteBishops & EvalMasks::BLACK_SQUARES).countBits();
        const int bwb = (blackBishops & EvalMasks::WHITE_SQUARES).countBits();
        const int bbb = (blackBishops & EvalMasks::BLACK_SQUARES).countBits();

        return (wwb == 0 && bbb == 0) || (wbb == 0 && bwb == 0);
    }

    return false;
}

int EvaluatorImpl::getMaterialValue(int piece) const
{
    return pieceValues_[static_cast<std::size_t>(piece)];
}

void EvaluatorImpl::determineGamePhase()
{
    const GamePhase& phaseW = determineGamePhase(true);
    const GamePhase& phaseB = determineGamePhase(false);

    if (phaseW == GamePhase::ENDGAME && phaseB == GamePhase::ENDGAME) {
        gamePhase_ = &GamePhase::ENDGAME;
    } else {
        const bool wkingCenter = !(board_.getMask(true, ChessConstants_Fields::KING) & EvalMasks::WHITE_KING_IN_CENTER).IsEmpty();
        const bool bkingCenter = !(board_.getMask(false, ChessConstants_Fields::KING) & EvalMasks::BLACK_KING_IN_CENTER).IsEmpty();

        const bool wDeveloped = (((board_.getMask(true, ChessConstants_Fields::KNIGHT) |
            board_.getMask(true, ChessConstants_Fields::BISHOP)) & EvalMasks::RANK_MASK[0]).countBits() == 0);

        const bool bDeveloped = (((board_.getMask(false, ChessConstants_Fields::KNIGHT) |
            board_.getMask(false, ChessConstants_Fields::BISHOP)) & EvalMasks::RANK_MASK[7]).countBits() == 0);

        if (wkingCenter || bkingCenter || !wDeveloped || !bDeveloped) {
            gamePhase_ = &GamePhase::OPENING;
        } else {
            gamePhase_ = &GamePhase::MIDDLEGAME;
        }
    }
}

const GamePhase& EvaluatorImpl::determineGamePhase(bool whiteToMove) const
{
    const int minors = (board_.getMask(whiteToMove, ChessConstants_Fields::KNIGHT) |
        board_.getMask(whiteToMove, ChessConstants_Fields::BISHOP)).countBits();

    const int majors = board_.getMask(whiteToMove, ChessConstants_Fields::ROOK).countBits()
        + 2 * board_.getMask(whiteToMove, ChessConstants_Fields::QUEEN).countBits();

    if (minors >= 2 && majors >= 2) {
        return GamePhase::MIDDLEGAME;
    }
    return GamePhase::ENDGAME;
}

} // namespace tgreiner::amy::chess::engine
