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
* $Id: BoardConstants.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.bitboard
{
    
    /// <summary> Defines several constants.
    /// 
    /// </summary>
    /// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
    /// </author>
    public struct BoardConstants_Fields{
        public const int LA = 0;
        public const int LB = 1;
        public const int LC = 5;
        public const int LD = 14;
        public const int LE = 30;
        public const int LF = 55;
        public const int LG = 91;
        public const int LH = 140;
        public const int LI = 204;
        public const int LJ = 253;
        public const int LK = 289;
        public const int LL = 314;
        public const int LM = 330;
        public const int LN = 339;
        public const int LO = 343;

        /// <summary>Symbolic names for squares on a board. </summary>
        public const int HA8 = LH + 56;
        public const int HB8 = LH + 57;
        public const int HC8 = LH + 58;
        public const int HD8 = LH + 59;
        public const int HE8 = LH + 60;
        public const int HF8 = LH + 61;
        public const int HG8 = LH + 62;
        public const int HH8 = LH + 63;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int HA7 = LH + 48;
        public const int HB7 = LH + 49;
        public const int HC7 = LH + 50;
        public const int HD7 = LH + 51;
        public const int HE7 = LH + 52;
        public const int HF7 = LH + 53;
        public const int HG7 = LH + 54;
        public const int HH7 = LH + 55;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int A6 = LH + 40;
        public const int B6 = LH + 41;
        public const int C6 = LH + 42;
        public const int D6 = LH + 43;
        public const int E6 = LH + 44;
        public const int F6 = LH + 45;
        public const int G6 = LH + 46;
        public const int H6 = LH + 47;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int A5 = LH + 32;
        public const int B5 = LH + 33;
        public const int C5 = LH + 34;
        public const int D5 = LH + 35;
        public const int E5 = LH + 36;
        public const int F5 = LH + 37;
        public const int G5 = LH + 38;
        public const int H5 = LH + 39;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int A4 = LH + 24;
        public const int B4 = LH + 25;
        public const int C4 = LH + 26;
        public const int D4 = LH + 27;
        public const int E4 = LH + 28;
        public const int F4 = LH + 29;
        public const int G4 = LH + 30;
        public const int H4 = LH + 31;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int A3 = LH + 16;
        public const int B3 = LH + 17;
        public const int C3 = LH + 18;
        public const int D3 = LH + 19;
        public const int E3 = LH + 20;
        public const int F3 = LH + 21;
        public const int G3 = LH + 22;
        public const int H3 = LH + 23;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int HA2 = LH + 8;
        public const int HB2 = LH + 9;
        public const int HC2 = LH + 10;
        public const int HD2 = LH + 11;
        public const int HE2 = LH + 12;
        public const int HF2 = LH + 13;
        public const int HG2 = LH + 14;
        public const int HH2 = LH + 15;
        /// <summary>Symbolic names for squares on a board. </summary>
        public const int HA1 = LH + 0;
        public const int HB1 = LH + 1;
        public const int HC1 = LH + 2;
        public const int HD1 = LH + 3;
        public const int HE1 = LH + 4;
        public const int HF1 = LH + 5;
        public const int HG1 = LH + 6;
        public const int HH1 = LH + 7;
    }

    // BUGBUG not needed
    /*
    public interface BoardConstants
    {
        //UPGRADE_NOTE: Members of interface 'BoardConstants' were extracted into structure 'BoardConstants_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
    }
    */
}