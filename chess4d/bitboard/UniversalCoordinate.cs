/*-
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
*/

namespace tgreiner.amy.bitboard
{
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
            if(Lfr.IsValid(obj.Level, obj.File, obj.Rank))
            {
                return BitBoard.BitOffset(obj.Level, obj.File, obj.Rank);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>Explicit conversion from square offset to UCoord.</summary>
        public static explicit operator UCoord(int offset)
        {
            return (UCoord)(Lfr)(offset);
        }

        /// <summary>Explicit conversion from UCoord to LRF.</summary>
        public static explicit operator Lfr(UCoord obj)
        {
            return new Lfr(obj.Level, obj.File, obj.Rank);
        }

        /// <summary>Explicit conversion from LRF to UCoord.</summary>
        public static explicit operator UCoord(Lfr lrf)
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
}