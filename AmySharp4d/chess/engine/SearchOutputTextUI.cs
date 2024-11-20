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
* $Id: SearchOutputTextUI.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> An implementation of SearchOutput that writes to System.out.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class SearchOutputTextUI : ISearchOutput
	{
		
		/// <summary>The threshold in microseconds after which search output is created. </summary>
		private int threshold = 300;
		
		/// <summary>The text width. </summary>
		private const int WIDTH = 80;
		
		/// <summary> Output the search header.</summary>
		public virtual void  header()
		{
			System.Console.Out.WriteLine("It    Time   Score  principal Variation");
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
			if (time >= threshold || score > tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT || score < - tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT)
			{
				System.String line = formatIteration(iteration) + formatTime(time) + formatScore(score) + "  " + formatPV(pv + " [" + formatNodes(nodes) + "]");
				System.Console.Out.WriteLine(line);
			}
		}
		
		/// <seealso cref="ISearchOutput.move">
		/// </seealso>
		public virtual void  move(int iteration, int time, System.String move, int cnt, int total)
		{
			if (false && time >= threshold)
			{
				System.String line = formatIteration(iteration) + formatTime(time) + formatCnt(cnt, total) + "  " + move;
				System.Console.Out.Write(line + "     \r");
			}
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
			if (time >= threshold)
			{
				System.String line = formatIteration(iteration) + formatTime(time) + "     +++  " + move;
				System.Console.Out.WriteLine(line + "     ");
			}
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
			if (time >= threshold)
			{
				System.String line = formatIteration(iteration) + formatTime(time) + "     ---  " + move;
				System.Console.Out.WriteLine(line + "     ");
			}
		}
		
		/// <summary> Format an iteration.
		/// 
		/// </summary>
		/// <param name="iter">the iteration
		/// </param>
		/// <returns> iter formatted
		/// </returns>
		private System.String formatIteration(int iter)
		{
			System.String result = System.Convert.ToString(iter);
			if (iter < 10)
			{
				result = " " + result;
			}
			return result;
		}
		
		/// <summary> Format the time.
		/// 
		/// </summary>
		/// <param name="time">the time
		/// </param>
		/// <returns> time formatted
		/// </returns>
		private System.String formatTime(int time)
		{
			System.String result = Formatter.timeToString(time);
			return "        ".Substring(result.Length) + result;
		}
		
		/// <summary> Format the score.
		/// 
		/// </summary>
		/// <param name="score">the score
		/// </param>
		/// <returns> score formatted
		/// </returns>
		private System.String formatScore(int score)
		{
			System.String result = Formatter.scoreToString(score);
			return "        ".Substring(result.Length) + result;
		}
		
		/// <summary> Format the move count.
		/// 
		/// </summary>
		/// <param name="cnt">the current move count
		/// </param>
		/// <param name="total">the total move count
		/// </param>
		/// <returns> the move count formatted
		/// </returns>
		private System.String formatCnt(int cnt, int total)
		{
			System.String result = (cnt + 1) + "/" + total;
			return "        ".Substring(result.Length) + result;
		}
		
		/// <summary> Format the number of nodes.
		/// 
		/// </summary>
		/// <param name="nodes">the number of nodes
		/// </param>
		/// <returns> nodes expressed as kiloNodes
		/// </returns>
		private System.String formatNodes(long nodes)
		{
			if (nodes >= 1000)
			{
				return (nodes / 1000) + " kN";
			}
			else
			{
				return "0." + (nodes / 100) + " kN";
			}
		}
		
		/// <summary> Format the PV by applying a line breaking algorithm.
		/// 
		/// </summary>
		/// <param name="thePV">the pv
		/// </param>
		/// <returns> the formatted pv
		/// </returns>
		internal virtual System.String formatPV(System.String thePV)
		{
			System.String prefix = "                    ";
			int maxLen = WIDTH - prefix.Length - 1;
			if (thePV.Length < maxLen)
			{
				return thePV;
			}
			
			SupportClass.Tokenizer st = new SupportClass.Tokenizer(thePV);
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			System.Text.StringBuilder line = new System.Text.StringBuilder();
			bool withPrefix = false;
			
			while (st.HasMoreTokens())
			{
				System.String token = st.NextToken();
				
				if (line.Length + token.Length + 1 < maxLen)
				{
					if (line.Length != 0)
					{
						line.Append(" ");
					}
					line.Append(token);
				}
				else
				{
					if (withPrefix)
					{
						result.Append(prefix);
					}
					result.Append(line);
					result.Append("\n");
					line = new System.Text.StringBuilder(token);
					withPrefix = true;
				}
			}
			if (line.Length != 0)
			{
				if (withPrefix)
				{
					result.Append(prefix);
				}
				result.Append(line);
			}
			
			return result.ToString();
		}
	}
}