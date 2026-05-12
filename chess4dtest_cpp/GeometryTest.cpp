#include <CppUnitTest.h>

#include "bitboard/BitBoard.h"
#include "bitboard/BoardConstants.h"
#include "bitboard/LevelRankFile.h"
#include "chess/engine/ChessConstants.h"
#include "chess/engine/Geometry.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using tgreiner::amy::bitboard::BitBoard;
using tgreiner::amy::bitboard::Lfr;
using tgreiner::amy::chess::engine::ChessConstants_Fields;
using tgreiner::amy::chess::engine::Geometry;
namespace BoardConstants = tgreiner::amy::bitboard::BoardConstants;

TEST_CLASS(GeometryTests)
{
public:
    TEST_METHOD(nextSquareTest)
    {
        Assert::AreEqual(BoardConstants::HA8, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HA1][BoardConstants::HA7]));
        Assert::AreEqual(BoardConstants::HH1, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HA1][BoardConstants::HG1]));
        Assert::AreEqual(BoardConstants::HH8, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HA1][BoardConstants::HG7]));
        Assert::AreEqual(BoardConstants::HH1, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HH8][BoardConstants::HH2]));
        Assert::AreEqual(BoardConstants::HA8, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HH8][BoardConstants::HB8]));
        Assert::AreEqual(BoardConstants::HA1, static_cast<int>(Geometry::NEXT_SQ[BoardConstants::HH8][BoardConstants::HB2]));

        for (int square = 0; square < BitBoard::SIZE; ++square)
        {
            Assert::IsTrue(Geometry::NEXT_SQ[square][square] == -1);
        }

        for (int square = 0; (square + 16) < BitBoard::SIZE; ++square)
        {
            Lfr nextSquare(square);
            nextSquare.Rank += 1;

            if (nextSquare.IsValid() && nextSquare.Rank < (BitBoard::LEVEL_WIDTH[nextSquare.Rank] - 1))
            {
                Assert::IsTrue(Geometry::NEXT_SQ[square][static_cast<int>(nextSquare)] >= 0);
            }
        }
    }

    TEST_METHOD(nextPosCoverageTest)
    {
        for (int piece = ChessConstants_Fields::PAWN; piece <= Geometry::BLACK_PAWN; ++piece)
        {
            for (int square = 0; square < BitBoard::SIZE; ++square)
            {
                if (piece == ChessConstants_Fields::PAWN && square >= 56)
                {
                    continue;
                }

                if (piece == Geometry::BLACK_PAWN && square < 8)
                {
                    continue;
                }

                Assert::IsTrue(Geometry::NEXT_POS[piece][square][square] >= 0);

                if (square <= BoardConstants::HA8 || square >= BoardConstants::HH1)
                {
                    continue;
                }

                Assert::IsTrue(Geometry::NEXT_DIR[piece][square][square] >= 0);
            }
        }
    }

    TEST_METHOD(nextPosTest)
    {
        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::PAWN][BoardConstants::HD4][BoardConstants::HD5] == -1);
        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::PAWN][BoardConstants::HD4][BoardConstants::HD4] > 0);
        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::PAWN][BoardConstants::HD4][Geometry::NEXT_POS[ChessConstants_Fields::PAWN][BoardConstants::HD4][BoardConstants::HD4]] > 0);

        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::KNIGHT][BoardConstants::HD4][BoardConstants::HD4] > 0);
        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::KNIGHT][BoardConstants::HD4][BoardConstants::HD6] == -1);

        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::PAWN][BoardConstants::HD4][BoardConstants::HD5] == -1);
        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::PAWN][BoardConstants::HD4][BoardConstants::HD4] > 0);

        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::KNIGHT][BoardConstants::HD4][BoardConstants::HD4] > 0);

        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD4] > 0);
        Assert::IsTrue(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD4] > 0);

        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD5] > 0);
        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE4] > 0);
        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD3] > 0);
        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HC4] > 0);
        Assert::IsTrue(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE5] > 0);

        Assert::AreEqual(BoardConstants::HD6, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD5]));
        Assert::AreEqual(BoardConstants::HF4, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE4]));
        Assert::AreEqual(BoardConstants::HD2, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD3]));
        Assert::AreEqual(BoardConstants::HB4, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HC4]));
        Assert::AreEqual(BoardConstants::HB6, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HC5]));
        Assert::AreEqual(BoardConstants::HF6, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE5]));
        Assert::AreEqual(BoardConstants::HF2, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE3]));
        Assert::AreEqual(BoardConstants::HB2, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HC3]));

        Assert::AreEqual(static_cast<int>(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD5]), static_cast<int>(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD8]));
        Assert::AreEqual(static_cast<int>(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HE4]), static_cast<int>(Geometry::NEXT_DIR[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HH4]));

        Assert::AreEqual(BoardConstants::HD8, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD7]));
        Assert::AreEqual(BoardConstants::HH4, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HG4]));
        Assert::AreEqual(BoardConstants::HD1, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HD2]));
        Assert::AreEqual(BoardConstants::HA4, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HB4]));
        Assert::AreEqual(BoardConstants::HA7, static_cast<int>(Geometry::NEXT_POS[ChessConstants_Fields::QUEEN][BoardConstants::HD4][BoardConstants::HB6]));
    }
};
