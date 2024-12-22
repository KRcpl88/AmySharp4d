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

        public UCoord() {}

        public UCoord(int x, int y, int z) 
        {
            X = x;
            Y = y;
            Z = z;
        }


        /// <summary>X axis goes from HA1->HH8</summary>
        public int X
        {
            get {return data[0];}
            set {data[0] = value;}
        }

        /// <summary>Y axis goes from HH1->HA8</summary>
        public int Y
        {
            get {return data[1];}
            set {data[1] = value;}
        }


        /// <summary>Level, Z coord goes from AA1->OA1</summary>
        public int Z
        {
            get {return data[2];}
            set {data[2] = value;}
        }

        /*
        
        K = levelOffset = BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[lrf.Level];
        C = fileOffset = BitBoard.LEVEL_WIDTH[lrf.Level] - 1;

        f = lrf.File 
        r = lrf.Rank

        uCoord.X = x = levelOffset + lrf.File + lrf.Rank;
        uCoord.Y = y = levelOffset + lrf.Rank - lrf.File + fileOffset;


        x = K + f + r;
        y = K + r - f + C

        f = (x - y + C) / 2
        r = (x + y - C) / 2 - K

        */

        /// <summary>Explicit conversion from UCoord to LRF.</summary>
        public static explicit operator Lfr(UCoord uCoord)
        {
            // BUGBUG build an inverse conversion
            var lrf = new Lfr();
            lrf.Level = uCoord.Z;

            int levelOffset = BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[lrf.Level];
            int fileOffset = BitBoard.LEVEL_WIDTH[lrf.Level] - 1;

            int doubleFile = uCoord.X - uCoord.Y + fileOffset;
            if ((doubleFile & 1) != 0)
            {
                throw new InvalidCastException("Invalid coordinates, X-Y is not an even number and can not be converted to Lfr");
            }
            lrf.File = doubleFile >> 1;

            int doubleRank = uCoord.X + uCoord.Y - fileOffset;
            if ((doubleRank & 1) != 0)
            {
                throw new InvalidCastException("Invalid coordinates, X+Y is not an even number and can not be converted to Lfr");
            }
            lrf.Rank = (doubleRank >> 1) - levelOffset;

            return lrf;
        }

        /// <summary>Explicit conversion from LRF to UCoord.</summary>
        public static explicit operator UCoord(Lfr lrf)
        {
            var uCoord = new UCoord();
            uCoord.Z = lrf.Level;
            int levelOffset = BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[lrf.Level];
            int fileOffset = BitBoard.LEVEL_WIDTH[lrf.Level] - 1;
            uCoord.X = levelOffset + lrf.File + lrf.Rank;
            uCoord.Y = levelOffset + lrf.Rank - lrf.File + fileOffset;
            return uCoord;
        }

        public static bool operator ==(UCoord a, UCoord b)
        {
            var result = new UCoord();

            for (int i = 0; result.data.Length > i; ++i)
            {
                if (a.data[i] != b.data[i])
                {
                    return false;
                };
            }

            return true;
        }
        public static bool operator !=(UCoord a, UCoord b)
        {
            var result = new UCoord();

            for (int i = 0; result.data.Length > i; ++i)
            {
                if (a.data[i] != b.data[i])
                {
                    return true;
                };
            }

            return false;
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