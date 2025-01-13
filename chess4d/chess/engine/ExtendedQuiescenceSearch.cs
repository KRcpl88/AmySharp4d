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
* $Id: ExtendedQuiescenceSearch.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using Generator = tgreiner.amy.common.engine.Generator;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Implements a quiescence search.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner.
	/// </author>
	public class ExtendedQuiescenceSearch
	{
		/// <summary> Get the number of nodes searched by this searcher.
		/// 
		/// </summary>
		/// <returns> the number of nodes.
		/// </returns>
		virtual public int Nodes
		{
			get
			{
				return nodes;
			}
			
		}
		/// <summary>The log4j Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.ExtendedQuiescenceSearch'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>The chess board. </summary>
		private ChessBoard board;
		
		/// <summary>The quiescence move generators. </summary>
		private Generator[] generators;
		
		/// <summary>The check evasion move generators. </summary>
		private Generator[] checkEvasionGenerators;
		
		/// <summary>The extended move generator. </summary>
		private Generator extendedGenerator;
		
		/// <summary>The position evaluator. </summary>
		private IEvaluator evaluator;
		
		/// <summary>Saves the principal variation. </summary>
		private PVSaver pvsaver;
		
		/// <summary>Number of nodes searched. </summary>
		private int nodes = 0;
		
		/// <summary>The maximum depth we search to. </summary>
		private const int MAX_DEPTH = 64;
		
		/// <summary>The transposition table. </summary>
		private ITransTable transTable;
		
		/// <summary> Create a quiescenc search.
		/// 
		/// </summary>
		/// <param name="cb">the chess board
		/// </param>
		/// <param name="pvsaver">the PVSaver.
		/// </param>
		/// <param name="theTtable">the transposition table
		/// </param>
		/// <param name="theHistory">the history table
		/// </param>
		public ExtendedQuiescenceSearch(ChessBoard cb, PVSaver pvsaver, ITransTable theTtable, HistoryTable theHistory)
		{
			this.board = cb;
			this.evaluator = cb.Evaluator;
			this.pvsaver = pvsaver;
			this.transTable = theTtable;
			
			initGenerators(theHistory);
		}
		
		/// <summary> Init the move generators.
		/// 
		/// </summary>
		/// <param name="theHistory">the history table
		/// </param>
		private void  initGenerators(HistoryTable theHistory)
		{
			
			generators = new Generator[MAX_DEPTH];
			checkEvasionGenerators = new Generator[2];
			
			for (int i = MAX_DEPTH - 1; i >= 0; i--)
			{
				generators[i] = new NonLoosingCaptureMoveGenerator(board);
			}
			
			for (int i = 0; i < 2; i++)
			{
				checkEvasionGenerators[i] = new CheckEvasionMoveGenerator(board, transTable, theHistory);
			}
			
			extendedGenerator = new ExtendedQuiescenceMoveGenerator(board, transTable);
		}
		
		/// <summary> The recursive quiescence search.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <param name="beta">the beta value
		/// </param>
		/// <param name="depth">the depth
		/// </param>
		/// <param name="ply">the ply
		/// </param>
		/// <returns> the quiescence value
		/// </returns>
		public virtual int search(int alpha, int beta, int depth, int ply)
		{
			
			nodes++;
			
			if (ply >= MAX_DEPTH)
			{
				return 0;
			}
			
			if (board.Repeated || board.InsufficientMaterial)
			{
				if (alpha < 0 && beta > 0)
				{
					pvsaver.terminal(ply);
				}
				return 0;
			}
			
			if (depth < 2 && board.InCheck)
			{
				return evadeCheck(alpha, beta, depth, ply);
			}
			
			if (depth == 0)
			{
				return extendedQ(alpha, beta, ply);
			}
			
			int best = evaluator.evaluate(alpha, beta);
			int staticEval = best;
			
			if (best >= beta)
			{
				return best;
			}
			if (best > alpha)
			{
				pvsaver.terminal(ply);
			}
			Generator gen = generators[ply];
			gen.reset();
			int move;
			
			while ((move = gen.nextMove()) != - 1)
			{
				if ((staticEval + ((move & Move.PROMO_QUEEN) != 0?(evaluator.getMaterialValue(ChessConstants_Fields.QUEEN) - evaluator.getMaterialValue(ChessConstants_Fields.PAWN)):0) + evaluator.getMaterialValue(board.getPieceAt(Move.getTo(move))) + evaluator.getMaterialValue(ChessConstants_Fields.PAWN)) < alpha)
				{
					continue;
				}
				
				board.doMove(move);
				if (board.OppInCheck)
				{
					board.undoMove();
					continue;
				}
				int tmp = - search(- beta, - System.Math.Max(alpha, best), depth + 1, ply + 1);
				board.undoMove();
				if (tmp > best)
				{
					best = tmp;
					if (best >= beta)
					{
						break;
					}
					if (best > alpha)
					{
						pvsaver.move(ply, move);
					}
				}
			}
			
			return best;
		}
		
		/// <summary> Search the current position with the evade check quiescence search.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <param name="beta">the beta value
		/// </param>
		/// <param name="depth">the depth
		/// </param>
		/// <param name="ply">the ply
		/// </param>
		/// <returns> the quiescence value
		/// </returns>
		private int evadeCheck(int alpha, int beta, int depth, int ply)
		{
			
			int best = - tgreiner.amy.chess.engine.Searcher_Fields.MATE + ply;
			int bestMove = 0;
			
			if (best >= beta)
			{
				return best;
			}
			if (best > alpha)
			{
				pvsaver.terminal(ply);
			}
			Generator gen = checkEvasionGenerators[depth];
			gen.reset();
			int move;
			
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
				int tmp = - search(- beta, - System.Math.Max(alpha, best), depth + 1, ply + 1);
				board.undoMove();
				if (tmp > best)
				{
					best = tmp;
					bestMove = move;
					if (best >= beta)
					{
						gen.failHigh(move, depth);
						break;
					}
					if (best > alpha)
					{
						pvsaver.move(ply, move);
					}
				}
			}
			
			transTable.store(board.PosHash, bestMove, 0, best, alpha, beta);
			
			return best;
		}
		
		/// <summary> Search the current position with the extended quiescence search,
		/// which includes checking moves and loosing captures.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <param name="beta">the beta value
		/// </param>
		/// <param name="ply">the ply
		/// </param>
		/// <returns> the quiescence value
		/// </returns>
		private int extendedQ(int alpha, int beta, int ply)
		{
			
			int best = evaluator.evaluate(alpha, beta);
			int bestMove = 0;
			int staticEval = best;
			
			if (best >= beta)
			{
				return best;
			}
			if (best > alpha)
			{
				pvsaver.terminal(ply);
			}
			Generator gen = extendedGenerator;
			gen.reset();
			int move;
			
			while ((move = gen.nextMove()) != - 1)
			{
				
				board.doMove(move);
				if (board.OppInCheck)
				{
					board.undoMove();
					continue;
				}
				int tmp = - search(- beta, - System.Math.Max(alpha, best), 1, ply + 1);
				board.undoMove();
				if (tmp > best)
				{
					best = tmp;
					bestMove = move;
					if (best >= beta)
					{
						gen.failHigh(move, 1);
						break;
					}
					if (best > alpha)
					{
						pvsaver.move(ply, move);
					}
				}
			}
			
			transTable.store(board.PosHash, bestMove, 0, best, alpha, beta);
			
			return best;
		}
		
		/// <summary> Reset the statistics.</summary>
		public virtual void  resetStats()
		{
			nodes = 0;
		}
		static ExtendedQuiescenceSearch()
		{
			log = LogManager.GetLogger(typeof(ExtendedQuiescenceSearch));
		}
	}
}