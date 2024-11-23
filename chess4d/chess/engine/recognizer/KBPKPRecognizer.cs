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
* $Id: KBPKPRecognizer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
using ChessBoard = tgreiner.amy.chess.engine.ChessBoard;
//using ChessConstants = tgreiner.amy.chess.engine.ChessConstants;
using EvalMasks = tgreiner.amy.chess.engine.EvalMasks;
namespace tgreiner.amy.chess.engine.recognizer
{
	
	/// <summary> A recognizer for K+B+P versus K+P endgames.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class KBPKPRecognizer:KBPKRecognizer
	{
		
		/// <seealso cref="IRecognizer.probe">
		/// </seealso>
		public override int probe(ChessBoard board)
		{
			if (board.getMaterialSignature(false) == 1)
			{
				BitBoard blackPawns = board.getMask(false, ChessConstants_Fields.PAWN);
				if ((blackKingDefendsH8(board) && ((blackPawns & EvalMasks.FILE_MASK[6]).IsEmpty())) 
					|| ((blackKingDefendsA8(board) && (blackPawns & EvalMasks.FILE_MASK[1]).IsEmpty())))
				{
					if (board.WhiteToMove)
					{
						return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.UPPER_BOUND;
					}
					else
					{
						return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.LOWER_BOUND;
					}
				}
			}
			if (board.getMaterialSignature(true) == 1)
			{
				BitBoard whitePawns = board.getMask(true, ChessConstants_Fields.PAWN);
				if ((whiteKingDefendsH1(board) && (whitePawns & EvalMasks.FILE_MASK[6]).IsEmpty()) 
					|| (whiteKingDefendsA1(board) && (whitePawns & EvalMasks.FILE_MASK[1]).IsEmpty()))
				{
					if (board.WhiteToMove)
					{
						return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.LOWER_BOUND;
					}
					else
					{
						return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.UPPER_BOUND;
					}
				}
			}
			
			return tgreiner.amy.chess.engine.recognizer.Recognizer_Fields.USELESS;
		}
	}
}