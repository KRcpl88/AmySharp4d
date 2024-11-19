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
* $Id: Searcher.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using NodeType = tgreiner.amy.common.engine.NodeType;
using TimeOutException = tgreiner.amy.common.timer.TimeOutException;
namespace tgreiner.amy.chess.engine
{
	
	
	/// <summary> The interface search algorithms have to implement.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public struct Searcher_Fields{
		/// <summary>Mate score. </summary>
		public readonly static int MATE = 32768;
		/// <summary>Mate limit score. </summary>
		public readonly static int MATE_LIMIT;
		static Searcher_Fields()
		{
			MATE_LIMIT = tgreiner.amy.chess.engine.Searcher_Fields.MATE - 100;
		}
	}
	public interface ISearcher
	{
		//UPGRADE_NOTE: Members of interface 'Searcher' were extracted into structure 'Searcher_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Get the number of nodes visited by the searcher.
		/// 
		/// </summary>
		/// <returns> the number of nodes.
		/// </returns>
		int Nodes
		{
			get;
			
		}
		
		/// <summary> Reset the searcher to start a new search.</summary>
		void  reset();
		
		/// <summary> Search with given alpha beta window.
		/// 
		/// </summary>
		/// <param name="alpha">the value of alpha.
		/// </param>
		/// <param name="beta">the value of beta.
		/// </param>
		/// <param name="depth">the search depth.
		/// </param>
		/// <param name="nodeType">the node type.
		/// </param>
		/// <returns> the score.
		/// </returns>
		/// <throws>  TimeOutException if the alloted time is exhausted </throws>
		int search(int alpha, int beta, int depth, NodeType nodeType);
	}
}