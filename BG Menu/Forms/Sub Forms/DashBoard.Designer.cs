﻿using System.Data.Entity.Core.Metadata.Edm;

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
            dataGridViewFaults = new DataGridView();
            machineNameColumn = new DataGridViewTextBoxColumn();
            locationColumn = new DataGridViewTextBoxColumn();
            faultColumn = new DataGridViewTextBoxColumn();
            dataGridView1 = new DataGridView();
            splitContainer2 = new SplitContainer();
            splitContainer1 = new SplitContainer();
            dataGridView2 = new DataGridView();
            Folder = new DataGridViewTextBoxColumn();
            Process = new DataGridViewButtonColumn();
            progressTextBox = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFaults).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.5F));
            tableLayoutPanel1.Controls.Add(dataGridViewFaults, 0, 0);
            tableLayoutPanel1.Controls.Add(dataGridView1, 1, 0);
            tableLayoutPanel1.Controls.Add(splitContainer2, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(989, 608);
            tableLayoutPanel1.TabIndex = 3;
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
            dataGridViewFaults.Size = new Size(414, 298);
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
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(423, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(142, 298);
            dataGridView1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(571, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            splitContainer2.Size = new Size(415, 298);
            splitContainer2.SplitterDistance = 199;
            splitContainer2.TabIndex = 7;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridView2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(progressTextBox);
            splitContainer1.Size = new Size(199, 298);
            splitContainer1.SplitterDistance = 130;
            splitContainer1.TabIndex = 6;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Folder, Process });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.Size = new Size(199, 130);
            dataGridView2.TabIndex = 5;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            // 
            // Folder
            // 
            Folder.HeaderText = "Folder";
            Folder.Name = "Folder";
            Folder.ReadOnly = true;
            // 
            // Process
            // 
            Process.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Process.HeaderText = "Process";
            Process.MinimumWidth = 80;
            Process.Name = "Process";
            Process.ReadOnly = true;
            Process.Width = 80;
            // 
            // progressTextBox
            // 
            progressTextBox.BackColor = Color.FromArgb(46, 51, 73);
            progressTextBox.BorderStyle = BorderStyle.None;
            progressTextBox.Dock = DockStyle.Fill;
            progressTextBox.ForeColor = Color.White;
            progressTextBox.Location = new Point(0, 0);
            progressTextBox.Multiline = true;
            progressTextBox.Name = "progressTextBox";
            progressTextBox.Size = new Size(199, 164);
            progressTextBox.TabIndex = 0;
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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridViewFaults;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn Folder;
        private DataGridViewButtonColumn Process;
        private SplitContainer splitContainer1;
        private TextBox progressTextBox;
        private SplitContainer splitContainer2;
        private DataGridViewTextBoxColumn machineNameColumn;
        private DataGridViewTextBoxColumn locationColumn;
        private DataGridViewTextBoxColumn faultColumn;
    }
}