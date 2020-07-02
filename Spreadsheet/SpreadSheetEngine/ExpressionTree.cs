// Alex Strawn
// 11632677

namespace CptS321
{
	using System;
	using System.Collections.Generic;

	public class ExpressionTree
	{
		private Node root;
		private Dictionary<string, double> variables = new Dictionary<string, double>();
		private string expression;

		public ExpressionTree(string expression)
		{
			this.expression = expression;
			List<Node> postfixExpression = new List<Node>();
			List<OperatorNode> operatorStack = new List<OperatorNode>();
			string variable = string.Empty;
			string integer = string.Empty;
			string expressionString = string.Empty;
			OperatorNode opNode;

			// create postfix expression from given expression

				// begin by building postfixExpression and operatorStack lists
			for (int i = 0; i < expression.Length; i++)
			{
				if (variable != string.Empty && Char.IsLetterOrDigit(expression[i]))
				{
					variable += expression[i];
				}
				else if (Char.IsLetter(expression[i]))
				{
					variable += expression[i];
				}
				else if (variable == string.Empty && Char.IsDigit(expression[i]))
				{
					integer += expression[i];
				}
				else if (integer != string.Empty && Char.IsDigit(expression[i]))
				{
					integer += expression[i];
				}
				else if (!Char.IsLetterOrDigit(expression[i]) && (expression[i] != ')') && variable != string.Empty)
				{
					postfixExpression.Add(new VariableNode(variable)); // add variable to postfix expression

					opNode = new OperatorNode(expression[i]);

					// only do if operator stack is not empty
					if (operatorStack.Count > 0)
					{
						bool overPrecedence = true;

						// encountered operator or parenthesis, determine what happens next
						while (overPrecedence == true && operatorStack.Count != 0)
						{
							// pop operators from operator stack and push onto list expression if precedence is greater than or equal to operator being pushed onto operator stack
							if (operatorStack[operatorStack.Count - 1].Precedence <= opNode.Precedence)
							{
								postfixExpression.Add(operatorStack[operatorStack.Count - 1]);
								operatorStack.RemoveAt(operatorStack.Count - 1);
							}
							else
							{
								overPrecedence = false;
							}
						}
					}

					operatorStack.Add(opNode); // push operator onto operator stack
					variables.Add(variable, 0); // add variable to dictionary, initialize to 0
					variable = string.Empty; // reset variable string to empty, prepare for new variable entry
				}
				else if (!Char.IsLetterOrDigit(expression[i]) && (expression[i] != ')') && integer != string.Empty)
				{
					postfixExpression.Add(new ConstantNode(double.Parse(integer))); // add integer to postfix expression

					opNode = new OperatorNode(expression[i]);

					if (operatorStack.Count > 0)
					{
						bool overPrecedence = true;

						// encountered operator or parenthesis, determine what happens next
						while (overPrecedence == true && operatorStack.Count != 0)
						{
							// pop operators from operator stack and push onto list expression if precedence is greater than or equal to operator being pushed onto operator stack
							if (operatorStack[operatorStack.Count - 1].Precedence <= opNode.Precedence)
							{
								postfixExpression.Add(operatorStack[operatorStack.Count - 1]);
								operatorStack.RemoveAt(operatorStack.Count - 1);
							}
							else
							{
								overPrecedence = false;
							}
						}
					}

					operatorStack.Add(opNode); // push operator onto operator stack
					integer = string.Empty; // reset integer string to empty, prepare for new integer entry
				}
				else if (expression[i] == '(')
				{
					operatorStack.Add(new OperatorNode('('));
				}
				else if (expression[i] == ')' && variable != string.Empty)
				{
					postfixExpression.Add(new VariableNode(variable)); // add variable to postfix expression

					if (!variables.ContainsKey(variable))
					{
						variables.Add(variable, 0); // add variable to dictionary, initialize to 0
					}

					variable = string.Empty; // reset variable string to empty, prepare for new variable entry

					// while no left bracket found, pop operators from operator stack onto expression
					while (operatorStack[operatorStack.Count - 1].Op != '(')
					{
						postfixExpression.Add(operatorStack[operatorStack.Count - 1]);
						operatorStack.RemoveAt(operatorStack.Count - 1);
					}

					operatorStack.RemoveAt(operatorStack.Count - 1); // remove left bracket from operator stack
				}
				else if (expression[i] == ')' && integer != string.Empty)
				{
					postfixExpression.Add(new ConstantNode(double.Parse(integer))); // add integer to postfix expression
					integer = string.Empty; // reset integer string to empty, prepare for new integer entry

					// while no left bracket found, pop operators from operator stack onto expression
					while (operatorStack[operatorStack.Count - 1].Op != '(')
					{
						postfixExpression.Add(operatorStack[operatorStack.Count - 1]);
						operatorStack.RemoveAt(operatorStack.Count - 1);
					}

					operatorStack.RemoveAt(operatorStack.Count - 1); // remove left bracket from operator stack
				}
				else if (expression[i] == ')')
				{
					// while no left bracket found, pop operators from operator stack onto expression
					while (operatorStack[operatorStack.Count - 1].Op != '(')
					{
						postfixExpression.Add(operatorStack[operatorStack.Count - 1]);
						operatorStack.RemoveAt(operatorStack.Count - 1);
					}

					operatorStack.RemoveAt(operatorStack.Count - 1); // remove left bracket from operator stack
				}
				else if (expression[i] == '*' || expression[i] == '+' || expression[i] == '-' || expression[i] == '/')
				{
					operatorStack.Add(new OperatorNode(expression[i])); // adds operator to stack, only used when an operator appears after a ')'
				}

				// takes into account an expression of only one variable or one constant  (ex. "x" or "12")
				if ((i + 1) == expression.Length && variable != string.Empty)
				{
					variables.Add(variable, 0);
					postfixExpression.Add(new VariableNode(variable));
				}
				else if ((i + 1) == expression.Length && integer != string.Empty)
				{
					postfixExpression.Add(new ConstantNode(double.Parse(integer)));
				}
			}

			int operands = postfixExpression.Count; // keeps track of how many operands in expression

				// pop the rest of the operators off of the operator stack
			for (int j = 0; j < operatorStack.Count; j++)
			{
				int index = operatorStack.Count - 1;

				postfixExpression.Add(operatorStack[index - j]);
			}

			Node curr = null; // used to keep track of what node we are on
			int postIndex = postfixExpression.Count - 1;
			List<Node> markers = new List<Node>();

				// build tree based on postfixExpression
			while (postIndex >= 0)
			{
				// setting first node
				if (root == null)
				{
					root = postfixExpression[postIndex]; // set root to first node encountered in list

					curr = root; // set tracker to root after it is created

					postIndex -= 1;
				}
				else
				{
					if (postIndex >= 0)
					{
						// determine where next node needs to go based on positional criteria
						if ((curr.Left != null) && (curr.Right != null) && markers.Count != 0 && postIndex == 0)
						{
							// Condition: if both left and right nodes are full, there are still markers in marker list, and we are now at the beginning of the postfixExpression
							// Do: then work back through markers until we've reached needed position and place last node
							while (curr.Left != null && curr.Right != null)
							{
								curr = markers[markers.Count - 1];
								markers.RemoveAt(markers.Count - 1);
							}

							if (curr.Right == null)
							{
								curr.Right = postfixExpression[postIndex];
							}
							else if (curr.Left == null)
							{
								curr.Left = postfixExpression[postIndex];
							}

							postIndex -= 1;
						}
						else if (!(curr.Left is OperatorNode) && !(curr.Right is OperatorNode) && (curr.Left != null) && (curr.Right != null) && markers.Count != 0)
						{
							// Condition: if both left and right nodes are not operators, are not null, and there are markers still in marker list
							// Do: then move back to the last node that has an open left or right and place next node in open position
							while (curr.Left != null && curr.Right != null)
							{
								curr = markers[markers.Count - 1];
								markers.RemoveAt(markers.Count - 1);
							}

							if (curr.Right == null)
							{
								curr.Right = postfixExpression[postIndex];
								if (curr.Right is OperatorNode)
								{
									curr = curr.Right;
								}
							}
							else if (curr.Left == null)
							{
								curr.Left = postfixExpression[postIndex];
								if (curr.Left is OperatorNode)
								{
									curr = curr.Left;
								}
							}

							postIndex -= 1;
						}
						else if (curr is OperatorNode && !(postfixExpression[postIndex] is OperatorNode) && !(postfixExpression[postIndex - 1] is OperatorNode))
						{
							// Condition: if we are currently on an operator node, and the next two nodes on postfixExpression are not operators
							// Do: then add these two nodes to the right and left of our current node
							curr.Right = postfixExpression[postIndex];
							curr.Left = postfixExpression[postIndex - 1];
							postIndex -= 2;
						}
						else if (curr is OperatorNode && postfixExpression[postIndex] is OperatorNode && curr.Right == null)
						{
							// Condition: if we are currently on an operator node, and the next node in the expression is an operator, and the current node's right is empty
							// Do: then add a marker to marker list that tracks current node, add node from expression to right of current node, and move current to the node we just placed
							markers.Add(curr); // drop marker to come back to
							curr.Right = postfixExpression[postIndex];
							curr = curr.Right;
							postIndex -= 1;
						}
						else if ((curr is OperatorNode) && (postfixExpression[postIndex] is OperatorNode) && (curr.Left == null))
						{
							// Condition: if our current node is an operator, and the next node in expression is an operator, and the current node's left is empty
							// Do: then add a marker to marker list that tracks current node, add node from expression to left of current node, and move current to the node we just placed
							markers.Add(curr);
							curr.Left = postfixExpression[postIndex];
							curr = curr.Left;
							postIndex -= 1;
						}
						else
						{
							// Condition: if none of the other conditional statements matched with conditions
							// Do: then place the next node in expression to the right of our current node
							curr.Right = postfixExpression[postIndex];
							postIndex -= 1;
						}
					}
					else
					{
						curr.Right = postfixExpression[postIndex]; // add node to right
						postIndex -= 1;
					}
				}
			}
		}

