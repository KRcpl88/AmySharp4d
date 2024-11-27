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
* $Id: EvalMasks.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Contains masks for the Evaluator.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public sealed class EvalMasks
	{
		/// <summary>Masks for white backward pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_BACKWARD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] WHITE_BACKWARD;
		
		/// <summary>Masks for black backward pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_BACKWARD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] BLACK_BACKWARD;
		
		/// <summary>Masks for isolated pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ISOLATED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] ISOLATED;
		
		/// <summary>Masks for white doubled pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_DOUBLED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] WHITE_DOUBLED;
		
		/// <summary>Masks for black doubled pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_DOUBLED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] BLACK_DOUBLED;
		
		/// <summary>Masks for white doubled pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_PASSED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] WHITE_PASSED;
		
		/// <summary>Masks for white doubled pawns. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_PASSED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] BLACK_PASSED;
		
		/// <summary>Masks for file. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'FILE_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] FILE_MASK = new long[8];
		
		/// <summary>Masks for rank. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'RANK_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] RANK_MASK = new long[8];
		
		/// <summary>BitBoard containing all black squares. </summary>
		//UPGRADE_TODO: Literal detected as an unsigned long can generate compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1175'"
		public const long BLACK_SQUARES = -6172840429334713771; // 0xaa55aa55aa55aa55L;
		
		/// <summary>BitBoard containing all black squares. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_SQUARES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long WHITE_SQUARES = ~ BLACK_SQUARES;
		
		/// <summary>Mask for white king in center. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_KING_IN_CENTER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'WHITE_KING_IN_CENTER' was moved to static method 'tgreiner.amy.chess.engine.EvalMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly long WHITE_KING_IN_CENTER;
		
		/// <summary>Mask for black king in center. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_KING_IN_CENTER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'BLACK_KING_IN_CENTER' was moved to static method 'tgreiner.amy.chess.engine.EvalMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		public static readonly long BLACK_KING_IN_CENTER;
		
		/// <summary> This class cannot be instantiated.</summary>
		private EvalMasks()
		{
		}
		
		/// <summary> Initialize the masks.</summary>
		internal static void  initMasks()
		{
			/* Initialize white backward pawn masks. */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				if ((i & 7) > 0)
				{
					for (int j = i - 1; j >= 0; j -= 8)
					{
						WHITE_BACKWARD[i] |= BitBoard.SET_MASK[j];
					}
				}
				if ((i & 7) < 7)
				{
					for (int j = i + 1; j >= 0; j -= 8)
					{
						WHITE_BACKWARD[i] |= BitBoard.SET_MASK[j];
					}
				}
			}
			
			/* Initialize black backward pawn masks. */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				if ((i & 7) > 0)
				{
					for (int j = i - 1; j < BitBoard.SIZE; j += 8)
					{
						BLACK_BACKWARD[i] |= BitBoard.SET_MASK[j];
					}
				}
				if ((i & 7) < 7)
				{
					for (int j = i + 1; j < BitBoard.SIZE; j += 8)
					{
						BLACK_BACKWARD[i] |= BitBoard.SET_MASK[j];
					}
				}
			}
			
			/* Initialize isolated pawn masks */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				if ((i & 7) > 0)
				{
					for (int j = (i & 7) - 1; j < BitBoard.SIZE; j += 8)
					{
						ISOLATED[i] |= BitBoard.SET_MASK[j];
					}
				}
				if ((i & 7) < 7)
				{
					for (int j = (i & 7) + 1; j < BitBoard.SIZE; j += 8)
					{
						ISOLATED[i] |= BitBoard.SET_MASK[j];
					}
				}
			}
			
			/* Initialize white doubled pawn masks */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				for (int j = i - 8; j >= 0; j -= 8)
				{
					WHITE_DOUBLED[i] |= BitBoard.SET_MASK[j];
				}
			}
			
			/* Initialize black doubled pawn masks */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				for (int j = i + 8; j < BitBoard.SIZE; j += 8)
				{
					BLACK_DOUBLED[i] |= BitBoard.SET_MASK[j];
				}
			}
			
			/* Initialize file mask. */
			for (int i = 0; i < 8; i++)
			{
				for (int j = i; j < BitBoard.SIZE; j += 8)
				{
					FILE_MASK[i] |= BitBoard.SET_MASK[j];
				}
			}
			
			/* Initialize rank mask. */
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					RANK_MASK[i] |= BitBoard.SET_MASK[8 * i + j];
				}
			}
			
			/* Initialize white passed pawn mask. */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				for (int j = i + 8; j < BitBoard.SIZE; j += 8)
				{
					WHITE_PASSED[i] |= BitBoard.SET_MASK[j];
					if ((j & 7) > 0)
					{
						WHITE_PASSED[i] |= BitBoard.SET_MASK[j - 1];
					}
					if ((j & 7) < 7)
					{
						WHITE_PASSED[i] |= BitBoard.SET_MASK[j + 1];
					}
				}
			}
			
			/* Initialize black passed pawn mask. */
			for (int i = 0; i < BitBoard.SIZE; i++)
			{
				for (int j = i - 8; j >= 0; j -= 8)
				{
					BLACK_PASSED[i] |= BitBoard.SET_MASK[j];
					if ((j & 7) > 0)
					{
						BLACK_PASSED[i] |= BitBoard.SET_MASK[j - 1];
					}
					if ((j & 7) < 7)
					{
						BLACK_PASSED[i] |= BitBoard.SET_MASK[j + 1];
					}
				}
			}
		}
		static EvalMasks()
		{
			WHITE_BACKWARD = new long[BitBoard.SIZE];
			BLACK_BACKWARD = new long[BitBoard.SIZE];
			ISOLATED = new long[BitBoard.SIZE];
			WHITE_DOUBLED = new long[BitBoard.SIZE];
			BLACK_DOUBLED = new long[BitBoard.SIZE];
			WHITE_PASSED = new long[BitBoard.SIZE];
			BLACK_PASSED = new long[BitBoard.SIZE];
			WHITE_KING_IN_CENTER = BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.E1] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.E2] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.D1] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.D2];
			BLACK_KING_IN_CENTER = BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.E8] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.E7] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.D8] | BitBoard.SET_MASK[tgreiner.amy.bitboard.BoardConstants_Fields.D7];
			{
				initMasks();
			}
		}
	}
}