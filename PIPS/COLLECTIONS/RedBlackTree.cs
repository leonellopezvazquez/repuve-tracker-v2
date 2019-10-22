using System;

namespace PIPS.Collections
{
	/// <summary>
	/// Summary description for RedBlackTree.
	/// </summary>
	public class RedBlackTree : BinarySearchTree
	{
		public RedBlackTree()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override BinarySearchNode CreateBinarySearchNode() {
			return new RedBlackNode();
		}

		protected override void OnItemInserted(BinarySearchNode node) {
			this.OnItemInsertedInternal(node);
			base.OnItemInserted(node);
		}
		private void OnItemInsertedInternal(BinarySearchNode token) {
			if(token.Parent != null) {//case 1: the new node is the root
				if(this.IsRed(token.Parent)) {//case 2: the new nodes parent is black
					//Since root must be black, and parent is red, grandparent must exist
					//and grandparent must be black since no two reds are touching
					if(this.IsRed(token.Parent.Sibling)) {
						this.SetColor(token.Parent.Sibling, RedBlackColors.Black);
						this.SetColor(token.Parent, RedBlackColors.Black);
						this.SetColor(token.Parent.Parent, RedBlackColors.Red);
						this.OnItemInsertedInternal(token.Parent.Parent);
					} else {
						if(token.IsRightChild && token.Parent.IsLeftChild) {
							this.RotateLeft(token.Parent);
							token = token.Left;
						} else if(token.IsLeftChild && token.Parent.IsRightChild) {
							this.RotateRight(token.Parent);
							token = token.Right;
						}
						this.SetColor(token.Parent, RedBlackColors.Black);
						this.SetColor(token.Parent.Parent, RedBlackColors.Red);
						if(token.IsLeftChild && token.Parent.IsLeftChild)
							this.RotateRight(token.Parent.Parent);
						else 
							this.RotateLeft(token.Parent.Parent);
					}
				}
			} else {
				this.SetColor(token, RedBlackColors.Black);
			}
		}

		

		private bool IsBlack(BinarySearchNode node) {
			return this.GetColor(node) == RedBlackColors.Black;
		}

		private bool IsRed(BinarySearchNode node) {
			return this.GetColor(node) == RedBlackColors.Red;
		}

		/*private void IsColor(BinarySearchNode node, RedBlackColors color) {
			if(color == RedBlackColors.Black)
				return this.IsBlack(node);
			return this.IsRed(node);
		}*/

		private void SetColor(BinarySearchNode node, RedBlackColors color) {
			(node as RedBlackNode).Color = color;
		}

		protected override void OnItemDeleted(BinarySearchNode deleted, BinarySearchNode location) {
			if(this.IsBlack(deleted)) {
				bool isleft = location.IsSentinel ? deleted.Parent.Left.IsSentinel : location.IsLeftChild;

				this.OnItemDeletedInternal(isleft, location, deleted.Parent, isleft ? deleted.Parent.Right : deleted.Parent.Left);
			}
			base.OnItemDeleted(deleted, location);
		}

		private RedBlackColors GetColor(BinarySearchNode node) {
			if((node == null) || (node.IsSentinel))
				return RedBlackColors.Black;
			return (node as RedBlackNode).Color;
		}

