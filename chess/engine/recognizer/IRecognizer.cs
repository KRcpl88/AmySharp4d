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
* $Id: Recognizer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using ChessBoard = tgreiner.amy.chess.engine.ChessBoard;
namespace tgreiner.amy.chess.engine.recognizer
{
	
	/// <summary> A recognizer detects certain kind of endgames and scores them.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public struct Recognizer_Fields{
		/// <summary>Indicates that an endgame could not be assessed. </summary>
		public readonly static int USELESS = 0;
		/// <summary>Indicates that an endgame could be scored exactly. </summary>
		public readonly static int EXACT = 1;
		/// <summary>Indicates that the score returned by getValue is a lower bound. </summary>
		public readonly static int LOWER_BOUND = 2;
		/// <summary>Indicates that the score returned by getValue is an upper bound. </summary>
		public readonly static int UPPER_BOUND = 3;
	}
	public interface IRecognizer
	{
		//UPGRADE_NOTE: Members of interface 'Recognizer' were extracted into structure 'Recognizer_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Return the score if a previous call to probe indicated a successful
		/// recognition.
		/// 
		/// </summary>
		/// <returns> the score. It is the responsibility of the caller to interpret
		/// the score as exact, upper or lower bound, depending on the
		/// return value of probe().
		/// </returns>
		int Value
		{
			get;
			
		}
		
		/// <summary> Probes a ChessBoard.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> an integer indicating wether the endgame could be scored exactly
		/// (EXACT), an upper bound (UPPER) or a lower bound (LOWER). Returns
		/// USELESS if the endgame could not be scored.
		/// </returns>
		int probe(ChessBoard board);
	}
}