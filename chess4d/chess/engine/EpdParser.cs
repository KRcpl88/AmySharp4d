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
* $Id: EpdParser.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Text;
using tgreiner.amy.bitboard;

namespace tgreiner.amy.chess.engine
{
	
	/// <summary> A parser for positions in EPD (Extended Position Description)
	/// format.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class EpdParser
	{
		
		/// <summary> Parse a position in EPD format.
		/// 
		/// </summary>
		/// <param name="epd">the EPD string
		/// </param>
		/// <returns> the position
		/// </returns>
		/// <throws>  IllegalEpdException if the EPD is not valid </throws>
        public virtual IPosition parse(System.String epd)
        {
            //UPGRADE_NOTE: Final was removed from the declaration of 'board '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            int[] board = new int[BitBoard.SIZE];

            string[] fenParts = epd.Split(new char[] { ' ' });
            if (fenParts.Length < 4)
            {
                throw new Exception("Invalid EPD");
            }

            int level = BitBoard.NUM_LEVELS -1;
            int rank = BitBoard.LEVEL_WIDTH[level]-1;
            int file = 0;

            foreach (char ch in fenParts[0])
            {
                if (file >= BitBoard.LEVEL_WIDTH[level])
                {
                    throw new IllegalEpdException(
                        $"EPD file {file} out of bounds on  Level: {level} Rank: {rank}");
                }
                if (!LRF.IsValid(level, rank, file))
                {
                    throw new IllegalEpdException(
                        $"EPD contains invalid posiiton on Level: {level} Rank: {rank} File: {file}");
                }
                int square = BitBoard.BitOffset(level, rank, file);
                switch (ch)
                {

                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                        file += Int32.Parse(ch.ToString());
                        break;

                    case 'P':
                        board[square] = ChessConstants_Fields.PAWN;
                        file++;
                        break;

                    case 'N':
                        board[square] = ChessConstants_Fields.KNIGHT;
                        file++;
                        break;

                    case 'B':
                        board[square] = ChessConstants_Fields.BISHOP;
                        file++;
                        break;

                    case 'R':
                        board[square] = ChessConstants_Fields.ROOK;
                        file++;
                        break;

                    case 'Q':
                        board[square] = ChessConstants_Fields.QUEEN;
                        file++;
                        break;

                    case 'K':
                        board[square] = ChessConstants_Fields.KING;
                        file++;
                        break;

                    case 'p':
                        board[square] = -ChessConstants_Fields.PAWN;
                        file++;
                        break;

                    case 'n':
                        board[square] = -ChessConstants_Fields.KNIGHT;
                        file++;
                        break;

                    case 'b':
                        board[square] = -ChessConstants_Fields.BISHOP;
                        file++;
                        break;

                    case 'r':
                        board[square] = -ChessConstants_Fields.ROOK;
                        file++;
                        break;

                    case 'q':
                        board[square] = -ChessConstants_Fields.QUEEN;
                        file++;
                        break;

                    case 'k':
                        board[square] = -ChessConstants_Fields.KING;
                        file++;
                        break;

                    case '/':
                        rank--;
                        file = 0;
                        if (rank < 0)
                        {
                            --level;
                            if (level < 0)
                            {
                                throw new IllegalEpdException(
                                    "EPD contains too many ranks");
                            }
                            rank = BitBoard.LEVEL_WIDTH[level]-1;
                        }
                        break;

                    default:
                        throw new IllegalEpdException(
                                "EPD contains illegal character '" + ch + "'");
                }
            }          

            bool whiteToMove = (fenParts[1].ToUpper().Equals("W")) ? true : false;

            // Scan castling status
            bool wCastleK = false;
            bool wCastleQ = false;
            bool bCastleK = false;
            bool bCastleQ = false;

            if (!fenParts[2].Equals("-"))
            {
                if (fenParts[2].IndexOf('K') != -1)
                {
                    wCastleK = true;
                }
                if (fenParts[2].IndexOf('Q') != -1)
                {
                    wCastleQ = true;
                }
                if (fenParts[2].IndexOf('k') != -1)
                {
                    bCastleK = true;
                }
                if (fenParts[2].IndexOf('q') != -1)
                {
                    bCastleQ = true;
                }
            }

            int enPassant = 0;
            if (!fenParts[3].Equals("-"))
            {
                if (fenParts[3].Length < 3)
                {
                    throw new IllegalEpdException("Illegal en passant square");
                }
                int epLevel = (int)(fenParts[3][0] - 'a');
                int epFile = (int)(fenParts[3][1] - 'a');
                int epRank = (int)(fenParts[3][2] - '1');

                if (!LRF.IsValid(epLevel, epRank, epFile))
                {
                    throw new IllegalEpdException("Illegal en passant square");
                }
                if (epFile < 0 || epFile > (BitBoard.LEVEL_WIDTH[epLevel] -1) || (epRank != 2 && epRank != (BitBoard.LEVEL_WIDTH[epLevel] -3)))
                {
                    throw new IllegalEpdException("Illegal en passant square");
                }

                enPassant = BitBoard.BitOffset(epLevel, epRank, epFile);
            }

            return new BoardPosition(board, whiteToMove, enPassant, wCastleK, wCastleQ, bCastleK, bCastleQ, this);
        }

        // Genrates FEN except the the last part(no of moves)
        public static string GetFEN(IPosition pos)
        {
            StringBuilder sb = new StringBuilder(100);
            for (int iy = 7; iy >= 0; iy--)
            {
                int emptyPos = 0;
                for (int ix = 0; ix < 8; ix++)
                {
                    int index = ix + iy * 8;
                    if (pos.Board[index] != 0)
                    {
                        if (emptyPos != 0)
                        {
                            sb.Append(emptyPos.ToString());
                            emptyPos = 0;
                        }

                        char ch = ChessBoard.PIECE_NAME[Math.Abs(pos.Board[index])];
                        if (pos.Board[index] < 0)
                        {
                            ch = Char.ToLower(ch);
                        }
                        sb.Append(ch);
                    }
                    else
                    {
                        emptyPos++;
                    }
                }

                if (emptyPos != 0)
                {
                    sb.Append(emptyPos.ToString());
                    emptyPos = 0;
                }

                sb.Append('/');
            }

            sb.Remove(sb.Length - 1, 1);

            if (pos.Wtm)
                sb.Append(" w ");
            else
                sb.Append(" b ");

            bool canCastle = false;

            if (pos.CanWhiteCastleKingSide())
            {
                sb.Append("K");
                canCastle = true;
            }
            if (pos.CanBlackCastleQueenSide())
            {
                sb.Append("Q");
                canCastle = true;
            }
            if (pos.CanBlackCastleKingSide())
            {
                sb.Append("k");
                canCastle = true;
            }
            if (pos.CanBlackCastleQueenSide())
            {
                sb.Append("q");
                canCastle = true;
            }
            if (!canCastle)
            {
                sb.Append("-");
            }

            if (pos.EnPassantSquare == 0)
            {
                sb.Append(" -");
            }
            else
            {
                sb.Append(" ");
                sb.Append(Move.file(pos.EnPassantSquare));
                sb.Append(Move.rank(pos.EnPassantSquare));
            }

            // sb.Append(" " + this.fifty.ToString());

            return sb.ToString();
        }
	}
}