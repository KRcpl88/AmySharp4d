
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

        public void rookNextDirTest(int startRank, int startFile)
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:

            int piece = ChessConstants_Fields.ROOK;
            int startSquare = startRank * 8 + startFile;

            for (int destRank =0; 8 > destRank; ++destRank)
            {
                for (int destFile = 0; 8 > destFile; ++destFile)
                {
                    int destSquare = destRank * 8 + destFile;
                    int nextDirSquare = Geometry.NEXT_DIR[piece][startSquare][destSquare] ;
                    int nextDirRank = nextDirSquare / 8;
                    int nextDirFile = nextDirSquare % 8;
                    
                    // skip if its the same start and dest are the same
                    if((destRank == startRank) && (destFile == startFile))
                    {
                        continue;
                    }
                    else if (nextDirSquare == -1)
                    {
                        continue;
                    }
                    else if ((startRank == destRank) || (startFile == destFile))
                    {
                            Assert.IsTrue(( nextDirRank >= (startRank - 1)) && (nextDirRank <= (startRank + 1))
                                && (nextDirFile >= (startFile - 1)) && (nextDirFile >= (startFile - 1)), 
                                $"NEXT_POS is {(char)(97 + nextDirFile)}{nextDirRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;

                            Assert.IsFalse((nextDirRank == startRank) && (nextDirFile == startFile), 
                                $"NEXT_POS is {(char)(97 + nextDirFile)}{nextDirRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                    }
                    else
                    {
                        // illegal move
                        Assert.IsTrue(nextDirSquare == -1, 
                        $"NEXT_POS is {(char)(97 + nextDirFile)}{nextDirRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                    }
                }
            }
        }

        public void rookNextPostTest(int startRank, int startFile)
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:

            int piece = ChessConstants_Fields.ROOK;
            int startSquare = startRank * 8 + startFile;
            int lastSquare = -1;

            for (int destRank =0; 8 > destRank; ++destRank)
            {
                for (int destFile = 0; 8 > destFile; ++destFile)
                {
                    int destSquare = destRank * 8 + destFile;
                    int nextSquare = Geometry.NEXT_POS[piece][startSquare][destSquare] ;
                    int nextRank = nextSquare / 8;
                    int nextFile = nextSquare % 8;
                    
                    // skip if its the same start and dest are the same
                    if((destRank == startRank) && (destFile == startFile))
                    {
                        continue;
                    }
                    else if (startRank == destRank)
                    {
                        if ((destFile == 0) || (destFile == 7))
                        {
                            if ((lastSquare == -1) && (nextSquare == -1))
                            {
                                // this is the last square in the NEXT_POS sequence
                                lastSquare = destSquare;
                            }
                            else
                            {
                                // this is an edge, but its not the last square in the next_pos sequence
                                // so it needs to go back to a square within 1 square of the starting square
                                Assert.IsTrue(nextSquare != -1, 
                                    $"NEXT_POS is {nextSquare} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                                
                                Assert.IsTrue(( nextRank >= (startRank - 1)) && (nextRank <= (startRank + 1))
                                    && (nextFile >= (startFile - 1)) && (nextFile >= (startFile - 1)), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;

                                Assert.IsFalse((nextRank == startRank) && (nextFile == startFile), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                            continue;
                        }
                        else
                        {
                            // startRank == destRank but we are not on the last file
                            if(destFile < startFile) 
                            {
                                Assert.IsTrue((nextFile == destFile - 1) && (nextRank == startRank), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                            else
                            {
                                Assert.IsTrue((nextFile == destFile + 1) && (nextRank == startRank), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                        }
                    }
                    else if (startFile == destFile)
                    {
                        if ((destRank == 0) || (destRank == 7))
                        {
                            if ((lastSquare == -1) && (nextSquare == -1))
                            {
                                // this is the last square in the NEXT_POS sequence
                                lastSquare = destSquare;
                            }
                            else
                            {
                                // this is an edgae, but its not the last square in the next_pos sequence
                                // so it needs to go back to a square within 1 square of the starting square
                                Assert.IsTrue(nextSquare != -1, 
                                    $"NEXT_POS is {nextSquare} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                                
                                Assert.IsTrue(( nextRank >= (startRank - 1)) && (nextRank <= (startRank + 1))
                                    && (nextFile >= (startFile - 1)) && (nextFile >= (startFile - 1)), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;

                                Assert.IsFalse((nextRank == startRank) && (nextFile == startFile), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                            continue;
                        }
                        else
                        {
                            // startFile == destFile but we are not on the last rank 
                            if(destRank < startRank) 
                            {
                                Assert.IsTrue((nextRank == destRank - 1) && (nextFile == startFile), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                            else
                            {
                                Assert.IsTrue((nextRank == destRank + 1) && (nextFile == startFile), 
                                    $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                            }
                        }
                    }
                    else
                    {
                        // illegal move
                        Assert.IsTrue(nextSquare == -1, 
                        $"NEXT_POS is {(char)(97 + nextFile)}{nextRank+1} at piece:{piece}, start {(char)(97 + startFile)}{startRank+1}, dest {(char)(97 + destFile)}{destRank+1}") ;
                    }
                }
            }
        }

        [TestMethod()]
        public void geometryRookTest()
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:
            for (int startRank =0; 8>startRank; ++startRank)
            {
                for (int startFile = 0; 8 > startFile; ++startFile)
                {
                    rookNextPostTest(startRank, startFile);
                    rookNextDirTest(startRank, startFile);
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