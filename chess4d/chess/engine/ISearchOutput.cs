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
* $Id: SearchOutput.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> SearchOutput is used by Driver to output search status.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public interface ISearchOutput
	{
		/// <summary> Output the search header.</summary>
		void  header();
		
		/// <summary> Output the principal variation.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="score">the score.
		/// </param>
		/// <param name="pv">the principal variation.
		/// </param>
		/// <param name="nodes">number of nodes searched.
		/// </param>
		void  pv(int iteration, int time, int score, System.String pv, long nodes);
		
		/// <summary> Output the move under investigation.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="move">the current move
		/// </param>
		/// <param name="count">the number of the move
		/// </param>
		/// <param name="total">the total number of moves
		/// </param>
		void  move(int iteration, int time, System.String move, int count, int total);
		
		/// <summary> Output a fail high.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="move">the current move
		/// </param>
		void  failHigh(int iteration, int time, System.String move);
		
		/// <summary> Output a fail high.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="move">the current move
		/// </param>
		void  failLow(int iteration, int time, System.String move);
	}
}