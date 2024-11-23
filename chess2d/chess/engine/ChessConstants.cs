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
* $Id: ChessConstants.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Defines several constants.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public struct ChessConstants_Fields{
		/// <summary>Constant for a pawn. </summary>
		public const int PAWN = 1;
		/// <summary>Constant for a knight. </summary>
		public const int KNIGHT = 2;
		/// <summary>Constant for a bishop. </summary>
		public const int BISHOP = 3;
		/// <summary>Constant for a rook. </summary>
		public const int ROOK = 4;
		/// <summary>Constant for a queen. </summary>
		public const int QUEEN = 5;
		/// <summary>Constant for a king. </summary>
		public const int KING = 6;
		/// <summary>Constant for a black pawn. </summary>
		public const int BLACK_PAWN = 7;
	}
	public interface ChessConstants
	{
		//UPGRADE_NOTE: Members of interface 'ChessConstants' were extracted into structure 'ChessConstants_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		
	}
}