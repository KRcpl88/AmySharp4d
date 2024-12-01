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
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
namespace tgreiner.amy.bitboard
{
    // Level, Rank and File of a square
    public struct LRF 
    {
        public LRF() {}
        public LRF(int level, int rank, int file) 
        {
            Level = level;
            Rank = rank;
            File = file;

            Validate();
        }

        public LRF(int offset) 
        {
            ValidateOffset(offset);

            Level = BitBoard.LEVEL_OFFSET.Length-1;

            while (offset < BitBoard.LEVEL_OFFSET[Level])
            {
                -- Level;
            }

            Rank = (offset - BitBoard.LEVEL_OFFSET[Level]) / BitBoard.LEVEL_WIDTH[Level];
            File = (offset - BitBoard.LEVEL_OFFSET[Level]) % BitBoard.LEVEL_WIDTH[Level];
        }

        public int Level=0; 
        public int Rank=0; 
        public int File=0;

        private void Validate()
        {
            if (!IsValid())
            {
                throw new IndexOutOfRangeException("LRF.Validate()");
            }
        }   

        private static void ValidateOffset(int offset)
        {
            if (!IsValid(offset))
            {
                throw new IndexOutOfRangeException("LRF.ValidateOffset(offset) offset");
            }
        }   

        public bool IsValid()
        {
            return IsValid(Level, Rank, File);
        }
        
        public static bool IsValid(int level, int rank, int file)
        {
            if ((level < 0) || (level >= BitBoard.NUM_LEVELS))
            {
                return false;
            }

            if ((rank < 0) || (rank >= BitBoard.LEVEL_WIDTH[level])
                || (file < 0) || (file >= BitBoard.LEVEL_WIDTH[level]))
            {
                return false;
            }

            return true;
        }   

        public static bool IsValid(int offset)
        {
            return ((offset < BitBoard.SIZE) && (offset >= 0));
        }   

        /// <summary>Explicit conversion from LRF to square offset.</summary>
        public static explicit operator int(LRF obj)
        {
            return BitBoard.BitOffset(obj.Level, obj.Rank, obj.File);
        }

        /// <summary>Explicit conversion from square offset to LRF.</summary>
        public static explicit operator LRF(int offset)
        {
            return new LRF(offset);
        }
    };

    /// <summary>Universal coordinates for level, rasnk and file in 1/2 squares, aligned with each level.</summary>
    public class UCoord
    {
        /// <summary>Cartersian coordinate data.</summary>
        public int[] data = new int[3];

        /// <summary>Data index for level.</summary>
        public const int idxLevel = 0;

        /// <summary>Data index for half ranks.</summary>
        public const int idxHalfRank = 1;

        /// <summary>Data index for half files.</summary>
        public const int idxHalfFile = 2;

        public UCoord() {}

        public UCoord(int level, int halfRank, int halfFile) 
        {
            Level = level;
            HalfRank = halfRank;
            HalfFile = halfFile;
        }

        /// <summary>Level.</summary>
        public int Level
        {
            get {return data[idxLevel];}
            set {data[idxLevel] = value;}
        }

        /// <summary>1/2 ranks.</summary>
        public int HalfRank
        {
            get {return data[idxHalfRank];}
            set {data[idxHalfRank] = value;}
        }

        /// <summary>1/2 files.</summary>
        public int HalfFile
        {
            get {return data[idxHalfFile];}
            set {data[idxHalfFile] = value;}
        }

        /// <summary>Rank adjusted for the current level</summary>
        public int Rank
        {
            get {return (data[idxHalfRank] - (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[Level]))/2;}
            set {data[idxHalfRank] = (value * 2) + (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[Level]);}
        }

        /// <summary>File adjusted for the current level</summary>
        public int File
        {
            get {return (data[idxHalfFile] - (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[Level]))/2;}
            set {data[idxHalfFile] = (value * 2) + (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[Level]);}
        }

