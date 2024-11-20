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
* $Id: PositionNodeImpl.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
using System.Collections.Generic;
namespace tgreiner.amy.chess.game
{
	
	/// <summary> A position in a game.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	
	/*
	public class PositionNodeImpl : IPositionNode
	{
		/// <summary> Get the list of variations.
		/// 
		/// </summary>
		/// <returns> the variations
		/// </returns>
		virtual public IList<IMoveNode> Variations
		{
			get
			{
				return variations;
			}
			
		}
		
		/// <summary>The MoveNode. </summary>
		private IMoveNode moveNode;
		
		/// <summary>The comment. </summary>
		private System.String comment;
		
		/// <summary>The variations. </summary>
		private IList<IMoveNode> variations;
		
		/// <summary> Set the MoveNode of the principal variation.
		/// 
		/// </summary>
		/// <param name="pvNode">the pv node
		/// </param>
		public virtual void  setPV(IMoveNode pvNode)
		{
			moveNode = pvNode;
		}
		
		/// <seealso cref="IPositionNode.getPV">
		/// </seealso>
		public virtual IMoveNode getPV()
		{
			return moveNode;
		}
		
		/// <summary> Set the comment.
		/// 
		/// </summary>
		/// <param name="theComment">the comment
		/// </param>
		public virtual void  setComment(System.String theComment)
		{
			this.comment = theComment;
		}
		
		/// <seealso cref="IPositionNode.getComment">
		/// </seealso>
		public virtual System.String getComment()
		{
			return comment;
		}
		
		/// <summary> Add a variation.
		/// 
		/// </summary>
		/// <param name="variation">the variation
		/// </param>
		public virtual void  addVariation(IMoveNode variation)
		{
			if (variations == null)
			{
				variations = new List<IMoveNode>();
			}
			variations.Add(variation);
		}
	}
	*/
}