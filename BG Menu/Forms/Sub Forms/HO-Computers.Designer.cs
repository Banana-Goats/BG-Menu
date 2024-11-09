namespace BG_Menu.Forms.Sub_Forms
{
    partial class HO_Computers
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
            dataGridViewPCInfo = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPCInfo).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewPCInfo
            // 
            dataGridViewPCInfo.AllowUserToAddRows = false;
            dataGridViewPCInfo.AllowUserToDeleteRows = false;
            dataGridViewPCInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPCInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPCInfo.Dock = DockStyle.Fill;
            dataGridViewPCInfo.Location = new Point(0, 0);
            dataGridViewPCInfo.Name = "dataGridViewPCInfo";
            dataGridViewPCInfo.ReadOnly = true;
            dataGridViewPCInfo.RowHeadersVisible = false;
            dataGridViewPCInfo.Size = new Size(989, 608);
            dataGridViewPCInfo.TabIndex = 1;
            // 
            // HO_Computers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(dataGridViewPCInfo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HO_Computers";
            Text = "HO_Computers";
            ((System.ComponentModel.ISupportInitialize)dataGridViewPCInfo).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridViewPCInfo;
        
    }
}