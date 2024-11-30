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
* $Id: ChessBoard.java 15 2010-03-23 09:03:40Z tetchu $
*/
using System;
using System.Collections.Generic;
using tgreiner.amy.chess.engine;
using tgreiner.amy.bitboard;
//using BoardConstants = tgreiner.amy.bitboard.BoardConstants;
using tgreiner.amy.common.engine;
using AmySharp.chess.engine.logger;
using System.Text;

namespace tgreiner.amy.chess.engine
{
    
    /// <summary> An implementation of a chess board using bitboards.
    /// 
    /// </summary>
    /// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
    /// </author>
    public class ChessBoard //: //ChessConstants //, BoardConstants
    {
        private void  InitBlock()
        {
            attackTo = BitBoard.CreateArray(BitBoard.SIZE);
            attackFrom = BitBoard.CreateArray(BitBoard.SIZE);
            for (int i = 0; i < 2; i++)
            {
                pieceMask[i] = BitBoard.CreateArray(ChessConstants_Fields.LAST_PIECE);
            }
            board = new int[BitBoard.SIZE];
        }


        /// <summary> Is white to move?
        /// 
        /// </summary>
        /// <returns> <code>true</code> if white is on the move.
        /// </returns>
        virtual public bool WhiteToMove
        {
            get
            {
                return whiteToMove;
            }
            
        }
        /// <summary> Check if the side to move is in check.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the side to move is in check.
        /// </returns>
        virtual public bool InCheck
        {
            get
            {
                if (whiteToMove)
                {
                    return (attackFrom[kingPos[0]] & pieceMask[1][0]).IsEmpty() == false;
                }
                else
                {
                    return (attackFrom[kingPos[1]] & pieceMask[0][0]).IsEmpty() == false;
                }
            }
            
        }
        /// <summary> Check if the side <b>not</b> to move is in check. This can
        /// only happen if the previous move was not legal by the
        /// rules of chess.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the side to move is in check.
        /// </returns>
        virtual public bool OppInCheck
        {
            get
            {
                if (!whiteToMove)
                {
                    return (attackFrom[kingPos[0]] & pieceMask[1][0]).IsEmpty() == false;
                }
                else
                {
                    return (attackFrom[kingPos[1]] & pieceMask[0][0]).IsEmpty() == false;
                }
            }
            
        }
        /// <summary> Get the bitboard of sliding pieces.
        /// 
        /// </summary>
        /// <returns> the bitboard of sliding pieces
        /// </returns>
        virtual internal BitBoard SlidingPieces
        {
            get
            {
                return slidingPieces;
            }
            
        }
        /// <summary> Get the non-pawn bitboard.
        /// 
        /// </summary>
        /// <returns> the bitboard of non-pawn pieces.
        /// </returns>
        virtual internal BitBoard MaskNonPawn
        {
            get
            {
                return pieceMask[0][ChessConstants_Fields.KNIGHT] | pieceMask[1][ChessConstants_Fields.KNIGHT] | pieceMask[0][ChessConstants_Fields.BISHOP] | pieceMask[1][ChessConstants_Fields.BISHOP] | pieceMask[0][ChessConstants_Fields.ROOK] | pieceMask[1][ChessConstants_Fields.ROOK] | pieceMask[0][ChessConstants_Fields.QUEEN] | pieceMask[1][ChessConstants_Fields.QUEEN];
            }
            
        }
        /// <summary> Get the enpassant square.
        /// 
        /// </summary>
        /// <returns> the enpassant square
        /// </returns>
        virtual public int EnPassant
        {
            get
            {
                return enPassant;
            }
            
        }
        /// <summary> Get the position hash code.
        /// 
        /// </summary>
        /// <returns> the position hash code.
        /// </returns>
        virtual public long PosHash
        {
            get
            {
                return positionHash;
            }
            
        }
        /// <summary> Get the pawn hash code.
        /// 
        /// </summary>
        /// <returns> the pawn hash code.
        /// </returns>
        virtual public long PawnHash
        {
            get
            {
                return pawnHash;
            }
            
        }
        /// <summary> Check if this is a draw by repetition.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the current position occurrs
        /// for the third time, <code>false</code> otherwise.
        /// </returns>
        virtual public bool DrawByRepetition
        {
            get
            {
                return checkDraw(2);
            }
            
        }
        /// <summary> Check if this position occurred at least once before.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the current position occured at least one
        /// time before.
        /// </returns>
        virtual protected internal bool Repeated
        {
            get
            {
                return checkDraw(1);
            }
            
        }
        /// <summary> Test if the current position is a checkmate.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the current position is a checkmate,
        /// <code>false</code> otherwise.
        /// </returns>
        virtual public bool CheckMate
        {
            get
            {
                if (InCheck)
                {
                    IntVector tmpMoves = new IntVector();
                    generateLegalMoves(tmpMoves);
                    return tmpMoves.size() == 0;
                }
                else
                {
                    return false;
                }
            }
            
        }
        /// <summary> Test if there is insufficient material present to checkmate.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if there is insufficient material to checkmate
        /// </returns>
        virtual public bool InsufficientMaterial
        {
            get
            {
                if ((materialSignature[0] & MAJOR_PIECES_OR_PAWNS) != 0 || (materialSignature[1] & MAJOR_PIECES_OR_PAWNS) != 0)
                {
                    return false;
                }
                
                BitBoard minors = pieceMask[0][ChessConstants_Fields.BISHOP] | pieceMask[1][ChessConstants_Fields.BISHOP] | pieceMask[0][ChessConstants_Fields.KNIGHT] | pieceMask[1][ChessConstants_Fields.KNIGHT];
                
                return minors.countBits() < 2;
            }
            
        }
        /// <summary> Check if the current position is a draw according to the fifty
        /// move rule.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if the current position is drawn according to
        /// the fify move rule.
        /// </returns>
        virtual public bool FiftyMoveRuleDraw
        {
            get
            {
                int bound = player - 100;
                if (bound < 0)
                {
                    return false;
                }
                for (int p = player - 1; p >= bound; p--)
                {
                    History h = (History) history[p];
                    if ((h.move & (Move.CAPTURE | Move.CASTLE | Move.PROMOTION)) != 0 || h.pawnHash != this.pawnHash)
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }

        /// <summary> Get the evaluator for this board.
        /// 
        /// </summary>
        /// <returns> the Evaluator.
        /// </returns>
        /// <summary> Set the evaluator for this board.
        /// 
        /// </summary>
        /// <param name="theEvaluator">the Evaluator.
        /// </param>
        virtual public IEvaluator Evaluator
        {
            get
            {
                return evaluator;
            }
            
            set
            {
                this.evaluator = value;
                evaluator.init();
            }
            
        }
        /// <summary> Get the last move made.
        /// 
        /// </summary>
        /// <returns> the last move made.
        /// </returns>
        virtual public int LastMove
        {
            get
            {
                History hist = (History) history[player - 1];
                return hist.move;
            }
            
        }
        /// <summary> Get the piece captured in the last move.
        /// 
        /// </summary>
        /// <returns> the piece captured in the last move
        /// </returns>
        virtual public int LastCaptured
        {
            get
            {
                History hist = (History) history[player - 1];
                return hist.capturedPiece;
            }
            
        }
        /// <summary> Set the current position.
        /// 
        /// </summary>
        /// <param name="pos">the position.
        /// </param>
        virtual public IPosition Position
        {
            get
            {
                return new BoardPosition(board, whiteToMove, enPassant, canWhiteCastleKingSide(), canWhiteCastleQueenSide(), canBlackCastleKingSide(), canBlackCastleQueenSide(), null);
            }

            set
            {
                Array.Copy(value.Board, 0, board, 0, board.Length);
                
                whiteToMove = value.Wtm;
                
                castle = 0;
                
                if (value.CanWhiteCastleKingSide())
                {
                    castle |= WHITE_CASTLE_KINGSIDE;
                }
                if (value.CanBlackCastleKingSide())
                {
                    castle |= BLACK_CASTLE_KINGSIDE;
                }
                if (value.CanWhiteCastleQueenSide())
                {
                    castle |= WHITE_CASTLE_QUEENSIDE;
                }
                if (value.CanBlackCastleQueenSide())
                {
                    castle |= BLACK_CASTLE_QUEENSIDE;
                }
                
                enPassant = value.EnPassantSquare;
                
                recalcAttacks();
            }
            
        }
        
        /// <summary>The Log. </summary>
        //UPGRADE_NOTE: The initialization of  'log' was moved to static method 'ChessBoard'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private static ILog log;
        
        /// <summary>Constant for castling right. </summary>
        private const int WHITE_CASTLE_KINGSIDE = 0x01;
        
        /// <summary>Constant for castling right. </summary>
        private const int WHITE_CASTLE_QUEENSIDE = 0x02;
        
        /// <summary>Constant for castling right. </summary>
        private const int BLACK_CASTLE_KINGSIDE = 0x04;
        
        /// <summary>Constant for castling right. </summary>
        private const int BLACK_CASTLE_QUEENSIDE = 0x08;
        
        /// <summary>The names of the chess pieces. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'PIECE_NAME'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        public static readonly char[] PIECE_NAME = new char[]{' ', 'P', 'N', 'B', 'R', 'Q', 'K'};
        
        /// <summary> An array of 64 bitboards containing the attacks originating from a
        /// square.
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'atkTo' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private BitBoard[] attackTo;
        
        /// <summary> An array of 64 bitboards indicating the squares from which the
        /// respective square is attacked.
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'atkFr' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private BitBoard[] attackFrom;
        
        /// <summary>Bitboards containing the piece masks. </summary>
        private BitBoard[][] pieceMask = new BitBoard[2][];
        
        /// <summary>A single bitboard for all sliding pieces on the board. </summary>
        private BitBoard slidingPieces;
        
        /// <summary>The board. </summary>
        //UPGRADE_NOTE: The initialization of  'board' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private int[] board;
        
        /// <summary>Position of the white/black king. </summary>
        private int[] kingPos = new int[2];
        
        /// <summary>The enPassant square. </summary>
        private int enPassant;
        
        /// <summary>The castle status. </summary>
        private int castle;
        
        /// <summary> Indicates the side to move, <code>true</code> if white to move,
        /// <code>false</code> otherwise.
        /// </summary>
        private bool whiteToMove;
        
        /// <summary>The current ply. </summary>
        private int player;
        
        /// <summary>History information, required for undo. </summary>
        private IList<History> history = new List<History>();
        
        /// <summary>The positional hash key. </summary>
        private long positionHash;
        
        /// <summary>The pawn hash key. </summary>
        private long pawnHash;
        
        /// <summary>The material signature. </summary>
        private int[] materialSignature = new int[2];
        
        /// <summary>The Evaluator for this board. </summary>
        private IEvaluator evaluator;
        
        /// <summary>The increment needed to advance one rank. </summary>
        private const int RANK_INC = 8;
        
        /// <summary> Indicates wether a piece is a sliding piece - <code>true</code> for
        /// Bishop, Rook and Queen.
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'IS_SLIDING'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly bool[] IS_SLIDING = new bool[]{false, false, false, true, true, true, false};
        
        /// <summary> Table to translate EnPassant squares.
        /// EPTranslate[e4] = e3 means a pawns double stepped to e4 can be
        /// captured enpassant on e3
        /// EPTranslate[e3] = e4 means a enpassant capture on e3 will remove
        /// pawn on e4
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'EP_TRANSLATE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly int[] EN_PASSANT_TRANSLATE = new int[]{
            // BUGBUG need to correct en passant moves for multiple levels
            //  for now, only level H is populated

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

            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 

            0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 
            // rank 3
            BoardConstants_Fields.HA4, 
            BoardConstants_Fields.HB4, 
            BoardConstants_Fields.HC4, 
            BoardConstants_Fields.HD4, 
            BoardConstants_Fields.HE4, 
            BoardConstants_Fields.HF4, 
            BoardConstants_Fields.HG4, 
            BoardConstants_Fields.HH4, 
            // rank 4
            BoardConstants_Fields.HA3, 
            BoardConstants_Fields.HB3, 
            BoardConstants_Fields.HC3, 
            BoardConstants_Fields.HD3, 
            BoardConstants_Fields.HE3, 
            BoardConstants_Fields.HF3, 
            BoardConstants_Fields.HG3, 
            BoardConstants_Fields.HH3, 
            // rank 5
            BoardConstants_Fields.HA6, 
            BoardConstants_Fields.HB6, 
            BoardConstants_Fields.HC6, 
            BoardConstants_Fields.HD6,
            BoardConstants_Fields.HE6,
            BoardConstants_Fields.HF6, 
            BoardConstants_Fields.HG6, 
            BoardConstants_Fields.HH6, 
            // rank 6
            BoardConstants_Fields.HA5, 
            BoardConstants_Fields.HB5, 
            BoardConstants_Fields.HC5, 
            BoardConstants_Fields.HD5, 
            BoardConstants_Fields.HE5, 
            BoardConstants_Fields.HF5, 
            BoardConstants_Fields.HG5, 
            BoardConstants_Fields.HH5, 
            0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0,

            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 

            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 
            
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

            0
        };
        
        /// <summary>Bits to calculate the material signature. </summary>
        private static short[] MATERIAL_SIGNATURE_BITS;
        
        /// <summary> This inner class keeps status information for doMove/undoMove.</summary>
        private sealed class History
        {
            
            /// <summary>The move. </summary>
            public int move;
            
            /// <summary>The en passant square. </summary>
            public int enPassant;
            
            /// <summary>The piece captured. </summary>
            public int capturedPiece;
            
            /// <summary>The castle status. </summary>
            public int castle;
            
            /// <summary>The position hash key. </summary>
            public long posHash;
            
            /// <summary>The pawn hash key. </summary>
            public long pawnHash;
            
            /// <summary> Create a History object.</summary>
            public History()
            {
            }
            
            /// <summary> Create a History object as a copy of another.
            /// 
            /// </summary>
            /// <param name="that">the source.
            /// </param>
            public History(History that)
            {
                this.move = that.move;
                this.enPassant = that.enPassant;
                this.capturedPiece = that.capturedPiece;
                this.castle = that.castle;
                this.posHash = that.posHash;
                this.pawnHash = that.pawnHash;
            }
        }
        
        
        /// <summary> Set the attacks of a piece of type <code>type</code> on
        /// <code>square</code> with <code>theWtm</code>.
        /// 
        /// </summary>
        /// <param name="type">the type of the piece (<code>Pawn</code> to
        /// <code>King</code>
        /// </param>
        /// <param name="theWtm"><code>true</code> if white to move
        /// </param>
        /// <param name="square">the target square
        /// </param>
        private void  attackSet(int type, bool theWtm, int square)
        {
            short[] np, nd;
            int nextSquare;
            
            if (type == ChessConstants_Fields.WHITE_PAWN)
            {
                if (theWtm)
                {
                    nd = Geometry.NEXT_DIR[Geometry.WHITE_PAWN][square];
                }
                else
                {
                    nd = Geometry.NEXT_DIR[Geometry.BLACK_PAWN][square];
                }

                nextSquare = nd[square];

                SetReciprocalAttacks(square, nextSquare);

                if ((nextSquare = nd[nextSquare]) >= 0)
                {
                    SetReciprocalAttacks(square, nextSquare);
                }
            }
            else
            {
                np = Geometry.NEXT_POS[type][square];
                nd = Geometry.NEXT_DIR[type][square];
                
                nextSquare = np[square];
                
                while (nextSquare >= 0)
                {
                    SetReciprocalAttacks(square, nextSquare);

                    if (board[nextSquare] != 0)
                    {
                        nextSquare = nd[nextSquare];
                    }
                    else
                    {
                        nextSquare = np[nextSquare];
                    }
                }
            }
        }

        private void SetReciprocalAttacks(int squareAttackFrom, int squareAttackTo)
        {
            attackTo[squareAttackFrom].SetBit(squareAttackTo);
            attackFrom[squareAttackTo].SetBit(squareAttackFrom);
        }

        /// <summary> Clear the attacks of a piece of type <code>type</code> on
        /// <code>square</code> with <code>theWtm</code>.
        /// 
        /// </summary>
        /// <param name="type">the type of the piece (<code>Pawn</code> to
        /// <code>King</code>
        /// </param>
        /// <param name="theWtm"><code>true</code> if white to move
        /// </param>
        /// <param name="square">the target square
        /// </param>
        private void  attackClr(int type, bool theWtm, int square)
        {
            short[] np, nd;
            int nsq;
            
            attackTo[square].IsEmpty();
            
            if (type == ChessConstants_Fields.PAWN)
            {
                if (theWtm)
                {
                    nd = Geometry.NEXT_DIR[Geometry.WHITE_PAWN][square];
                }
                else
                {
                    nd = Geometry.NEXT_DIR[Geometry.BLACK_PAWN][square];
                }
                
                nsq = nd[square];
                
                attackFrom[nsq].ClearBit(square);
                
                if ((nsq = nd[nsq]) >= 0)
                {
                    attackFrom[nsq].ClearBit(square);
                }
            }
            else
            {
                np = Geometry.NEXT_POS[type][square];
                nd = Geometry.NEXT_DIR[type][square];
                
                nsq = np[square];
                
                while (nsq >= 0)
                {
                    attackFrom[nsq].ClearBit(square);
                    
                    if (board[nsq] != 0)
                    {
                        nsq = nd[nsq];
                    }
                    else
                    {
                        nsq = np[nsq];
                    }
                }
            }
        }
        
        /// <summary> Output debugging information.</summary>
        public virtual void  debug()
        {
            log.Error("Problem at ply " + player);
            //log.Error(this);
            System.Collections.IEnumerator e = history.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (e.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                History h = (History) e.Current;
                log.Error(Move.toString(h.move));
            }
            // throw new RuntimeException();
        }
        
        /// <summary> Check the sanity of the internal data structures.
        /// 
        /// </summary>
        /// <throws>  IllegalStateException if the internal state of this ChessBoard </throws>
        /// <summary>                               has been corrupted
        /// </summary>
        internal virtual void  checkSanity()
        {
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                BitBoard tmp = new BitBoard(attackTo[i]);
                while (tmp.IsEmpty() == false)
                {
                    int idx = tmp.findFirstOne();
                    tmp[idx] = 0;
                    if (attackFrom[idx][i] == 0)
                    {
                        throw new System.SystemException();
                    }
                }
            }
            
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                BitBoard tmp = attackFrom[i];
                while (tmp.IsEmpty() == false)
                {
                    int idx = tmp.findFirstOne();
                    tmp[idx] = 0;
                    if ((attackTo[idx][i]) == 0)
                    {
                        log.Error("Panic 2.");
                        
                        log.Error("atkFr[" + i + "]");
                        log.Error(attackFrom[i]);
                        log.Error("---");
                        log.Error("atkTo[" + idx + "]");
                        log.Error(attackTo[idx]);
                        
                        throw new System.SystemException();
                    }
                }
            }
            
