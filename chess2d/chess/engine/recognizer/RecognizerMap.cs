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
* $Id: RecognizerMap.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using ChessBoard = tgreiner.amy.chess.engine.ChessBoard;
namespace tgreiner.amy.chess.engine.recognizer
{
	
	/// <summary> Maps material signatures to Recognizer.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class RecognizerMap
	{
		private void  InitBlock()
		{
			for (int i = 0; i < 32; i++)
			{
				map[i] = new Recognizer[32];
			}
		}
		
		/// <summary>The recognizers, indexed by material signatures. </summary>
		private Recognizer[][] map = new Recognizer[32][];
		
		/// <summary> Create a RecognizerMap.</summary>
		public RecognizerMap()
		{
			InitBlock();
			init();
		}
		
		/// <summary> Calculate the index for a given signature.
		/// 
		/// </summary>
		/// <param name="p">pawn bit
		/// </param>
		/// <param name="n">knight bit
		/// </param>
		/// <param name="b">bishop bit
		/// </param>
		/// <param name="r">rook bit
		/// </param>
		/// <param name="q">queen bit
		/// </param>
		/// <returns> the index
		/// </returns>
		private static int index(int p, int n, int b, int r, int q)
		{
			return p | (n << 1) | (b << 2) | (r << 3) | (q << 4);
		}
		
		/// <summary> Initialize the RecognizerMap</summary>
		private void  init()
		{
			Recognizer kbpk = new KBPKRecognizer();
			
			//        P  N  B  R  Q         P  N  B  R  Q
			map[index(0, 0, 0, 0, 0)][index(1, 0, 1, 0, 0)] = kbpk;
			map[index(1, 0, 1, 0, 0)][index(0, 0, 0, 0, 0)] = kbpk;
			
			Recognizer kbpkp = new KBPKPRecognizer();
			//        P  N  B  R  Q         P  N  B  R  Q
			map[index(1, 0, 0, 0, 0)][index(1, 0, 1, 0, 0)] = kbpkp;
			map[index(1, 0, 1, 0, 0)][index(1, 0, 0, 0, 0)] = kbpkp;
		}
		
		/// <summary> Get the recognizer for a given position.
		/// 
		/// </summary>
		/// <param name="board">the board
		/// </param>
		/// <returns> the recognizer or <code>null</code>
		/// </returns>
		public virtual Recognizer getRecognizer(ChessBoard board)
		{
			return map[board.getMaterialSignature(true)][board.getMaterialSignature(false)];
		}
	}
}