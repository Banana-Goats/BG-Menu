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
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvFolders).BeginInit();
            SuspendLayout();
            // 
            // cmbMailboxes
            // 
            cmbMailboxes.FormattingEnabled = true;
            cmbMailboxes.Location = new Point(12, 12);
            cmbMailboxes.Name = "cmbMailboxes";
            cmbMailboxes.Size = new Size(326, 23);
            cmbMailboxes.TabIndex = 0;
            cmbMailboxes.SelectedIndexChanged += cmbMailboxes_SelectedIndexChanged;
            // 
            // dgvFolders
            // 
            dgvFolders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFolders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFolders.Location = new Point(12, 41);
            dgvFolders.Name = "dgvFolders";
            dgvFolders.RowHeadersVisible = false;
            dgvFolders.Size = new Size(933, 477);
            dgvFolders.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(364, 11);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // EmailManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(957, 530);
            Controls.Add(button1);
            Controls.Add(dgvFolders);
            Controls.Add(cmbMailboxes);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EmailManager";
            Text = "EmailManager";
            ((System.ComponentModel.ISupportInitialize)dgvFolders).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbMailboxes;
        private DataGridView dgvFolders;
        private Button button1;
    }
}