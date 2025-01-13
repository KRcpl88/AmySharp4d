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
* $Id: PVSaver.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Collections.Generic;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Helper class to save principal variations.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class PVSaver
	{
		/// <summary> Get the ponder move.
		/// 
		/// </summary>
		/// <returns> the ponder move
		/// </returns>
		virtual public int PonderMove
		{
			get
			{
				int[] pv = (int[]) pvs[0];
				
				if (pv.Length > 1)
				{
					return pv[1];
				}
				else
				{
					return 0;
				}
			}
			
		}
		
		/// <summary>List of PVs. </summary>
        private IList<int[]> pvs = new List<int[]>();
		
		/// <summary> Create a PVSaver.</summary>
		public PVSaver()
		{
		}
		
		/// <summary> Ensure that at least <code>size</code> entries in the PV table are
		/// available.
		/// 
		/// </summary>
		/// <param name="size">the size
		/// </param>
		private void  ensureSize(int size)
		{
			while (pvs.Count < size)
			{
				pvs.Add(new int[0]);
			}
		}
		
		/// <summary> 
		/// Record a terminal position.
		/// 
		/// </summary>
		/// <param name="ply">the ply.
		/// </param>
		public virtual void  terminal(int ply)
		{
			ensureSize(ply + 1);
			
			int[] pv = (int[]) pvs[ply];
			
			if (pv.Length != ply)
			{
				pv = new int[ply];
				pvs[ply] = pv;
			}
		}
		
		/// <summary> Record a move.
		/// 
		/// </summary>
		/// <param name="ply">the ply.
		/// </param>
		/// <param name="move">the move.
		/// </param>
		public virtual void  move(int ply, int move)
		{
			ensureSize(ply + 2);
			
			int[] previousPV = (int[]) pvs[ply + 1];
			int[] pv = (int[]) pvs[ply];
			
			int len = System.Math.Max(previousPV.Length, ply + 1);
			
			if (pv.Length != len)
			{
				pv = new int[len];
				pvs[ply] = pv;
			}
			
			pv[ply] = move;
			
			if (previousPV.Length > (ply + 1))
			{
				Array.Copy(previousPV, ply + 1, pv, ply + 1, previousPV.Length - ply - 1);
			}
		}
		
		/// <summary> Get the principal variation.
		/// 
		/// </summary>
		/// <returns> the principal variation
		/// </returns>
		internal virtual int[] getPV()
		{
			return (int[]) pvs[0];
		}
		
		/// <summary> Get the saved PV as String.
		/// 
		/// </summary>
		/// <param name="board">the board.
		/// </param>
		/// <returns> the saved principal variation
		/// </returns>
		public virtual System.String getPV(ChessBoard board)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			int i;
			int[] pv = (int[]) pvs[0];
			for (i = 0; i < pv.Length; i++)
			{
				int move = pv[i];
				if (!board.isLegalMove(move))
				{
					break;
				}
				result.Append(Move.toSAN(board, move));
				result.Append(" ");
				board.doMove(move);
			}
			for (int j = i - 1; j >= 0; j--)
			{
				board.undoMove();
			}
			return result.ToString();
		}
	}
}