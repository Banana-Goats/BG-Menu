namespace BG_Menu.Forms.Sub_Forms
{
    partial class ActiveDirectory
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
            txtUsername = new TextBox();
            txtEmail = new TextBox();
            txtDescription = new TextBox();
            chkAccountLocked = new Class.Design.Custom_Items.ToggleSlider();
            cmbUsers = new ComboBox();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            panel4 = new Panel();
            button3 = new Button();
            btnImportCSV = new Button();
            pictureBox6 = new PictureBox();
            txtNewPassword = new TextBox();
            button2 = new Button();
            dgvUsers = new DataGridView();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(46, 51, 73);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtUsername.ForeColor = SystemColors.Window;
            txtUsername.Location = new Point(65, 13);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(335, 28);
            txtUsername.TabIndex = 1;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(46, 51, 73);
            txtEmail.BorderStyle = BorderStyle.None;
            txtEmail.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtEmail.ForeColor = SystemColors.Window;
            txtEmail.Location = new Point(65, 113);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(335, 28);
            txtEmail.TabIndex = 2;
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(46, 51, 73);
            txtDescription.BorderStyle = BorderStyle.None;
            txtDescription.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtDescription.ForeColor = SystemColors.Window;
            txtDescription.Location = new Point(65, 63);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(335, 28);
            txtDescription.TabIndex = 3;
            // 
            // chkAccountLocked
            // 
            chkAccountLocked.AutoSize = true;
            chkAccountLocked.Checked = true;
            chkAccountLocked.CheckState = CheckState.Checked;
            chkAccountLocked.Location = new Point(68, 172);
            chkAccountLocked.MinimumSize = new Size(45, 22);
            chkAccountLocked.Name = "chkAccountLocked";
            chkAccountLocked.OffBackColor = Color.FromArgb(24, 30, 54);
            chkAccountLocked.OffToggleColor = Color.Gainsboro;
            chkAccountLocked.OnBackColor = Color.Gold;
            chkAccountLocked.OnToggleColor = Color.FromArgb(24, 30, 54);
            chkAccountLocked.Size = new Size(45, 22);
            chkAccountLocked.TabIndex = 4;
            chkAccountLocked.UseVisualStyleBackColor = true;
            chkAccountLocked.CheckedChanged += ChkAccountLocked_CheckedChanged;
            // 
            // cmbUsers
            // 
            cmbUsers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbUsers.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbUsers.BackColor = Color.FromArgb(46, 51, 73);
            cmbUsers.Dock = DockStyle.Right;
            cmbUsers.FlatStyle = FlatStyle.Flat;
            cmbUsers.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            cmbUsers.ForeColor = SystemColors.Window;
            cmbUsers.FormattingEnabled = true;
            cmbUsers.Location = new Point(716, 0);
            cmbUsers.Name = "cmbUsers";
            cmbUsers.Size = new Size(273, 38);
            cmbUsers.TabIndex = 5;
            cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Image = Properties.Resources.Search;
            pictureBox1.Location = new Point(677, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(39, 39);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(65, 150);
            panel1.Name = "panel1";
            panel1.Size = new Size(335, 1);
            panel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(65, 100);
            panel2.Name = "panel2";
            panel2.Size = new Size(335, 1);
            panel2.TabIndex = 12;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Location = new Point(65, 50);
            panel3.Name = "panel3";
            panel3.Size = new Size(335, 1);
            panel3.TabIndex = 13;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.User;
            pictureBox2.Location = new Point(13, 13);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(39, 38);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Info;
            pictureBox3.Location = new Point(13, 63);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(39, 38);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.Mail;
            pictureBox4.Location = new Point(13, 113);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(39, 38);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.Lock;
            pictureBox5.Location = new Point(13, 164);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(39, 38);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 17;
            pictureBox5.TabStop = false;
            // 
            // panel4
            // 
            panel4.Controls.Add(button3);
            panel4.Controls.Add(btnImportCSV);
            panel4.Controls.Add(pictureBox1);
            panel4.Controls.Add(cmbUsers);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(989, 39);
            panel4.TabIndex = 18;
            // 
            // button3
            // 
            button3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.Dock = DockStyle.Left;
            button3.FlatAppearance.BorderColor = Color.White;
            button3.FlatAppearance.MouseOverBackColor = Color.Silver;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Yu Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.White;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(124, 0);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.RightToLeft = RightToLeft.No;
            button3.Size = new Size(124, 39);
            button3.TabIndex = 25;
            button3.Text = "Email";
            button3.TextImageRelation = TextImageRelation.ImageBeforeText;
            button3.UseVisualStyleBackColor = true;
            button3.Click += BtnSendEmail_Click;
            // 
            // btnImportCSV
            // 
            btnImportCSV.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnImportCSV.BackgroundImageLayout = ImageLayout.Zoom;
            btnImportCSV.Dock = DockStyle.Left;
            btnImportCSV.FlatAppearance.BorderColor = Color.White;
            btnImportCSV.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnImportCSV.FlatStyle = FlatStyle.Flat;
            btnImportCSV.Font = new Font("Yu Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImportCSV.ForeColor = Color.White;
            btnImportCSV.ImageAlign = ContentAlignment.MiddleLeft;
            btnImportCSV.Location = new Point(0, 0);
            btnImportCSV.Margin = new Padding(2);
            btnImportCSV.Name = "btnImportCSV";
            btnImportCSV.RightToLeft = RightToLeft.No;
            btnImportCSV.Size = new Size(124, 39);
            btnImportCSV.TabIndex = 23;
            btnImportCSV.Text = "Import";
            btnImportCSV.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnImportCSV.UseVisualStyleBackColor = true;
            btnImportCSV.Click += btnImportCSV_Click;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = Properties.Resources.Password;
            pictureBox6.Location = new Point(13, 214);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(39, 38);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.TabIndex = 19;
            pictureBox6.TabStop = false;
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtNewPassword.Location = new Point(65, 217);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(206, 35);
            txtNewPassword.TabIndex = 20;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.FlatAppearance.BorderColor = Color.White;
            button2.FlatAppearance.MouseOverBackColor = Color.Silver;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Yu Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(276, 217);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.RightToLeft = RightToLeft.No;
            button2.Size = new Size(124, 35);
            button2.TabIndex = 21;
            button2.Text = "Change";
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dgvUsers
            // 
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Location = new Point(0, 0);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(287, 569);
            dgvUsers.TabIndex = 22;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 39);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox2);
            splitContainer1.Panel1.Controls.Add(txtUsername);
            splitContainer1.Panel1.Controls.Add(txtEmail);
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(txtDescription);
            splitContainer1.Panel1.Controls.Add(txtNewPassword);
            splitContainer1.Panel1.Controls.Add(chkAccountLocked);
            splitContainer1.Panel1.Controls.Add(pictureBox6);
            splitContainer1.Panel1.Controls.Add(panel1);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(pictureBox5);
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(pictureBox4);
            splitContainer1.Panel1.Controls.Add(pictureBox3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(989, 569);
            splitContainer1.SplitterDistance = 419;
            splitContainer1.TabIndex = 24;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dgvUsers);
            splitContainer2.Size = new Size(566, 569);
            splitContainer2.SplitterDistance = 287;
            splitContainer2.TabIndex = 24;
            // 
            // ActiveDirectory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(splitContainer1);
            Controls.Add(panel4);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ActiveDirectory";
            Text = "ActiveDirectory";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtDescription;
        private ComboBox cmbUsers;
        private PictureBox pictureBox1;
        private Class.Design.Custom_Items.ToggleSlider chkAccountLocked;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private Panel panel4;
        private PictureBox pictureBox6;
        private TextBox txtNewPassword;
        private Button button2;
        private DataGridView dgvUsers;
        private Button btnImportCSV;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Button button3;
    }
}