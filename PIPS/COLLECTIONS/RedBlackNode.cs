using System;

namespace PIPS.Collections
{
	/// <summary>
	/// Summary description for RedBlackNode.
	/// </summary>
	public class RedBlackNode : BinarySearchNode
	{
		public RedBlackNode()
		{
			this.Color = RedBlackColors.Red;
		}

		private RedBlackColors intColor;
		///<summary>
		///Color
		///</summary>
		public RedBlackColors Color {
			get {
				return intColor;
			}
			set {
				intColor = value;
			}
		}
	}

	public enum RedBlackColors {
		Red,
		Black
	}
}
