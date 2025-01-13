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
* $Id: PonderingTimerDecorator.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> A Timer implementation intended for use during pondering. It is implemented
	/// as a decorator which takes a Timer and intercepts all checking calls to the
	/// Timer until <code>stopPondering()</code> is invoked on it. At this point
	/// it will pass the checking calls to the underlying Timer which can then
	/// perform its normal checking.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class PonderingTimerDecorator : IChessTimer
	{
		/// <seealso cref="IChessTimer.getTime">
		/// </seealso>
		virtual public int Time
		{
			get
			{
				return decorated.Time;
			}
			
		}
		
		/// <summary>Indicates pondering mode. </summary>
		private bool pondering = true;
		
		/// <summary>Indicates aborted mode. </summary>
		private bool aborted = false;
		
		/// <summary>The decorated Timer. </summary>
		private IChessTimer decorated;
		
		/// <summary> Create a PonderingTimerDecorator.
		/// 
		/// </summary>
		/// <param name="theDecorated">the Timer to decorate.
		/// </param>
		public PonderingTimerDecorator(IChessTimer theDecorated)
		{
			this.decorated = theDecorated;
		}
		
		/// <seealso cref="IChessTimer.check">
		/// </seealso>
		public virtual void  check()
		{
			if (aborted)
			{
				throw new TimeOutException();
			}
			if (!pondering)
			{
				decorated.check();
			}
		}
		
		/// <seealso cref="IChessTimer.start">
		/// </seealso>
		public virtual void  start()
		{
			decorated.start();
		}
		
		/// <seealso cref="IChessTimer.iterationFinished">
		/// </seealso>
		public virtual void  iterationFinished(int iteration)
		{
			if (aborted)
			{
				throw new TimeOutException();
			}
			if (!pondering)
			{
				decorated.iterationFinished(iteration);
			}
		}
		
		/// <seealso cref="IChessTimer.failLow">
		/// </seealso>
		public virtual void  failLow()
		{
			decorated.failLow();
		}
		
		/// <summary> Stop the pondering mode.</summary>
		public virtual void  stopPondering()
		{
			this.pondering = false;
		}
		
		/// <summary> Abort pondering.</summary>
		public virtual void  abort()
		{
			aborted = true;
		}
	}
}