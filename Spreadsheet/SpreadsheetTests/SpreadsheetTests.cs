// Alex Strawn
// 11632677

namespace SpreadsheetTests
{
    using System.Collections;
    using System.Collections.Generic;
    using CptS321;
    using NUnit.Framework;
    using Spreadsheet_Alex_Strawn;

    [TestFixture]
    public class SpreadsheetTests
    {
        // get cell based on a name (ex. "B4")
        [Test]
        public void GetCellNameTest()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell expectedCell = sheet.GetCell(0, 2); // expecting to get cell A3
            Assert.AreEqual(expectedCell, sheet.GetCell("A3"));
        }

        // get a cell based on its coordinate values (column and row)
        [Test]
        public void GetCellCoordTest()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell expectedCell = sheet.GetCell("K12");
            Assert.AreEqual(expectedCell, sheet.GetCell(10, 11));
        }

        // showcases setting the value of a cell using another cell's name
        [Test]
        public void DemoValueTest()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.Demo(); // sets value "This is cell B1" to cell B1 and then copies this value to cell A1 using "=B1"
            Assert.AreEqual(sheet.GetCell("B1").Value, sheet.GetCell("A1").Value); // we expect cell A1 to return the same value as cell B1
        }

        // tests evaluation tree implementation in spreadsheet
        [Test]
        public void CellEvaluationTreeTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "16";
            sheet.GetCell("C2").Text = "7";
            finalCell.Text = "=(B1-C2)/3";
            Assert.AreEqual("3", finalCell.Value);
        }

        [Test]
        public void CellEvaluationTreeTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("E13");
            sheet.GetCell("E8").Text = "2";
            sheet.GetCell("Z20").Text = "10";
            sheet.GetCell("A2").Text = "12";
            finalCell.Text = "=A2*(E8+Z20/4)";
            Assert.AreEqual("54", finalCell.Value);
        }

        [Test]
        public void CellEvaluationTreeTest3()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A3");
            sheet.GetCell("G12").Text = "16";
            sheet.GetCell("I5").Text = "14";
            sheet.GetCell("A1").Text = "2";
            sheet.GetCell("Q3").Text = "=A1+I5"; // "16"
            finalCell.Text = "=G12-A1*I5/Q3+2";
            Assert.AreEqual("12.25", finalCell.Value);
        }

        [Test]
        public void CellEvaluationTreeTest4()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "12";
            finalCell.Text = "=B1-2";
            Assert.AreEqual("10", finalCell.Value);
        }

        [Test]
        public void CellEvaluationTest()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("C1");
            sheet.GetCell("F11").Text = "C1 should have the same value as this cell!";
            finalCell.Text = "=F11";
            Assert.AreEqual("C1 should have the same value as this cell!", finalCell.Value);
        }

        // tests updating cells when other cells, that their value relies upon, change
        [Test]
        public void VariableChangeTest()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "16";
            finalCell.Text = "=B1";
            Assert.AreEqual("16", finalCell.Value);
            sheet.GetCell("B1").Text = "4";
            Assert.AreEqual("4", finalCell.Value);
        }

        [Test]
        public void VariableChangeTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "16";
            sheet.GetCell("C1").Text = "4";
            finalCell.Text = "=B1-C1/2";
            Assert.AreEqual("14", finalCell.Value);
            sheet.GetCell("B1").Text = "4";
            Assert.AreEqual("2", finalCell.Value);
        }

        [Test]
        public void VariableChangeTest3()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "16";
            sheet.GetCell("C1").Text = "This is cell C1";
            finalCell.Text = "=B1-C1/2";
            Assert.AreEqual("16", finalCell.Value);
            sheet.GetCell("B1").Text = "4";
            Assert.AreEqual("4", finalCell.Value);
        }

        [Test]
        public void VariableChangeTest4()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell finalCell = sheet.GetCell("A1");
            sheet.GetCell("B1").Text = "16";
            sheet.GetCell("V1").Text = "5";
            sheet.GetCell("C1").Text = "=V1-1";
            finalCell.Text = "=B1+C1";
            Assert.AreEqual("20", finalCell.Value);
            sheet.GetCell("V1").Text = "=B1-10";
            Assert.AreEqual("21", finalCell.Value);
        }

        // tests changing background color
        [Test]
        public void ColorChangeTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            sheet.ChangeCellColor(cell, 0xFFF5FFFA);
            Assert.AreEqual(0xFFF5FFFA, cell.Color);
        }

        [Test]
        public void ColorChangeTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("C20");
            sheet.ChangeCellColor(cell, 0xFFFF0000);
            Assert.AreEqual(0xFFFF0000, cell.Color);
        }

        // tests undo/redo functions
        [Test]
        public void UndoTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "value";
            cell.Text = "no value";
            sheet.Undo();
            Assert.AreEqual("value", cell.Text);
        }

        [Test]
        public void UndoTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "True";
            Cell cell2 = sheet.GetCell("B2");
            cell2.Text = "Something";
            cell.Text = "False";
            sheet.Undo();
            Assert.AreEqual("True", cell.Text);
            sheet.Undo();
            Assert.AreEqual(null, cell2.Text);
        }

        [Test]
        public void RedoTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "True";
            sheet.Undo();
            sheet.Redo();
            Assert.AreEqual("True", cell.Text);
        }

        [Test]
        public void RedoTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("B2");
            Cell cell2 = sheet.GetCell("C17");
            cell.Text = "True";
            cell2.Text = "False";
            cell2.Text = "=B2";
            sheet.Undo();
            sheet.Redo();
            Assert.AreEqual("=B2", cell2.Text);
            cell.Text = "False";
            sheet.Undo();
            sheet.Redo();
            Assert.AreEqual("False", cell.Text);
        }

        // Tests saving and loading .xml files
        [Test]
        public void FileSaveTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.GetCell("A1").Text = "12";
            Assert.AreEqual(true, sheet.SaveFile("testsave"));
        }

        [Test]
        public void FileSaveTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.GetCell("A1").Text = "12";
            Assert.AreEqual(true, sheet.SaveFile("testsave"));
            sheet.GetCell("A1").Text = "13";
            sheet.GetCell("B1").Text = "=A1";
            Assert.AreEqual(true, sheet.SaveFile("testsave"));
        }

        // for load tests, will need to specify what directory you want the file to be found in
        [Test]
        public void FileLoadTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.LoadFile("C:\\Users\\Alex Strawn\\cpts321-spring2020\\Spreadsheet_Alex_Strawn\\SpreadsheetTests\\testsave1.xml");
            Assert.AreEqual("15", sheet.GetCell("A1").Text);
        }

        [Test]
        public void FileLoadTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.LoadFile("C:\\Users\\Alex Strawn\\cpts321-spring2020\\Spreadsheet_Alex_Strawn\\SpreadsheetTests\\testsave1.xml");
            Assert.AreEqual("15", sheet.GetCell("A1").Text);
            sheet.LoadFile("C:\\Users\\Alex Strawn\\cpts321-spring2020\\Spreadsheet_Alex_Strawn\\SpreadsheetTests\\testsave2.xml");
            Assert.AreEqual("22", sheet.GetCell("A1").Text);
            Assert.AreEqual("=A1", sheet.GetCell("C12").Text);
            Assert.AreEqual(0xFF00FF40, sheet.GetCell("C12").Color);
        }

        [Test]
        public void FileLoadTest3()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            sheet.LoadFile("C:\\Users\\Alex Strawn\\cpts321-spring2020\\Spreadsheet_Alex_Strawn\\SpreadsheetTests\\testsave3.xml");
            Assert.AreEqual("6", sheet.GetCell("B1").Value);
            Assert.AreEqual("=A1+6", sheet.GetCell("B1").Text);
            Assert.AreEqual(16744448, sheet.GetCell("B1").Color);
        }

        // Tests for reference errors
        [Test]
        public void RefErrorTest1()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=A1";
            Assert.AreEqual("!(self reference)", cell.Value);
        }

        [Test]
        public void RefErrorTest2()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=Ba";
            Assert.AreEqual("!(bad reference)", cell.Value);
        }

        [Test]
        public void RefErrorTest3()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=K1234";
            Assert.AreEqual("!(bad reference)", cell.Value);
        }

        [Test]
        public void RefErrorTest4()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=(2*A1)-1";
            Assert.AreEqual("!(self reference)", cell.Value);
        }

        [Test]
        public void RefErrorTest5()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=B1";
            sheet.GetCell("B1").Text = "=B2";
            sheet.GetCell("B2").Text = "=A1";
            Assert.AreEqual("!(circular reference)", sheet.GetCell("B2").Value);
        }

        [Test]
        public void RefErrorTest6()
        {
            Spreadsheet sheet = new Spreadsheet(26, 50);
            Cell cell = sheet.GetCell("A1");
            cell.Text = "=B1";
            sheet.GetCell("B1").Text = "=B2";
            sheet.GetCell("B2").Text = "=Z123456*2";
            Assert.AreEqual("!(bad reference)", cell.Value);
        }
    }
}