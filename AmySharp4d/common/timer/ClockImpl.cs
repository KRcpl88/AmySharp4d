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
* $Id: ClockImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> Implementation of a game clock.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class ClockImpl : Clock
	{
		/// <seealso cref="Clock.isRunning">
		/// </seealso>
		virtual public bool Running
		{
			get
			{
				return isRunning_Renamed_Field;
			}
			
		}
		
		/// <summary>The wall time. </summary>
		private long[] wallTime = new long[2];
		
		/// <summary>The time the clock was started. </summary>
		private long startTime;
		
		/// <summary>The side for which the clock is ticking. </summary>
		private int clockTickingFor;
		
		/// <summary>Indicates wether the clock is running. </summary>
		private bool isRunning_Renamed_Field;
		
		/// <seealso cref="Clock.start">
		/// </seealso>
		public virtual void  start(int side)
		{
			startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			clockTickingFor = side;
			isRunning_Renamed_Field = true;
		}
		
		/// <seealso cref="Clock.stop">
		/// </seealso>
		public virtual void  stop()
		{
			if (isRunning_Renamed_Field)
			{
				wallTime[clockTickingFor] += ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime);
				isRunning_Renamed_Field = false;
			}
		}
		
		/// <seealso cref="Clock.getWallTime">
		/// </seealso>
		public virtual int getWallTime(int side)
		{
			long time = wallTime[side];
			if (isRunning_Renamed_Field && (side == clockTickingFor))
			{
				time += ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime);
			}
			return (int) (time / 1000);
		}
		
		/// <seealso cref="Clock.reset">
		/// </seealso>
		public virtual void  reset()
		{
			wallTime[tgreiner.amy.common.timer.Clock_Fields.WHITE] = 0;
			wallTime[tgreiner.amy.common.timer.Clock_Fields.BLACK] = 0;
			isRunning_Renamed_Field = false;
		}
	}
}