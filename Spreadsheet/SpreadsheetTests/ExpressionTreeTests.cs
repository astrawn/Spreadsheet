// Alex Strawn
// 11632677

namespace ExpressionTreeTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CptS321;
    using NUnit.Framework;

    [TestFixture]
    public class ExpressionTreeTests
    {
        // Test expression tree evaluate method with different inputs
        [Test]
        public void EvaluateTest1()
        {
            ExpressionTree tree = new ExpressionTree("A1-12-B1");
            Assert.AreEqual(-12, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest2()
        {
            ExpressionTree tree = new ExpressionTree("A1+B1+12");
            tree.SetVariable("A1", 2);
            tree.SetVariable("B1", 20);
            Assert.AreEqual(34, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest3()
        {
            ExpressionTree tree = new ExpressionTree("x");
            Assert.AreEqual(0, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest4()
        {
            ExpressionTree tree = new ExpressionTree("2");
            Assert.AreEqual(2, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest5()
        {
            ExpressionTree tree = new ExpressionTree(string.Empty);
            Assert.AreEqual(0, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest6()
        {
            ExpressionTree tree = new ExpressionTree("hello+52+WORLD");
            tree.SetVariable("hello", 12);
            tree.SetVariable("WORLD", 4);
            Assert.AreEqual(68, tree.Evaluate());
        }

        // new evaluate tests for HW6
        [Test]
        public void EvaluateTest7()
        {
            ExpressionTree tree = new ExpressionTree("12+13*2");
            Assert.AreEqual(38, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest8()
        {
            ExpressionTree tree = new ExpressionTree("3*(13-2)");
            Assert.AreEqual(33, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest9()
        {
            ExpressionTree tree = new ExpressionTree("3+(13-2)*2");
            Assert.AreEqual(25, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest10()
        {
            ExpressionTree tree = new ExpressionTree("3+(2*(3+2))*2");
            Assert.AreEqual(23, tree.Evaluate());
        }

        [Test]
        public void EvaluateTest11()
        {
            ExpressionTree tree = new ExpressionTree("(A1+2)*B1-D1*4");
            tree.SetVariable("A1", 3);
            tree.SetVariable("B1", 2);
            tree.SetVariable("D1", 6);
            Assert.AreEqual(-14, tree.Evaluate());
        }

        // test set variable method with input and one test without input
        [Test]
        public void SetVariableTest1()
        {
            ExpressionTree tree = new ExpressionTree("x");
            tree.SetVariable("x", 4);
            Assert.AreEqual(4, tree.Variables["x"]);
        }

        [Test]
        public void SetVariableTest2()
        {
            ExpressionTree tree = new ExpressionTree("x");
            Assert.AreEqual(0, tree.Variables["x"]);
        }
    }
}
