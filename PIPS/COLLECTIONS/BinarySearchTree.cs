///<summary>
///A red-black tree must satisfy these properties:
///
///1. The root is black. 
///2. All leaves are black. 
///3. Red nodes can only have black children. 
///4. All paths from a node to its leaves contain the same number of black nodes.
///</summary>

using System.Collections;
using System.Text;
using System;
using System.Reflection;

namespace PIPS.Collections
{
	public class BinarySearchTree : IEnumerable
	{
		// the number of nodes contained in the tree
		private int count;
		// the tree
		private BinarySearchNode tree;      
		public event EventHandler ItemInserted, ItemDeleted;

		public BinarySearchTree() 
        {
            tree = BinarySearchNode.Sentinel;
        }
		
		protected BinarySearchNode Root {
			get {
				return tree;
			}
			set {
				tree = value;
			}
		}
		public void OutputStructure(System.IO.StreamWriter sw) {
			this.OutputStructure(sw, this.tree, 0);
		}
		private void OutputStructure(System.IO.StreamWriter sw, BinarySearchNode node, int depth) {
			if(!node.IsSentinel) {
				sw.WriteLine("{0}: {1}", depth, node.Data);
				this.OutputStructure(sw, node.Left, depth + 1);
				this.OutputStructure(sw, node.Right, depth + 1);
			}
		}
		private BinarySearchNode FindInternal(IComparable data) {
			if(!tree.IsSentinel) {
				BinarySearchNode temp = tree;
				while(true) {
					int result = data.CompareTo(temp.Key);
					if(result == 0) {
						break;
					} else if(result > 0) {
						if(temp.Right.IsSentinel)
							break;
						temp = temp.Right;
					} else {
						if(temp.Left.IsSentinel)
							break;
						temp = temp.Left;
					}
				}
				return temp;
			}
			return null;
		}

		public void Add(IComparable key, object data) {
			this.SetData(key, data, false);
		}

		protected virtual BinarySearchNode CreateBinarySearchNode() {
			return new BinarySearchNode();
		}

		private void SetData(IComparable key, object data, bool overwrite) {
			BinarySearchNode node = this.CreateBinarySearchNode();
			node.Key = key;
			node.Data = data;
			node.Parent = this.FindInternal(node.Key);
			if(node.Parent != null) {
				int result = node.Key.CompareTo(node.Parent.Key);
				if(result == 0) {
					if(overwrite) {
						node.Parent.Data = node.Data;
						return;
					}
					throw new BinarySearchException("Key already exists in BinarySearchTree!");
				} else if(result > 0) {
					node.Parent.Right = node;
				} else {
					node.Parent.Left = node;
				}
			} else {
				tree = node;	
			}
			count++;
			this.OnItemInserted(node);
		}

		protected virtual void OnItemInserted(BinarySearchNode node) {
			if(this.ItemInserted != null)
				this.ItemInserted(this, EventArgs.Empty);
		}

		///<summary>
		/// Find
		/// Gets the data object associated with the specified key
		///<summary>
		public object Find(IComparable key) {
			BinarySearchNode node = this.FindInternal(key);
			if(node != null) {
				int result = node.Key.CompareTo(key);
				if(result == 0)
					return node.Data;
			}
			return null;
		}

		public object this[IComparable key] {
			get {
				return this.Find(key);
			}
			set {
				this.SetData(key, value, true);
			}
		}

		public bool Contains(IComparable key) {
			BinarySearchNode node = this.FindInternal(key);
			if(node != null)
				return (node.Key.CompareTo(key) == 0);
			return false;
		}

		protected BinarySearchNode MinimumNode
		{
			get {
				if(!this.IsEmpty) {
					BinarySearchNode treeNode = tree;
					while(!treeNode.Left.IsSentinel)
						treeNode = treeNode.Left;
					return treeNode;
				}
				return null;
			}
		}

		protected BinarySearchNode MaximumNode
		{
			get {
				if(!this.IsEmpty) {
					BinarySearchNode treeNode = tree;
					while(!treeNode.Right.IsSentinel)
						treeNode = treeNode.Right;
					return treeNode;
				}
				return null;
			}
		}
		///<summary>
		/// GetMinValue
		/// Returns the object having the minimum key value
		///<summary>
		public object MinimumValue
		{
			get {
				return this.MinimumNode.Data;
			}
		}
		///<summary>
		/// GetMaxValue
		/// Returns the object having the maximum key
		///<summary>
		public object MaximumValue {
			get {
				return this.MinimumNode.Data;
			}
		}
		
