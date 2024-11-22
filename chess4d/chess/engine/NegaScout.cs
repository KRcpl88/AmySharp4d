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
* $Id: NegaScout.java 15 2010-03-23 09:03:40Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using IRecognizer = tgreiner.amy.chess.engine.recognizer.IRecognizer;
using RecognizerMap = tgreiner.amy.chess.engine.recognizer.RecognizerMap;
//import tgreiner.amy.chess.tablebases.TableBaseProber;
using Generator = tgreiner.amy.common.engine.Generator;
using NodeType = tgreiner.amy.common.engine.NodeType;
using TimeOutException = tgreiner.amy.common.timer.TimeOutException;
using Timer = tgreiner.amy.common.timer.Timer;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Implements a nega scout search algorithm.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class NegaScout : ISearcher //, ChessConstants
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
				return nodes + qsearch.Nodes + extQsearch.Nodes;
			}
			
		}
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.NegaScout'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>The chess board to search. </summary>
		private ChessBoard board;
		
		/// <summary>The quiescence search. </summary>
		private ExtendedQuiescenceSearch extQsearch;
		
		/// <summary>The quiescence search. </summary>
		private QuiescenceSearch qsearch;
		
		/// <summary>The move generators. </summary>
		private Generator[] generators;
		
		/// <summary>The move generators. </summary>
		private Generator[] checkEvasionGenerators;
		
		/// <summary>The transposition table. </summary>
		private ITransTable ttable;
		
		/// <summary>The history table. </summary>
		private HistoryTable history;
		
		/// <summary>The evaluator. </summary>
		private IEvaluator evaluator;
		
		/// <summary>The PVSaver. </summary>
		private PVSaver pvsaver;
		
		/// <summary>The TableBaseProber. </summary>
		//private TableBaseProber tableBaseProber;
		
		/// <summary>Controls search selectivity. </summary>
		private ISelectivity sel = new SelectivityImpl();
		
		/// <summary>Recognizes special endgames. </summary>
		private RecognizerMap recognizerMap = new RecognizerMap();
		
		/// <summary>The timer. </summary>
		private Timer timer;
		
		/// <summary>The number of nodes visited. </summary>
		private int nodes = 0;
		
		/// <summary>number of null moves tried. </summary>
		private int nullTried;
		
		/// <summary>number of null move cutoffs. </summary>
		private int nullCutOffs;
		
		/// <summary>The maximum search depth. </summary>
		private const int MAX_DEPTH = BitBoard.SIZE;
		
		/// <summary>Records extensions at each ply. </summary>
		private int[] extensions = new int[MAX_DEPTH];
		
		/// <summary>Records in check status at each ply. </summary>
		private bool[] inCheckTab = new bool[MAX_DEPTH];
		
		/// <summary>This is one search ply. </summary>
		internal const int DEPTH_STEP = 16;
		
		/// <summary>Reduction for null move. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'REDUCE_NULLMOVE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int REDUCE_NULLMOVE = 3 * DEPTH_STEP;
		
		/// <summary> Create a NegaScout searcher.
		/// 
		/// </summary>
		/// <param name="theBoard">the ChessBoard to search.
		/// </param>
		/// <param name="theTtable">the transposition table.
		/// </param>
		/// <param name="thePVSaver">the principal variation saver.
		/// </param>
		/// <param name="theTimer">the timer.
		/// </param>
		public NegaScout(ChessBoard theBoard, ITransTable theTtable, PVSaver thePVSaver, Timer theTimer)
		{
			this.board = theBoard;
			this.ttable = theTtable;
			this.pvsaver = thePVSaver;
			this.timer = theTimer;
			
			initGenerators();
			reset();
		}
		
		/// <summary> Initialize the move generators.</summary>
		private void  initGenerators()
		{
			generators = new Generator[MAX_DEPTH];
			checkEvasionGenerators = new Generator[MAX_DEPTH];
			
			history = new HistoryTable();
			
			for (int i = MAX_DEPTH - 1; i >= 0; i--)
			{
				generators[i] = new MoveGenerator2(board, ttable, history);
				checkEvasionGenerators[i] = new CheckEvasionMoveGenerator(board, ttable, history);
			}
			
			for (int i = 2; i < MAX_DEPTH; i++)
			{
				MoveGenerator2 gen = (MoveGenerator2) generators[i];
				MoveGenerator2 gen2 = (MoveGenerator2) generators[i - 2];
				gen.TwoPliesBelow = gen2;
			}
			
			qsearch = new QuiescenceSearch(board, pvsaver);
			
			extQsearch = new ExtendedQuiescenceSearch(board, pvsaver, ttable, history);
		}
		
		/// <seealso cref="ISearcher.search">
		/// </seealso>
		public virtual int search(int alpha, int beta, int depth, NodeType nodeType)
		{
			
			return search(alpha, beta, depth * DEPTH_STEP, 1, nodeType);
		}
		
		/// <summary> Search the current position.
		/// 
		/// </summary>
		/// <param name="theAlpha">the alpha bound.
		/// </param>
		/// <param name="theBeta">the beta bound.
		/// </param>
		/// <param name="depth">the remaining search depth.
		/// </param>
		/// <param name="ply">the ply
		/// </param>
		/// <param name="nodeType">the node type
		/// </param>
		/// <returns> the value of this position
		/// </returns>
		/// <throws>  TimeOutException when the timer expires </throws>
		private int search(int theAlpha, int theBeta, int depth, int ply, NodeType nodeType)
		{
			
			if (ply >= MAX_DEPTH)
			{
				throw new TimeOutException();
			}
			
			int alpha = theAlpha;
			int beta = theBeta;
			
			if (depth <= 0)
			{
				if (ply >= 3 && inCheckTab[ply - 1] && inCheckTab[ply - 3])
				{
					return extQsearch.search(alpha, beta, 0, ply);
				}
				else
				{
					return qsearch.search(alpha, beta, 0, ply);
				}
			}
			
			timer.check();
			
			bool tryNullMove = true;
			
			nodes++;
			
			// Check for draw by repetition.
			if (board.Repeated || board.InsufficientMaterial)
			{
				if (alpha < 0 && beta > 0)
				{
					pvsaver.terminal(ply);
				}
				return 0;
			}
			
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
				if (entry.Upper)
				{
					tryNullMove = false;
				}
			}
			
			/*
			// Try endgame tablebases
			if (tableBaseProber != null && tableBaseProber.probe(board)) {
			int best;
			
			if (tableBaseProber.getValue() == 0) {
			best = 0;
			} else if (tableBaseProber.getValue() > 0) {
			best =
			MATE - 2 * (127 - tableBaseProber.getValue()) + ply;
			} else {
			best =
			-MATE + 2 * (127 + tableBaseProber.getValue()) + ply;
			}
			
			
			//ttable.store(
			//    board.getPosHash(), 0, depth, best, alpha, beta);               
			
			return best;
			}
			*/
			
			IRecognizer recog = recognizerMap.getRecognizer(board);
			if (recog != null)
			{
				int result = recog.probe(board);
				if (result == tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.EXACT)
				{
					return recog.Value;
				}
				else if (result == tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.LOWER_BOUND)
				{
					if (recog.Value >= beta)
					{
						return recog.Value;
					}
					if (recog.Value > alpha)
					{
						alpha = recog.Value;
					}
				}
				else if (result == tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.UPPER_BOUND)
				{
					if (recog.Value <= alpha)
					{
						return recog.Value;
					}
					if (recog.Value < beta)
					{
						beta = recog.Value;
					}
				}
			}
			
			bool inCheck = board.InCheck;
			inCheckTab[ply] = inCheck;
			
			
			// Try a null move...
			if (tryNullMove && (board.LastMove != 0) && !inCheck && (board.MaskNonPawn.countBits() > 1))
			{
				nullTried++;
				board.doNull();
				int tmp;
				
				// int reduction = Math.max(REDUCE_NULLMOVE, depth / 2);
				int reduction = REDUCE_NULLMOVE;
				
				extensions[ply] = 0;
				
				try
				{
					int nextDepth = depth - reduction;
					if (nextDepth <= 0)
					{
						tmp = - extQsearch.search(- beta, - beta + 1, 0, ply + 1);
					}
					else
					{
						tmp = - search(- beta, - beta + 1, nextDepth, ply + 1, NodeType.ALL);
					}
				}
				finally
				{
					board.undoMove();
				}
				
				if (tmp >= beta)
				{
					nullCutOffs++;
					
					/*
					if ((nullCutOffs & 0xffff) == 0) {
					log.Info("nullTried: "
					+ nullTried
					+ " nullCutOffs: "
					+ nullCutOffs
					+ " perc: "
					+ (100.0*nullCutOffs/nullTried));
					}
					*/
					ttable.store(board.PosHash, 0, depth, tmp, alpha, beta);
					
					return tmp;
				}
			}
			
			// Internal iterative deepening to populate the hashtable.
			if (entry == null && (nodeType == NodeType.CUT || nodeType == NodeType.PV) && (alpha + 1 != beta) && depth >= 3 * DEPTH_STEP)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("Using internal iterative deepening at ply " + ply);
				}
				search(alpha, beta, depth - 2 * DEPTH_STEP, ply, nodeType);
			}
			
			Generator gen = inCheck?checkEvasionGenerators[ply]:generators[ply];
			gen.reset();
			int move;
			int bestMove = 0;
			int best = - tgreiner.amy.chess.engine.Searcher_Fields.MATE;
			NodeType nextNodeType = nodeType.getSiblingType();
			
			while ((move = gen.nextMove()) != - 1)
			{
				if ((move & Move.CASTLE) != 0 && !board.isCastleLegal(move))
				{
					continue;
				}
				
				int nextDepth = depth - DEPTH_STEP;
				int extend = sel.extendPreDoMove(board, move);
				
				if (!inCheck && bestMove != 0)
				{
					if (sel.isFutile(board, nextDepth + extend, move, alpha))
					{
						continue;
					}
				}
				
				board.doMove(move);
				
				if (board.OppInCheck)
				{
					board.undoMove();
					continue;
				}
				
				extend += sel.extendAfterDoMove(board);
				
				if (ply > 0)
				{
					if ((extend + extensions[ply - 1]) > 2 * DEPTH_STEP)
					{
						// log.Info("Urgs");
						extend = 2 * DEPTH_STEP - extensions[ply - 1];
					}
				}
				
				nextDepth += extend;
				extensions[ply] = extend;
				
				int tmp;
				
				try
				{
					if (bestMove == 0)
					{
						tmp = - search(- beta, - System.Math.Max(alpha, best), nextDepth, ply + 1, nextNodeType);
					}
					else
					{
						int talpha = System.Math.Max(alpha, best);
						tmp = - search(- (talpha + 1), - talpha, nextDepth, ply + 1, nextNodeType);
						if (tmp > talpha && tmp < beta)
						{
							nextNodeType = nodeType.getSiblingType();
							
							tmp = - search(- beta, - talpha, nextDepth, ply + 1, NodeType.PV);
						}
					}
					nextNodeType = NodeType.CUT;
				}
				finally
				{
					board.undoMove();
				}
				
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
			
			if (bestMove != 0)
			{
				if (best > alpha)
				{
					history.addHistory(bestMove, (depth / DEPTH_STEP));
				}
			}
			else
			{
				if (!inCheck)
				{
					// stalemate
					best = 0;
				}
				else
				{
					best = - tgreiner.amy.chess.engine.Searcher_Fields.MATE + ply;
				}
				
				if (alpha < best && best < beta)
				{
					pvsaver.terminal(ply);
				}
			}
			
			ttable.store(board.PosHash, bestMove, depth, best, alpha, beta);
			
			return best;
		}
		
		/// <seealso cref="ISearcher.reset">
		/// </seealso>
		public virtual void  reset()
		{
			history.reset();
			qsearch.resetStats();
			nodes = 0;
			nullTried = 0;
			nullCutOffs = 0;
			evaluator = board.Evaluator;
			if (evaluator != null)
			{
				evaluator.init();
			}
		}
		
		/// <summary> Set the table base prober.
		/// 
		/// </summary>
		/// <param name="theTableBaseProber">the table base prober
		/// 
		/// public void setTableBaseProber(final TableBaseProber theTableBaseProber) {
		/// this.tableBaseProber = theTableBaseProber;
		/// }
		/// </param>
		static NegaScout()
		{
			log = LogManager.GetLogger();
		}
	}
}