            checkMaterialSignatures();
        }
        
        /// <summary> Check the consistency of the material signatures.</summary>
        private void  checkMaterialSignatures()
        {
            int tw = 0;
            int tb = 0;
            
            for (int i = ChessConstants_Fields.PAWN; i <= ChessConstants_Fields.QUEEN; i++)
            {
                if (getMask(true, i).IsEmpty() == false)
                {
                    tw |= (ushort)MATERIAL_SIGNATURE_BITS[i];
                }
                if (getMask(false, i).IsEmpty() == false)
                {
                    tb |= (ushort)MATERIAL_SIGNATURE_BITS[i];
                }
            }
            
            if (tw != materialSignature[0])
            {
                log.Error("Material signature mismatch for white, expected " + System.Convert.ToString(tw, 16) + " but it is " + System.Convert.ToString(materialSignature[0], 16));
                debug();
                throw new System.SystemException();
            }
            if (tb != materialSignature[1])
            {
                log.Error("Material signature mismatch for black, expected " + System.Convert.ToString(tb, 16) + " but it is " + System.Convert.ToString(materialSignature[1], 16));
                debug();
                throw new System.SystemException();
            }
        }
        
        /// <summary> Recalculate Attacks from "from" to "to" after the piece on "to" has
        /// been removed.
        /// 
        /// </summary>
        /// <param name="from">the from square
        /// </param>
        /// <param name="to">the to square
        /// </param>
        private void  gainAttack(int from, int to)
        {
            short[] nsq = Geometry.NEXT_SQ[from];
            int square = to;
            
            for (; ; )
            {
                square = nsq[square];
                if (square < 0)
                {
                    break;
                }

                SetReciprocalAttacks(from, square);
                
                if (board[square] != 0)
                {
                    break;
                }
            }
        }
        
