namespace BG_Menu.Forms.Sub_Forms
{
    partial class AddUserForm
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
            UsernameTextBox = new TextBox();
            PasswordTextBox = new TextBox();
            RankTextBox = new TextBox();
            AddUserButton = new Button();
            button1 = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.BackColor = Color.FromArgb(46, 51, 73);
            UsernameTextBox.BorderStyle = BorderStyle.None;
            UsernameTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            UsernameTextBox.ForeColor = Color.White;
            UsernameTextBox.Location = new Point(86, 245);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.PlaceholderText = "Username";
            UsernameTextBox.Size = new Size(216, 28);
            UsernameTextBox.TabIndex = 0;
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.BackColor = Color.FromArgb(46, 51, 73);
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            PasswordTextBox.ForeColor = Color.White;
            PasswordTextBox.Location = new Point(86, 290);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PlaceholderText = "Password";
            PasswordTextBox.Size = new Size(216, 28);
            PasswordTextBox.TabIndex = 1;
            // 
            // RankTextBox
            // 
            RankTextBox.BackColor = Color.FromArgb(46, 51, 73);
            RankTextBox.BorderStyle = BorderStyle.None;
            RankTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            RankTextBox.ForeColor = Color.White;
            RankTextBox.Location = new Point(86, 337);
            RankTextBox.Name = "RankTextBox";
            RankTextBox.PlaceholderText = "Rank";
            RankTextBox.Size = new Size(216, 28);
            RankTextBox.TabIndex = 2;
            // 
            // AddUserButton
            // 
            AddUserButton.BackColor = Color.FromArgb(46, 51, 73);
            AddUserButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            AddUserButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            AddUserButton.ForeColor = SystemColors.ControlLightLight;
            AddUserButton.Location = new Point(86, 395);
            AddUserButton.Name = "AddUserButton";
            AddUserButton.Size = new Size(104, 40);
            AddUserButton.TabIndex = 3;
            AddUserButton.Text = "Add User";
            AddUserButton.UseVisualStyleBackColor = false;
            AddUserButton.Click += AddUserButton_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(46, 51, 73);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(198, 395);
            button1.Name = "button1";
            button1.Size = new Size(104, 40);
            button1.TabIndex = 4;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(86, 276);
            panel1.Name = "panel1";
            panel1.Size = new Size(216, 1);
            panel1.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(86, 368);
            panel2.Name = "panel2";
            panel2.Size = new Size(216, 1);
            panel2.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Location = new Point(86, 321);
            panel3.Name = "panel3";
            panel3.Size = new Size(216, 1);
            panel3.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.User;
            pictureBox1.Location = new Point(44, 241);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Password;
            pictureBox2.Location = new Point(44, 286);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(31, 32);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 9;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Rank;
            pictureBox3.Location = new Point(44, 333);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(31, 32);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 10;
            pictureBox3.TabStop = false;
            // 
            // AddUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(378, 477);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(button1);
            Controls.Add(AddUserButton);
            Controls.Add(RankTextBox);
            Controls.Add(PasswordTextBox);
            Controls.Add(UsernameTextBox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddUserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddUserForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox UsernameTextBox;
        private TextBox PasswordTextBox;
        private TextBox RankTextBox;
        private Button AddUserButton;
        private Button button1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
    }
}