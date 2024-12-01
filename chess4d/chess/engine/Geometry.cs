/*-
* Copyright (c) 2003, 2004 Thorsten Greiner
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in the
*    documentation and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
* FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
* OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
*
* $Id: Geometry.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Encapsulates the geometry of the chessboard.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public sealed class Geometry //: ChessConstants
	{
		/// <summary>Constant for a pawn. </summary>
		public const int WHITE_PAWN = ChessConstants_Fields.WHITE_PAWN;
		/// <summary>Constant for a knight. </summary>
		public const int KNIGHT = ChessConstants_Fields.KNIGHT;
		/// <summary>Constant for a bishop. </summary>
		public const int BISHOP = ChessConstants_Fields.BISHOP;
		/// <summary>Constant for a rook. </summary>
		public const int ROOK = ChessConstants_Fields.ROOK;
		/// <summary>Constant for a queen. </summary>
		public const int QUEEN = ChessConstants_Fields.QUEEN;
		/// <summary>Constant for a king. </summary>
		public const int KING = ChessConstants_Fields.KING;
		/// <summary>Constant for a black pawn. </summary>
		public const int BLACK_PAWN = KING + 1;

		/// <summary>Mark end of Geometry arrays. </summary>
		public const int LAST_PIECE = BLACK_PAWN + 1;


		/// <summary> This class cannot be instantiated.</summary>
		private Geometry()
		{
		}

		private static readonly UCoord[][] ATTACK_DELTA = new UCoord [][]
		{
			// first piece 0 is not valid
			null,
			// WHITE_PAWN
			new UCoord[]
			{
				new UCoord(0, 2, -2),
				new UCoord(0, 2, 2)
			},
			// KNIGHT
			new UCoord[] 
			{
				new UCoord(0, -4, -2),
				new UCoord(0, -4, 2),
				new UCoord(0, -2, 4),
				new UCoord(0, 2, 4),
				new UCoord(0, 4, -2),
				new UCoord(0, 4, 2),
				new UCoord(0, -2, -4),
				new UCoord(0, 2, -4)
			},
			// BISHOP
			new UCoord[] 
			{
				new UCoord(0,  2, 2),
				new UCoord(0,  2, -2),
				new UCoord(0, -2, -2),
				new UCoord(0, -2, 2)
			},
			// ROOK
			new UCoord[] 
			{
				new UCoord(0,  0, 2),
				new UCoord(0,  2, 0),
				new UCoord(0,  0, -2),
				new UCoord(0, -2, 0)
			},
			// QUEEN
			new UCoord[] 
			{
				new UCoord(0,  0, 2),
				new UCoord(0,  2, 2),
				new UCoord(0,  2, 0),
				new UCoord(0,  2, -2),
				new UCoord(0,  0, -2),
				new UCoord(0, -2, -2),
				new UCoord(0, -2, 0),
				new UCoord(0, -2, 2)
			},
			// KING
			new UCoord[] 
			{
				new UCoord(0,  0, 2),
				new UCoord(0,  2, 2),
				new UCoord(0,  2, 0),
				new UCoord(0,  2, -2),
				new UCoord(0,  0, -2),
				new UCoord(0, -2, -2),
				new UCoord(0, -2, 0),
				new UCoord(0, -2, 2)
			},
			// BLACK_PAWN
			new UCoord[] 
			{
				new UCoord(0, -2, -2),
				new UCoord(0, -2, 2)
			}
		};
		
		/// <summary> Next positions indexed by piece type, starting square, current square.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_POS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_POS' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly short[][][] NEXT_POS;
		
		/// <summary> Next directions indexed by piece type, starting square, current square.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_DIR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_DIR' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly short[][][] NEXT_DIR;
		
		/// <summary>Next square on a row, file or diagonal. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_SQ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_SQ' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly short[][] NEXT_SQ;
		
		/// <summary>Bitboard of ray squares. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'RAY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'RAY' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly BitBoard[][] RAY;
		
		/// <summary>Bitboard of squares between two squares. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'INTER_PATH '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'INTER_PATH' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly BitBoard[][] INTER_PATH;
		
		/// <summary>White pawn ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_PAWN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] WHITE_PAWN_EPM;
		
		/// <summary>Black pawn ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_PAWN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] BLACK_PAWN_EPM;
		
		/// <summary>Knight ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KNIGHT_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] KNIGHT_EPM;
		
		/// <summary>Bishop ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BISHOP_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] BISHOP_EPM;
		
		/// <summary>Rook ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ROOK_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] ROOK_EPM;
		
		/// <summary>Queen ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'QUEEN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] QUEEN_EPM;
		
		/// <summary>King ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KING_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly BitBoard[] KING_EPM;
		
		/// <summary>The major directions on a 10x10 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_10'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_10 = new int[]{1, - 1, 10, - 10, 9, - 9, 11, - 11};
		
		/// <summary>The knight move offsets on a 10x10 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'OFFSETS_KNIGHT_10'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] OFFSETS_KNIGHT_10 = new int[]{19, 21, - 19, - 21, 12, 8, - 12, - 8};
		
		/// <summary>The bishop directions on a 10x10 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_BISHOP_10'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_BISHOP_10 = new int[]{9, - 9, 11, - 11};
		
		/// <summary>The rook directions on a 10x10 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_ROOK_10'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_ROOK_10 = new int[]{1, - 1, 10, - 10};
		
		/// <summary>The king move offsets on a 10x10 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'OFFSETS_KING_10'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] OFFSETS_KING_10 = new int[]{- 11, - 10, - 9, - 1, 1, 9, 10, 11};
		
		/// <summary>The knight move offsets on a 16x16 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'OFFSETS_KNIGHT_16'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] OFFSETS_KNIGHT_16 = new int[]{14, 31, 33, 18, - 14, - 31, - 33, - 18};
		
		/// <summary>The king move offsets on a 16x16 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'OFFSETS_KING_16'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] OFFSETS_KING_16 = new int[]{- 1, 15, 16, 17, 1, - 17, - 16, - 15};
		
		/// <summary>The bishop move offsets on a 16x16 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_BISHOP_16'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_BISHOP_16 = new int[]{15, 17, - 15, - 17};
		
		/// <summary>The rook move offsets on a 16x16 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_ROOK_16'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_ROOK_16 = new int[]{16, 1, - 16, - 1};
		
		/// <summary>The queen move offsets on a 16x16 board. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'DIRS_QUEEN_16'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] DIRS_QUEEN_16 = new int[]{16, 1, - 16, - 1, 15, 17, - 15, - 17};
		
		/// <summary>Constant for 0x88 algorithm. </summary>
		private const int OX88 = 0x88;
		
		/// <summary> Flip a square along the X-axis. Example: the square a1 becomes
		/// a8, e4 becomes e5.
		/// 
		/// </summary>
		/// <param name="square">the square.
		/// </param>
		/// <returns> the square flipped along the X-axis.
		/// </returns>
		public static int flipX(int square)
		{
			return square ^ 0x38;
		}
		
		/// <summary> Initialize nextPos/nextDir arrays.
		/// 
		/// </summary>
		/// <param name="dirs">an integer array of directions
		/// </param>
		/// <param name="nextPos">the nextPos array
		/// </param>
		/// <param name="nextDir">the nextDir array
		/// </param>
		/// <param name="conv">array the translate from 16x16 board to 8x8 board
		/// </param>
		private static void  initSlidingNextPos(int piece)
		{
			for (int square = 0; square < BitBoard.SIZE; square++)
            {

                for (short direction = 0; direction < ATTACK_DELTA[piece].Length; ++direction)
                {
					var nextCoord = (UCoord)(LRF)square;
					int prevSquare = square;
					UCoord delta;
					long nextDirection = -1;

					if ((direction + 1) < (ATTACK_DELTA[piece].Length ))
					{
						delta = ATTACK_DELTA[piece][direction+1];
						nextDirection = (int)(delta + nextCoord);
					}

					delta = ATTACK_DELTA[piece][direction];

					prevSquare = (int)nextCoord;
					NEXT_DIR[piece][square][prevSquare] = (short)nextDirection;

                    while (LRF.IsValid(nextCoord.Level, nextCoord.Rank, nextCoord.File))
                    {
                        NEXT_POS[piece][square][prevSquare] = (short)nextCoord;

						if((piece == QUEEN) && (square != prevSquare))
						{
							// update NEXT_SQ too
	                        NEXT_SQ[square][prevSquare] = NEXT_POS[piece][square][prevSquare];
						}

                        prevSquare = (int)nextCoord;
						nextCoord += delta;

                        NEXT_DIR[piece][square][prevSquare] = (short)nextDirection;
                    }
                }
            }
		}
		
		/// <summary> Initialize the data structures for move generation.</summary>
		private static void  initMoves()
        {
            // conversion array to convert offset into 16x16 board array to an 8x8 board 
            int square, square2;
            int piece;

            for (square = 0; square < BitBoard.SIZE; square++)
            {
                for (square2 = 0; square2 < BitBoard.SIZE; square2++)
                {
                    for (piece = WHITE_PAWN; piece <= BLACK_PAWN; piece++)
                    {
                        NEXT_POS[piece][square][square2] = -1;
                        NEXT_DIR[piece][square][square2] = -1;
                    }
                    NEXT_SQ[square][square2] = -1;
                }
            }

            /*
			* Pawns
			*/

            initNextPos(WHITE_PAWN);
			initNextPos(BLACK_PAWN);

            /*
			* Knight
			*/

			initNextPos(KNIGHT);

            /*
			* King
			*/
			initNextPos(KING);

            initSlidingNextPos(BISHOP);
            initSlidingNextPos(ROOK);
            initSlidingNextPos(QUEEN);
        }

        private static void initNextPos(int piece)
        {
            for (int square = 0; square < BitBoard.SIZE; square++)
            {
                LRF levelRankFile = new LRF(square);
                int prevSquare = square;

                foreach (UCoord delta in ATTACK_DELTA[piece])
                {
					UCoord temp = (UCoord)levelRankFile + delta;
                    if (LRF.IsValid(temp.Level, temp.Rank, temp.File))
                    {
                        LRF nextLrf = (LRF)temp;

                        NEXT_POS[piece][square][prevSquare] = (short)(int)nextLrf;
                        NEXT_DIR[piece][square][prevSquare] = (short)(int)nextLrf;

                        prevSquare = (int)nextLrf;
                    }
                }
            }
        }

        /// <summary> Initializes Geometry Masks.</summary>
        private static void  initGeometry()
		{
			
			bool[] edge = new bool[100];
			int[] trto = new int[100];
			int[] trfr = new int[BitBoard.SIZE];
			int i, j, k, l;
			
			for (i = 0; i < 100; i++)
			{
				edge[i] = false;
				trto[i] = 0;
			}
			for (i = 0; i < BitBoard.SIZE; i++)
			{
				trfr[i] = 0;
			}
			
			for (i = 0; i < 10; i++)
			{
				edge[i] = true;
				edge[90 + i] = true;
				edge[10 * i] = true;
				edge[10 * i + 9] = true;
				
				for (j = 0; j < 10; j++)
				{
					int x = i - 1;
					int y = j - 1;
					if (x >= 0 && y >= 0 && x < 8 && y < 8)
					{
						trto[i + 10 * j] = x + 8 * y;
						trfr[x + 8 * y] = i + 10 * j;
					}
				}
			}
			
			for (i = 0; i < BitBoard.SIZE; i++)
			{
				for (j = 0; j < BitBoard.SIZE; j++)
				{
					INTER_PATH[i][j] = new BitBoard();
					RAY[i][j] = new BitBoard();
				}
				WHITE_PAWN_EPM[i] = new BitBoard();
				BLACK_PAWN_EPM[i] = new BitBoard();
				KNIGHT_EPM[i] = new BitBoard();
				BISHOP_EPM[i] = new BitBoard();
				ROOK_EPM[i] = new BitBoard();
				QUEEN_EPM[i] = new BitBoard();
				KING_EPM[i] = new BitBoard();
			}
			
			for (j = 0; j < 100; j++)
			{
				int x = trto[j];
				if (edge[j])
				{
					continue;
				}
				for (i = 0; i < 8; i++)
				{
					int d = DIRS_10[i];
					for (k = j + d; !edge[k]; k += d)
					{
						int y = trto[k];
						for (l = j + d; l != k; l += d)
						{
							INTER_PATH[x][y].SetBit(trto[l]);
						}
						for (l = k + d; !edge[l]; l += d)
						{
							RAY[x][y].SetBit(trto[l]);
						}
					}
				}
				for (i = 0; i < DIRS_BISHOP_10.Length; i++)
				{
					int d = DIRS_BISHOP_10[i];
					for (k = j + d; !edge[k]; k += d)
					{
						BISHOP_EPM[x].SetBit(trto[k]);
						QUEEN_EPM[x].SetBit(trto[k]);
					}
				}
				for (i = 0; i < DIRS_ROOK_10.Length; i++)
				{
					int d = DIRS_ROOK_10[i];
					for (k = j + d; !edge[k]; k += d)
					{
						ROOK_EPM[x].SetBit(trto[k]);
						QUEEN_EPM[x].SetBit(trto[k]);
					}
				}
				for (i = 0; i < OFFSETS_KNIGHT_10.Length; i++)
				{
					k = j + OFFSETS_KNIGHT_10[i];
					if (k >= 0 && k < 100 && !edge[k])
					{
						KNIGHT_EPM[x].SetBit(trto[k]);
					}
				}
				for (i = 0; i < OFFSETS_KING_10.Length; i++)
				{
					k = j + OFFSETS_KING_10[i];
					if (k >= 0 && k < 100 && !edge[k])
					{
						KING_EPM[x].SetBit(trto[k]);
					}
				}
				if (!edge[j + 9])
				{
					WHITE_PAWN_EPM[x].SetBit(x + 7);
				}
				if (!edge[j + 11])
				{
					WHITE_PAWN_EPM[x].SetBit(x + 9);
				}
				if (!edge[j - 9])
				{
					BLACK_PAWN_EPM[x].SetBit(x - 7);
				}
				if (!edge[j - 11])
				{
					BLACK_PAWN_EPM[x].SetBit(x - 9);
				}
			}
		}
		static Geometry()
		{
			NEXT_POS = new short[LAST_PIECE][][];
			for (int i = 0; i < LAST_PIECE; i++)
			{
				NEXT_POS[i] = new short[BitBoard.SIZE][];
				for (int i2 = 0; i2 < BitBoard.SIZE; i2++)
				{
					NEXT_POS[i][i2] = new short[BitBoard.SIZE];
				}
			}

			NEXT_DIR = new short[LAST_PIECE][][];
			for (int i3 = 0; i3 < 8; i3++)
			{
				NEXT_DIR[i3] = new short[BitBoard.SIZE][];
				for (int i4 = 0; i4 < BitBoard.SIZE; i4++)
				{
					NEXT_DIR[i3][i4] = new short[BitBoard.SIZE];
				}
			}

			NEXT_SQ = new short[BitBoard.SIZE][];
			for (int i5 = 0; i5 < BitBoard.SIZE; i5++)
			{
				NEXT_SQ[i5] = new short[BitBoard.SIZE];
			}

			RAY = new BitBoard[BitBoard.SIZE][];
			for (int i6 = 0; i6 < BitBoard.SIZE; i6++)
			{
				RAY[i6] = BitBoard.CreateArray(BitBoard.SIZE);
			}

			INTER_PATH = new BitBoard[BitBoard.SIZE][];
			for (int i7 = 0; i7 < BitBoard.SIZE; i7++)
			{
				INTER_PATH[i7] = BitBoard.CreateArray(BitBoard.SIZE);
			}

			//ATTACK_DELTA = new short[LAST_PIECE][][];
			WHITE_PAWN_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			BLACK_PAWN_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			KNIGHT_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			BISHOP_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			ROOK_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			QUEEN_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			KING_EPM = BitBoard.CreateArray(BitBoard.SIZE);
			{
				initMoves();
				initGeometry();
			}
		}
	}
}