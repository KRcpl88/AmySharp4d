
using tgreiner.amy.bitboard;
using tgreiner.amy.chess.engine;

namespace tgreiner.amy.chess.engine.Tests
{
    [TestClass()]
    public class GeometryTests
    {
 
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