namespace tgreiner.amy.bitboard.Tests
{
    [TestClass()]
    public class BitBoardTests
    {
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
