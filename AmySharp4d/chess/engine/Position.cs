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
* $Id: Position.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;

namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Encapsulates a chess position.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public struct Position_Fields{
		/// <summary> The initial position.</summary>
		public readonly static IPosition INITIAL_POSITION;
		static Position_Fields()
		{
            INITIAL_POSITION = new InitialPosition();
		}
	}
	
    public class InitialPosition : IPosition
	{
		/// <seealso cref="IPosition.getBoard">
		/// </seealso>
		virtual public int[] Board
		{
			get
			{
				
				
				return new int[]
				{
					0,
					0, 0,
					0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN,
					ChessConstants_Fields.WHITE_ROOK, ChessConstants_Fields.WHITE_KNIGHT, ChessConstants_Fields.WHITE_BISHOP, ChessConstants_Fields.WHITE_QUEEN, ChessConstants_Fields.WHITE_KING, ChessConstants_Fields.WHITE_BISHOP, ChessConstants_Fields.WHITE_KNIGHT, ChessConstants_Fields.WHITE_ROOK, 
					ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, 
					0, 0, 0, 0, 0, 0, 0, 0, 
					0, 0, 0, 0, 0, 0, 0, 0, 
					0, 0, 0, 0, 0, 0, 0, 0, 
					0, 0, 0, 0, 0, 0, 0, 0, 
					ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, 
					ChessConstants_Fields.BLACK_ROOK, ChessConstants_Fields.BLACK_KNIGHT, ChessConstants_Fields.BLACK_BISHOP, ChessConstants_Fields.BLACK_QUEEN, ChessConstants_Fields.BLACK_KING, ChessConstants_Fields.BLACK_BISHOP, ChessConstants_Fields.BLACK_KNIGHT, ChessConstants_Fields.BLACK_ROOK,
					ChessConstants_Fields.WHITE_ROOK, ChessConstants_Fields.WHITE_KNIGHT, ChessConstants_Fields.WHITE_BISHOP, ChessConstants_Fields.WHITE_QUEEN, ChessConstants_Fields.WHITE_KNIGHT, ChessConstants_Fields.WHITE_BISHOP, ChessConstants_Fields.WHITE_ROOK,
					ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0,
					ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN,
					ChessConstants_Fields.BLACK_ROOK, ChessConstants_Fields.BLACK_KNIGHT, ChessConstants_Fields.BLACK_BISHOP, ChessConstants_Fields.BLACK_QUEEN, ChessConstants_Fields.BLACK_KNIGHT, ChessConstants_Fields.BLACK_BISHOP, ChessConstants_Fields.BLACK_ROOK,
					ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN, ChessConstants_Fields.WHITE_PAWN,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0,
					ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN, ChessConstants_Fields.BLACK_PAWN,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0, 0,
					0, 0,
					0, 0,
					0,
				};
			}

		}
		/// <seealso cref="IPosition.getBoard">
		/// </seealso>
		virtual public bool Wtm
		{
			get
			{
				return true;
			}
			
		}
		/// <seealso cref="IPosition.getEnPassantSquare">
		/// </seealso>
		virtual public int EnPassantSquare
		{
			get
			{
				return 0;
			}
			
		}
		
		/// <seealso cref="IPosition.CanWhiteCastleKingSide">
		/// </seealso>
		public virtual bool CanWhiteCastleKingSide()
		{
			return true;
		}
		
		/// <seealso cref="IPosition.CanWhiteCastleQueenSide">
		/// </seealso>
		public virtual bool CanWhiteCastleQueenSide()
		{
			return true;
		}
		
		/// <seealso cref="IPosition.CanBlackCastleKingSide">
		/// </seealso>
		public virtual bool CanBlackCastleKingSide()
		{
			return true;
		}
		
		/// <seealso cref="IPosition.CanBlackCastleQueenSide">
		/// </seealso>
		public virtual bool CanBlackCastleQueenSide()
		{
			return true;
		}
	}
	public interface IPosition
	{
		//UPGRADE_NOTE: Members of interface 'Position' were extracted into structure 'Position_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Get the board. This method must return an array of 64 integers
		/// encoding the pieces on the squares.
		/// 
		/// </summary>
		/// <returns> a 64 element array containing the pieces
		/// </returns>
		int[] Board
		{
			get;
			
		}
		/// <summary> Get the side to move.
		/// 
		/// </summary>
		/// <returns> <code>true</code> for white to move, <code>false</code> otherwise
		/// </returns>
		bool Wtm
		{
			get;
			
		}
		/// <summary> Get the en passant square.
		/// 
		/// </summary>
		/// <returns> the en passant square
		/// </returns>
		int EnPassantSquare
		{
			get;
			
		}
		
		/// <summary> Check if white can castle king side.
		/// 
		/// </summary>
		/// <returns> <code>true</code>  if white can castle king side
		/// </returns>
		bool CanWhiteCastleKingSide();
		
		/// <summary> Check if white can castle queen side.
		/// 
		/// </summary>
		/// <returns> <code>true</code>  if white can castle queen side
		/// </returns>
		bool CanWhiteCastleQueenSide();
		
		/// <summary> Check if black can castle king side.
		/// 
		/// </summary>
		/// <returns> <code>true</code>  if black can castle king side
		/// </returns>
		bool CanBlackCastleKingSide();
		
		/// <summary> Check if black can castle queen side.
		/// 
		/// </summary>
		/// <returns> <code>true</code>  if black can castle queen side
		/// </returns>
		bool CanBlackCastleQueenSide();
	}
}