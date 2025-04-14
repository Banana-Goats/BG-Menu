namespace BG_Menu.Forms.Sub_Forms
{
    partial class BudgetsExtract
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
            labelProgress = new Label();
            progressBar1 = new ProgressBar();
            button6 = new Button();
            panel3 = new Panel();
            dataGridViewMapping = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMapping).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(labelProgress);
            panel1.Controls.Add(progressBar1);
            panel1.Controls.Add(button6);
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(973, 39);
            panel1.TabIndex = 3;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.BackColor = Color.Transparent;
            labelProgress.ForeColor = SystemColors.ButtonHighlight;
            labelProgress.Location = new Point(397, 14);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(38, 15);
            labelProgress.TabIndex = 16;
            labelProgress.Text = "label1";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(110, 6);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(281, 30);
            progressBar1.TabIndex = 15;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(46, 51, 73);
            button6.Dock = DockStyle.Left;
            button6.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 224, 224);
            button6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button6.ForeColor = SystemColors.ControlLightLight;
            button6.Location = new Point(0, 0);
            button6.Name = "button6";
            button6.Size = new Size(104, 39);
            button6.TabIndex = 14;
            button6.Text = "Import";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(0, 39);
            panel3.TabIndex = 11;
            // 
            // dataGridViewMapping
            // 
            dataGridViewMapping.AllowUserToAddRows = false;
            dataGridViewMapping.AllowUserToDeleteRows = false;
            dataGridViewMapping.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMapping.Location = new Point(0, 39);
            dataGridViewMapping.Name = "dataGridViewMapping";
            dataGridViewMapping.RowHeadersVisible = false;
            dataGridViewMapping.Size = new Size(391, 253);
            dataGridViewMapping.TabIndex = 5;
            // 
            // BudgetsExtract
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(973, 569);
            Controls.Add(dataGridViewMapping);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BudgetsExtract";
            Text = "BudgetsExtract";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMapping).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Panel panel3;
        private DataGridView dataGridViewMapping;
        private Button button6;
        private Label labelProgress;
        private ProgressBar progressBar1;
    }
}