namespace tgreiner.amy.bitboard.Tests
{
    [TestClass()]
    public class BitBoardTests
    {
 
        [TestMethod()]
        public void levelRankFileTest()
        {
            int transSquare = 0;
            LRF lrf;
            
            for(int square = 0; BitBoard.SIZE > square; ++square)
            {
                lrf = BitBoard.LevelRankFile(square);
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
