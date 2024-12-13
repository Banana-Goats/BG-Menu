namespace BG_Menu.Forms.Sub_Forms
{
    partial class Refunds
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
            btnSalespersonTotals = new Button();
            btnWarehouseTotals = new Button();
            dateTimePickerFrom = new DateTimePicker();
            btnRun = new Button();
            dataGridViewResults = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnSalespersonTotals);
            panel1.Controls.Add(btnWarehouseTotals);
            panel1.Controls.Add(dateTimePickerFrom);
            panel1.Controls.Add(btnRun);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 28);
            panel1.TabIndex = 0;
            // 
            // btnSalespersonTotals
            // 
            btnSalespersonTotals.Dock = DockStyle.Left;
            btnSalespersonTotals.Location = new Point(330, 0);
            btnSalespersonTotals.Name = "btnSalespersonTotals";
            btnSalespersonTotals.Size = new Size(85, 28);
            btnSalespersonTotals.TabIndex = 3;
            btnSalespersonTotals.Text = "Person Total";
            btnSalespersonTotals.UseVisualStyleBackColor = true;
            btnSalespersonTotals.Click += btnSalespersonTotals_Click;
            // 
            // btnWarehouseTotals
            // 
            btnWarehouseTotals.Dock = DockStyle.Left;
            btnWarehouseTotals.Location = new Point(248, 0);
            btnWarehouseTotals.Name = "btnWarehouseTotals";
            btnWarehouseTotals.Size = new Size(82, 28);
            btnWarehouseTotals.TabIndex = 2;
            btnWarehouseTotals.Text = "Store Total";
            btnWarehouseTotals.UseVisualStyleBackColor = true;
            btnWarehouseTotals.Click += btnWarehouseTotals_Click;
            // 
            // dateTimePickerFrom
            // 
            dateTimePickerFrom.CalendarFont = new Font("Segoe UI", 9F);
            dateTimePickerFrom.Dock = DockStyle.Left;
            dateTimePickerFrom.Font = new Font("Segoe UI", 11F);
            dateTimePickerFrom.Location = new Point(75, 0);
            dateTimePickerFrom.Name = "dateTimePickerFrom";
            dateTimePickerFrom.Size = new Size(173, 27);
            dateTimePickerFrom.TabIndex = 1;
            // 
            // btnRun
            // 
            btnRun.Dock = DockStyle.Left;
            btnRun.Location = new Point(0, 0);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 28);
            btnRun.TabIndex = 0;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.AllowUserToAddRows = false;
            dataGridViewResults.AllowUserToDeleteRows = false;
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Dock = DockStyle.Fill;
            dataGridViewResults.Location = new Point(0, 28);
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.ReadOnly = true;
            dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.Size = new Size(800, 449);
            dataGridViewResults.TabIndex = 1;
            // 
            // Refunds
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(800, 477);
            Controls.Add(dataGridViewResults);
            Controls.Add(panel1);
            Name = "Refunds";
            Text = "Refunds";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnRun;
        private DataGridView dataGridViewResults;
        private DateTimePicker dateTimePickerFrom;
        private Button btnSalespersonTotals;
        private Button btnWarehouseTotals;
    }
}