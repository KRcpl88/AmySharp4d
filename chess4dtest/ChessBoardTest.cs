
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
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

            Assert.IsTrue(plm.size() == 20, $"King moves: {plm.size()}");
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
        public void parseSquareSanTest()
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

            int move = Move.parseSquareSan(board,"Khf5-hf6");
            var from = Move.getFrom(move);
            Assert.IsTrue(from == BoardConstants_Fields.HF5);
            var to = Move.getTo(move);
            Assert.IsTrue(to == BoardConstants_Fields.HF6);

            /*
            BitBoard pieces = board.getMask(false, ChessConstants_Fields.PAWN);
            int count = 0;
            LFR lfr;
            while (pieces.IsEmpty() == false)
            {
                lfr = (LFR)pieces.findFirstOne();
                switch(count)
                {
                    case 0:
                        Assert.IsTrue(lfr.Level == 7 && lfr.Rank == 3 && lfr.File == 1) ;
                        break;
                    case 1:
                        Assert.IsTrue(lfr.Level == 7 && lfr.Rank == 5 && lfr.File == 4) ;
                        break;
                }
                pieces.ClearBit((int)lfr);
                ++count;
            }
            */

        }

        [TestMethod()]
        public void parseHexSanTest()
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

            var  hexBoard = board.ToStringHex(-1);

            /*

              /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
8|   |   |   |   |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
7  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
6    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
5      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
4        |   |   |   |   |
          \ /f\ /g\ /h\ /
3          |   |   |   |
            \ /g\ /h\ /
2            |   |   |
              \ /h\ /
1              |   |
h               \ / 

    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
8  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
7|   |   |   |   |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
6  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
5    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
4      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
3        |   |   |   |   |
          \ /f\ /g\ /h\ /
2          |   |   |   |
            \ /g\ /h\ /
1            |   |   |
g             \ / \ / 

      /a\ /b\ /c\ /d\ /e\ /f\ 
8    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
7  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
6|   |   |   |   |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
5  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
4    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
3      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
2        |   |   |   |   |
          \ /f\ /g\ /h\ /
1          |   |   |   |
f           \ / \ / \ / 

        /a\ /b\ /c\ /d\ /e\ 
8      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
7    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
6  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
5|   |   |*K*|   |   | K |   |   |  Hashkey: fd7e7324c5f77f41
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
4  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
3    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
2      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
1        |   |   |   |   | *
e         \ / \ / \ / \ / 

          /a\ /b\ /c\ /d\ 
8        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
7      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
6    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
5  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
4|   |*P*|   |   |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
3  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
2    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
1      |   |   |   |   |   |
d       \ / \ / \ / \ / \ / 

            /a\ /b\ /c\ 
8          |   |   |   |
          /a\ /b\ /c\ /d\ 
7        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
6      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
5    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
4  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
3|   |   |   |   |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
2  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
1    |   |   |   |   |   |   |
c     \ / \ / \ / \ / \ / \ / 

              /a\ /b\ 
8            |   |   |
            /a\ /b\ /c\ 
7          |   |   |   |
          /a\ /b\ /c\ /d\ 
6        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
5      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
4    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
3  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
2|   |   |   |   |   |   | P |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
1  |   |   |   |   |   |   |   |
b   \ / \ / \ / \ / \ / \ / \ / 

                /a\ 
8              |   |
              /a\ /b\ 
7            |   |   |
            /a\ /b\ /c\ 
6          |   |   |   |
          /a\ /b\ /c\ /d\ 
5        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
4      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
3    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
2  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
1|   |   |   |   |   |   |   |   |
a \ / \ / \ / \ / \ / \ / \ / \ / 


            */

            Assert.IsTrue(board.WhiteToMove == true);

            int move = Move.parseHexSan(board,"Kef5-ef6");
            var from = Move.getFrom(move);
            Assert.IsTrue(from == BoardConstants_Fields.HF5);
            Assert.IsTrue(from == (int)(SquareLfr)(new HexLfr(4,5,4)));
            var to = Move.getTo(move);
            Assert.IsTrue(to == (int)(SquareLfr)(new HexLfr(4,5,5)));

        }

