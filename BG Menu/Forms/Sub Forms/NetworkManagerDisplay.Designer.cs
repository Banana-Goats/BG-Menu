namespace BG_Menu.Forms.Sub_Forms
{
    partial class NetworkManagerDisplay
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
            panel1 = new Panel();
            checkBoxLaptop = new CheckBox();
            checkBoxTablet = new CheckBox();
            checkBoxTill = new CheckBox();
            checkBoxPreserveScroll = new Class.Design.Custom_Items.ToggleSlider();
            dataGridView1 = new DataGridView();
            flowLayoutPanel = new FlowLayoutPanel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBoxLaptop);
            panel1.Controls.Add(checkBoxTablet);
            panel1.Controls.Add(checkBoxTill);
            panel1.Controls.Add(checkBoxPreserveScroll);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(989, 40);
            panel1.TabIndex = 0;
            // 
            // checkBoxLaptop
            // 
            checkBoxLaptop.AutoSize = true;
            checkBoxLaptop.CheckAlign = ContentAlignment.BottomCenter;
            checkBoxLaptop.Checked = true;
            checkBoxLaptop.CheckState = CheckState.Checked;
            checkBoxLaptop.Dock = DockStyle.Left;
            checkBoxLaptop.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            checkBoxLaptop.ForeColor = Color.Black;
            checkBoxLaptop.Location = new Point(208, 0);
            checkBoxLaptop.MinimumSize = new Size(100, 0);
            checkBoxLaptop.Name = "checkBoxLaptop";
            checkBoxLaptop.RightToLeft = RightToLeft.No;
            checkBoxLaptop.Size = new Size(100, 40);
            checkBoxLaptop.TabIndex = 16;
            checkBoxLaptop.Text = "Workshop";
            checkBoxLaptop.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxLaptop.UseVisualStyleBackColor = true;
            // 
            // checkBoxTablet
            // 
            checkBoxTablet.AutoSize = true;
            checkBoxTablet.CheckAlign = ContentAlignment.BottomCenter;
            checkBoxTablet.Checked = true;
            checkBoxTablet.CheckState = CheckState.Checked;
            checkBoxTablet.Dock = DockStyle.Left;
            checkBoxTablet.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            checkBoxTablet.ForeColor = Color.Black;
            checkBoxTablet.Location = new Point(100, 0);
            checkBoxTablet.MinimumSize = new Size(100, 0);
            checkBoxTablet.Name = "checkBoxTablet";
            checkBoxTablet.RightToLeft = RightToLeft.No;
            checkBoxTablet.Size = new Size(108, 40);
            checkBoxTablet.TabIndex = 15;
            checkBoxTablet.Text = "Queuebuster";
            checkBoxTablet.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxTablet.UseVisualStyleBackColor = true;
            // 
            // checkBoxTill
            // 
            checkBoxTill.AutoSize = true;
            checkBoxTill.CheckAlign = ContentAlignment.BottomCenter;
            checkBoxTill.Checked = true;
            checkBoxTill.CheckState = CheckState.Checked;
            checkBoxTill.Dock = DockStyle.Left;
            checkBoxTill.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold);
            checkBoxTill.ForeColor = Color.Black;
            checkBoxTill.Location = new Point(0, 0);
            checkBoxTill.MinimumSize = new Size(100, 0);
            checkBoxTill.Name = "checkBoxTill";
            checkBoxTill.RightToLeft = RightToLeft.No;
            checkBoxTill.Size = new Size(100, 40);
            checkBoxTill.TabIndex = 14;
            checkBoxTill.Text = "Main Till";
            checkBoxTill.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxTill.UseVisualStyleBackColor = true;
            // 
            // checkBoxPreserveScroll
            // 
            checkBoxPreserveScroll.AutoSize = true;
            checkBoxPreserveScroll.Dock = DockStyle.Right;
            checkBoxPreserveScroll.Location = new Point(944, 0);
            checkBoxPreserveScroll.MaximumSize = new Size(0, 22);
            checkBoxPreserveScroll.MinimumSize = new Size(45, 22);
            checkBoxPreserveScroll.Name = "checkBoxPreserveScroll";
            checkBoxPreserveScroll.OffBackColor = Color.Gray;
            checkBoxPreserveScroll.OffToggleColor = Color.Gainsboro;
            checkBoxPreserveScroll.OnBackColor = Color.MediumSlateBlue;
            checkBoxPreserveScroll.OnToggleColor = Color.WhiteSmoke;
            checkBoxPreserveScroll.Size = new Size(45, 22);
            checkBoxPreserveScroll.TabIndex = 6;
            checkBoxPreserveScroll.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 40);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(989, 568);
            dataGridView1.TabIndex = 1;
            // 
            // flowLayoutPanel
            // 
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.Location = new Point(0, 40);
            flowLayoutPanel.Name = "flowLayoutPanel";
            flowLayoutPanel.Size = new Size(989, 568);
            flowLayoutPanel.TabIndex = 1;
            flowLayoutPanel.Visible = false;
            // 
            // NetworkManagerDisplay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(989, 608);
            Controls.Add(dataGridView1);
            Controls.Add(flowLayoutPanel);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "NetworkManagerDisplay";
            Text = "NetworkManagerDisplay";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView1;
        private FlowLayoutPanel flowLayoutPanel;
        private Class.Design.Custom_Items.ToggleSlider checkBoxPreserveScroll;
        private CheckBox checkBoxLaptop;
        private CheckBox checkBoxTablet;
        private CheckBox checkBoxTill;
    }
}