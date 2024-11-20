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
* $Id: Hashing.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy;
using tgreiner.amy.bitboard;

namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Hashkeys used by ChessBoard for Zobrist hashing of positions.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	sealed class Hashing //: ChessConstants
	{
		/// <summary>Generates the random numbers. </summary>
		//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.util.Random.Random'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
		private static System.Random random = new System.Random((System.Int32) 4711L);
		
		/// <summary>The hash keys for pieces. </summary>
		public static long[][][] HASH_KEYS;
		
		/// <summary>The hash keys for en passant squares. </summary>
		public static long[] EN_PASSANT_HASH_KEYS;
		
		/// <summary>The hash keys for castling rights. </summary>
		public static long[] CASTLE_HASH_KEYS;
		
		/// <summary>The hash key for side to move. </summary>
		//UPGRADE_TODO: Literal detected as an unsigned long can generate compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1175'"
		public const long WTM_HASH = -6172840429334713771; // 0xaa55aa55aa55aa55L;
		
		/// <summary> This class is not instantiable.</summary>
		private Hashing()
		{
		}
		static Hashing()
		{
			{
				HASH_KEYS = new long[2][][];
				for (int i = 0; i < 2; i++)
				{
					HASH_KEYS[i] = new long[tgreiner.amy.chess.engine.ChessConstants_Fields.KING + 1][];
					for (int i2 = 0; i2 < tgreiner.amy.chess.engine.ChessConstants_Fields.KING + 1; i2++)
					{
						HASH_KEYS[i][i2] = new long[BitBoard.SIZE];
					}
				}
				EN_PASSANT_HASH_KEYS = new long[BitBoard.SIZE];
				
				for (int pc = 0; pc <= tgreiner.amy.chess.engine.ChessConstants_Fields.KING; pc++)
				{
					for (int sq = 0; sq < BitBoard.SIZE; sq++)
					{
						//UPGRADE_TODO: Method 'java.util.Random.nextlong' was converted to 'SupportClass.Nextlong' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilRandomnextlong'"
						HASH_KEYS[0][pc][sq] = SupportClass.NextLong(random);
						//UPGRADE_TODO: Method 'java.util.Random.nextlong' was converted to 'SupportClass.Nextlong' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilRandomnextlong'"
						HASH_KEYS[1][pc][sq] = SupportClass.NextLong(random);
						//UPGRADE_TODO: Method 'java.util.Random.nextlong' was converted to 'SupportClass.Nextlong' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilRandomnextlong'"
						EN_PASSANT_HASH_KEYS[sq] = SupportClass.NextLong(random);
					}
				}
				
				CASTLE_HASH_KEYS = new long[16];
				for (int castle = 0; castle < 16; castle++)
				{
					//UPGRADE_TODO: Method 'java.util.Random.nextlong' was converted to 'SupportClass.Nextlong' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilRandomnextlong'"
                    CASTLE_HASH_KEYS[castle] = SupportClass.NextLong(random);
				}
			}
		}
	}
}