        /// <summary>Explicit conversion from UCoord to square offset.</summary>
        public static explicit operator int(UCoord obj)
        {
            if(LRF.IsValid(obj.Level, obj.Rank, obj.File))
            {
                return BitBoard.BitOffset(obj.Level, obj.Rank, obj.File);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>Explicit conversion from square offset to UCoord.</summary>
        public static explicit operator UCoord(int offset)
        {
            return (UCoord)(LRF)(offset);
        }

        /// <summary>Explicit conversion from UCoord to LRF.</summary>
        public static explicit operator LRF(UCoord obj)
        {
            return new LRF(obj.Level, obj.Rank, obj.File);
        }

        /// <summary>Explicit conversion from LRF to UCoord.</summary>
        public static explicit operator UCoord(LRF lrf)
        {
            var result = new UCoord();
            result.Level = lrf.Level;
            result.Rank = lrf.Rank;
            result.File = lrf.File;
            return result;
        }

        public static UCoord operator +(UCoord a, UCoord b)
        {
            var result = new UCoord();

            for (int i = 0; result.data.Length > i; ++i)
            {
                result.data[i] = a.data[i] + b.data[i];
            }

            return result;
        }

        public static UCoord operator -(UCoord a, UCoord b)
        {
            var result = new UCoord();

            for (int i = 0; result.data.Length > i; ++i)
            {
                result.data[i] = a.data[i] - b.data[i];
            }

            return result;
        }

        public static UCoord operator -(UCoord a)
        {
            var result = new UCoord();

            for (int i = 0; result.data.Length > i; ++i)
            {
                result.data[i] = - a.data[i];
            }

            return result;
        }
    }

    /// <summary> Some useful methods for using bitboards.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
    /// </author>
    public sealed class BitBoard
    {
        /// <summary>The number of squares in each level.</summary>
        public static readonly int[] LEVEL_SIZE = { 1, 4, 9, 16, 25, 36, 49, 64, 49, 36, 25, 16, 9, 4, 1 };

        /// <summary>The width of each level.</summary>
        public static readonly int[] LEVEL_WIDTH = { 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1 };

        /// <summary>offset in squares to the first square in each level.</summary>
        public static readonly int[] LEVEL_OFFSET = 
        {
            BoardConstants_Fields.LA, 
            BoardConstants_Fields.LB, 
            BoardConstants_Fields.LC, 
            BoardConstants_Fields.LD, 
            BoardConstants_Fields.LE, 
            BoardConstants_Fields.LF, 
            BoardConstants_Fields.LG, 
            BoardConstants_Fields.LH, 
            BoardConstants_Fields.LI, 
            BoardConstants_Fields.LJ, 
            BoardConstants_Fields.LK, 
            BoardConstants_Fields.LL, 
            BoardConstants_Fields.LM, 
            BoardConstants_Fields.LN, 
            BoardConstants_Fields.LO
        };

        /// <summary>The number of levels in a bitboard.</summary>
        public const int NUM_LEVELS = 15;

        /// <summary>The width of the largest level in the botboard.</summary>
        public const int MAX_LEVEL_WIDTH = 8;

        /// <summary>The total number of squares of a bitboard. </summary>
        public const int SIZE = 344;

        /// <summary>The total number of bits in a ulong. </summary>
        public const int ULONG_SIZE_BITS = 8 * sizeof(ulong);

        /// <summary>The number of ulongs needed to store the bits for each square in the botboard.</summary>
        public const int SIZE_LONG = (344 / ULONG_SIZE_BITS) + 1;



        /// <summary>Masks invalid bits off of the end of the bit array, the last 24 bits. </summary>
        private const ulong INVALID_SQUARE_MASK = 0xFFFFFFFFFFFFFFFFUL >> (SIZE - (ULONG_SIZE_BITS * (SIZE / ULONG_SIZE_BITS)));


        /// <summary>Masks all but first column. </summary>
        //private const long ALL_BUT_FIRST_COLUMN = 0x7F7F7F7F7F7F7F7FL;

        /// <summary>Masks all but last column. </summary>
        //UPGRADE_TODO: Literal detected as an unsigned long can generate compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1175'"
        //private const long ALL_BUT_LAST_COLUMN = -72340172838076674; //0xFEFEFEFEFEFEFEFEL

        private ulong[] data;


        /// <summary> This class cannot be instantiated.</summary>
        public BitBoard()
        {
            data = new ulong[SIZE_LONG];
        }

        public BitBoard(BitBoard other)
        {
            data = new ulong[SIZE_LONG];

            for (int i =0;i < data.Length; ++i)
            {
                data[i] = other.data[i];
            }
        }

        public BitBoard(int [] offsets)
        {
            data = new ulong[SIZE_LONG];
            SetBits(offsets);
        }

        public static BitBoard[] CreateArray(int size)
        {
            return Enumerable.Repeat(new BitBoard(), size).ToArray();
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

            result.ClearInvalidBits();

            return result;
        }

        private void ClearInvalidBits()
        {
            data[data.Length - 1] &= INVALID_SQUARE_MASK;
        }

        public bool IsEmpty()
        {

            for (int i = 0; data.Length > i; ++i)
            {
                if (data[i] != 0UL)
                {
                    return false;
                }
            }

            return true;
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
            ValidateOffset(offset);
            int countUlongOffset = offset / ULONG_SIZE_BITS;

            return (int)((data[countUlongOffset] >> (offset - UlongOffset2BitOffset(countUlongOffset))) & 1UL) == 0 ? 0 : 1;
        }

        private static void ValidateOffset(int offset)
        {
            if ((offset >= SIZE) || (offset < 0))
            {
                throw new IndexOutOfRangeException("BitBoard.GetBit(offset) offset");
            }
        }

        public int this[int level, int rank, int file]
        {
            get { return GetBit(BitOffset(level, rank, file)); }
            set 
            {
                int offset = BitOffset(level, rank, file);
                if (value == 0)
                {
                    ClearBit(offset);
                }
                else
                {
                    SetBit(offset);
                }
            }
        }

        public int this[int offset]
        {
            get { return GetBit(offset); }
            set
            {
                if (value == 0)
                {
                    ClearBit(offset);
                }
                else
                {
                    SetBit(offset);
                }
            }
        }



        public int GetBit(int level, int rank, int file)
        {
            return GetBit(BitOffset(level, rank, file));
        }

        public void SetBit(int offset)
        {
            ValidateOffset(offset);

            int countUlongOffset = offset / ULONG_SIZE_BITS;

            data[countUlongOffset] |= 1UL << (offset - UlongOffset2BitOffset(countUlongOffset));
        }


        public void SetBit(int level, int rank, int file)
        {
            SetBit(BitOffset(level, rank, file));
        }

        public void SetBits(int[] offsets)
        {
            foreach(int i in offsets)
            {
                SetBit(i);
            }
        }

        public void ClearBit(int offset)
        {
            ValidateOffset(offset);
            int countUlongOffset = offset / ULONG_SIZE_BITS;

            data[countUlongOffset] &= ~(1UL << (offset - UlongOffset2BitOffset(countUlongOffset)));
        }

        private static int UlongOffset2BitOffset(int countUlongOffset)
        {
            return (countUlongOffset * ULONG_SIZE_BITS);
        }

        public void ClearBit(int level, int rank, int file)
        {
            ClearBit(BitOffset(level, rank, file));
        }

        public static int BitOffset(int level, int rank, int file)
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


        public static implicit operator System.String(BitBoard board)
        {
            return board.ToString();
        }

        /// <summary> Create a String representation of a bitboard.
        /// 
        /// </summary>
        /// <param name="bitboard">the bitboard.
        /// </param>
        /// <returns> a String representation of <code>bitboard</code>
        /// </returns>
        public override System.String ToString()
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


        public void Clear()
        {
            for (int i = 0; data.Length > i; ++i)
            {
                data[i] = 0UL;
            }
        }
    }
}