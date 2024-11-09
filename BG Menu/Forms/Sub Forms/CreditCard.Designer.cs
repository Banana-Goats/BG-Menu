namespace BG_Menu.Forms.Sub_Forms
{
    partial class CreditCard
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            Description = new DataGridViewTextBoxColumn();
            GLAccount = new DataGridViewComboBoxColumn();
            Dimension = new DataGridViewTextBoxColumn();
            GLName = new DataGridViewTextBoxColumn();
            VAT = new DataGridViewComboBoxColumn();
            Total = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            InvoiceImported = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            button3 = new Button();
            button2 = new Button();
            comboBoxFiles = new ComboBox();
            button1 = new Button();
            btnExport = new Button();
            TotalValue = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Description, GLAccount, Dimension, GLName, VAT, Total, Value, InvoiceImported });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = SystemColors.Control;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = Color.White;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 35);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(989, 573);
            dataGridView1.TabIndex = 0;
            // 
            // Description
            // 
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(46, 51, 73);
            Description.DefaultCellStyle = dataGridViewCellStyle2;
            Description.HeaderText = "Description";
            Description.Name = "Description";
            // 
            // GLAccount
            // 
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(46, 51, 73);
            GLAccount.DefaultCellStyle = dataGridViewCellStyle3;
            GLAccount.HeaderText = "GL Account";
            GLAccount.Items.AddRange(new object[] { "640025", "605065" });
            GLAccount.Name = "GLAccount";
            // 
            // Dimension
            // 
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(46, 51, 73);
            Dimension.DefaultCellStyle = dataGridViewCellStyle4;
            Dimension.HeaderText = "Dimension";
            Dimension.Name = "Dimension";
            // 
            // GLName
            // 
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(46, 51, 73);
            GLName.DefaultCellStyle = dataGridViewCellStyle5;
            GLName.HeaderText = "GL Name";
            GLName.Name = "GLName";
            GLName.ReadOnly = true;
            // 
            // VAT
            // 
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(46, 51, 73);
            VAT.DefaultCellStyle = dataGridViewCellStyle6;
            VAT.HeaderText = "VAT Code";
            VAT.Items.AddRange(new object[] { "I1", "I2" });
            VAT.Name = "VAT";
            // 
            // Total
            // 
            dataGridViewCellStyle7.ForeColor = Color.FromArgb(46, 51, 73);
            Total.DefaultCellStyle = dataGridViewCellStyle7;
            Total.HeaderText = "Total";
            Total.Name = "Total";
            // 
            // Value
            // 
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(46, 51, 73);
            Value.DefaultCellStyle = dataGridViewCellStyle8;
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            // 
            // InvoiceImported
            // 
            dataGridViewCellStyle9.ForeColor = Color.Black;
            InvoiceImported.DefaultCellStyle = dataGridViewCellStyle9;
            InvoiceImported.HeaderText = "Invoice Imported";
            InvoiceImported.Name = "InvoiceImported";
            InvoiceImported.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(comboBoxFiles);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnExport);
            panel1.Controls.Add(TotalValue);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(989, 35);
            panel1.TabIndex = 1;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Left;
            button3.Location = new Point(422, 0);
            button3.Name = "button3";
            button3.Size = new Size(82, 35);
            button3.TabIndex = 6;
            button3.Text = "Import";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Left;
            button2.Location = new Point(340, 0);
            button2.Name = "button2";
            button2.Size = new Size(82, 35);
            button2.TabIndex = 5;
            button2.Text = "Combine";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBoxFiles
            // 
            comboBoxFiles.Dock = DockStyle.Left;
            comboBoxFiles.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBoxFiles.FormattingEnabled = true;
            comboBoxFiles.Location = new Point(157, 0);
            comboBoxFiles.Name = "comboBoxFiles";
            comboBoxFiles.Size = new Size(183, 33);
            comboBoxFiles.TabIndex = 3;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Left;
            button1.Location = new Point(75, 0);
            button1.Name = "button1";
            button1.Size = new Size(82, 35);
            button1.TabIndex = 4;
            button1.Text = "Upload PDF";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnExport
            // 
            btnExport.Dock = DockStyle.Left;
            btnExport.Location = new Point(0, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 35);
            btnExport.TabIndex = 2;
            btnExport.Text = "Save";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // TotalValue
            // 
            TotalValue.BackColor = Color.FromArgb(46, 51, 73);
            TotalValue.Dock = DockStyle.Right;
            TotalValue.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TotalValue.ForeColor = SystemColors.Window;
            TotalValue.Location = new Point(889, 0);
            TotalValue.Name = "TotalValue";
            TotalValue.Size = new Size(100, 35);
            TotalValue.TabIndex = 0;
            TotalValue.TextAlign = HorizontalAlignment.Center;
            // 
            // CreditCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "CreditCard";
            Text = "CreditCard";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel panel1;
        private TextBox TotalValue;
        private Button btnExport;
        private ComboBox comboBoxFiles;
        private Button button1;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewComboBoxColumn GLAccount;
        private DataGridViewTextBoxColumn Dimension;
        private DataGridViewTextBoxColumn GLName;
        private DataGridViewComboBoxColumn VAT;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewTextBoxColumn Value;
        private DataGridViewTextBoxColumn InvoiceImported;
        private Button button2;
        private Button button3;
    }
}