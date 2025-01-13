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
* $Id: ExtendOnFailLowTimerAlgorithm.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> A TimerAlgorithm which expires after a fixed amount of time.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class ExtendOnFailLowTimerAlgorithm:TimerAlgorithm
	{
		
		/// <summary>The current threshold. </summary>
		private int threshold;
		
		/// <summary>The expiration time. </summary>
		private int time;
		
		/// <summary>The extended time. </summary>
		private int extended;
		
		/// <summary> Create an ExtendOnFailLowTimerAlgorithm.
		/// 
		/// </summary>
		/// <param name="theTime">the time after which the timer shall expire if there is no
		/// fail low
		/// </param>
		/// <param name="theExtended">the maximum time to extend the search to if there is
		/// a fail low
		/// </param>
		public ExtendOnFailLowTimerAlgorithm(int theTime, int theExtended)
		{
			setDuration(theTime, theExtended);
		}
		
		/// <seealso cref="TimerAlgorithm.check">
		/// </seealso>
		public override void  check(int currentTime)
		{
			if (currentTime >= threshold)
			{
				throw new TimeOutException();
			}
		}
		
		/// <summary> Set the search duration.
		/// 
		/// </summary>
		/// <param name="theTime">the time after which the timer shall expire if there is no
		/// fail low
		/// </param>
		/// <param name="theExtended">the maximum time to extend the search to if there is
		/// a fail low
		/// </param>
		public virtual void  setDuration(int theTime, int theExtended)
		{
			this.time = theTime;
			this.extended = theExtended;
			this.threshold = time;
		}
		
		/// <seealso cref="TimerAlgorithm.failLow">
		/// </seealso>
		public override void  failLow()
		{
			threshold = extended;
		}
		
		/// <seealso cref="TimerAlgorithm.iterationFinished">
		/// </seealso>
		public override void  iterationFinished(int iteration)
		{
			threshold = time;
		}
	}
}