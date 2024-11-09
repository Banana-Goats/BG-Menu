namespace BG_Menu.Forms.Sales_Summary
{
    partial class SalesSummary
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            dataGridViewUKStores = new DataGridView();
            dataGridViewFranchiseStores = new DataGridView();
            dataGridViewCompanyStores = new DataGridView();
            comboBox1 = new ComboBox();
            chkShow2022 = new CheckBox();
            chkShow2021 = new CheckBox();
            chkShow2020 = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            lblStoresOverTarget = new Label();
            lblStoresOverProgress = new Label();
            lblCurrentWeek = new Label();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            listBoxWeeks = new Class.Design.Custom_Items.CenteredListBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            chkShow2023 = new CheckBox();
            button3 = new Button();
            button4 = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUKStores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFranchiseStores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompanyStores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewUKStores
            // 
            dataGridViewUKStores.AllowUserToAddRows = false;
            dataGridViewUKStores.AllowUserToDeleteRows = false;
            dataGridViewUKStores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUKStores.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewUKStores.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewUKStores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewUKStores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewUKStores.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewUKStores.Dock = DockStyle.Fill;
            dataGridViewUKStores.Location = new Point(0, 0);
            dataGridViewUKStores.Margin = new Padding(2);
            dataGridViewUKStores.Name = "dataGridViewUKStores";
            dataGridViewUKStores.ReadOnly = true;
            dataGridViewUKStores.RowHeadersVisible = false;
            dataGridViewUKStores.RowHeadersWidth = 62;
            dataGridViewUKStores.Size = new Size(647, 784);
            dataGridViewUKStores.TabIndex = 4;
            // 
            // dataGridViewFranchiseStores
            // 
            dataGridViewFranchiseStores.AllowUserToAddRows = false;
            dataGridViewFranchiseStores.AllowUserToDeleteRows = false;
            dataGridViewFranchiseStores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFranchiseStores.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewFranchiseStores.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridViewFranchiseStores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewFranchiseStores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dataGridViewFranchiseStores.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewFranchiseStores.Dock = DockStyle.Fill;
            dataGridViewFranchiseStores.Location = new Point(0, 0);
            dataGridViewFranchiseStores.Margin = new Padding(2);
            dataGridViewFranchiseStores.Name = "dataGridViewFranchiseStores";
            dataGridViewFranchiseStores.ReadOnly = true;
            dataGridViewFranchiseStores.RowHeadersVisible = false;
            dataGridViewFranchiseStores.RowHeadersWidth = 62;
            dataGridViewFranchiseStores.Size = new Size(707, 421);
            dataGridViewFranchiseStores.TabIndex = 5;
            // 
            // dataGridViewCompanyStores
            // 
            dataGridViewCompanyStores.AllowUserToAddRows = false;
            dataGridViewCompanyStores.AllowUserToDeleteRows = false;
            dataGridViewCompanyStores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCompanyStores.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCompanyStores.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridViewCompanyStores.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCompanyStores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dataGridViewCompanyStores.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCompanyStores.Dock = DockStyle.Fill;
            dataGridViewCompanyStores.Location = new Point(0, 0);
            dataGridViewCompanyStores.Margin = new Padding(2);
            dataGridViewCompanyStores.Name = "dataGridViewCompanyStores";
            dataGridViewCompanyStores.ReadOnly = true;
            dataGridViewCompanyStores.RowHeadersVisible = false;
            dataGridViewCompanyStores.RowHeadersWidth = 62;
            dataGridViewCompanyStores.Size = new Size(707, 359);
            dataGridViewCompanyStores.TabIndex = 6;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = SystemColors.Control;
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("Microsoft Sans Serif", 13.25F, FontStyle.Bold);
            comboBox1.ForeColor = Color.Black;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(194, 30);
            comboBox1.TabIndex = 9;
            comboBox1.Text = "Month Selection";
            // 
            // chkShow2022
            // 
            chkShow2022.AutoSize = true;
            chkShow2022.CheckAlign = ContentAlignment.BottomCenter;
            chkShow2022.Dock = DockStyle.Fill;
            chkShow2022.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            chkShow2022.ForeColor = Color.Black;
            chkShow2022.Location = new Point(373, 3);
            chkShow2022.Name = "chkShow2022";
            chkShow2022.RightToLeft = RightToLeft.No;
            chkShow2022.Size = new Size(44, 30);
            chkShow2022.TabIndex = 12;
            chkShow2022.Text = "2022";
            chkShow2022.TextAlign = ContentAlignment.MiddleCenter;
            chkShow2022.UseVisualStyleBackColor = true;
            chkShow2022.CheckedChanged += PreviousYearChange;
            // 
            // chkShow2021
            // 
            chkShow2021.AutoSize = true;
            chkShow2021.CheckAlign = ContentAlignment.BottomCenter;
            chkShow2021.Dock = DockStyle.Fill;
            chkShow2021.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            chkShow2021.ForeColor = Color.Black;
            chkShow2021.Location = new Point(423, 3);
            chkShow2021.Name = "chkShow2021";
            chkShow2021.RightToLeft = RightToLeft.No;
            chkShow2021.Size = new Size(44, 30);
            chkShow2021.TabIndex = 13;
            chkShow2021.Text = "2021";
            chkShow2021.TextAlign = ContentAlignment.MiddleCenter;
            chkShow2021.UseVisualStyleBackColor = true;
            chkShow2021.CheckedChanged += PreviousYearChange;
            // 
            // chkShow2020
            // 
            chkShow2020.AutoSize = true;
            chkShow2020.CheckAlign = ContentAlignment.BottomCenter;
            chkShow2020.Dock = DockStyle.Fill;
            chkShow2020.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            chkShow2020.ForeColor = Color.Black;
            chkShow2020.Location = new Point(473, 3);
            chkShow2020.Name = "chkShow2020";
            chkShow2020.RightToLeft = RightToLeft.No;
            chkShow2020.Size = new Size(44, 30);
            chkShow2020.TabIndex = 14;
            chkShow2020.Text = "2020";
            chkShow2020.TextAlign = ContentAlignment.MiddleCenter;
            chkShow2020.UseVisualStyleBackColor = true;
            chkShow2020.CheckedChanged += PreviousYearChange;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Control;
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(202, 2);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(116, 32);
            button1.TabIndex = 19;
            button1.Text = "Load Sales";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            button2.Location = new Point(1320, 2);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(36, 32);
            button2.TabIndex = 20;
            button2.Text = "%";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lblStoresOverTarget
            // 
            lblStoresOverTarget.AutoSize = true;
            lblStoresOverTarget.Dock = DockStyle.Fill;
            lblStoresOverTarget.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblStoresOverTarget.ForeColor = Color.Black;
            lblStoresOverTarget.Location = new Point(3, 0);
            lblStoresOverTarget.Name = "lblStoresOverTarget";
            lblStoresOverTarget.Size = new Size(198, 15);
            lblStoresOverTarget.TabIndex = 21;
            lblStoresOverTarget.Text = "-";
            lblStoresOverTarget.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStoresOverProgress
            // 
            lblStoresOverProgress.AutoSize = true;
            lblStoresOverProgress.Dock = DockStyle.Fill;
            lblStoresOverProgress.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblStoresOverProgress.ForeColor = Color.Black;
            lblStoresOverProgress.Location = new Point(3, 15);
            lblStoresOverProgress.Name = "lblStoresOverProgress";
            lblStoresOverProgress.Size = new Size(198, 15);
            lblStoresOverProgress.TabIndex = 22;
            lblStoresOverProgress.Text = "-";
            lblStoresOverProgress.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCurrentWeek
            // 
            lblCurrentWeek.AutoSize = true;
            lblCurrentWeek.Dock = DockStyle.Fill;
            lblCurrentWeek.ForeColor = Color.Black;
            lblCurrentWeek.Location = new Point(733, 0);
            lblCurrentWeek.Name = "lblCurrentWeek";
            lblCurrentWeek.Size = new Size(204, 36);
            lblCurrentWeek.TabIndex = 23;
            lblCurrentWeek.Text = "-";
            lblCurrentWeek.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(38, 36);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridViewUKStores);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1358, 784);
            splitContainer1.SplitterDistance = 647;
            splitContainer1.TabIndex = 24;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dataGridViewFranchiseStores);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dataGridViewCompanyStores);
            splitContainer2.Size = new Size(707, 784);
            splitContainer2.SplitterDistance = 421;
            splitContainer2.TabIndex = 27;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(listBoxWeeks, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Left;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(38, 820);
            tableLayoutPanel1.TabIndex = 25;
            // 
            // listBoxWeeks
            // 
            listBoxWeeks.BackColor = SystemColors.Control;
            listBoxWeeks.Dock = DockStyle.Fill;
            listBoxWeeks.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxWeeks.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBoxWeeks.ForeColor = Color.Black;
            listBoxWeeks.FormattingEnabled = true;
            listBoxWeeks.Location = new Point(3, 3);
            listBoxWeeks.Name = "listBoxWeeks";
            listBoxWeeks.SelectionMode = SelectionMode.MultiExtended;
            listBoxWeeks.Size = new Size(32, 814);
            listBoxWeeks.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = SystemColors.Control;
            tableLayoutPanel2.ColumnCount = 12;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(button1, 1, 0);
            tableLayoutPanel2.Controls.Add(lblCurrentWeek, 7, 0);
            tableLayoutPanel2.Controls.Add(chkShow2022, 3, 0);
            tableLayoutPanel2.Controls.Add(chkShow2021, 4, 0);
            tableLayoutPanel2.Controls.Add(chkShow2023, 2, 0);
            tableLayoutPanel2.Controls.Add(chkShow2020, 5, 0);
            tableLayoutPanel2.Controls.Add(button2, 11, 0);
            tableLayoutPanel2.Controls.Add(button3, 9, 0);
            tableLayoutPanel2.Controls.Add(button4, 10, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 6, 0);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.Location = new Point(38, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.Size = new Size(1358, 36);
            tableLayoutPanel2.TabIndex = 26;
            // 
            // chkShow2023
            // 
            chkShow2023.AutoSize = true;
            chkShow2023.CheckAlign = ContentAlignment.BottomCenter;
            chkShow2023.Dock = DockStyle.Fill;
            chkShow2023.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            chkShow2023.ForeColor = Color.Black;
            chkShow2023.Location = new Point(323, 3);
            chkShow2023.Name = "chkShow2023";
            chkShow2023.RightToLeft = RightToLeft.No;
            chkShow2023.Size = new Size(44, 30);
            chkShow2023.TabIndex = 13;
            chkShow2023.Text = "2023";
            chkShow2023.TextAlign = ContentAlignment.MiddleCenter;
            chkShow2023.UseVisualStyleBackColor = true;
            chkShow2023.CheckedChanged += PreviousYearChange;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            button3.Location = new Point(1241, 3);
            button3.Name = "button3";
            button3.Size = new Size(34, 30);
            button3.TabIndex = 23;
            button3.Text = "<";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            button4.Location = new Point(1281, 3);
            button4.Name = "button4";
            button4.Size = new Size(34, 30);
            button4.TabIndex = 24;
            button4.Text = ">";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(lblStoresOverProgress, 0, 1);
            tableLayoutPanel3.Controls.Add(lblStoresOverTarget, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(523, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(204, 30);
            tableLayoutPanel3.TabIndex = 27;
            // 
            // SalesSummary
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1396, 820);
            Controls.Add(splitContainer1);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(2);
            Name = "SalesSummary";
            Text = "Sales Summary";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUKStores).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFranchiseStores).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompanyStores).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private DataGridView dataGridViewUKStores;
        private DataGridView dataGridViewFranchiseStores;
        private DataGridView dataGridViewCompanyStores;
        private ComboBox comboBox1;
        private CheckBox chkShow2022;
        private CheckBox chkShow2021;
        private CheckBox chkShow2020;
        private Button button1;
        private Button button2;
        private Label lblStoresOverTarget;
        private Label lblStoresOverProgress;
        private Label lblCurrentWeek;
        private Button button3;
        private Button button4;
        private TableLayoutPanel tableLayoutPanel3;
        private CheckBox chkShow2023;
        private Class.Design.Custom_Items.CenteredListBox listBoxWeeks;
    }
}