/*

      /a\ /b\ /c\ /d\ /e\ /f\ 
8    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
7  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
6|   |   |. .|   |. .|   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
5  |   |. .|   |   |. .|   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
4    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
3      |   |. .|. .|   |   |
        \ /e\ /f\ /g\ /h\ /
2        |   |   |   |   |
          \ /f\ /g\ /h\ /
1          |   |   |   |
f           \ / \ / \ / 

        /a\ /b\ /c\ /d\ /e\ 
8      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
7    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
6  |   |. .|   |   |. .|   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
5|   |. .|   |   |   |. .|   |   |  Hashkey: d1e4bacbf27ed539
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
4  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
3    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
2      |   |. .|. .|   |   |
        \ /e\ /f\ /g\ /h\ /
1        |   |   |   |   | *
e         \ / \ / \ / \ / 

          /a\ /b\ /c\ /d\ 
8        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
7      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
6    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
5  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
4|   |   |   | N |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
3  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
2    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
1      |   |   |   |   |   |
d       \ / \ / \ / \ / \ / 

            /a\ /b\ /c\ 
8          |   |   |   |
          /a\ /b\ /c\ /d\ 
7        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
6      |   |. .|. .|   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
5    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
4  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
3|   |. .|   |   |   |. .|   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
2  |   |. .|   |   |. .|   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
1    |   |   |   |   |   |   |
c     \ / \ / \ / \ / \ / \ / 

              /a\ /b\ 
8            |   |   |
            /a\ /b\ /c\ 
7          |   |   |   |
          /a\ /b\ /c\ /d\ 
6        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
5      |   |. .|. .|   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
4    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
3  |   |. .|   |   |. .|   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
2|   |   |. .|   |. .|   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
1  |   |   |   |   |   |   |   |
b   \ / \ / \ / \ / \ / \ / \ / 

*/
        [TestMethod()]
        public void knightAttackTests()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3N/8/8/7k w - -");

            BitBoard pieces = board.getMask(true, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            int squareKing = pieces.findFirstOne();
            SquareLfr lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HA8);

            pieces = board.getMask(false, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            squareKing = pieces.findFirstOne();
            lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HH1);

            pieces = board.getMask(true, ChessConstants_Fields.KNIGHT);
            Assert.IsTrue(pieces.countBits() == 1);


            int squareKnight = pieces.findFirstOne();
            SquareLfr lrfKnight = (SquareLfr) squareKnight;
            Assert.IsTrue(squareKnight == BoardConstants_Fields.HD4);

            String knightAttacks = board.ToStringHex(squareKnight);

            BitBoard moves = board.getAttackTo(squareKnight);

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,4,5))) ==1); //fe6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,4))) ==1); //ff5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,2))) ==1); //ff3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,4,2))) ==1); //fe3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,2,4))) ==1); //fc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,2,5))) ==1); //fc6

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,5))) ==1); //ee6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,5,4))) ==1); //ef5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,5,1))) ==1); //ef2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,1))) ==1); //ee2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,1,4))) ==1); //eb5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,1,5))) ==1); //eb6

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,1,5))) ==1); //cb6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,5))) ==1); //cc6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,5,2))) ==1); //cf3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,5,1))) ==1); //cf2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,1))) ==1); //cc2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,1,2))) ==1); //cb3

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,4))) ==1); //bb5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,2,4))) ==1); //bc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,4,2))) ==1); //be3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,4,1))) ==1); //be2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,2,1))) ==1); //bc2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,4))) ==1); //bb3
        }


