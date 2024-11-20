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
* $Id: EvaluatorImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using BitBoard = tgreiner.amy.bitboard.BitBoard;
using EvalCache = tgreiner.amy.common.engine.EvalCache;
//UPGRADE_TODO: The type 'org.apache.log4j.Logger' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
using AmySharp.chess.engine.logger;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> The board evaluator.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class EvaluatorImpl : IEvaluator //, ChessConstants
	{
		private void  InitBlock()
		{
			log = LogManager.GetLogger(typeof(EvaluatorImpl));
			pieceValues = new int[]{0, pawnValue, knightValue, bishopValue, rookValue, queenValue};
		}
		/// <summary> Get the white material.
		/// 
		/// </summary>
		/// <returns> the material value.
		/// </returns>
		virtual public int WhiteMaterial
		{
			get
			{
				return whiteMaterial;
			}
			
		}
		/// <summary> Get the black material.
		/// 
		/// </summary>
		/// <returns> the material value.
		/// </returns>
		virtual public int BlackMaterial
		{
			get
			{
				return blackMaterial;
			}
			
		}
		/// <summary> Get the positional score. This is based on piece/square tables only
		/// 
		/// </summary>
		/// <returns> the positional score.
		/// </returns>
		virtual public int PosScore
		{
			get
			{
				return posScore;
			}
			
		}
		/// <summary>The log. </summary>
		//UPGRADE_NOTE: The initialization of  'log' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ILog log;
		
		/// <summary>The ChessBoard we evaluate. </summary>
		private ChessBoard board;
		
		/// <summary>Identifies outside passed pawns. </summary>
		private OutsidePassedPawnIdentifier outsidePassedPawnId = new OutsidePassedPawnIdentifier();
		
		/// <summary>Material value of a pawn. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'pawnValue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int pawnValue = 100;
		
		/// <summary>Material value of a knight. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'knightValue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int knightValue = 325;
		
		/// <summary>Material value of a bishop. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'bishopValue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int bishopValue = 325;
		
		/// <summary>Material value of a rook. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'rookValue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int rookValue = 500;
		
		/// <summary>Material value of a queen. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'queenValue '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int queenValue = 975;
		
		/// <summary>The piece values, indexed by piece type. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'pieceValues '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'pieceValues' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private int[] pieceValues;
		
		/// <summary>White's material. </summary>
		private int whiteMaterial;
		
		/// <summary>Black's material. </summary>
		private int blackMaterial;
		
		/// <summary>The positional score. </summary>
		private int posScore;
		
		/// <summary>The game phase. </summary>
		private GamePhase gamePhase;
		
		/// <summary>The evaluation cache. </summary>
		private EvalCache cache = new EvalCache();
		
		/// <summary>The pawn evaluation cache. </summary>
		private PawnEvalCache pawnCache = new PawnEvalCache();
		
		/// <summary>The maximum positional score. </summary>
		private int maxPos = 150;
		
		/// <summary>A null piece/square table. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'ZERO_POS'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly int[] ZERO_POS = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		
		/// <summary>Piece/square table for pawns. </summary>
		internal static int[] pawnPos = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, - 9, - 9, 9, 13, 13, 6, 6, 6, 0, 0, 6, 9, 9, 0, 0, 13, 19, 19, 13, 0, 0, 0, 0, 16, 22, 22, 16, 0, 0, 0, 0, 19, 25, 25, 19, 0, 0, 16, 19, 22, 28, 28, 22, 19, 16, 0, 0, 0, 0, 0, 0, 0, 0};
		
		/// <summary>Piece/square table for knights. </summary>
		internal static int[] knightPos = new int[]{- 16, - 16, - 16, - 16, - 16, - 16, - 16, - 16, - 16, - 3, 6, 6, 6, 6, - 3, - 16, - 16, 6, 13, 13, 13, 13, 6, - 16, - 16, 13, 19, 19, 19, 19, 13, - 16, - 13, 13, 19, 25, 25, 19, 13, - 13, - 13, 19, 25, 25, 25, 25, 19, - 13, - 13, 9, 16, 16, 16, 16, 9, - 13, - 13, - 13, - 13, - 13, - 13, - 13, - 13, - 13};
		
		/// <summary>Piece/square table for bishops. </summary>
		internal static int[] bishopPos = new int[]{6, 6, 6, 6, 6, 6, 6, 6, 6, 25, 6, 6, 6, 6, 25, 6, 6, 16, 16, 16, 16, 16, 16, 6, 16, 25, 28, 34, 34, 28, 25, 16, 16, 25, 28, 34, 34, 28, 25, 16, 16, 25, 28, 28, 28, 28, 25, 16, 16, 25, 25, 25, 25, 25, 25, 16, 16, 16, 16, 16, 16, 16, 16, 16};
		
		/// <summary>Piece/square table for rooks. </summary>
		internal static int[] rookPos = new int[]{0, 9, 13, 22, 22, 13, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 13, 13, 13, 13, 13, 13, 13, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20};
		
		/// <summary>Piece/square table for king during opening. </summary>
		internal static int[] kingPosOpening = new int[]{0, 22, 9, - 9, - 9, 9, 28, 16, - 9, - 9, - 9, - 9, - 9, - 9, - 9, - 9, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 22, - 28, - 28, - 28, - 28, - 28, - 28, - 28, - 28, - 34, - 34, - 34, - 34, - 34, - 34, - 34, - 34, - 41, - 41, - 41, - 41, - 41, - 41, - 41, - 41};
		
		/// <summary>Piece/square table for king during endgame. </summary>
		internal static int[] kingPosEndgame = new int[]{- 10, - 10, - 10, - 10, - 10, - 10, - 10, - 10, - 10, 0, 0, 0, 0, 0, 0, - 10, - 10, 0, 10, 10, 10, 10, 0, - 10, - 10, 0, 10, 25, 25, 10, 0, - 10, - 10, 0, 10, 25, 25, 10, 0, - 10, - 10, 0, 10, 10, 10, 10, 0, - 10, - 10, 0, 0, 0, 0, 0, 0, - 10, - 10, - 10, - 10, - 10, - 10, - 10, - 10, - 10};
		
		/// <summary>Piece/square table for queens. </summary>
		internal static int[] queenPos = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 6, 6, 3, 3, 0, 0, - 10, 6, 6, 6, 6, 3, 0, - 10, 3, 6, 9, 9, 6, 3, 0, 0, 3, 6, 9, 9, 6, 3, 0, 0, 3, 6, 6, 6, 6, 3, 0, 0, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		
		/// <summary>The piece/square table.  </summary>
		internal int[][] pieceSquare = new int[][]{null, pawnPos, knightPos, bishopPos, rookPos, queenPos, null};
		
		/// <summary>Multiplier for bishop mobility. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'bishopMobility '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int bishopMobility = 3;
		
		/// <summary>Bonus for bishop pair. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'bishopPair '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int bishopPair = 40;
		
		/// <summary>Multiplier for rook mobility. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'rookMobility '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int rookMobility = 1;
		
		/// <summary>Bonus for rook on open file. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'rookOnOpenFile '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int rookOnOpenFile = 10;
		
		/// <summary>Bonus for rook on semi-open file. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'rookOnSemiOpenFile '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int rookOnSemiOpenFile = 3;
		
		/// <summary>Penalty for isolated pawn, indexed by file. </summary>
		private int[] isolatedPawn = new int[]{- 7, - 8, - 9, - 10, - 10, - 9, - 8, - 7};
		
		/// <summary>Penalty for backward pawn. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'backwardPawn '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int backwardPawn = - 10;
		
		/// <summary>Penalty for doubled pawn. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'doubledPawn '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int doubledPawn = - 7;
		
		/// <summary>Bonus for outside passed pawn. </summary>
		//UPGRADE_NOTE: Final was removed from the declaration of 'outsidePassedPawn '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private int outsidePassedPawn = 150;
		
		/// <summary> Create an EvaluatorImpl.
		/// 
		/// </summary>
		/// <param name="theBoard">the board.
		/// </param>
		public EvaluatorImpl(ChessBoard theBoard)
		{
			InitBlock();
			this.board = theBoard;
		}
		
		/// <summary> Initialize this evaluator.</summary>
		public virtual void  init()
		{
			whiteMaterial = pawnValue * BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)) + knightValue * BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT)) + bishopValue * BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP)) + rookValue * BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK)) + queenValue * BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN));
			blackMaterial = pawnValue * BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN)) + knightValue * BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT)) + bishopValue * BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP)) + rookValue * BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK)) + queenValue * BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN));
			
			posScore = 0;
			
			determineGamePhase();
			
			if (log.IsDebugEnabled)
			{
				log.Debug("Game phase is " + gamePhase);
			}
			
			if (gamePhase == GamePhase.OPENING || gamePhase == GamePhase.MIDDLEGAME)
			{
				pieceSquare[tgreiner.amy.chess.engine.ChessConstants_Fields.KING] = kingPosOpening;
			}
			else
			{
				pieceSquare[tgreiner.amy.chess.engine.ChessConstants_Fields.KING] = kingPosEndgame;
			}
			
			for (int type = tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN; type <= tgreiner.amy.chess.engine.ChessConstants_Fields.KING; type++)
			{
				long all = board.getMask(true, type);
				while (all != 0L)
				{
					int sq = BitBoard.findFirstOne(all);
					all &= BitBoard.CLEAR_MASK[sq];
					posScore += pieceSquare[type][sq];
				}
				all = board.getMask(false, type);
				while (all != 0L)
				{
					int sq = BitBoard.findFirstOne(all);
					all &= BitBoard.CLEAR_MASK[sq];
					posScore -= pieceSquare[type][Geometry.flipX(sq)];
				}
			}
		}
		
		/// <seealso cref="IEvaluator.move">
		/// </seealso>
		public virtual void  move(int from, int to, int type, bool wtm)
		{
			int[] pq = pieceSquare[type];
			
			if (wtm)
			{
				posScore += pq[to] - pq[from];
			}
			else
			{
				posScore -= (pq[Geometry.flipX(to)] - pq[Geometry.flipX(from)]);
			}
		}
		
		/// <seealso cref="IEvaluator.capture">
		/// </seealso>
		public virtual void  capture(int sq, int type, bool wtm)
		{
			if (wtm)
			{
				whiteMaterial -= pieceValues[type];
				posScore -= pieceSquare[type][sq];
			}
			else
			{
				blackMaterial -= pieceValues[type];
				posScore += pieceSquare[type][Geometry.flipX(sq)];
			}
		}
		
		/// <seealso cref="IEvaluator.add">
		/// </seealso>
		public virtual void  add(int sq, int type, bool wtm)
		{
			if (wtm)
			{
				whiteMaterial += pieceValues[type];
				posScore += pieceSquare[type][sq];
			}
			else
			{
				blackMaterial += pieceValues[type];
				posScore -= pieceSquare[type][Geometry.flipX(sq)];
			}
		}
		
		/// <summary> Evaluate the current position.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <param name="beta">the beta value
		/// </param>
		/// <returns> the positional score.
		/// </returns>
		public virtual int evaluate(int alpha, int beta)
		{
			if (board.Wtm)
			{
				return evaluateInternal(alpha, beta);
			}
			else
			{
				return - evaluateInternal(- beta, - alpha);
			}
		}
		
		/// <summary> Evaluate the current position from white's point of view.
		/// 
		/// </summary>
		/// <param name="alpha">the alpha value
		/// </param>
		/// <param name="beta">the beta value
		/// </param>
		/// <returns> the positional score.
		/// </returns>
		private int evaluateInternal(int alpha, int beta)
		{
			
			if (cache.probe(board.PosHash))
			{
				return cache.Value;
			}
			
			int score = whiteMaterial - blackMaterial;
			
			/*
			int tmp = posScore;
			init();
			if (score != (whiteMaterial - blackMaterial)
			|| tmp != posScore) {
			System.err.println("score = " + tmp);
			System.err.println("score2 = " + posScore);
			board.debug();
			throw new RuntimeException("evaluator problem");
			}
			*/
			
			score += posScore;
			
			if ((score + maxPos) < alpha)
			{
				return score + maxPos;
			}
			else if ((score - maxPos) > beta)
			{
				return score - maxPos;
			}
			
			int fastScore = score;
			
			score += evalPawns();
			score += evalPassedPawns();
			score += evalKnights();
			score += evalBishops();
			score += evalRooks();
			score += evalQueens();
			score += evalMaterialImbalance();
			
			if (oppositeColoredBishopsAndPawns())
			{
				score = (3 * score) / 4;
			}
			
			if (System.Math.Abs(score - fastScore) > maxPos)
			{
				maxPos = System.Math.Abs(score - fastScore);
				if (log.IsDebugEnabled)
				{
					log.Debug("Increasing maxPos to " + maxPos);
				}
			}
			
			cache.store(board.PosHash, score);
			
			return score;
		}
		
		/// <summary>BitBoard for the white passed pawns, set by evalPawns(). </summary>
		private long whitePassedPawns;
		
		/// <summary>BitBoard for the black passed pawns, set by evalPawns(). </summary>
		private long blackPassedPawns;
		
		/// <summary> Evaluate the pawn structure.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalPawns()
		{
			
			if (pawnCache.probe(board.PawnHash))
			{
				whitePassedPawns = pawnCache.WhitePassedPawns;
				blackPassedPawns = pawnCache.BlackPassedPawns;
				return pawnCache.Value;
			}
			
			int score = 0;
			
			long wpawns = board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
			long bpawns = board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
			long mask;
			
			whitePassedPawns = blackPassedPawns = 0L;
			
			mask = wpawns;
			while (mask != 0)
			{
				int sq = BitBoard.findFirstOne(mask);
				mask &= BitBoard.CLEAR_MASK[sq];
				
				if ((wpawns & EvalMasks.ISOLATED[sq]) == 0L)
				{
					score += isolatedPawn[sq & 7];
				}
				else if ((wpawns & EvalMasks.WHITE_BACKWARD[sq]) == 0L)
				{
					score += backwardPawn;
				}
				
				if ((wpawns & EvalMasks.WHITE_DOUBLED[sq]) != 0L)
				{
					score += doubledPawn;
				}
				
				if ((bpawns & EvalMasks.WHITE_PASSED[sq]) == 0L)
				{
					whitePassedPawns |= BitBoard.SET_MASK[sq];
				}
			}
			
			mask = bpawns;
			while (mask != 0)
			{
				int sq = BitBoard.findFirstOne(mask);
				mask &= BitBoard.CLEAR_MASK[sq];
				
				if ((bpawns & EvalMasks.ISOLATED[sq]) == 0L)
				{
					score -= isolatedPawn[sq & 7];
				}
				else if ((bpawns & EvalMasks.BLACK_BACKWARD[sq]) == 0L)
				{
					score -= backwardPawn;
				}
				
				if ((bpawns & EvalMasks.BLACK_DOUBLED[sq]) != 0L)
				{
					score -= doubledPawn;
				}
				
				if ((wpawns & EvalMasks.BLACK_PASSED[sq]) == 0L)
				{
					blackPassedPawns |= BitBoard.SET_MASK[sq];
				}
			}
			
			pawnCache.store(board.PawnHash, score, whitePassedPawns, blackPassedPawns);
			
			return score;
		}
		
		/// <summary>Maximum scaling factor for passed pawn evaluation. </summary>
		private const int MAX_SCALE_PASSED_PAWN = 12;
		
		/// <summary> Determine the scale factor for passed pawn evaluation.
		/// 
		/// </summary>
		/// <param name="wtm">the side
		/// </param>
		/// <returns> the scale factor in the range 0..MAX_SCALE_PASSED_PAWN
		/// </returns>
		private int getScale(bool wtm)
		{
			int scale = BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP)) + 2 * BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK)) + 4 * BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN));
			
			return MAX_SCALE_PASSED_PAWN - System.Math.Min(MAX_SCALE_PASSED_PAWN, scale);
		}
		
		/// <summary> Evaluate passed pawns.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalPassedPawns()
		{
			int wscale = getScale(false);
			int bscale = getScale(true);
			
			int score = 0;
			
			long mask;
			
			mask = whitePassedPawns;
			while (mask != 0L)
			{
				int sq = BitBoard.findFirstOne(mask);
				mask &= BitBoard.CLEAR_MASK[sq];
				
				int rank = (sq >> 3) - 1;
				
				score += rank * wscale;
			}
			
			mask = blackPassedPawns;
			while (mask != 0L)
			{
				int sq = BitBoard.findFirstOne(mask);
				mask &= BitBoard.CLEAR_MASK[sq];
				
				int rank = 6 - (sq >> 3);
				
				score -= rank * bscale;
			}
			
			if (board.MaskNonPawn == 0L)
			{
				outsidePassedPawnId.probe(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN), board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN));
				
				if (outsidePassedPawnId.WhiteOutsidePassedPawns != 0L && outsidePassedPawnId.BlackOutsidePassedPawns == 0L)
				{
					
					score += outsidePassedPawn;
				}
				
				if (outsidePassedPawnId.WhiteOutsidePassedPawns == 0L && outsidePassedPawnId.BlackOutsidePassedPawns != 0L)
				{
					score -= outsidePassedPawn;
				}
			}
			
			return score;
		}
		
		/// <summary> Evaluate knights.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalKnights()
		{
			return 0;
		}
		
		/// <summary> Evaluate bishops.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalBishops()
		{
			int score = 0;
			
			long wbishops = board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP);
			long bbishops = board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP);
			
			int cnt = 0;
			
			while (wbishops != 0)
			{
				int sq = BitBoard.findFirstOne(wbishops);
				wbishops &= BitBoard.CLEAR_MASK[sq];
				
				score += bishopMobility * (BitBoard.countBits(board.getAttackTo(sq)) - 6);
				cnt++;
			}
			
			if (cnt > 1)
			{
				score += bishopPair;
			}
			
			cnt = 0;
			while (bbishops != 0)
			{
				int sq = BitBoard.findFirstOne(bbishops);
				bbishops &= BitBoard.CLEAR_MASK[sq];
				
				score -= bishopMobility * (BitBoard.countBits(board.getAttackTo(sq)) - 6);
				cnt++;
			}
			
			if (cnt > 1)
			{
				score -= bishopPair;
			}
			
			return score;
		}
		
		/// <summary> Evaluate rooks.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalRooks()
		{
			int score = 0;
			
			long wrooks = board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK);
			long brooks = board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK);
			
			long wpawns = board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
			long bpawns = board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN);
			
			while (wrooks != 0)
			{
				int sq = BitBoard.findFirstOne(wrooks);
				wrooks &= BitBoard.CLEAR_MASK[sq];
				
				score += rookMobility * (BitBoard.countBits(board.getAttackTo(sq)) - 7);
				
				long fileMask = EvalMasks.FILE_MASK[sq & 7];
				
				if ((wpawns & fileMask) == 0)
				{
					if ((bpawns & fileMask) == 0)
					{
						score += rookOnOpenFile;
					}
					else
					{
						score += rookOnSemiOpenFile;
					}
				}
			}
			
			while (brooks != 0)
			{
				int sq = BitBoard.findFirstOne(brooks);
				brooks &= BitBoard.CLEAR_MASK[sq];
				
				score -= rookMobility * (BitBoard.countBits(board.getAttackTo(sq)) - 7);
				
				long fileMask = EvalMasks.FILE_MASK[sq & 7];
				
				if ((bpawns & fileMask) == 0)
				{
					if ((wpawns & fileMask) == 0)
					{
						score -= rookOnOpenFile;
					}
					else
					{
						score -= rookOnSemiOpenFile;
					}
				}
			}
			
			return score;
		}
		
		/// <summary> Evaluate queens.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalQueens()
		{
			return 0;
		}
		
		/// <summary> Evaluate the material imbalance.
		/// 
		/// </summary>
		/// <returns> the score
		/// </returns>
		protected internal virtual int evalMaterialImbalance()
		{
			int wrookCnt = BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK));
			int brookCnt = BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK));
			
			int wknightCnt = BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT));
			int bknightCnt = BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT));
			
			int wpawnCnt = BitBoard.countBits(board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN));
			int bpawnCnt = BitBoard.countBits(board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.PAWN));
			
			return ((wknightCnt * (wpawnCnt - 5) * pawnValue) >> 4) - ((bknightCnt * (bpawnCnt - 5) * pawnValue) >> 4) + ((wrookCnt * (5 - wpawnCnt) * pawnValue) >> 3) - ((brookCnt * (5 - bpawnCnt) * pawnValue) >> 3);
			/*
			return
			(((wknightCnt * (wpawnCnt-5) - bknightCnt * (bpawnCnt-5))
			* pawnValue) >> 4)
			+ (((wrookCnt * (5-wpawnCnt) - brookCnt * (5-bpawnCnt))
			* pawnValue) >> 3);
			*/
		}
		
		/// <summary> Detects if there are opposite coloured bishops and pawns present.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if opposite coloured bishops and pawns are
		/// present
		/// </returns>
		protected internal virtual bool oppositeColoredBishopsAndPawns()
		{
			
			int matSigWhite = board.getMaterialSignature(true);
			int matSigBlack = board.getMaterialSignature(false);
			
			if (((matSigWhite | matSigBlack) == 0x05) && ((matSigWhite & 0x04) != 0) && ((matSigBlack & 0x04) != 0))
			{
				long whiteBishops = board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP);
				long blackBishops = board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP);
				
				int wwb = BitBoard.countBits(whiteBishops & EvalMasks.WHITE_SQUARES);
				int wbb = BitBoard.countBits(whiteBishops & EvalMasks.BLACK_SQUARES);
				
				int bwb = BitBoard.countBits(blackBishops & EvalMasks.WHITE_SQUARES);
				int bbb = BitBoard.countBits(blackBishops & EvalMasks.BLACK_SQUARES);
				
				return (wwb == 0 && bbb == 0) || (wbb == 0 && bwb == 0);
			}
			
			return false;
		}
		
		/// <summary> Get the material value of a chess piece.
		/// 
		/// </summary>
		/// <param name="piece">the piece (Pawn ... King)
		/// </param>
		/// <returns> the material value.
		/// </returns>
		public virtual int getMaterialValue(int piece)
		{
			return pieceValues[piece];
		}
		
		/// <summary> Determine the current game phase.</summary>
		protected internal virtual void  determineGamePhase()
		{
			GamePhase phaseW = determineGamePhase(true);
			GamePhase phaseB = determineGamePhase(false);
			
			if (phaseW == GamePhase.ENDGAME && phaseB == GamePhase.ENDGAME)
			{
				gamePhase = GamePhase.ENDGAME;
			}
			else
			{
				bool wkingCenter = (board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.KING) & EvalMasks.WHITE_KING_IN_CENTER) != 0;
				bool bkingCenter = (board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.KING) & EvalMasks.BLACK_KING_IN_CENTER) != 0;
				
				bool wDeveloped = BitBoard.countBits((board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(true, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP)) & EvalMasks.RANK_MASK[0]) == 0;
				
				bool bDeveloped = BitBoard.countBits((board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(false, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP)) & EvalMasks.RANK_MASK[7]) == 0;
				
				if (wkingCenter || bkingCenter || !wDeveloped || !bDeveloped)
				{
					gamePhase = GamePhase.OPENING;
				}
				else
				{
					gamePhase = GamePhase.MIDDLEGAME;
				}
			}
		}
		
		/// <summary> Determine the game phase for a single side.
		/// 
		/// </summary>
		/// <param name="wtm">the side to move.
		/// </param>
		/// <returns> the game phase as seen by wtm.
		/// </returns>
		private GamePhase determineGamePhase(bool wtm)
		{
			int minors = BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.KNIGHT) | board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.BISHOP));
			int majors = BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.ROOK)) + 2 * BitBoard.countBits(board.getMask(wtm, tgreiner.amy.chess.engine.ChessConstants_Fields.QUEEN));
			
			if (minors >= 2 && majors >= 2)
			{
				return GamePhase.MIDDLEGAME;
			}
			else
			{
				return GamePhase.ENDGAME;
			}
		}
	}
}