
namespace BG_Menu.Forms.Sub_Forms
{
    partial class DashBoard
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            listView1 = new ListView();
            dataGridViewFaults = new DataGridView();
            machineNameColumn = new DataGridViewTextBoxColumn();
            locationColumn = new DataGridViewTextBoxColumn();
            faultColumn = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFaults).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.5F));
            tableLayoutPanel1.Controls.Add(dataGridViewFaults, 0, 0);
            tableLayoutPanel1.Controls.Add(listView1, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(989, 608);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // listView1
            // 
            listView1.BackColor = Color.FromArgb(46, 51, 73);
            listView1.BorderStyle = BorderStyle.None;
            listView1.Dock = DockStyle.Fill;
            listView1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            listView1.ForeColor = Color.White;
            listView1.Location = new Point(571, 3);
            listView1.Name = "listView1";
            listView1.Size = new Size(415, 602);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.List;
            listView1.DoubleClick += listView1_DoubleClick;
            // 
            // dataGridViewFaults
            // 
            dataGridViewFaults.AllowUserToAddRows = false;
            dataGridViewFaults.AllowUserToDeleteRows = false;
            dataGridViewFaults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFaults.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewFaults.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewFaults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewFaults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFaults.Columns.AddRange(new DataGridViewColumn[] { machineNameColumn, locationColumn, faultColumn });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = Color.Silver;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dataGridViewFaults.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewFaults.Dock = DockStyle.Fill;
            dataGridViewFaults.Location = new Point(3, 3);
            dataGridViewFaults.Name = "dataGridViewFaults";
            dataGridViewFaults.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.ControlLight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridViewFaults.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewFaults.RowHeadersVisible = false;
            dataGridViewFaults.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewFaults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFaults.Size = new Size(414, 602);
            dataGridViewFaults.TabIndex = 3;
            // 
            // machineNameColumn
            // 
            machineNameColumn.DataPropertyName = "MachineName";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(46, 51, 73);
            machineNameColumn.DefaultCellStyle = dataGridViewCellStyle2;
            machineNameColumn.FillWeight = 68.527916F;
            machineNameColumn.HeaderText = "Machine Name";
            machineNameColumn.MinimumWidth = 45;
            machineNameColumn.Name = "machineNameColumn";
            machineNameColumn.ReadOnly = true;
            // 
            // locationColumn
            // 
            locationColumn.DataPropertyName = "Location";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            locationColumn.DefaultCellStyle = dataGridViewCellStyle3;
            locationColumn.FillWeight = 73.84829F;
            locationColumn.HeaderText = "Location";
            locationColumn.MinimumWidth = 60;
            locationColumn.Name = "locationColumn";
            locationColumn.ReadOnly = true;
            // 
            // faultColumn
            // 
            faultColumn.DataPropertyName = "Fault";
            faultColumn.FillWeight = 157.6238F;
            faultColumn.HeaderText = "Fault";
            faultColumn.MinimumWidth = 92;
            faultColumn.Name = "faultColumn";
            faultColumn.ReadOnly = true;
            // 
            // DashBoard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DashBoard";
            Text = "DashBoard";
            Load += DashBoard_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewFaults).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridViewFaults;
        private DataGridViewTextBoxColumn machineNameColumn;
        private DataGridViewTextBoxColumn locationColumn;
        private DataGridViewTextBoxColumn faultColumn;
        private ListView listView1;
    }
}