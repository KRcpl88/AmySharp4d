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
* $Id: QuiescenceSearch.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using Generator = tgreiner.amy.common.engine.Generator;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Implements a quiescence search.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner.
	/// </author>
	public class QuiescenceSearch
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
		/// <summary>The chess board. </summary>
		private ChessBoard board;
		
		/// <summary>The quiescence move generators. </summary>
		private Generator[] generators;
		
		/// <summary>The position evaluator. </summary>
		private Evaluator evaluator;
		
		/// <summary>Saves the principal variation. </summary>
		private PVSaver pvsaver;
		
		/// <summary>Number of nodes searched. </summary>
		private int nodes = 0;
		
		/// <summary>The maximum depth we search to. </summary>
		private const int MAX_DEPTH = 64;
		
		/// <summary> Create a quiescence search.
		/// 
		/// </summary>
		/// <param name="cb">the chess board
		/// </param>
		/// <param name="thePVSaver">the PVSaver.
		/// </param>
		public QuiescenceSearch(ChessBoard cb, PVSaver thePVSaver)
		{
			this.board = cb;
			this.evaluator = cb.Evaluator;
			this.pvsaver = thePVSaver;
			
			initGenerators();
		}
		
		/// <summary> Init the move generators.</summary>
		private void  initGenerators()
		{
			generators = new Generator[MAX_DEPTH];
			
			for (int i = MAX_DEPTH - 1; i >= 0; i--)
			{
				generators[i] = new NonLoosingCaptureMoveGenerator(board);
			}
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
			
			if (board.InsufficientMaterial)
			{
				return 0;
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
				if ((staticEval + ((move & Move.PROMO_QUEEN) != 0?(evaluator.getMaterialValue(tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN) - evaluator.getMaterialValue(tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)):0) + evaluator.getMaterialValue(board.getPieceAt(Move.getTo(move))) + evaluator.getMaterialValue(tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)) < alpha)
				{
					continue;
				}
				
				board.doMove(move);
				if (board.OppInCheck)
				{
					board.undoMove();
					continue;
				}
				int tmp = - search(- beta, - System.Math.Max(alpha, best), depth - 1, ply + 1);
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
		
		/// <summary> Reset the statistics.</summary>
		public virtual void  resetStats()
		{
			nodes = 0;
		}
	}
}