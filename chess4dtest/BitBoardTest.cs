namespace tgreiner.amy.bitboard.Tests
{
    [TestClass()]
    public class BitBoardTests
    {
 
        [TestMethod()]
        public void hexLrfTest()
        {
            HexLrf hexLrf = new HexLrf(new Lrf(0,0,0)); // aa1
            Assert.IsTrue((hexLrf.Level == 0) && (hexLrf.Rank == 7) && (hexLrf.File == 0),
                $"Lrf aa1 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // aa8

            hexLrf = new HexLrf(new Lrf(14,0,0)); // oa1
            Assert.IsTrue((hexLrf.Level == 7) && (hexLrf.Rank == 0) && (hexLrf.File == 7),
                $"Lrf oa1 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // hh1

            hexLrf = new HexLrf(new Lrf(7,0,0)); // ha1
            Assert.IsTrue((hexLrf.Level == 0) && (hexLrf.Rank == 0) && (hexLrf.File == 0),
                $"Lrf ha1 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // aa1

            hexLrf = new HexLrf(new Lrf(7,7,0)); // ha8
            Assert.IsTrue((hexLrf.Level == 7) && (hexLrf.Rank == 7) && (hexLrf.File == 0),
                $"Lrf ha8 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // ha8

            hexLrf = new HexLrf(new Lrf(7,0,7)); // hh1
            Assert.IsTrue((hexLrf.Level == 0) && (hexLrf.Rank == 0) && (hexLrf.File == 7),
                $"Lrf hh1 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // ah1

            hexLrf = new HexLrf(new Lrf(7,7,7)); // hh8
            Assert.IsTrue((hexLrf.Level == 7) && (hexLrf.Rank == 7) && (hexLrf.File == 7),
                $"Lrf hh8 => x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}"); // hh8

            // count all LRF squares mapped to each hexLrf, there should be no double counts
            bool[] hexes = new bool[8*8*8];

            for(int square = 0; square < BitBoard.SIZE; ++square)
            {
                hexLrf = new HexLrf(new Lrf(square));
                Assert.IsFalse(hexes[hexLrf.Level * 64 + hexLrf.Rank * 8 + hexLrf.File],
                    $"Lrf already mapped at offset {square} x{(char)(97 + hexLrf.Level)}{(char)(97 + hexLrf.File)}{hexLrf.Rank+1}");

                hexes[hexLrf.Level * 64 + hexLrf.Rank * 8 + hexLrf.File] = true;
            }

            Lrf lrf = (Lrf) (new HexLrf(0,0,0)); // aa1
            Assert.IsTrue((lrf.Level == 7) && (lrf.Rank == 0) && (lrf.File == 0),
                $"HexLrf xaa1 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // ha1

            lrf = (Lrf) (new HexLrf(0,7,0)); // ah1
            Assert.IsTrue((lrf.Level == 7) && (lrf.Rank == 0) && (lrf.File == 7),
                $"HexLrf xah1 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // hh1

            lrf = (Lrf) (new HexLrf(0,7,0)); // aa8
            Assert.IsTrue((lrf.Level == 0) && (lrf.Rank == 0) && (lrf.File == 0),
                $"HexLrf xaa8 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // aa1



            lrf = (Lrf) (new HexLrf(7,7,0)); // ha8
            Assert.IsTrue((lrf.Level == 7) && (lrf.Rank == 7) && (lrf.File == 0),
                $"HexLrf xha8 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // ha8

            lrf = (Lrf) (new HexLrf(7,7,7)); // hh8
            Assert.IsTrue((lrf.Level == 7) && (lrf.Rank == 7) && (lrf.File == 7),
                $"HexLrf xhh8 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // hh8

            lrf = (Lrf) (new HexLrf(7,0,7)); // hh1
            Assert.IsTrue((lrf.Level == 14) && (lrf.Rank == 0) && (lrf.File == 0),
                $"HexLrf xhh1 => {(char)(97 + lrf.Level)}{(char)(97 + lrf.File)}{lrf.Rank+1}"); // oa1
        }

        [TestMethod()]
        public void levelRankFileTest()
        {
            int transSquare = 0;
            Lrf lrf;

            lrf = (Lrf)(BoardConstants_Fields.LA);
            Assert.IsTrue(lrf.Level == 0);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
           
            lrf = (Lrf)(BoardConstants_Fields.LB);
            Assert.IsTrue(lrf.Level == 1);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lrf)(BoardConstants_Fields.LC);
            Assert.IsTrue(lrf.Level == 2);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
           
            lrf = (Lrf)(BoardConstants_Fields.LH);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lrf)(BoardConstants_Fields.LI);
            Assert.IsTrue(lrf.Level == 8);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lrf)(BoardConstants_Fields.LO);
            Assert.IsTrue(lrf.Level == 14);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lrf)(BoardConstants_Fields.HA1);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lrf)(BoardConstants_Fields.HH1);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 7);
            
            lrf = (Lrf)(BoardConstants_Fields.HA8);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 7);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lrf)(BoardConstants_Fields.HH8);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 7);
            Assert.IsTrue(lrf.File == 7);

            Assert.IsTrue(BoardConstants_Fields.HH8 == (int)lrf);

            Assert.IsFalse(Lrf.IsValid(-1));
            Assert.IsFalse(Lrf.IsValid(BitBoard.SIZE));
            Assert.IsTrue(Lrf.IsValid(0));
            Assert.IsTrue(Lrf.IsValid(BitBoard.SIZE-1));
            
            Assert.IsFalse(Lrf.IsValid(8,7,7));
            Assert.IsFalse(Lrf.IsValid(0,0,2));
            Assert.IsFalse(Lrf.IsValid(0,2,0));
            Assert.IsFalse(Lrf.IsValid(-1,-1,-1));
            Assert.IsFalse(Lrf.IsValid(BitBoard.NUM_LEVELS,0,0));            

            Assert.IsTrue(Lrf.IsValid(0,0,0));
            Assert.IsTrue(Lrf.IsValid(7,0,0));
            Assert.IsTrue(Lrf.IsValid(7,7,7));

            bool caughtException = false;
            try
            {
                lrf = (Lrf)(BitBoard.SIZE);
            }
            catch(IndexOutOfRangeException e)
            {
                caughtException = true;
            }
            Assert.IsTrue(caughtException);

            for(int square = 0; BitBoard.SIZE > square; ++square)
            {
                lrf = (Lrf)square;
                transSquare = BitBoard.BitOffset(lrf.Level, lrf.Rank, lrf.File);
                Assert.IsTrue(transSquare == square);
            }
        }

 
        [TestMethod()]
        public void countBitsTest()
        {
            BitBoard test;
            
            test = new BitBoard(new int[]{});
            Assert.IsTrue(test.countBits() == 0);

            test = new BitBoard(new int[]{BitBoard.SIZE-1});
            Assert.IsTrue(test.countBits() == 1);

            test = new BitBoard(new int[]{1,2,3});
            Assert.IsTrue(test.countBits() == 3);

            test = new BitBoard(new int[]{1,2,3,4,5,6,7,8});
            Assert.IsTrue(test.countBits() == 8);

            test = new BitBoard(new int[]{10,20,30,40,50,60,70,80});
            Assert.IsTrue(test.countBits() == 8);

            test = new BitBoard(new int[]{100,200,300});
            Assert.IsTrue(test.countBits() == 3);

            test = new BitBoard(new int[]{110,120,130,140,150,160,170,180,210,220,230,240,250,260,270,280,10,20,30,40,50,60,70,80});
            Assert.IsTrue(test.countBits() == 24);

            test = new BitBoard(new int[]{1,2,3,4,5,6,7,8,10,20,30,40,50,60,70,80,110,120,130,140,150,160,170,180,210,220,230,240,250,260,270,280});
            Assert.IsTrue(test.countBits() == 32);

        }


        [TestMethod()]
        public void findFirstOneTest()
        {
            Assert.IsTrue(sizeof(ulong) == 8);
            Assert.IsTrue((63 / BitBoard.ULONG_SIZE_BITS) == 0);
            Assert.IsTrue((64 / BitBoard.ULONG_SIZE_BITS) == 1);
            var test = new BitBoard();

            for(int i = 0; BitBoard.SIZE > i; ++i)
            {
                test.SetBit(i);
                int result = test.findFirstOne();
                Assert.IsTrue(result == i);
                test.ClearBit(i);
            }

            int tailBitCount = BitBoard.SIZE - (BitBoard.ULONG_SIZE_BITS * (BitBoard.SIZE / BitBoard.ULONG_SIZE_BITS));
            Assert.IsTrue(tailBitCount == 24);

            ulong invalidBitMask = 0xFFFFFFFFFFFFFFFFUL >> tailBitCount;
            Assert.IsTrue(invalidBitMask == 0xFFFFFFFFFFUL);
        }


        [TestMethod()]
        public void ClearInvalidBitsTest()
        {
            int tailBitCount = BitBoard.SIZE - (BitBoard.ULONG_SIZE_BITS * (BitBoard.SIZE / BitBoard.ULONG_SIZE_BITS));
            Assert.IsTrue(tailBitCount == 24);

            ulong invalidBitMask = 0xFFFFFFFFFFFFFFFFUL >> tailBitCount;
            Assert.IsTrue(invalidBitMask == 0xFFFFFFFFFFUL);

            var test = new BitBoard();
            Assert.IsTrue(test.IsEmpty());

            var inverted = ~test;
            Assert.IsFalse(inverted.IsEmpty());

            var doubleInverted = ~inverted;
            Assert.IsTrue(doubleInverted.IsEmpty());

            test[BitBoard.SIZE - 1] = 1;
            Assert.IsTrue(test[BitBoard.SIZE - 1] == 1);
            Assert.IsTrue(test[BitBoard.SIZE - 2] == 0);

            inverted = ~test;
            Assert.IsFalse(inverted.IsEmpty());
            Assert.IsTrue(inverted[BitBoard.SIZE - 1] == 0);
            Assert.IsTrue(inverted[BitBoard.SIZE - 2] == 1);

            doubleInverted = ~inverted;
            Assert.IsFalse(doubleInverted.IsEmpty());
            Assert.IsTrue(doubleInverted[BitBoard.SIZE - 1] == 1);
            Assert.IsTrue(doubleInverted[BitBoard.SIZE - 2] == 0);

        }

    }
}
