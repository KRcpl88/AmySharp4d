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
* $Id: NonLoosingCaptureMoveGenerator.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using Generator = tgreiner.amy.common.engine.Generator;
using IntVector = tgreiner.amy.common.engine.IntVector;
namespace tgreiner.amy.chess.engine
{
	
	
	/// <summary> A move generator that generates capturing moves which do not loose
	/// material according to a static exchange evaluator.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class NonLoosingCaptureMoveGenerator : Generator
	{
		
		/// <summary>Constant for phase 'generate gaining captures' </summary>
		private const int GENERATE_CAPTURES = 0;
		
		/// <summary>Constant for phase 'gaining captures' </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'GAINING_CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int GAINING_CAPTURES = GENERATE_CAPTURES + 1;
		
		/// <summary>The board. </summary>
		protected internal ChessBoard board;
		
		/// <summary>Keeps moves. </summary>
		private IntVector moves = new IntVector();
		
		/// <summary>Keeps swap off values. </summary>
		private IntVector swapOffs = new IntVector();
		
		/// <summary>Number of moves. </summary>
		private int nMoves;
		
		/// <summary>The phase. </summary>
		private int phase;
		
		/// <summary>Evaluates static exchanges. </summary>
		private Swapper swapper;
		
		/// <summary> Create a NonLoosingCaptureMoveGenerator.
		/// 
		/// </summary>
		/// <param name="theBoard">the board
		/// </param>
		public NonLoosingCaptureMoveGenerator(ChessBoard theBoard)
		{
			this.board = theBoard;
			swapper = new Swapper();
			reset();
		}

        /// <seealso cref="Generator.nextMove">
        /// </seealso>
        public virtual int nextMove()
        {
            switch (phase)
            {

                case GENERATE_CAPTURES:
                    moves.Size = 0;
                    swapOffs.Size = 0;
                    long victims = board.getMask(!board.Wtm);
                    while (victims != 0L)
                    {
                        int sq = BitBoard.findFirstOne(victims);
                        victims &= BitBoard.CLEAR_MASK[sq];
                        board.generateTo(sq, moves);
                    }

                    long pawnOn7th = board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN) & EvalMasks.RANK_MASK[board.Wtm ? 6 : 1];

                    while (pawnOn7th != 0L)
                    {
                        int sq = BitBoard.findFirstOne(pawnOn7th);
                        pawnOn7th &= BitBoard.CLEAR_MASK[sq];

                        int to = board.Wtm ? sq + 8 : sq - 8;
                        if (board.getPieceAt(to) != 0)
                        {
                            continue;
                        }

                        moves.add(Move.makeMove(sq, to) | Move.PROMO_QUEEN);
                    }

                    nMoves = moves.size();
                    for (int i = 0; i < nMoves; i++)
                    {
                        swapOffs.add(swapper.swap(board, moves.get_Renamed(i)));
                    }

                    // Add en passant captures, hard code their swap off value
                    // to zero
                    board.generateEnPassant(moves);
                    while (swapOffs.size() < moves.size())
                    {
                        swapOffs.add(0);
                    }

                    // Fallthrough
                    goto case GAINING_CAPTURES;

                case GAINING_CAPTURES:
                    {
                        while (nMoves > 0)
                        {
                            int bestIdx = 0;
                            int bestSwap = swapOffs.get_Renamed(0);

                            for (int i = 1; i < nMoves; i++)
                            {
                                if (swapOffs.get_Renamed(i) > bestSwap)
                                {
                                    bestSwap = swapOffs.get_Renamed(i);
                                    bestIdx = i;
                                }
                            }

                            if (bestSwap < 0)
                            {
                                break;
                            }

                            nMoves--;
                            int move = moves.get_Renamed(bestIdx);
                            moves.swap(bestIdx, nMoves);
                            swapOffs.swap(bestIdx, nMoves);

                            phase = GAINING_CAPTURES;
                            return move;
                        }
                    }
                    break;
            }
            return -1;
        }
		
		/// <seealso cref="ISearcher.reset">
		/// </seealso>
		public virtual void  reset()
		{
			phase = GENERATE_CAPTURES;
		}
		
		/// <seealso cref="ISearcher.failHigh">
		/// </seealso>
		public virtual void  failHigh(int move, int depth)
		{
			// do nothing...
		}
	}
}