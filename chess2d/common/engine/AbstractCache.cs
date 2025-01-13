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
* $Id: AbstractCache.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.engine
{
	
	/// <summary> Caches evaluation results.
	/// 
	/// </summary>
	/// <author>  <a href="mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public abstract class AbstractCache
	{
		/// <summary> Get the value for the last probe.
		/// 
		/// </summary>
		/// <returns> the value
		/// </returns>
		virtual public int Value
		{
			get
			{
				return probed.value_Renamed;
			}
			
		}
		
		/// <summary> An Entry into the cache.</summary>
		protected internal class Entry
		{
			/// <summary>The hash key. </summary>
			public long key;
			
			/// <summary>The value. </summary>
			public int value_Renamed;
		}
		
		
		/// <summary>The cache entries. </summary>
		private Entry[] entries;
		
		/// <summary>Mask to convert hash keys to entry indices. </summary>
		private int mask;
		
		/// <summary> Create an AbstractCache.</summary>
		public AbstractCache()
		{
			
			int size = (1 << 15);
			
			entries = new Entry[size];
			mask = size - 1;
		}
		
		/// <summary>The last entry probed. </summary>
		protected internal Entry probed;
		
		/// <summary> Probe the cache.
		/// 
		/// </summary>
		/// <param name="key">the key
		/// </param>
		/// <returns> a boolean indicating wether the probe was successful
		/// </returns>
		public virtual bool probe(long key)
		{
			int index = (int) (key & mask);
			
			probed = entries[index];
			
			return (probed != null) && (probed.key == key);
		}
		
		/// <summary> Create an Entry for the cache. Subclasses must override this method
		/// to customize the entries.
		/// 
		/// </summary>
		/// <returns> an Entry.
		/// </returns>
		protected internal abstract Entry createEntry();
		
		/// <summary> Get an entry.
		/// 
		/// </summary>
		/// <param name="key">the key
		/// </param>
		/// <returns> the entry for <code>key</code>
		/// </returns>
		protected internal virtual Entry getEntry(long key)
		{
			int index = (int) (key & mask);
			
			if (entries[index] == null)
			{
				entries[index] = createEntry();
			}
			return entries[index];
		}
	}
}