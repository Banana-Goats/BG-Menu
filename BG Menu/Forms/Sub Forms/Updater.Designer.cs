namespace BG_Menu.Forms.Sub_Forms
{
    partial class Updater
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
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            txtSearch = new TextBox();
            button2 = new Button();
            button1 = new Button();
            button6 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 35);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(1071, 501);
            dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(46, 51, 73);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(button6);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1071, 35);
            panel1.TabIndex = 6;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(46, 51, 73);
            button5.Dock = DockStyle.Left;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button5.ForeColor = SystemColors.Control;
            button5.Location = new Point(765, 0);
            button5.Name = "button5";
            button5.Size = new Size(153, 35);
            button5.TabIndex = 14;
            button5.Text = "Speed Test";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(46, 51, 73);
            button4.Dock = DockStyle.Left;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button4.ForeColor = SystemColors.Control;
            button4.Location = new Point(612, 0);
            button4.Name = "button4";
            button4.Size = new Size(153, 35);
            button4.TabIndex = 13;
            button4.Text = "Run Commands";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(46, 51, 73);
            button3.Dock = DockStyle.Left;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button3.ForeColor = SystemColors.Control;
            button3.Location = new Point(459, 0);
            button3.Name = "button3";
            button3.Size = new Size(153, 35);
            button3.TabIndex = 12;
            button3.Text = "Update Templates";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(924, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(135, 23);
            txtSearch.TabIndex = 11;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(46, 51, 73);
            button2.Dock = DockStyle.Left;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button2.ForeColor = SystemColors.Control;
            button2.Location = new Point(306, 0);
            button2.Name = "button2";
            button2.Size = new Size(153, 35);
            button2.TabIndex = 10;
            button2.Text = "Call Pop Lite";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(46, 51, 73);
            button1.Dock = DockStyle.Left;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button1.ForeColor = SystemColors.Control;
            button1.Location = new Point(153, 0);
            button1.Name = "button1";
            button1.Size = new Size(153, 35);
            button1.TabIndex = 9;
            button1.Text = "Till Updater";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(46, 51, 73);
            button6.Dock = DockStyle.Left;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button6.ForeColor = SystemColors.Control;
            button6.Location = new Point(0, 0);
            button6.Name = "button6";
            button6.Size = new Size(153, 35);
            button6.TabIndex = 8;
            button6.Text = "Network Detector";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // Updater
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1071, 536);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Name = "Updater";
            Text = "Updater";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel panel1;
        private Button button2;
        private Button button1;
        private Button button6;
        private TextBox txtSearch;
        private Button button3;
        private Button button4;
        private Button button5;
    }
}