		public string Expression
		{
			get { return expression; }
			set { expression = value; }
		}

		public Dictionary<string, double> Variables
		{
			get { return variables; }
			internal set { variables = value; }
		}

		public double Evaluate()
		{
			double answer = 0;

			// accounts for empty expression
			if (root == null)
			{
				return 0;
			}
			else
			{
				answer = RecursiveEvaluate(root); // evaluate tree
			}

			return answer;
		}

		// recursively evaluates tree
		private double RecursiveEvaluate(Node root)
		{
			OperatorNode operatorHolder = new OperatorNode(' ');
			ConstantNode constantHolder = new ConstantNode(0);
			VariableNode variableHolder = new VariableNode(string.Empty);
			double num = 0;

			if (root is OperatorNode)
			{
				// if node is operator, evaluate left and right subtrees
				operatorHolder = (OperatorNode)root;
				return Operation(RecursiveEvaluate(root.Left), RecursiveEvaluate(root.Right), operatorHolder.Op);
			}
			else if (root is VariableNode)
			{
				// if node is variable, return value stored in variable
				variableHolder = (VariableNode)root;
				num = Variables[variableHolder.Name];
				return num;
			}
			else
			{
				// if node is constant, return value of constant
				constantHolder = (ConstantNode)root;
				num = constantHolder.Value;
				return num;
			}
		}

