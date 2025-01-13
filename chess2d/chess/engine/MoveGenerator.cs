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
* $Id: MoveGenerator.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using IntVector = tgreiner.amy.common.engine.IntVector;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Generates moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class MoveGenerator:MVVLVAGenerator
	{
		
		/// <summary>Constant for phase 'hash move'. </summary>
		private const int HASHMOVE = 0;
		
		/// <summary>Constant for phase 'captures'. </summary>
		private const int CAPTURES = 1;
		
		/// <summary>Constant for phase 'killer1'. </summary>
		private const int KILLER1 = 2;
		
		/// <summary>Constant for phase 'killer2'. </summary>
		private const int KILLER2 = 3;
		
		/// <summary>Constant for phase 'generate'. </summary>
		private const int GENERATE = 4;
		
		/// <summary>Constant for phase 'rest'. </summary>
		private const int REST = 5;
		
		/// <summary>The current phase. </summary>
		private int phase;
		
		/// <summary>The move index. </summary>
		private int idx;
		
		/// <summary>The move from the hash table. </summary>
		private int hashmove;
		
		/// <summary>Killer move 1. </summary>
		private int killer1 = 0;
		
		/// <summary>Number of fail-highs of killer move 1. </summary>
		private int killer1cnt = 0;
		
		/// <summary>Killer move 2. </summary>
		private int killer2 = 0;
		
		/// <summary>Number of fail-highs of killer move 2. </summary>
		private int killer2cnt = 0;
		
		/// <summary>The moves. </summary>
		private IntVector moves = new IntVector();
		
		/// <summary>The transposition table. </summary>
		private TransTable ttable;
		
		/// <summary>The history table. </summary>
		private HistoryTable history;
		
		/// <summary> Create a MoveGenerator.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="theTransTable">the transposition table
		/// </param>
		/// <param name="theHistory">the history table.
		/// </param>
		public MoveGenerator(ChessBoard board, TransTable theTransTable, HistoryTable theHistory):base(board)
		{
			this.ttable = theTransTable;
			this.history = theHistory;
		}
		
		/// <seealso cref="Generator.nextMove">
		/// </seealso>
		public override int nextMove()
		{
			switch (phase)
			{
				
				case HASHMOVE: 
					phase = CAPTURES;
					TTEntry entry = ttable.get_Renamed(board.PosHash);
					if (entry != null && board.isPseudoLegalMove(entry.move))
					{
						hashmove = entry.move;
						return hashmove;
					}
					// Fallthrough
					goto case CAPTURES;
				
				case CAPTURES:  {
						int move;
						do 
						{
							move = base.nextMove();
						}
						while (move == hashmove);
						if (move != - 1)
						{
							return move;
						}
					}
					// Fallthrough
					goto case KILLER1;
				
				case KILLER1: 
					if (board.isPseudoLegalMove(killer1))
					{
						phase = KILLER2;
						return killer1;
					}
					// Fallthrough
					goto case KILLER2;
				
				case KILLER2: 
					if (board.isPseudoLegalMove(killer2))
					{
						phase = GENERATE;
						return killer2;
					}
					// Fallthrough
					goto case GENERATE;
				
				case GENERATE: 
					moves.Size = 0;
					long all = board.getMask(board.Wtm);
					while (all != 0L)
					{
						int sq = BitBoard.findFirstOne(all);
						all &= BitBoard.CLEAR_MASK[sq];
						board.generateFrom(sq, moves);
					}
					idx = moves.size();
					phase = REST;
					goto case REST;
				
				case REST: 
					while (idx > 0)
					{
						int move = history.select(moves, idx);
						idx--;
						
						if (move != hashmove && move != killer1 && move != killer2)
						{
							return move;
						}
					}
					break;
				}
			
			return - 1;
		}
		
		/// <seealso cref="Generator.reset">
		/// </seealso>
		public override void  reset()
		{
			base.reset();
			phase = HASHMOVE;
			hashmove = 0;
		}
		
		/// <seealso cref="Generator.failHigh">
		/// </seealso>
		public override void  failHigh(int move, int depth)
		{
			if ((move & (Move.CAPTURE | Move.ENPASSANT)) == 0)
			{
				if (killer1 == 0)
				{
					killer1 = move;
					killer1cnt = 1;
				}
				else if (move == killer1)
				{
					killer1cnt++;
				}
				else if (move == killer2)
				{
					killer2cnt++;
					if (killer2cnt > killer1cnt)
					{
						int tmp = killer1;
						killer1 = killer2;
						killer2 = tmp;
						
						tmp = killer1cnt;
						killer1cnt = killer2cnt;
						killer2cnt = tmp;
					}
				}
				else
				{
					killer2 = move;
					killer2cnt = 1;
				}
			}
		}
	}
}