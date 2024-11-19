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
* $Id: Benchmark.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using AlgorithmBasedTimer = tgreiner.amy.common.timer.AlgorithmBasedTimer;
using FixedTimeTimerAlgorithm = tgreiner.amy.common.timer.FixedTimeTimerAlgorithm;
using Timer = tgreiner.amy.common.timer.Timer;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> A benchmark for speed testing Java implementations and/or CPUs.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class Benchmark
	{
		
		/// <summary> This class cannot be instantiated.</summary>
		private Benchmark()
		{
		}
		
		/// <summary> The main method.
		/// 
		/// </summary>
		/// <param name="args">the command line arguments.
		/// </param>
		/// <throws>  Exception if an error occurs </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			ChessBoard board = new ChessBoard("r4k2/p3nppp/3q4/2Np1b2/1r1P3P/5QP1/P4PB1/2R1R1K1 w - -");
			
			TransTable ttable = new TransTableImpl2(14);
			Timer timer = new AlgorithmBasedTimer(new FixedTimeTimerAlgorithm(30 * 1000));
			
			Driver d = new Driver(board, ttable, timer);
			d.search();
		}
	}
}