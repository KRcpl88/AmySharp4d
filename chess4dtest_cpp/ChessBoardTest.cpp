#include <CppUnitTest.h>

#include <initializer_list>
#include <string>

#include "bitboard/BitBoard.h"
#include "bitboard/BoardConstants.h"
#include "bitboard/HexLevelRankFile.h"
#include "bitboard/LevelRankFile.h"
#include "chess/engine/ChessBoard.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/Move.h"
#include "common/engine/IntVector.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using tgreiner::amy::bitboard::BitBoard;
using tgreiner::amy::bitboard::HexLfr;
using tgreiner::amy::bitboard::Lfr;
using tgreiner::amy::chess::engine::ChessBoard;
using tgreiner::amy::chess::engine::ChessConstants_Fields;
using tgreiner::amy::chess::engine::Move;
using tgreiner::amy::chess::engine::Player;
using tgreiner::amy::common::engine::IntVector;
namespace BoardConstants = tgreiner::amy::bitboard::BoardConstants;

namespace
{
    struct HexCoord
    {
        int level;
        int file;
        int rank;
    };

    int ToSquare(const HexCoord& coord)
    {
        return static_cast<int>(static_cast<Lfr>(HexLfr(coord.level, coord.file, coord.rank)));
    }

    void AssertHexBits(const BitBoard& moves, std::initializer_list<HexCoord> coords)
    {
        for (const auto& coord : coords)
        {
            Assert::AreEqual(1, moves.GetBit(ToSquare(coord)));
        }
    }
}