        /// <summary> Recalculate Attacks from "from" to "to" after a piece has been put
        /// onto "to".
        /// 
        /// </summary>
        /// <param name="from">the from square
        /// </param>
        /// <param name="to">the to square
        /// </param>
        private void  looseAttack(int from, int to)
        {
            short[] nsq = Geometry.NEXT_SQ[from];
            int square = to;
            
            for (; ; )
            {
                square = nsq[square];
                if (square < 0)
                {
                    break;
                }
                
                attackTo[from].ClearBit(square);
                attackFrom[square].ClearBit(from);
                
                if (board[square] != 0)
                {
                    break;
                }
            }
        }
        
        /// <summary> Recalculate all ray attacks which pass through square "to" after
        /// the piece on this square has been removed.
        /// 
        /// </summary>
        /// <param name="to">the square on which the piece was removed.
        /// </param>
        private void  gainAttacks(int to)
        {
            BitBoard tmp = attackFrom[to] & slidingPieces;
            
            while (tmp.IsEmpty() == false)
            {
                int i = tmp.findFirstOne();
                tmp.ClearBit(i);
                gainAttack(i, to);
            }
        }
        
        /// <summary> Recalculate all ray attacks which pass through square "to" after
        /// a piece has been put onto this square.
        /// 
        /// </summary>
        /// <param name="to">the square on which the piece was placed
        /// </param>
        private void  looseAttacks(int to)
        {
            BitBoard tmp = attackFrom[to] & slidingPieces;
            
            while (tmp.IsEmpty() == false)
            {
                int i = tmp.findFirstOne();
                tmp.ClearBit(i);
                looseAttack(i, to);
            }
        }
        
        /// <summary> Given the Masks and the board[] array, recalculate all necessary
        /// data.
        /// </summary>
        private void  recalcAttacks()
        {
            int i;
            BitBoard tmp;
            
            for (i = 0; i < BitBoard.SIZE; i++)
            {
                attackTo[i] = new BitBoard();
                attackFrom[i] = new BitBoard();
            }
            
            for (i = 0; i <= ChessConstants_Fields.KING; i++)
            {
                pieceMask[0][i] = new BitBoard();
                pieceMask[1][i] = new BitBoard();
            }
            
            slidingPieces = new BitBoard();
            
            positionHash = 0L;
            pawnHash = 0L;
            
            materialSignature[0] = 0;
            materialSignature[1] = 0;
            
            for (int square = 0; square < BitBoard.SIZE; square++)
            {
                int pc = board[square];
                
                if (pc > 0)
                {
                    pieceMask[0][0].SetBit(square);
                    pieceMask[0][pc].SetBit(square);
                    materialSignature[0] |= (ushort)MATERIAL_SIGNATURE_BITS[pc];
                    
                    if (IS_SLIDING[pc])
                    {
                        slidingPieces[square] = 1;
                    }
                    
                    positionHash ^= Hashing.HASH_KEYS[0][pc][square];
                    if (pc == ChessConstants_Fields.PAWN)
                    {
                        pawnHash ^= Hashing.HASH_KEYS[0][ChessConstants_Fields.PAWN][square];
                    }
                    if (pc == ChessConstants_Fields.KING)
                    {
                        kingPos[0] = square;
                    }
                }
                else if (pc < 0)
                {
                    pieceMask[1][0].SetBit(square);
                    pieceMask[1][- pc].SetBit(square);
                    materialSignature[1] |= (ushort)MATERIAL_SIGNATURE_BITS[-pc];
                    
                    if (IS_SLIDING[- pc])
                    {
                        slidingPieces[square] = 1;
                    }
                    
                    positionHash ^= Hashing.HASH_KEYS[1][- pc][square];
                    if (- pc == ChessConstants_Fields.PAWN)
                    {
                        pawnHash ^= Hashing.HASH_KEYS[1][ChessConstants_Fields.PAWN][square];
                    }
                    if (- pc == ChessConstants_Fields.KING)
                    {
                        kingPos[1] = square;
                    }
                }
            }
            
            tmp = new BitBoard(pieceMask[0][0]);
            while (tmp.IsEmpty() == false)
            {
                int square = tmp.findFirstOne();
                tmp[square] = 0;
                attackSet(board[square], true, square);
            }
            
            tmp = new BitBoard(pieceMask[1][0]);
            while (tmp.IsEmpty() == false)
            {
                int square = tmp.findFirstOne();
                tmp[square] = 0;
                attackSet(- board[square], false, square);
            }
            
            // Clear en passant square if it is set but no pawn attacks it
            if (enPassant != 0 && ((getMask(whiteToMove, ChessConstants_Fields.PAWN) & attackFrom[enPassant]).IsEmpty()))
            {
                enPassant = 0;
            }
            
            if (enPassant != 0)
            {
                positionHash ^= Hashing.EN_PASSANT_HASH_KEYS[enPassant];
            }
            positionHash ^= Hashing.CASTLE_HASH_KEYS[castle];
        }
        
