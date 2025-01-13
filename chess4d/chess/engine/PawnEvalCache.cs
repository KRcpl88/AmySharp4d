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
* $Id: PawnEvalCache.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using tgreiner.amy.bitboard;
using AbstractCache = tgreiner.amy.common.engine.AbstractCache;
namespace tgreiner.amy.chess.engine
{
	
	/// <summary> Caches pawn evaluation results.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	class PawnEvalCache:AbstractCache
	{
		/// <summary> Get the bitboard of white passed pawns.
		/// 
		/// </summary>
		/// <returns> the bitboard of white passed pawns
		/// </returns>
		virtual public BitBoard WhitePassedPawns
		{
			get
			{
				return ((Entry) probed).whitePassedPawns;
			}
			
		}
		/// <summary> Get the bitboard of black passed pawns.
		/// 
		/// </summary>
		/// <returns> the bitboard of black passed pawns
		/// </returns>
		virtual public BitBoard BlackPassedPawns
		{
			get
			{
				return ((Entry) probed).blackPassedPawns;
			}
			
		}
		
		/// <summary> An entry in the cache.</summary>
		new protected internal class Entry:AbstractCache.Entry
		{
			/// <summary>Bitboard for white passed pawns. </summary>
			public BitBoard whitePassedPawns = new BitBoard();
			
			/// <summary>Bitboard for black passed pawns. </summary>
			public BitBoard blackPassedPawns = new BitBoard();
		}
		
		/// <seealso cref="AbstractCache.createEntry">
		/// </seealso>
		protected internal override AbstractCache.Entry createEntry()
		{
			return new Entry();
		}
		
		/// <summary> Store an entry in the cache.
		/// 
		/// </summary>
		/// <param name="key">the hash key
		/// </param>
		/// <param name="value">the value
		/// </param>
		/// <param name="whitePassedPawns">bitboard for white passed pawns
		/// </param>
		/// <param name="blackPassedPawns">bitboard for black passed pawns
		/// </param>
		public virtual void  store(long key, int value_Renamed, BitBoard whitePassedPawns, BitBoard blackPassedPawns)
		{
			Entry e = (Entry) getEntry(key);
			e.key = key;
			e.value_Renamed = value_Renamed;
			e.whitePassedPawns = whitePassedPawns;
			e.blackPassedPawns = blackPassedPawns;
		}
	}
}