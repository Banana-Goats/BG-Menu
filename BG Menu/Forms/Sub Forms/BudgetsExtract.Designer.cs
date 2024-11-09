namespace BG_Menu.Forms.Sub_Forms
{
    partial class BudgetsExtract
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
            dataGridView1 = new DataGridView();
            panel1 = new Panel();
            button4 = new Button();
            button2 = new Button();
            button3 = new Button();
            panel3 = new Panel();
            textBox1 = new TextBox();
            panel2 = new Panel();
            comboBoxTables = new ComboBox();
            button1 = new Button();
            dataGridViewDB = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDB).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Left;
            dataGridView1.Location = new Point(0, 39);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(385, 530);
            dataGridView1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(comboBoxTables);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(973, 39);
            panel1.TabIndex = 3;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(46, 51, 73);
            button4.Dock = DockStyle.Right;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button4.ForeColor = SystemColors.ControlLightLight;
            button4.Location = new Point(486, 0);
            button4.Name = "button4";
            button4.Size = new Size(104, 39);
            button4.TabIndex = 12;
            button4.Text = "Upload";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(46, 51, 73);
            button2.Dock = DockStyle.Right;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button2.ForeColor = SystemColors.ControlLightLight;
            button2.Location = new Point(590, 0);
            button2.Name = "button2";
            button2.Size = new Size(104, 39);
            button2.TabIndex = 9;
            button2.Text = "DB File";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(46, 51, 73);
            button3.Dock = DockStyle.Right;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button3.ForeColor = SystemColors.ControlLightLight;
            button3.Location = new Point(694, 0);
            button3.Name = "button3";
            button3.Size = new Size(104, 39);
            button3.TabIndex = 11;
            button3.Text = "Update";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(panel2);
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(104, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(95, 39);
            panel3.TabIndex = 11;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(46, 51, 73);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "X8:X82";
            textBox1.Size = new Size(89, 28);
            textBox1.TabIndex = 7;
            textBox1.Text = "U8:U82";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(3, 34);
            panel2.Name = "panel2";
            panel2.Size = new Size(89, 1);
            panel2.TabIndex = 10;
            // 
            // comboBoxTables
            // 
            comboBoxTables.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxTables.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxTables.BackColor = Color.FromArgb(46, 51, 73);
            comboBoxTables.Dock = DockStyle.Right;
            comboBoxTables.FlatStyle = FlatStyle.Flat;
            comboBoxTables.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            comboBoxTables.ForeColor = SystemColors.Window;
            comboBoxTables.FormattingEnabled = true;
            comboBoxTables.Location = new Point(798, 0);
            comboBoxTables.Name = "comboBoxTables";
            comboBoxTables.Size = new Size(175, 38);
            comboBoxTables.TabIndex = 10;
            comboBoxTables.SelectedIndexChanged += comboBoxTables_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(46, 51, 73);
            button1.Dock = DockStyle.Left;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(104, 39);
            button1.TabIndex = 8;
            button1.Text = "Import";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // dataGridViewDB
            // 
            dataGridViewDB.AllowUserToAddRows = false;
            dataGridViewDB.AllowUserToDeleteRows = false;
            dataGridViewDB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewDB.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDB.Dock = DockStyle.Left;
            dataGridViewDB.Location = new Point(385, 39);
            dataGridViewDB.Name = "dataGridViewDB";
            dataGridViewDB.ReadOnly = true;
            dataGridViewDB.RowHeadersVisible = false;
            dataGridViewDB.Size = new Size(385, 530);
            dataGridViewDB.TabIndex = 4;
            // 
            // BudgetsExtract
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(973, 569);
            Controls.Add(dataGridViewDB);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BudgetsExtract";
            Text = "BudgetsExtract";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewDB).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private Panel panel1;
        private Panel panel3;
        private TextBox textBox1;
        private Panel panel2;
        private Button button1;
        private Button button2;
        private ComboBox comboBoxTables;
        private Button button3;
        private DataGridView dataGridViewDB;
        private Button button4;
    }
}