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
* $Id: MVVLVAGenerator.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
//using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
using Generator = tgreiner.amy.common.engine.Generator;
namespace tgreiner.amy.chess.engine
{
	
	
	/// <summary> A move generator that generates capturing moves in MVV/LVA
	/// (most valuable victim/least valuable attacker) order.
	/// </summary>
	public class MVVLVAGenerator : Generator
	{
		/// <summary>The board. </summary>
		protected internal ChessBoard board;
		
		/// <summary>The current victim (PAWN..QUEEN). </summary>
		private int victim;
		
		/// <summary>The current attacker (PAWN..KING). </summary>
		private int attacker;
		
		/// <summary>The victim square. </summary>
		private int victimSq;
		
		/// <summary>Bitboard of all victims. </summary>
		private BitBoard victims = new BitBoard();
		
		/// <summary>Bitboard of all attackers. </summary>
		private BitBoard attackers = new BitBoard();
		
		/// <summary>Indicates the side to move. </summary>
		private bool whiteToMove;
		
		/// <summary> Create a MVVLVAGenerator.
		/// 
		/// </summary>
		/// <param name="theBoard">the board
		/// </param>
		public MVVLVAGenerator(ChessBoard theBoard)
		{
			this.board = theBoard;
			reset();
		}
		
		/// <seealso cref="Generator.nextMove">
		/// </seealso>
		public virtual int nextMove()
		{
			while (victimSq != - 1)
			{
				int attSq = nextAttacker();
				if (attSq == - 1)
				{
					nextVictim();
					attacker = 0;
					attackers.Clear();
					continue;
				}
				if (board.getPieceAt(attSq) == ChessConstants_Fields.PAWN)
				{
					if (board.WhiteToMove && victimSq >= BoardConstants_Fields.HA8)
					{
						return attSq | (victimSq << 6) | Move.CAPTURE | Move.PROMO_QUEEN;
					}
					if (!board.WhiteToMove && victimSq <= BoardConstants_Fields.HH1)
					{
						return attSq | (victimSq << 6) | Move.CAPTURE | Move.PROMO_QUEEN;
					}
				}
				return attSq | (victimSq << 6) | Move.CAPTURE;
			}
			return - 1;
		}
		
		/// <summary> Find the next victim.</summary>
		private void  nextVictim()
		{
			while (victim >= ChessConstants_Fields.PAWN)
			{
				if (victims.IsEmpty())
				{
					victim--;
					victims = board.getMask(!whiteToMove, victim);
					continue;
				}
				victimSq = victims.findFirstOne();
				victims.ClearBit(victimSq);
				return ;
			}
			victimSq = - 1;
		}
		
		/// <summary> Find the next attacker.
		/// 
		/// </summary>
		/// <returns> the next attacker (PAWN..KING)
		/// </returns>
		private int nextAttacker()
		{
			while (attacker <= ChessConstants_Fields.KING)
			{
				if (attackers.IsEmpty())
				{
					attacker++;
					if (attacker > ChessConstants_Fields.KING)
					{
						break;
					}
					attackers = board.getMask(whiteToMove, attacker);
					attackers &= board.getAttackFrom(victimSq);
					continue;
				}
				int attackSquare = attackers.findFirstOne();
				attackers.ClearBit(attackSquare);
				return attackSquare;
			}
			return - 1;
		}
		
		/// <seealso cref="Generator.reset">
		/// </seealso>
		public virtual void  reset()
		{
			this.whiteToMove = board.WhiteToMove;
			
			victim = ChessConstants_Fields.QUEEN;
			attacker = 0;
			
			victims = board.getMask(!whiteToMove, victim);
			attackers.Clear();
			
			nextVictim();
		}
		
		/// <seealso cref="Generator.failHigh">
		/// </seealso>
		public virtual void  failHigh(int move, int depth)
		{
			// do nothing...
		}
	}
}