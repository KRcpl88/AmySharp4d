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
* $Id: BitBoard.java 13 2010-01-01 15:09:22Z tetchu $
*/
using System;
namespace tgreiner.amy.bitboard
{

	/// <summary> Some useful methods for using bitboards.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public sealed class BitBoard
	{
		public static readonly int[] LEVEL_SIZE = { 1, 4, 9, 16, 25, 36, 49, 64, 49, 36, 25, 16, 9, 4, 1 };
		public static readonly int[] LEVEL_WIDTH = { 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1 };
		public static readonly int[] LEVEL_OFFSET = { 0, 1, 5, 14, 30, 55, 91, 140, 204, 253, 289, 314, 330, 339, 343 };

		/// <summary>The size of a bitboard. </summary>
		public const int SIZE = 344;
		public const int ULONG_SIZE_BITS = 8 * sizeof(ulong);
		public const int SIZE_LONG = (344 / ULONG_SIZE_BITS) + 1;

		/// <summary>Masks all but first column. </summary>
		private const long ALL_BUT_FIRST_COLUMN = 0x7F7F7F7F7F7F7F7FL;

		/// <summary>Masks all but last column. </summary>
		//UPGRADE_TODO: Literal detected as an unsigned long can generate compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1175'"
		private const long ALL_BUT_LAST_COLUMN = -72340172838076674; //0xFEFEFEFEFEFEFEFEL

		/// <summary>Used to set a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'SET_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] SET_MASK = new long[SIZE];

		/// <summary>Used to clear a bit in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'CLEAR_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly long[] CLEAR_MASK = new long[SIZE];

		/// <summary>Used to find the first bit set in a BitBoard. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'FIRST_ONES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] FIRST_ONES = new int[65536];

		private ulong[] data;


		/// <summary> This class cannot be instantiated.</summary>
		public BitBoard()
		{
			data = new ulong[SIZE_LONG];
		}

		public static BitBoard operator &(BitBoard a, BitBoard b)
		{
			var result = new BitBoard();

			for (int i =0; result.data.Length > i; ++i)
            {
				result.data[i] = a.data[i] & b.data[i];
            }

			return result;
		}

		public static BitBoard operator |(BitBoard a, BitBoard b)
		{
			var result = new BitBoard();

			for (int i = 0; result.data.Length > i; ++i)
			{
				result.data[i] = a.data[i] | b.data[i];
			}

			return result;
		}

		public static BitBoard operator ~(BitBoard a)
		{
			var result = new BitBoard();

			for (int i = 0; result.data.Length > i; ++i)
			{
				result.data[i] = ~(a.data[i]);
			}

			return result;
		}

		/// <summary> Find the number of the first bit set in a bitboard.
		/// 
		/// </summary>
		/// <param name="bitboard">a bitboard
		/// </param>
		/// <returns> the bit number of the first bit set in <code>bitboard</code>
		/// </returns>
		public int findFirstOne()
		{
			int bit = 0;
			ulong mask = 1;
			int i = 0;

			while (data[i] == 0 && i < data.Length)
            {
				++i;
            }
			if (i  == data.Length)
            {
				return -1;
            }


			bit = 0;

			while ((mask & data[i]) == 0)
            {
				mask <<= 1;
				++bit;
			}

			return (int)(i*ULONG_SIZE_BITS + bit);
		}

		public int GetBit(int offset)
		{
			int i = offset / ULONG_SIZE_BITS;

			return (int)(data[i] >> (offset - BitOffset(i))) == 0 ? 0 : 1;
		}

		public int this[int level, int rank, int file]
        {
			get { return GetBit(offset(level, rank, file)); }
        }


		public int GetBit(int level, int rank, int file)
		{
			return GetBit(offset(level, rank, file));
		}

		public void SetBit(int offset)
		{
			int i = offset / ULONG_SIZE_BITS;

			data[i] |= 1UL << (offset - BitOffset(i));
		}


		public void SetBit(int level, int rank, int file)
        {
			SetBit(offset(level, rank, file));
		}

		public void ClearBit(int offset)
        {
            int i = offset / ULONG_SIZE_BITS;

            data[i] &= ~(1UL << (offset - BitOffset(i)));
        }

        private static int BitOffset(int i)
        {
            return (i * ULONG_SIZE_BITS);
        }

        public void ClearBit(int level, int rank, int file)
		{
			ClearBit(offset(level, rank, file));
		}

		public static int offset(int level, int rank, int file)
        {
			return LEVEL_OFFSET[level] + rank * LEVEL_WIDTH[level] + file;
		}

		/// <summary> Count the number of bits set in a bitboard.
		/// 
		/// </summary>
		/// <param name="bitboard">a bitboard
		/// </param>
		/// <returns> the number of bits set in <code>bitboard</code>
		/// 
		/// </returns>
		public int countBits()
		{
			int count = 0;

			for (int i=0; i < data.Length; ++i)
            {
				ulong tmp = data[i];
			
				while (tmp != 0L)
				{
					count++;
					tmp &= tmp - 1;
				}
            }

			
			return count;
		}
		
		/// <summary> Create a String representation of a bitboard.
		/// 
		/// </summary>
		/// <param name="bitboard">the bitboard.
		/// </param>
		/// <returns> a String representation of <code>bitboard</code>
		/// </returns>
		public System.String toString()
		{
			System.Text.StringBuilder buf = new System.Text.StringBuilder();

			for (int level = LEVEL_WIDTH.Length; level >= 0; level--)
			{
				for (int rank = LEVEL_WIDTH[level] - 1; rank >= 0; rank--)
				{
					for (int file = 0; file < LEVEL_WIDTH[level]; file++)
					{
						if (this[level, rank, file] != 0)
						{
							buf.Append("x");
						}
						else
						{
							buf.Append(".");
						}
					}
					buf.Append("\n");
				}
			}
			
			return buf.ToString();
		}

		// BUGBUG unused stupid bitwsie shifts, what is this, are we programmin in assembly?
		/*
		/// <summary> Shift a bitboard 'up'.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard shifted 'up'
		/// </returns>
		public static long shiftUp(long a)
		{
			return (a << 8);
		}
		
		/// <summary> Shift a bitboard 'down'.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard shifted 'down'
		/// </returns>
		public static long shiftDown(long a)
		{
			return (SupportClass.URShift(a, 8));
		}
		
		/// <summary> Shift a bitboard 'left'.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard shifted 'left'
		/// </returns>
		public static long shiftLeft(long a)
		{
			return (SupportClass.URShift(a, 1)) & ALL_BUT_FIRST_COLUMN;
		}
		
		/// <summary> Shift a bitboard 'right'.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard shifted 'right'
		/// </returns>
		public static long shiftRight(long a)
		{
			return (a << 1) & ALL_BUT_LAST_COLUMN;
		}

		/// <summary> Mirror a bitboard along the x-axis.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard mirrored along the x-axis
		/// </returns>
		public static long mirrorX(long a)
		{
			long tmp = a;
			long result = 0;
			while (tmp != 0)
			{
				int idx = findFirstOne(tmp);
				tmp &= CLEAR_MASK[idx];
				result |= SET_MASK[(idx & 0x38) | (7 - (idx & 7))];
			}
			return result;
		}
		
		/// <summary> Mirror a bitboard along the y-axis.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard mirrored along the y-axis
		/// </returns>
		public static long mirrorY(long a)
		{
			long tmp = a;
			long result = 0;
			while (tmp != 0)
			{
				int idx = findFirstOne(tmp);
				tmp &= CLEAR_MASK[idx];
				result |= SET_MASK[(0x38 - (idx & 0x38)) | (idx & 7)];
			}
			return result;
		}
		
		internal static int[] RX = new int[]{7, 15, 23, 31, 39, 47, 55, 63, 6, 14, 22, 30, 38, 46, 54, 62, 5, 13, 21, 29, 37, 45, 53, 61, 4, 12, 20, 28, 36, 44, 52, 60, 3, 11, 19, 27, 35, 43, 51, 59, 2, 10, 18, 26, 34, 42, 50, 58, 1, 9, 17, 25, 33, 41, 49, 57, 0, 8, 16, 24, 32, 40, 48, 56};
		
		/// <summary> Mirror a bitboard along the y-axis.
		/// 
		/// </summary>
		/// <param name="a">the bitboard
		/// </param>
		/// <returns> the bitboard mirrored along the y-axis
		/// </returns>
		public static long rotateCW(long a)
		{
			long tmp = a;
			long result = 0;
			while (tmp != 0)
			{
				int idx = findFirstOne(tmp);
				tmp &= CLEAR_MASK[idx];
				result |= SET_MASK[RX[idx]];
			}
			return result;
		}
		*/
		public static void StaticInitializer()
		{
			{
				//long one = 1;
				for (int mask = 0; mask < 65536; mask++)
				{
					for (int bit = 0; bit < 16; bit++)
					{
						if ((mask & (1 << bit)) != 0)
						{
							FIRST_ONES[mask] = bit;
							break;
						}
					}
				}
			}
		}

	}
}