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
* $Id: RootMoveList.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using IntVector = tgreiner.amy.common.engine.IntVector;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Contains the root moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class RootMoveList
	{
		/// <summary> Set the number of nodes searched for the current move.
		/// 
		/// </summary>
		/// <param name="nodes">the number of nodes searched
		/// </param>
		virtual public int Nodes
		{
			set
			{
				current.nodes = value;
			}
			
		}
		/// <summary> Get the best move.
		/// 
		/// </summary>
		/// <returns> the best move
		/// </returns>
        //public int Best
        //{
        //    get
        //    {
        //        return entries[0].move;
        //    }
			
        //}

        public int Best(int n)
        {
            while (entries.Length <= n)
                n--;

            return entries[n].move;
        }
		
		/// <summary>The current index. </summary>
		private int ptr = - 1;
		
		/// <summary>The current entry. </summary>
		private Entry current;
		
		/// <summary> Encapsulates an entry.</summary>
		private class Entry
		{
			/// <summary>The move. </summary>
			public int move;
			
			/// <summary>The number of nodes. </summary>
            public int nodes;
		}
		
		/// <summary>The entries. </summary>
		private Entry[] entries;
		
		/// <summary> Create a RootMoveList.
		/// 
		/// </summary>
		/// <param name="list">list of moves.
		/// </param>
		public RootMoveList(IntVector list)
		{
			entries = new Entry[list.size()];
			
			for (int i = 0; i < list.size(); i++)
			{
				Entry e = new Entry();
				e.move = list.get_Renamed(i);
				entries[i] = e;
			}
		}
		
		/// <summary> Test if there is a next move available.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if a next move is available
		/// </returns>
		public virtual bool hasNext()
		{
			return (ptr + 1) < entries.Length;
		}
		
		/// <summary> Get the next move.
		/// 
		/// </summary>
		/// <returns> the next move.
		/// </returns>
		public virtual int next()
		{
			ptr++;
			current = entries[ptr];
			return current.move;
		}
		
		/// <summary> Mark the current move as fail high. The current move is swapped to
		/// the beginning of the list.
		/// </summary>
		public virtual void  failHigh()
		{
			Entry tmp = entries[0];
			entries[0] = entries[ptr];
			entries[ptr] = tmp;
		}
		
		/// <summary> Resort the list. This sorts the moves in descending order with respect
		/// to the number of nodes searched.
		/// </summary>
		public virtual void  resort()
		{
			ptr = - 1;
			
			for (int i = 1; i < entries.Length; i++)
			{
				int bestidx = i;
				int nodes = entries[i].nodes;
				for (int j = i + 1; j < entries.Length; j++)
				{
					if (entries[j].nodes > nodes)
					{
						nodes = entries[j].nodes;
						bestidx = j;
					}
				}
				if (bestidx != i)
				{
					Entry tmp = entries[i];
					entries[i] = entries[bestidx];
					entries[bestidx] = tmp;
				}
			}
		}
	}
}