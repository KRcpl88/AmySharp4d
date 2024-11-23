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
* $Id: GameImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.game
{
	
	/// <summary> Implementation of the Game interface.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class GameImpl : Game
	{
		/// <summary>The game's properties </summary>
		private System.Collections.IDictionary properties;
		
		/// <summary>The game's starting position. </summary>
		private PositionNode startingPosition;
		
		/// <summary>The game's result. </summary>
		private System.String result;
		
		/// <summary> Set the properties.
		/// 
		/// </summary>
		/// <param name="theProperties">the properties
		/// </param>
		public virtual void  setProperties(System.Collections.IDictionary theProperties)
		{
			this.properties = theProperties;
		}
		
		/// <seealso cref="Game.getProperties">
		/// </seealso>
		public virtual System.Collections.IDictionary getProperties()
		{
			return properties;
		}
		
		/// <summary> Set the starting position.
		/// 
		/// </summary>
		/// <param name="pos">the starting position
		/// </param>
		public virtual void  setStartingPosition(PositionNode pos)
		{
			this.startingPosition = pos;
		}
		
		/// <seealso cref="Game.getStartingPosition">
		/// </seealso>
		public virtual PositionNode getStartingPosition()
		{
			return startingPosition;
		}
		
		/// <summary> Set the result.
		/// 
		/// </summary>
		/// <param name="theResult">the result
		/// </param>
		public virtual void  setResult(System.String theResult)
		{
			this.result = theResult;
		}
		
		/// <seealso cref="Game.getResult">
		/// </seealso>
		public virtual System.String getResult()
		{
			return result;
		}
	}
}