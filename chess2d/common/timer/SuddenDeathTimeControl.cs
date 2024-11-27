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
* $Id: SuddenDeathTimeControl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> An implementation of TimeControl which represents a time control of
	/// all moves in a given amount of time.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class SuddenDeathTimeControl : TimeControl
	{
		/// <seealso cref="TimeControl.getSoftLimit">
		/// </seealso>
		virtual public int SoftLimit
		{
			get
			{
				int limit = (MICROS_PER_SECOND * time) / MOVES;
				if (limit > remainingTime)
				{
					limit = remainingTime / 2;
				}
				
				return limit;
			}
			
		}
		/// <seealso cref="TimeControl.getHardLimit">
		/// </seealso>
		virtual public int HardLimit
		{
			get
			{
				int limit = 4 * SoftLimit;
				if (limit > remainingTime)
				{
					limit = (3 * remainingTime) / 4;
				}
				
				return limit;
			}
			
		}
		/// <seealso cref="TimeControl.setRemainingTime">
		/// </seealso>
		virtual public int RemainingTime
		{
			set
			{
				remainingTime = value;
			}
			
		}
		
		/// <summary>Constant for microseconds per second. </summary>
		private const int MICROS_PER_SECOND = 1000;
		
		/// <summary>The number of moves allocated for the rest of the game. </summary>
		private const int MOVES = 60;
		
		/// <summary>The time. </summary>
		private int time;
		
		/// <summary>The remaining time. </summary>
		private int remainingTime;
		
		/// <summary> Create a SuddenDeathTimeControl.
		/// 
		/// </summary>
		/// <param name="theTime">the time (in seconds)
		/// </param>
		public SuddenDeathTimeControl(int theTime)
		{
			this.time = theTime;
			this.remainingTime = MICROS_PER_SECOND * theTime;
		}
	}
}