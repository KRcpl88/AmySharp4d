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
* $Id: Clock.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.timer
{
	
	/// <summary> Interface of a game clock.</summary>
	public struct Clock_Fields{
		/// <summary>Constant for white.  </summary>
		public readonly static int WHITE = 0;
		/// <summary>Constant for black.  </summary>
		public readonly static int BLACK = 1;
	}
	public interface Clock
	{
		//UPGRADE_NOTE: Members of interface 'Clock' were extracted into structure 'Clock_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Check if the clock is running.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the clock is running.
		/// </returns>
		bool Running
		{
			get;
			
		}
		
		/// <summary> Start the clock for a side.
		/// 
		/// </summary>
		/// <param name="side">the side, either <code>BLACK</code> or <code>White</code>
		/// </param>
		void  start(int side);
		
		/// <summary> Stop the clock.</summary>
		void  stop();
		
		/// <summary> Get the wall time for a side.
		/// 
		/// </summary>
		/// <param name="side">the side, either <code>BLACK</code> or <code>White</code>
		/// </param>
		/// <returns> the wall time for <code>side</code>
		/// </returns>
		int getWallTime(int side);
		
		/// <summary> Reset the clock.</summary>
		void  reset();
	}
}