/*

  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
8| K |   |   |   |   |   |   |. .|
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
7  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
6    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
5      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
4        |   |   |   |   |
          \ /f\ /g\ /h\ /
3          |   |   |   |
            \ /g\ /h\ /
2            |   |   |
              \ /h\ /
1              |   |
h               \ / 

    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
8  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
7|. .|   |   |   |   |   |. .|   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
6  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
5    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
4      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
3        |   |   |   |   |
          \ /f\ /g\ /h\ /
2          |   |   |   |
            \ /g\ /h\ /
1            |. .|   |
g             \ / \ / 

      /a\ /b\ /c\ /d\ /e\ /f\ 
8    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
7  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
6|   |. .|   |   |   |. .|   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
5  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
4    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
3      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
2        |   |. .|   |   |
          \ /f\ /g\ /h\ /
1          |   |   |   |
f           \ / \ / \ / 

        /a\ /b\ /c\ /d\ /e\ 
8      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
7    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
6  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
5|   |   |. .|   |. .|   |   |   |  Hashkey: d2f2c33345d86a3
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
4  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
3    |   |   |. .|   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
2      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
1        |   |   |   |   | *
e         \ / \ / \ / \ / 

          /a\ /b\ /c\ /d\ 
8        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
7      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
6    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
5  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
4|   |   |   | B |   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
3  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
2    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
1      |   |   |   |   |   |
d       \ / \ / \ / \ / \ / 

            /a\ /b\ /c\ 
8          |   |   |   |
          /a\ /b\ /c\ /d\ 
7        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
6      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
5    |   |   |. .|   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
4  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
3|   |   |. .|   |. .|   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
2  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
1    |   |   |   |   |   |   |
c     \ / \ / \ / \ / \ / \ / 

              /a\ /b\ 
8            |   |   |
            /a\ /b\ /c\ 
7          |   |   |   |
          /a\ /b\ /c\ /d\ 
6        |   |. .|   |   |
        /a\ /b\ /c\ /d\ /e\ 
5      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
4    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
3  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
2|   |. .|   |   |   |. .|   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
1  |   |   |   |   |   |   |   |
b   \ / \ / \ / \ / \ / \ / \ / 

                /a\ 
8              |   |
              /a\ /b\ 
7            |. .|   |
            /a\ /b\ /c\ 
6          |   |   |   |
          /a\ /b\ /c\ /d\ 
5        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
4      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
3    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
2  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
1|. .|   |   |   |   |   |. .|*K*|
a \ / \ / \ / \ / \ / \ / \ / \ / 

*/

        [TestMethod()]
        public void bishopAttackTests()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3B/8/8/7k w - -");

            BitBoard pieces = board.getMask(true, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            int squareKing = pieces.findFirstOne();
            SquareLfr lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HA8);

            pieces = board.getMask(false, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            squareKing = pieces.findFirstOne();
            lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HH1);

            pieces = board.getMask(true, ChessConstants_Fields.BISHOP);
            Assert.IsTrue(pieces.countBits() == 1);


            int squareBishop = pieces.findFirstOne();
            SquareLfr lrfKnight = (SquareLfr) squareBishop;
            Assert.IsTrue(squareBishop == BoardConstants_Fields.HD4);

            String bishopAttacks = board.ToStringHex(squareBishop);

            BitBoard moves = board.getAttackTo(squareBishop);

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,7,7))) ==1); //hh8

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,6))) ==1); //gg7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,0))) ==1); //gg1
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,0,6))) ==1); //ga7

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,5))) ==1); //ff6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,1))) ==1); //ff2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,1,5))) ==1); //fb6

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,4))) ==1); //ee5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,2))) ==1); //ee3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,2,4))) ==1); //ec5

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,4))) ==1); //cc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,4,2))) ==1); //ce3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,4))) ==1); //cc3

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,5))) ==1); //bb6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,5,1))) ==1); //bf2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,1))) ==1); //bb2

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,6))) ==1); //aa7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,6,0))) ==1); //ag1
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,0))) ==1); //aa1

        }

