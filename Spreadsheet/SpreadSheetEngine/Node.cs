// Alex Strawn
// 11632677

namespace CptS321
{
	public abstract class Node
	{
		private Node left;
		private Node right;

		// Node constructor
		public Node()
		{
			left = null;
			right = null;
		}

		public Node Left
		{
			get { return left; }
			set { left = value; }
		}

		public Node Right
		{
			get { return right; }
			set { right = value; }
		}
	}
}
