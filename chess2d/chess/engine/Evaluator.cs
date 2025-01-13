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
* $Id: Evaluator.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> The interface for evaluators.
	/// 
	/// </summary>
	/// <author>  Thorsten Greiner
	/// </author>
	public interface Evaluator
	{
		/// <summary> Get the white material value.
		/// 
		/// </summary>
		/// <returns> the white material value
		/// </returns>
		int WhiteMaterial
		{
			get;
			
		}
		/// <summary> Get the black material value.
		/// 
		/// </summary>
		/// <returns> the black material value
		/// </returns>
		int BlackMaterial
		{
			get;
			
		}
		
		/// <summary> Evaluate the board.
		/// 
		/// </summary>
		/// <param name="alpha">value of alpha
		/// </param>
		/// <param name="beta">value of beta
		/// </param>
		/// <returns> the evaluation
		/// </returns>
		int evaluate(int alpha, int beta);
		
		/// <summary> Initialize this evaluator.</summary>
		void  init();
		
		/// <summary> A piece has moved.
		/// 
		/// </summary>
		/// <param name="from">the moves' from square
		/// </param>
		/// <param name="to">the moves' to square
		/// </param>
		/// <param name="type">the piece type
		/// </param>
		/// <param name="wtm">indicates wether white or black made the move
		/// </param>
		void  move(int from, int to, int type, bool wtm);
		
		/// <summary> A piece was captured.
		/// 
		/// </summary>
		/// <param name="sq">the capture square
		/// </param>
		/// <param name="type">the piece type
		/// </param>
		/// <param name="wtm">indicates wether white or black made the move
		/// </param>
		void  capture(int sq, int type, bool wtm);
		
		/// <summary> A piece was added.
		/// 
		/// </summary>
		/// <param name="sq">the capture square
		/// </param>
		/// <param name="type">the piece type
		/// </param>
		/// <param name="wtm">indicates wether white or black made the move
		/// </param>
		void  add(int sq, int type, bool wtm);
		
		/// <summary> Get the material value of a chess piece.
		/// 
		/// </summary>
		/// <param name="piece">the piece (Pawn ... King)
		/// </param>
		/// <returns> the material value.
		/// </returns>
		int getMaterialValue(int piece);
	}
}