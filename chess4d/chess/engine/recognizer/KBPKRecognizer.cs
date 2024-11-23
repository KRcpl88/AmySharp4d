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
* $Id: KBPKRecognizer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using ChessBoard = tgreiner.amy.chess.engine.ChessBoard;
//using ChessConstants = tgreiner.amy.chess.engine.ChessConstants;
using EvalMasks = tgreiner.amy.chess.engine.EvalMasks;
namespace tgreiner.amy.chess.engine.recognizer
{
	
	/// <summary> A recognizer for K+B+P versus K endgames.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class KBPKRecognizer : IRecognizer
	{
		/// <seealso cref="IRecognizer.getValue">
		/// </seealso>
		virtual public int Value
		{
			get
			{
				return 0;
			}
			
		}
		
		/// <seealso cref="IRecognizer.probe">
		/// </seealso>
		public virtual int probe(ChessBoard board)
		{
			if (board.getMaterialSignature(false) == 0)
			{
				if (blackKingDefendsH8(board) || blackKingDefendsA8(board))
				{
					return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.EXACT;
				}
			}
			if (board.getMaterialSignature(true) == 0)
			{
				if (whiteKingDefendsH1(board) || whiteKingDefendsA1(board))
				{
					return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.EXACT;
				}
			}
			
			return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.USELESS;
		}
		
		/// <summary> Check wether the black king defends h8.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> <code>true</code> if the black king defends h8.
		/// </returns>
		internal virtual bool blackKingDefendsH8(ChessBoard board)
		{
			
			if ((board.getMask(true, ChessConstants_Fields.BISHOP) & EvalMasks.BLACK_SQUARES).IsEmpty() == false)
			{
				return false;
			}
			
			if ((board.getMask(true, ChessConstants_Fields.PAWN) & ~ EvalMasks.FILE_MASK[7]).IsEmpty() == false)
			{
				return false;
			}
			
			int square = board.getKingPos(false);
			return (square >> 3) >= 6 && (square & 7) >= 6;
		}
		
		/// <summary> Check wether the black king defends a8.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> <code>true</code> if the black king defends a8.
		/// </returns>
		internal virtual bool blackKingDefendsA8(ChessBoard board)
		{
			
			if ((board.getMask(true, ChessConstants_Fields.BISHOP) & EvalMasks.WHITE_SQUARES).IsEmpty() == false)
			{
				return false;
			}
			
			if ((board.getMask(true, ChessConstants_Fields.PAWN) & ~ EvalMasks.FILE_MASK[0]).IsEmpty() == false)
			{
				return false;
			}
			
			int square = board.getKingPos(false);
			return (square >> 3) >= 6 && (square & 7) <= 1;
		}
		
		/// <summary> Check wether the white king defends h1.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> <code>true</code> if the white king defends h1.
		/// </returns>
		internal virtual bool whiteKingDefendsH1(ChessBoard board)
		{
			
			if ((board.getMask(false, ChessConstants_Fields.BISHOP) & EvalMasks.WHITE_SQUARES).IsEmpty() == false)
			{
				return false;
			}
			
			if ((board.getMask(false, ChessConstants_Fields.PAWN) & ~ EvalMasks.FILE_MASK[7]).IsEmpty() == false)
			{
				return false;
			}
			
			int square = board.getKingPos(true);
			// BUGBUG everywhere it says xxx >> 3 we are converting square to file
			// BUGBUG everywhere it says xxx & 7 we are converting square to rank
			// BUGBUG this logic only works on a 2D board
			return (square >> 3) <= 1 && (square & 7) >= 6;
		}
		
		/// <summary> Check wether the white king defends a1.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> <code>true</code> if the white king defends a1.
		/// </returns>
		internal virtual bool whiteKingDefendsA1(ChessBoard board)
		{
			
			if ((board.getMask(false, ChessConstants_Fields.BISHOP) & EvalMasks.BLACK_SQUARES).IsEmpty() == false)
			{
				return false;
			}
			
			if ((board.getMask(false, ChessConstants_Fields.PAWN) & ~ EvalMasks.FILE_MASK[0]).IsEmpty() == false)
			{
				return false;
			}
			
			int square = board.getKingPos(true);
			return (square >> 3) <= 1 && (square & 7) <= 1;
		}
	}
}