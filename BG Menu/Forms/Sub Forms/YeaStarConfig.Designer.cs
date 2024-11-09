namespace BG_Menu.Forms.Sub_Forms
{
    partial class YeaStarConfig
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
            btnImportConfig = new Button();
            txtConfig = new TextBox();
            dgvExpansionModule1 = new DataGridView();
            Label = new DataGridViewTextBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            dgvExpansionModule2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dgvExpansionModule3 = new DataGridView();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            txtSearchUser = new TextBox();
            btnExportConfig = new Button();
            panel1 = new Panel();
            cmbDeviceType = new ComboBox();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule3).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnImportConfig
            // 
            btnImportConfig.Dock = DockStyle.Left;
            btnImportConfig.Location = new Point(0, 0);
            btnImportConfig.Name = "btnImportConfig";
            btnImportConfig.Size = new Size(63, 23);
            btnImportConfig.TabIndex = 2;
            btnImportConfig.Text = "Load";
            btnImportConfig.UseVisualStyleBackColor = true;
            btnImportConfig.Click += btnImportConfig_Click;
            // 
            // txtConfig
            // 
            txtConfig.Dock = DockStyle.Fill;
            txtConfig.Location = new Point(750, 23);
            txtConfig.Multiline = true;
            txtConfig.Name = "txtConfig";
            txtConfig.ScrollBars = ScrollBars.Vertical;
            txtConfig.Size = new Size(239, 585);
            txtConfig.TabIndex = 3;
            // 
            // dgvExpansionModule1
            // 
            dgvExpansionModule1.AllowUserToAddRows = false;
            dgvExpansionModule1.AllowUserToDeleteRows = false;
            dgvExpansionModule1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpansionModule1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExpansionModule1.ColumnHeadersVisible = false;
            dgvExpansionModule1.Columns.AddRange(new DataGridViewColumn[] { Label, Value });
            dgvExpansionModule1.Dock = DockStyle.Left;
            dgvExpansionModule1.Location = new Point(0, 0);
            dgvExpansionModule1.MaximumSize = new Size(250, 0);
            dgvExpansionModule1.Name = "dgvExpansionModule1";
            dgvExpansionModule1.ReadOnly = true;
            dgvExpansionModule1.RowHeadersVisible = false;
            dgvExpansionModule1.Size = new Size(250, 585);
            dgvExpansionModule1.TabIndex = 4;
            // 
            // Label
            // 
            Label.HeaderText = "Label";
            Label.Name = "Label";
            Label.ReadOnly = true;
            // 
            // Value
            // 
            Value.HeaderText = "Value";
            Value.Name = "Value";
            Value.ReadOnly = true;
            // 
            // dgvExpansionModule2
            // 
            dgvExpansionModule2.AllowUserToAddRows = false;
            dgvExpansionModule2.AllowUserToDeleteRows = false;
            dgvExpansionModule2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpansionModule2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExpansionModule2.ColumnHeadersVisible = false;
            dgvExpansionModule2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dgvExpansionModule2.Dock = DockStyle.Left;
            dgvExpansionModule2.Location = new Point(250, 0);
            dgvExpansionModule2.MaximumSize = new Size(250, 0);
            dgvExpansionModule2.Name = "dgvExpansionModule2";
            dgvExpansionModule2.ReadOnly = true;
            dgvExpansionModule2.RowHeadersVisible = false;
            dgvExpansionModule2.Size = new Size(250, 585);
            dgvExpansionModule2.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Label";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Value";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dgvExpansionModule3
            // 
            dgvExpansionModule3.AllowUserToAddRows = false;
            dgvExpansionModule3.AllowUserToDeleteRows = false;
            dgvExpansionModule3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpansionModule3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExpansionModule3.ColumnHeadersVisible = false;
            dgvExpansionModule3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dgvExpansionModule3.Dock = DockStyle.Left;
            dgvExpansionModule3.Location = new Point(500, 0);
            dgvExpansionModule3.MaximumSize = new Size(250, 0);
            dgvExpansionModule3.Name = "dgvExpansionModule3";
            dgvExpansionModule3.ReadOnly = true;
            dgvExpansionModule3.RowHeadersVisible = false;
            dgvExpansionModule3.Size = new Size(250, 585);
            dgvExpansionModule3.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Label";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Value";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // txtSearchUser
            // 
            txtSearchUser.Dock = DockStyle.Left;
            txtSearchUser.Location = new Point(119, 0);
            txtSearchUser.Name = "txtSearchUser";
            txtSearchUser.Size = new Size(154, 23);
            txtSearchUser.TabIndex = 7;
            // 
            // btnExportConfig
            // 
            btnExportConfig.Dock = DockStyle.Left;
            btnExportConfig.Location = new Point(63, 0);
            btnExportConfig.Name = "btnExportConfig";
            btnExportConfig.Size = new Size(56, 23);
            btnExportConfig.TabIndex = 8;
            btnExportConfig.Text = "Update";
            btnExportConfig.UseVisualStyleBackColor = true;
            btnExportConfig.Click += btnExportConfig_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(cmbDeviceType);
            panel1.Controls.Add(txtSearchUser);
            panel1.Controls.Add(btnExportConfig);
            panel1.Controls.Add(btnImportConfig);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(989, 23);
            panel1.TabIndex = 9;
            // 
            // cmbDeviceType
            // 
            cmbDeviceType.Dock = DockStyle.Right;
            cmbDeviceType.FormattingEnabled = true;
            cmbDeviceType.Items.AddRange(new object[] { "T46S", "T46U" });
            cmbDeviceType.Location = new Point(868, 0);
            cmbDeviceType.Name = "cmbDeviceType";
            cmbDeviceType.Size = new Size(121, 23);
            cmbDeviceType.TabIndex = 9;
            cmbDeviceType.SelectedIndexChanged += CmbDeviceType_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.Controls.Add(dgvExpansionModule3);
            panel2.Controls.Add(dgvExpansionModule2);
            panel2.Controls.Add(dgvExpansionModule1);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 23);
            panel2.Name = "panel2";
            panel2.Size = new Size(750, 585);
            panel2.TabIndex = 10;
            // 
            // YeaStarConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(txtConfig);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "YeaStarConfig";
            Text = "YeaStarConfig";
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvExpansionModule3).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnImportConfig;
        private TextBox txtConfig;
        private DataGridView dgvExpansionModule1;
        private DataGridViewTextBoxColumn Label;
        private DataGridViewTextBoxColumn Value;
        private DataGridView dgvExpansionModule2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridView dgvExpansionModule3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private TextBox txtSearchUser;
        private Button btnExportConfig;
        private Panel panel1;
        private Panel panel2;
        private ComboBox cmbDeviceType;
    }
}