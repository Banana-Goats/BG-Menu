namespace BG_Menu.Forms.Sub_Forms
{
    partial class CommandAutoUpdateForm
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
            button4 = new Button();
            dataGridViewCommands = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCommands).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(46, 51, 73);
            panel1.Controls.Add(button4);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(733, 35);
            panel1.TabIndex = 7;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(46, 51, 73);
            button4.Dock = DockStyle.Left;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            button4.ForeColor = SystemColors.Control;
            button4.Location = new Point(0, 0);
            button4.Name = "button4";
            button4.Size = new Size(152, 35);
            button4.TabIndex = 13;
            button4.Text = "Save";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnSave_Click;
            // 
            // dataGridViewCommands
            // 
            dataGridViewCommands.AllowUserToAddRows = false;
            dataGridViewCommands.AllowUserToDeleteRows = false;
            dataGridViewCommands.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCommands.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCommands.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridViewCommands.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCommands.Dock = DockStyle.Fill;
            dataGridViewCommands.GridColor = SystemColors.InactiveCaptionText;
            dataGridViewCommands.Location = new Point(0, 35);
            dataGridViewCommands.Name = "dataGridViewCommands";
            dataGridViewCommands.RowHeadersVisible = false;
            dataGridViewCommands.Size = new Size(733, 411);
            dataGridViewCommands.TabIndex = 8;
            // 
            // CommandAutoUpdateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(733, 446);
            Controls.Add(dataGridViewCommands);
            Controls.Add(panel1);
            Name = "CommandAutoUpdateForm";
            Text = "CommandAutoUpdateForm";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCommands).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button4;
        private DataGridView dataGridViewCommands;
    }
}