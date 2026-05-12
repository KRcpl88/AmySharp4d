#include "chess/engine/CheckingMoveGenerator.h"

#include "chess/engine/Geometry.h"
#include "chess/engine/Move.h"

namespace tgreiner::amy::chess::engine {

CheckingMoveGenerator::CheckingMoveGenerator(ChessBoard& theBoard)
    : board_(theBoard) {
}

void CheckingMoveGenerator::generateBRQChecks(tgreiner::amy::common::engine::IMoveList& moves) {
    const auto allPieces = board_.getMask(true) | board_.getMask(false);
    const int oppKing = board_.getKingPos(!board_.getWhiteToMove());
    const auto toSquaresB = Geometry::BISHOP_EPM[oppKing] & ~allPieces;
    const auto toSquaresR = Geometry::ROOK_EPM[oppKing] & ~allPieces;

    generateChecks(moves,
                   board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::BISHOP) |
                       board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::QUEEN),
                   allPieces,
                   toSquaresB,
                   oppKing,
                   false);

    generateChecks(moves,
                   board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::ROOK) |
                       board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::QUEEN),
                   allPieces,
                   toSquaresR,
                   oppKing,
                   false);
}

void CheckingMoveGenerator::generateNChecks(tgreiner::amy::common::engine::IMoveList& moves) {
    const auto allPieces = board_.getMask(true) | board_.getMask(false);
    const int oppKing = board_.getKingPos(!board_.getWhiteToMove());
    const auto toSquares = Geometry::KNIGHT_EPM[oppKing] & ~allPieces;

    generateChecks(moves,
                   board_.getMask(board_.getWhiteToMove(), ChessConstants_Fields::KNIGHT),
                   allPieces,
                   toSquares,
                   oppKing,
                   true);
}

void CheckingMoveGenerator::generateChecks(tgreiner::amy::common::engine::IMoveList& moves,
                                           tgreiner::amy::bitboard::BitBoard pcs,
                                           tgreiner::amy::bitboard::BitBoard allPieces,
                                           tgreiner::amy::bitboard::BitBoard toSquares,
                                           int oppKing,
                                           bool ignoreInterPath) {
    auto tmp = pcs;
    while (!tmp.IsEmpty()) {
        const int from = tmp.findFirstOne();
        tmp.ClearBit(from);

        auto targets = board_.getAttackTo(from) & toSquares;
        while (!targets.IsEmpty()) {
            const int to = targets.findFirstOne();
            targets.ClearBit(to);
            if (ignoreInterPath || (allPieces & Geometry::INTER_PATH[to][oppKing]).IsEmpty()) {
                moves.add(Move::makeMove(from, to));
            }
        }
    }
}

} // namespace tgreiner::amy::chess::engine
