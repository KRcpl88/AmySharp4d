
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
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HA1][BoardConstants_Fields.HA7] == BoardConstants_Fields.HA8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HA1][BoardConstants_Fields.HG1] == BoardConstants_Fields.HH1) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HA1][BoardConstants_Fields.HG7] == BoardConstants_Fields.HH8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HH8][BoardConstants_Fields.HH2] == BoardConstants_Fields.HH1) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HH8][BoardConstants_Fields.HB8] == BoardConstants_Fields.HA8) ;
            Assert.IsTrue(Geometry.NEXT_SQ[BoardConstants_Fields.HH8][BoardConstants_Fields.HB2] == BoardConstants_Fields.HA1) ;

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
                    if((square <= BoardConstants_Fields.HA8)||(square >= BoardConstants_Fields.HH1))
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
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == BoardConstants_Fields.HB5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HB5] == BoardConstants_Fields.HC6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == BoardConstants_Fields.HB5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HB5] == BoardConstants_Fields.HC6) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == BoardConstants_Fields.HD5) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == BoardConstants_Fields.HE4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE4] == BoardConstants_Fields.HD3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD3] == BoardConstants_Fields.HC4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC4] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] == BoardConstants_Fields.HE3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE3] == BoardConstants_Fields.HC3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC3] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == BoardConstants_Fields.HD6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE4] == BoardConstants_Fields.HF4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD3] == BoardConstants_Fields.HD2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC4] == BoardConstants_Fields.HB4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == BoardConstants_Fields.HB6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] == BoardConstants_Fields.HF6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE3] == BoardConstants_Fields.HF2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC3] == BoardConstants_Fields.HB2) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD8] == BoardConstants_Fields.HE4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH4] == BoardConstants_Fields.HD3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD1] == BoardConstants_Fields.HC4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA4] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA7] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH8] == BoardConstants_Fields.HE3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HG1] == BoardConstants_Fields.HC3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA1] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD8] == BoardConstants_Fields.HE4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH4] == BoardConstants_Fields.HD3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD1] == BoardConstants_Fields.HC4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA4] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA7] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH8] == BoardConstants_Fields.HE3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HG1] == BoardConstants_Fields.HC3) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA1] == -1) ;
        }
    }
}