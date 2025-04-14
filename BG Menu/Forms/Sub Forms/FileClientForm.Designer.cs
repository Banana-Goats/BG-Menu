namespace BG_Menu.Forms.Sub_Forms
{
    partial class FileClientForm
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
            button1 = new Button();
            richTextBoxLogs = new RichTextBox();
            listViewFiles = new ListView();
            button2 = new Button();
            panel1 = new Panel();
            button7 = new Button();
            button5 = new Button();
            button6 = new Button();
            button4 = new Button();
            button3 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(46, 51, 73);
            button1.Dock = DockStyle.Left;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button1.ForeColor = SystemColors.Control;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(126, 35);
            button1.TabIndex = 1;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnConnect_Click;
            // 
            // richTextBoxLogs
            // 
            richTextBoxLogs.BackColor = Color.FromArgb(46, 51, 73);
            richTextBoxLogs.Dock = DockStyle.Right;
            richTextBoxLogs.ForeColor = SystemColors.Window;
            richTextBoxLogs.Location = new Point(858, 35);
            richTextBoxLogs.Name = "richTextBoxLogs";
            richTextBoxLogs.Size = new Size(474, 676);
            richTextBoxLogs.TabIndex = 2;
            richTextBoxLogs.Text = "";
            // 
            // listViewFiles
            // 
            listViewFiles.BackColor = Color.FromArgb(46, 51, 73);
            listViewFiles.Dock = DockStyle.Fill;
            listViewFiles.ForeColor = SystemColors.Window;
            listViewFiles.Location = new Point(0, 35);
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new Size(858, 676);
            listViewFiles.TabIndex = 3;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(46, 51, 73);
            button2.Dock = DockStyle.Left;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button2.ForeColor = SystemColors.Control;
            button2.Location = new Point(126, 0);
            button2.Name = "button2";
            button2.Size = new Size(126, 35);
            button2.TabIndex = 4;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnBack_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(46, 51, 73);
            panel1.Controls.Add(button7);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button6);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1332, 35);
            panel1.TabIndex = 5;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(46, 51, 73);
            button7.Dock = DockStyle.Right;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button7.ForeColor = SystemColors.Control;
            button7.Location = new Point(1206, 0);
            button7.Name = "button7";
            button7.Size = new Size(126, 35);
            button7.TabIndex = 9;
            button7.Text = "Push Updates";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(46, 51, 73);
            button5.Dock = DockStyle.Left;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button5.ForeColor = SystemColors.Control;
            button5.Location = new Point(630, 0);
            button5.Name = "button5";
            button5.Size = new Size(126, 35);
            button5.TabIndex = 7;
            button5.Text = "Create Folder";
            button5.UseVisualStyleBackColor = false;
            button5.Click += btnCreateFolder_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(46, 51, 73);
            button6.Dock = DockStyle.Left;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button6.ForeColor = SystemColors.Control;
            button6.Location = new Point(504, 0);
            button6.Name = "button6";
            button6.Size = new Size(126, 35);
            button6.TabIndex = 8;
            button6.Text = "Upload Folder";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnUploadFolder_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(46, 51, 73);
            button4.Dock = DockStyle.Left;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button4.ForeColor = SystemColors.Control;
            button4.Location = new Point(378, 0);
            button4.Name = "button4";
            button4.Size = new Size(126, 35);
            button4.TabIndex = 6;
            button4.Text = "Upload File";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnUploadFile_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(46, 51, 73);
            button3.Dock = DockStyle.Left;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button3.ForeColor = SystemColors.Control;
            button3.Location = new Point(252, 0);
            button3.Name = "button3";
            button3.Size = new Size(126, 35);
            button3.TabIndex = 5;
            button3.Text = "Download";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnDownload_Click;
            // 
            // FileClientForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1332, 711);
            Controls.Add(listViewFiles);
            Controls.Add(richTextBoxLogs);
            Controls.Add(panel1);
            Name = "FileClientForm";
            Text = "FileClientForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private RichTextBox richTextBoxLogs;
        private ListView listViewFiles;
        private Button button2;
        private Panel panel1;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
    }
}