TEST_CLASS(ChessBoardTests)
{
public:
    TEST_METHOD(fenTest)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(2, board.getMask(true, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(2, board.getMask(false, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(8, board.getMask(true, ChessConstants_Fields::PAWN).countBits());
        Assert::AreEqual(8, board.getMask(false, ChessConstants_Fields::PAWN).countBits());

        board = ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/2k2K///r6R w - -");
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::ROOK).countBits());

        board = ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/6k/1r////1R/6K w - -");
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::ROOK).countBits());
    }

    TEST_METHOD(generateMoveTest)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/8/8/8/2k2K/1p//6P w - -");
        Assert::AreEqual(ChessConstants_Fields::KING, board.getPieceAt(BoardConstants::HF5));
        Assert::IsTrue(board.getSideAt(BoardConstants::HF5) == Player::white);
        Assert::AreEqual(ChessConstants_Fields::KING, board.getPieceAt(BoardConstants::HC5));
        Assert::IsTrue(board.getSideAt(BoardConstants::HC5) == Player::black);

        IntVector plm;
        board.generatePseudoLegalMoves(plm);

        Assert::AreEqual(20, plm.size());
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HF6)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HG6)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HG5)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HG4)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HF4)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HE4)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HE5)));
        Assert::IsTrue(plm.contains(Move::makeMove(BoardConstants::HF5, BoardConstants::HE6)));

        IntVector moves;
        board.generateLegalMoves(moves);
    }

    TEST_METHOD(parseSanTest)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/8/8/8/2k2K/1p//6P w - -");
        Assert::AreEqual(ChessConstants_Fields::KING, board.getPieceAt(7, 5, 4));
        Assert::IsTrue(board.getSideAt(BitBoard::BitOffset(7, 5, 4)) == Player::white);
        Assert::AreEqual(ChessConstants_Fields::KING, board.getPieceAt(7, 2, 4));
        Assert::IsTrue(board.getSideAt(BitBoard::BitOffset(7, 2, 4)) == Player::black);
        Assert::AreEqual(ChessConstants_Fields::PAWN, board.getPieceAt(7, 6, 1));
        Assert::IsTrue(board.getSideAt(BitBoard::BitOffset(7, 6, 1)) == Player::white);
        Assert::AreEqual(ChessConstants_Fields::PAWN, board.getPieceAt(7, 1, 3));
        Assert::IsTrue(board.getSideAt(BitBoard::BitOffset(7, 1, 3)) == Player::black);
        Assert::IsTrue(board.getWhiteToMove());

        const int move = Move::parseSAN(board, "Khf5-hf6");
        Assert::AreEqual(BoardConstants::HF5, Move::getFrom(move));
        Assert::AreEqual(BoardConstants::HF6, Move::getTo(move));
    }

    TEST_METHOD(knightAttackTests)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3N/8/8/7k w - -");

        Assert::AreEqual(BoardConstants::HA8, board.getMask(true, ChessConstants_Fields::KING).findFirstOne());
        Assert::AreEqual(BoardConstants::HH1, board.getMask(false, ChessConstants_Fields::KING).findFirstOne());

        BitBoard pieces = board.getMask(true, ChessConstants_Fields::KNIGHT);
        Assert::AreEqual(1, pieces.countBits());

        const int squareKnight = pieces.findFirstOne();
        Assert::AreEqual(BoardConstants::HD4, squareKnight);

        const std::string knightAttacks = board.toStringHex(squareKnight);
        (void)knightAttacks;

        const BitBoard moves = board.getAttackTo(squareKnight);
        AssertHexBits(moves, {
            {5, 4, 5}, {5, 5, 4}, {5, 5, 2}, {5, 4, 2}, {5, 2, 4}, {5, 2, 5},
            {4, 4, 5}, {4, 5, 4}, {4, 5, 1}, {4, 4, 1}, {4, 1, 4}, {4, 1, 5},
            {2, 1, 5}, {2, 2, 5}, {2, 5, 2}, {2, 5, 1}, {2, 2, 1}, {2, 1, 2},
            {1, 1, 4}, {1, 2, 4}, {1, 4, 2}, {1, 4, 1}, {1, 2, 1}, {1, 1, 4}
        });
    }

    TEST_METHOD(bishopAttackTests)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3B/8/8/7k w - -");

        Assert::AreEqual(BoardConstants::HA8, board.getMask(true, ChessConstants_Fields::KING).findFirstOne());
        Assert::AreEqual(BoardConstants::HH1, board.getMask(false, ChessConstants_Fields::KING).findFirstOne());

        BitBoard pieces = board.getMask(true, ChessConstants_Fields::BISHOP);
        Assert::AreEqual(1, pieces.countBits());

        const int squareBishop = pieces.findFirstOne();
        Assert::AreEqual(BoardConstants::HD4, squareBishop);

        const std::string bishopAttacks = board.toStringHex(squareBishop);
        (void)bishopAttacks;

        const BitBoard moves = board.getAttackTo(squareBishop);
        AssertHexBits(moves, {
            {7, 7, 7},
            {6, 6, 6}, {6, 6, 0}, {6, 0, 6},
            {5, 5, 5}, {5, 5, 1}, {5, 1, 5},
            {4, 4, 4}, {4, 4, 2}, {4, 2, 4},
            {2, 2, 4}, {2, 4, 2}, {2, 2, 4},
            {1, 1, 5}, {1, 5, 1}, {1, 1, 1},
            {0, 0, 6}, {0, 6, 0}, {0, 0, 0}
        });
    }

    TEST_METHOD(rookAttackTests)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3R/8/8/7k w - -");

        Assert::AreEqual(BoardConstants::HA8, board.getMask(true, ChessConstants_Fields::KING).findFirstOne());
        Assert::AreEqual(BoardConstants::HH1, board.getMask(false, ChessConstants_Fields::KING).findFirstOne());

        BitBoard pieces = board.getMask(true, ChessConstants_Fields::ROOK);
        Assert::AreEqual(1, pieces.countBits());

        const int squareRook = pieces.findFirstOne();
        Assert::AreEqual(BoardConstants::HD4, squareRook);

        const std::string rookAttacks = board.toStringHex(squareRook);
        (void)rookAttacks;

        BitBoard moves = board.getAttackTo(squareRook);
        moves.SetBit(BoardConstants::HD4);

        Assert::AreEqual(41, moves.countBits());
        AssertHexBits(moves, {
            {7, 3, 7}, {7, 7, 3},
            {6, 3, 6}, {6, 6, 3}, {6, 3, 3},
            {5, 3, 5}, {5, 5, 3}, {5, 3, 3},
            {4, 3, 4}, {4, 4, 3}, {4, 3, 3},
            {3, 0, 6}, {3, 1, 5}, {3, 2, 4}, {3, 3, 3}, {3, 4, 2}, {3, 5, 1}, {3, 6, 0},
            {3, 3, 7}, {3, 3, 6}, {3, 3, 5}, {3, 3, 4}, {3, 3, 2}, {3, 3, 1}, {3, 3, 0},
            {3, 0, 3}, {3, 1, 3}, {3, 2, 3}, {3, 4, 3}, {3, 5, 3}, {3, 6, 3}, {3, 7, 3},
            {2, 2, 3}, {2, 3, 3}, {2, 3, 2},
            {1, 1, 3}, {1, 3, 3}, {1, 3, 1},
            {0, 0, 3}, {0, 3, 3}, {0, 3, 0}
        });
    }

    TEST_METHOD(queenAttackTests)
    {
        ChessBoard board("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3Q/8/8/7k w - -");

        Assert::AreEqual(BoardConstants::HA8, board.getMask(true, ChessConstants_Fields::KING).findFirstOne());
        Assert::AreEqual(BoardConstants::HH1, board.getMask(false, ChessConstants_Fields::KING).findFirstOne());

        BitBoard pieces = board.getMask(true, ChessConstants_Fields::QUEEN);
        Assert::AreEqual(1, pieces.countBits());

        const int squareQueen = pieces.findFirstOne();
        Assert::AreEqual(BoardConstants::HD4, squareQueen);

        const std::string queenAttacks = board.toStringHex(squareQueen);
        (void)queenAttacks;

        BitBoard moves = board.getAttackTo(squareQueen);
        moves.SetBit(BoardConstants::HD4);

        Assert::AreEqual(60, moves.countBits());
        AssertHexBits(moves, {
            {7, 7, 7},
            {6, 6, 6}, {6, 6, 0}, {6, 0, 6},
            {5, 5, 5}, {5, 5, 1}, {5, 1, 5},
            {4, 4, 4}, {4, 4, 2}, {4, 2, 4},
            {2, 2, 4}, {2, 4, 2}, {2, 2, 4},
            {1, 1, 5}, {1, 5, 1}, {1, 1, 1},
            {0, 0, 6}, {0, 6, 0}, {0, 0, 0},
            {7, 3, 7}, {7, 7, 3},
            {6, 3, 6}, {6, 6, 3}, {6, 3, 3},
            {5, 3, 5}, {5, 5, 3}, {5, 3, 3},
            {4, 3, 4}, {4, 4, 3}, {4, 3, 3},
            {3, 0, 6}, {3, 1, 5}, {3, 2, 4}, {3, 3, 3}, {3, 4, 2}, {3, 5, 1}, {3, 6, 0},
            {3, 3, 7}, {3, 3, 6}, {3, 3, 5}, {3, 3, 4}, {3, 3, 2}, {3, 3, 1}, {3, 3, 0},
            {3, 0, 3}, {3, 1, 3}, {3, 2, 3}, {3, 4, 3}, {3, 5, 3}, {3, 6, 3}, {3, 7, 3},
            {2, 2, 3}, {2, 3, 3}, {2, 3, 2},
            {1, 1, 3}, {1, 3, 3}, {1, 3, 1},
            {0, 0, 3}, {0, 3, 3}, {0, 3, 0}
        });
    }

    TEST_METHOD(standardBoardLayoutTest)
    {
        ChessBoard board;
        Assert::AreEqual(1, board.getMask(true, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(1, board.getMask(false, ChessConstants_Fields::KING).countBits());
        Assert::AreEqual(2, board.getMask(true, ChessConstants_Fields::QUEEN).countBits());
        Assert::AreEqual(2, board.getMask(false, ChessConstants_Fields::QUEEN).countBits());
        Assert::AreEqual(4, board.getMask(true, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(4, board.getMask(false, ChessConstants_Fields::ROOK).countBits());
        Assert::AreEqual(28, board.getMask(true, ChessConstants_Fields::PAWN).countBits());
        Assert::AreEqual(28, board.getMask(false, ChessConstants_Fields::PAWN).countBits());
    }
};
