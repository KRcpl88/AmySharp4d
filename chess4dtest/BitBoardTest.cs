namespace tgreiner.amy.bitboard.Tests
{
    [TestClass()]
    public class BitBoardTests
    {
 
        [TestMethod()]
        public void hexLrfTest()
        {
            HexLfr hexLfr = new HexLfr(new Lfr(0,0,0)); // aa1
            Assert.IsTrue((hexLfr.Level == 0) && (hexLfr.Rank == 7) && (hexLfr.File == 0),
                $"Lrf aa1 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // aa8

            hexLfr = new HexLfr(new Lfr(14,0,0)); // oa1
            Assert.IsTrue((hexLfr.Level == 7) && (hexLfr.Rank == 0) && (hexLfr.File == 7),
                $"Lrf oa1 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // hh1

            hexLfr = new HexLfr(new Lfr(7,0,0)); // ha1
            Assert.IsTrue((hexLfr.Level == 0) && (hexLfr.Rank == 0) && (hexLfr.File == 0),
                $"Lrf ha1 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // aa1

            hexLfr = new HexLfr(new Lfr(7,0,7)); // ha8
            Assert.IsTrue((hexLfr.Level == 7) && (hexLfr.Rank == 7) && (hexLfr.File == 0),
                $"Lrf ha8 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // ha8

            hexLfr = new HexLfr(new Lfr(7,7,0)); // hh1
            Assert.IsTrue((hexLfr.Level == 0) && (hexLfr.Rank == 0) && (hexLfr.File == 7),
                $"Lrf hh1 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // ah1

            hexLfr = new HexLfr(new Lfr(7,7,7)); // hh8
            Assert.IsTrue((hexLfr.Level == 7) && (hexLfr.Rank == 7) && (hexLfr.File == 7),
                $"Lrf hh8 => x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}"); // hh8

            // count all LRF squares mapped to each hexLrf, there should be no double counts
            bool[] hexes = new bool[8*8*8];
            Lfr lfr;

            for(int square = 0; square < BitBoard.SIZE; ++square)
            {
                hexLfr = new HexLfr(new Lfr(square));
                Assert.IsFalse(hexes[hexLfr.Level * 64 + hexLfr.Rank * 8 + hexLfr.File],
                    $"Lrf already mapped at offset {square} x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1}");

                hexes[hexLfr.Level * 64 + hexLfr.Rank * 8 + hexLfr.File] = true;
                lfr = (Lfr)hexLfr;

                Assert.IsTrue(square == (int)lfr, 
                    $"Offset {square} x{(char)(97 + hexLfr.Level)}{(char)(97 + hexLfr.File)}{hexLfr.Rank+1} is mapped to {(int)lfr} ({(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1})");
            }

            lfr = (Lfr) (new HexLfr(0,0,0)); // aa1
            Assert.IsTrue((lfr.Level == 7) && (lfr.Rank == 0) && (lfr.File == 0),
                $"HexLrf xaa1 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // ha1

            lfr = (Lfr) (new HexLfr(0,7,0)); // ah1
            Assert.IsTrue((lfr.Level == 7) && (lfr.Rank == 0) && (lfr.File == 7),
                $"HexLrf xah1 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // hh1

            lfr = (Lfr) (new HexLfr(0,0,7)); // aa8
            Assert.IsTrue((lfr.Level == 0) && (lfr.Rank == 0) && (lfr.File == 0),
                $"HexLrf xaa8 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // aa1



            lfr = (Lfr) (new HexLfr(7,0,7)); // ha8
            Assert.IsTrue((lfr.Level == 7) && (lfr.Rank == 7) && (lfr.File == 0),
                $"HexLrf xha8 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // ha8

            lfr = (Lfr) (new HexLfr(7,7,7)); // hh8
            Assert.IsTrue((lfr.Level == 7) && (lfr.Rank == 7) && (lfr.File == 7),
                $"HexLrf xhh8 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // hh8

            lfr = (Lfr) (new HexLfr(7,7,0)); // hh1
            Assert.IsTrue((lfr.Level == 14) && (lfr.Rank == 0) && (lfr.File == 0),
                $"HexLrf xhh1 => {(char)(97 + lfr.Level)}{(char)(97 + lfr.File)}{lfr.Rank+1}"); // oa1

            
        }

        [TestMethod()]
        public void levelRankFileTest()
        {
            int transSquare = 0;
            Lfr lrf;

            lrf = (Lfr)(BoardConstants_Fields.LA);
            Assert.IsTrue(lrf.Level == 0);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
           
            lrf = (Lfr)(BoardConstants_Fields.LB);
            Assert.IsTrue(lrf.Level == 1);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lfr)(BoardConstants_Fields.LC);
            Assert.IsTrue(lrf.Level == 2);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
           
            lrf = (Lfr)(BoardConstants_Fields.LH);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lfr)(BoardConstants_Fields.LI);
            Assert.IsTrue(lrf.Level == 8);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lfr)(BoardConstants_Fields.LO);
            Assert.IsTrue(lrf.Level == 14);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);

            lrf = (Lfr)(BoardConstants_Fields.HA1);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lfr)(BoardConstants_Fields.HH1);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 0);
            Assert.IsTrue(lrf.File == 7);
            
            lrf = (Lfr)(BoardConstants_Fields.HA8);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 7);
            Assert.IsTrue(lrf.File == 0);
            
            lrf = (Lfr)(BoardConstants_Fields.HH8);
            Assert.IsTrue(lrf.Level == 7);
            Assert.IsTrue(lrf.Rank == 7);
            Assert.IsTrue(lrf.File == 7);

            Assert.IsTrue(BoardConstants_Fields.HH8 == (int)lrf);

            Assert.IsFalse(Lfr.IsValid(-1));
            Assert.IsFalse(Lfr.IsValid(BitBoard.SIZE));
            Assert.IsTrue(Lfr.IsValid(0));
            Assert.IsTrue(Lfr.IsValid(BitBoard.SIZE-1));
            
            Assert.IsFalse(Lfr.IsValid(8,7,7));
            Assert.IsFalse(Lfr.IsValid(0,0,2));
            Assert.IsFalse(Lfr.IsValid(0,2,0));
            Assert.IsFalse(Lfr.IsValid(-1,-1,-1));
            Assert.IsFalse(Lfr.IsValid(BitBoard.NUM_LEVELS,0,0));            

            Assert.IsTrue(Lfr.IsValid(0,0,0));
            Assert.IsTrue(Lfr.IsValid(7,0,0));
            Assert.IsTrue(Lfr.IsValid(7,7,7));

            bool caughtException = false;
            try
            {
                lrf = (Lfr)(BitBoard.SIZE);
            }
            catch(IndexOutOfRangeException e)
            {
                caughtException = true;
            }
            Assert.IsTrue(caughtException);

            for(int square = 0; BitBoard.SIZE > square; ++square)
            {
                lrf = (Lfr)square;
                transSquare = BitBoard.BitOffset(lrf.Level, lrf.File, lrf.Rank);
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
