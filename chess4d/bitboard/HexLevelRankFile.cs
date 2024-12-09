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
    /// <summary>
    /// Hex based position coordinates in level, rank and file
    /// </summary>
    public class HexLrf 
    {
        /*

Level, rank and file for a hexaganol board layout (from a rectilinear layout)
                 +---+
               1 |hh1|
                 +---+
               o   a
               +---+---+
             2 |hg2|hh2|
               +---+---+
             1 |   |   |
               +---+---+
             n   a   b
             +---+---+---+
           3 |hf3|hg3|hh3|
             +---+---+---+
           2 |   |   |   |
             +---+---+---+
           1 |   |   |   |
             +---+---+---+
           m   a   b   c
           +---+---+---+---+
         4 |he4|hf4|hg4|hh4|
           +---+---+---+---+
         3 |   |   |   |   |
           +---+---+---+---+
         2 |   |   |   |   |
           +---+---+---+---+
         1 |   |   |   |   |
           +---+---+---+---+
         l   a   b   c   d
         +---+---+---+---+---+
       5 |hd5|he5|hf5|hg5|hh5|
         +---+---+---+---+---+
       4 |   |   |   |   |   |
         +---+---+---+---+---+
       3 |   |   |   |   |   |
         +---+---+---+---+---+
       2 |   |   |   |   |   |
         +---+---+---+---+---+
       1 |   |   |   |   |   |
         +---+---+---+---+---+
       k   a   b   c   d   e
       +---+---+---+---+---+---+
     6 |hc6|hd6|he6|hf6|hg6|hh6|
       +---+---+---+---+---+---+
     5 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     4 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     3 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     2 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     1 |cc1|cd1|ce1|cf1|cg1|ch1|
       +---+---+---+---+---+---+
     j   a   b   c   d   e   f
     +---+---+---+---+---+---+---+
   7 |hb7|hc7|hd7|he7|hf7|hg7|hh7|
     +---+---+---+---+---+---+---+
   6 |   |   |   |   |   |   |   |
     +---+---+---+---+---+---+---+
   5 |eb5|ec5|ed5|ee5|ef5|eg5|eh5|
     +---+---+---+---+---+---+---+
   4 |eb4|ec4|ed4|ee4|ef4|eg4|eh4|
     +---+---+---+---+---+---+---+
   3 |db3|dc3|dd3|de3|df3|dg3|dh3|
     +---+---+---+---+---+---+---+
   2 |cb2|cc2|cd2|ce2|cf2|cg2|ch2|
     +---+---+---+---+---+---+---+
   1 |bb1|bc1|bd1|be1|bf1|bg1|bh1|
     +---+---+---+---+---+---+---+
   i   a   b   c   d   e   f   g
   +---+---+---+---+---+---+---+---+
 8 |ha8|hb8|hc8|hd8|he8|hf8|hg8|hh8|
   +---+---+---+---+---+---+---+---+
 7 |ga7|gb7|gc7|gd7|ge7|gf7|gg7|gh7|
   +---+---+---+---+---+---+---+---+
 6 |fa6|fb6|fc6|fd6|fe6|ff6|fg6|fh6|
   +---+---+---+---+---+---+---+---+
 5 |ea5|eb5|ec5|ed5|ee5|ef5|eg5|eh5|
   +---+---+---+---+---+---+---+---+
 4 |da4|db4|dc4|dd4|de4|df4|dg4|dh4|
   +---+---+---+---+---+---+---+---+
 3 |ca3|cb3|cc3|cd3|ce3|cf3|cg3|ch3|
   +---+---+---+---+---+---+---+---+
 2 |ba2|bb2|bc2|bd2|be2|bf2|bg2|bh2|
   +---+---+---+---+---+---+---+---+
 1 |aa1|ab1|ac1|ad1|ae1|af1|ag1|ah1|
   +---+---+---+---+---+---+---+---+
 h   a   b   c   d   e   f   g   h
     +---+---+---+---+---+---+---+
   7 |   |   |   |   |   |   |   |
     +---+---+---+---+---+---+---+
   6 |   |   |   |   |   |   |   |
     +---+---+---+---+---+---+---+
   5 |ea6|eb6|ec6|ed6|ee6|ef6|eg6|
     +---+---+---+---+---+---+---+
   4 |da5|db5|dc5|dd5|de5|df5|dg5|
     +---+---+---+---+---+---+---+
   3 |ca4|cb4|cc4|cd4|ce4|cf4|cg4|
     +---+---+---+---+---+---+---+
   2 |ba3|bb3|bc3|bd3|be3|bf3|bg3|
     +---+---+---+---+---+---+---+
   1 |aa2|ab2|ac2|ad2|ae2|af2|ag2|
     +---+---+---+---+---+---+---+
   g   a   b   c   d   e   f   g
       +---+---+---+---+---+---+
     6 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     5 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     4 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     3 |   |   |   |   |   |   |
       +---+---+---+---+---+---+
     2 |ba4|bb4|bc4|bd4|be4|bf4|
       +---+---+---+---+---+---+
     1 |aa3|ab3|ac3|ad3|ae3|af3|
       +---+---+---+---+---+---+
     f   a   b   c   d   e   f
         +---+---+---+---+---+
       5 |   |   |   |   |   |
         +---+---+---+---+---+
       4 |   |   |   |   |   |
         +---+---+---+---+---+
       3 |   |   |   |   |   |
         +---+---+---+---+---+
       2 |ba5|bb5|bc5|bd5|be5|
         +---+---+---+---+---+
       1 |aa4|ab4|ac4|ad4|ae4|
         +---+---+---+---+---+
       e   a   b   c   d   e
           +---+---+---+---+
         4 |   |   |   |   |
           +---+---+---+---+
         3 |   |   |   |   |
           +---+---+---+---+
         2 |ba6|bb6|bc6|bd6|
           +---+---+---+---+
         1 |aa5|ab5|ac5|ad5|
           +---+---+---+---+
         d   a   b   c   d
             +---+---+---+
           3 |   |   |   |
             +---+---+---+
           2 |ba7|bb7|bc7|
             +---+---+---+
           1 |aa6|ab6|ac6|
             +---+---+---+
           c   a   b   c
               +---+---+
             2 |ba8|bb8|
               +---+---+
             1 |aa7|ab7|
               +---+---+
             b   a   b
                 +---+
               1 |aa8|
                 +---+
               a   a


Level, rank and file for rectilinear Lrf coordinates (in a hexagonal board layout)

   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
8 |ha8|hb8|hc8|hd8|he8|hf8|hg8|hh8|
   \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
  7 |ia7|ib7|ic7|id7|ie7|if7|ig7|
     \ /c\ /d\ /e\ /f\ /g\ /h\ /
    6 |ja6|jb6|jc6|jd6|je6|jf6|
       \ /d\ /e\ /f\ /g\ /h\ /
      5 |ka5|kb5|kc5|kd5|ke5|
         \ /e\ /f\ /g\ /h\ /
        4 |la4|lb4|lc4|ld4|
           \ /f\ /g\ /h\ /
          3 |ma3|mb3|mc3|
             \ /g\ /h\ /
            2 |na2|nb2|
               \ /h\ /
              1 |oa1|
              h  \ /


         /a\ /b\ /c\ /d\ /e\
      8 |ea5|eb5|ec5|ed5|ee5|
       /a\ /b\ /c\ /d\ /e\ /f\
    7 |fa5|fb5|fc5|fd5|fe5|ff5|
     /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
  6 |ga5|gb5|gc5|gd5|ge5|gf5|gg5|
   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
5 |ha5|hb5|hc5|hd5|he5|hf5|hg5|hh5|
   \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
  4 |ia4|ib4|ic4|id4|ie4|if4|ig4|
     \ /c\ /d\ /e\ /f\ /g\ /h\ /
    3 |ja3|jb3|jc3|jd3|je3|jf3|
       \ /d\ /e\ /f\ /g\ /h\ /
      2 |ka2|kb2|kc2|kd2|ke2|
         \ /e\ /f\ /g\ /h\ /
        1 |la1|lb1|lc1|ld1|
        e  \ / \ / \ / \ /


           /a\ /b\ /c\ /d\
        8 |da4|db4|dc4|dd4|
         /a\ /b\ /c\ /d\ /e\
      7 |ea4|eb4|ec4|ed4|ee4|
       /a\ /b\ /c\ /d\ /e\ /f\
    6 |fa4|fb4|fc4|fd4|fe4|ff4|
     /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
  5 |ga4|gb4|gc4|gd4|ge4|gf4|gg4|
   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
4 |ha4|hb4|hc4|hd4|he4|hf4|hg4|hh4|
   \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
  3 |ia3|ib3|ic3|id3|ie3|if3|ig3|
     \ /c\ /d\ /e\ /f\ /g\ /h\ /
    2 |ja2|jb2|jc2|jd2|je2|jf2|
       \ /d\ /e\ /f\ /fg /h\ /
      1 |ka1|kb1|kc1|kd1|ke1|
      d  \ / \ / \ / \ / \ /


             /a\ /b\ /c\
          8 |ca3|cb3|cc3|
           /a\ /b\ /c\ /d\
        7 |da3|db3|dc3|dd3|
         /a\ /b\ /c\ /d\ /e\
      6 |ea3|eb3|ec3|ed3|ee3|
       /a\ /b\ /c\ /d\ /e\ /f\
    5 |fa3|fb3|fc3|fd3|fe3|ff3|
     /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
  4 |ga3|gb3|gc3|gd3|ge3|gf3|gg3|
   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
3 |ha3|hb3|hc3|hd3|he3|hf3|hg3|hh3|
   \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
  2 |ia2|ib2|ic2|id2|ie2|if2|ig2|
     \ /c\ /d\ /e\ /f\ /g\ /h\ /
    1 |ja1|jb1|jc1|jd1|je1|jf1|
    c  \ / \ / \ / \ / \ / \ /

               /a\ /b\
            8 |ba2|bb2|
             /a\ /b\ /c\
          7 |ca2|cb2|cc2|
           /a\ /b\ /c\ /d\
        6 |da2|db2|dc2|dd2|
         /a\ /b\ /c\ /d\ /e\
      5 |ea2|eb2|ec2|ed2|ee2|
       /a\ /b\ /c\ /d\ /e\ /f\
    4 |fa2|fb2|fc2|fd2|fe2|ff2|
     /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
  3 |ga2|gb2|gc2|gd2|ge2|gf2|gg2|
   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
2 |ha2|hb2|hc2|hd2|he2|hf2|hg2|hh2|
   \ /b\ /c\ /d\ /e\ /f\ /g\ /h\ /
  1 |ia1|ib1|ic1|id1|ie1|if1|ig1|
  b  \ / \ / \ / \ / \ / \ / \ /

                 /a\
              8 |aa1|
               /a\ /b\
            7 |ba1|bb1|
             /a\ /b\ /c\
          6 |ca1|cb1|cc1|
           /a\ /b\ /c\ /d\
        5 |da1|db1|dc1|dd1|
         /a\ /b\ /c\ /d\ /e\
      4 |ea1|eb1|ec1|ed1|ee1|
       /a\ /b\ /c\ /d\ /e\ /f\
    3 |fa1|fb1|fc1|fd1|fe1|ff1|
     /a\ /b\ /c\ /d\ /e\ /f\ /g\ 
  2 |ga1|gb1|gc1|gd1|ge1|gf1|gg1|
   /a\ /b\ /c\ /d\ /e\ /f\ /g\ /h\
1 |ha1|hb1|hc1|hd1|he1|hf1|hg1|hh1|
a  \ / \ / \ / \ / \ / \ / \ / \ /


        */

        public HexLrf() {}
        public HexLrf(int level, int rank, int file) 
        {
            Level = level;
            Rank = rank;
            File = file;

            Validate();
        }

        /// <summary>
        /// Offest to the HexLrf level or file for each Lrf level
        /// </summary>
        public int[] HexLrfLevelFileOffset = new int[] {0,0,0,0,0,0,0,0,1,2,3,4,5,6,7};

        /// <summary>
        /// Offest to the HexLrf rank for each Lrf level
        /// </summary>
        public int[] HexLrfRankOffset = new int[] {7,6,5,4,3,2,1,0,0,0,0,0,0,0,0};

        public HexLrf(Lrf lrf) 
        {
            lrf.Validate();

            Level = lrf.Rank + HexLrfLevelFileOffset[lrf.Level]; // {0,0,0,0,0,0,0,0,1,2,3,4,5,6,7};
            Rank = lrf.Rank + HexLrfRankOffset[lrf.Level]; // {7,6,5,4,3,2,1,0,0,0,0,0,0,0,0};
            File = lrf.File + HexLrfLevelFileOffset[lrf.Level]; // {0,0,0,0,0,0,0,0,1,2,3,4,5,6,7};

            //Rank = (offset - BitBoard.LEVEL_OFFSET[Level]) / BitBoard.LEVEL_WIDTH[Level];
            //File = (offset - BitBoard.LEVEL_OFFSET[Level]) % BitBoard.LEVEL_WIDTH[Level];
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
        public static explicit operator int(HexLrf obj)
        {
            return BitBoard.BitOffset(obj.Level, obj.Rank, obj.File);
        }

        /// <summary>Explicit conversion from square offset to LRF.</summary>
        public static explicit operator HexLrf(Lrf lrf)
        {
            return new HexLrf(lrf);
        }
    };
}