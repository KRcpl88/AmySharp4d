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
* $Id: FixedDepthTimerAlgorithm.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> A TimerAlgorithm which expires after a fixed depth.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class FixedDepthTimerAlgorithm:TimerAlgorithm
	{
		
		/// <summary>The expiration time. </summary>
		private int depth;
		
		/// <summary> Create a FixedDepthTimerAlgorithm.
		/// 
		/// </summary>
		/// <param name="theDepth">the search depth
		/// </param>
		public FixedDepthTimerAlgorithm(int theDepth)
		{
			this.depth = theDepth;
		}
		
		/// <seealso cref="Timer.iterationFinished">
		/// </seealso>
		public override void  iterationFinished(int currentDepth)
		{
			if (currentDepth >= depth)
			{
				throw new TimeOutException();
			}
		}
	}
}