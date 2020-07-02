// Alex Strawn
// 11632677

namespace CptS321
{
    public class OperatorNode : Node
    {
        private int precedence;
        private char op;

        public OperatorNode(char c)
        {
            if (c == '*')
            {
                precedence = 1;
            }
            else if (c == '/')
            {
                precedence = 2;
            }
            else if (c == '+')
            {
                precedence = 3;
            }
            else if (c == '-')
            {
                precedence = 4;
            }
            else if (c == '(')
            {
                precedence = 5;
            }

            Op = c;
        }

        public int Precedence
        {
            get { return precedence; }
            set { precedence = value; }
        }

        public char Op
        {
            get { return op; }
            set { op = value; }
        }
    }
}
