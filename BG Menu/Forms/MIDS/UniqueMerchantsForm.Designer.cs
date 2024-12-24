namespace BG_Menu.Forms.MIDS
{
    partial class UniqueMerchantsForm
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
            panel1 = new Panel();
            btnUserAdd = new Button();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnUserAdd);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(562, 43);
            panel1.TabIndex = 0;
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
            btnUserAdd.Image = Properties.Resources.Push;
            btnUserAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnUserAdd.Location = new Point(0, 0);
            btnUserAdd.Margin = new Padding(2);
            btnUserAdd.Name = "btnUserAdd";
            btnUserAdd.RightToLeft = RightToLeft.No;
            btnUserAdd.Size = new Size(100, 43);
            btnUserAdd.TabIndex = 7;
            btnUserAdd.Text = "Save";
            btnUserAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUserAdd.UseVisualStyleBackColor = true;
            btnUserAdd.Click += BtnSaveChanges_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.Gainsboro;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 43);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(562, 492);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            // 
            // UniqueMerchantsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(562, 535);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Name = "UniqueMerchantsForm";
            Text = "PCI DSS";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnUserAdd;
        private DataGridView dataGridView1;
    }
}