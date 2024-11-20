using Microsoft.VisualStudio.TestTools.UnitTesting;
using tgreiner.amy.bitboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            for (int i = 0; BitBoard.SIZE > i; ++i)
            {
                test.SetBit(i);
                int result = test.findFirstOne();
                Assert.IsTrue(result == i);
                test.ClearBit(i);
            }
        }
    }
}