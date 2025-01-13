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
* $Id: NodeType.java 2 2007-08-09 07:05:44Z tetchu $
*/
using System;
namespace tgreiner.amy.common.engine
{
	
	/// <summary> NodeType models the three possible types of nodes encountered during a
	/// tree search: pv node, cut node and all node.
	/// 
	/// </summary>
	/// <author>  <a href = "mailto:thorsten.greiner@googlemail.com">Thorsten Greiner</a>
	/// </author>
	public sealed class NodeType
	{
		
		/// <summary>The PV node type. </summary>
		public static NodeType PV;
		
		/// <summary>The CUT node type. </summary>
		public static NodeType CUT;
		
		/// <summary>The ALL node type. </summary>
		public static NodeType ALL;
		
		/// <summary>The node type of sibling nodes. </summary>
		private NodeType siblingType;
		
		/// <summary> This class may not be instantiated from outside.</summary>
		private NodeType()
		{
		}
		
		/// <summary> Set the sibling type.
		/// 
		/// </summary>
		/// <param name="theSiblingType">the sibling type.
		/// </param>
		private void  setSiblingType(NodeType theSiblingType)
		{
			this.siblingType = theSiblingType;
		}
		
		/// <summary> Get the sibling type.
		/// 
		/// </summary>
		/// <returns> the sibling type.
		/// </returns>
		public NodeType getSiblingType()
		{
			return siblingType;
		}
		static NodeType()
		{
			{
				PV = new NodeType();
				CUT = new NodeType();
				ALL = new NodeType();
				
				PV.setSiblingType(PV);
				CUT.setSiblingType(ALL);
				ALL.setSiblingType(CUT);
			}
		}
	}
}