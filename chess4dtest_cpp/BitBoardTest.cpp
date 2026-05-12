#include <CppUnitTest.h>

#include <cstdint>
#include <stdexcept>
#include <utility>
#include <vector>

#include "bitboard/BitBoard.h"
#include "bitboard/BoardConstants.h"
#include "bitboard/HexLevelRankFile.h"
#include "bitboard/LevelRankFile.h"
#include "bitboard/UniversalCoordinate.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using tgreiner::amy::bitboard::BitBoard;
using tgreiner::amy::bitboard::HexLfr;
using tgreiner::amy::bitboard::Lfr;
using tgreiner::amy::bitboard::UCoord;
namespace BoardConstants = tgreiner::amy::bitboard::BoardConstants;

TEST_CLASS(BitBoardTests)
{
public:
    TEST_METHOD(hexLfrTest)
    {
        HexLfr hexLfr(Lfr(0, 0, 0));
        Assert::IsTrue(hexLfr.Level == 0 && hexLfr.Rank == 7 && hexLfr.File == 0);

        hexLfr = HexLfr(Lfr(14, 0, 0));
        Assert::IsTrue(hexLfr.Level == 7 && hexLfr.Rank == 0 && hexLfr.File == 7);

        hexLfr = HexLfr(Lfr(7, 0, 0));
        Assert::IsTrue(hexLfr.Level == 0 && hexLfr.Rank == 0 && hexLfr.File == 0);

        hexLfr = HexLfr(Lfr(7, 0, 7));
        Assert::IsTrue(hexLfr.Level == 7 && hexLfr.Rank == 7 && hexLfr.File == 0);

        hexLfr = HexLfr(Lfr(7, 7, 0));
        Assert::IsTrue(hexLfr.Level == 0 && hexLfr.Rank == 0 && hexLfr.File == 7);

        hexLfr = HexLfr(Lfr(7, 7, 7));
        Assert::IsTrue(hexLfr.Level == 7 && hexLfr.Rank == 7 && hexLfr.File == 7);

        std::vector<bool> hexes(8 * 8 * 8, false);
        for (int square = 0; square < BitBoard::SIZE; ++square)
        {
            hexLfr = HexLfr(Lfr(square));
            const int index = hexLfr.Level * 64 + hexLfr.Rank * 8 + hexLfr.File;
            Assert::IsFalse(hexes[index]);
            hexes[index] = true;

            const Lfr lfr = static_cast<Lfr>(hexLfr);
            Assert::AreEqual(square, static_cast<int>(lfr));
        }

        Lfr lfr = static_cast<Lfr>(HexLfr(0, 0, 0));
        Assert::IsTrue(lfr.Level == 7 && lfr.Rank == 0 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(0, 7, 0));
        Assert::IsTrue(lfr.Level == 7 && lfr.Rank == 0 && lfr.File == 7);

        lfr = static_cast<Lfr>(HexLfr(0, 0, 7));
        Assert::IsTrue(lfr.Level == 0 && lfr.Rank == 0 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(7, 0, 7));
        Assert::IsTrue(lfr.Level == 7 && lfr.Rank == 7 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(7, 7, 7));
        Assert::IsTrue(lfr.Level == 7 && lfr.Rank == 7 && lfr.File == 7);

        lfr = static_cast<Lfr>(HexLfr(7, 7, 0));
        Assert::IsTrue(lfr.Level == 14 && lfr.Rank == 0 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(7, 1, 6));
        Assert::IsTrue(lfr.Level == 8 && lfr.Rank == 6 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(7, 7, 6));
        Assert::IsTrue(lfr.Level == 8 && lfr.Rank == 6 && lfr.File == 6);

        lfr = static_cast<Lfr>(HexLfr(7, 2, 5));
        Assert::IsTrue(lfr.Level == 9 && lfr.Rank == 5 && lfr.File == 0);

        lfr = static_cast<Lfr>(HexLfr(7, 7, 5));
        Assert::IsTrue(lfr.Level == 9 && lfr.Rank == 5 && lfr.File == 5);
    }

    TEST_METHOD(universalCoordinateTest)
    {
        int transSquare = 0;
        UCoord uCoord{Lfr(BoardConstants::HA1)};
        Assert::AreEqual(0, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(7, uCoord.getZ());

        const UCoord c2(0, 7, 7);
        Assert::IsTrue(uCoord == c2);

        uCoord = UCoord{Lfr(BoardConstants::HH1)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(0, uCoord.getY());
        Assert::AreEqual(7, uCoord.getZ());

        uCoord = UCoord{Lfr(BoardConstants::HA8)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(14, uCoord.getY());
        Assert::AreEqual(7, uCoord.getZ());

        uCoord = UCoord{Lfr(BoardConstants::HH8)};
        Assert::AreEqual(14, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(7, uCoord.getZ());

        uCoord = UCoord{Lfr(BoardConstants::HA1)};
        Assert::AreEqual(0, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(7, uCoord.getZ());

        uCoord = UCoord{Lfr(6, 0, 0)};
        Assert::AreEqual(1, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(6, uCoord.getZ());

        uCoord = UCoord{Lfr(6, 0, 6)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(13, uCoord.getY());
        Assert::AreEqual(6, uCoord.getZ());

        uCoord = UCoord{Lfr(6, 6, 6)};
        Assert::AreEqual(13, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(6, uCoord.getZ());

        uCoord = UCoord{Lfr(6, 6, 0)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(1, uCoord.getY());
        Assert::AreEqual(6, uCoord.getZ());

        uCoord = UCoord{Lfr(BoardConstants::LA)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(0, uCoord.getZ());

        uCoord = UCoord{Lfr(BoardConstants::LO)};
        Assert::AreEqual(7, uCoord.getX());
        Assert::AreEqual(7, uCoord.getY());
        Assert::AreEqual(14, uCoord.getZ());

        for (int square = 0; square < BitBoard::SIZE; ++square)
        {
            uCoord = UCoord{Lfr(square)};
            transSquare = static_cast<int>(static_cast<Lfr>(uCoord));
            Assert::AreEqual(square, transSquare);
        }
    }

    TEST_METHOD(LfrFromUCoordTest)
    {
        UCoord uCoord(0, 7, 7);
        Lfr lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(0, lfr.File);
        Assert::AreEqual(0, lfr.Rank);

        uCoord = UCoord(7, 0, 7);
        lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(7, lfr.File);
        Assert::AreEqual(0, lfr.Rank);

        uCoord = UCoord(7, 14, 7);
        lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(0, lfr.File);
        Assert::AreEqual(7, lfr.Rank);

        uCoord = UCoord(14, 7, 7);
        lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(7, lfr.File);
        Assert::AreEqual(7, lfr.Rank);

        uCoord = UCoord(7, 7, 0);
        lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(0, lfr.Level);
        Assert::AreEqual(0, lfr.File);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(BoardConstants::LA, static_cast<int>(lfr));

        uCoord = UCoord(7, 7, 14);
        lfr = static_cast<Lfr>(uCoord);
        Assert::AreEqual(14, lfr.Level);
        Assert::AreEqual(0, lfr.File);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(BoardConstants::LO, static_cast<int>(lfr));
    }

    TEST_METHOD(levelRankFileTest)
    {
        int transSquare = 0;
        Lfr lfr(BoardConstants::LA);
        Assert::AreEqual(0, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::LB);
        Assert::AreEqual(1, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::LC);
        Assert::AreEqual(2, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::LH);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::LI);
        Assert::AreEqual(8, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::LO);
        Assert::AreEqual(14, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::HA1);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::HH1);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(0, lfr.Rank);
        Assert::AreEqual(7, lfr.File);

        lfr = Lfr(BoardConstants::HA8);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(7, lfr.Rank);
        Assert::AreEqual(0, lfr.File);

        lfr = Lfr(BoardConstants::HH8);
        Assert::AreEqual(7, lfr.Level);
        Assert::AreEqual(7, lfr.Rank);
        Assert::AreEqual(7, lfr.File);

        Assert::AreEqual(BoardConstants::HH8, static_cast<int>(lfr));

        Assert::IsFalse(Lfr::IsValid(-1));
        Assert::IsFalse(Lfr::IsValid(BitBoard::SIZE));
        Assert::IsTrue(Lfr::IsValid(0));
        Assert::IsTrue(Lfr::IsValid(BitBoard::SIZE - 1));

        Assert::IsFalse(Lfr::IsValid(8, 7, 7));
        Assert::IsFalse(Lfr::IsValid(0, 0, 2));
        Assert::IsFalse(Lfr::IsValid(0, 2, 0));
        Assert::IsFalse(Lfr::IsValid(-1, -1, -1));
        Assert::IsFalse(Lfr::IsValid(BitBoard::NUM_LEVELS, 0, 0));

        Assert::IsTrue(Lfr::IsValid(0, 0, 0));
        Assert::IsTrue(Lfr::IsValid(7, 0, 0));
        Assert::IsTrue(Lfr::IsValid(7, 7, 7));

        Assert::AreEqual(0, static_cast<int>(Lfr(0, 0, 0)));
        Assert::AreEqual(1, static_cast<int>(Lfr(1, 0, 0)));
        Assert::AreEqual(2, static_cast<int>(Lfr(1, 1, 0)));
        Assert::AreEqual(3, static_cast<int>(Lfr(1, 0, 1)));
        Assert::AreEqual(4, static_cast<int>(Lfr(1, 1, 1)));
        Assert::AreEqual(5, static_cast<int>(Lfr(2, 0, 0)));
        Assert::AreEqual(6, static_cast<int>(Lfr(2, 1, 0)));
        Assert::AreEqual(7, static_cast<int>(Lfr(2, 2, 0)));
        Assert::AreEqual(8, static_cast<int>(Lfr(2, 0, 1)));
        Assert::AreEqual(9, static_cast<int>(Lfr(2, 1, 1)));
        Assert::AreEqual(10, static_cast<int>(Lfr(2, 2, 1)));
        Assert::AreEqual(11, static_cast<int>(Lfr(2, 0, 2)));
        Assert::AreEqual(12, static_cast<int>(Lfr(2, 1, 2)));
        Assert::AreEqual(13, static_cast<int>(Lfr(2, 2, 2)));

        Assert::ExpectException<std::out_of_range>([] { (void)Lfr(BitBoard::SIZE); });

        for (int square = 0; square < BitBoard::SIZE; ++square)
        {
            lfr = Lfr(square);
            transSquare = BitBoard::BitOffset(lfr.Level, lfr.File, lfr.Rank);
            Assert::AreEqual(square, transSquare);
        }
    }

    TEST_METHOD(countBitsTest)
    {
        const std::vector<std::pair<std::vector<int>, int>> testCases{
            {{}, 0},
            {{BitBoard::SIZE - 1}, 1},
            {{1, 2, 3}, 3},
            {{1, 2, 3, 4, 5, 6, 7, 8}, 8},
            {{10, 20, 30, 40, 50, 60, 70, 80}, 8},
            {{100, 200, 300}, 3},
            {{110, 120, 130, 140, 150, 160, 170, 180, 210, 220, 230, 240, 250, 260, 270, 280, 10, 20, 30, 40, 50, 60, 70, 80}, 24},
            {{1, 2, 3, 4, 5, 6, 7, 8, 10, 20, 30, 40, 50, 60, 70, 80, 110, 120, 130, 140, 150, 160, 170, 180, 210, 220, 230, 240, 250, 260, 270, 280}, 32}};

        for (const auto& [bits, expected] : testCases)
        {
            BitBoard test(bits);
            Assert::AreEqual(expected, test.countBits());
        }
    }

    TEST_METHOD(findFirstOneTest)
    {
        Assert::IsTrue(sizeof(std::uint64_t) == 8);
        Assert::AreEqual(0, 63 / BitBoard::ULONG_SIZE_BITS);
        Assert::AreEqual(1, 64 / BitBoard::ULONG_SIZE_BITS);

        BitBoard test;
        for (int i = 0; i < BitBoard::SIZE; ++i)
        {
            test.SetBit(i);
            Assert::AreEqual(i, test.findFirstOne());
            test.ClearBit(i);
        }

        const int tailBitCount = BitBoard::SIZE - (BitBoard::ULONG_SIZE_BITS * (BitBoard::SIZE / BitBoard::ULONG_SIZE_BITS));
        Assert::AreEqual(24, tailBitCount);

        const std::uint64_t invalidBitMask = 0xFFFFFFFFFFFFFFFFULL >> tailBitCount;
        Assert::IsTrue(invalidBitMask == 0xFFFFFFFFFFULL);
    }

    TEST_METHOD(ClearInvalidBitsTest)
    {
        const int tailBitCount = BitBoard::SIZE - (BitBoard::ULONG_SIZE_BITS * (BitBoard::SIZE / BitBoard::ULONG_SIZE_BITS));
        Assert::AreEqual(24, tailBitCount);

        const std::uint64_t invalidBitMask = 0xFFFFFFFFFFFFFFFFULL >> tailBitCount;
        Assert::IsTrue(invalidBitMask == 0xFFFFFFFFFFULL);

        BitBoard test;
        Assert::IsTrue(test.IsEmpty());

        BitBoard inverted = ~test;
        Assert::IsFalse(inverted.IsEmpty());

        BitBoard doubleInverted = ~inverted;
        Assert::IsTrue(doubleInverted.IsEmpty());

        test.SetBit(BitBoard::SIZE - 1);
        Assert::AreEqual(1, test.GetBit(BitBoard::SIZE - 1));
        Assert::AreEqual(0, test.GetBit(BitBoard::SIZE - 2));

        inverted = ~test;
        Assert::IsFalse(inverted.IsEmpty());
        Assert::AreEqual(0, inverted.GetBit(BitBoard::SIZE - 1));
        Assert::AreEqual(1, inverted.GetBit(BitBoard::SIZE - 2));

        doubleInverted = ~inverted;
        Assert::IsFalse(doubleInverted.IsEmpty());
        Assert::AreEqual(1, doubleInverted.GetBit(BitBoard::SIZE - 1));
        Assert::AreEqual(0, doubleInverted.GetBit(BitBoard::SIZE - 2));
    }
};
