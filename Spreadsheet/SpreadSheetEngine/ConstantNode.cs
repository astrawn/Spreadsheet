// Alex Strawn
// 11632677

namespace CptS321
{
	public class ConstantNode : Node
	{
		private double value;

		public ConstantNode(double value)
		{
			this.value = value;
		}

		public ConstantNode(int value)
		{
			this.value = (double)value;
		}

		public double Value
		{
			get { return this.value; }
			set { this.value = value; }
		}
	}
}
