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
* $Id: Swapper.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> A static exchange evaluator.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class Swapper : ChessConstants
	{
		
		/// <summary>The swaplist. </summary>
		private int[] swaplist = new int[32];
		
		/// <summary>The piece values. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'PIECE_VALUES'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] PIECE_VALUES = new int[]{0, 1, 3, 3, 5, 9, 100};
		
		/// <summary> Create a swapper.</summary>
		public Swapper()
		{
		}
		
		/// <summary> Recalculate attacks after a swap has been made.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="atks">the attacks
		/// </param>
		/// <param name="from">the from square
		/// </param>
		/// <param name="to">the to square
		/// </param>
		/// <returns> the attacks
		/// </returns>
		private long swapReRay(ChessBoard board, long atks, int from, int to)
		{
			long result = atks & BitBoard.CLEAR_MASK[from];
			
			result |= (Geometry.RAY[to][from] & board.SlidingPieces & board.getAtkFr(from));
			
			return result;
		}
		
		/// <summary> Calculate the 'swap off' value of <code>move</code> using a static
		/// exchange evaluator.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <param name="move">the move
		/// </param>
		/// <returns> the 'swap off' value of <code>move</code>
		/// </returns>
		public virtual int swap(ChessBoard board, int move)
		{
			int from = Move.getFrom(move);
			int to = Move.getTo(move);
			int swapval;
			
			swapval = PIECE_VALUES[board.getPieceAt(from)];
			swaplist[0] = PIECE_VALUES[board.getPieceAt(to)];
			
			long atks = board.getAtkFr(to);
			
			int swapcnt = 0;
			bool swapwtm = !(board.isWhiteAt(from));
			int swapsign = - 1;
			
			atks = swapReRay(board, atks, from, to);
			
			while (atks != 0L)
			{
				long tmp;
				int sq;
				
				tmp = atks & board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
				if (tmp != 0L)
				{
					sq = BitBoard.findFirstOne(tmp);
				}
				else
				{
					tmp = atks & (board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP));
					if (tmp != 0L)
					{
						sq = BitBoard.findFirstOne(tmp);
					}
					else
					{
						tmp = atks & board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK);
						if (tmp != 0L)
						{
							sq = BitBoard.findFirstOne(tmp);
						}
						else
						{
							tmp = atks & board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN);
							if (tmp != 0L)
							{
								sq = BitBoard.findFirstOne(tmp);
							}
							else
							{
								tmp = atks & board.getMask(swapwtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KING);
								if (tmp != 0L)
								{
									sq = BitBoard.findFirstOne(tmp);
								}
								else
								{
									break;
								}
							}
						}
					}
				}
				
				swapcnt++;
				swaplist[swapcnt] = swaplist[swapcnt - 1] + swapsign * swapval;
				swapval = PIECE_VALUES[board.getPieceAt(sq)];
				swapsign = - swapsign;
				swapwtm = !swapwtm;
				
				atks = swapReRay(board, atks, sq, to);
			}
			
			if ((swapcnt & 1) != 0)
			{
				swapsign = - 1;
			}
			else
			{
				swapsign = 1;
			}
			
			while (swapcnt != 0)
			{
				if (swapsign < 0)
				{
					if (swaplist[swapcnt] <= swaplist[swapcnt - 1])
					{
						swaplist[swapcnt - 1] = swaplist[swapcnt];
					}
				}
				else
				{
					if (swaplist[swapcnt] >= swaplist[swapcnt - 1])
					{
						swaplist[swapcnt - 1] = swaplist[swapcnt];
					}
				}
				swapcnt--;
				swapsign = - swapsign;
			}
			
			return swaplist[0];
		}
	}
}