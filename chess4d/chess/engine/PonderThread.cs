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
* $Id: PonderThread.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Threading;
using PonderingTimerDecorator = tgreiner.amy.common.timer.PonderingTimerDecorator;
using IChessTimer = tgreiner.amy.common.timer.IChessTimer;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
using tgreiner.amy;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Implements a ponder thread.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class PonderThread
	{
		/// <summary> Get the ponder move.
		/// 
		/// </summary>
		/// <returns> the ponder move
		/// </returns>
		virtual public int PonderMove
		{
			get
			{
				return ponderMove;
			}
			
		}
		/// <summary> Get the best move.
		/// 
		/// </summary>
		/// <returns> the best move
		/// </returns>
		virtual public int BestMove
		{
			get
			{
				return bestMove;
			}
			
		}
		/// <summary> Get the move to ponder on next.
		/// 
		/// </summary>
		/// <returns> the move to ponder on next.
		/// </returns>
		virtual public int NextPonderMove
		{
			get
			{
				return nextPonderMove;
			}
			
		}
		
		/// <summary>The log4j Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.PonderThread'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>The timer. </summary>
		private PonderingTimerDecorator ponderTimer;
		
		/// <summary>The Java thread which ponders. </summary>
		private Thread ponderThread;
		
		/// <summary>The board. </summary>
		private ChessBoard board;
		
		/// <summary>The transposition table. </summary>
		private ITransTable transTable;
		
		/// <summary>The search output. </summary>
		private ISearchOutput searchOutput;
		
		/// <summary>The move to ponder on. </summary>
		private int ponderMove;
		
		/// <summary>The best move. </summary>
		private int bestMove;
		
		/// <summary>The move to ponder on next. </summary>
		private int nextPonderMove;

        private int maxDepth;

		/// <summary> Create a PonderThread.
		/// 
		/// </summary>
		/// <param name="theBoard">the board to search
		/// </param>
		/// <param name="thePonderMove">the move
		/// </param>
		/// <param name="theTransTable">the transposition table
		/// </param>
		/// <param name="theTimer">the timer
		/// </param>
		/// <param name="theSearchOutput">the search output
		/// </param>
		public PonderThread(ChessBoard theBoard, int thePonderMove, ITransTable theTransTable, IChessTimer theTimer, ISearchOutput theSearchOutput, int maxDepth)
		{
			
			this.board = new ChessBoard(theBoard);
			this.ponderMove = thePonderMove;
			this.board.doMove(thePonderMove);
			this.ponderTimer = new PonderingTimerDecorator(theTimer);
			this.transTable = theTransTable;
			this.searchOutput = theSearchOutput;
            this.maxDepth = maxDepth;
			
			this.ponderThread = new Thread(this.Run);
			this.ponderThread.Start();
		}
		
		/// <seealso cref="Runnable.run">
		/// </seealso>
		public virtual void  Run()
		{
			Driver driver = new Driver(board, transTable, ponderTimer);
			driver.SearchOutput = searchOutput;
			bestMove = driver.search(this.maxDepth);
			nextPonderMove = driver.PonderMove;
		}
		
		/// <summary> Abort pondering.</summary>
		public virtual void  abort()
		{
			try
			{
				ponderTimer.abort();
				ponderThread.Join();
			}
			catch (ThreadAbortException)
			{
				// IGNORED
			}
		}
		
		/// <summary> The ponder move has been performed, continue searching.
		/// This method blocks until the search returns.
		/// </summary>
		public virtual void  ponderHit()
		{
			try
			{
				ponderTimer.stopPondering();
				ponderThread.Join();
			}
            catch (ThreadAbortException)
			{
				// IGNORED
			}
		}
		static PonderThread()
		{
			log = LogManager.GetLogger();
		}
	}
}