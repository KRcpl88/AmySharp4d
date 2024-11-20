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
* $Id: CheckingMoveGenerator.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using IMoveList = tgreiner.amy.common.engine.IMoveList;
namespace tgreiner.amy.chess.engine
{
	
	
	/// <summary> Generates checking moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	class CheckingMoveGenerator
	{
		
		/// <summary>The board. </summary>
		private ChessBoard board;
		
		/// <summary> Create a checking move generator.
		/// 
		/// </summary>
		/// <param name="theBoard">the chess board
		/// </param>
		internal CheckingMoveGenerator(ChessBoard theBoard)
		{
			this.board = theBoard;
		}
		
		/// <summary> Generate checking moves for bishops, rooks and queens.
		/// 
		/// </summary>
		/// <param name="moves">the moves.
		/// </param>
		internal virtual void  generateBRQChecks(IMoveList moves)
		{
			long allPieces = board.getMask(true) | board.getMask(false);
			int oppKing = board.getKingPos(!board.Wtm);
			long toSquaresB = Geometry.BISHOP_EPM[oppKing] & ~ allPieces;
			long toSquaresR = Geometry.ROOK_EPM[oppKing] & ~ allPieces;
			
			generateChecks(moves, board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP) | board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN), allPieces, toSquaresB, oppKing, false);
			
			generateChecks(moves, board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK) | board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN), allPieces, toSquaresR, oppKing, false);
		}
		
		/// <summary> Generate checking moves for knights.
		/// 
		/// </summary>
		/// <param name="moves">the moves.
		/// </param>
		internal virtual void  generateNChecks(IMoveList moves)
		{
			long allPieces = board.getMask(true) | board.getMask(false);
			int oppKing = board.getKingPos(!board.Wtm);
			long toSquares = Geometry.KNIGHT_EPM[oppKing] & ~ allPieces;
			
			generateChecks(moves, board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT), allPieces, toSquares, oppKing, true);
		}
		
		/// <summary> Generate checking moves.
		/// 
		/// </summary>
		/// <param name="moves">the moves
		/// </param>
		/// <param name="pcs">the pieces to generate moves for
		/// </param>
		/// <param name="allPieces">Bitboard of all pieces
		/// </param>
		/// <param name="toSquares">valid to squares
		/// </param>
		/// <param name="oppKing">position of the opponents king
		/// </param>
		/// <param name="ignoreInterPath">indicates wether the path between the 'to' square
		/// and the king needs to be empty
		/// </param>
		private void  generateChecks(IMoveList moves, long pcs, long allPieces, long toSquares, int oppKing, bool ignoreInterPath)
		{
			
			long tmp = pcs;
			
			while (tmp != 0L)
			{
				int from = BitBoard.findFirstOne(tmp);
				tmp &= BitBoard.CLEAR_MASK[from];
				
				long targets = board.getAttackTo(from) & toSquares;
				
				while (targets != 0L)
				{
					int to = BitBoard.findFirstOne(targets);
					targets &= BitBoard.CLEAR_MASK[to];
					
					if (ignoreInterPath || (allPieces & Geometry.INTER_PATH[to][oppKing]) == 0L)
					{
						moves.add(Move.makeMove(from, to));
					}
				}
			}
		}
	}
}