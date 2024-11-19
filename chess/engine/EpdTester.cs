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
* $Id: EpdTester.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Text;
using System.Text.RegularExpressions;
using AlgorithmBasedTimer = tgreiner.amy.common.timer.AlgorithmBasedTimer;
using FixedDepthTimerAlgorithm = tgreiner.amy.common.timer.FixedDepthTimerAlgorithm;
using FixedTimeTimerAlgorithm = tgreiner.amy.common.timer.FixedTimeTimerAlgorithm;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Simple test driver for running EPD test suites.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class EpdTester
	{
		
		/// <summary>The reader from which EPD data is read. </summary>
		private System.IO.StreamReader in_Renamed;
		
		/// <summary>The time. </summary>
		private int time;
		
		/// <summary> Create an EpdTester.
		/// 
		/// </summary>
		/// <param name="theReader">the reader from which EPD data is read
		/// </param>
		/// <param name="theTime">the time
		/// </param>
		public EpdTester(System.IO.StreamReader theReader, int theTime)
		{
			this.in_Renamed = theReader;
			this.time = theTime;
		}
		
		/// <summary> Run a Testsuite.
		/// 
		/// </summary>
		/// <throws>  IOException if an I/O error occurs </throws>
		public virtual void  run()
		{
			System.String line;
			
			TransTable ttable = new TransTableImpl2(21);
			
			int ok = 0;
			int bad = 0;
			
			while ((line = in_Renamed.ReadLine()) != null)
			{
				System.Console.Out.WriteLine(line);
				try
				{
					ChessBoard board = new ChessBoard(line);
					
					Driver d = new Driver(board, ttable, new AlgorithmBasedTimer(new FixedTimeTimerAlgorithm(time)));
					
					//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
					System.Console.Out.WriteLine(board);
					int move = d.search();
					
					if (isSolution(board, move, line))
					{
						ok++;
					}
					else
					{
						bad++;
						addToNotSolved(line);
					}
					
					System.Console.Out.WriteLine("Solved " + ok + " of " + (ok + bad));
				}
				catch (IllegalEpdException ex)
				{
					System.Console.Error.WriteLine("Illegal epd " + line);
				}
			}
		}
		
		/// <summary>Pattern for a 'best move' entry. </summary>
		private Regex bmPattern = new Regex(".*bm ([^;]*);.*");
		
		/// <summary>Pattern for an 'avoid move' entry. </summary>
        private Regex amPattern = new Regex(".*am ([^;]*);.*");
		
		/// <summary> Checks if a given move is a solution.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <param name="epd">the EPD
		/// </param>
		/// <returns> <code>true</code> if move is present in the 'bm' tag of the
		/// EPD or absent in the 'am' tag
		/// </returns>
		private bool isSolution(ChessBoard board, int move, System.String epd)
		{
            Match bm = bmPattern.Match(epd);
			if (bm.Success)
			{
				System.String moves = bm.Groups[1].Value;
				
				SupportClass.Tokenizer tok = new SupportClass.Tokenizer(moves, " ");
				while (tok.HasMoreTokens())
				{
					System.String san = tok.NextToken();
					try
					{
						if (Move.parseSAN(board, san) == move)
						{
							return true;
						}
					}
					catch (IllegalSANException ex)
					{
						// Ignored
					}
				}
			}

            Match am = amPattern.Match(epd);
			if (am.Success)
			{
				System.String moves = am.Groups[1].Value;
				
				SupportClass.Tokenizer tok = new SupportClass.Tokenizer(moves, " ");
				while (tok.HasMoreTokens())
				{
					System.String san = tok.NextToken();
					try
					{
						if (Move.parseSAN(board, san) == move)
						{
							return false;
						}
					}
					catch (IllegalSANException ex)
					{
						// Ignored
					}
				}
				return true;
			}
			
			return false;
		}
		
		/// <summary> Add an entry to the 'nsolved.epd' file.
		/// 
		/// </summary>
		/// <param name="line">the line
		/// </param>
		private void  addToNotSolved(System.String line)
		{
			try
			{
				//UPGRADE_TODO: Class 'java.io.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriter'"
				//UPGRADE_TODO: Constructor 'java.io.FileWriter.FileWriter' was converted to 'System.IO.StreamWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileWriterFileWriter_javalangString_boolean'"
				System.IO.StreamWriter fout = new System.IO.StreamWriter("nsolved.epd", true, System.Text.Encoding.Default);
				fout.Write(line);
				fout.Write("\n");
				fout.Close();
			}
			catch (System.IO.IOException ex)
			{
				// IGNORED
			}
		}
		
		/// <summary> The main method to run the EpdTester as a command line application.
		/// 
		/// </summary>
		/// <param name="args">the command line arguments
		/// </param>
		/// <throws>  IOException if there is a problem accessing the input file </throws>
		[STAThread]
		public static void  Main(System.String[] args)
		{
			System.String filename = args[0];
			
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.BufferedReader.BufferedReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			//UPGRADE_TODO: Constructor 'java.io.FileReader.FileReader' was converted to 'System.IO.StreamReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
			System.IO.StreamReader in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(filename, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(filename, System.Text.Encoding.Default).CurrentEncoding);
			
			System.Console.Out.WriteLine("Warming up search...");
			ChessBoard b = new ChessBoard();
			TransTable ttable = new TransTableImpl(10);
			Driver d = new Driver(b, ttable, new AlgorithmBasedTimer(new FixedDepthTimerAlgorithm(6)));
			d.search();
			
			int time = 60;
			if (args.Length > 1)
			{
				time = System.Int32.Parse(args[1]);
			}
			
			EpdTester e = new EpdTester(in_Renamed, time * 1000);
			
			e.run();
		}
	}
}