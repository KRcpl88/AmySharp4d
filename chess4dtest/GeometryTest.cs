
using System.Runtime.CompilerServices;
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
                Lrf nextSquare = new Lrf(square);
                nextSquare.Rank += 1;

                if (nextSquare.IsValid() && nextSquare.Rank < (BitBoard.LEVEL_WIDTH[nextSquare.Rank] - 1))
                {
                    Assert.IsTrue(Geometry.NEXT_SQ[square][(int)nextSquare] >= 0, $"NEXT_SQ is {Geometry.NEXT_SQ[square][(int)nextSquare]} at square: {square}, {(int)nextSquare}") ;
                }
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

        public void rookNextDirTest(Lrf start)
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:

            int piece = ChessConstants_Fields.ROOK;
            int startSquare = (int)start;

            Lrf dest = new Lrf
            {
                Level = 7
            };

            for (dest.Rank =0; 8 > dest.Rank; ++dest.Rank)
            {
                for (dest.File = 0; 8 > dest.File; ++dest.File)
                {
                    int nextDirSquare = Geometry.NEXT_DIR[piece][(int)start][((int)dest)];
                    Lrf nextDir = new Lrf();
                    if (Lrf.IsValid(nextDirSquare))
                    {
                        nextDir = (Lrf) nextDirSquare;
                    }

                    // skip if its the same start and dest are the same
                    if((dest.Rank == start.Rank) && (dest.File == start.File))
                    {
                        continue;
                    }
                    else if (nextDirSquare == -1)
                    {
                        continue;
                    }
                    else if ((start.Rank == dest.Rank) || (start.File == dest.File))
                    {
                            Assert.IsTrue(( nextDir.Rank >= (start.Rank - 1)) && (nextDir.Rank <= (start.Rank + 1))
                                && (nextDir.File >= (start.File - 1)) && (nextDir.File >= (start.File - 1)), 
                                $"NEXT_POS is {(char)(97 + nextDir.File)}{nextDir.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;

                            Assert.IsFalse((nextDir.Rank == start.Rank) && (nextDir.File == start.File), 
                                $"NEXT_POS is {(char)(97 + nextDir.File)}{nextDir.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                    }
                    else
                    {
                        // illegal move
                        Assert.IsTrue(nextDirSquare == -1, 
                            $"NEXT_POS is {nextDirSquare} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                    }
                }
            }
        }

        public void rookNextPostTest(Lrf start)
        {
            // make sure NEXT_POS and NEXT_DIR are fully initialized:

            int piece = ChessConstants_Fields.ROOK;
            int lastSquare = -1;
            Lrf dest = new Lrf
            {
                Level = 7
            };

            for (dest.Rank =0; 8 > dest.Rank; ++dest.Rank)
            {
                for (dest.File = 0; 8 > dest.File; ++dest.File)
                {
                    int destSquare = (int)dest;
                    int nextSquare = (Geometry.NEXT_POS[piece][(int)start][destSquare] );
                    Lrf next = new Lrf();
                    if (Lrf.IsValid(nextSquare))
                    {
                        next = (Lrf) nextSquare;
                    }
                    
                    // skip if its the same start and dest are the same
                    if((dest.Rank == start.Rank) && (dest.File == start.File))
                    {
                        continue;
                    }
                    else if (start.Rank == dest.Rank)
                    {
                        if ((dest.File == 0) || (dest.File == 7))
                        {
                            if ((lastSquare == -1) && (-1 == nextSquare))
                            {
                                // this is the last square in the NEXT_POS sequence
                                lastSquare = destSquare;
                            }
                            else
                            {
                                // this is an edge, but its not the last square in the next_pos sequence
                                // so it needs to go back to a square within 1 square of the starting square
                                Assert.IsTrue(Lrf.IsValid(nextSquare), 
                                    $"NEXT_POS is {nextSquare} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                                
                                Assert.IsTrue(( next.Rank >= (start.Rank - 1)) && (next.Rank <= (start.Rank + 1))
                                    && (next.File >= (start.File - 1)) && (next.File >= (start.File - 1)), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;

                                Assert.IsFalse((next.Rank == start.Rank) && (next.File == start.File), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                            continue;
                        }
                        else
                        {
                            // startRank == destRank but we are not on the last file
                            if(dest.File < start.File) 
                            {
                                Assert.IsTrue((next.File == dest.File - 1) && (next.Rank == start.Rank), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                            else
                            {
                                Assert.IsTrue((next.File == dest.File + 1) && (next.Rank == start.Rank), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                        }
                    }
                    else if (start.File == dest.File)
                    {
                        if ((dest.Rank == 0) || (dest.Rank == 7))
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
                                Assert.IsTrue(Lrf.IsValid(nextSquare), 
                                    $"NEXT_POS is {nextSquare} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}, last square was {(char)(97 + ((Lrf)lastSquare).File)}{((Lrf)lastSquare).Rank + 1}") ;
                                
                                Assert.IsTrue(( next.Rank >= (start.Rank - 1)) && (next.Rank <= (start.Rank + 1))
                                    && (next.File >= (start.File - 1)) && (next.File >= (start.File - 1)), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;

                                Assert.IsFalse((next.Rank == start.Rank) && (next.File == start.File), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                            continue;
                        }
                        else
                        {
                            // startFile == destFile but we are not on the last rank 
                            if(dest.Rank < start.Rank) 
                            {
                                Assert.IsTrue((next.Rank == dest.Rank - 1) && (next.File == start.File), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                            else
                            {
                                Assert.IsTrue((next.Rank == dest.Rank + 1) && (next.File == start.File), 
                                    $"NEXT_POS is {(char)(97 + next.File)}{next.Rank+1} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                            }
                        }
                    }
                    else
                    {
                        // illegal move
                        Assert.IsTrue(nextSquare == -1, 
                            $"NEXT_POS is {nextSquare} at piece:{piece}, start {(char)(97 + start.File)}{start.Rank+1}, dest {(char)(97 + dest.File)}{dest.Rank+1}") ;
                    }
                }
            }
        }

        [TestMethod()]
        public void geometryRookTest()
        {
            Lrf start = new Lrf
            {
                Level = 7
            };

            // make sure NEXT_POS and NEXT_DIR are fully initialized:
            for (start.Rank =0; 8>start.Rank; ++start.Rank)
            {
                for (start.File = 0; 8 > start.File; ++start.File)
                {
                    rookNextPostTest(start);
                    rookNextDirTest(start);
                }
            }
        }


        [TestMethod()]
        public void nextPosTest()
        {
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] > 0) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][
                Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4]] > 0 ) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][
                Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][
                    Geometry.NEXT_POS[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4]]] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HB5] > 0) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == -1) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.PAWN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HB5] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.KNIGHT][BoardConstants_Fields.HD4][BoardConstants_Fields.HD6] == -1) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] == -1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD4] > 0) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] > 0) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE4] > 0) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD3] > 0) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC4] > 0) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] > 0) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE3] > 0) ;
            //Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC3] == -1) ;

            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5] == BoardConstants_Fields.HD6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE4] == BoardConstants_Fields.HF4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD3] == BoardConstants_Fields.HD2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC4] == BoardConstants_Fields.HB4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC5] == BoardConstants_Fields.HB6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE5] == BoardConstants_Fields.HF6) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE3] == BoardConstants_Fields.HF2) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HC3] == BoardConstants_Fields.HB2) ;

            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD8] == Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD5]) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH4] == Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HE4]) ;
/*
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD1] == BoardConstants_Fields.HC4) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA4] == BoardConstants_Fields.HC5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA7] == BoardConstants_Fields.HE5) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HH8] == BoardConstants_Fields.HE3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HG1] == BoardConstants_Fields.HC3) ;
            Assert.IsTrue(Geometry.NEXT_DIR[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA1] == -1) ;
*/
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD7] == BoardConstants_Fields.HD8) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HG4] == BoardConstants_Fields.HH4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HD2] == BoardConstants_Fields.HD1) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HB4] == BoardConstants_Fields.HA4) ;
            Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HB6] == BoardConstants_Fields.HA7) ;
            //Assert.IsTrue(Geometry.NEXT_POS[ChessConstants_Fields.QUEEN][BoardConstants_Fields.HD4][BoardConstants_Fields.HA1] == -1) ;
        }
    }
}