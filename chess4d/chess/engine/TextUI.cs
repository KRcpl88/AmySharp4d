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
* $Id: TextUI.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using AlgorithmBasedTimer = tgreiner.amy.common.timer.AlgorithmBasedTimer;
using FixedTimeTimerAlgorithm = tgreiner.amy.common.timer.FixedTimeTimerAlgorithm;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Simple text based user interface.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class TextUI
	{
		/// <summary>The chessboard. </summary>
		private ChessBoard board;
		
		/// <summary>The reader to read command line input. </summary>
		private System.IO.StreamReader in_Renamed;
		
		/// <summary> Create a TextUI.</summary>
		public TextUI()
		{
			board = new ChessBoard();
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.Default).CurrentEncoding);
		}
		
		/// <summary> Run the command loop.
		/// 
		/// </summary>
		/// <throws>  IOException if an IO/Error occurs </throws>
		/// <throws>  IllegalSANException if an invalid move was entered </throws>
		public virtual void  run()
		{
			System.String line;
			
			TransTable ttable = new TransTableImpl(14);
			
			Evaluator evaluator = new EvaluatorImpl(board);
			board.Evaluator = evaluator;
			
			Driver d = new Driver(board, ttable, new AlgorithmBasedTimer(new FixedTimeTimerAlgorithm(5)));
			
			for (; ; )
			{
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
				System.Console.Out.WriteLine(board);
				line = in_Renamed.ReadLine();
				if (line == null)
				{
					break;
				}
				
				int move;
				try
				{
					move = Move.parseSAN(board, line);
				}
				catch (IllegalSANException ex)
				{
					System.Console.Out.WriteLine("Illegal move " + line);
					continue;
				}
				board.doMove(move);
				// move = d.search();
				// board.doMove(move);
			}
		}
		
		/// <summary> Main Driver.
		/// 
		/// </summary>
		/// <param name="args">the command line arguments
		/// </param>
		/// <throws>  Exception if an error occurs </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			TextUI t = new TextUI();
			t.run();
		}
	}
}