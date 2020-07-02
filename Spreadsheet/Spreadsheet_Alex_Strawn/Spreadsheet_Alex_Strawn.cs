// Alex Strawn
// 11632677

namespace Spreadsheet_Alex_Strawn
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using CptS321;

    public partial class Spreadsheet_Alex_Strawn : Form
    {
        private Spreadsheet sheet;

        public Spreadsheet_Alex_Strawn()
        {
            InitializeComponent();

            // initializes columns in spreadsheet graphic
            for (int i = 65; i < 91; i++)
            {
                char k = (char)i;
                dataGridView1.Columns.Add(k.ToString(), k.ToString());
            }

            // initializes rows in spreadsheet graphic
            for (int j = 0; j < 50; j++)
            {
                dataGridView1.Rows.Add(); // adds row
                dataGridView1.Rows[j].HeaderCell.Value = (j + 1).ToString(); // sets row's name
            }

            // creates new spreadsheet object
            sheet = new Spreadsheet(26, 50);

            // subscribes to event notification of spreadsheet
            sheet.TextChange += Sheet_PropertyChanged;
            sheet.ValueChange += Sheet_PropertyChanged;
            sheet.ColorChange += CellColorChanged;
            sheet.AlertOfAddUndo += ChangeUndoMenuText;
            sheet.AlertOfAddRedo += ChangeRedoMenuText;

            BeginCellEdit += HandleBeginCellEdit;
            EndCellEdit += HandleEndCellEdit;
        }

        public delegate void BeginCellEditEventHandler(object sender, EventArgs e);

        public delegate void EndCellEditEventHandler(object sender, EventArgs e);

        public event BeginCellEditEventHandler BeginCellEdit;

        public event EndCellEditEventHandler EndCellEdit;

        public Spreadsheet Sheet
        {
            get { return sheet; }
            private set { sheet = value; }
        }

        // updates spreadsheet graphic when spreadsheet event triggers due to cell values changing
        protected void Sheet_PropertyChanged(object sender)
        {
            Spreadsheet spreadsheet = sender as Spreadsheet;

            // gets column and row values of cell that was changed
            int column = spreadsheet.AlteredCell.ColumnIndex;
            int row = spreadsheet.AlteredCell.RowIndex;

            // updates spreadsheet graphic to reflect changes
            this.dataGridView1[column, row].Value = spreadsheet.AlteredCell.Value;
            object num = this.dataGridView1[column, row].Value;
            dataGridView1.Refresh();
        }

        // updates UI with new cell color
        protected void CellColorChanged(object sender)
        {
            Spreadsheet spreadsheet = sender as Spreadsheet;

            uint color = spreadsheet.AlteredCell.Color;

            int column = spreadsheet.AlteredCell.ColumnIndex;
            int row = spreadsheet.AlteredCell.RowIndex;

            Color cellColor = Color.FromArgb((int)color);

            dataGridView1.Rows[row].Cells[column].Style.BackColor = cellColor;
            dataGridView1.Refresh();
        }

        // handling begin cell edit event
        private void HandleBeginCellEdit(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            DataGridViewCell cell = grid.CurrentCell;
            int column = cell.ColumnIndex;
            int row = cell.RowIndex;

            Cell sheetCell = sheet.GetCell(column, row);

            this.dataGridView1[column, row].Value = sheetCell.Text; // update value in grid cell with text of spreadsheet cell
            dataGridView1.Refresh();
        }

        // handling end cell edit event
        private void HandleEndCellEdit(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            DataGridViewCell cell = grid.CurrentCell;
            int column = cell.ColumnIndex;
            int row = cell.RowIndex;

            Cell sheetCell = sheet.GetCell(column, row);

            // if the grid cell is not blank, then update the spreadsheet cell's text with this value
            // and set the grid cell value to the spreadsheet cell value
            // otherwise, set both the spreadsheet cell text and the grid cell value to null/blank
            if (cell.Value != null)
            {
                sheetCell.Text = this.dataGridView1[column, row].Value.ToString();
                this.dataGridView1[column, row].Value = sheetCell.Value;
            }
            else
            {
                sheetCell.Text = string.Empty;
                this.dataGridView1[column, row].Value = null;
            }

            dataGridView1.Refresh();
        }

        // allows updating of spreadsheet cell color based on dialog input
        private void ChangeCellColor(DataGridViewCell gridCell, Color color)
        {
            int column = gridCell.ColumnIndex;
            int row = gridCell.RowIndex;

            Cell cell = sheet.GetCell(column, row);
            int newColor = color.ToArgb();
            sheet.ChangeCellColor(cell, (uint)newColor);
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            BeginCellEdit(sender, e);
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            EndCellEdit(sender, e);
        }

        private void ChangeCellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            DataGridViewSelectedCellCollection cellCollection = dataGridView1.SelectedCells;
            IEnumerable<DataGridViewCell> cellList = cellCollection.Cast<DataGridViewCell>();
            List<DataGridViewCell> dgvcList = cellList.ToList();
            List<Cell> undoList = new List<Cell>();

            // create list of cells that are being changed
            for (int j = 0; j < dgvcList.Count; j++)
            {
                int column = dgvcList[j].ColumnIndex;
                int row = dgvcList[j].RowIndex;
                undoList.Add(sheet.GetCell(column, row));
            }

            // push list to undo stack
            sheet.AddtoUndoStack(undoList);

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color newColor = colorDialog.Color;

                // change color of selected cells based on dialog input
                for (int i = 0; i < dgvcList.Count; i++)
                {
                    DataGridViewCell cell = dgvcList[i];
                    ChangeCellColor(cell, newColor);
                }
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string change = sheet.Undo(); // undoes previous change

            // changes menu item text
            if (change != string.Empty)
            {
                item.Text = "Undo cell " + change + " change";
            }
            else
            {
                item.Text = "Nothing to Undo";
            }
        }

        // updates menu item text
        private void ChangeUndoMenuText(object sender, string change)
        {
            undoToolStripMenuItem.Text = "Undo cell " + change + " change";
        }


        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string change = sheet.Redo(); // redoes previously undone change

            // changes menu item text
            if (change != string.Empty)
            {
                item.Text = "Redo cell " + change + " change";
            }
            else
            {
                item.Text = "Nothing to Redo";
            }
        }

        // updates menu item text
        private void ChangeRedoMenuText(object sender, string change)
        {
            redoToolStripMenuItem.Text = "Redo cell " + change + " change";
        }

        private void saveDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML-File (.xml) | *.xml"; // restrict to .xml files
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string savename = saveFileDialog1.FileName; // get save name of file
                sheet.SaveFile(savename); // save spreadsheet to file
            }
        }

        private void loadDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML-File (.xml) | *.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // clear spreadsheet UI data before load
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = null;
                        uint color = 0xFFFFFFFF;
                        dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb((int)color);
                    }
                }

                string savename = openFileDialog1.FileName; // get name of file to be loaded
                sheet.LoadFile(savename); // loaded file into spreadsheet
            }

            // makes sure undo/redo menu items text are reset
            undoToolStripMenuItem.Text = "Nothing to Undo";
            redoToolStripMenuItem.Text = "Nothing to Redo";
        }

        private void demoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // carry out demo on button click
            sheet.Demo();
            dataGridView1.Refresh();
        }
    }
}