		///<summary>
		/// IsEmpty
		/// Is the tree empty?
		///<summary>
		public bool IsEmpty
		{
			get {
				return ((tree == null) || (tree.IsSentinel));
			}
		}
		///<summary>
		/// Remove
		/// removes the key and data object (delete)
		///<summary>
		public void Remove(IComparable key)
		{
			//if(key != null) {
				// find node
				BinarySearchNode node = this.FindInternal(key);
				if(node != null) {
					int result = key.CompareTo(node.Key);
					if(result == 0) {
						Delete(node);
					}
				}

			//}
		}
		///<summary>
		/// Delete
		/// Delete a node from the tree and restore red black properties
		///<summary>
		private void Delete(BinarySearchNode delete)
		{
			if(delete.Left.IsSentinel && delete.Right.IsSentinel) {
				//zero children
				if(delete.IsLeftChild)
					delete.Parent.Left = BinarySearchNode.Sentinel;
				else if(delete.IsRightChild)
					delete.Parent.Right = BinarySearchNode.Sentinel;
				else
					this.Root = BinarySearchNode.Sentinel;
				count--;
				this.OnItemDeleted(delete, BinarySearchNode.Sentinel);
			} else if(delete.Left.IsSentinel) {
				//one children
				if(delete.IsLeftChild) {
					delete.Parent.Left = delete.Right;
					delete.Right.Parent = delete.Parent;
				} else if(delete.IsRightChild) {
					delete.Parent.Right = delete.Right;
					delete.Right.Parent = delete.Parent;
				} else {
					this.Root = delete.Right;
					delete.Right.Parent = null;
				}
				count--;
				this.OnItemDeleted(delete, delete.Right);
			} else if(delete.Right.IsSentinel) {
				//one children
				if(delete.IsLeftChild) {
					delete.Parent.Left = delete.Left;
					delete.Left.Parent = delete.Parent;
				} else if(delete.IsRightChild) {
					delete.Parent.Right = delete.Left;
					delete.Left.Parent = delete.Parent;
				} else {
					this.Root = delete.Left;
					delete.Left.Parent = null;
				}
				count--;
				this.OnItemDeleted(delete, delete.Left);
			} else {
				//two children
				BinarySearchNode replace = delete.Right;
				while(!replace.Left.IsSentinel)
					replace = replace.Left;
				delete.Key = replace.Key;
				delete.Data = replace.Data;
				this.Delete(replace);
			}
		}

		protected virtual void OnItemDeleted(BinarySearchNode deleted, BinarySearchNode location) {
			if(this.ItemDeleted != null)
				this.ItemDeleted(this, EventArgs.Empty);
		}
		
		///<summary>
		/// Clear
		/// Empties or clears the tree
		///<summary>
		public void Clear ()
		{
			tree = BinarySearchNode.Sentinel;
			count = 0;
		}
		///<summary>
		/// Size
		/// returns the size (number of nodes) in the tree
		///<summary>
		public int Count
		{
			get {
				// number of keys
				return count;
			}
		}

		#region IEnumerable Members

		private class BinaryAscendingEnumerator : IEnumerator {
			private BinarySearchTree tree;
			private BinarySearchNode current;

			public BinaryAscendingEnumerator(BinarySearchTree tree) {
				this.tree = tree;
				this.current = BinarySearchNode.Sentinel;
			}

			#region IEnumerator Members

			public void Reset() {
				this.current = BinarySearchNode.Sentinel;
			}

			public object Current {
				get {
					if(this.current.IsSentinel)
						throw new BinarySearchException("Enumerator not initialized!");
					return current.Data;
				}
			}

			public bool MoveNext() {
				if(this.current != null) {
					this.current = this.GetNextNode(this.current);
				}
				return this.current != null;
			}

			private BinarySearchNode FindMinimum(BinarySearchNode token) {
				while(!token.Left.IsSentinel)
					token = token.Left;
				return token;
			}

			private BinarySearchNode FindRoot(BinarySearchNode token) {
				while(token.IsRightChild)
					token = token.Parent;
				return token;
			}

			private BinarySearchNode GetNextNode(BinarySearchNode token) {
				if(token.IsSentinel) {
					return this.tree.MinimumNode;
				} else if(token.IsLeftChild) {
					if(token.Right.IsSentinel)
						return token.Parent;
					return this.FindMinimum(token.Right);
				} else if(token.IsRightChild) {
					if(token.Right.IsSentinel)
						return this.FindRoot(token).Parent;
					return this.FindMinimum(token.Right);
				} else if(!token.Right.IsSentinel) {
						return this.FindMinimum(token.Right);
				}
				return null;
			}

			#endregion

		}
		public IEnumerator GetEnumerator() {
			return new BinaryAscendingEnumerator(this);
		}

		#endregion
	}
}
