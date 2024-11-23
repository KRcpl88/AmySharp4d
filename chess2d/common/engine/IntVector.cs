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
* $Id: IntVector.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.engine
{
	
	/// <summary> A specialized containter for keeping <code>int</code> values. We do not want
	/// to use Vector or anything like that, since we will have a lot of overhead due
	/// to wrapping <code>int</code>'s in <code>java.lang.Integer</code> objects.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public class IntVector : MoveList
	{
		/// <summary> Set the size.
		/// 
		/// </summary>
		/// <param name="newSize">the new size
		/// </param>
		virtual public int Size
		{
			set
			{
				size_Renamed_Field = value;
			}
			
		}
		
		/// <summary>The storage. </summary>
		private int[] storage;
		
		/// <summary>The vector's size. </summary>
		private int size_Renamed_Field;
		
		/// <summary>The vector's capacity. </summary>
		private int capacity;
		
		/// <summary> Create an IntVector.</summary>
		public IntVector()
		{
			// default size is 16.
			
			size_Renamed_Field = 0;
			capacity = 16;
			storage = new int[capacity];
		}
		
		/// <summary> Get an element.
		/// 
		/// </summary>
		/// <param name="idx">the index
		/// </param>
		/// <returns> the element at <code>idx</code>
		/// </returns>
		public virtual int get_Renamed(int idx)
		{
			return storage[idx];
		}
		
		/// <summary> Set an element.
		/// 
		/// </summary>
		/// <param name="element">the element to set
		/// </param>
		/// <param name="idx">the index
		/// </param>
		public virtual void  set_Renamed(int element, int idx)
		{
			storage[idx] = element;
		}
		
		/// <summary> Add an entry.
		/// 
		/// </summary>
		/// <param name="move">the entry
		/// </param>
		public virtual void  add(int move)
		{
			if (size_Renamed_Field >= capacity)
			{
				int[] tmp = new int[2 * capacity];
				Array.Copy(storage, 0, tmp, 0, capacity);
				capacity *= 2;
				storage = tmp;
			}
			
			storage[size_Renamed_Field] = move;
			size_Renamed_Field++;
		}
		
		/// <summary> Swap two elements.
		/// 
		/// </summary>
		/// <param name="idx1">index 1
		/// </param>
		/// <param name="idx2">index 2
		/// </param>
		public virtual void  swap(int idx1, int idx2)
		{
			int tmp = storage[idx1];
			storage[idx1] = storage[idx2];
			storage[idx2] = tmp;
		}
		
		/// <summary> Get the size.
		/// 
		/// </summary>
		/// <returns> the size
		/// </returns>
		public virtual int size()
		{
			return size_Renamed_Field;
		}
		
		/// <summary> Pop the last entry.
		/// 
		/// </summary>
		/// <returns> the last entry
		/// </returns>
		public virtual int pop()
		{
			size_Renamed_Field--;
			return storage[size_Renamed_Field];
		}
		
		/// <summary> Test if this vector contains an element.
		/// 
		/// </summary>
		/// <param name="x">the element to test for.
		/// </param>
		/// <returns> <code>true</code> if this vector contains <code>x</code>
		/// </returns>
		public virtual bool contains(int x)
		{
			for (int i = size_Renamed_Field - 1; i >= 0; i--)
			{
				if (x == storage[i])
				{
					return true;
				}
			}
			return false;
		}
	}
}