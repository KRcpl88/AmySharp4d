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
* $Id: SearchOutputXBoard.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy.common.engine;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> An implementation of SearchOutput that writes to System.out.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class SearchOutputXBoard : SearchOutput
	{
		/// <summary> Set the post flag.
		/// 
		/// </summary>
		/// <param name="thePost">the post flag
		/// </param>
		virtual public bool Post
		{
			set
			{
				this.post = value;
			}
			
		}
		
		/// <summary>The threshold time after which to output. </summary>
		private int threshold = 300;
		
		
		/// <summary>The post flag. </summary>
		private bool post = true;

        private IComm comm;

		/// <summary> Create a SearchOutputXBoard.
		/// 
		/// </summary>
		/// <param name="theOutput">the output writer
		/// </param>
		public SearchOutputXBoard(IComm comm)
		{
			this.comm = comm;
		}
		
		/// <summary> Output the search header.</summary>
		public virtual void  header()
		{
		}
		
		/// <summary> Output the principal variation.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time (in ms).
		/// </param>
		/// <param name="score">the score.
		/// </param>
		/// <param name="pv">the principal variation.
		/// </param>
		/// <param name="nodes">the nodes
		/// </param>
		public virtual void  pv(int iteration, int time, int score, System.String pv, long nodes)
		{
			
			if (!post)
			{
				return ;
			}
			
			if (time >= threshold || score > tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT || score < - tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT)
			{
				this.comm.OnResponse(iteration.ToString());
				this.comm.OnResponse(" ");
				this.comm.OnResponse(score.ToString());
				this.comm.OnResponse(" ");
				this.comm.OnResponse((time / 10).ToString());
				this.comm.OnResponse(" ");
				this.comm.OnResponse(nodes.ToString());
				this.comm.OnResponse(" ");
				this.comm.OnResponse(pv + Environment.NewLine);
			}
		}
		
		/// <seealso cref="SearchOutput.move">
		/// </seealso>
		public virtual void  move(int iteration, int time, System.String move, int cnt, int total)
		{
		}
		
		/// <summary> Output a fail high.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="move">the current move
		/// </param>
		public virtual void  failHigh(int iteration, int time, System.String move)
		{
		}
		
		/// <summary> Output a fail low.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration.
		/// </param>
		/// <param name="time">the current search time.
		/// </param>
		/// <param name="move">the current move
		/// </param>
		public virtual void  failLow(int iteration, int time, System.String move)
		{
		}
	}
}