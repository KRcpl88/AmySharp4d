
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
            var board = new ChessBoard("/////////////////////////////3r4/pr1Pk1p1/8/7P/6P1/3R3K/5R2 w - -");

            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.PAWN).countBits() == 8) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.PAWN).countBits() == 8) ;
        }
    }
}