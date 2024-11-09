namespace BG_Menu.Forms.Sub_Forms
{
    partial class UserManagement
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            listBoxUsers = new Class.Design.Custom_Items.CenteredListBox();
            dataGridViewPermissions = new DataGridView();
            dataGridViewStoresFranchises = new DataGridView();
            panel1 = new Panel();
            btnUserRemove = new Button();
            btnUserEdit = new Button();
            btnUserAdd = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPermissions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewStoresFranchises).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxUsers
            // 
            listBoxUsers.BackColor = Color.FromArgb(24, 30, 54);
            listBoxUsers.Dock = DockStyle.Fill;
            listBoxUsers.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxUsers.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listBoxUsers.ForeColor = SystemColors.Window;
            listBoxUsers.FormattingEnabled = true;
            listBoxUsers.ItemHeight = 17;
            listBoxUsers.Location = new Point(3, 3);
            listBoxUsers.Name = "listBoxUsers";
            listBoxUsers.Size = new Size(94, 563);
            listBoxUsers.TabIndex = 0;
            listBoxUsers.SelectedIndexChanged += listBoxUsers_SelectedIndexChanged;
            // 
            // dataGridViewPermissions
            // 
            dataGridViewPermissions.AllowUserToAddRows = false;
            dataGridViewPermissions.AllowUserToDeleteRows = false;
            dataGridViewPermissions.BackgroundColor = Color.FromArgb(24, 30, 54);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewPermissions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewPermissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.Control;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewPermissions.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewPermissions.Dock = DockStyle.Fill;
            dataGridViewPermissions.Location = new Point(103, 3);
            dataGridViewPermissions.Name = "dataGridViewPermissions";
            dataGridViewPermissions.RowHeadersVisible = false;
            dataGridViewPermissions.Size = new Size(244, 563);
            dataGridViewPermissions.TabIndex = 1;
            // 
            // dataGridViewStoresFranchises
            // 
            dataGridViewStoresFranchises.AllowUserToAddRows = false;
            dataGridViewStoresFranchises.AllowUserToDeleteRows = false;
            dataGridViewStoresFranchises.BackgroundColor = Color.FromArgb(24, 30, 54);
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridViewStoresFranchises.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewStoresFranchises.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.Control;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dataGridViewStoresFranchises.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewStoresFranchises.Dock = DockStyle.Fill;
            dataGridViewStoresFranchises.GridColor = SystemColors.Menu;
            dataGridViewStoresFranchises.Location = new Point(353, 3);
            dataGridViewStoresFranchises.Name = "dataGridViewStoresFranchises";
            dataGridViewStoresFranchises.RowHeadersVisible = false;
            dataGridViewStoresFranchises.Size = new Size(494, 563);
            dataGridViewStoresFranchises.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnUserRemove);
            panel1.Controls.Add(btnUserEdit);
            panel1.Controls.Add(btnUserAdd);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(989, 39);
            panel1.TabIndex = 5;
            // 
            // btnUserRemove
            // 
            btnUserRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUserRemove.BackgroundImageLayout = ImageLayout.Zoom;
            btnUserRemove.Dock = DockStyle.Left;
            btnUserRemove.FlatAppearance.BorderSize = 0;
            btnUserRemove.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnUserRemove.FlatStyle = FlatStyle.Flat;
            btnUserRemove.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUserRemove.ForeColor = Color.FromArgb(158, 161, 176);
            btnUserRemove.Image = Properties.Resources.Person_Remove;
            btnUserRemove.ImageAlign = ContentAlignment.MiddleLeft;
            btnUserRemove.Location = new Point(336, 0);
            btnUserRemove.Margin = new Padding(2);
            btnUserRemove.Name = "btnUserRemove";
            btnUserRemove.RightToLeft = RightToLeft.No;
            btnUserRemove.Size = new Size(168, 39);
            btnUserRemove.TabIndex = 7;
            btnUserRemove.Text = "Remove User";
            btnUserRemove.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUserRemove.UseVisualStyleBackColor = true;
            btnUserRemove.Click += btnUserRemove_Click;
            // 
            // btnUserEdit
            // 
            btnUserEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnUserEdit.BackgroundImageLayout = ImageLayout.Zoom;
            btnUserEdit.Dock = DockStyle.Left;
            btnUserEdit.FlatAppearance.BorderSize = 0;
            btnUserEdit.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnUserEdit.FlatStyle = FlatStyle.Flat;
            btnUserEdit.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUserEdit.ForeColor = Color.FromArgb(158, 161, 176);
            btnUserEdit.Image = Properties.Resources.Person_Manage;
            btnUserEdit.ImageAlign = ContentAlignment.MiddleLeft;
            btnUserEdit.Location = new Point(168, 0);
            btnUserEdit.Margin = new Padding(2);
            btnUserEdit.Name = "btnUserEdit";
            btnUserEdit.RightToLeft = RightToLeft.No;
            btnUserEdit.Size = new Size(168, 39);
            btnUserEdit.TabIndex = 6;
            btnUserEdit.Text = "Edit User";
            btnUserEdit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUserEdit.UseVisualStyleBackColor = true;
            btnUserEdit.Click += btnUserEdit_Click;
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
            btnUserAdd.Image = Properties.Resources.Person_Add;
            btnUserAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnUserAdd.Location = new Point(0, 0);
            btnUserAdd.Margin = new Padding(2);
            btnUserAdd.Name = "btnUserAdd";
            btnUserAdd.RightToLeft = RightToLeft.No;
            btnUserAdd.Size = new Size(168, 39);
            btnUserAdd.TabIndex = 5;
            btnUserAdd.Text = "Add User";
            btnUserAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUserAdd.UseVisualStyleBackColor = true;
            btnUserAdd.Click += btnUserAdd_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(listBoxUsers, 0, 0);
            tableLayoutPanel1.Controls.Add(dataGridViewPermissions, 1, 0);
            tableLayoutPanel1.Controls.Add(dataGridViewStoresFranchises, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 39);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(989, 569);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // UserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "UserManagement";
            Text = "UserManagement";
            ((System.ComponentModel.ISupportInitialize)dataGridViewPermissions).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewStoresFranchises).EndInit();
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridViewPermissions;
        private DataGridView dataGridViewStoresFranchises;
        private Panel panel1;
        private Button btnUserAdd;
        private Button btnUserRemove;
        private Button btnUserEdit;
        private TableLayoutPanel tableLayoutPanel1;
        private Class.Design.Custom_Items.CenteredListBox listBoxUsers;
    }
}