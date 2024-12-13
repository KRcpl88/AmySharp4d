
using tgreiner.amy.bitboard;
using tgreiner.amy.chess.engine;
using tgreiner.amy.common.engine;

namespace tgreiner.amy.chess.engine.Tests
{
    [TestClass()]
    public class ChessBoardTests
    {
        [TestMethod()]
        public void fenTest()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -");
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.ROOK).countBits() == 2) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.ROOK).countBits() == 2) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.PAWN).countBits() == 8) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.PAWN).countBits() == 8) ;

            board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/2k2K///r6R w - -");
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.ROOK).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.ROOK).countBits() == 1) ;

            board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/6k/1r////1R/6K w - -");
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.ROOK).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.ROOK).countBits() == 1) ;
        }

        [TestMethod()]
        public void generateMoveTest()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/8/8/8/2k2K/1p//6P w - -");
            Assert.IsTrue(board.getPieceAt(BoardConstants_Fields.HF5) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.getSideAt(BoardConstants_Fields.HF5) == Player.white);
            Assert.IsTrue(board.getPieceAt(BoardConstants_Fields.HC5) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.getSideAt(BoardConstants_Fields.HC5) == Player.black);

			var plm = new IntVector();
			board.generatePseudoLegalMoves(plm);

            Assert.IsTrue(plm.size() == 10);
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HF6)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG6)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG5)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG4)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HF4)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE4)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE5)));
            Assert.IsTrue(plm.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE6)));

            var moves = new IntVector();

            board.generateLegalMoves(moves);
/*
            Assert.IsTrue(moves.size() == 10);

            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HF6)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG6)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG5)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HG4)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HF4)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE4)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE5)));
            Assert.IsTrue(moves.contains(Move.makeMove(BoardConstants_Fields.HF5, BoardConstants_Fields.HE6)));
*/
        }
        
        [TestMethod()]
        public void parseSanTest()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/8/8/8/2k2K/1p//6P w - -");
            Assert.IsTrue(board.getPieceAt(7,5,4) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,5,4)) == Player.white);
            Assert.IsTrue(board.getPieceAt(7,2,4) == ChessConstants_Fields.KING);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,2,4)) == Player.black);
            Assert.IsTrue(board.getPieceAt(7,6,1) == ChessConstants_Fields.PAWN);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,6,1)) == Player.white);
            Assert.IsTrue(board.getPieceAt(7,1,3) == ChessConstants_Fields.PAWN);
            Assert.IsTrue(board.getSideAt(BitBoard.BitOffset(7,1,3)) == Player.black);
            Assert.IsTrue(board.WhiteToMove == true);

            int move = Move.parseSAN(board,"Khf5-hf6");
            var from = Move.getFrom(move);
            Assert.IsTrue(from == BoardConstants_Fields.HF5);
            var to = Move.getTo(move);
            Assert.IsTrue(to == BoardConstants_Fields.HF6);

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
        public void attackTests()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/8/1r/6R/2k2K/1p//7P w - -");

            BitBoard pieces = board.getMask(false, ChessConstants_Fields.PAWN);
            Assert.IsTrue(pieces.countBits() == 1);
            pieces = board.getMask(true, ChessConstants_Fields.PAWN);
            Assert.IsTrue(pieces.countBits() == 1);
            pieces = board.getMask(false, ChessConstants_Fields.ROOK);
            Assert.IsTrue(pieces.countBits() == 1);
            pieces = board.getMask(true, ChessConstants_Fields.ROOK);
            Assert.IsTrue(pieces.countBits() == 1);


            int squareWhiteRook = pieces.findFirstOne();
            Lfr lrfWhiteRook = (Lfr) squareWhiteRook;
            Assert.IsTrue(squareWhiteRook == BoardConstants_Fields.HG6);

            BitBoard moves = board.getAttackTo(squareWhiteRook);
            moves.SetBit(BoardConstants_Fields.HG6);

            for (int i = 0; 8 > i; ++i)
            {
                Assert.IsTrue(moves.GetBit(lrfWhiteRook.Level, i, lrfWhiteRook.Rank) == 1);
                Assert.IsTrue(moves.GetBit(lrfWhiteRook.Level, lrfWhiteRook.File, i) == 1);
            }

            while(moves.IsEmpty() == false)
            {
                Lfr square = (Lfr)(moves.findFirstOne());
                Assert.IsTrue(square.Level == lrfWhiteRook.Level);
                Assert.IsTrue((square.Rank == lrfWhiteRook.Rank) || (square.File == lrfWhiteRook.File));
            }
        }


        [TestMethod()]
        public void standardBoardLayoutTest()
        {
            var board = new ChessBoard();
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.KING).countBits() == 1) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.QUEEN).countBits() == 2) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.QUEEN).countBits() == 2) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.ROOK).countBits() == 4) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.ROOK).countBits() == 4) ;
            Assert.IsTrue(board.getMask(true, ChessConstants_Fields.PAWN).countBits() == 28) ;
            Assert.IsTrue(board.getMask(false, ChessConstants_Fields.PAWN).countBits() == 28) ;
        }
    }
}