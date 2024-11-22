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
* $Id: Futility.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Determines futility.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	sealed class Futility //: ChessConstants
	{
		
		/// <summary> This class cannot be instantiated.</summary>
		private Futility()
		{
		}
		
		/// <summary> Estimate the best score that can be produced by executing a
		/// move.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <returns> an optimistic bound for the score for <code>move</code>
		/// </returns>
		internal static int estimateMove(ChessBoard board, int move)
		{
			IEvaluator eval = board.Evaluator;
			int matBalance = eval.WhiteMaterial - eval.BlackMaterial;
			if (!board.Wtm)
			{
				matBalance = - matBalance;
			}
			
			if ((move & Move.ENPASSANT) != 0)
			{
				matBalance += eval.getMaterialValue(ChessConstants_Fields.PAWN);
			}
			else if ((move & Move.CAPTURE) != 0)
			{
				matBalance += eval.getMaterialValue(board.getPieceAt(Move.getTo(move)));
			}
			if ((move & Move.PROMOTION) != 0)
			{
				matBalance += eval.getMaterialValue(Move.getPromoPiece(move));
			}
			return matBalance;
		}
		
		/// <summary> Determine if a move is futile.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <returns> <code>true</code> if <code>move</code> is futile
		/// </returns>
		internal static bool isFutile(ChessBoard board, int move, int alpha)
		{
			
			IEvaluator eval = board.Evaluator;
			int matBalance = estimateMove(board, move);
			
			if ((matBalance + 2 * eval.getMaterialValue(ChessConstants_Fields.PAWN)) >= alpha)
			{
				return false;
			}
			
			if (board.isCheckingMove(move))
			{
				return false;
			}
			
			return true;
		}
		
		/// <summary> Determine if a move is futile.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <returns> <code>true</code> if <code>move</code> is futile
		/// </returns>
		internal static bool isFutile2(ChessBoard board, int move, int alpha)
		{
			
			IEvaluator eval = board.Evaluator;
			int matBalance = estimateMove(board, move);
			
			if ((matBalance + eval.getMaterialValue(ChessConstants_Fields.BISHOP)) >= alpha)
			{
				return false;
			}
			
			if (board.isCheckingMove(move))
			{
				return false;
			}
			
			return true;
		}
	}
}