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
* $Id: GameEndRecognizer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using IntVector = tgreiner.amy.common.engine.IntVector;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Determines wether a game is ended.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class GameEndRecognizer
	{
		/// <summary> Get the result string.
		/// 
		/// </summary>
		/// <returns> the result string (one of "1-0", "0-1" or "1/2-1/2")
		/// </returns>
		virtual public System.String Result
		{
			get
			{
				return result;
			}
			
		}
		/// <summary> Get the comment.
		/// 
		/// </summary>
		/// <returns> the comment (e.g. "White mates")
		/// </returns>
		virtual public System.String Comment
		{
			get
			{
				return comment;
			}
			
		}
		
		/// <summary>Constant for draw result. </summary>
		public const System.String DRAW = "1/2-1/2";
		
		/// <summary>Constant for white wins result. </summary>
		public const System.String WHITE_WINS = "1-0";
		
		/// <summary>Constant for black wins result. </summary>
		public const System.String BLACK_WINS = "0-1";
		
		/// <summary>Holds the legal moves. </summary>
		private IntVector legalMoves = new IntVector();
		
		/// <summary>The result. </summary>
		private System.String result;
		
		/// <summary>The additional comment. </summary>
		private System.String comment;
		
		/// <summary> Creata a GameEndRecognizer.</summary>
		public GameEndRecognizer()
		{
		}
		
		/// <summary> Checks wether the game played on the ChessBoard is ended according
		/// to the rules of chess.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> <code>true</code> if the game is ended.
		/// </returns>
		public virtual bool isGameEnded(ChessBoard board)
		{
			legalMoves.Size = 0;
			board.generateLegalMoves(legalMoves);
			if (legalMoves.size() == 0)
			{
				if (board.InCheck)
				{
					if (board.Wtm)
					{
						result = BLACK_WINS;
						comment = "Black mates";
					}
					else
					{
						result = WHITE_WINS;
						comment = "White mates";
					}
				}
				else
				{
					result = DRAW;
					comment = "Stalemate";
				}
				return true;
			}
			else if (board.DrawByRepetition)
			{
				result = DRAW;
				comment = "Draw by repetition";
				return true;
			}
			else if (board.FiftyMoveRuleDraw)
			{
				result = DRAW;
				comment = "Draw by fifty move rule";
				return true;
			}
			else if (board.InsufficientMaterial)
			{
				result = DRAW;
				comment = "Insufficient material";
				return true;
			}
			
			return false;
		}
	}
}