        /// <summary> Make a move on the board.
        /// 
        /// </summary>
        /// <param name="move">the move to be made
        /// </param>
        public virtual void  doMove(int move)
        {
            
            if (history.Count <= player)
            {
                history.Add(new History());
            }
            History hist = (History) history[player];
            
            // remember status information
            hist.enPassant = enPassant;
            hist.castle = castle;
            hist.move = move;
            hist.posHash = positionHash;
            hist.pawnHash = pawnHash;
            
            int from = Move.getFrom(move);
            int to = Move.getTo(move);
            int type = getPieceAt(from);
            //UPGRADE_NOTE: Final was removed from the declaration of 'side '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
            int side = Side(whiteToMove);
            
            //
            // Remove the piece from its from square
            //
            
            attackClr(type, whiteToMove, from);
            
            board[from] = 0;
            pieceMask[side][0].ClearBit(from);
            pieceMask[side][type].ClearBit(from);
            if (IS_SLIDING[type])
            {
                slidingPieces.ClearBit(from);
            }
            gainAttacks(from);
            
            positionHash ^= Hashing.HASH_KEYS[side][type][from];
            if (type == ChessConstants_Fields.PAWN)
            {
                pawnHash ^= Hashing.HASH_KEYS[side][ChessConstants_Fields.PAWN][from];
            }
            
            if ((move & Move.CAPTURE) != 0)
            {
                //
                // Handle captures
                //
                
                //UPGRADE_NOTE: Final was removed from the declaration of 'captured '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
                int captured = getPieceAt(to);
                if (captured == ChessConstants_Fields.KING)
                {
                    log.Fatal("Captured a king.");
                    throw new System.SystemException("Captured a king.");
                }
                
                attackClr(captured, !whiteToMove, to);
                hist.capturedPiece = captured;
                pieceMask[1 ^ side][0].ClearBit(to);
                pieceMask[1 ^ side][captured].ClearBit(to);
                if (IS_SLIDING[captured])
                {
                    slidingPieces.ClearBit(to);
                }
                
                // Update castling rights if rook captured
                
                if (whiteToMove)
                {
                    if (to == BoardConstants_Fields.HH8)
                    {
                        castle &= ~ BLACK_CASTLE_KINGSIDE;
                    }
                    if (to == BoardConstants_Fields.HA8)
                    {
                        castle &= ~ BLACK_CASTLE_QUEENSIDE;
                    }
                }
                else
                {
                    if (to == BoardConstants_Fields.HH1)
                    {
                        castle &= ~ WHITE_CASTLE_KINGSIDE;
                    }
                    if (to == BoardConstants_Fields.HA1)
                    {
                        castle &= ~ WHITE_CASTLE_QUEENSIDE;
                    }
                }
                
                positionHash ^= Hashing.HASH_KEYS[1 ^ side][captured][to];
                if (captured == ChessConstants_Fields.PAWN)
                {
                    pawnHash ^= Hashing.HASH_KEYS[1 ^ side][ChessConstants_Fields.PAWN][to];
                }
                
                if (evaluator != null)
                {
                    evaluator.move(from, to, type, whiteToMove);
                    evaluator.capture(to, captured, !whiteToMove);
                }
                
                if (pieceMask[1 ^ side][captured].IsEmpty())
                {
                    materialSignature[1 ^ side] &= ~ MATERIAL_SIGNATURE_BITS[captured];
                }
            }
            else if ((move & Move.ENPASSANT) != 0)
            {
                //
                // Handle en passant captures
                //
                
                int epto = EN_PASSANT_TRANSLATE[to];
                attackClr(ChessConstants_Fields.PAWN, !whiteToMove, epto);
                hist.capturedPiece = ChessConstants_Fields.PAWN;
                pieceMask[1 ^ side][0].ClearBit(epto);
                pieceMask[1 ^ side][ChessConstants_Fields.PAWN].ClearBit(epto);
                board[epto] = 0;
                gainAttacks(epto);
                looseAttacks(to);
                
                positionHash ^= Hashing.HASH_KEYS[1 ^ side][ChessConstants_Fields.PAWN][epto];
                pawnHash ^= Hashing.HASH_KEYS[1 ^ side][ChessConstants_Fields.PAWN][epto];
                
                if (evaluator != null)
                {
                    evaluator.move(from, to, ChessConstants_Fields.PAWN, whiteToMove);
                    evaluator.capture(epto, ChessConstants_Fields.PAWN, !whiteToMove);
                }
                
                if (pieceMask[1 ^ side][ChessConstants_Fields.PAWN].IsEmpty())
                {
                    materialSignature[1 ^ side] &= ~ MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.PAWN];
                }
            }
            else if (Move.isCastle(move))
            {
                //
                // Handle castling
                //
                
                int rookFrom;
                int rookTo;
                
                if (Move.isKingSideCastle(move))
                {
                    rookFrom = from + 3;
                    rookTo = from + 1;
                }
                else
                {
                    rookFrom = from - 4;
                    rookTo = from - 1;
                }
                
                attackClr(ChessConstants_Fields.ROOK, whiteToMove, rookFrom);
                
                board[rookTo] = board[rookFrom];
                board[rookFrom] = 0;
                
                pieceMask[side][0].ClearBit(rookFrom);
                pieceMask[side][ChessConstants_Fields.ROOK].ClearBit(rookFrom);
                slidingPieces.ClearBit(rookFrom);
                
                pieceMask[side][0].SetBit(rookTo);
                pieceMask[side][ChessConstants_Fields.ROOK].SetBit(rookTo);
                slidingPieces.SetBit(rookTo);
                
                attackSet(ChessConstants_Fields.ROOK, whiteToMove, rookTo);
                looseAttacks(rookTo);
                looseAttacks(to);
                
                // Update castling rights
                
                if (whiteToMove)
                {
                    castle &= ~ (WHITE_CASTLE_QUEENSIDE | WHITE_CASTLE_KINGSIDE);
                }
                else
                {
                    castle &= ~ (BLACK_CASTLE_QUEENSIDE | BLACK_CASTLE_KINGSIDE);
                }
                
                positionHash ^= (Hashing.HASH_KEYS[side][ChessConstants_Fields.ROOK][rookFrom] ^ Hashing.HASH_KEYS[side][ChessConstants_Fields.ROOK][rookTo]);
                
                if (evaluator != null)
                {
                    evaluator.move(from, to, ChessConstants_Fields.KING, whiteToMove);
                    evaluator.move(rookFrom, rookTo, ChessConstants_Fields.ROOK, whiteToMove);
                }
            }
            else
            {
                //
                // Handle ordinary move
                //
                
                looseAttacks(to);
                
                if (evaluator != null)
                {
                    evaluator.move(from, to, type, whiteToMove);
                }
            }
            
            // Update castling rights if rook or king moved
            if (whiteToMove)
            {
                if (from == BoardConstants_Fields.HH1)
                {
                    castle &= ~ WHITE_CASTLE_KINGSIDE;
                }
                if (from == BoardConstants_Fields.HA1)
                {
                    castle &= ~ WHITE_CASTLE_QUEENSIDE;
                }
                if (from == BoardConstants_Fields.HE1)
                {
                    castle &= ~ (WHITE_CASTLE_QUEENSIDE | WHITE_CASTLE_KINGSIDE);
                }
            }
            else
            {
                if (from == BoardConstants_Fields.HH8)
                {
                    castle &= ~ BLACK_CASTLE_KINGSIDE;
                }
                if (from == BoardConstants_Fields.HA8)
                {
                    castle &= ~ BLACK_CASTLE_QUEENSIDE;
                }
                if (from == BoardConstants_Fields.HE8)
                {
                    castle &= ~ (BLACK_CASTLE_QUEENSIDE | BLACK_CASTLE_KINGSIDE);
                }
            }
            
            //
            // Handle promotion
            //
            
            if (Move.isPromotion(move))
            {
                int promotedTo = Move.getPromoPiece(move);
                if (evaluator != null)
                {
                    evaluator.capture(to, ChessConstants_Fields.PAWN, whiteToMove);
                    evaluator.add(to, promotedTo, whiteToMove);
                }

                materialSignature[side] |= (ushort)MATERIAL_SIGNATURE_BITS[promotedTo];
                if (pieceMask[side][ChessConstants_Fields.PAWN].IsEmpty())
                {
                    materialSignature[side] &= ~ MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.PAWN];
                }
                
