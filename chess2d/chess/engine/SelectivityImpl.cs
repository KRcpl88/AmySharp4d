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
* $Id: SelectivityImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Default Selectivity implementation.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class SelectivityImpl : Selectivity, ChessConstants
	{
		
		/// <summary>The swapper. </summary>
		private Swapper swapper = new Swapper();
		
		/// <summary>Extension if pawn recapture. </summary>
		private const int EXTEND_RECAPTURE_PAWN = 6;
		
		/// <summary>Extension if knight/bishop recapture. </summary>
		private const int EXTEND_RECAPTURE_MINOR = 8;
		
		/// <summary>Extension if rook recapture. </summary>
		private const int EXTEND_RECAPTURE_ROOK = 10;
		
		/// <summary>Extension if queen recapture. </summary>
		private const int EXTEND_RECAPTURE_QUEEN = 12;
		
		/// <summary>Extension for pawn push to the 7th rank. </summary>
		private const int EXTEND_PASSED_PAWN = 16;
		
		/// <summary>Extension if in check. </summary>
		private const int EXTEND_CHECK = 16;
		
		/// <seealso cref="Selectivity.extendPreDoMove">
		/// </seealso>
		public virtual int extendPreDoMove(ChessBoard board, int move)
		{
			return extendReCapture(board, move) + extendPassedPawnPush(board, move);
		}
		
		/// <seealso cref="Selectivity.extendAfterDoMove">
		/// </seealso>
		public virtual int extendAfterDoMove(ChessBoard board)
		{
			if (board.InCheck)
			{
				return EXTEND_CHECK;
			}
			return 0;
		}
		
		/// <summary> Check if the search should be extended because of a recapture.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move.
		/// </param>
		/// <returns> the extension.
		/// </returns>
		internal virtual int extendReCapture(ChessBoard board, int move)
		{
			if ((move & Move.CAPTURE) != 0)
			{
				int lmove = board.LastMove;
				if ((lmove & Move.CAPTURE) != 0 && Move.getTo(move) == Move.getTo(lmove))
				{
					int captured = board.getPieceAt(Move.getTo(move));
					switch (board.LastCaptured)
					{
						
						case tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN: 
							if (captured == tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)
							{
								return EXTEND_RECAPTURE_PAWN;
							}
							break;
						
						case tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT: 
						case tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP: 
							if ((captured == tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) || (captured == tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP))
							{
								return EXTEND_RECAPTURE_MINOR;
							}
							break;
						
						case tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK: 
							if (captured == tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK)
							{
								return EXTEND_RECAPTURE_ROOK;
							}
							break;
						
						case tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN: 
							if (captured == tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN)
							{
								return EXTEND_RECAPTURE_QUEEN;
							}
							break;
						}
				}
			}
			return 0;
		}
		
		/// <summary> Check if the search should be extended for a passed pawn push.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <returns> the amount of extension
		/// </returns>
		internal virtual int extendPassedPawnPush(ChessBoard board, int move)
		{
			if (board.getPieceAt(Move.getFrom(move)) == tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)
			{
				int to = Move.getTo(move);
				if ((board.Wtm && to >= tgreiner.amy.bitboard.BoardConstants_Fields.A7 && to <= tgreiner.amy.bitboard.BoardConstants_Fields.H7) || (!board.Wtm && to >= tgreiner.amy.bitboard.BoardConstants_Fields.A2 && to <= tgreiner.amy.bitboard.BoardConstants_Fields.H2))
				{
					if (swapper.swap(board, move) >= 0)
					{
						return EXTEND_PASSED_PAWN;
					}
				}
			}
			return 0;
		}
		
		/// <seealso cref="SelectivityImpl.isFutile">
		/// </seealso>
		public virtual bool isFutile(ChessBoard board, int nextDepth, int move, int alpha)
		{
			
			if ((nextDepth <= 0) && Futility.isFutile(board, move, alpha))
			{
				return true;
			}
			else if (nextDepth <= NegaScout.DEPTH_STEP && nextDepth > 0 && Futility.isFutile2(board, move, alpha))
			{
				return true;
			}
			
			return false;
		}
	}
}