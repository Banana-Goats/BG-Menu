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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
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
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.3549042F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58.6450958F));
            tableLayoutPanel1.Controls.Add(dataGridViewFaults, 0, 0);
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewFaults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewFaults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFaults.Columns.AddRange(new DataGridViewColumn[] { machineNameColumn, locationColumn, faultColumn });
            dataGridViewFaults.Dock = DockStyle.Fill;
            dataGridViewFaults.Location = new Point(3, 3);
            dataGridViewFaults.Name = "dataGridViewFaults";
            dataGridViewFaults.ReadOnly = true;
            dataGridViewFaults.RowHeadersVisible = false;
            dataGridViewFaults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFaults.Size = new Size(403, 298);
            dataGridViewFaults.TabIndex = 3;
            // 
            // machineNameColumn
            // 
            machineNameColumn.DataPropertyName = "MachineName";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
    }
}