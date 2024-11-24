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
* $Id: EvalMasks.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
//using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
namespace tgreiner.amy.chess.engine
{
    
    /// <summary> Contains masks for the Evaluator.
    /// 
    /// </summary>
    /// <author>  Thorsten Greiner
    /// </author>
    public sealed class EvalMasks
    {
        /// <summary>Masks for white backward pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_BACKWARD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] WHITE_BACKWARD;
        
        /// <summary>Masks for black backward pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_BACKWARD '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] BLACK_BACKWARD;
        
        /// <summary>Masks for isolated pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'ISOLATED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] ISOLATED;
        
        /// <summary>Masks for white doubled pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_DOUBLED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] WHITE_DOUBLED;
        
        /// <summary>Masks for black doubled pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_DOUBLED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] BLACK_DOUBLED;
        
        /// <summary>Masks for white doubled pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_PASSED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] WHITE_PASSED;
        
        /// <summary>Masks for white doubled pawns. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_PASSED '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] BLACK_PASSED;
        
        /// <summary>Masks for file. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'FILE_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] FILE_MASK = BitBoard.CreateArray(8);
        
        /// <summary>Masks for rank. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'RANK_MASK '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly BitBoard[] RANK_MASK = BitBoard.CreateArray(8);

        /// <summary>BitBoard containing all black squares. </summary>
        //UPGRADE_TODO: Literal detected as an unsigned long can generate compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1175'"
        // BUGBUG initialize with other BitBoards
        public static readonly BitBoard BLACK_SQUARES = new BitBoard(); //= -6172840429334713771; // 0xaa55aa55aa55aa55L;

        /// <summary>BitBoard containing all black squares. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_SQUARES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        // BUGBUG initialize with other BitBoards
        public static readonly BitBoard WHITE_SQUARES= new BitBoard(); //= ~ BLACK_SQUARES;
        
        /// <summary>Mask for white king in center. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'WHITE_KING_IN_CENTER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //UPGRADE_NOTE: The initialization of  'WHITE_KING_IN_CENTER' was moved to static method 'tgreiner.amy.chess.engine.EvalMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly BitBoard WHITE_KING_IN_CENTER;
        
        /// <summary>Mask for black king in center. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'BLACK_KING_IN_CENTER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        //UPGRADE_NOTE: The initialization of  'BLACK_KING_IN_CENTER' was moved to static method 'tgreiner.amy.chess.engine.EvalMasks'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public static readonly BitBoard BLACK_KING_IN_CENTER;
        
        /// <summary> This class cannot be instantiated.</summary>
        private EvalMasks()
        {
        }
        
        /// <summary> Initialize the masks.</summary>
        internal static void  initMasks()
        {
            /* Initialize white backward pawn masks. */
            // BUGBUG this class needs to be updsted for level, rank and file
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                if ((i & 7) > 0)
                {
                    for (int j = i - 1; j >= 0; j -= 8)
                    {
                        WHITE_BACKWARD[i].SetBit(j);
                    }
                }
                if ((i & 7) < 7)
                {
                    for (int j = i + 1; j >= 0; j -= 8)
                    {
                        WHITE_BACKWARD[i].SetBit(j);
                    }
                }
            }
            
            /* Initialize black backward pawn masks. */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                if ((i & 7) > 0)
                {
                    for (int j = i - 1; j < BitBoard.SIZE; j += 8)
                    {
                        BLACK_BACKWARD[i].SetBit(j);
                    }
                }
                if ((i & 7) < 7)
                {
                    for (int j = i + 1; j < BitBoard.SIZE; j += 8)
                    {
                        BLACK_BACKWARD[i].SetBit(j);
                    }
                }
            }
            
            /* Initialize isolated pawn masks */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                if ((i & 7) > 0)
                {
                    for (int j = (i & 7) - 1; j < BitBoard.SIZE; j += 8)
                    {
                        ISOLATED[i].SetBit(j);
                    }
                }
                if ((i & 7) < 7)
                {
                    for (int j = (i & 7) + 1; j < BitBoard.SIZE; j += 8)
                    {
                        ISOLATED[i].SetBit(j);
                    }
                }
            }
            
            /* Initialize white doubled pawn masks */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                for (int j = i - 8; j >= 0; j -= 8)
                {
                    WHITE_DOUBLED[i].SetBit(j);
                }
            }
            
            /* Initialize black doubled pawn masks */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                for (int j = i + 8; j < BitBoard.SIZE; j += 8)
                {
                    BLACK_DOUBLED[i].SetBit(j);
                }
            }
            
            /* Initialize file mask. */
            for (int i = 0; i < 8; i++)
            {
                // BUGBUG the math is wrong for j += 8, that only works for 8x8 board
                for (int j = i; j < BitBoard.SIZE; j += 8)
                {
                    FILE_MASK[i].SetBit(j);
                }
            }
            
            /* Initialize rank mask. */
            // BUGBUG the math is wrong, that only works for 8x8 board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    RANK_MASK[i].SetBit(8 * i + j);
                }
            }
            
            /* Initialize white passed pawn mask. */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                for (int j = i + 8; j < BitBoard.SIZE; j += 8)
                {
                    WHITE_PASSED[i].SetBit(j);
                    if ((j & 7) > 0)
                    {
                        WHITE_PASSED[i].SetBit(j - 1);
                    }
                    if ((j & 7) < 7)
                    {
                        WHITE_PASSED[i].SetBit(j + 1);
                    }
                }
            }
            
            /* Initialize black passed pawn mask. */
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                for (int j = i - 8; j >= 0; j -= 8)
                {
                    BLACK_PASSED[i].SetBit(j);
                    if ((j & 7) > 0)
                    {
                        BLACK_PASSED[i].SetBit(j - 1);
                    }
                    if ((j & 7) < 7)
                    {
                        BLACK_PASSED[i].SetBit(j + 1);
                    }
                }
            }
        }
        static EvalMasks()
        {
            // BUGBUG do we need to initialize the members of thre array tto?
            WHITE_BACKWARD = BitBoard.CreateArray(BitBoard.SIZE);
            BLACK_BACKWARD = BitBoard.CreateArray(BitBoard.SIZE);
            ISOLATED = BitBoard.CreateArray(BitBoard.SIZE);
            WHITE_DOUBLED = BitBoard.CreateArray(BitBoard.SIZE);
            BLACK_DOUBLED = BitBoard.CreateArray(BitBoard.SIZE);
            WHITE_PASSED = BitBoard.CreateArray(BitBoard.SIZE);
            BLACK_PASSED = BitBoard.CreateArray(BitBoard.SIZE);
            WHITE_KING_IN_CENTER = new BitBoard( new int [] 
            { 
                BoardConstants_Fields.HE1, 
                BoardConstants_Fields.HE2,
                BoardConstants_Fields.HD1,
                BoardConstants_Fields.HD2
            });
            
            BLACK_KING_IN_CENTER = new BitBoard( new int [] 
            { 
                BoardConstants_Fields.HE8, 
                BoardConstants_Fields.HE7,
                BoardConstants_Fields.HD8,
                BoardConstants_Fields.HD7
            });

            {
                initMasks();
            }
        }
    }
}