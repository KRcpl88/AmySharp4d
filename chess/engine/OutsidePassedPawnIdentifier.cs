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
* $Id: OutsidePassedPawnIdentifier.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Identifies outside passed pawns.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class OutsidePassedPawnIdentifier
	{
		/// <summary> Get the bitboard of white's outside passed pawns.
		/// 
		/// </summary>
		/// <returns> white's outside passed pawns
		/// </returns>
		virtual public long WhiteOutsidePassedPawns
		{
			get
			{
				return whiteOutsidePassedPawns;
			}
			
		}
		/// <summary> Get the bitboard of black's outside passed pawns.
		/// 
		/// </summary>
		/// <returns> black's outside passed pawns
		/// </returns>
		virtual public long BlackOutsidePassedPawns
		{
			get
			{
				return blackOutsidePassedPawns;
			}
			
		}
		
		
		private static long[] FILES_LEFT_QS;
		
		
		private static long[] FILES_RIGHT_QS;
		
		
		private static long[] FILES_LEFT_KS;
		
		
		private static long[] FILES_RIGHT_KS;
		
		/// <summary>White's outside passed pawns. </summary>
		private long whiteOutsidePassedPawns;
		
		/// <summary>Black's outside passed pawns. </summary>
		private long blackOutsidePassedPawns;
		
		/// <summary> Probe for outside passed pawns.
		/// 
		/// </summary>
		/// <param name="whitePawns">bitboard of white's pawns
		/// </param>
		/// <param name="blackPawns">bitboard of black's pawns
		/// </param>
		public virtual void  probe(long whitePawns, long blackPawns)
		{
			whiteOutsidePassedPawns = 0L;
			blackOutsidePassedPawns = 0L;
			
			for (int file = 0; file < 4; file++)
			{
				if ((whitePawns & EvalMasks.FILE_MASK[file]) == 0L)
				{
					continue;
				}
				
				if ((blackPawns & FILES_LEFT_QS[file]) != 0L)
				{
					//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
					goto outer_brk;
				}
				
				if ((whitePawns & FILES_RIGHT_QS[file]) != 0L && (blackPawns & FILES_RIGHT_QS[file]) != 0L)
				{
					whiteOutsidePassedPawns |= (whitePawns & EvalMasks.FILE_MASK[file]);
				}
				
				break;
			}
			//UPGRADE_NOTE: Label 'outer_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"

outer_brk: ;
			
			
			for (int file = 0; file < 4; file++)
			{
				if ((blackPawns & EvalMasks.FILE_MASK[file]) == 0L)
				{
					continue;
				}
				
				if ((whitePawns & FILES_LEFT_QS[file]) != 0L)
				{
					//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
					goto outer2_brk;
				}
				
				if ((whitePawns & FILES_RIGHT_QS[file]) != 0L && (blackPawns & FILES_RIGHT_QS[file]) != 0L)
				{
					blackOutsidePassedPawns |= (blackPawns & EvalMasks.FILE_MASK[file]);
				}
				
				break;
			}
			//UPGRADE_NOTE: Label 'outer2_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"

outer2_brk: ;
			
			
			for (int file = 7; file >= 4; file--)
			{
				if ((whitePawns & EvalMasks.FILE_MASK[file]) == 0L)
				{
					continue;
				}
				
				if ((blackPawns & FILES_RIGHT_KS[file]) != 0L)
				{
					//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
					goto outer3_brk;
				}
				
				if ((whitePawns & FILES_LEFT_KS[file]) != 0L && (blackPawns & FILES_LEFT_KS[file]) != 0L)
				{
					whiteOutsidePassedPawns |= (whitePawns & EvalMasks.FILE_MASK[file]);
				}
				
				break;
			}
			//UPGRADE_NOTE: Label 'outer3_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"

outer3_brk: ;
			
			
			for (int file = 7; file >= 4; file--)
			{
				if ((blackPawns & EvalMasks.FILE_MASK[file]) == 0L)
				{
					continue;
				}
				
				if ((whitePawns & FILES_RIGHT_KS[file]) != 0L)
				{
					//UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
					goto outer4_brk;
				}
				
				if ((whitePawns & FILES_LEFT_KS[file]) != 0L && (blackPawns & FILES_LEFT_KS[file]) != 0L)
				{
					blackOutsidePassedPawns |= (blackPawns & EvalMasks.FILE_MASK[file]);
				}
				
				break;
			}
			//UPGRADE_NOTE: Label 'outer4_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"

outer4_brk: ;
			
		}
		static OutsidePassedPawnIdentifier()
		{
			{
				FILES_LEFT_QS = new long[8];
				FILES_RIGHT_QS = new long[8];
				FILES_LEFT_KS = new long[8];
				FILES_RIGHT_KS = new long[8];
				
				for (int file = 0; file < 8; file++)
				{
					for (int f2 = 0; f2 < file + 1; f2++)
					{
						FILES_LEFT_QS[file] |= EvalMasks.FILE_MASK[f2];
					}
					for (int f3 = file + 2; f3 < 8; f3++)
					{
						FILES_RIGHT_QS[file] |= EvalMasks.FILE_MASK[f3];
					}
					for (int f4 = 0; f4 < file - 1; f4++)
					{
						FILES_LEFT_KS[file] |= EvalMasks.FILE_MASK[f4];
					}
					for (int f5 = System.Math.Max(file - 1, 0); f5 < 8; f5++)
					{
						FILES_RIGHT_KS[file] |= EvalMasks.FILE_MASK[f5];
					}
				}
			}
		}
	}
}