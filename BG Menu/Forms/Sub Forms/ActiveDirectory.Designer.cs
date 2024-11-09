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
            pictureBox6 = new PictureBox();
            txtNewPassword = new TextBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(46, 51, 73);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtUsername.ForeColor = SystemColors.Window;
            txtUsername.Location = new Point(64, 55);
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
            txtEmail.Location = new Point(64, 155);
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
            txtDescription.Location = new Point(64, 105);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(335, 28);
            txtDescription.TabIndex = 3;
            // 
            // chkAccountLocked
            // 
            chkAccountLocked.AutoSize = true;
            chkAccountLocked.Checked = true;
            chkAccountLocked.CheckState = CheckState.Checked;
            chkAccountLocked.Location = new Point(67, 214);
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
            panel1.Location = new Point(64, 192);
            panel1.Name = "panel1";
            panel1.Size = new Size(335, 1);
            panel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(64, 142);
            panel2.Name = "panel2";
            panel2.Size = new Size(335, 1);
            panel2.TabIndex = 12;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Location = new Point(64, 92);
            panel3.Name = "panel3";
            panel3.Size = new Size(335, 1);
            panel3.TabIndex = 13;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.User;
            pictureBox2.Location = new Point(12, 55);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(39, 38);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Info;
            pictureBox3.Location = new Point(12, 105);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(39, 38);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.Mail;
            pictureBox4.Location = new Point(12, 155);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(39, 38);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.Lock;
            pictureBox5.Location = new Point(12, 206);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(39, 38);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 17;
            pictureBox5.TabStop = false;
            // 
            // panel4
            // 
            panel4.Controls.Add(pictureBox1);
            panel4.Controls.Add(cmbUsers);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(989, 39);
            panel4.TabIndex = 18;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = Properties.Resources.Password;
            pictureBox6.Location = new Point(12, 256);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(39, 38);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.TabIndex = 19;
            pictureBox6.TabStop = false;
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            txtNewPassword.Location = new Point(64, 259);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(206, 35);
            txtNewPassword.TabIndex = 20;
            txtNewPassword.UseSystemPasswordChar = true;
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
            button2.Location = new Point(275, 259);
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
            // ActiveDirectory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(button2);
            Controls.Add(txtNewPassword);
            Controls.Add(pictureBox6);
            Controls.Add(panel4);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(chkAccountLocked);
            Controls.Add(txtDescription);
            Controls.Add(txtEmail);
            Controls.Add(txtUsername);
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
            ResumeLayout(false);
            PerformLayout();
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
    }
}