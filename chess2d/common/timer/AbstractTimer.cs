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
* $Id: AbstractTimer.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.log4net;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> Abstract base class for timer implementations which provides basic
	/// time keeping functionality.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public abstract class AbstractTimer : Timer
	{
		/// <seealso cref="Timer.getTime">
		/// </seealso>
		virtual public int Time
		{
			get
			{
				return (int) ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime);
			}
			
		}
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.common.timer.AbstractTimer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>time of search start. </summary>
		protected internal long startTime;
		
		/// <seealso cref="Timer.start">
		/// </seealso>
		public virtual void  start()
		{
			startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
		}
		public abstract void  check();
		public abstract void  failLow();
		public abstract void  iterationFinished(int param1);
		static AbstractTimer()
		{
			log = LogManager.GetLogger(typeof(AbstractTimer));
		}
	}
}