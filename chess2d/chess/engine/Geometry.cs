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
using BitBoard = tgreiner.amy.bitboard.BitBoard;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Encapsulates the geometry of the chessboard.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public sealed class Geometry : ChessConstants
	{
		/// <summary> This class cannot be instantiated.</summary>
		private Geometry()
		{
		}
		
		/// <summary> Next positions indexed by piece type, starting square, current square.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_POS '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_POS' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte[][][] NEXT_POS;
		
		/// <summary> Next directions indexed by piece type, starting square, current square.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_DIR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_DIR' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte[][][] NEXT_DIR;
		
		/// <summary>Next square on a row, file or diagonal. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'NEXT_SQ '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'NEXT_SQ' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly sbyte[][] NEXT_SQ;
		
		/// <summary>Bitboard of ray squares. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'RAY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'RAY' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly long[][] RAY;
		
		/// <summary>Bitboard of squares between two squares. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'INTER_PATH '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'INTER_PATH' was moved to static method 'tgreiner.amy.chess.engine.Geometry'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly long[][] INTER_PATH;
		
		/// <summary>White pawn ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_PAWN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] WHITE_PAWN_EPM;
		
		/// <summary>Black pawn ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_PAWN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] BLACK_PAWN_EPM;
		
		/// <summary>Knight ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KNIGHT_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] KNIGHT_EPM;
		
		/// <summary>Bishop ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BISHOP_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] BISHOP_EPM;
		
		/// <summary>Rook ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ROOK_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] ROOK_EPM;
		
		/// <summary>Queen ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'QUEEN_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] QUEEN_EPM;
		
		/// <summary>King ever possible moves. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KING_EPM '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] KING_EPM;
		
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
		/// <param name="sq">the square.
		/// </param>
		/// <returns> the square flipped along the X-axis.
		/// </returns>
		public static int flipX(int sq)
		{
			return sq ^ 0x38;
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
		private static void  initNextPos(int[] dirs, sbyte[][] nextPos, sbyte[][] nextDir, sbyte[] conv)
		{
			
			for (int sq = 0; sq < 128; sq++)
			{
				int dir, nextdir;
				int next;
				bool start = true;
				
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				dir = 0;
				
				while (dir < dirs.Length)
				{
					int next2 = - 1;
					
					next = sq + dirs[dir];
					if ((next & OX88) != 0)
					{
						dir++;
						continue;
					}
					
					nextdir = dir + 1;
					while (nextdir < dirs.Length)
					{
						next2 = sq + dirs[nextdir];
						if (0 == (next2 & OX88))
						{
							break;
						}
						nextdir++;
					}
					
					if (start)
					{
						nextPos[conv[sq]][conv[sq]] = conv[next];
						start = false;
					}
					
					for (; ; )
					{
						int next3 = next + dirs[dir];
						
						if ((next3 & OX88) != 0)
						{
							if (nextdir < dirs.Length)
							{
								nextPos[conv[sq]][conv[next]] = conv[next2];
								nextDir[conv[sq]][conv[next]] = conv[next2];
							}
							break;
						}
						else
						{
							nextPos[conv[sq]][conv[next]] = conv[next3];
							if (nextdir < dirs.Length)
							{
								nextDir[conv[sq]][conv[next]] = conv[next2];
							}
						}
						next = next3;
					}
					dir++;
				}
			}
		}
		
		/// <summary> Initialize the data structures for move generation.</summary>
		private static void  initMoves()
		{
			sbyte[] conv = new sbyte[128];
			
			int sq, sq2;
			int pc;
			
			for (sq = 0; sq < 128; sq++)
			{
				conv[sq] = 127;
			}
			for (sq = 0; sq < 128; sq++)
			{
				if (0 == (sq & OX88))
				{
					sq2 = (sq & 7) | (sq & 0x70) >> 1;
					conv[sq] = (sbyte) sq2;
				}
			}
			
			for (sq = 0; sq < BitBoard.SIZE; sq++)
			{
				for (sq2 = 0; sq2 < BitBoard.SIZE; sq2++)
				{
					for (pc = tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN; pc <= tgreiner.amy.chess.engine.ChessConstants_Fields.BLACK_PAWN; pc++)
					{
						NEXT_POS[pc][sq][sq2] = - 1;
						NEXT_DIR[pc][sq][sq2] = - 1;
					}
					NEXT_SQ[sq][sq2] = - 1;
				}
			}
			
			/*
			* Pawns
			*/
			
			for (sq = 0; sq < 128; sq++)
			{
				int next;
				int next2;
				
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				next = sq + 0x11;
				if (0 == (next & OX88))
				{
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN][conv[sq]][conv[sq]] = conv[next];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN][conv[sq]][conv[sq]] = conv[next];
				}
				else
				{
					next = sq;
				}
				
				next2 = sq + 0x0f;
				if (0 == (next2 & OX88))
				{
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN][conv[sq]][conv[next]] = conv[next2];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN][conv[sq]][conv[next]] = conv[next2];
				}
			}
			
			for (sq = 0; sq < 128; sq++)
			{
				int next;
				int next2;
				
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				next = sq - 0x11;
				if (0 == (next & OX88))
				{
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.BLACK_PAWN][conv[sq]][conv[sq]] = conv[next];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.BLACK_PAWN][conv[sq]][conv[sq]] = conv[next];
				}
				else
				{
					next = sq;
				}
				
				next2 = sq - 0x0f;
				if (0 == (next2 & OX88))
				{
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.BLACK_PAWN][conv[sq]][conv[next]] = conv[next2];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.BLACK_PAWN][conv[sq]][conv[next]] = conv[next2];
				}
			}
			
			/*
			* Knight
			*/
			
			for (sq = 0; sq < 128; sq++)
			{
				int next;
				int i;
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				next = sq;
				
				for (i = 0; i < OFFSETS_KNIGHT_16.Length; i++)
				{
					int next2 = sq + OFFSETS_KNIGHT_16[i];
					if ((next2 & OX88) != 0)
					{
						continue;
					}
					
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT][conv[sq]][conv[next]] = conv[next2];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT][conv[sq]][conv[next]] = conv[next2];
					
					next = next2;
				}
			}
			
			/*
			* King
			*/
			
			for (sq = 0; sq < 128; sq++)
			{
				int next;
				int i;
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				next = sq;
				
				for (i = 0; i < OFFSETS_KING_16.Length; i++)
				{
					int next2 = sq + OFFSETS_KING_16[i];
					if ((next2 & OX88) != 0)
					{
						continue;
					}
					
					NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.KING][conv[sq]][conv[next]] = conv[next2];
					NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.KING][conv[sq]][conv[next]] = conv[next2];
					
					next = next2;
				}
			}
			
			initNextPos(DIRS_BISHOP_16, NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP], NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP], conv);
			initNextPos(DIRS_ROOK_16, NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK], NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK], conv);
			initNextPos(DIRS_QUEEN_16, NEXT_POS[tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN], NEXT_DIR[tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN], conv);
			
			/*
			* Inititialize NEXT_SQ
			*/
			
			for (sq = 0; sq < 128; sq++)
			{
				int dir;
				
				if ((sq & OX88) != 0)
				{
					continue;
				}
				
				for (dir = 0; dir < DIRS_QUEEN_16.Length; dir++)
				{
					int next, next2;
					
					next = sq + DIRS_QUEEN_16[dir];
					if ((next & OX88) != 0)
					{
						continue;
					}
					
					for (; ; )
					{
						next2 = next + DIRS_QUEEN_16[dir];
						if ((next2 & OX88) != 0)
						{
							break;
						}
						NEXT_SQ[conv[sq]][conv[next]] = conv[next2];
						next = next2;
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
					INTER_PATH[i][j] = 0;
					RAY[i][j] = 0;
				}
				WHITE_PAWN_EPM[i] = 0L;
				BLACK_PAWN_EPM[i] = 0L;
				KNIGHT_EPM[i] = 0L;
				BISHOP_EPM[i] = 0L;
				ROOK_EPM[i] = 0L;
				QUEEN_EPM[i] = 0L;
				KING_EPM[i] = 0;
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
							INTER_PATH[x][y] |= BitBoard.SET_MASK[trto[l]];
						}
						for (l = k + d; !edge[l]; l += d)
						{
							RAY[x][y] |= BitBoard.SET_MASK[trto[l]];
						}
					}
				}
				for (i = 0; i < DIRS_BISHOP_10.Length; i++)
				{
					int d = DIRS_BISHOP_10[i];
					for (k = j + d; !edge[k]; k += d)
					{
						BISHOP_EPM[x] |= BitBoard.SET_MASK[trto[k]];
						QUEEN_EPM[x] |= BitBoard.SET_MASK[trto[k]];
					}
				}
				for (i = 0; i < DIRS_ROOK_10.Length; i++)
				{
					int d = DIRS_ROOK_10[i];
					for (k = j + d; !edge[k]; k += d)
					{
						ROOK_EPM[x] |= BitBoard.SET_MASK[trto[k]];
						QUEEN_EPM[x] |= BitBoard.SET_MASK[trto[k]];
					}
				}
				for (i = 0; i < OFFSETS_KNIGHT_10.Length; i++)
				{
					k = j + OFFSETS_KNIGHT_10[i];
					if (k >= 0 && k < 100 && !edge[k])
					{
						KNIGHT_EPM[x] |= BitBoard.SET_MASK[trto[k]];
					}
				}
				for (i = 0; i < OFFSETS_KING_10.Length; i++)
				{
					k = j + OFFSETS_KING_10[i];
					if (k >= 0 && k < 100 && !edge[k])
					{
						KING_EPM[x] |= BitBoard.SET_MASK[trto[k]];
					}
				}
				if (!edge[j + 9])
				{
					WHITE_PAWN_EPM[x] |= BitBoard.SET_MASK[x + 7];
				}
				if (!edge[j + 11])
				{
					WHITE_PAWN_EPM[x] |= BitBoard.SET_MASK[x + 9];
				}
				if (!edge[j - 9])
				{
					BLACK_PAWN_EPM[x] |= BitBoard.SET_MASK[x - 7];
				}
				if (!edge[j - 11])
				{
					BLACK_PAWN_EPM[x] |= BitBoard.SET_MASK[x - 9];
				}
			}
		}
		static Geometry()
		{
			NEXT_POS = new sbyte[8][][];
			for (int i = 0; i < 8; i++)
			{
				NEXT_POS[i] = new sbyte[BitBoard.SIZE][];
				for (int i2 = 0; i2 < BitBoard.SIZE; i2++)
				{
					NEXT_POS[i][i2] = new sbyte[BitBoard.SIZE];
				}
			}
			NEXT_DIR = new sbyte[8][][];
			for (int i3 = 0; i3 < 8; i3++)
			{
				NEXT_DIR[i3] = new sbyte[BitBoard.SIZE][];
				for (int i4 = 0; i4 < BitBoard.SIZE; i4++)
				{
					NEXT_DIR[i3][i4] = new sbyte[BitBoard.SIZE];
				}
			}
			NEXT_SQ = new sbyte[BitBoard.SIZE][];
			for (int i5 = 0; i5 < BitBoard.SIZE; i5++)
			{
				NEXT_SQ[i5] = new sbyte[BitBoard.SIZE];
			}
			RAY = new long[BitBoard.SIZE][];
			for (int i6 = 0; i6 < BitBoard.SIZE; i6++)
			{
				RAY[i6] = new long[BitBoard.SIZE];
			}
			INTER_PATH = new long[BitBoard.SIZE][];
			for (int i7 = 0; i7 < BitBoard.SIZE; i7++)
			{
				INTER_PATH[i7] = new long[BitBoard.SIZE];
			}
			WHITE_PAWN_EPM = new long[BitBoard.SIZE];
			BLACK_PAWN_EPM = new long[BitBoard.SIZE];
			KNIGHT_EPM = new long[BitBoard.SIZE];
			BISHOP_EPM = new long[BitBoard.SIZE];
			ROOK_EPM = new long[BitBoard.SIZE];
			QUEEN_EPM = new long[BitBoard.SIZE];
			KING_EPM = new long[BitBoard.SIZE];
			{
				initMoves();
				initGeometry();
			}
		}
	}
}