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
* $Id: GamePhase.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Enumeration class for game phase.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public sealed class GamePhase
	{
		/// <summary>The name of the game phase. </summary>
		private System.String name;
		
		/// <summary>Constant for Opening. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'OPENING '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly GamePhase OPENING = new GamePhase("Opening");
		
		/// <summary>Constant for Middlegame. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'MIDDLEGAME '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly GamePhase MIDDLEGAME = new GamePhase("Middlegame");
		
		/// <summary>Constant for Endgame. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ENDGAME '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly GamePhase ENDGAME = new GamePhase("Endgame");
		
		/// <summary> Create a game phase.
		/// 
		/// </summary>
		/// <param name="theName">the name.
		/// </param>
		private GamePhase(System.String theName)
		{
			this.name = theName;
		}
		
		/// <summary> Create a string representation of the game phase.
		/// 
		/// </summary>
		/// <returns> the name of the game phase.
		/// </returns>
		public override System.String ToString()
		{
			return name;
		}
	}
}