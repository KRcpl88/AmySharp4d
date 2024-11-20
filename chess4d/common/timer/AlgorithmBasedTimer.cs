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
* $Id: AlgorithmBasedTimer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> A Timer based on a TimerAlgorithm.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class AlgorithmBasedTimer:AbstractTimer
	{
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.common.timer.AlgorithmBasedTimer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>The last time check() was called. </summary>
		private long lastCheckTime;
		
		/// <summary>check current time every calls time. </summary>
		private const int CHECK_CALLS = 10000;
		
		/// <summary> The number of times check() is called befored it actually checks
		/// the system time.
		/// </summary>
		private int callsBetweenChecks = CHECK_CALLS;
		
		/// <summary>counts calls. </summary>
		private int calls = 0;
		
		/// <summary>The algorithm this timer uses. </summary>
		private TimerAlgorithm algorithm;
		
		/// <summary> Create an AlgorithmBasedTimer.
		/// 
		/// </summary>
		/// <param name="theAlgorithm">the algorithm.
		/// </param>
		public AlgorithmBasedTimer(TimerAlgorithm theAlgorithm)
		{
			this.algorithm = theAlgorithm;
		}
		
		/// <seealso cref="Timer.check">
		/// </seealso>
		public override void  check()
		{
			if (--calls <= 0)
			{
				// System.out.println("---> Check <---                          ");
				long now = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				if (now > lastCheckTime)
				{
					
					algorithm.check((int) (now - startTime));
					
					double factor = 100.0 / (now - lastCheckTime);
					
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					callsBetweenChecks = (int) (callsBetweenChecks * factor);
					if (log.IsDebugEnabled)
					{
						log.Debug("Adjusted callsBetweenChecks to " + callsBetweenChecks + "   ");
					}
					
					calls = callsBetweenChecks;
					lastCheckTime = now;
				}
			}
		}
		
		/// <seealso cref="Timer.start">
		/// </seealso>
		public override void  start()
		{
			base.start();
			lastCheckTime = startTime;
			calls = callsBetweenChecks;
		}
		
		/// <seealso cref="Timer.iterationFinished">
		/// </seealso>
		public override void  iterationFinished(int iteration)
		{
			algorithm.iterationFinished(iteration);
		}
		
		/// <seealso cref="Timer.failLow">
		/// </seealso>
		public override void  failLow()
		{
			algorithm.failLow();
		}
		static AlgorithmBasedTimer()
		{
			log = LogManager.GetLogger(typeof(AlgorithmBasedTimer));
		}
	}
}