/*
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
8| K |   |   |. .|   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
7  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
6    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
5      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
4        |   |   |   |. .|
          \ /f\ /g\ /h\ /
3          |   |   |   |
            \ /g\ /h\ /
2            |   |   |
              \ /h\ /
1              |   |
h               \ / 

    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
8  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
7|   |   |   |. .|   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
6  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
5    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
4      |. .|   |   |. .|   |
        \ /e\ /f\ /g\ /h\ /
3        |   |   |   |   |
          \ /f\ /g\ /h\ /
2          |   |   |   |
            \ /g\ /h\ /
1            |   |   |
g             \ / \ / 

      /a\ /b\ /c\ /d\ /e\ /f\ 
8    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
7  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
6|   |   |   |. .|   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
5  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
4    |   |. .|   |. .|   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
3      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
2        |   |   |   |   |
          \ /f\ /g\ /h\ /
1          |   |   |   |
f           \ / \ / \ / 

        /a\ /b\ /c\ /d\ /e\ 
8      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
7    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
6  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
5|   |   |   |. .|   |   |   |   |  Hashkey: e4224088b99a4cdc
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
4  |   |   |. .|. .|   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
3    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
2      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
1        |   |   |   |   | *
e         \ / \ / \ / \ / 

          /a\ /b\ /c\ /d\ 
8        |   |   |   |. .|
        /a\ /b\ /c\ /d\ /e\ 
7      |. .|   |   |. .|   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
6    |   |. .|   |. .|   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
5  |   |   |. .|. .|   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
4|. .|. .|. .| R |. .|. .|. .|. .|
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
3  |   |   |. .|. .|   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
2    |   |. .|   |. .|   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
1      |. .|   |   |. .|   |
d       \ / \ / \ / \ / \ / 

            /a\ /b\ /c\ 
8          |   |   |   |
          /a\ /b\ /c\ /d\ 
7        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
6      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
5    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
4  |   |   |. .|. .|   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
3|   |   |   |. .|   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
2  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
1    |   |   |   |   |   |   |
c     \ / \ / \ / \ / \ / \ / 

              /a\ /b\ 
8            |   |   |
            /a\ /b\ /c\ 
7          |   |   |   |
          /a\ /b\ /c\ /d\ 
6        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
5      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
4    |   |. .|   |. .|   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
3  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
2|   |   |   |. .|   |   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
1  |   |   |   |   |   |   |   |
b   \ / \ / \ / \ / \ / \ / \ / 

                /a\ 
8              |   |
              /a\ /b\ 
7            |   |   |
            /a\ /b\ /c\ 
6          |   |   |   |
          /a\ /b\ /c\ /d\ 
5        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
4      |. .|   |   |. .|   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
3    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
2  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
1|   |   |   |. .|   |   |   |*K*|
a \ / \ / \ / \ / \ / \ / \ / \ / 


*/
        [TestMethod()]
        public void rookAttackTests()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3R/8/8/7k w - -");

            BitBoard pieces = board.getMask(true, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            int squareKing = pieces.findFirstOne();
            SquareLfr lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HA8);

            pieces = board.getMask(false, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            squareKing = pieces.findFirstOne();
            lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HH1);

            pieces = board.getMask(true, ChessConstants_Fields.ROOK);
            Assert.IsTrue(pieces.countBits() == 1);

            int squareRook = pieces.findFirstOne();
            SquareLfr lrfRook = (SquareLfr) squareRook;
            Assert.IsTrue(squareRook == BoardConstants_Fields.HD4);

            String rookAttacks = board.ToStringHex(squareRook);

            BitBoard moves = board.getAttackTo(squareRook);
            moves[BoardConstants_Fields.HD4] = 1;

            int count = moves.countBits();

            Assert.AreEqual(count, 41);

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,3,7))) ==1); //hb8
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,7,3))) ==1); //hh4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,3,6))) ==1); //gd7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,3))) ==1); //gg4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,3,3))) ==1); //gd4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,3,5))) ==1); //fd6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,3))) ==1); //ff4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,3,3))) ==1); //fd4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,3,4))) ==1); //ed5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,3))) ==1); //ee4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,3,3))) ==1); //ed4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,0,6))) ==1); //da7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,1,5))) ==1); //db6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,2,4))) ==1); //dc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,3))) ==1); //dd4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,4,2))) ==1); //de3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,5,1))) ==1); //df2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,6,0))) ==1); //dg1  

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,7))) ==1); //dd8
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,6))) ==1); //dd7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,5))) ==1); //dd6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,4))) ==1); //dd5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,2))) ==1); //dd3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,1))) ==1); //dd2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,0))) ==1); //dd1

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,0,3))) ==1); //da4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,1,3))) ==1); //db4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,2,3))) ==1); //dc4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,4,3))) ==1); //de4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,5,3))) ==1); //df4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,6,3))) ==1); //dg4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,7,3))) ==1); //dh4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,3))) ==1); //cc4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,3,3))) ==1); //cd4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,3,2))) ==1); //cd3

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,3))) ==1); //bb4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,3,3))) ==1); //bd4 
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,3,1))) ==1); //bd2

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,3))) ==1); //aa4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,3,3))) ==1); //ad4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,3,0))) ==1); //ad1          
        }

        /*

  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
8| K |   |   |. .|   |   |   |. .|
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
7  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
6    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
5      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
4        |   |   |   |. .|
          \ /f\ /g\ /h\ /
3          |   |   |   |
            \ /g\ /h\ /
2            |   |   |
              \ /h\ /
1              |   |
h               \ / 

    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
8  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
7|. .|   |   |. .|   |   |. .|   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
6  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
5    |   |   |   |   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
4      |. .|   |   |. .|   |
        \ /e\ /f\ /g\ /h\ /
3        |   |   |   |   |
          \ /f\ /g\ /h\ /
2          |   |   |   |
            \ /g\ /h\ /
1            |. .|   |
g             \ / \ / 

      /a\ /b\ /c\ /d\ /e\ /f\ 
8    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
7  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
6|   |. .|   |. .|   |. .|   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
5  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
4    |   |. .|   |. .|   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
3      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
2        |   |. .|   |   |
          \ /f\ /g\ /h\ /
1          |   |   |   |
f           \ / \ / \ / 

        /a\ /b\ /c\ /d\ /e\ 
8      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
7    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
6  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
5|   |   |. .|. .|. .|   |   |   |  Hashkey: 92e9cc1ee0f663be
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
4  |   |   |. .|. .|   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
3    |   |   |. .|   |   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
2      |   |   |   |   |   |
        \ /e\ /f\ /g\ /h\ /
1        |   |   |   |   | *
e         \ / \ / \ / \ / 

          /a\ /b\ /c\ /d\ 
8        |   |   |   |. .|
        /a\ /b\ /c\ /d\ /e\ 
7      |. .|   |   |. .|   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
6    |   |. .|   |. .|   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
5  |   |   |. .|. .|   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
4|. .|. .|. .| Q |. .|. .|. .|. .|
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
3  |   |   |. .|. .|   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
2    |   |. .|   |. .|   |   |
      \ /d\ /e\ /f\ /g\ /h\ /
1      |. .|   |   |. .|   |
d       \ / \ / \ / \ / \ / 

            /a\ /b\ /c\ 
8          |   |   |   |
          /a\ /b\ /c\ /d\ 
7        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
6      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
5    |   |   |. .|   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
4  |   |   |. .|. .|   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
3|   |   |. .|. .|. .|   |   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
2  |   |   |   |   |   |   |   |
    \ /c\ /d\ /e\ /f\ /g\ /h\ /
1    |   |   |   |   |   |   |
c     \ / \ / \ / \ / \ / \ / 

              /a\ /b\ 
8            |   |   |
            /a\ /b\ /c\ 
7          |   |   |   |
          /a\ /b\ /c\ /d\ 
6        |   |. .|   |   |
        /a\ /b\ /c\ /d\ /e\ 
5      |   |   |   |   |   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
4    |   |. .|   |. .|   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
3  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
2|   |. .|   |. .|   |. .|   |   |
  \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
1  |   |   |   |   |   |   |   |
b   \ / \ / \ / \ / \ / \ / \ / 

                /a\ 
8              |   |
              /a\ /b\ 
7            |. .|   |
            /a\ /b\ /c\ 
6          |   |   |   |
          /a\ /b\ /c\ /d\ 
5        |   |   |   |   |
        /a\ /b\ /c\ /d\ /e\ 
4      |. .|   |   |. .|   |
      /a\ /b\ /c\ /d\ /e\ /f\ 
3    |   |   |   |   |   |   |
    /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
2  |   |   |   |   |   |   |   |
  /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\ 
1|. .|   |   |. .|   |   |. .|*K*|
a \ / \ / \ / \ / \ / \ / \ / \ / 

        */

        [TestMethod()]
        public void queenAttackTests()
        {
            var board = new ChessBoard("1/2/2/3/3/3/4/4/4/4/5/5/5/5/5/6/6/6/6/6/6/7/7/7/7/7/7/7/K/8/8/8/3Q/8/8/7k w - -");

            BitBoard pieces = board.getMask(true, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            int squareKing = pieces.findFirstOne();
            SquareLfr lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HA8);

            pieces = board.getMask(false, ChessConstants_Fields.KING);
            Assert.IsTrue(pieces.countBits() == 1);
            squareKing = pieces.findFirstOne();
            lfrKing = (SquareLfr) squareKing;
            Assert.IsTrue(squareKing == BoardConstants_Fields.HH1);

            pieces = board.getMask(true, ChessConstants_Fields.QUEEN);
            Assert.IsTrue(pieces.countBits() == 1);

            int squareQueen = pieces.findFirstOne();
            SquareLfr lrfQueen = (SquareLfr) squareQueen;
            Assert.IsTrue(squareQueen == BoardConstants_Fields.HD4);

            String queenAttacks = board.ToStringHex(squareQueen);

            BitBoard moves = board.getAttackTo(squareQueen);
            moves[BoardConstants_Fields.HD4] = 1;

            int count = moves.countBits();

            Assert.AreEqual(count, 60);


            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,7,7))) ==1); //hh8

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,6))) ==1); //gg7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,0))) ==1); //gg1
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,0,6))) ==1); //ga7

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,5))) ==1); //ff6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,1))) ==1); //ff2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,1,5))) ==1); //fb6

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,4))) ==1); //ee5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,2))) ==1); //ee3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,2,4))) ==1); //ec5

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,4))) ==1); //cc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,4,2))) ==1); //ce3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,4))) ==1); //cc3

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,5))) ==1); //bb6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,5,1))) ==1); //bf2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,1))) ==1); //bb2

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,6))) ==1); //aa7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,6,0))) ==1); //ag1
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,0))) ==1); //aa1

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,3,7))) ==1); //hb8
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(7,7,3))) ==1); //hh4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,3,6))) ==1); //gd7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,6,3))) ==1); //gg4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(6,3,3))) ==1); //gd4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,3,5))) ==1); //fd6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,5,3))) ==1); //ff4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(5,3,3))) ==1); //fd4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,3,4))) ==1); //ed5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,4,3))) ==1); //ee4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(4,3,3))) ==1); //ed4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,0,6))) ==1); //da7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,1,5))) ==1); //db6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,2,4))) ==1); //dc5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,3))) ==1); //dd4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,4,2))) ==1); //de3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,5,1))) ==1); //df2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,6,0))) ==1); //dg1  

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,7))) ==1); //dd8
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,6))) ==1); //dd7
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,5))) ==1); //dd6
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,4))) ==1); //dd5
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,2))) ==1); //dd3
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,1))) ==1); //dd2
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,3,0))) ==1); //dd1

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,0,3))) ==1); //da4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,1,3))) ==1); //db4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,2,3))) ==1); //dc4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,4,3))) ==1); //de4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,5,3))) ==1); //df4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,6,3))) ==1); //dg4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(3,7,3))) ==1); //dh4

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,2,3))) ==1); //cc4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,3,3))) ==1); //cd4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(2,3,2))) ==1); //cd3

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,1,3))) ==1); //bb4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,3,3))) ==1); //bd4 
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(1,3,1))) ==1); //bd2

            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,0,3))) ==1); //aa4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,3,3))) ==1); //ad4
            Assert.IsTrue(moves.GetBit((int)(SquareLfr)(new HexLfr(0,3,0))) ==1); //ad1          

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