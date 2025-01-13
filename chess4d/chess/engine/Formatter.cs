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
* $Id: Formatter.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Utility class to format search engine output.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public sealed class Formatter
	{
		/// <summary> This class cannot be instantiated.</summary>
		private Formatter()
		{
		}
		
		/// <summary> Format a time.
		/// 
		/// </summary>
		/// <param name="time">the time
		/// </param>
		/// <returns> the formatted time
		/// </returns>
		public static System.String timeToString(int time)
		{
			int seconds = time / 1000;
			int tenths = (time - seconds * 1000) / 100;
			int minutes = seconds / 60;
			seconds -= minutes * 60;
			
			if (minutes == 0)
			{
				return seconds + "." + tenths;
			}
			else
			{
				return minutes + ":" + ((seconds < 10)?("0" + seconds):System.Convert.ToString(seconds)) + "." + tenths;
			}
		}
		
		/// <summary> Format a score.
		/// 
		/// </summary>
		/// <param name="score">the score
		/// </param>
		/// <returns> the formatted score
		/// </returns>
		public static System.String scoreToString(int score)
		{
			if (score > tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT)
			{
				int plies = (tgreiner.amy.chess.engine.Searcher_Fields.MATE - score + 1) / 2;
				return "+M" + plies;
			}
			else if (score < - tgreiner.amy.chess.engine.Searcher_Fields.MATE_LIMIT)
			{
				int plies = (tgreiner.amy.chess.engine.Searcher_Fields.MATE + score + 1) / 2;
				return "-M" + plies;
			}
			
			bool neg = score < 0;
			int absScore = System.Math.Abs(score);
			int pawns = absScore / 100;
			int centipawns = absScore - (100 * pawns);
			
			System.String cp = (centipawns < 10)?"0" + centipawns:System.Convert.ToString(centipawns);
			
			return (neg?"-":"") + pawns + "." + cp;
		}
	}
}