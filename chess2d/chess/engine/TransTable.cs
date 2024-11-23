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
* $Id: TransTable.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> The interface to access the transposition table.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public interface TransTable
	{
		
		/// <summary> Retrieve an entry from the transposition table.
		/// 
		/// </summary>
		/// <param name="hashkey">the hashkey of the entry.
		/// </param>
		/// <returns> the entry found or <code>null</code>.
		/// </returns>
		TTEntry get_Renamed(long hashkey);
		
		/// <summary> Store an entry in the transposition table.
		/// 
		/// </summary>
		/// <param name="hashkey">the hashkey.
		/// </param>
		/// <param name="move">the move.
		/// </param>
		/// <param name="depth">the search depth.
		/// </param>
		/// <param name="score">the score.
		/// </param>
		/// <param name="alpha">the alpha value.
		/// </param>
		/// <param name="beta">the beta value.
		/// </param>
		void  store(long hashkey, int move, int depth, int score, int alpha, int beta);
		
		/// <summary> Clear the transposition table.</summary>
		void  clear();
	}
}