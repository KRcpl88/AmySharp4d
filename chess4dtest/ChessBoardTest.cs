
using tgreiner.amy.bitboard;
using tgreiner.amy.chess.engine;

namespace tgreiner.amy.chess.engine.Tests
{
    [TestClass()]
    public class ChessBoardTests
    {
 
        [TestMethod()]
        public void fenTest()
        {
            //var board = new ChessBoard("////////////////////////////rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
            var board = new ChessBoard("///////////////////////////////2k2K///r6R w - -");

            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.ROOK).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.ROOK).countBits() == 1) ;
        }
    }
}