using System;

namespace PIPS.Collections
{
	/// <summary>
	/// Summary description for BinarySearchTreeNode.
	/// </summary>
	public class BinarySearchNode
	{
		private static BinarySearchNode sentinel;
		static BinarySearchNode() {
			sentinel = new BinarySearchNode(true);
		}
		public static BinarySearchNode Sentinel {
			get {
				return sentinel;
			}
		}

		// key provided by the calling class
		private IComparable ordKey;
		// the data or value associated with the key
		private object objData;
		// left node 
		private BinarySearchNode rbnLeft;
		// right node 
		private BinarySearchNode rbnRight;
		// parent node 
		private BinarySearchNode rbnParent;
		
		public bool IsSentinel {
			get {
				return (this == Sentinel);
			}
		}

		///<summary>
		///Key
		///</summary>
		public IComparable Key {
			get {
				return ordKey;
			}
			set {
				if(this.IsSentinel)
					throw new BinarySearchException("Cannot set key for sentinel node!");
				ordKey = value;
			}
		}
		///<summary>
		///Data
		///</summary>
		public object Data {
			get {
				return objData;
			}
			set {
				if(this.IsSentinel)
					throw new BinarySearchException("Cannot set data for sentinel node!");
				objData = value;
			}
		}

		///<summary>
		///Left
		///</summary>
		public BinarySearchNode Left {
			get {
				return rbnLeft;
			}
			set {
				if(this.IsSentinel)
					throw new BinarySearchException("Cannot set links for sentinel node!");
				rbnLeft = value;
			}
		}

		public BinarySearchNode Sibling {
			get {
				if(this.IsLeftChild)
					return this.Parent.Right;
				else if(this.IsRightChild)
					return this.Parent.Left;
				return null;
			}
		}

		/*public BinarySearchNode NearNephew {
			get {
				if(this.IsLeftChild)
					return this.Sibling.Left;
				else if(this.IsRightChild)
					return this.Sibling.Right;
				return null;
			}
		}

		public BinarySearchNode FarNephew {
			get {
				if(this.IsLeftChild)
					return this.Sibling.Right;
				else if(this.IsRightChild)
					return this.Sibling.Left;
				return null;
			}
		}*/

		public bool IsLeftChild {
			get {
				if(this.Parent != null) {
					return (this == this.Parent.Left);
				}
				return false;
			}
		}

		public bool IsRightChild {
			get {
				if(this.Parent != null) {
					return (this == this.Parent.Right);
				}
				return false;
			}
		}

		///<summary>
		/// Right
		///</summary>
		public BinarySearchNode Right {
			get {
				return rbnRight;
			}
			set {
				if(this.IsSentinel)
					throw new BinarySearchException("Cannot set links for sentinel node!");
				rbnRight = value;
			}
		}
		public BinarySearchNode Parent {
			get {
				return rbnParent;
			}
			set {
				if(this.IsSentinel)
					throw new BinarySearchException("Cannot set links for sentinel node!");
				rbnParent = value;
			}
		}

		private BinarySearchNode(bool isSentinel) {
			if(isSentinel) {
				this.rbnLeft = this.rbnParent = this.rbnRight = null;
				this.objData = null;
				this.ordKey = null;
			} else {
				this.rbnLeft = this.rbnRight = BinarySearchNode.Sentinel;
				this.rbnParent = null;
				this.objData = null;
				this.ordKey = null;
			}
		}

		public BinarySearchNode() : this(false) {}
	}
}
