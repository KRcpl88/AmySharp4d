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
* $Id: Move.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Collections.Generic;
using tgreiner.amy.common.engine;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> This class contains several convenience methods to deal with moves.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public sealed class Move //: ChessConstants
	{
		
		/// <summary>The shift constant for the 'to' part of a move. </summary>
		private const int SHIFT_TO = 6;
		
		/// <summary>Constant for a capture move. </summary>
		public const int CAPTURE = (1 << 13);
		
		/// <summary>Constant for a king side castling move. </summary>
		public const int CASTLE_KSIDE = (1 << 14);
		
		/// <summary>Constant for a queen side castling move. </summary>
		public const int CASTLE_QSIDE = (1 << 15);
		
		/// <summary>Constant for a castling move. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'CASTLE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int CASTLE = CASTLE_KSIDE | CASTLE_QSIDE;
		
		/// <summary>Constant for a promotion to knight. </summary>
		public const int PROMO_KNIGHT = (1 << 16);
		
		/// <summary>Constant for a promotion to bishop. </summary>
		public const int PROMO_BISHOP = (1 << 17);
		
		/// <summary>Constant for a promotion to rook. </summary>
		public const int PROMO_ROOK = (1 << 18);
		
		/// <summary>Constant for a promotion to queen. </summary>
		public const int PROMO_QUEEN = (1 << 19);
		
		/// <summary>Constant for a promotion. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'PROMOTION '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly int PROMOTION = PROMO_KNIGHT | PROMO_BISHOP | PROMO_ROOK | PROMO_QUEEN;
		
		/// <summary>Constant for a double pawn move. </summary>
		public const int PAWN_DOUBLE = (1 << 20);
		
		/// <summary>Constant for an en passant capture. </summary>
		public const int ENPASSANT = (1 << 21);
		
		/// <summary>SAN for king side castling move. </summary>
		public const System.String KSIDE_CASTLE_SAN = "O-O";
		
		/// <summary>SAN for queen side castling move. </summary>
		public const System.String QSIDE_CASTLE_SAN = "O-O-O";
		
		/// <summary> This class cannot be instantiated.</summary>
		private Move()
		{
		}
		
		/// <summary> Get the <i>from</i> square of a move.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <returns> the from square
		/// </returns>
		public static int getFrom(int move)
		{
			return move & 63;
		}
		
		/// <summary> Get the <i>to</i> square of a move.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <returns> the to square
		/// </returns>
		public static int getTo(int move)
		{
			return (move >> SHIFT_TO) & 63;
		}
		
		/// <summary> Given 'from' and 'to' square, create a move.
		/// 
		/// </summary>
		/// <param name="from">the 'from' square
		/// </param>
		/// <param name="to">the 'to' square
		/// </param>
		/// <returns> the a move
		/// </returns>
		public static int makeMove(int from, int to)
		{
			return from | (to << SHIFT_TO);
		}
		
		/// <summary> Check if a move is a promotion move.
		/// 
		/// </summary>
		/// <param name="move">the move.
		/// </param>
		/// <returns> <code>true</code> if <code>move</code> is a promotion move
		/// </returns>
		public static bool isPromotion(int move)
		{
			return (move & PROMOTION) != 0;
		}
		
		/// <summary> Check if a move is a castle move.
		/// 
		/// </summary>
		/// <param name="move">the move.
		/// </param>
		/// <returns> <code>true</code> if <code>move</code> is a castle move
		/// </returns>
		public static bool isCastle(int move)
		{
			return (move & CASTLE) != 0;
		}
		
		/// <summary> Check if a move is a king side castle move.
		/// 
		/// </summary>
		/// <param name="move">the move.
		/// </param>
		/// <returns> <code>true</code> if <code>move</code> is a king side castle
		/// move
		/// </returns>
		public static bool isKingSideCastle(int move)
		{
			return (move & Move.CASTLE_KSIDE) != 0;
		}
		
		/// <summary> Given a promotion move, return the promotion piece.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <returns> the promotion piece.
		/// </returns>
		public static int getPromoPiece(int move)
		{
			switch (move & PROMOTION)
			{
				
				case PROMO_KNIGHT: 
					return ChessConstants_Fields.KNIGHT;
				
				case PROMO_BISHOP: 
					return ChessConstants_Fields.BISHOP;
				
				case PROMO_ROOK: 
					return ChessConstants_Fields.ROOK;
				
				case PROMO_QUEEN: 
					return ChessConstants_Fields.QUEEN;
				
				default: 
					throw new System.SystemException();
				
			}
		}
		
		/// <summary> Create a String representation of a move.
		/// 
		/// </summary>
		/// <param name="move">the move
		/// </param>
		/// <returns> a String representation of <code>move</code>
		/// </returns>
		public static System.String toString(int move)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			int from = getFrom(move);
			int to = getTo(move);
			
			result.Append(file(from));
			result.Append(rank(from));
			
			/*
             * if ((move & CAPTURE) != 0)
			{
				result.Append('x');
			}
			else
			{
				result.Append("-");
			}
             * */
			
			result.Append(file(to));
			result.Append(rank(to));
			
			if (isPromotion(move))
			{
				result.Append(ChessBoard.PIECE_NAME[getPromoPiece(move)]);
			}
			
			return result.ToString();
		}
		
		/// <summary> Generate the SAN (Standard Algebraic Notation) of a move.
		/// 
		/// </summary>
		/// <param name="board">the board representing the position the move
		/// was made in.
		/// </param>
		/// <param name="move">the move.
		/// </param>
		/// <returns> the SAN representation of <code>move</code>.
		/// </returns>
        public static System.String toSAN(ChessBoard board, int move)
        {
            return Move.toString(move);
        }

        public static System.String toSAN1(ChessBoard board, int move)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			int from = getFrom(move);
			int to = getTo(move);
			int type = board.getPieceAt(from);
			
			if (type == ChessConstants_Fields.PAWN)
			{
				if ((move & (CAPTURE | ENPASSANT)) != 0)
				{
					result.Append(file(from));
					result.Append('x');
				}
				
				result.Append(file(to));
				result.Append(rank(to));
				
				if ((move & PROMOTION) != 0)
				{
					result.Append('=');
					result.Append(ChessBoard.PIECE_NAME[getPromoPiece(move)]);
				}
			}
			else if ((move & CASTLE_KSIDE) != 0)
			{
				result.Append(KSIDE_CASTLE_SAN);
			}
			else if ((move & CASTLE_QSIDE) != 0)
			{
				result.Append(QSIDE_CASTLE_SAN);
			}
			else
			{
				result.Append(ChessBoard.PIECE_NAME[type]);
				
				IntVector legal = new IntVector();
				board.generateLegalMoves(legal);
				
				bool ambiguous = false;
				bool rankAmbiguous = false;
				bool fileAmbiguous = false;
				
				for (int i = 0; i < legal.size(); i++)
				{
					int move2 = legal.get_Renamed(i);
					if (move2 == move)
					{
						continue;
					}
					int from2 = getFrom(move2);
					int to2 = getTo(move2);
					
					if (to == to2 && board.getPieceAt(from2) == type)
					{
						ambiguous = true;
						if ((from2 & 7) == (from & 7))
						{
							fileAmbiguous = true;
						}
						if ((from2 >> 3) == (from >> 3))
						{
							rankAmbiguous = true;
						}
					}
				}
				
				if (ambiguous)
				{
					if (!fileAmbiguous)
					{
						result.Append(file(from));
					}
					else
					{
						if (!rankAmbiguous)
						{
							result.Append(rank(from));
						}
						else
						{
							result.Append(file(from));
							result.Append(rank(from));
						}
					}
				}
				if ((move & CAPTURE) != 0)
				{
					result.Append('x');
				}
				result.Append(file(to));
				result.Append(rank(to));
			}
			
			board.doMove(move);
			bool inCheck = board.InCheck;
			if (inCheck)
			{
				if (board.CheckMate)
				{
					result.Append('#');
				}
				else
				{
					result.Append('+');
				}
			}
			board.undoMove();
			
			return result.ToString();
		}
		
		/// <summary> Parse a move in SAN (Standard Algebraic Notation).
		/// 
		/// </summary>
		/// <param name="board">the chess board
		/// </param>
		/// <param name="san">the SAN to parse
		/// </param>
		/// <returns> the move
		/// </returns>
		/// <throws>  IllegalSANException if the SAN is not legal </throws>
		public static int parseSAN(ChessBoard board, System.String san)
		{
			int toRank = - 1, toFile = - 1;
			int fromRank = - 1, fromFile = - 1;
			int fromLevel = -1, toLevel = -1;
			int type = 0;
			int promotion = 0;
			int castle = 0;
			
			SupportClass.StringIterator iter = new SupportClass.StringIterator(san);
			
			for (int c = iter.First; c != '\uFFFF'; c = iter.Next())
			{
				
				switch (c)
				{
					
					case 'a': 
					case 'b': 
					case 'c': 
					case 'd': 
					case 'e': 
					case 'f': 
					case 'g': 
					case 'h': 
						if (toLevel == -1)
						{
							toLevel = c - 'a';							
						}
						else if ((toFile >= 0) && (fromLevel == -1))
						{
							fromLevel = toLevel;
							toLevel = c - 'a';
						}
						else
						{
							fromFile = toFile;
							toFile = c - 'a';
						}
						break;

					case 'i': 
					case 'j': 
					case 'k': 
					case 'l': 
					case 'm': 
					case 'n': 
					case 'o': 
						if (fromLevel == -1)
						{
							fromLevel = c - 'a';							
						}
						else if (toLevel == -1)
						{
							toLevel = c - 'a';
						}
						break;


					case '1': 
					case '2': 
					case '3': 
					case '4': 
					case '5': 
					case '6': 
					case '7': 
					case '8': 
						fromRank = toRank;
						toRank = c - '1';
						break;
					
					case 'N': 
						type = ChessConstants_Fields.KNIGHT;
						break;
					
					case 'B': 
						type = ChessConstants_Fields.BISHOP;
						break;
					
					case 'R': 
						type = ChessConstants_Fields.ROOK;
						break;
					
					case 'Q': 
						type = ChessConstants_Fields.QUEEN;
						break;
					
					case 'K': 
						type = ChessConstants_Fields.KING;
						break;
					
					case '=': 
						c = iter.Next();
						switch (c)
						{
							
							case 'Q': 
								promotion = PROMO_QUEEN;
								break;
							
							case 'R': 
								promotion = PROMO_ROOK;
								break;
							
							case 'B': 
								promotion = PROMO_BISHOP;
								break;
							
							case 'N': 
								promotion = PROMO_KNIGHT;
								break;
							
							default: 
								throw new IllegalSANException("Illegal promotion");
							
						}
						break;
					
					case 'O': 
					case '0': 
						castle++;
						break;
					
					
					case 'x': 
					case '+': 
					case '#': 
					case '-': 
						
						// We simply ignore these
						
						break;
					
					
					default: 
						throw new IllegalSANException("Illegal character " + c);
					
				}
			}
			
			int move = 0;
			
			if (castle != 0)
			{
				if (castle < 2 || castle > 3)
				{
					throw new IllegalSANException("Illegal castle");
				}
				type = ChessConstants_Fields.KING;
			}
			if (type == 0)
			{
				type = ChessConstants_Fields.PAWN;
			}
			
			IntVector mvs = new IntVector();
			board.generatePseudoLegalMoves(mvs);
			
			for (int i = 0; i < mvs.size(); i++)
			{
				int m = mvs.get_Renamed(i);
				
                int from = getFrom(m);
                if (board.getPieceAt(from) != type) { continue; }
                if (fromRank != -1 && (from >> 3) != fromRank) { continue; }
                if (fromFile != -1 && (from & 7) != fromFile) { continue; }

                int to = getTo(m);
                if (toRank != -1 && (to >> 3) != toRank) { continue; }
                if (toFile != -1 && (to & 7) != toFile) { continue; }

                if (promotion != 0 && (m & PROMOTION) != promotion) { continue; }

                if (castle == 2 && ((m & CASTLE_KSIDE) == 0)) { continue; }
                if (castle == 3 && ((m & CASTLE_QSIDE) == 0)) { continue; }

                if ((m & CASTLE) != 0 && !board.isCastleLegal(m)) { continue; }
				
				board.doMove(m);
				bool inCheck = board.OppInCheck;
				board.undoMove();
				
				if (inCheck)
				{
					continue;
				}
				
				if (move == 0)
				{
					move = m;
				}
				else
				{
					throw new IllegalSANException("Ambiguous move");
				}
			}
			
			if (move == 0)
			{
				throw new IllegalSANException("No matching move.");
			}
			
			return move;
		}

        public static int GetMove(ChessBoard board, string move)
        {
            IList<string> moves = new List<string>();

            int from = 8 * (move[1] - '1') + (move[0] - 'a');
            int to = 8 * (move[3] - '1') + (move[2] - 'a');

            IntVector movesList = new IntVector();
            board.generateLegalMoves(movesList);

            for (int i = 0; i < movesList.size(); i++)
            {
                int m = movesList.get_Renamed(i);

                if (from == Move.getFrom(m) && to == Move.getTo(m))
                {
                    if (move.Length == 4)
                    {
                        return m;
                    }
                    else
                    {
                        int promotion = 0;
                        switch (move[4])
                        {

                            case 'Q':
                                promotion = PROMO_QUEEN;
                                break;

                            case 'R':
                                promotion = PROMO_ROOK;
                                break;

                            case 'B':
                                promotion = PROMO_BISHOP;
                                break;

                            case 'N':
                                promotion = PROMO_KNIGHT;
                                break;

                            default:
                                throw new IllegalSANException("Illegal promotion");

                        }

                        if (promotion != 0 && (m & PROMOTION) == promotion)
                        {
                            return m;
                        }
                    }
                }
            }

            throw new Exception("Illegal Move");
        }
		
		/// <summary> Given a square, return its file as character.
		/// 
		/// </summary>
		/// <param name="square">the square
		/// </param>
		/// <returns> the file ('a'...'h')
		/// </returns>
		public static char file(int square)
		{
			return (char) ('a' + (square & 7));
		}
		
		/// <summary> Given a square, return its rank as character.
		/// 
		/// </summary>
		/// <param name="square">the square
		/// </param>
		/// <returns> the file ('a'...'h')
		/// </returns>
		public static char rank(int square)
		{
			return (char) ('1' + (square >> 3));
		}
	}
}