		// carries out operation based on operator given
		private double Operation(double num1, double num2, char op)
		{
			double answer = 0;

			if (op == '+')
			{
				answer = num1 + num2;
			}
			else if (op == '-')
			{
				answer = num1 - num2;
			}
			else if (op == '*')
			{
				answer = num1 * num2;
			}
			else if (op == '/')
			{
				answer = num1 / num2;
			}

			return answer;
		}

		public void SetVariable(string name, double value)
        {
            variables[name] = value;
        }

		public bool SetVariables(Spreadsheet sheet, List<string> var)
		{
			double parseValue;
			bool error = false;

			// set variable values based on others cell's values
			for (int i = 0; i < Variables.Count; i++)
			{
				/*
				   Numerical value of a cell will be:
					- The numerical value parsed if double.TryParse on the value string succeeds
					- 0 otherwise
				 */

				// ensure that value in this cell can be parsed into a double
				if (sheet.GetCell(var[i]) == null)
				{
					error = true;
				}
				else if (double.TryParse(sheet.GetCell(var[i]).Value, out parseValue) == true)
				{
					Variables[var[i]] = parseValue;
				}
				else if (sheet.GetCell(var[i]).Value == "!(bad reference)" || sheet.GetCell(var[i]).Value == "!(self reference)")
				{
					error = true;
				}
				else
				{
					Variables[var[i]] = 0;
				}
			}

			return error;

		}
	}
}