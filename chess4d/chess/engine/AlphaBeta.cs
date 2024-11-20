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
* $Id: AlphaBeta.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using Generator = tgreiner.amy.common.engine.Generator;
using NodeType = tgreiner.amy.common.engine.NodeType;
using tgreiner.amy.bitboard;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Implements the alpha-beta search.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class AlphaBeta : ISearcher
	{
		/// <summary> Get the number of nodes visited by this searcher. The number returned
		/// includes the nodes visited by the quiescence search.
		/// 
		/// </summary>
		/// <returns> the number of nodes visited.
		/// </returns>
		virtual public int Nodes
		{
			get
			{
				return nodes + qsearch.Nodes;
			}
			
		}
		
		/// <summary>The chess board to search. </summary>
		private ChessBoard board;
		
		/// <summary>The quiescence search. </summary>
		private QuiescenceSearch qsearch;
		
		/// <summary>The move generators. </summary>
		private Generator[] generators;
		
		/// <summary>The transposition table. </summary>
		private ITransTable ttable;
		
		/// <summary>The history table. </summary>
		private HistoryTable history;
		
		/// <summary>The PVSaver. </summary>
		private PVSaver pvsaver;
		
		/// <summary>The number of nodes visited. </summary>
		private int nodes = 0;
		
		/// <summary>The maximum search depth. </summary>
		private const int MAX_DEPTH = BitBoard.SIZE;
		
		/// <summary> Create a AlphaBeta searcher.
		/// 
		/// </summary>
		/// <param name="theBoard">the ChessBoard to search.
		/// </param>
		/// <param name="theTtable">the transposition table.
		/// </param>
		/// <param name="thePvsaver">the principal variation saver.
		/// </param>
		public AlphaBeta(ChessBoard theBoard, ITransTable theTtable, PVSaver thePvsaver)
		{
			
			this.board = theBoard;
			this.ttable = theTtable;
			this.pvsaver = thePvsaver;
			qsearch = new QuiescenceSearch(board, pvsaver);
			
			initGenerators();
		}
		
		/// <summary> Init the move generators.</summary>
		private void  initGenerators()
		{
			generators = new Generator[MAX_DEPTH];
			history = new HistoryTable();
			
			for (int i = MAX_DEPTH - 1; i >= 0; i--)
			{
				generators[i] = new MoveGenerator(board, ttable, history);
			}
		}
		
		/// <seealso cref="ISearcher.search">
		/// </seealso>
		public virtual int search(int alpha, int beta, int depth, NodeType nodeType)
		{
			return search(alpha, beta, depth, 1);
		}
		
		/// <summary> Search the current positon.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha bound.
		/// </param>
		/// <param name="beta">the beta bound.
		/// </param>
		/// <param name="depth">the remaining search depth.
		/// </param>
		/// <param name="ply">the ply
		/// </param>
		/// <returns> the search value
		/// </returns>
		private int search(int alpha, int beta, int depth, int ply)
		{
			
			if (depth <= 0)
			{
				return qsearch.search(alpha, beta, 0, ply);
			}
			
			nodes++;
			
			// Check the transposition table
			TTEntry entry = ttable.get_Renamed(board.PosHash);
			if (entry != null)
			{
				if (entry.Depth >= depth)
				{
					if (entry.Exact)
					{
						pvsaver.terminal(ply);
						return entry.score;
					}
					if (entry.Lower)
					{
						if (entry.score >= beta)
						{
							return entry.score;
						}
						// beta = Math.min(beta, entry.score);
					}
					if (entry.Upper)
					{
						if (entry.score <= alpha)
						{
							return entry.score;
						}
						// alpha = Math.max(alpha, entry.score);
					}
				}
			}
			
			// Try a null move...
			if (!board.InCheck && (board.MaskNonPawn != 0L))
			{
				board.doNull();
				int tmp = - search(- beta, - beta + 1, depth - 3, ply + 1);
				board.undoMove();
				if (tmp >= beta)
				{
					return tmp;
				}
			}
			
			Generator gen = generators[ply];
			gen.reset();
			int move;
			int bestmove = 0;
			int best = - tgreiner.amy.chess.engine.Searcher_Fields.MATE + ply;
			
			while ((move = gen.nextMove()) != - 1)
			{
				if ((move & Move.CASTLE) != 0 && !board.isCastleLegal(move))
				{
					continue;
				}
				board.doMove(move);
				if (board.OppInCheck)
				{
					board.undoMove();
					continue;
				}
				int depthSub = 1;
				
				if (board.InCheck)
				{
					depthSub = 0;
				}
				int tmp = - search(- beta, - System.Math.Max(alpha, best), depth - depthSub, ply + 1);
				
				board.undoMove();
				if (tmp > best)
				{
					best = tmp;
					bestmove = move;
					if (tmp < beta)
					{
						pvsaver.move(ply, bestmove);
					}
				}
				if (best >= beta)
				{
					gen.failHigh(move, depth);
					break;
				}
			}
			
			if (bestmove != 0)
			{
				ttable.store(board.PosHash, bestmove, depth, best, alpha, beta);
			}
			else
			{
				if (!board.InCheck)
				{
					// stalemate
					best = 0;
				}
				
				if (alpha < best && best < beta)
				{
					pvsaver.terminal(ply);
				}
			}
			
			return best;
		}
		
		/// <seealso cref="ISearcher.reset">
		/// </seealso>
		public virtual void  reset()
		{
			qsearch.resetStats();
			nodes = 0;
		}
	}
}