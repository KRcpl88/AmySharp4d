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
* $Id: MoveSelector.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using IntVector = tgreiner.amy.common.engine.IntVector;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> A utility class to select moves by from/to square.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public sealed class MoveSelector
	{
		/// <summary> This class cannot be instantiated.</summary>
		private MoveSelector()
		{
		}
		
		/// <summary> Select a move via from/to square.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="from">the from square
		/// </param>
		/// <param name="to">the to square
		/// </param>
		/// <returns> the selected move
		/// </returns>
		public static int selectMove(ChessBoard board, int from, int to)
		{
			IntVector moves = new IntVector();
			board.generateLegalMoves(moves);
			
			for (int i = 0; i < moves.size(); i++)
			{
				int move = moves.get_Renamed(i);
				
				if (Move.getTo(move) == to && Move.getFrom(move) == from)
				{
					return move;
				}
			}
			
			return - 1;
		}
	}
}