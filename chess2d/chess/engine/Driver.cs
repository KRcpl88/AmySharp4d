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
* $Id: Driver.java 2 2007-08-09 07:05:44Z tetchu $
*/
// import tgreiner.amy.chess.tablebases.Loader;
// import tgreiner.amy.chess.tablebases.TableBaseProber;
using System;
using IntVector = tgreiner.amy.common.engine.IntVector;
using NodeType = tgreiner.amy.common.engine.NodeType;
using TimeOutException = tgreiner.amy.common.timer.TimeOutException;
using Timer = tgreiner.amy.common.timer.Timer;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.log4net;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> The driver for the search algorithms.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class Driver : ChessConstants
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary>The TableBaseProber. 
		/// private static TableBaseProber tableBaseProber;
		/// /**
		/// Create a TableBaseProber.
		/// 
		/// </summary>
		/// <returns> a TableBaseProber
		/// private static TableBaseProber getTableBaseProber() {
		/// if (tableBaseProber == null) {
		/// try {
		/// log.Info("Loading tablebases.");
		/// Loader l = new Loader();
		/// tableBaseProber = l.load();
		/// } catch (Exception ex) {
		/// log.Error("Caught Exception", ex);
		/// }
		/// }
		/// return tableBaseProber;
		/// }
		/// </returns>
		/// <summary> Set the search output.
		/// 
		/// </summary>
		/// <param name="theOutput">the search output.
		/// </param>
		virtual public SearchOutput SearchOutput
		{
			set
			{
				this.output = value;
			}
			
		}
		/// <summary> Return the score associated with the last move returned by
		/// {@link #search}. Actually this may or may not represent the
		/// true score associated with the move since
		/// <ul>
		/// <li>the search has not been performed since the move was forced
		/// <li>there was a fail high or fail low and the research was not
		/// completed
		/// </ul>
		/// 
		/// </summary>
		/// <returns> the score associated with the last move returned by
		/// {@link #search}
		/// </returns>
		virtual public int RootScore
		{
			get
			{
				return bestScore;
			}
			
		}
		/// <summary> Get the ponder move. This is the second move of the principal
		/// variation, i.e. the move we expected the opponent to make after the
		/// move returned by {@link #search} has been executed.
		/// 
		/// </summary>
		/// <returns> the ponder move
		/// </returns>
		virtual public int PonderMove
		{
			get
			{
				return ponderMove;
			}
			
		}
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.Driver'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>The board to search. </summary>
		private ChessBoard board;
		
		/// <summary>The searcher. </summary>
		private Searcher searcher;
		
		/// <summary>Outputs the search output. </summary>
		private SearchOutput output;
		
		/// <summary>Saves principal variations. </summary>
		private PVSaver pvsaver;
		
		/// <summary>The transposition table. </summary>
		private TransTable transTable;
		
		/// <summary>The timer. </summary>
		private Timer timer;
		
		/// <summary>The search window for the aspiration search. </summary>
		private int window = 33;
		
		/// <summary>The search window for the first research. </summary>
		private int researchWindow1 = 150;
		
		/// <summary>The ponder move. </summary>
		private int ponderMove;
		
		/// <summary>The search score. </summary>
		private int bestScore;
		
		/// <summary> Create a Driver.
		/// 
		/// </summary>
		/// <param name="theBoard">the board to search
		/// </param>
		/// <param name="ttable">the transposition table
		/// </param>
		/// <param name="theTimer">the timer
		/// </param>
		public Driver(ChessBoard theBoard, TransTable ttable, Timer theTimer)
		{
			
			this.transTable = ttable;
			this.board = theBoard;
			this.timer = theTimer;
			
			init();
		}
		
		/// <summary> Initialize the Driver.</summary>
		private void  init()
		{
			Evaluator evaluator = new EvaluatorImpl(board);
			board.Evaluator = evaluator;
			
			pvsaver = new PVSaver();
			output = new SearchOutputTextUI();
			NegaScout n = new NegaScout(board, transTable, pvsaver, timer);
			// n.setTableBaseProber(getTableBaseProber());
			
			searcher = n;
		}
		
		/// <summary> Research a fail low.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <param name="bound">the bound
		/// </param>
		/// <param name="depth">the seach depth
		/// </param>
		/// <returns> the score for move
		/// </returns>
		/// <throws>  TimeOutException if the timer expired </throws>
		private int researchFailLow(int move, int bound, int depth)
		{
			
			board.doMove(move);
			try
			{
				
				int beta = bound;
				int alpha = beta - researchWindow1;
				
				int tmp = - searcher.search(- beta, - alpha, depth - 1, NodeType.PV);
				
				if (tmp <= alpha)
				{
					beta = tmp;
					alpha = - tgreiner.amy.chess.engine.Searcher_Fields.MATE;
					
					tmp = - searcher.search(- beta, - alpha, depth - 1, NodeType.PV);
				}
				
				return tmp;
			}
			finally
			{
				board.undoMove();
			}
		}
		
		/// <summary> Research a fail high.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <param name="bound">the bound
		/// </param>
		/// <param name="depth">the seach depth
		/// </param>
		/// <returns> the score for move
		/// </returns>
		/// <throws>  TimeOutException if the timer expired </throws>
		private int researchFailHigh(int move, int bound, int depth)
		{
			
			board.doMove(move);
			try
			{
				
				int alpha = bound;
				int beta = alpha + researchWindow1;
				
				int tmp = - searcher.search(- beta, - alpha, depth - 1, NodeType.PV);
				
				if (tmp >= beta)
				{
					alpha = tmp;
					beta = tgreiner.amy.chess.engine.Searcher_Fields.MATE;
					
					tmp = - searcher.search(- beta, - alpha, depth - 1, NodeType.PV);
				}
				
				return tmp;
			}
			finally
			{
				board.undoMove();
			}
		}
		
		/// <summary> Search the position.
		/// 
		/// </summary>
		/// <returns> the best move found.
		/// </returns>
		public virtual int search(int max_depth)
		{
			IntVector moves = new IntVector();
			board.generateLegalMoves(moves);
			
			bestScore = 0;
			
			if (moves.size() == 0)
			{
				log.Info("No legal moves found.");
				return 0;
			}
			if (moves.size() == 1)
			{
				log.Info("Forced move " + Move.toSAN(board, moves.get_Renamed(0)) + " found.");
				return moves.get_Renamed(0);
			}
			
			searcher.reset();
			
			transTable.clear();
			
			timer.start();
			
			int alpha;
			int beta;
			int mateFound = 0;
			
			RootMoveList rml = new RootMoveList(moves);
			
			output.header();
			
			try
			{
                for (int depth = 1; depth < max_depth; depth++)
				{
					// Search move 0 with window.
					int move = rml.next();
					int nodes = searcher.Nodes;
					
					System.String pv;
					output.move(depth, timer.Time, Move.toSAN(board, move), 0, moves.size());
					
					alpha = bestScore - window;
					beta = bestScore + window;
					
					board.doMove(move);
					try
					{
						bestScore = - searcher.search(- beta, - alpha, depth - 1, NodeType.PV);
					}
					finally
					{
						board.undoMove();
					}
					
					if (bestScore <= alpha)
					{
						// fail low
						timer.failLow();
						
						output.failLow(depth, timer.Time, Move.toSAN(board, move));
						
						bestScore = researchFailLow(move, bestScore, depth);
					}
					else if (bestScore >= beta)
					{
						// fail high
						output.failHigh(depth, timer.Time, Move.toSAN(board, move));
						
						bestScore = researchFailHigh(move, bestScore, depth);
					}
					pvsaver.move(0, move);
					pv = pvsaver.getPV(board);
					ponderMove = pvsaver.PonderMove;
					
					output.pv(depth, timer.Time, bestScore, pv, searcher.Nodes);
					
					alpha = bestScore;
					beta = bestScore + 1;
					
					nodes = searcher.Nodes - nodes;
					rml.Nodes = nodes;
					
					int i = 0;
					while (rml.hasNext())
					{
						move = rml.next();
						i++;
						
						output.move(depth, timer.Time, Move.toSAN(board, move), i, moves.size());
						
						nodes = searcher.Nodes;
						board.doMove(move);
						int tmp;
						try
						{
							tmp = - searcher.search(- beta, - alpha, depth - 1, NodeType.CUT);
						}
						finally
						{
							board.undoMove();
						}
						
						if (tmp > bestScore)
						{
							output.failHigh(depth, timer.Time, Move.toSAN(board, move));
							
							rml.failHigh();
							
							bestScore = researchFailHigh(move, tmp, depth);
							pvsaver.move(0, move);
							pv = pvsaver.getPV(board);
							ponderMove = pvsaver.PonderMove;
							alpha = bestScore;
							beta = bestScore + 1;
							
							output.pv(depth, timer.Time, bestScore, pv, searcher.Nodes);
							
							nodes = searcher.Nodes - nodes;
							rml.Nodes = nodes;
						}
						else
						{
							nodes = searcher.Nodes - nodes;
							rml.Nodes = nodes;
						}
					}
					
					rml.resort();
					output.pv(depth, timer.Time, bestScore, pv, searcher.Nodes);
					
					if (bestScore < - tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT || bestScore > tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT)
					{
						mateFound++;
						if (mateFound == 3)
						{
							break;
						}
					}
					else
					{
						// reset any mate scores
						mateFound = 0;
					}
					
					timer.iterationFinished(depth);
				}
			}
			catch (TimeOutException)
			{
				// Ends loop...
			}
			
			int nodes2 = searcher.Nodes;
			long nps = nodes2 * 1000L / System.Math.Max(1, timer.Time);
			
			log.Info("Search finished.");
			log.Info("Total nodes: " + nodes2);
			log.Info("Nodes/sec: " + nps);
			log.Info("Best move is " + Move.toSAN(board, rml.Best(0)) + ".");
			
			return rml.Best(0);
		}

		static Driver()
		{
			log = LogManager.GetLogger(typeof(Driver));
		}
	}
}