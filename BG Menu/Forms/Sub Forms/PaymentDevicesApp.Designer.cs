namespace BG_Menu.Forms.Sub_Forms
{
    partial class PaymentDevicesApp
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
            panel1 = new Panel();
            button2 = new Button();
            txtSearch = new TextBox();
            button1 = new Button();
            btnUserAdd = new Button();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnUserAdd);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 43);
            panel1.TabIndex = 0;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.Dock = DockStyle.Left;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.Silver;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(158, 161, 176);
            button2.Image = Properties.Resources.Push;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(200, 0);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.RightToLeft = RightToLeft.No;
            button2.Size = new Size(100, 43);
            button2.TabIndex = 9;
            button2.Text = "Add";
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(305, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(219, 23);
            txtSearch.TabIndex = 8;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // button1
            // 
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Left;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.Silver;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(158, 161, 176);
            button1.Image = Properties.Resources.Push;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(100, 0);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.RightToLeft = RightToLeft.No;
            button1.Size = new Size(100, 43);
            button1.TabIndex = 7;
            button1.Text = "Push";
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnSaveChanges_Click;
            // 
            // btnUserAdd
            // 
            btnUserAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUserAdd.BackgroundImageLayout = ImageLayout.Zoom;
            btnUserAdd.Dock = DockStyle.Left;
            btnUserAdd.FlatAppearance.BorderSize = 0;
            btnUserAdd.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnUserAdd.FlatStyle = FlatStyle.Flat;
            btnUserAdd.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUserAdd.ForeColor = Color.FromArgb(158, 161, 176);
            btnUserAdd.Image = Properties.Resources.Pull;
            btnUserAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnUserAdd.Location = new Point(0, 0);
            btnUserAdd.Margin = new Padding(2);
            btnUserAdd.Name = "btnUserAdd";
            btnUserAdd.RightToLeft = RightToLeft.No;
            btnUserAdd.Size = new Size(100, 43);
            btnUserAdd.TabIndex = 6;
            btnUserAdd.Text = "Pull";
            btnUserAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUserAdd.UseVisualStyleBackColor = true;
            btnUserAdd.Click += btnUserAdd_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 43);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(800, 407);
            dataGridView1.TabIndex = 1;
            // 
            // PaymentDevicesApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Name = "PaymentDevicesApp";
            Text = "PaymentDevicesApp";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private Button btnUserAdd;
        private DataGridView dataGridView1;
        private TextBox txtSearch;
        private Button button2;
    }
}