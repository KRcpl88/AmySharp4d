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
* $Id: Timer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> Interface for time management.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public interface Timer
	{
		/// <summary> Get the elapsed time of the search.
		/// 
		/// </summary>
		/// <returns> the time since the last call to startSearch() in milliseconds.
		/// </returns>
		int Time
		{
			get;
			
		}
		/// <summary> Checks if the time is up. If the timer has expired this method fires
		/// a {@link TimeOutException} otherwise it just returns.
		/// 
		/// </summary>
		/// <throws>  TimeOutException if this timer has expired. </throws>
		void  check();
		
		/// <summary> Signals start of search. Users of Timer must call this
		/// method at the beginning of a search.
		/// </summary>
		void  start();
		
		/// <summary> Signals the completion of an iteration of search. Users of Timer must
		/// call this method after they have completed an iteration.
		/// 
		/// </summary>
		/// <param name="iteration">the iteration that was completed
		/// </param>
		/// <throws>  TimeOutException if this timer has expired. </throws>
		void  iterationFinished(int iteration);
		
		/// <summary> Signals a fail low condition.</summary>
		void  failLow();
	}
}