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
* $Id: CheckEvasionMoveGenerator.java 12 2009-12-08 08:45:51Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
//using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
using tgreiner.amy.common.engine;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Generates check evasions.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public class CheckEvasionMoveGenerator : Generator
	{
		
		/// <summary>The Logger. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to static method 'tgreiner.amy.chess.engine.CheckEvasionMoveGenerator'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static ILog log;

        /// <summary>Constant for phase 'hashmove'. </summary>
        private const int HASHMOVE = 0;
        /// <summary>Constant for phase 'generate gaining captures'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'GENERATE_CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int GENERATE_CAPTURES = HASHMOVE + 1;
        /// <summary>Constant for phase 'captures'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'CAPTURES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int CAPTURES = GENERATE_CAPTURES + 1;
        /// <summary>Constant for phase 'killer move 1'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'KILLER1 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int KILLER1 = CAPTURES + 1;
        /// <summary>Constant for phase 'killer move 2'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'KILLER2 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int KILLER2 = KILLER1 + 1;
        /// <summary>Constant for phase 'generate rest of the moves'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'GENERATE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int GENERATE = KILLER2 + 1;
        /// <summary>Constant for phase 'rest of the moves'. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'REST '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private const int REST = GENERATE + 1;
		
		/// <summary>The current phase. </summary>
		private int phase;
		
		/// <summary>The current index into the moves array. </summary>
		private int idx;
		
		/// <summary>The move from the hash table. </summary>
		private int hashMove;
		
		/// <summary>Killer move 1. </summary>
		private int killer1 = 0;
		
		/// <summary>Number of fail-highs of killer move 1. </summary>
		private int killer1cnt = 0;
		
		/// <summary>Killer move 2. </summary>
		private int killer2 = 0;
		
		/// <summary>Number of fail-highs of killer move 2. </summary>
		private int killer2cnt = 0;
		
		/// <summary>Keeps moves. </summary>
		private IntVector moves = new IntVector();
		
		/// <summary>Keeps captures. </summary>
		private IntVector captures = new IntVector();
		
		/// <summary>Number of captures. </summary>
		private int nCaptures;
		
		/// <summary>The transposition table. </summary>
		private ITransTable ttable;
		
		/// <summary>The history table. </summary>
		private HistoryTable history;
		
		/// <summary>The board. </summary>
		private ChessBoard board;
		
		/// <summary> Create a CheckEvasionMoveGenerator.
		/// 
		/// </summary>
		/// <param name="theBoard">the board
		/// </param>
		/// <param name="theTtable">the transposition table
		/// </param>
		/// <param name="theHistory">the history table.
		/// </param>
		public CheckEvasionMoveGenerator(ChessBoard theBoard, ITransTable theTtable, HistoryTable theHistory)
		{
			this.board = theBoard;
			this.ttable = theTtable;
			this.history = theHistory;
		}

        /// <seealso cref="Generator.nextMove">
        /// </seealso>
        public virtual int nextMove()
        {

            switch (phase)
            {

                case HASHMOVE:
                    phase = GENERATE_CAPTURES;
                    TTEntry entry = ttable.get_Renamed(board.PosHash);
                    if (entry != null && entry.move != 0)
                    {
                        if (board.isPseudoLegalMove(entry.move))
                        {
                            hashMove = entry.move;
                            return hashMove;
                        }
                        else
                        {
                            log.Error("Got illegal move from hash, " + "possible hash collision");
                        }
                    }
                    // Fallthrough
                    goto case GENERATE_CAPTURES;

                case GENERATE_CAPTURES:
                    captures.Size = 0;

                    int kingPos = board.getKingPos(board.Wtm);
                    BitBoard victims = board.getMask(!board.Wtm) & (board.getAttackFrom(kingPos) | board.getAttackTo(kingPos));

                    while (victims != 0L)
                    {
                        int sq = BitBoard.findFirstOne(victims);
                        victims &= BitBoard.CLEAR_MASK[sq];
                        board.generateTo(sq, captures);
                    }

                    // Add en passant captures
                    board.generateEnPassant(captures);
                    nCaptures = captures.size();

                    // Fallthrough
                    goto case CAPTURES;

                case CAPTURES:
                    {
                        while (nCaptures > 0)
                        {
                            nCaptures--;
                            int move = captures.get_Renamed(nCaptures);

                            if (move != hashMove)
                            {
                                phase = CAPTURES;
                                return move;
                            }
                        }
                    }
                    // Fallthrough
                    goto case KILLER1;

                case KILLER1:
                    if (killer1 != hashMove && board.isPseudoLegalMove(killer1))
                    {
                        phase = KILLER2;
                        return killer1;
                    }
                    // Fallthrough
                    goto case KILLER2;

                case KILLER2:
                    if (killer2 != hashMove && board.isPseudoLegalMove(killer2))
                    {
                        phase = GENERATE;
                        return killer2;
                    }
                    // Fallthrough
                    goto case GENERATE;

                case GENERATE:
                    moves.Size = 0;
                    generateEvasions(moves);
                    idx = moves.size();
                    phase = REST;
                    goto case REST;

                case REST:
                    while (idx > 0)
                    {
                        int move = history.select(moves, idx);
                        idx--;

                        if (move != hashMove && move != killer1 && move != killer2)
                        {
                            return move;
                        }
                    }
                    break;
            }

            return -1;
        }
		
		/// <summary> Generate check evasions.
		/// 
		/// </summary>
		/// <param name="theMoves">the moves
		/// </param>
		internal virtual void  generateEvasions(IMoveList theMoves)
		{
			int kingPos = board.getKingPos(board.Wtm);
			BitBoard oppPieces = board.getMask(!board.Wtm);
			BitBoard kingSqs = board.getAttackTo(kingPos) & ~ (board.getMask(true) | board.getMask(false));
			
			while (kingSqs != 0L)
			{
				int to = BitBoard.findFirstOne(kingSqs);
				kingSqs.ClearBit(to);
				
				if ((board.getAttackFrom(to) & oppPieces) == 0L)
				{
					theMoves.add(Move.makeMove(kingPos, to));
				}
			}

			BitBoard attackers = board.getAttackFrom(kingPos) & oppPieces;
			
			if (attackers.countBits() == 1)
			{
				BitBoard rayAttackers = attackers & (board.getMask(!board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP) | board.getMask(!board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK) | board.getMask(!board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN));
				if (rayAttackers != 0L)
				{
					int attackerPos = rayAttackers.findFirstOne();
					//UPGRADE_NOTE: Final was removed from the declaration of 'validToSquares '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
					BitBoard validToSquares = Geometry.INTER_PATH[attackerPos][kingPos];
					
					long defenders = board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP) | board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK) | board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN);
					
					long tmp = validToSquares;
					while (tmp != 0L)
					{
						int to = BitBoard.findFirstOne(tmp);
						tmp &= BitBoard.CLEAR_MASK[to];
						
						long tmp2 = board.getAttackFrom(to) & defenders;
						while (tmp2 != 0L)
						{
							int from = BitBoard.findFirstOne(tmp2);
							tmp2 &= BitBoard.CLEAR_MASK[from];
							theMoves.add(Move.makeMove(from, to));
						}
					}

					BitBoard pawns = board.getMask(board.Wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
					while (pawns != 0L)
					{
						int from = BitBoard.findFirstOne(pawns);
						pawns &= BitBoard.CLEAR_MASK[from];
						
						if (board.Wtm)
						{
							int to = from + 8;
							if (board.getPieceAt(to) != 0)
							{
								continue;
							}
							if ((validToSquares.SetBit(to)) != 0L)
							{
								if (to >= tgreiner.amy.bitboard.BoardConstants_Fields.HA8)
								{
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_QUEEN);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_ROOK);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_BISHOP);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_KNIGHT);
								}
								else
								{
									theMoves.add(Move.makeMove(from, to));
								}
							}
							if (from <= tgreiner.amy.bitboard.BoardConstants_Fields.HH2)
							{
								to = from + 16;
								if (validToSquares[to] != 0)
								{
									theMoves.add(Move.makeMove(from, to) | Move.PAWN_DOUBLE);
								}
							}
						}
						else
						{
							int to = from - 8;
							if (board.getPieceAt(to) != 0)
							{
								continue;
							}
							if (validToSquares[to] != 0)
							{
								if (to <= tgreiner.amy.bitboard.BoardConstants_Fields.HH1)
								{
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_QUEEN);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_ROOK);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_BISHOP);
									theMoves.add(Move.makeMove(from, to) | Move.PROMO_KNIGHT);
								}
								else
								{
									theMoves.add(Move.makeMove(from, to));
								}
							}
							if (from >= tgreiner.amy.bitboard.BoardConstants_Fields.HA7)
							{
								to = from - 16;
								if (validToSquares[to] != 0)
								{
									theMoves.add(Move.makeMove(from, to) | Move.PAWN_DOUBLE);
								}
							}
						}
					}
				}
			}
		}
		
		/// <seealso cref="Generator.reset">
		/// </seealso>
		public virtual void  reset()
		{
			phase = HASHMOVE;
			hashMove = 0;
		}
		
		/// <seealso cref="Generator.failHigh">
		/// </seealso>
		public virtual void  failHigh(int move, int depth)
		{
			if ((move & (Move.CAPTURE | Move.ENPASSANT)) == 0)
			{
				if (killer1 == 0)
				{
					killer1 = move;
					killer1cnt = 1;
				}
				else if (move == killer1)
				{
					killer1cnt++;
				}
				else if (move == killer2)
				{
					killer2cnt++;
					if (killer1cnt > 1)
					{
						killer1cnt--;
					}
					if (killer2cnt > killer1cnt)
					{
						int tmp = killer1;
						killer1 = killer2;
						killer2 = tmp;
						
						tmp = killer1cnt;
						killer1cnt = killer2cnt;
						killer2cnt = tmp;
					}
				}
				else
				{
					killer2 = move;
					killer2cnt = 1;
					if (killer1cnt > 1)
					{
						killer1cnt--;
					}
				}
			}
		}
		static CheckEvasionMoveGenerator()
		{
			log = LogManager.GetLogger(typeof(MoveGenerator2));
		}
	}
}