// Alex Strawn
// 11632677

namespace CptS321
{
	public class VariableNode : Node
	{
		private string name;

		public VariableNode(string name)
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
