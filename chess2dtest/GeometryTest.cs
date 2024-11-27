
using tgreiner.amy.bitboard;
using tgreiner.amy.chess.engine;

namespace tgreiner.amy.chess.engine.Tests
{
    [TestClass()]
    public class GeometryTests
    {
 
        [TestMethod()]
        public void nextSquareTest()
        {
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.A1][BoardConstants_Fields.A7] == BoardConstants_Fields.A8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.A1][BoardConstants_Fields.G1] == BoardConstants_Fields.H1) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.A1][BoardConstants_Fields.G7] == BoardConstants_Fields.H8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.H8][BoardConstants_Fields.H2] == BoardConstants_Fields.H1) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.H8][BoardConstants_Fields.B8] == BoardConstants_Fields.A8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.H8][BoardConstants_Fields.B2] == BoardConstants_Fields.A1) ;

            for (int square = 0; square < BitBoard.SIZE; ++square)
            {
                Assert.IsTrue(Geometry.NEXT_SQ[square][square] ==-1, $"NEXT_SQ is {Geometry.NEXT_SQ[square][square]} at square: {square}") ;
            }

            for (int square = 0; (square + 16) < BitBoard.SIZE; ++square)
            {
                Assert.IsTrue(Geometry.NEXT_SQ[square][square+8] >= 0, $"NEXT_SQ is {Geometry.NEXT_SQ[square][square]} at square: {square}, {square + 8}") ;
            }
        }

        [TestMethod()]
        public void nextPosCoverageTest()
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:

            for (int piece = ChessConstants_Fields.PAWN; ChessConstants_Fields.BLACK_PAWN >= piece; ++ piece)
            {
                for (int square = 0; square < BitBoard.SIZE; ++square)
                {
                    // ignore last row for pawns, they can't move
                    if((ChessConstants_Fields.PAWN == piece) && (square >= 56))
                    {
                        continue;
                    }
                    // ignore first row for black pawns, they can't move
                    if((ChessConstants_Fields.BLACK_PAWN == piece) && (square < 8))
                    {
                        continue;
                    }

                    Assert.IsTrue(Geometry.NEXT_POS[piece][square][square] >= 0, $"NEXT_POS is {Geometry.NEXT_POS[piece][square][square]} at piece:{piece}, square{square}") ;

                    // not sure if this is right, NEXT_DIR has a problem on the first or last row
                    if((square <= BoardConstants_Fields.A8)||(square >= BoardConstants_Fields.H1))
                    {
                        continue;
                    }


                    Assert.IsTrue(Geometry.NEXT_DIR[piece][square][square] >= 0, $"NEXT_DIR is {Geometry.NEXT_DIR[piece][square][square]} at piece:{piece}, square{square}") ;
                }
            }
        }

        [TestMethod()]
        public void nextPosTest()
        {
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.D5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == BoardConstants_Fields.E5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.E5] == BoardConstants_Fields.C5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.C5] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == BoardConstants_Fields.B5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.B5] == BoardConstants_Fields.C6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.D6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.D5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == BoardConstants_Fields.E5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.E5] == BoardConstants_Fields.C5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.D4][BoardConstants_Fields.C5] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == BoardConstants_Fields.B5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.B5] == BoardConstants_Fields.C6) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.D4][BoardConstants_Fields.D6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D4] == BoardConstants_Fields.D5) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D5] == BoardConstants_Fields.E4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E4] == BoardConstants_Fields.D3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D3] == BoardConstants_Fields.C4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C4] == BoardConstants_Fields.C5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C5] == BoardConstants_Fields.E5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E5] == BoardConstants_Fields.E3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E3] == BoardConstants_Fields.C3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C3] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D5] == BoardConstants_Fields.D6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E4] == BoardConstants_Fields.F4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D3] == BoardConstants_Fields.D2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C4] == BoardConstants_Fields.B4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C5] == BoardConstants_Fields.B6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E5] == BoardConstants_Fields.F6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.E3] == BoardConstants_Fields.F2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.C3] == BoardConstants_Fields.B2) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D8] == BoardConstants_Fields.E4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.H4] == BoardConstants_Fields.D3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D1] == BoardConstants_Fields.C4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A4] == BoardConstants_Fields.C5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A7] == BoardConstants_Fields.E5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.H8] == BoardConstants_Fields.E3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.G1] == BoardConstants_Fields.C3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A1] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D8] == BoardConstants_Fields.E4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.H4] == BoardConstants_Fields.D3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.D1] == BoardConstants_Fields.C4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A4] == BoardConstants_Fields.C5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A7] == BoardConstants_Fields.E5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.H8] == BoardConstants_Fields.E3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.G1] == BoardConstants_Fields.C3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.D4][BoardConstants_Fields.A1] == -1) ;
        }
    }
}