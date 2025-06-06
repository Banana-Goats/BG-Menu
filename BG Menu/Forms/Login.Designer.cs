﻿namespace BG_Menu
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            loginButton = new Button();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            SetPasswordButton = new Button();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            panel3 = new Panel();
            panel1 = new Panel();
            pictureBox3 = new PictureBox();
            label1 = new Label();
            statusLabel = new Label();
            networkTestListView = new ListView();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.FromArgb(46, 51, 73);
            loginButton.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            loginButton.ForeColor = SystemColors.Control;
            loginButton.Location = new Point(56, 430);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(108, 52);
            loginButton.TabIndex = 0;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += loginButton_Click;
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = Color.FromArgb(46, 51, 73);
            usernameTextBox.BorderStyle = BorderStyle.None;
            usernameTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            usernameTextBox.ForeColor = SystemColors.Info;
            usernameTextBox.Location = new Point(102, 313);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(296, 28);
            usernameTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = Color.FromArgb(46, 51, 73);
            passwordTextBox.BorderStyle = BorderStyle.None;
            passwordTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold);
            passwordTextBox.ForeColor = SystemColors.Info;
            passwordTextBox.Location = new Point(102, 368);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(263, 28);
            passwordTextBox.TabIndex = 2;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // SetPasswordButton
            // 
            SetPasswordButton.BackColor = Color.FromArgb(46, 51, 73);
            SetPasswordButton.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            SetPasswordButton.ForeColor = SystemColors.Control;
            SetPasswordButton.Location = new Point(290, 430);
            SetPasswordButton.Name = "SetPasswordButton";
            SetPasswordButton.Size = new Size(108, 52);
            SetPasswordButton.TabIndex = 3;
            SetPasswordButton.Text = "Exit";
            SetPasswordButton.UseVisualStyleBackColor = false;
            SetPasswordButton.Click += SetPasswordButton_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Password;
            pictureBox2.Location = new Point(56, 361);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(40, 40);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.User;
            pictureBox1.Location = new Point(56, 306);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Location = new Point(102, 344);
            panel3.Name = "panel3";
            panel3.Size = new Size(296, 1);
            panel3.TabIndex = 13;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(102, 400);
            panel1.Name = "panel1";
            panel1.Size = new Size(296, 1);
            panel1.TabIndex = 14;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Eye;
            pictureBox3.Location = new Point(370, 368);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(28, 28);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(56, 496);
            label1.Name = "label1";
            label1.Size = new Size(342, 37);
            label1.TabIndex = 16;
            label1.Text = "Version : 2.9.4";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // statusLabel
            // 
            statusLabel.BorderStyle = BorderStyle.FixedSingle;
            statusLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            statusLabel.ForeColor = Color.White;
            statusLabel.Location = new Point(56, 252);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(342, 34);
            statusLabel.TabIndex = 17;
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // networkTestListView
            // 
            networkTestListView.BackColor = Color.FromArgb(46, 51, 73);
            networkTestListView.BorderStyle = BorderStyle.None;
            networkTestListView.Font = new Font("Segoe UI", 9.25F, FontStyle.Bold);
            networkTestListView.Location = new Point(56, 33);
            networkTestListView.MultiSelect = false;
            networkTestListView.Name = "networkTestListView";
            networkTestListView.Size = new Size(342, 184);
            networkTestListView.TabIndex = 19;
            networkTestListView.UseCompatibleStateImageBehavior = false;
            networkTestListView.View = View.List;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(46, 51, 73);
            button1.Font = new Font("Segoe UI", 10.25F, FontStyle.Bold);
            button1.ForeColor = SystemColors.Control;
            button1.Location = new Point(173, 430);
            button1.Name = "button1";
            button1.Size = new Size(108, 52);
            button1.TabIndex = 20;
            button1.Text = "Network Test";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(458, 566);
            Controls.Add(button1);
            Controls.Add(networkTestListView);
            Controls.Add(statusLabel);
            Controls.Add(label1);
            Controls.Add(pictureBox3);
            Controls.Add(panel1);
            Controls.Add(panel3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(SetPasswordButton);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(loginButton);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button loginButton;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button SetPasswordButton;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Panel panel1;
        private PictureBox pictureBox3;
        private Label label1;
        private Label statusLabel;
        private ListView networkTestListView;
        private Button button1;
    }
}