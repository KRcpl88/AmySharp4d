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
    /// <summary> Some useful methods for using bitboards.
    /// 
    /// </summary>
    /// Board position coordinates in level, rank and file
    /// </author>
    public class Lrf 
    {
        public Lrf() {}
        public Lrf(int level, int rank, int file) 
        {
            Level = level;
            Rank = rank;
            File = file;

            Validate();
        }

        public Lrf(int offset) 
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

        public void Validate()
        {
            if (!IsValid())
            {
                throw new IndexOutOfRangeException("Lrf.Validate()");
            }
        }   

        private static void ValidateOffset(int offset)
        {
            if (!IsValid(offset))
            {
                throw new IndexOutOfRangeException("Lrf.ValidateOffset(offset) offset");
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
        public static explicit operator int(Lrf obj)
        {
            return BitBoard.BitOffset(obj.Level, obj.Rank, obj.File);
        }

        /// <summary>Explicit conversion from square offset to LRF.</summary>
        public static explicit operator Lrf(int offset)
        {
            return new Lrf(offset);
        }
    };
}