// Alex Strawn
// 11632677

namespace ExpressionTreeConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CptS321;

    public class ExpressionTreeConsoleApp
    {
        static void Main(string[] args)
        {
            int choice = 0;
            string val = " ";

            ExpressionTree tree = new ExpressionTree("A1-12-B1");

            // exit when 4 is read
            while (choice != 4)
            {
                // print menu
                Console.WriteLine("Menu (current expression= \"" + tree.Expression + "\")");
                Console.WriteLine("   1 = Enter a new expression");
                Console.WriteLine("   2 = Set a variable value");
                Console.WriteLine("   3 = Evaluate tree");
                Console.WriteLine("   4 = Quit");

                // read input for choice
                val = Console.ReadLine();
                choice = Convert.ToInt32(val);

                // carry out function based on input
                if (choice == 1)
                {
                    Console.WriteLine("Enter new expression: ");

                    // change expression
                    val = Console.ReadLine();
                    tree = new ExpressionTree(val);
                }
                else if (choice == 2)
                {
                    // change variable value
                    Console.WriteLine("Enter variable name: ");

                    val = Console.ReadLine();

                    string number = " ";
                    int num = 0;

                    Console.WriteLine("Enter variable value: ");

                    number = Console.ReadLine();
                    num = Convert.ToInt32(number);

                    tree.SetVariable(val, num);
                }
                else if (choice == 3)
                {
                    // evaluate tree
                    double num = tree.Evaluate();
                    Console.WriteLine("Tree evaluates to: " + num);
                }
            }

            Console.WriteLine("Task Finished");
        }
    }
}
