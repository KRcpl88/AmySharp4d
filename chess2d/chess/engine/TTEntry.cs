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
* $Id: TTEntry.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Data class holding a transposition table entry.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class TTEntry
	{
		/// <summary> Get the depth associated with this entry.
		/// 
		/// </summary>
		/// <returns> the depth.
		/// </returns>
		virtual public int Depth
		{
			get
			{
				return depth & DEPTH_MASK;
			}
			
		}
		/// <summary> Test if the score is an exact score.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the score is exact.
		/// </returns>
		virtual public bool Exact
		{
			get
			{
				return (depth & FLAG_MASK) == EXACT;
			}
			
		}
		/// <summary> Test if the score is a lower bound.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the score is a lower bound.
		/// </returns>
		virtual public bool Lower
		{
			get
			{
				return (depth & FLAG_MASK) == LOWER;
			}
			
		}
		/// <summary> Test if the score is an upper bound.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the score is an upper bound.
		/// </returns>
		virtual public bool Upper
		{
			get
			{
				return (depth & FLAG_MASK) == UPPER;
			}
			
		}
		/// <summary>Mask for the depth part. </summary>
		internal const int DEPTH_MASK = 0x3fff;
		
		/// <summary>Mask for the flags part. </summary>
		internal const int FLAG_MASK = 0xc000;
		
		/// <summary>Flags an exact score. </summary>
		internal const int EXACT = -16384; // 0xc000;
		/// <summary>Flags a lower bound. </summary>
		internal const int LOWER = -32768; // 0x8000;
		/// <summary>Flags an upper bound. </summary>
		internal const int UPPER = 0x4000;
		
		/// <summary>The haskey. </summary>
		public long hashkey;
		/// <summary>The move. </summary>
		public int move;
		/// <summary>The score. </summary>
		public short score;
		/// <summary>The depth. </summary>
		private short depth;
		
		/// <summary> Set the fields of this entry.
		/// 
		/// </summary>
		/// <param name="hashkey">the hashkey.
		/// </param>
		/// <param name="move">the move.
		/// </param>
		/// <param name="depth">the depth.
		/// </param>
		/// <param name="score">the score.
		/// </param>
		/// <param name="alpha">the alpha bound.
		/// </param>
		/// <param name="beta">the beta bound.
		/// </param>
		public virtual void  set_Renamed(long hashkey, int move, int depth, int score, int alpha, int beta)
		{
			this.hashkey = hashkey;
			this.move = move;
			this.score = (short) score;
			this.depth = (short) (depth & DEPTH_MASK);
			
			if (score <= alpha)
			{
				this.depth |= (short) (UPPER);
			}
			else if (score >= beta)
			{
				this.depth |= (short) (LOWER);
			}
			else
			{
				this.depth |= (short) (EXACT);
			}
		}
	}
}