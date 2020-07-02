namespace Spreadsheet_Alex_Strawn
{
    partial class Spreadsheet_Alex_Strawn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeCellColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.carryOutDemoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(800, 417);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellEndEdit);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.fileToolStripMenuItem,
            this.cellToolStripMenuItem,
            this.demoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 33);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveDocumentToolStripMenuItem,
            this.loadDocumentToolStripMenuItem,
            this.demoToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // saveDocumentToolStripMenuItem
            // 
            this.saveDocumentToolStripMenuItem.Name = "saveDocumentToolStripMenuItem";
            this.saveDocumentToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.saveDocumentToolStripMenuItem.Text = "Save Document";
            this.saveDocumentToolStripMenuItem.Click += new System.EventHandler(this.saveDocumentToolStripMenuItem_Click);
            // 
            // loadDocumentToolStripMenuItem
            // 
            this.loadDocumentToolStripMenuItem.Name = "loadDocumentToolStripMenuItem";
            this.loadDocumentToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.loadDocumentToolStripMenuItem.Text = "Load Document";
            this.loadDocumentToolStripMenuItem.Click += new System.EventHandler(this.loadDocumentToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            this.fileToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(250, 34);
            this.undoToolStripMenuItem.Text = "Nothing to Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(250, 34);
            this.redoToolStripMenuItem.Text = "Nothing to Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // cellToolStripMenuItem
            // 
            this.cellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeCellColorToolStripMenuItem});
            this.cellToolStripMenuItem.Name = "cellToolStripMenuItem";
            this.cellToolStripMenuItem.Size = new System.Drawing.Size(56, 29);
            this.cellToolStripMenuItem.Text = "Cell";
            // 
            // changeCellColorToolStripMenuItem
            // 
            this.changeCellColorToolStripMenuItem.Name = "changeCellColorToolStripMenuItem";
            this.changeCellColorToolStripMenuItem.Size = new System.Drawing.Size(255, 34);
            this.changeCellColorToolStripMenuItem.Text = "Change Cell Color";
            this.changeCellColorToolStripMenuItem.Click += new System.EventHandler(this.ChangeCellColorToolStripMenuItem_Click);
            // 
            // demoToolStripMenuItem
            // 
            this.demoToolStripMenuItem.Name = "demoToolStripMenuItem";
            this.demoToolStripMenuItem.Size = new System.Drawing.Size(16, 29);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // carryOutDemoToolStripMenuItem
            // 
            this.carryOutDemoToolStripMenuItem.Name = "carryOutDemoToolStripMenuItem";
            this.carryOutDemoToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.carryOutDemoToolStripMenuItem.Text = "Carry out demo";
            // 
            // demoToolStripMenuItem1
            // 
            this.demoToolStripMenuItem1.Name = "demoToolStripMenuItem1";
            this.demoToolStripMenuItem1.Size = new System.Drawing.Size(270, 34);
            this.demoToolStripMenuItem1.Text = "Demo";
            this.demoToolStripMenuItem1.Click += new System.EventHandler(this.demoToolStripMenuItem1_Click);
            // 
            // Spreadsheet_Alex_Strawn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Spreadsheet_Alex_Strawn";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeCellColorToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDocumentToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem demoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem carryOutDemoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demoToolStripMenuItem1;
    }
}

