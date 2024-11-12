namespace BG_Menu.Forms.Sub_Forms
{
    partial class EmailManager
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
            cmbMailboxes = new ComboBox();
            dgvFolders = new DataGridView();
            btnCopyEmails = new Button();
            panel1 = new Panel();
            btnPauseResume = new Button();
            btnCleanFilenames = new Button();
            splitContainer1 = new SplitContainer();
            txtProgress = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvFolders).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // cmbMailboxes
            // 
            cmbMailboxes.Dock = DockStyle.Left;
            cmbMailboxes.FormattingEnabled = true;
            cmbMailboxes.Location = new Point(75, 0);
            cmbMailboxes.Name = "cmbMailboxes";
            cmbMailboxes.Size = new Size(326, 23);
            cmbMailboxes.TabIndex = 0;
            cmbMailboxes.SelectedIndexChanged += cmbMailboxes_SelectedIndexChanged;
            // 
            // dgvFolders
            // 
            dgvFolders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFolders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFolders.Dock = DockStyle.Fill;
            dgvFolders.Location = new Point(0, 0);
            dgvFolders.Name = "dgvFolders";
            dgvFolders.RowHeadersVisible = false;
            dgvFolders.Size = new Size(687, 506);
            dgvFolders.TabIndex = 1;
            // 
            // btnCopyEmails
            // 
            btnCopyEmails.Dock = DockStyle.Left;
            btnCopyEmails.Location = new Point(0, 0);
            btnCopyEmails.Name = "btnCopyEmails";
            btnCopyEmails.Size = new Size(75, 24);
            btnCopyEmails.TabIndex = 2;
            btnCopyEmails.Text = "Export";
            btnCopyEmails.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnPauseResume);
            panel1.Controls.Add(btnCleanFilenames);
            panel1.Controls.Add(cmbMailboxes);
            panel1.Controls.Add(btnCopyEmails);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(957, 24);
            panel1.TabIndex = 3;
            // 
            // btnPauseResume
            // 
            btnPauseResume.Dock = DockStyle.Left;
            btnPauseResume.Location = new Point(476, 0);
            btnPauseResume.Name = "btnPauseResume";
            btnPauseResume.Size = new Size(75, 24);
            btnPauseResume.TabIndex = 4;
            btnPauseResume.Text = "Pause";
            btnPauseResume.UseVisualStyleBackColor = true;
            // 
            // btnCleanFilenames
            // 
            btnCleanFilenames.Dock = DockStyle.Left;
            btnCleanFilenames.Location = new Point(401, 0);
            btnCleanFilenames.Name = "btnCleanFilenames";
            btnCleanFilenames.Size = new Size(75, 24);
            btnCleanFilenames.TabIndex = 3;
            btnCleanFilenames.Text = "Clean File's";
            btnCleanFilenames.UseVisualStyleBackColor = true;
            btnCleanFilenames.Click += btnCleanFilenames_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvFolders);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(txtProgress);
            splitContainer1.Size = new Size(957, 506);
            splitContainer1.SplitterDistance = 687;
            splitContainer1.TabIndex = 4;
            // 
            // txtProgress
            // 
            txtProgress.Dock = DockStyle.Fill;
            txtProgress.Location = new Point(0, 0);
            txtProgress.Multiline = true;
            txtProgress.Name = "txtProgress";
            txtProgress.ScrollBars = ScrollBars.Vertical;
            txtProgress.Size = new Size(266, 506);
            txtProgress.TabIndex = 0;
            // 
            // EmailManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(957, 530);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EmailManager";
            Text = "EmailManager";
            ((System.ComponentModel.ISupportInitialize)dgvFolders).EndInit();
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbMailboxes;
        private DataGridView dgvFolders;
        private Button btnCopyEmails;
        private Panel panel1;
        private SplitContainer splitContainer1;
        private TextBox txtProgress;
        private Button btnCleanFilenames;
        private Button btnPauseResume;
    }
}