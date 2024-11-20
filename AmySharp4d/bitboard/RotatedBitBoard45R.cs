/*-
* Copyright (c) 2009, 2010 Thorsten Greiner
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
* $Id: BitBoard.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.bitboard
{
	/*
	public class RotatedBitBoard45R
	{
		
		/// <summary>Used to set a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'SET_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] SET_MASK;
		
		/// <summary>Used to clear a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'CLEAR_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] CLEAR_MASK;
		
		/// <summary> The transpose matrix.</summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'TRANSPOSE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int[] TRANSPOSE = new int[]{0, 40, 48, 56, 36, 61, 54, 47, 8, 1, 41, 49, 57, 37, 62, 55, 16, 9, 2, 42, 50, 58, 38, 63, 24, 17, 10, 3, 43, 51, 59, 39, 32, 25, 18, 11, 4, 44, 52, 60, 29, 33, 26, 19, 12, 5, 45, 53, 22, 30, 34, 27, 20, 13, 6, 46, 15, 23, 31, 35, 28, 21, 14, 7};
		
		/// <summary> Utility method to retrieve the bits on the diagonal a1 - h8.
		/// 
		/// </summary>
		/// <param name="b">the bitboard
		/// </param>
		/// <returns> the bits on diagonal a1 - h8.
		/// </returns>
		public static int getDiagA1H8(long b)
		{
			return (int) (b & 255);
		}
		
		public static int getDiagA2G8(long b)
		{
			return (int) ((b >> 8) & 127);
		}
		
		public static int getDiagA3F8(long b)
		{
			return (int) ((b >> 16) & 63);
		}
		
		public static int getDiagA4E8(long b)
		{
			return (int) ((b >> 24) & 31);
		}
		
		public static int getDiagA5D8(long b)
		{
			return (int) ((b >> 32) & 15);
		}
		
		public static int getDiagA6C8(long b)
		{
			return (int) ((b >> 29) & 7);
		}
		
		public static int getDiagA7B8(long b)
		{
			return (int) ((b >> 22) & 3);
		}
		
		public static int getDiagB1H7(long b)
		{
			return (int) ((b >> 40) & 127);
		}
		
		public static int getDiagC1H6(long b)
		{
			return (int) ((b >> 48) & 63);
		}
		
		public static int getDiagD1H5(long b)
		{
			return (int) ((b >> 56) & 31);
		}
		
		public static int getDiagE1H4(long b)
		{
			return (int) ((b >> 36) & 15);
		}
		
		public static int getDiagF1H3(long b)
		{
			return (int) ((b >> 61) & 7);
		}
		
		public static int getDiagG1H2(long b)
		{
			return (int) ((b >> 54) & 3);
		}
		static RotatedBitBoard45R()
		{
			SET_MASK = new long[BitBoard.SIZE];
			CLEAR_MASK = new long[BitBoard.SIZE];
			{
				for (int i = 0; i < BitBoard.SIZE; i++)
				{
					SET_MASK[i] = BitBoard.SET_MASK[TRANSPOSE[i]];
					CLEAR_MASK[i] = BitBoard.CLEAR_MASK[TRANSPOSE[i]];
				}
			}
		}
	}
	*/
}