                type = promotedTo;
            }
            
            board[to] = whiteToMove?type:- type;
            pieceMask[side][0].SetBit(to);
            pieceMask[side][type].SetBit(to);
            if (IS_SLIDING[type])
            {
                slidingPieces.SetBit(to);
            }
            if (type == ChessConstants_Fields.KING)
            {
                kingPos[side] = to;
            }
            
            attackSet(type, whiteToMove, to);
            
            positionHash ^= Hashing.HASH_KEYS[side][type][to];
            if (type == ChessConstants_Fields.PAWN)
            {
                pawnHash ^= Hashing.HASH_KEYS[side][ChessConstants_Fields.PAWN][to];
            }
            
            //
            // If double pawn push, set en passant square
            //
            
            if ((move & Move.PAWN_DOUBLE) != 0)
            {
                int etmp = EN_PASSANT_TRANSLATE[to];
                if ((pieceMask[1 ^ side][ChessConstants_Fields.PAWN] & attackFrom[etmp]).IsEmpty() == false)
                {
                    enPassant = etmp;
                }
                else
                {
                    enPassant = 0;
                }
            }
            else
            {
                enPassant = 0;
            }
            
            if (castle != hist.castle)
            {
                positionHash ^= (Hashing.CASTLE_HASH_KEYS[castle] ^ Hashing.CASTLE_HASH_KEYS[hist.castle]);
            }
            if (enPassant != hist.enPassant)
            {
                positionHash ^= (Hashing.EN_PASSANT_HASH_KEYS[enPassant] ^ Hashing.EN_PASSANT_HASH_KEYS[hist.enPassant]);
            }
            
            positionHash ^= Hashing.WTM_HASH;
            
            player++;
            whiteToMove = !whiteToMove;
        }
        
        /// <summary> Do a null move.</summary>
        public virtual void  doNull()
        {
            if (history.Count <= player)
            {
                history.Add(new History());
            }
            History hist = (History) history[player];
            
            // remember status information
            hist.enPassant = enPassant;
            hist.castle = castle;
            hist.move = 0;
            hist.posHash = positionHash;
            hist.pawnHash = pawnHash;
            
            enPassant = 0;
            
            if (enPassant != hist.enPassant)
            {
                positionHash ^= (Hashing.EN_PASSANT_HASH_KEYS[enPassant] ^ Hashing.EN_PASSANT_HASH_KEYS[hist.enPassant]);
            }
            
            positionHash ^= Hashing.WTM_HASH;
            
            player++;
            whiteToMove = !whiteToMove;
        }
        
        /// <summary> Un-Make the last move on the board.</summary>
        public virtual void  undoMove()
        {
            player--;
            whiteToMove = !whiteToMove;
            History hist = (History) history[player];
            
            int move = hist.move;
            
            if (move != 0)
            {
                int from = Move.getFrom(move);
                int to = Move.getTo(move);
                int type = getPieceAt(to);
                int side = Side(whiteToMove);
                
                attackClr(type, whiteToMove, to);
                pieceMask[side][0].ClearBit(to);
                pieceMask[side][type].ClearBit(to);
                if (IS_SLIDING[type])
                {
                    slidingPieces.ClearBit(to);
                }
                
                if ((move & Move.CAPTURE) != 0)
                {
                    //
                    // Handle captures
                    //
                    
                    int captured = hist.capturedPiece;
                    
                    attackSet(captured, !whiteToMove, to);
                    pieceMask[1 ^ side][0].SetBit(to);
                    pieceMask[1 ^ side][captured].SetBit(to);
                    if (IS_SLIDING[captured])
                    {
                        slidingPieces.SetBit(to);
                    }
                    board[to] = whiteToMove?- captured:captured;
                    
                    if (evaluator != null)
                    {
                        evaluator.add(to, captured, !whiteToMove);
                        evaluator.move(to, from, type, whiteToMove);
                    }
                    materialSignature[1 ^ side] |= (ushort)MATERIAL_SIGNATURE_BITS[captured];
                }
                else if ((move & Move.ENPASSANT) != 0)
                {
                    //
                    // Handle en passant captures
                    //
                    
                    int enPassantTo = EN_PASSANT_TRANSLATE[to];
                    
                    attackSet(ChessConstants_Fields.PAWN, !whiteToMove, enPassantTo);
                    pieceMask[1 ^ side][0].SetBit(enPassantTo);
                    pieceMask[1 ^ side][ChessConstants_Fields.PAWN].SetBit(enPassantTo);
                    board[enPassantTo] = whiteToMove? ChessConstants_Fields.BLACK_PAWN:ChessConstants_Fields.WHITE_PAWN;
                    looseAttacks(enPassantTo);
                    
                    board[to] = 0;
                    gainAttacks(to);
                    
                    if (evaluator != null)
                    {
                        evaluator.add(enPassantTo, ChessConstants_Fields.PAWN, !whiteToMove);
                        evaluator.move(to, from, ChessConstants_Fields.PAWN, whiteToMove);
                    }
                    materialSignature[1 ^ side] |= (ushort)MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.PAWN];
                }
                else if (Move.isCastle(move))
                {
                    //
                    // Handle castling
                    //
                    
                    int rookFrom;
                    int rookTo;
                    
                    if (Move.isKingSideCastle(move))
                    {
                        rookFrom = from + 3;
                        rookTo = from + 1;
                    }
                    else
                    {
                        rookFrom = from - 4;
                        rookTo = from - 1;
                    }
                    
                    attackClr(ChessConstants_Fields.ROOK, whiteToMove, rookTo);
                    
                    board[rookFrom] = board[rookTo];
                    board[rookTo] = 0;
                    board[to] = 0;
                    
                    pieceMask[side][0].ClearBit(rookTo);
                    pieceMask[side][ChessConstants_Fields.ROOK].ClearBit(rookTo);
                    slidingPieces.ClearBit(rookTo);

                    pieceMask[side][0].SetBit(rookFrom);
                    pieceMask[side][ChessConstants_Fields.ROOK].SetBit(rookFrom);
                    slidingPieces.SetBit(rookFrom);

                    attackSet(ChessConstants_Fields.ROOK, whiteToMove, rookFrom);
                    gainAttacks(rookTo);
                    gainAttacks(to);
                    
                    if (evaluator != null)
                    {
                        evaluator.move(to, from, ChessConstants_Fields.KING, whiteToMove);
                        evaluator.move(rookTo, rookFrom, ChessConstants_Fields.ROOK, whiteToMove);
                    }
                }
                else
                {
                    //
                    // Handle ordinary move
                    //
                    
                    board[to] = 0;
                    gainAttacks(to);
                    
                    if (evaluator != null)
                    {
                        evaluator.move(to, from, type, whiteToMove);
                    }
                }
                
                //
                // Handle promotion
                //
                
                if (Move.isPromotion(move))
                {
                    if (evaluator != null)
                    {
                        evaluator.capture(from, type, whiteToMove);
                        evaluator.add(from, ChessConstants_Fields.PAWN, whiteToMove);
                    }
                    materialSignature[side] |= (ushort)MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.PAWN];
                    if (pieceMask[side][type].IsEmpty())
                    {
                        materialSignature[side] &= ~ MATERIAL_SIGNATURE_BITS[type];
                    }
                    type = ChessConstants_Fields.PAWN;
                }
                
                board[from] = whiteToMove?type:- type;
                pieceMask[side][0].SetBit(from);
                pieceMask[side][type].SetBit(from);

                if (IS_SLIDING[type])
                {
                    slidingPieces.SetBit(from);
                }
                attackSet(type, whiteToMove, from);
                looseAttacks(from);
                if (type == ChessConstants_Fields.KING)
                {
                    kingPos[side] = from;
                }
            }
            
            enPassant = hist.enPassant;
            castle = hist.castle;
            positionHash = hist.posHash;
            pawnHash = hist.pawnHash;
        }
        
        /// <summary> Get the piece on <code>square</code>.
        /// 
        /// </summary>
        /// <param name="square">the square on the chessboard (0...343)
        /// </param>
        /// <returns> the type of the piece on <code>square</code> (0,
        /// <code>Pawn</code> ... <code>King</code>
        /// </returns>
        public int getPieceAt(int square)
        {
            return System.Math.Abs(board[square]);
        }

        /// <summary> Get the piece on <code>square</code>.
        /// 
        /// </summary>
        /// <param name="square">the square on the chessboard (0...343)
        /// </param>
        /// <returns> the type of the piece on <code>square</code> (0,
        /// <code>Pawn</code> ... <code>King</code>
        /// </returns>
        public int getPieceAt(int level, int rank, int file)
        {
            return System.Math.Abs(board[BitBoard.BitOffset(level, rank, file)]);
        }

        /// <summary> Get the color of the piece on <code>square</code>.
        /// 
        /// </summary>
        /// <param name="square">the square on the chessboard (0...343)
        /// </param>
        /// <returns> <code>true</code> if the piece on <code>square</code>
        /// is white, <code>false</code> otherwise.
        /// </returns>
        public engine.Player getSideAt(int square)
        {
            return (board[square] == 0) ? Player.none : ((board[square] < 0) ? Player.black : Player.white);
        }

        /// <summary> Get the color of the piece on <code>square</code>.
        /// 
        /// </summary>
        /// <param name="square">the square on the chessboard (0...343)
        /// </param>
        /// <returns> <code>true</code> if the piece on <code>square</code>
        /// is white, <code>false</code> otherwise.
        /// </returns>
        public bool isWhiteAt(int level, int rank, int file)
        {
            return board[BitBoard.BitOffset(level, rank, file)] > 0;
        }

        /// <summary> Check if castling is legal in the current position.
        /// 
        /// </summary>
        /// <param name="move">the castling move to check
        /// </param>
        /// <returns> <code>true</code> if castling is legal.
        /// </returns>
        public bool isCastleLegal(int move)
        {
            int from = Move.getFrom(move);
            int oside = whiteToMove?1:0;
            
            if ((move & Move.CASTLE_KSIDE) != 0)
            {
                if (whiteToMove && (castle & WHITE_CASTLE_KINGSIDE) == 0)
                {
                    return false;
                }
                if (!whiteToMove && (castle & BLACK_CASTLE_KINGSIDE) == 0)
                {
                    return false;
                }
                if (board[from + 1] != 0 || board[from + 2] != 0)
                {
                    return false;
                }
                
                return ((attackFrom[from] | attackFrom[from + 1] | attackFrom[from + 2]) & pieceMask[oside][0]).IsEmpty();
            }
            else
            {
                if (whiteToMove && (castle & WHITE_CASTLE_QUEENSIDE) == 0)
                {
                    return false;
                }
                if (!whiteToMove && (castle & BLACK_CASTLE_QUEENSIDE) == 0)
                {
                    return false;
                }
                if (board[from - 1] != 0 || board[from - 2] != 0 || board[from - 3] != 0)
                {
                    return false;
                }
                return ((attackFrom[from] | attackFrom[from - 1] | attackFrom[from - 2]) & pieceMask[oside][0]).IsEmpty();
            }
            
            // never reached
        }
        
        /// <summary> Generate the pseudo legal moves in this position.
        /// A pseudo legal move is a move which may leave the moving
        /// side's king in check but is legal otherwise.
        /// 
        /// </summary>
        /// <param name="mvs">the MoveList to add moves to.
        /// </param>
        public virtual void  generatePseudoLegalMoves(IMoveList mvs)
        {
            BitBoard tmp = getMask(!whiteToMove);

            while (tmp.IsEmpty() == false)
            {
                int to = tmp.findFirstOne();
                tmp.ClearBit(to);

                generateTo(to, mvs);
            }

            tmp = getMask(whiteToMove);

            while (tmp.IsEmpty() == false)
            {
                int to = tmp.findFirstOne();
                tmp.ClearBit(to);

                generateFrom(to, mvs);
            }
            
            generateEnPassant(mvs);
        }

        /// <summary> Generate all legal moves in this position.
        /// 
        /// </summary>
        /// <param name="mvs">the MoveList in which the moves will be inserted
        /// </param>
        public virtual void  generateLegalMoves(IMoveList mvs)
        {
            IntVector tmvs = new IntVector();
            generatePseudoLegalMoves(tmvs);
            
            for (int i = 0; i < tmvs.size(); i++)
            {
                int move = tmvs.get_Renamed(i);
                if ((move & Move.CASTLE) != 0 && !isCastleLegal(move))
                {
                    continue;
                }
                doMove(move);
                bool inCheck = OppInCheck;
                undoMove();
                if (!inCheck)
                {
                    mvs.add(move);
                }
            }
        }
        
        /// <summary> Generate all capturing moves on a target square.
        /// 
        /// </summary>
        /// <param name="to">the target square.
        /// </param>
        /// <param name="mvs">the list to insert the moves into.
        /// </param>
        protected internal virtual void  generateTo(int to, IMoveList mvs)
        {
            BitBoard attacks = attackFrom[to] & getMask(whiteToMove);
            
            while (attacks.IsEmpty() == false)
            {
                int from = attacks.findFirstOne();
                attacks.ClearBit(from);
                
                // BUGBUG fix row calculation for pawn prmootion to 3D
                if (getPieceAt(from) == ChessConstants_Fields.PAWN && (to <= BoardConstants_Fields.HH1 || to >= BoardConstants_Fields.HA8))
                {
                    // promotion
                    int move = Move.makeMove(from, to) | Move.CAPTURE;
                    mvs.add(move | Move.PROMO_QUEEN);
                    mvs.add(move | Move.PROMO_ROOK);
                    mvs.add(move | Move.PROMO_BISHOP);
                    mvs.add(move | Move.PROMO_KNIGHT);
                }
                else
                {
                    // normal capture
                    mvs.add(Move.makeMove(from, to) | Move.CAPTURE);
                }
            }
        }
        
        /// <summary> Generate all non-capturing moves originating from a square.
        /// 
        /// </summary>
        /// <param name="from">the originating square.
        /// </param>
        /// <param name="mvs">the list to insert the moves into.
        /// </param>
        protected internal virtual void  generateFrom(int from, IMoveList mvs)
        {
            int pc = getPieceAt(from);
            bool isWhite = getSideAt(from) == Player.white;
            if (pc == ChessConstants_Fields.PAWN)
            {
                if (isWhite)
                {
                    
                    // white pawns
                    
                    int next = from + RANK_INC;
                    if (board[next] == 0)
                    {
                        // BUGBUG fix row calculation for pawn prmootion to 3D
                        if (next >= BoardConstants_Fields.HA8)
                        {
                            // promotion
                            int move = Move.makeMove(from, next);
                            mvs.add(move | Move.PROMO_QUEEN);
                            mvs.add(move | Move.PROMO_ROOK);
                            mvs.add(move | Move.PROMO_BISHOP);
                            mvs.add(move | Move.PROMO_KNIGHT);
                        }
                        else
                        {
                            mvs.add(Move.makeMove(from, next));
                            if (next >= BoardConstants_Fields.HA3 && next <= BoardConstants_Fields.HH3)
                            {
                                next += RANK_INC;
                                if (board[next] == 0)
                                {
                                    mvs.add(Move.makeMove(from, next) | Move.PAWN_DOUBLE);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // black pawns
                    int next = from - RANK_INC;
                    if (board[next] == 0)
                    {
                        // BUGBUG fix row calculation for pawn prmootion to 3D
                        if (next <= BoardConstants_Fields.HH1)
                        {
                            // promotion
                            int move = Move.makeMove(from, next);
                            mvs.add(move | Move.PROMO_QUEEN);
                            mvs.add(move | Move.PROMO_ROOK);
                            mvs.add(move | Move.PROMO_BISHOP);
                            mvs.add(move | Move.PROMO_KNIGHT);
                        }
                        else
                        {
                            mvs.add(Move.makeMove(from, next));
                            if (next >= BoardConstants_Fields.HA6 && next <= BoardConstants_Fields.HH6)
                            {
                                next -= RANK_INC;
                                if (board[next] == 0)
                                {
                                    mvs.add(Move.makeMove(from, next) | Move.PAWN_DOUBLE);
                                }
                            }
                        }
                    }
                }
            }
            else if (pc == ChessConstants_Fields.KING)
            {
                BitBoard attack = attackTo[from] & ~ (pieceMask[0][0] | pieceMask[1][0]);
                BitBoard opponent = getMask(!isWhite);
                
                while (attack.IsEmpty() == false)
                {
                    int to = attack.findFirstOne();
                    attack.ClearBit(to);
                    
                    if ((attackFrom[to] & opponent).IsEmpty())
                    {
                        mvs.add(Move.makeMove(from, to));
                    }
                }
                
                if (isWhite)
                {
                    if ((castle & WHITE_CASTLE_KINGSIDE) != 0)
                    {
                        mvs.add(Move.makeMove(BoardConstants_Fields.HE1, BoardConstants_Fields.HG1) | Move.CASTLE_KSIDE);
                    }
                    if ((castle & WHITE_CASTLE_QUEENSIDE) != 0)
                    {
                        mvs.add(Move.makeMove(BoardConstants_Fields.HE1, BoardConstants_Fields.HC1) | Move.CASTLE_QSIDE);
                    }
                }
                else
                {
                    if ((castle & BLACK_CASTLE_KINGSIDE) != 0)
                    {
                        mvs.add(Move.makeMove(BoardConstants_Fields.HE8, BoardConstants_Fields.HG8) | Move.CASTLE_KSIDE);
                    }
                    if ((castle & BLACK_CASTLE_QUEENSIDE) != 0)
                    {
                        mvs.add(Move.makeMove(BoardConstants_Fields.HE8, BoardConstants_Fields.HC8) | Move.CASTLE_QSIDE);
                    }
                }
            }
            else
            {
                BitBoard attacks = attackTo[from] & ~ (pieceMask[0][0] | pieceMask[1][0]);
                while (attacks.IsEmpty() == false)
                {
                    int to = attacks.findFirstOne();
                    attacks.ClearBit(to);
                    
                    mvs.add(Move.makeMove(from, to));
                }
            }
        }
        
        /// <summary> Generate en passant captures.
        /// 
        /// </summary>
        /// <param name="mvs">the list to insert the moves into.
        /// </param>
        protected internal virtual void  generateEnPassant(IMoveList mvs)
        {
            if (enPassant != 0)
            {
                BitBoard attacks = attackFrom[enPassant] & getMask(whiteToMove, ChessConstants_Fields.PAWN);
                
                while (attacks.IsEmpty() == false)
                {
                    int from = attacks.findFirstOne();
                    attacks.ClearBit(from);
                    
                    mvs.add(Move.makeMove(from, enPassant) | Move.ENPASSANT);
                }
            }
        }
        
        /// <summary> Check if <code>move</code> is pseudo legal in the current position.
        /// 
        /// </summary>
        /// <param name="move">the move
        /// </param>
        /// <returns> the legality of the move
        /// </returns>
        public virtual bool isPseudoLegalMove(int move)
        {
            int from = Move.getFrom(move);
            
            // There must be a right-colored piece on the from-square
            if (whiteToMove && board[from] <= 0)
            {
                return false;
            }
            if (!whiteToMove && board[from] >= 0)
            {
                return false;
            }
            
            // if promotion, moving piece must be a pawn
            if (Move.isPromotion(move) && getPieceAt(from) != ChessConstants_Fields.PAWN)
            {
                return false;
            }
            
            int to = Move.getTo(move);
            
            if ((move & Move.CAPTURE) != 0)
            {
                // if capture, there must be an enemy piece on the to-square,
                // and the to square must be attacked from the from-square.
                if (whiteToMove && board[to] >= 0)
                {
                    return false;
                }
                if (!whiteToMove && board[to] <= 0)
                {
                    return false;
                }
                
                return (attackTo[from][to]) != 0;
            }
            else if ((move & Move.ENPASSANT) != 0)
            {
                // if enpassant capture, the to-square must be the enpassant
                // square, and the moving piece must be a pawn
                if (enPassant == 0 || to != enPassant)
                {
                    return false;
                }
                if (getPieceAt(from) != ChessConstants_Fields.PAWN)
                {
                    return false;
                }
                return (attackTo[from][to]) != 0;
            }
            else if ((move & Move.CASTLE) != 0)
            {
                return isCastleLegal(move);
            }
            else
            {
                // to-square must be empty
                if (board[to] != 0)
                {
                    return false;
                }
                if (getPieceAt(from) == ChessConstants_Fields.PAWN)
                {
                    int next = whiteToMove?from + RANK_INC:from - RANK_INC;
                    if ((move & Move.PAWN_DOUBLE) != 0)
                    {
                        if (board[next] != 0)
                        {
                            return false;
                        }
                        next = whiteToMove?next + RANK_INC:next - RANK_INC;
                    }
                    
                    if (next != to)
                    {
                        return false;
                    }
                    
                    if (whiteToMove && !Move.isPromotion(move) && to >= BoardConstants_Fields.HA8)
                    {
                        return false;
                    }
                    if (!whiteToMove && !Move.isPromotion(move) && to <= BoardConstants_Fields.HH1)
                    {
                        return false;
                    }
                }
                else
                {
                    if ((attackTo[from][to]) == 0)
                    {
                        return false;
                    }
                    if ((move & Move.PAWN_DOUBLE) != 0)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        /// <summary> Create a textual representatiquion of the current position.
        /// 
        /// </summary>
        /// <returns> the current position as an ASCII graphics board.
        /// </returns>
        public override System.String ToString()
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            
            for (int level = BitBoard.LEVEL_WIDTH.Length -1; level >= 0; level--)
            {
                IndentRow(buffer, level);

                buffer.Append("   +");
                for (int file = 0; file < BitBoard.LEVEL_WIDTH[level]; file++)
                {
                    buffer.Append("---+");
                }
                buffer.Append("\n");
                for (int rank = BitBoard.LEVEL_WIDTH[level] - 1; rank >= 0; rank--)
                {
                    IndentRow(buffer, level);
                    buffer.Append(' ');
                    buffer.Append((char)('1' + rank));
                    buffer.Append(' ');
                    for (int file = 0; file < BitBoard.LEVEL_WIDTH[level]; file++)
                    {
                        int i = BitBoard.BitOffset(level, rank, file);

                        buffer.Append('|');
                        if (enPassant != 0 && i == enPassant)
                        {
                            buffer.Append("<E>");
                        }
                        else
                        {
                            if (getSideAt(i) == Player.black)
                            {
                                buffer.Append('*');
                            }
                            else
                            {
                                buffer.Append(' ');
                            }

                            buffer.Append(PIECE_NAME[getPieceAt(i)]);

                            if (getSideAt(i) == Player.black)
                            {
                                buffer.Append('*');
                            }
                            else
                            {
                                buffer.Append(' ');
                            }
                        }
                    }
                    buffer.Append("|");
                    if (level == 7)
                    {
                        if (rank == 4)
                        {
                            buffer.Append("  Hashkey: ");
                            buffer.Append(System.Convert.ToString(positionHash, 16));
                        }
                        if ((rank == (BitBoard.LEVEL_WIDTH[level] - 1) && !whiteToMove) || (rank == 0 && whiteToMove))
                        {
                            buffer.Append(" *");
                        }
                    }

                    buffer.Append("\n   ");
                    IndentRow(buffer, level);
                    buffer.Append("+");
                    for (int file = 0; file < BitBoard.LEVEL_WIDTH[level]; file++)
                    {
                        buffer.Append("---+");
                    }
                    buffer.Append("\n");
                }
                buffer.Append("  ");
                IndentRow(buffer, level);
                for (int file = 0; file < BitBoard.LEVEL_WIDTH[level]; file++)
                {
                    buffer.Append("   ");
                    buffer.Append((char)(97 + file));
                }
                buffer.Append("\n");
            }
            return buffer.ToString();
        }

        private static void IndentRow(StringBuilder buffer, int level)
        {
            int File = 8 - BitBoard.LEVEL_WIDTH[level];

            while (File >0)
            {
                buffer.Append("  ");
                --File;
            }
        }

        /// <summary> Get the position of certain piece types.
        /// 
        /// </summary>
        /// <param name="theWhiteToMove">the side to move
        /// </param>
        /// <param name="type">the type
        /// </param>
        /// <param name="squares">array to store positions in
        /// </param>
        /// <returns> the count
        /// </returns>
        public virtual int getPositions(bool theWhiteToMove, int type, int[] squares)
        {
            BitBoard pieces = getMask(theWhiteToMove, type);
            int count = 0;
            
            while (pieces.IsEmpty() == false)
            {
                int square = pieces.findFirstOne();
                pieces.ClearBit(square);

                squares[count++] = square;
            }
            return count;
        }
        
        /// <summary> Get the material signature for a side.
        /// <p>
        /// The material signature is an integer derived by logical 'OR'ing
        /// the following constants:
        /// <ul>
        /// <li> 1 if at least one pawn present
        /// <li> 2 if at least one knight present
        /// <li> 4 if at least one bishop present
        /// <li> 8 if at least one rook present
        /// <li> 16 if at least one queen present
        /// </ul>
        /// 
        /// </summary>
        /// <param name="theWhiteToMove">the side to move, <code>true</code> for white
        /// </param>
        /// <returns> the material signature
        /// </returns>
        public virtual int getMaterialSignature(bool theWhiteToMove)
        {
            return materialSignature[Side(theWhiteToMove)];
        }
        
        /// <summary> Get the king's position.
        /// 
        /// </summary>
        /// <param name="theWhiteToMove">the side
        /// </param>
        /// <returns> the king's position for <code>theWtm</code>
        /// </returns>
        public int getKingPos(bool theWhiteToMove)
        {
            return kingPos[Side(theWhiteToMove)];
        }
        
        /// <summary> Get the mask for a given side.
        /// 
        /// </summary>
        /// <param name="theWhiteToMove">the side
        /// </param>
        /// <returns> a bitboard for the given side
        /// </returns>
        public BitBoard getMask(bool theWhiteToMove)
        {
            return new BitBoard(pieceMask[Side(theWhiteToMove)][0]);
        }
        
        /// <summary> Get the mask for a given side/piece type.
        /// 
        /// </summary>
        /// <param name="theWhiteToMove">the side
        /// </param>
        /// <param name="type">the piece type
        /// </param>
        /// <returns> a bitboard for the given side/piece type
        /// </returns>
        public BitBoard getMask(bool theWhiteToMove, int type)
        {
            return new BitBoard(pieceMask[Side(theWhiteToMove)][type]);
        }
        
        /// <summary> Check if white can castle to the king side.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if white can castle king side
        /// </returns>
        public virtual bool canWhiteCastleKingSide()
        {
            return (castle & WHITE_CASTLE_KINGSIDE) != 0;
        }
        
        /// <summary> Check if white can castle to the queen side.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if white can castle queen side
        /// </returns>
        public virtual bool canWhiteCastleQueenSide()
        {
            return (castle & WHITE_CASTLE_QUEENSIDE) != 0;
        }
        
        /// <summary> Check if black can castle to the king side.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if black can castle king side
        /// </returns>
        public virtual bool canBlackCastleKingSide()
        {
            return (castle & BLACK_CASTLE_KINGSIDE) != 0;
        }
        
        /// <summary> Check if black can castle to the queen side.
        /// 
        /// </summary>
        /// <returns> <code>true</code> if black can castle queen side
        /// </returns>
        public virtual bool canBlackCastleQueenSide()
        {
            return (castle & BLACK_CASTLE_QUEENSIDE) != 0;
        }
        
        // ----------------------------------------------------------------
        // -- package access only!
        // ----------------------------------------------------------------
        
        /// <summary> Get the atkFr mask for a square.
        /// 
        /// </summary>
        /// <param name="square">the square
        /// </param>
        /// <returns> the atkFr mask
        /// </returns>
        internal BitBoard getAttackFrom(int square)
        {
            return attackFrom[square];
        }
        
        /// <summary> Get the atkTo mask for a square.
        /// 
        /// </summary>
        /// <param name="square">the square
        /// </param>
        /// <returns> the atkTo mask
        /// </returns>
        internal BitBoard getAttackTo(int square)
        {
            return attackTo[square];
        }
        
        /// <summary> Check for a draw by detecting repeated positions.
        /// 
        /// </summary>
        /// <param name="count">number of occurences of the current position
        /// </param>
        /// <returns> <code>true</code> if at least <code>count</code> occurences
        /// of the current position where detected
        /// </returns>
        private bool checkDraw(int count)
        {
            int reps = count;
            for (int p = player - 1; p >= 0; p--)
            {
                History h = (History) history[p];
                if ((h.move & (Move.CAPTURE | Move.CASTLE | Move.PROMOTION)) != 0 || h.pawnHash != this.pawnHash)
                {
                    return false;
                }
                if (h.posHash == positionHash)
                {
                    reps--;
                    if (reps == 0)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        /// <summary> Check if a move is legal in the current position.
        /// 
        /// </summary>
        /// <param name="move">the move to check.
        /// </param>
        /// <returns> <code>true</code> if <code>move</code> is a legal move,
        /// <code>false</code> otherwise.
        /// </returns>
        public virtual bool isLegalMove(int move)
        {
            IntVector tmpMoves = new IntVector();
            generateLegalMoves(tmpMoves);
            return tmpMoves.contains(move);
        }
        
        /// <summary> Test for a direct check.
        /// 
        /// </summary>
        /// <param name="everPossibleMoves">the ever possible moves bitboard
        /// </param>
        /// <param name="to">the to square
        /// </param>
        /// <param name="opponentKing">the opponents king position
        /// </param>
        /// <returns> <code>true</code> if direct check
        /// </returns>
        private bool testDirectCheck(BitBoard[] everPossibleMoves, int to, int opponentKing)
        {
            if ((everPossibleMoves[opponentKing][to]) != 0)
            {
                /* BUGBUG wait till we fix Geometry to fix this
                    return (Geometry.INTER_PATH[to][opponentKing] & (pieceMask[0][0] | pieceMask[1][0])) == 0L;
                */
                return true;
            }
            return false;
        }

        // BGUBUG this is a stub until Geometry is upgraded to use BitBoard
        private bool testDirectCheck(long[] everPossibleMoves, int to, int opponentKing)
        {
            return false;
        }

        /// <summary> Check if a move gives check.
        /// 
        /// </summary>
        /// <param name="move">the move
        /// </param>
        /// <returns> a boolean indicating if the move is a checking move
        /// </returns>
        internal virtual bool isCheckingMove(int move)
        {
            int from = Move.getFrom(move);
            int to = Move.getTo(move);
            int oppKing = kingPos[whiteToMove?1:0];
            
            switch (board[from])
            {
                
                case ChessConstants_Fields.PAWN: 
                    if ((Geometry.BLACK_PAWN_EPM[oppKing].GetBit(to)) != 0L)
                    {
                        return true;
                    }
                    break;
                
                case - ChessConstants_Fields.PAWN: 
                    if ((Geometry.WHITE_PAWN_EPM[oppKing].GetBit(to)) != 0L)
                    {
                        return true;
                    }
                    break;
                
                case ChessConstants_Fields.KNIGHT: 
                case - ChessConstants_Fields.KNIGHT: 
                    if ((Geometry.KNIGHT_EPM[oppKing].GetBit(to)) != 0L)
                    {
                        return true;
                    }
                    break;
                
                case ChessConstants_Fields.BISHOP: 
                case - ChessConstants_Fields.BISHOP: 
                    if (testDirectCheck(Geometry.BISHOP_EPM, to, oppKing))
                    {
                        return true;
                    }
                    break;
                
                case ChessConstants_Fields.ROOK: 
                case - ChessConstants_Fields.ROOK: 
                    if (testDirectCheck(Geometry.ROOK_EPM, to, oppKing))
                    {
                        return true;
                    }
                    break;
                
                case ChessConstants_Fields.QUEEN: 
                case - ChessConstants_Fields.QUEEN: 
                    if (testDirectCheck(Geometry.QUEEN_EPM, to, oppKing))
                    {
                        return true;
                    }
                    break;
                }
            
            if ((Geometry.ROOK_EPM[oppKing].GetBit(from)) != 0L)
            {
                BitBoard rookOrQueen = Geometry.RAY[oppKing][from] & (getMask(whiteToMove, ChessConstants_Fields.ROOK) | getMask(whiteToMove, ChessConstants_Fields.QUEEN));
                while (rookOrQueen.IsEmpty() == false)
                {
                    int idx = rookOrQueen.findFirstOne();
                    BitBoard tmp = (pieceMask[0][0] | pieceMask[1][0]) & Geometry.INTER_PATH[idx][oppKing];
                    if (tmp.countBits() == 1)
                    {
                        return true;
                    }
                    rookOrQueen.ClearBit(idx);
                }
            }
            
            if ((Geometry.BISHOP_EPM[oppKing].GetBit(from)) != 0L)
            {
                BitBoard bishopOrQueen = Geometry.RAY[oppKing][from] & (getMask(whiteToMove, ChessConstants_Fields.BISHOP) | getMask(whiteToMove, ChessConstants_Fields.QUEEN));
                while (bishopOrQueen.IsEmpty() == false)
                {
                    int idx = bishopOrQueen.findFirstOne();
                    BitBoard tmp = (pieceMask[0][0] | pieceMask[1][0]) & Geometry.INTER_PATH[idx][oppKing];
                    if (tmp.countBits() == 1)
                    {
                        return true;
                    }
                    bishopOrQueen.ClearBit(idx);
                }
            }
            
            return false;
        }
        
        /// <summary>Material signature mask for major pieces and pawns. </summary>
        private const short MAJOR_PIECES_OR_PAWNS = (short) (0x19);
        
        /// <summary> Test wether the last move can be undone.
        /// 
        /// </summary>
        /// <returns> a boolean indicating if the last move can be undone.
        /// </returns>
        public virtual bool canUndo()
        {
            return player > 0;
        }
        
        /// <summary> Construct a board with the starting position.</summary>
        public ChessBoard():this(Position_Fields.INITIAL_POSITION)
        {
        }
        
        /// <summary>Parses EPD positions. </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'EPD_PARSER '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
        private static readonly EpdParser EPD_PARSER = new EpdParser();
        
        /// <summary> Construct a board from an EPD description.
        /// 
        /// </summary>
        /// <param name="epd">the position in EPD format.
        /// </param>
        /// <exception cref="IllegalEpdException">if the EPD is invalid.
        /// </exception>
        public ChessBoard(System.String epd):this(EPD_PARSER.parse(epd))
        {
        }
        
        /// <summary> Construct a board by creating a deep copy of an existing board.
        /// 
        /// </summary>
        /// <param name="theBoard">the board to copy
        /// </param>
        public ChessBoard(ChessBoard theBoard)
        {
            InitBlock();
            for (int i = 0; i < BitBoard.SIZE; i++)
            {
                this.attackTo[i] = new BitBoard(theBoard.attackTo[i]);
                this.attackFrom[i] = new BitBoard(theBoard.attackFrom[i]);
                this.board[i] = theBoard.board[i];
            }
            
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= ChessConstants_Fields.KING; j++)
                {
                    this.pieceMask[i][j] = new BitBoard(theBoard.pieceMask[i][j]);
                }
                this.kingPos[i] = theBoard.kingPos[i];
            }
            
            this.slidingPieces = theBoard.slidingPieces;
            
            this.enPassant = theBoard.enPassant;
            this.castle = theBoard.castle;
            this.whiteToMove = theBoard.whiteToMove;
            this.player = theBoard.player;
            this.positionHash = theBoard.positionHash;
            this.pawnHash = theBoard.pawnHash;
            
            this.materialSignature[0] = theBoard.materialSignature[0];
            this.materialSignature[1] = theBoard.materialSignature[1];
            
            this.evaluator = theBoard.evaluator;
            
            System.Collections.IEnumerator iter = theBoard.history.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (iter.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                this.history.Add(new History((History) iter.Current));
            }
        }
        
        /// <summary> Create a ChessBoard with a given position.
        /// 
        /// </summary>
        /// <param name="pos">the position.
        /// </param>
        public ChessBoard(IPosition pos)
        {
            InitBlock();
            Position = pos;
        }
        
        /// <summary> Given a whiteToMove flag, return the appropriate index.
        /// 
        /// </summary>
        /// <param name="theWtm">the flag
        /// </param>
        /// <returns> 0 if <code>theWtm</code> is <code>true</code>, 1 otherwise
        /// </returns>
        private static int Side(bool theWtm)
        {
            if (theWtm)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        static ChessBoard()
        {
            log = LogManager.GetLogger(typeof(ChessBoard));
            {
                MATERIAL_SIGNATURE_BITS = new short[ChessConstants_Fields.KING + 1];
                
                MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.PAWN] = (short) (0x01);
                MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.KNIGHT] = (short) (0x02);
                MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.BISHOP] = (short) (0x04);
                MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.ROOK] = (short) (0x08);
                MATERIAL_SIGNATURE_BITS[ChessConstants_Fields.QUEEN] = (short) (0x10);
            }
        }
    }
}