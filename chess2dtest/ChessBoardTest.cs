
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
            var board = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.ROOK)) == 2) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.ROOK)) == 2) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.PAWN)) == 8) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.PAWN)) == 8) ;

            board = new ChessBoard("2k2K///r6R w - -");
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.ROOK)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.ROOK)) == 1) ;

            board = new ChessBoard("6k/1r////1R/6K w - -");
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.ROOK)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.ROOK)) == 1) ;
        }

        [TestMethod()]
        public void parseSanTest()
        {
            var board = new ChessBoard("8/8/8/2k2K/1p///6P w - -");
            Assert.IsTrue(board.getPieceAt(BoardConstants_Fields.F5) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.isWhiteAt(BoardConstants_Fields.F5) == true);
            Assert.IsTrue(board.getPieceAt(BoardConstants_Fields.C5) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.isWhiteAt(BoardConstants_Fields.C5) == false);

            /*
            Assert.IsTrue(board.getPieceAt(7,0,6) == ChessConstants_Fields.PAWN);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,0,6)) == Player.white);
            Assert.IsTrue(board.getPieceAt(7,3,1) == ChessConstants_Fields.PAWN);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,3,1)) == Player.black);
            Assert.IsTrue(board.WhiteToMove == true);
            */

            int move = Move.parseSAN(board,"Ke6-f6");
            /*
            BitBoard pieces = board.getMask(false, ChessConstants_Fields.PAWN);
            int count = 0;
            LRF lrf;
            while (pieces.IsEmpty() == false)
            {
                lrf = (LRF)pieces.findFirstOne();
                switch(count)
                {
                    case 0:
                        Assert.IsTrue(lrf.Level == 7 && lrf.Rank == 3 && lrf.File == 1) ;
                        break;
                    case 1:
                        Assert.IsTrue(lrf.Level == 7 && lrf.Rank == 5 && lrf.File == 4) ;
                        break;
                }
                pieces.ClearBit((int)lrf);
                ++count;
            }
            */

        }

        [TestMethod()]
        public void standardBoardLayoutTest()
        {
            var board = new ChessBoard();
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.KING)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.QUEEN)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.QUEEN)) == 1) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.ROOK)) == 2) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.ROOK)) == 2) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(true, ChessConstants_Fields.PAWN)) == 8) ;
            Assert.IsTrue(BitBoard.countBits(board.getMask(false, ChessConstants_Fields.PAWN)) == 8) ;
        }
    }
}