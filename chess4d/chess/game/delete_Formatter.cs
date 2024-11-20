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
* $Id: Formatter.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.game
{
	
	/// <summary> Formats games.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	
	// BUGBUG not used
	/*
	public class Formatter
	{
		
		/// <summary> Format a game text.
		/// 
		/// </summary>
		/// <param name="rootNode">the game root node
		/// </param>
		/// <returns> the moves as formatted String
		/// </returns>
		public virtual System.String formatMoves(IPositionNode rootNode)
		{
			IPositionNode current = rootNode;
			System.Text.StringBuilder text = new System.Text.StringBuilder();
			int ply = 0;
			while (current != null)
			{
				IMoveNode moveNode = current.getPV();
				if (moveNode == null)
				{
					break;
				}
				if ((ply & 1) == 0)
				{
					text.Append(1 + ply / 2);
					text.Append(". ");
				}
				text.Append(moveNode.getMove());
				text.Append(" ");
				if (moveNode.Annotations != null && moveNode.Annotations.Count != 0)
				{
					text.Append("(");
					text.Append(moveNode.Annotations[0]);
					text.Append(") ");
				}
				ply++;
				current = moveNode.getPosition();
			}
			return text.ToString();
		}
	}
	*/
}