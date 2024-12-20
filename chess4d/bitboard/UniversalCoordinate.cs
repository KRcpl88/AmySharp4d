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

        /// <summary>Explicit conversion from UCoord to LRF.</summary>
        public static explicit operator Lfr(UCoord obj)
        {
            // BUGBUG build an inverse conversion
            return new Lfr(0, 0, 0);
        }

        /// <summary>Explicit conversion from LRF to UCoord.</summary>
        public static explicit operator UCoord(Lfr lrf)
        {
            var result = new UCoord();
            result.Z = lrf.Level;
            result.X = (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[lrf.Level]) + lrf.File + lrf.Rank;
            result.Y = (BitBoard.MAX_LEVEL_WIDTH - BitBoard.LEVEL_WIDTH[lrf.Level]) + lrf.Rank - lrf.File + BitBoard.LEVEL_WIDTH[lrf.Level] - 1;
            return result;
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