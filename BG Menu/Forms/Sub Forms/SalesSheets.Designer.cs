namespace BG_Menu.Forms.Sub_Forms
{
    partial class SalesSheets
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            btnLoadMaster = new Button();
            btnSelectFolder = new Button();
            txtFolderPath = new TextBox();
            txtCopyRange = new TextBox();
            txtPasteRange = new TextBox();
            btnExecuteCopyPaste = new Button();
            lblStatus = new Label();
            dataGridViewMappings = new DataGridView();
            btnCreateStoreFiles = new Button();
            txtCopyRange2 = new TextBox();
            txtPasteRange2 = new TextBox();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMappings).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnLoadMaster
            // 
            btnLoadMaster.BackColor = Color.FromArgb(46, 51, 73);
            btnLoadMaster.Dock = DockStyle.Top;
            btnLoadMaster.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLoadMaster.ForeColor = SystemColors.ControlLightLight;
            btnLoadMaster.Location = new Point(0, 79);
            btnLoadMaster.Name = "btnLoadMaster";
            btnLoadMaster.Size = new Size(181, 30);
            btnLoadMaster.TabIndex = 0;
            btnLoadMaster.Text = "Load Master Spreadsheet";
            btnLoadMaster.UseVisualStyleBackColor = false;
            btnLoadMaster.Click += btnLoadMaster_Click;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.BackColor = Color.FromArgb(46, 51, 73);
            btnSelectFolder.Dock = DockStyle.Top;
            btnSelectFolder.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSelectFolder.ForeColor = SystemColors.ControlLightLight;
            btnSelectFolder.Location = new Point(0, 0);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(181, 49);
            btnSelectFolder.TabIndex = 2;
            btnSelectFolder.Text = "Select Folder for Store Spreadsheets";
            btnSelectFolder.UseVisualStyleBackColor = false;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.BackColor = Color.FromArgb(46, 51, 73);
            txtFolderPath.Dock = DockStyle.Left;
            txtFolderPath.ForeColor = SystemColors.Window;
            txtFolderPath.Location = new Point(0, 0);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.ReadOnly = true;
            txtFolderPath.Size = new Size(339, 23);
            txtFolderPath.TabIndex = 3;
            // 
            // txtCopyRange
            // 
            txtCopyRange.Location = new Point(3, 3);
            txtCopyRange.Name = "txtCopyRange";
            txtCopyRange.PlaceholderText = "PY Sales Copy";
            txtCopyRange.Size = new Size(84, 23);
            txtCopyRange.TabIndex = 5;
            txtCopyRange.Text = "N8:Q82";
            txtCopyRange.TextAlign = HorizontalAlignment.Center;
            // 
            // txtPasteRange
            // 
            txtPasteRange.Location = new Point(3, 32);
            txtPasteRange.Name = "txtPasteRange";
            txtPasteRange.PlaceholderText = "PY Sales Paste";
            txtPasteRange.Size = new Size(84, 23);
            txtPasteRange.TabIndex = 6;
            txtPasteRange.Text = "N8:Q82";
            txtPasteRange.TextAlign = HorizontalAlignment.Center;
            // 
            // btnExecuteCopyPaste
            // 
            btnExecuteCopyPaste.BackColor = Color.FromArgb(46, 51, 73);
            btnExecuteCopyPaste.Dock = DockStyle.Top;
            btnExecuteCopyPaste.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExecuteCopyPaste.ForeColor = SystemColors.ControlLightLight;
            btnExecuteCopyPaste.Location = new Point(0, 109);
            btnExecuteCopyPaste.Name = "btnExecuteCopyPaste";
            btnExecuteCopyPaste.Size = new Size(181, 30);
            btnExecuteCopyPaste.TabIndex = 7;
            btnExecuteCopyPaste.Text = "Execute Copy-Paste";
            btnExecuteCopyPaste.UseVisualStyleBackColor = false;
            btnExecuteCopyPaste.Click += btnExecuteCopyPaste_Click;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Left;
            lblStatus.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = SystemColors.Control;
            lblStatus.Location = new Point(339, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(52, 23);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "lblStatus";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dataGridViewMappings
            // 
            dataGridViewMappings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewMappings.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewMappings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewMappings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewMappings.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewMappings.Dock = DockStyle.Fill;
            dataGridViewMappings.GridColor = SystemColors.Window;
            dataGridViewMappings.Location = new Point(181, 23);
            dataGridViewMappings.Name = "dataGridViewMappings";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.Window;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridViewMappings.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewMappings.RowHeadersVisible = false;
            dataGridViewMappings.Size = new Size(792, 546);
            dataGridViewMappings.TabIndex = 9;
            dataGridViewMappings.CellValueChanged += dataGridViewMappings_CellValueChanged;
            // 
            // btnCreateStoreFiles
            // 
            btnCreateStoreFiles.BackColor = Color.FromArgb(46, 51, 73);
            btnCreateStoreFiles.Dock = DockStyle.Top;
            btnCreateStoreFiles.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCreateStoreFiles.ForeColor = SystemColors.ControlLightLight;
            btnCreateStoreFiles.Location = new Point(0, 49);
            btnCreateStoreFiles.Name = "btnCreateStoreFiles";
            btnCreateStoreFiles.Size = new Size(181, 30);
            btnCreateStoreFiles.TabIndex = 10;
            btnCreateStoreFiles.Text = "Create Store Files";
            btnCreateStoreFiles.UseVisualStyleBackColor = false;
            btnCreateStoreFiles.Click += btnCreateStoreFiles_Click;
            // 
            // txtCopyRange2
            // 
            txtCopyRange2.Location = new Point(93, 3);
            txtCopyRange2.Name = "txtCopyRange2";
            txtCopyRange2.PlaceholderText = "Budgets Copy";
            txtCopyRange2.Size = new Size(85, 23);
            txtCopyRange2.TabIndex = 11;
            txtCopyRange2.Text = "U8:U82";
            txtCopyRange2.TextAlign = HorizontalAlignment.Center;
            // 
            // txtPasteRange2
            // 
            txtPasteRange2.Location = new Point(93, 32);
            txtPasteRange2.Name = "txtPasteRange2";
            txtPasteRange2.PlaceholderText = "Budgets Paste";
            txtPasteRange2.Size = new Size(85, 23);
            txtPasteRange2.TabIndex = 12;
            txtPasteRange2.Text = "S8:S82";
            txtPasteRange2.TextAlign = HorizontalAlignment.Center;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnExecuteCopyPaste);
            panel1.Controls.Add(btnLoadMaster);
            panel1.Controls.Add(btnCreateStoreFiles);
            panel1.Controls.Add(btnSelectFolder);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(181, 569);
            panel1.TabIndex = 13;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(txtCopyRange, 0, 0);
            tableLayoutPanel1.Controls.Add(txtPasteRange2, 1, 1);
            tableLayoutPanel1.Controls.Add(txtCopyRange2, 1, 0);
            tableLayoutPanel1.Controls.Add(txtPasteRange, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 511);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(181, 58);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblStatus);
            panel2.Controls.Add(txtFolderPath);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(181, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(792, 23);
            panel2.TabIndex = 14;
            // 
            // SalesSheets
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(973, 569);
            Controls.Add(dataGridViewMappings);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SalesSheets";
            Text = "Store Sales Sheet Builder";
            ((System.ComponentModel.ISupportInitialize)dataGridViewMappings).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoadMaster;
        private Button btnSelectFolder;
        private TextBox txtFolderPath;
        private TextBox txtCopyRange;
        private TextBox txtPasteRange;
        private Button btnExecuteCopyPaste;
        private Label lblStatus;
        private DataGridView dataGridViewMappings;
        private Button btnCreateStoreFiles;
        private TextBox txtCopyRange2;
        private TextBox txtPasteRange2;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
    }
}
