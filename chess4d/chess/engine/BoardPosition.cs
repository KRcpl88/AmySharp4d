using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tgreiner.amy.chess.engine
{
    public class BoardPosition : IPosition
    {
        public BoardPosition(int[] board, bool whiteToMove, int enPassant, bool canWhiteCastleKingSide, bool canWhiteCastleQueenSide, bool canBlackCastleKingSide, bool canBlackCastleQueenSide, EpdParser enclosingInstance)
        {
            InitBlock(board, whiteToMove, enPassant, canWhiteCastleKingSide, canWhiteCastleQueenSide, canBlackCastleKingSide, canBlackCastleQueenSide, enclosingInstance);
        }
        private void InitBlock(int[] board, bool whiteToMove, int enPassant, bool canWhiteCastleKingSide, bool canWhiteCastleQueenSide, bool canBlackCastleKingSide, bool canBlackCastleQueenSide, EpdParser enclosingInstance)
        {
            this.board = board;
            this.whiteToMove = whiteToMove;
            this.enPassant = enPassant;
            this.canWhiteCastleKingSide = canWhiteCastleKingSide;
            this.canWhiteCastleQueenSide = canWhiteCastleQueenSide;
            this.canBlackCastleKingSide = canBlackCastleKingSide;
            this.canBlackCastleQueenSide = canBlackCastleQueenSide;
            this.enclosingInstance = enclosingInstance;
        }
        //UPGRADE_NOTE: Final variable board was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private int[] board;
        //UPGRADE_NOTE: Final variable whiteToMove was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private bool whiteToMove;
        //UPGRADE_NOTE: Final variable enPassant was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private int enPassant;
        //UPGRADE_NOTE: Final variable canWhiteCastleKingSide was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private bool canWhiteCastleKingSide;
        //UPGRADE_NOTE: Final variable canWhiteCastleQueenSide was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private bool canWhiteCastleQueenSide;
        //UPGRADE_NOTE: Final variable canBlackCastleKingSide was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private bool canBlackCastleKingSide;
        //UPGRADE_NOTE: Final variable canBlackCastleQueenSide was copied into class AnonymousClassPosition. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
        private bool canBlackCastleQueenSide;
        private EpdParser enclosingInstance;
        virtual public int[] Board
        {
            get
            {
                return board;
            }

        }
        virtual public bool Wtm
        {
            get
            {
                return whiteToMove;
            }

        }
        virtual public int EnPassantSquare
        {
            get
            {
                return enPassant;
            }

        }
        public EpdParser Enclosing_Instance
        {
            get
            {
                return enclosingInstance;
            }

        }
        public virtual bool CanWhiteCastleKingSide()
        {
            return canWhiteCastleKingSide;
        }
        public virtual bool CanWhiteCastleQueenSide()
        {
            return canWhiteCastleQueenSide;
        }
        public virtual bool CanBlackCastleKingSide()
        {
            return canBlackCastleKingSide;
        }
        public virtual bool CanBlackCastleQueenSide()
        {
            return canBlackCastleQueenSide;
        }
    }
}
