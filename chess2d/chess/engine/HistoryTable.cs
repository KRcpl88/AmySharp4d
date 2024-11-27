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
* $Id: HistoryTable.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using IntVector = tgreiner.amy.common.engine.IntVector;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.log4net;
namespace tgreiner.amy.chess.engine
{
	/// <summary> Implementation of history moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	//UPGRADE_NOTE: The access modifier for this class or class field has been changed in order to prevent compilation errors due to the visibility level. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1296'"
	public class HistoryTable
	{
		
		/// <summary>The log4j Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.HistoryTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;
		
		/// <summary>Mask the move from/to part of a move. </summary>
		public const int MASK = 4095;
		
		/// <summary>The history table. </summary>
		private int[] table = new int[4096];
		
		/// <summary> Add a move to the history table.
		/// 
		/// </summary>
		/// <param name="move">the move.
		/// </param>
		/// <param name="depth">the search depth.
		/// </param>
		public virtual void  addHistory(int move, int depth)
		{
			if ((move & (Move.CAPTURE | Move.ENPASSANT)) == 0)
			{
				int index = (move & MASK);
				int value_Renamed = table[index];
				int newValue = value_Renamed + (depth * depth);
				if (newValue < value_Renamed)
				{
					log.Error("Wrap around in addHistory()");
				}
				table[index] = newValue;
			}
		}
		
		/// <summary> Reset the history table.</summary>
		public virtual void  reset()
		{
			for (int i = table.Length - 1; i >= 0; i--)
			{
				table[i] = 0;
			}
		}
		
		/// <summary> Select the move with the best history entry from a move list.
		/// 
		/// </summary>
		/// <param name="moves">the moves vector
		/// </param>
		/// <param name="size">the size
		/// </param>
		/// <returns> the move
		/// </returns>
		public virtual int select(IntVector moves, int size)
		{
			int bestidx = 0;
			int move = moves.get_Renamed(bestidx);
			int best = table[move & MASK];
			
			for (int i = 1; i < size; i++)
			{
				move = moves.get_Renamed(i);
				int tmp = table[move & MASK];
				if (tmp > best)
				{
					best = tmp;
					bestidx = i;
				}
			}
			
			move = moves.get_Renamed(bestidx);
			if (bestidx != (size - 1))
			{
				moves.swap(bestidx, size - 1);
			}
			
			return move;
		}
		static HistoryTable()
		{
			log = LogManager.GetLogger();
		}
	}
}