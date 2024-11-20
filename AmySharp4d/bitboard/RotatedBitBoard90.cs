/*-
* Copyright (c) 2009 Thorsten Greiner
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
	public class RotatedBitBoard90
	{
		
		/// <summary>Used to set a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'SET_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] SET_MASK;
		
		/// <summary>Used to clear a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'CLEAR_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] CLEAR_MASK;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'TRANSPOSE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int[] TRANSPOSE = new int[]{0, 8, 16, 24, 32, 40, 48, 56, 1, 9, 17, 25, 33, 41, 49, 57, 2, 10, 18, 26, 34, 42, 50, 58, 3, 11, 19, 27, 35, 43, 51, 59, 4, 12, 20, 28, 36, 44, 52, 60, 5, 13, 21, 29, 37, 45, 53, 61, 6, 14, 22, 30, 38, 46, 54, 62, 7, 15, 23, 31, 39, 47, 55, 63};
		static RotatedBitBoard90()
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