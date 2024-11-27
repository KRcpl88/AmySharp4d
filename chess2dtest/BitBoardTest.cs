namespace tgreiner.amy.bitboard.Tests
{
    [TestClass()]
    public class BitBoardTests
    {
 
        [TestMethod()]
        public void countBitsTest()
        {
           
            Assert.IsTrue(BitBoard.countBits(0L) == 0);

            Assert.IsTrue(BitBoard.countBits(1L) == 1);

            Assert.IsTrue(BitBoard.countBits(7L) == 3);

            Assert.IsTrue(BitBoard.countBits(0xf00f00L) == 8);

            Assert.IsTrue(BitBoard.countBits(0xf000f000L) == 8);

            Assert.IsTrue(BitBoard.countBits(0x70000000L) == 3);

            Assert.IsTrue(BitBoard.countBits(0x77777777L) == 24);

            Assert.IsTrue(BitBoard.countBits(0xffffffffL) == 32);

        }
    }
}