		private void OnItemDeletedInternal(bool isleft, BinarySearchNode token, BinarySearchNode parent, BinarySearchNode sibling) {
			if((parent == null) || this.IsRed(token)) {
				if(!token.IsSentinel)
					this.SetColor(token, RedBlackColors.Black);
			} else {
				//token can be sentinel
				if(this.IsRed(sibling)) {
					this.SetColor(sibling, RedBlackColors.Black);
					this.SetColor(parent, RedBlackColors.Red);
					if(isleft)
						this.RotateLeft(parent);
					else
						this.RotateRight(parent);
					this.OnItemDeletedInternal(isleft, token, parent, isleft ? parent.Right : parent.Left);
				} /*else if(token.Sibling.IsSentinel) {
					this.OnItemDeletedInternal(token.Parent);
				}*/ else {
					BinarySearchNode near = isleft ? sibling.Left : sibling.Right;
					BinarySearchNode far = isleft ? sibling.Right : sibling.Left;
					if(this.IsBlack(near) && this.IsBlack(far)) {
						//this holds if both near and far are sentinels
						//after this, there has to be a red nephew

						//I am having problem with token.sibling being sentinel
						this.SetColor(sibling, RedBlackColors.Red);
						this.OnItemDeletedInternal(parent.IsLeftChild, parent, parent.Parent, parent.Sibling);
					} else if(this.IsRed(near) && this.IsBlack(far)) {
						this.SetColor(near, RedBlackColors.Black);
						this.SetColor(sibling, RedBlackColors.Red);
						if(isleft)
							this.RotateRight(sibling);
						else
							this.RotateLeft(sibling);
						this.OnItemDeletedInternal(isleft, token, parent, isleft ? parent.Right : parent.Left);
					} else {
						this.SetColor(sibling, this.GetColor(parent));
						this.SetColor(parent, RedBlackColors.Black);
						this.SetColor(far, RedBlackColors.Black);
						if(isleft)
							this.RotateLeft(parent);
						else
							this.RotateRight(parent);
					}
				}
			}



				/*if(token.IsRoot || this.IsRed(token)) {
								if(!token.IsSentinel)
									this.SetColor(token, RedBlackColors.Black);
							} else if(token.Sibling.IsSentinel) {
								this.OnItemDeletedInternal(token.Parent);
							} else if(this.IsRed(token.Sibling)) {
								this.SetColor(token.Sibling, RedBlackColors.Black);
								this.SetColor(token.Parent, RedBlackColors.Red);
								if(token.IsLeftChild)
									this.RotateLeft(token.Parent);
								else
									this.RotateRight(token.Parent);
								this.OnItemDeletedInternal(token);
							} else if(this.IsBlack(token.Sibling)) {
								BinarySearchNode nearNephew = token.IsRightChild ? token.Sibling.Right : token.Sibling.Left;
								BinarySearchNode farNephew = token.IsRightChild ? token.Sibling.Left : token.Sibling.Right;
								if(this.IsBlack(nearNephew) && this.IsBlack(farNephew)) {
									//this holds if both near and far are sentinels
									//after this, there has to be a red nephew
									this.SetColor(token.Sibling, RedBlackColors.Red);
									this.OnItemDeletedInternal(token.Parent);
								} else if(nearNephew.IsSentinel || farNephew.IsSentinel) {
									this.SetColor(farNephew.IsSentinel ? nearNephew : farNephew, RedBlackColors.Black);
									if(token.IsLeftChild)
										this.RotateLeft(token.Parent);
									else
										this.RotateRight(token.Parent);
								} else {
									//black sibling, at least one red nephew
									if(this.IsBlack(farNephew)) {
										this.SetColor(token.Sibling, RedBlackColors.Red);
										this.SetColor(nearNephew, RedBlackColors.Black);
										if(token.IsLeftChild)
											this.RotateRight(token.Sibling);
										else
											this.RotateLeft(token.Sibling);
									}
									this.SetColor(farNephew, RedBlackColors.Black);
									if(token.IsLeftChild)
										this.RotateLeft(token.Parent);
									else
										this.RotateRight(token.Parent);
								}
							}
							*/











				//since the item deleted and its child were black, in order
				//for the same number of black nodes along any path to leaf,
				//the the token(node that took over for the deleted node)
				//must have a sibling and the sibling must have two children
				/*RedBlackNode parent = token.Parent as RedBlackNode;
				RedBlackNode sibling = token.Sibling as RedBlackNode;

				if(sibling.Color == RedBlackColors.Red) {
					parent.Color = RedBlackColors.Red;
					sibling.Color = RedBlackColors.Black;
					if(token.IsLeftChild)
						this.RotateLeft(parent);
					else
						this.RotateRight(parent);
				}

				RedBlackNode nephew1 = sibling.Left as RedBlackNode;
				RedBlackNode nephew2 = sibling.Right as RedBlackNode;
				if ((parent.Color == RedBlackColors.Black) &&
					sibling.Color == RedBlackColors.Black &&
					nephew1.Color == RedBlackColors.Black &&
					nephew2.Color == RedBlackColors.Black) {
					sibling.Color = RedBlackColors.Red;
					this.OnItemDeletedInternal(parent);
				} else {
					if ((parent.Color == RedBlackColors.Red) &&
						(sibling.Color == RedBlackColors.Black) &&
						(nephew1.Color == RedBlackColors.Black) &&
						(nephew2.Color == RedBlackColors.Black)) {
						sibling.Color = RedBlackColors.Red;
						parent.Color = RedBlackColors.Black;
					} else {
						if (token.IsLeftChild &&
							(sibling.Color == RedBlackColors.Black) &&
							(nephew1.Color == RedBlackColors.Red) &&
							(nephew2.Color == RedBlackColors.Black)) {
							sibling.Color = RedBlackColors.Red;
							nephew1.Color = RedBlackColors.Black;
							this.RotateRight(sibling);
						} else if (token.IsRightChild &&
							(sibling.Color == RedBlackColors.Black) &&
							(nephew2.Color == RedBlackColors.Red) &&
							(nephew1.Color == RedBlackColors.Black)) {
							sibling.Color = RedBlackColors.Red;
							nephew2.Color = RedBlackColors.Black;
							this.RotateLeft(sibling);
						}
						sibling.Color = parent.Color;
						parent.Color = RedBlackColors.Black;
						if (token.IsLeftChild) {
							nephew2.Color = RedBlackColors.Black;
							this.RotateLeft(parent);
						} else {
							nephew1.Color = RedBlackColors.Black;
							this.RotateRight(parent);
						}
					}
				}
			}*/
		}

		///<summary>
		/// RotateLeft
		/// Rebalance the tree by rotating the nodes to the left
		///</summary>
		public void RotateLeft(BinarySearchNode x) {
			if(!x.IsSentinel && !x.Right.IsSentinel) {
				BinarySearchNode y = x.Right;
				x.Right = y.Left;
				if(!y.Left.IsSentinel)
					y.Left.Parent = x;
				y.Parent = x.Parent;
				if(x.Parent != null) {		
					if(x == x.Parent.Left)			
						x.Parent.Left = y;
					else
						x.Parent.Right = y;
				} 
				else
					this.Root = y;
				y.Left = x;
				x.Parent = y;		
			}
		}
		///<summary>
		/// RotateLeft
		/// Rebalance the tree by rotating the nodes to the right
		///</summary>
		public void RotateRight(BinarySearchNode x) {
			if(!x.IsSentinel && !x.Left.IsSentinel) {
				BinarySearchNode y = x.Left;
				x.Left = y.Right;
				if(!y.Right.IsSentinel)
					y.Right.Parent = x;
				y.Parent = x.Parent;
				if(x.Parent != null) {		
					if(x == x.Parent.Left)			
						x.Parent.Left = y;
					else
						x.Parent.Right = y;
				} 
				else 
					this.Root = y;
				y.Right = x;
				x.Parent = y;		
			}
		}
	}
}
