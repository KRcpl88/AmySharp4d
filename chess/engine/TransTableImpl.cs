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
* $Id: TransTableImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> An implementation of a simple transposition table.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	/*
	public class TransTableImpl : ITransTable
	{
		/// <summary>Size of the transposition table. </summary>
		private int size;
		
		/// <summary>Mask for the hashkey. </summary>
		private int mask;
		
		/// <summary>The transposition table. </summary>
		private TTEntry[] table;
		
		/// <summary> Create a TranspositionTable.
		/// 
		/// </summary>
		/// <param name="bits">the number of entries in the table. This implementation
		/// requires size to be a power of two.
		/// </param>
		public TransTableImpl(int bits)
		{
			size = (1 << bits);
			mask = size - 1;
			
			table = new TTEntry[size];
			for (int i = 0; i < size; i++)
			{
				table[i] = new TTEntry();
			}
		}
		
		/// <summary> Retrieve an entry from the transposition table.
		/// 
		/// </summary>
		/// <param name="hashkey">the hashkey of the entry.
		/// </param>
		/// <returns> the entry found or <code>null</code>.
		/// </returns>
		public virtual TTEntry get_Renamed(long hashkey)
		{
			int idx = (int) (hashkey & mask);
			TTEntry entry = table[idx];
			
			if (entry != null && entry.hashkey != hashkey)
			{
				entry = null;
			}
			
			return entry;
		}
		
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
		public virtual void  store(long hashkey, int move, int depth, int score, int alpha, int beta)
		{
			int idx = (int) (hashkey & mask);
			TTEntry entry = table[idx];
			if (depth > entry.Depth)
			{
				entry.set_Renamed(hashkey, move, depth, score, alpha, beta);
			}
		}
		
		/// <summary> Clear the transposition table.</summary>
		public virtual void  clear()
		{
			for (int i = 0; i < size; i++)
			{
				table[i].set_Renamed(0, 0, 0, 0, 0, 0);
			}
		}
	}
	*/
}