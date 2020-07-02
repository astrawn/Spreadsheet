// Alex Strawn
// 11632677

namespace CptS321
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;

    // abstract cell class
    public abstract class Cell
    {
        protected string text;
        protected string value;
        private int rowIndex;
        private int columnIndex;
        protected List<Cell> cellDependencies; // keeps a log of what cells depend on this cell's value
        protected List<Cell> previousVariables; // keeps track of cells that were last used as variables in an expression
        protected List<Cell> variables;
        protected uint BGColor;

        // cell constructor
        public Cell(int x, int y)
        {
            // set row and column values for cell
            rowIndex = y;
            columnIndex = x;
            cellDependencies = new List<Cell>();
            previousVariables = new List<Cell>();
            variables = new List<Cell>();
            BGColor = 0xFFFFFFFF;
        }

        public delegate void CellPropertyChangedEventHandler(object sender);

        public delegate void AlertUndoEventHandler(object sender, string change);

        public event CellPropertyChangedEventHandler TextChanged;

        public event CellPropertyChangedEventHandler ValueChanged;

        public event CellPropertyChangedEventHandler ColorChanged;

        public event AlertUndoEventHandler Undo;

        public int RowIndex
        {
            get { return rowIndex; }
        }

        public int ColumnIndex
        {
            get { return columnIndex; }
        }

        public List<Cell> CellDependencies
        {
            get { return cellDependencies; }
            set { cellDependencies = value; }
        }

        public List<Cell> PreviousVariables
        {
            get { return previousVariables; }
            set { previousVariables = value; }
        }

        public List<Cell> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        public uint Color
        {
            get { return BGColor; }

            internal set
            {
                if (BGColor != value)
                {
                    AlertUndo(this, "color");
                    BGColor = value;
                    OnColorChanged();
                }
            }
        }

        public string Text
        {
            get { return text; }

            set
            {
                // set text to new value if it does not already equal this value
                if (text != value)
                {
                    AlertUndo(this, "text");
                    text = value;
                    OnTextChanged(); // notify spreadsheet of property change
                }
            }
        }

        public string Value
        {
            get { return this.value; }

            // setter is internal, only classes within library can change this value
            // value is only changed if the new value is different from original value
            internal set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnValueChanged(); // notify spreadsheet of property changed
                }
            }
        }

        protected virtual void OnTextChanged()
        {
            if (TextChanged != null)
            {
                TextChanged(this); // raise event
            }
        }

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this); // raise event
            }
        }

        protected virtual void OnColorChanged()
        {
            if (ColorChanged != null)
            {
                ColorChanged(this); // raise event
            }
        }

        protected virtual void AlertUndo(object sender, string change)
        {
            if (Undo != null)
            {
                Undo(this, change);
            }
        }
    }

    // instantiable cell class
    internal class SpreadsheetCell : Cell
    {
        // inherit constructor of Cell class
        public SpreadsheetCell(int x, int y) : base(x, y)
        {
        }
    }

    // spreadsheet class
    public class Spreadsheet
    {
        private Cell[,] sheet;
        private Cell alteredCell;
        private int columnCount;
        private int rowCount;
        private List<List<Cell>> undoStack; // keep track of changes made as a group of cells
        private List<Tuple<Cell, string>> undoChangeList; // keep track of individual cell changes
        private List<List<Cell>> redoStack; // keep track of changes made as a group of cells
        private List<Tuple<Cell, string>> redoChangeList; // keep track of individual cell changes
        private Cell undoRedoCell; // keep track of the cell currently being worked on in the undo/redo process to avoid overlap

        // constructor
        public Spreadsheet(int x, int y)
        {
            sheet = new SpreadsheetCell[x, y]; // create new spreadsheet with dimensions x, y
            undoStack = new List<List<Cell>>();
            undoChangeList = new List<Tuple<Cell, string>>();
            redoStack = new List<List<Cell>>();
            redoChangeList = new List<Tuple<Cell, string>>();

            // fill in spreadsheet with cell objects
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    sheet[i, j] = new SpreadsheetCell(i, j); // create new cell
                    sheet[i, j].TextChanged += CellTextChanged; // subscribe to this cell's text event notification
                    sheet[i, j].ValueChanged += CellValueChanged; // subscribe to this cell's value event notification
                    sheet[i, j].ColorChanged += CellColorChanged; // subscribe to this cell's color event notification
                    sheet[i, j].Undo += AddUndo; // subscribe to this cell's undo event notification
                }
            }

            columnCount = x;
            rowCount = y;
        }

        public delegate void SpreadsheetCellChangeEventHandler(object sender);

        public delegate void SpreadsheetStackChangeEventHandler(object sender, string change);

        public event SpreadsheetCellChangeEventHandler ValueChange;

        public event SpreadsheetCellChangeEventHandler TextChange;

        public event SpreadsheetCellChangeEventHandler ColorChange;

        public event SpreadsheetStackChangeEventHandler AlertOfAddUndo;

        public event SpreadsheetStackChangeEventHandler AlertOfAddRedo;

        public Cell[,] Sheet
        {
            get { return sheet; }
            private set { sheet = value; }
        }

        public Cell AlteredCell
        {
            get { return alteredCell; }
            private set { alteredCell = value; }
        }

        public int ColumnCount
        {
            get { return columnCount; }
            private set { columnCount = value; }
        }

        public int RowCount
        {
            get { return rowCount; }
            private set { rowCount = value; }
        }

        // gets cell based on coordinates
        public Cell GetCell(int columnIndex, int rowIndex)
        {
            return sheet[columnIndex, rowIndex];
        }

        // gets cell based on cell name
        public Cell GetCell(string cellName)
        {
            int column = 0;
            int row = 0;

            try
            {
                // separate name into substrings
                char letter = cellName[0];
                string number = cellName.Substring(1);

                // turn substrings into "coorinates" of cell
                column = (int)letter - 65;
                row = int.Parse(number) - 1;
                return this.sheet[column, row]; // return cell at specified position
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // tells spreadsheet what to do when a cell is changed
        public void CellTextChanged(object sender)
        {
            SpreadsheetCell cell = sender as SpreadsheetCell;

            alteredCell = cell; // set alteredCell to current cell value (allows spreadsheet graphic to view this cell to know what to change)

            // determines what the cell's value should be set to based on its text attribute
            if (cell.Text == string.Empty || cell.Text == null)
            {
                cell.Value = string.Empty;
                cell.Variables = null;
            }
            else if (cell.Text[0] == '=')
            {
                cell.PreviousVariables = cell.Variables;
                string expression = cell.Text.Substring(1); // grab expression from cell

                ExpressionTree tree = new ExpressionTree(expression); // create new expression tree based on expression in cell

                List<string> var = tree.Variables.Keys.ToList<string>(); // create list of variables needing values
                List<Cell> var2 = new List<Cell>();

                for (int k = 0; k < var.Count; k++)
                {
                    if (GetCell(var[k]) != null)
                    {
                        var2.Add(GetCell(var[k])); // create list of cells that this cell depends on
                    }
                }

                cell.Variables = var2; // set cell's variable list

                if (cell.PreviousVariables.Count != 0)
                {
                    List<Cell> difference = cell.PreviousVariables.Except(var2).ToList(); // find difference between what variables are in current expression and what was in expression before

                    if (difference.Count > 0)
                    {
                        for (int t = 0; t < difference.Count; t++)
                        {
                           cell.CellDependencies.Remove(difference[t]); // removes cell from dependency list
                        }
                    }
                }

                // if there are variables in the expression tree, then set the cells with the names of the variables to have this cell as a dependent cell
                if (var.Count != 0)
                {
                    for (int i = 0; i < var.Count; i++)
                    {
                        // check to see if this cell is already a dependent of the variable cell
                        if (GetCell(var[i]) != null)
                        {
                            if (!GetCell(var[i]).CellDependencies.Contains(cell))
                            {
                                GetCell(var[i]).CellDependencies.Add(cell); // add to cell dependency if the current cell is not already there
                            }
                        }
                    }
                }

                // if there is only a single variable and the expression is comprised of only this variable, then copy the value of the variable over to the value of this cell
                // otherwise, evaluate the expression tree and set this evaluated value as the value for this cell
                if (tree.Variables.Count == 1 && expression.Length == var[0].Length)
                {
                    if (GetCell(expression) == null)
                    {
                        cell.Value = "!(bad reference)"; // set to bad reference if cell cannot be found in spreadsheet
                    }
                    else if (GetCell(expression).Value == "!(self reference)")
                    {
                        cell.Value = "!(bad reference)"; // if cell being referred to has a self reference, then set to bad reference
                    }
                    else
                    {
                        bool error = IsReferenced(cell.Variables, cell); // check for circular reference

                        if (cell.CellDependencies.Contains(cell))
                        {
                            cell.Value = "!(self reference)"; // set to self reference if variables in cell includes itself
                        }
                        else if (error == true)
                        {
                            cell.Value = "!(circular reference)"; // sets circular reference if one is found
                        }
                        else if (GetCell(expression).Value == "!(self reference)" || GetCell(expression).Value == "!(bad reference)")
                        {
                            cell.Value = "!(bad reference)"; // sets bad reference if variable in cell is either self referenced or also has a bad reference
                        }
                        else
                        {
                            cell.Value = GetCell(expression).Value;
                        }
                    }
                }
                else
                {
                    bool error = tree.SetVariables(this, var); // set variables based on cell values
                    string name = string.Empty;
                    char column = (char)(cell.ColumnIndex + 65);
                    string row = (cell.RowIndex + 1).ToString();
                    name = name + column;
                    name = name + row;

                    bool refError = IsReferenced(cell.Variables, cell);

                    if (GetCell(name).Variables.Contains(GetCell(name)))
                    {
                        cell.Value = "!(self reference)"; // sets self referenced if cell references itself
                    }
                    else if (refError == true)
                    {
                        cell.Value = "!(circular reference)"; // sets circular reference if one is found
                    }
                    else if (error == true)
                    {
                        cell.Value = "!(bad reference)"; // sets bad reference if one is found
                    }
                    else
                    {
                        cell.Value = tree.Evaluate().ToString(); // evaluate tree and set cell's value
                    }
                }
            }
            else
            {
                // if we changed from an expression containing '=' to one that does not, then remove this cell from all
                // dependency lists of other cells that were previously variables in this cell's expression
                if (cell.PreviousVariables.Count > 0)
                {
                    for (int i = 0; i < cell.PreviousVariables.Count; i++)
                    {
                        cell.PreviousVariables[i].CellDependencies.Remove(cell);
                    }
                }

                cell.Variables = new List<Cell>(); // clear variable list

                cell.Value = cell.Text; // set this cell's value to its text
            }

            if (TextChange != null)
            {
                TextChange(this); // notify spreadsheet graphic of change in cell
            }
        }

        public void CellValueChanged(object sender)
        {
            SpreadsheetCell cell = sender as SpreadsheetCell;
            double parsed;

            // if the value of this cell was changed, and there are other cells that depend on it, and its value is able to be parsed into a double,
            // then update the cells that depended on its value
            if (cell.CellDependencies.Count > 0 && double.TryParse(cell.Value, out parsed) == true)
            {
                Cell updateCell;

                for (int i = 0; i < cell.CellDependencies.Count; i++)
                {
                    updateCell = cell.CellDependencies[i];

                    string expression = updateCell.Text.Substring(1); // grab expression from cell

                    ExpressionTree tree = new ExpressionTree(expression); // create new expression tree based on expression in cell

                    List<string> var = tree.Variables.Keys.ToList<string>(); // create list of variables needing values

                    if (tree.Variables.Count == 1 && expression.Length == var[0].Length)
                    {
                        bool error = IsReferenced(updateCell.Variables, updateCell); // checks for a circular reference

                        if (error == true)
                        {
                            updateCell.Value = "!(self reference)"; // sets self reference if circular reference is returned
                        }
                        else if (GetCell(expression).Value == "!(self reference)" || GetCell(expression).Value == "!(bad reference)")
                        {
                            updateCell.Value = "!(bad reference)"; // sets bad reference if variable in cell has self or bad reference
                        }
                        else
                        {
                            updateCell.Value = GetCell(expression).Value;
                        }
                    }
                    else
                    {
                        bool error = IsReferenced(updateCell.Variables, updateCell);

                        if (error == true)
                        {
                            updateCell.Value = "!(bad reference)"; // sets bad reference if circular reference is found
                        }
                        else
                        {
                            tree.SetVariables(this, var); // set variables based on cell values

                            updateCell.Value = tree.Evaluate().ToString(); // evaluate tree and set cell's value
                        }
                    }

                    alteredCell = updateCell;
                    if (ValueChange != null)
                    {
                        ValueChange(this);
                    }
                }
            }
            else if (cell.CellDependencies.Count > 0 && (cell.Value == "!(bad reference)" || cell.Value == "!(self reference)"))
            {
                for (int i = 0; i < cell.CellDependencies.Count; i++)
                {
                    if (cell.CellDependencies[i] != cell)
                    {
                        cell.CellDependencies[i].Value = "!(bad reference)"; // sets bad reference to all dependent cells if this cell has self or bad reference
                    }
                    else
                    {
                        cell.CellDependencies[i].Value = "!(self reference)"; // sets self reference if cell finds itself in its dependent cells
                    }
                }
            }
            else if (cell.Variables != null && cell.CellDependencies.Count > 0)
            {
                for (int i = 0; i < cell.Variables.Count; i++)
                {
                    if (IsReferenced(cell.Variables[i].Variables, cell.Variables[i]))
                    {
                        cell.Value = "!(circular reference)"; // sets circular reference if found
                    }
                }
            }

            alteredCell = cell;

            if (ValueChange != null)
            {
                ValueChange(this); // notify spreadsheet graphic of change in cell
            }
        }

        // notifies UI of color change
        public void CellColorChanged(object sender)
        {
            Cell cell = sender as Cell;

            alteredCell = cell;

            if (ColorChange != null)
            {
                ColorChange(this); // notify spreadsheet graphic of change in cell
            }
        }

        // changes color of a cell
        public void ChangeCellColor(Cell cell, uint color)
        {
            cell.Color = color;
        }

        // undo changes in all cells that were changed with the last action
        public string Undo()
        {
            if (undoStack.Count > 0)
            {
                List<Cell> list = undoStack[undoStack.Count - 1]; // get list of cells from top of undo stack

                // undo all changes in list
                for (int i = 0; i < list.Count; i++)
                {
                    Tuple<Cell, string> tuple = undoChangeList[undoChangeList.Count - 1];
                    UndoCell(tuple);
                }

                redoStack.Add(list); // copy list from undo stack to redo stack

                undoStack.RemoveAt(undoStack.Count - 1); // remove list from undo stack
            }

            // update tool strip text based on what is now on top of stack
            if (undoStack.Count > 0 && undoChangeList.Count > 0)
            {
                string change = string.Empty;
                change = undoChangeList[undoChangeList.Count - 1].Item2;
                return change;
            }
            else
            {
                return string.Empty;
            }
        }

        // undo a previously made singular cell change, pop from changeList
        public void UndoCell(Tuple<Cell, string> tuple)
        {
            Cell cell = tuple.Item1; // get cell from tuple in changeList

            // get the column and row of the cell
            int column = cell.ColumnIndex;
            int row = cell.RowIndex;

            Cell temp = this.GetCell(column, row); // get the corresponding node in the spreadsheet

            undoRedoCell = temp; // set undoRedoCell to this spreadsheet cell, will help avoid duplications of undos

            Cell saveCell = new SpreadsheetCell(column, row); // create a new cell that will be added to the redo changeList

            // copy the spreadsheet cell's current property values over to this new cell
            saveCell.Color = temp.Color;
            saveCell.Text = temp.Text;

            string change = tuple.Item2; // copy the string from the tuples second value

            AddRedo(saveCell, change); // pass the new cell and string to AddRedo function

            // change the spreadsheet cell's property values based on the tuple cell's values
            temp.Color = cell.Color;
            temp.Text = cell.Text;

            undoChangeList.RemoveAt(undoChangeList.Count - 1); // remove the cell from the undo change list
        }

        // redo an action, pop from stack
        public string Redo()
        {
            if (redoStack.Count > 0)
            {
                List<Cell> list = redoStack[redoStack.Count - 1]; // get list of cells off top of redoStack

                // redo all changes in list
                for (int i = 0; i < list.Count; i++)
                {
                    Tuple<Cell, string> tuple = redoChangeList[redoChangeList.Count - 1];
                    RedoCell(tuple);
                }

                undoStack.Add(list); // push the cell list to the undo stack

                redoStack.RemoveAt(redoStack.Count - 1); // remove list from redo stack
            }

            // update tool strip text based on what is now on top of stack
            if (redoStack.Count > 0)
            {
                string change = string.Empty;
                change = redoChangeList[redoChangeList.Count - 1].Item2;
                return change;
            }
            else
            {
                return string.Empty;
            }
        }

        // redo a change to a cell
        public void RedoCell(Tuple<Cell, string> tuple)
        {
            Cell cell = tuple.Item1; // get cell from tuple

            // copy column and row of cell
            int column = cell.ColumnIndex;
            int row = cell.RowIndex;

            Cell temp = this.GetCell(column, row); // get the corresponding spreadsheet cell

            // copy property values from tuple cell to spreadsheet cell,
            // an undo is automatically created due to undo event firing when the cell's property values are changed
            temp.Color = cell.Color;
            temp.Text = cell.Text;

            redoChangeList.RemoveAt(redoChangeList.Count - 1); // remove cell from redo change list
        }

        // push undo to change list
        public void AddUndo(object sender, string change)
        {
            Cell copy = sender as Cell;

            // the three conditions that allow for a push are as follows:
            // if list is empty or the cell attempting to push is not the same as undoRedoCell or the cell is a different cell entirely,
            // then push to list
            if (undoStack.Count == 0 || undoRedoCell != copy || (copy.RowIndex != undoChangeList[undoChangeList.Count - 1].Item1.RowIndex || copy.ColumnIndex != undoChangeList[undoChangeList.Count - 1].Item1.ColumnIndex))
            {
                // copy column and row from sender cell
                int column = copy.ColumnIndex;
                int row = copy.RowIndex;

                Cell temp = new SpreadsheetCell(column, row); // get corresponding cell from spreadsheet

                // copy property values from sender cell
                temp.Color = copy.Color;
                temp.Text = copy.Text;

                string menuChange = string.Empty;

                // set string to be passed to tuple
                if (change == "text" || change == "color")
                {
                    menuChange = change;
                }
                else
                {
                    menuChange = "cell";
                }

                // create new tuple with created cell and string
                Tuple<Cell, string> stackValue;
                stackValue = new Tuple<Cell, string>(temp, menuChange);

                undoChangeList.Add(stackValue); // push tuple to undo change list

                // only pushes to undo stack if the change is a text change
                // colors are done elsewhere due to needing to allow for group changes
                if (change == "text")
                {
                    List<Cell> list = new List<Cell>();
                    list.Add(copy);
                    undoStack.Add(list);
                }
            }

            string cellChange = undoChangeList[undoChangeList.Count - 1].Item2; // get string from pushed tuple

            OnAddUndo(this, cellChange); // fire event that notifies UI of change

            undoRedoCell = null; // reset undoRedoCell to null
        }

        // push change to stack
        public void AddRedo(object sender, string change)
        {
            Cell cell = sender as SpreadsheetCell;

            // copy column and row from sender cell
            int column = cell.ColumnIndex;
            int row = cell.RowIndex;

            Cell temp = new SpreadsheetCell(column, row); // create new cell to be used in tuple

            // copy property values from sender cell to tuple cell
            temp.Color = cell.Color;
            temp.Text = cell.Text;

            Tuple<Cell, string> tuple = new Tuple<Cell, string>(temp, change); // create new tuple

            redoChangeList.Add(tuple); // push tuple to redo change list

            OnAddRedo(sender, change); // fire event that notifies UI of change
        }

        // adds a list of cells to the undo stack
        public void AddtoUndoStack(List<Cell> list)
        {
            undoStack.Add(list);
        }

        // following two functions notify UI of list change so that it may update the names of its menu options
        public void OnAddRedo(object sender, string change)
        {
            if (AlertOfAddRedo != null)
            {
                AlertOfAddRedo(this, change);
            }
        }

        public void OnAddUndo(object sender, string change)
        {
            if (AlertOfAddUndo != null)
            {
                AlertOfAddUndo(this, change);
            }
        }

        // saves spreadsheet to .xml file
        public bool SaveFile(string filename)
        {
            // create xml document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<spreadsheet></spreadsheet>");

            // check to see if cells contain text or if their color has changed
            // if they do then write this cell to the xml file
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    if (sheet[i, j].Color != 0xFFFFFFFF | sheet[i, j].Text != null)
                    {
                        // get cell from spreadsheet
                        Cell sheetCell = sheet[i, j];

                        XmlElement cell = doc.CreateElement("cell"); // create new cell node
                        cell.SetAttribute("column", sheetCell.ColumnIndex.ToString()); // set column attribute of cell node
                        cell.SetAttribute("row", sheetCell.RowIndex.ToString()); // set row attribute of cell node

                        XmlElement text = doc.CreateElement("text"); // create text node
                        text.InnerText = sheetCell.Text; // set value in text node
                        cell.AppendChild(text); // put text node in cell node

                        XmlElement color = doc.CreateElement("color"); // create color node
                        color.InnerText = sheetCell.Color.ToString("X2"); // set value in color node
                        cell.AppendChild(color); // put color node in cell node

                        doc.DocumentElement.AppendChild(cell); // close cell node
                    }
                }
            }

            doc.Save(filename); // save xml nodes to file

            return true;
        }

        // load spreadsheet from .xml file
        public void LoadFile(string filename)
        {
            // clear and reinitialize spreadsheet
            int x = columnCount;
            int y = rowCount;
            sheet = null;
            sheet = new SpreadsheetCell[26, 50];

            // fill in spreadsheet with cell objects
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    sheet[i, j] = new SpreadsheetCell(i, j); // create new cell
                    sheet[i, j].TextChanged += CellTextChanged; // subscribe to this cell's text event notification
                    sheet[i, j].ValueChanged += CellValueChanged; // subscribe to this cell's value event notification
                    sheet[i, j].ColorChanged += CellColorChanged; // subscribe to this cell's color event notification
                    sheet[i, j].Undo += AddUndo; // subscribe to this cell's undo event notification
                }
            }

            // load from file
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // create lists of nodes that are needed
            XmlNodeList cells = doc.DocumentElement.GetElementsByTagName("cell");
            XmlNodeList cellText = doc.DocumentElement.GetElementsByTagName("text");
            XmlNodeList cellColor = doc.DocumentElement.GetElementsByTagName("color");

            int k = 0;

            // for each cell node, input the appropriate data
            foreach (XmlNode xmlNode in cells)
            {
                int column = int.Parse(xmlNode.Attributes["column"].Value);
                int row = int.Parse(xmlNode.Attributes["row"].Value);

                Cell cell = sheet[column, row];

                string text = cellText[k].InnerText;
                uint color = (uint)Convert.ToInt32(cellColor[k].InnerText, 16);

                cell.Text = text;
                cell.Color = color;

                k++;
            }

            // clear and reinitialize undo/redo stacks
            undoStack = null;
            undoChangeList = null;
            redoStack = null;
            redoChangeList = null;
            undoStack = new List<List<Cell>>();
            undoChangeList = new List<Tuple<Cell, string>>();
            redoStack = new List<List<Cell>>();
            redoChangeList = new List<Tuple<Cell, string>>();
        }

        public bool IsReferenced(List<Cell> variables, Cell reference)
        {
            if (variables != null)
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    Cell cell = variables[i];

                    if (cell.Variables != null)
                    {
                        if (cell.Variables.Contains(reference))
                        {
                            return true;
                        }
                        else
                        {
                            return IsReferenced(cell.Variables, reference);
                        }
                    }
                }
            }

            return false;
        }

        // carries out demo
        public void Demo()
        {
            Random rand = new Random();

            int column = 0;
            int row = 0;

            for (int i = 0; i < 50; i++)
            {
                // sets all column B values to "This is cell B#"
                GetCell(1, i).Text = "This is cell B" + (i + 1);

                // get random cell
                column = rand.Next(0, 26);
                while (column == 1)
                {
                    column = rand.Next(0, 26);
                }

                row = rand.Next(0, 50);

                GetCell(column, row).Text = "H3110 W0R1D!"; // set random cell to "H3110 W0R1D!"

                GetCell(0, i).Text = "=B" + (i + 1); // set all column A values to the corresponding values in column B
            }
        }
    }
}
