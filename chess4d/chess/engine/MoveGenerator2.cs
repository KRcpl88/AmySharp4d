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
* $Id: MoveGenerator2.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using Generator = tgreiner.amy.common.engine.Generator;
using IntVector = tgreiner.amy.common.engine.IntVector;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Generates moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class MoveGenerator2 : Generator
	{
		/// <summary> Get the first killer move.
		/// 
		/// </summary>
		/// <returns> the first killer move
		/// </returns>
		private int Killer1
		{
			get
			{
				return killer1;
			}
			
		}
		/// <summary> Get the second killer move.
		/// 
		/// </summary>
		/// <returns> the second killer move
		/// </returns>
		private int Killer2
		{
			get
			{
				return killer2;
			}
			
		}
		/// <summary> Set the MoveGenerator from two plies below. This is used as the source
		/// of the third killer move.
		/// 
		/// </summary>
		/// <param name="gen">the MoveGenerator from two plies below
		/// </param>
		virtual public MoveGenerator2 TwoPliesBelow
		{
			set
			{
				twoPliesBelow = value;
			}
			
		}
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.MoveGenerator2'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>Constant for phase 'hashmove'. </summary>
		private const int HASHMOVE = 0;
		/// <summary>Constant for phase 'generate gaining captures'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'GENERATE_CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int GENERATE_CAPTURES = HASHMOVE + 1;
		/// <summary>Constant for phase 'gaining captures'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'GAINING_CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int GAINING_CAPTURES = GENERATE_CAPTURES + 1;
		/// <summary>Constant for phase 'killer move 1'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KILLER1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int KILLER1 = GAINING_CAPTURES + 1;
		/// <summary>Constant for phase 'killer move 2'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KILLER2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int KILLER2 = KILLER1 + 1;
		/// <summary>Constant for phase 'killer move 3'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'KILLER3 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int KILLER3 = KILLER2 + 1;
		/// <summary>Constant for phase 'loosing captures'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'LOOSING_CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int LOOSING_CAPTURES = KILLER3 + 1;
		/// <summary>Constant for phase 'generate rest of the moves'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'GENERATE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int GENERATE = LOOSING_CAPTURES + 1;
		/// <summary>Constant for phase 'rest of the moves'. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'REST '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private const int REST = GENERATE + 1;
		
		/// <summary>The current phase. </summary>
		private int phase;
		
		/// <summary>The current index into the moves array. </summary>
		private int idx;
		
		/// <summary>The move from the hash table. </summary>
		private int hashMove;
		
		/// <summary>Killer move 1. </summary>
		private int killer1 = 0;
		
		/// <summary>Number of fail-highs of killer move 1. </summary>
		private int killer1cnt = 0;
		
		/// <summary>Killer move 2. </summary>
		private int killer2 = 0;
		
		/// <summary>Number of fail-highs of killer move 2. </summary>
		private int killer2cnt = 0;
		
		/// <summary>The killer from two plies below </summary>
		private int killer3 = 0;
		
		/// <summary>Keeps moves. </summary>
		private IntVector moves = new IntVector();
		
		/// <summary>Keeps captures. </summary>
		private IntVector captures = new IntVector();
		
		/// <summary>Keeps swap off values. </summary>
		private IntVector swapOffs = new IntVector();
		
		/// <summary>Number of captures. </summary>
		private int nCaptures;
		
		/// <summary>The transposition table. </summary>
		private ITransTable ttable;
		
		/// <summary>The history table. </summary>
		private HistoryTable history;
		
		/// <summary>The board. </summary>
		private ChessBoard board;
		
		/// <summary>Evaluates static exchanges. </summary>
		private Swapper swapper;
		
		/// <summary>Source for killer moves. </summary>
		private MoveGenerator2 twoPliesBelow = null;
		
		/// <summary>Indicates wether the side to move is in check. </summary>
		private bool inCheck;
		
		/// <summary> Create a MoveGenerator.
		/// 
		/// </summary>
		/// <param name="theBoard">the board
		/// </param>
		/// <param name="theTtable">the transposition table
		/// </param>
		/// <param name="theHistory">the history table.
		/// </param>
		public MoveGenerator2(ChessBoard theBoard, ITransTable theTtable, HistoryTable theHistory)
		{
			this.board = theBoard;
			this.ttable = theTtable;
			this.history = theHistory;
			
			swapper = new Swapper();
		}

        /// <seealso cref="Generator.nextMove">
        /// </seealso>
        public virtual int nextMove()
        {

            switch (phase)
            {

                case HASHMOVE:
                    phase = GENERATE_CAPTURES;
                    TTEntry entry = ttable.get_Renamed(board.PosHash);
                    if (entry != null && entry.move != 0)
                    {
                        if (board.isPseudoLegalMove(entry.move))
                        {
                            hashMove = entry.move;
                            return hashMove;
                        }
                        else
                        {
                            log.Error("Got illegal move from hash, " + "possible hash collision");
                        }
                    }
                    // Fallthrough
                    goto case GENERATE_CAPTURES;

                case GENERATE_CAPTURES:
                    captures.Size = 0;
                    swapOffs.Size = 0;
                    long victims = board.getMask(!board.Wtm);

                    if (inCheck)
                    {
                        int kingPos = board.getKingPos(board.Wtm);
                        victims &= (board.getAttackFrom(kingPos) | board.getAttackTo(kingPos));
                    }

                    while (victims != 0L)
                    {
                        int sq = BitBoard.findFirstOne(victims);
                        victims.ClearBit(sq);
                        board.generateTo(sq, captures);
                    }
                    nCaptures = captures.size();
                    for (int i = 0; i < nCaptures; i++)
                    {
                        swapOffs.add(swapper.swap(board, captures.get_Renamed(i)));
                    }

                    // Add en passant captures, hard code their swap off value
                    // to zero
                    board.generateEnPassant(captures);
                    while (swapOffs.size() < captures.size())
                    {
                        swapOffs.add(0);
                        nCaptures++;
                    }

                    // Fallthrough
                    goto case GAINING_CAPTURES;

                case GAINING_CAPTURES:
                    {
                        while (nCaptures > 0)
                        {
                            int bestIdx = 0;
                            int bestSwap = swapOffs.get_Renamed(0);

                            for (int i = 1; i < nCaptures; i++)
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

                            nCaptures--;
                            int move = captures.get_Renamed(bestIdx);
                            captures.swap(bestIdx, nCaptures);
                            swapOffs.swap(bestIdx, nCaptures);

                            if (move != hashMove)
                            {
                                phase = GAINING_CAPTURES;
                                return move;
                            }
                        }
                    }
                    // Fallthrough
                    goto case KILLER1;

                case KILLER1:
                    if (killer1 != hashMove && board.isPseudoLegalMove(killer1))
                    {
                        phase = KILLER2;
                        return killer1;
                    }
                    // Fallthrough
                    goto case KILLER2;

                case KILLER2:
                    if (killer2 != hashMove && board.isPseudoLegalMove(killer2))
                    {
                        phase = KILLER3;
                        return killer2;
                    }
                    // Fallthrough
                    goto case KILLER3;

                case KILLER3:
                    if (twoPliesBelow != null)
                    {
                        killer3 = twoPliesBelow.Killer1;
                        if (killer3 != hashMove && killer3 != killer1 && killer3 != killer2 && board.isPseudoLegalMove(killer3))
                        {
                            phase = LOOSING_CAPTURES;
                            return killer3;
                        }
                        killer3 = twoPliesBelow.Killer2;
                        if (killer3 != hashMove && killer3 != killer1 && killer3 != killer2 && board.isPseudoLegalMove(killer3))
                        {
                            phase = LOOSING_CAPTURES;
                            return killer3;
                        }
                    }
                    // Fallthrough
                    goto case LOOSING_CAPTURES;

                case LOOSING_CAPTURES:
                    {
                        while (nCaptures > 0)
                        {
                            int bestIdx = 0;
                            int bestSwap = swapOffs.get_Renamed(0);

                            for (int i = 1; i < nCaptures; i++)
                            {
                                if (swapOffs.get_Renamed(i) > bestSwap)
                                {
                                    bestSwap = swapOffs.get_Renamed(i);
                                    bestIdx = i;
                                }
                            }

                            nCaptures--;
                            int move = captures.get_Renamed(bestIdx);
                            captures.swap(bestIdx, nCaptures);
                            swapOffs.swap(bestIdx, nCaptures);

                            if (move != hashMove)
                            {
                                phase = LOOSING_CAPTURES;
                                return move;
                            }
                        }
                    }
                    // Fallthrough
                    goto case GENERATE;

                case GENERATE:
                    moves.Size = 0;
                    long all = board.getMask(board.Wtm);
                    while (all != 0L)
                    {
                        int sq = BitBoard.findFirstOne(all);
                        all.ClearBit(sq);
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

                        if (move != hashMove && move != killer1 && move != killer2 && move != killer3)
                        {
                            return move;
                        }
                    }
                    break;
            }

            return -1;
        }
		
		/// <seealso cref="Generator.reset">
		/// </seealso>
		public virtual void  reset()
		{
			phase = HASHMOVE;
			hashMove = 0;
			killer3 = 0;
			
			inCheck = board.InCheck;
		}
		
		/// <seealso cref="Generator.failHigh">
		/// </seealso>
		public virtual void  failHigh(int move, int depth)
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
					if (killer1cnt > 1)
					{
						killer1cnt--;
					}
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
					if (killer1cnt > 1)
					{
						killer1cnt--;
					}
				}
			}
		}
		static MoveGenerator2()
		{
			log = LogManager.GetLogger();
		}
	}
}