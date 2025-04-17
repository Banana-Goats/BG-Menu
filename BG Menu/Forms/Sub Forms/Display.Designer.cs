namespace BG_Menu.Forms.Sub_Forms
{
    partial class Display
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            roundedPanel7 = new Class.Design.Custom_Items.RoundedPanel();
            label12 = new Label();
            dataGridViewCompanies = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            roundedPanel4 = new Class.Design.Custom_Items.RoundedPanel();
            lblDifference = new Label();
            lblLatestTotal = new Label();
            lblLastRunTime = new Label();
            label4 = new Label();
            roundedPanel2 = new Class.Design.Custom_Items.RoundedPanel();
            button1 = new Button();
            progressBarSAP = new ProgressBar();
            labelVatFormCount = new Label();
            label2 = new Label();
            roundedPanel1 = new Class.Design.Custom_Items.RoundedPanel();
            progressTextBox = new RichTextBox();
            lblCallsProcessed = new Label();
            lblLastProcessed = new Label();
            label1 = new Label();
            roundedPanel3 = new Class.Design.Custom_Items.RoundedPanel();
            flowLayoutPanelTiles = new FlowLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            roundedPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompanies).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            roundedPanel4.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel1.SuspendLayout();
            roundedPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(46, 51, 73);
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.9999962F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0000076F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 0);
            tableLayoutPanel1.Controls.Add(roundedPanel3, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1536, 648);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(roundedPanel7, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(370, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(362, 642);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // roundedPanel7
            // 
            roundedPanel7.BackColor = Color.Transparent;
            roundedPanel7.BorderColor = Color.White;
            roundedPanel7.BorderRadius = 20;
            roundedPanel7.BorderWidth = 3;
            roundedPanel7.Controls.Add(label12);
            roundedPanel7.Controls.Add(dataGridViewCompanies);
            roundedPanel7.Dock = DockStyle.Top;
            roundedPanel7.Location = new Point(3, 3);
            roundedPanel7.Name = "roundedPanel7";
            roundedPanel7.Padding = new Padding(10);
            roundedPanel7.Size = new Size(356, 349);
            roundedPanel7.TabIndex = 3;
            // 
            // label12
            // 
            label12.Dock = DockStyle.Top;
            label12.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline);
            label12.ForeColor = Color.White;
            label12.Location = new Point(10, 10);
            label12.Name = "label12";
            label12.Size = new Size(336, 50);
            label12.TabIndex = 6;
            label12.Text = "* FSM Users *";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dataGridViewCompanies
            // 
            dataGridViewCompanies.AllowUserToAddRows = false;
            dataGridViewCompanies.AllowUserToDeleteRows = false;
            dataGridViewCompanies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCompanies.BackgroundColor = Color.FromArgb(46, 51, 73);
            dataGridViewCompanies.BorderStyle = BorderStyle.None;
            dataGridViewCompanies.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridViewCompanies.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = Color.Transparent;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewCompanies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCompanies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(46, 51, 73);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewCompanies.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCompanies.Dock = DockStyle.Bottom;
            dataGridViewCompanies.GridColor = Color.White;
            dataGridViewCompanies.Location = new Point(10, 60);
            dataGridViewCompanies.MultiSelect = false;
            dataGridViewCompanies.Name = "dataGridViewCompanies";
            dataGridViewCompanies.ReadOnly = true;
            dataGridViewCompanies.RowHeadersVisible = false;
            dataGridViewCompanies.Size = new Size(336, 279);
            dataGridViewCompanies.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(roundedPanel4, 0, 2);
            tableLayoutPanel2.Controls.Add(roundedPanel2, 0, 1);
            tableLayoutPanel2.Controls.Add(roundedPanel1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(738, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(394, 642);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // roundedPanel4
            // 
            roundedPanel4.AutoSize = true;
            roundedPanel4.BackColor = Color.Transparent;
            roundedPanel4.BorderColor = Color.White;
            roundedPanel4.BorderRadius = 20;
            roundedPanel4.BorderWidth = 3;
            roundedPanel4.Controls.Add(lblDifference);
            roundedPanel4.Controls.Add(lblLatestTotal);
            roundedPanel4.Controls.Add(lblLastRunTime);
            roundedPanel4.Controls.Add(label4);
            roundedPanel4.Dock = DockStyle.Top;
            roundedPanel4.Location = new Point(3, 379);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(10);
            roundedPanel4.Size = new Size(388, 175);
            roundedPanel4.TabIndex = 5;
            // 
            // lblDifference
            // 
            lblDifference.AutoSize = true;
            lblDifference.Dock = DockStyle.Top;
            lblDifference.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDifference.ForeColor = Color.White;
            lblDifference.Location = new Point(10, 130);
            lblDifference.Name = "lblDifference";
            lblDifference.Padding = new Padding(0, 5, 0, 5);
            lblDifference.Size = new Size(65, 35);
            lblDifference.TabIndex = 9;
            lblDifference.Text = "label1";
            lblDifference.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLatestTotal
            // 
            lblLatestTotal.AutoSize = true;
            lblLatestTotal.Dock = DockStyle.Top;
            lblLatestTotal.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLatestTotal.ForeColor = Color.White;
            lblLatestTotal.Location = new Point(10, 95);
            lblLatestTotal.Name = "lblLatestTotal";
            lblLatestTotal.Padding = new Padding(0, 5, 0, 5);
            lblLatestTotal.Size = new Size(65, 35);
            lblLatestTotal.TabIndex = 8;
            lblLatestTotal.Text = "label1";
            lblLatestTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLastRunTime
            // 
            lblLastRunTime.AutoSize = true;
            lblLastRunTime.Dock = DockStyle.Top;
            lblLastRunTime.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLastRunTime.ForeColor = Color.White;
            lblLastRunTime.Location = new Point(10, 60);
            lblLastRunTime.Name = "lblLastRunTime";
            lblLastRunTime.Padding = new Padding(0, 5, 0, 5);
            lblLastRunTime.Size = new Size(65, 35);
            lblLastRunTime.TabIndex = 4;
            lblLastRunTime.Text = "label1";
            lblLastRunTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline);
            label4.ForeColor = Color.White;
            label4.Location = new Point(10, 10);
            label4.Name = "label4";
            label4.Size = new Size(368, 50);
            label4.TabIndex = 7;
            label4.Text = "* Sales Data *";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.Transparent;
            roundedPanel2.BorderColor = Color.White;
            roundedPanel2.BorderRadius = 20;
            roundedPanel2.BorderWidth = 3;
            roundedPanel2.Controls.Add(button1);
            roundedPanel2.Controls.Add(progressBarSAP);
            roundedPanel2.Controls.Add(labelVatFormCount);
            roundedPanel2.Controls.Add(label2);
            roundedPanel2.Dock = DockStyle.Top;
            roundedPanel2.Location = new Point(3, 213);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(10);
            roundedPanel2.Size = new Size(388, 160);
            roundedPanel2.TabIndex = 4;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Bottom;
            button1.Location = new Point(10, 127);
            button1.Name = "button1";
            button1.Size = new Size(368, 23);
            button1.TabIndex = 5;
            button1.Text = "Manual";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // progressBarSAP
            // 
            progressBarSAP.Dock = DockStyle.Top;
            progressBarSAP.Location = new Point(10, 95);
            progressBarSAP.Name = "progressBarSAP";
            progressBarSAP.Size = new Size(368, 24);
            progressBarSAP.TabIndex = 3;
            // 
            // labelVatFormCount
            // 
            labelVatFormCount.AutoSize = true;
            labelVatFormCount.Dock = DockStyle.Top;
            labelVatFormCount.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelVatFormCount.ForeColor = Color.White;
            labelVatFormCount.Location = new Point(10, 60);
            labelVatFormCount.Name = "labelVatFormCount";
            labelVatFormCount.Padding = new Padding(0, 5, 0, 5);
            labelVatFormCount.Size = new Size(65, 35);
            labelVatFormCount.TabIndex = 4;
            labelVatFormCount.Text = "label1";
            labelVatFormCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline);
            label2.ForeColor = Color.White;
            label2.Location = new Point(10, 10);
            label2.Name = "label2";
            label2.Size = new Size(368, 50);
            label2.TabIndex = 7;
            label2.Text = "* Vat Forms *";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // roundedPanel1
            // 
            roundedPanel1.AutoSize = true;
            roundedPanel1.BackColor = Color.Transparent;
            roundedPanel1.BorderColor = Color.White;
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.BorderWidth = 3;
            roundedPanel1.Controls.Add(progressTextBox);
            roundedPanel1.Controls.Add(lblCallsProcessed);
            roundedPanel1.Controls.Add(lblLastProcessed);
            roundedPanel1.Controls.Add(label1);
            roundedPanel1.Dock = DockStyle.Top;
            roundedPanel1.Location = new Point(3, 3);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(10);
            roundedPanel1.Size = new Size(388, 204);
            roundedPanel1.TabIndex = 3;
            // 
            // progressTextBox
            // 
            progressTextBox.BackColor = Color.FromArgb(46, 51, 73);
            progressTextBox.BorderStyle = BorderStyle.None;
            progressTextBox.Dock = DockStyle.Top;
            progressTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            progressTextBox.ForeColor = Color.White;
            progressTextBox.Location = new Point(10, 130);
            progressTextBox.Name = "progressTextBox";
            progressTextBox.ScrollBars = RichTextBoxScrollBars.None;
            progressTextBox.Size = new Size(368, 64);
            progressTextBox.TabIndex = 1;
            progressTextBox.Text = "";
            // 
            // lblCallsProcessed
            // 
            lblCallsProcessed.AutoSize = true;
            lblCallsProcessed.Dock = DockStyle.Top;
            lblCallsProcessed.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCallsProcessed.ForeColor = Color.White;
            lblCallsProcessed.Location = new Point(10, 95);
            lblCallsProcessed.Name = "lblCallsProcessed";
            lblCallsProcessed.Padding = new Padding(0, 5, 0, 5);
            lblCallsProcessed.Size = new Size(273, 35);
            lblCallsProcessed.TabIndex = 1;
            lblCallsProcessed.Text = "Calls Processed In Last Batch: ";
            // 
            // lblLastProcessed
            // 
            lblLastProcessed.AutoSize = true;
            lblLastProcessed.Dock = DockStyle.Top;
            lblLastProcessed.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLastProcessed.ForeColor = Color.White;
            lblLastProcessed.Location = new Point(10, 60);
            lblLastProcessed.Name = "lblLastProcessed";
            lblLastProcessed.Padding = new Padding(0, 5, 0, 5);
            lblLastProcessed.Size = new Size(273, 35);
            lblLastProcessed.TabIndex = 0;
            lblLastProcessed.Text = "Calls Processed In Last Batch: ";
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Underline);
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 10);
            label1.Name = "label1";
            label1.Size = new Size(368, 50);
            label1.TabIndex = 6;
            label1.Text = "* Call Recordings *";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.Transparent;
            roundedPanel3.BorderColor = Color.White;
            roundedPanel3.BorderRadius = 20;
            roundedPanel3.BorderWidth = 3;
            roundedPanel3.Controls.Add(flowLayoutPanelTiles);
            roundedPanel3.Dock = DockStyle.Fill;
            roundedPanel3.Location = new Point(1138, 3);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(10);
            roundedPanel3.Size = new Size(395, 642);
            roundedPanel3.TabIndex = 3;
            // 
            // flowLayoutPanelTiles
            // 
            flowLayoutPanelTiles.Dock = DockStyle.Fill;
            flowLayoutPanelTiles.Location = new Point(10, 10);
            flowLayoutPanelTiles.Name = "flowLayoutPanelTiles";
            flowLayoutPanelTiles.Size = new Size(375, 622);
            flowLayoutPanelTiles.TabIndex = 0;
            // 
            // Display
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1536, 648);
            Controls.Add(tableLayoutPanel1);
            Name = "Display";
            Text = "Display";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            roundedPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompanies).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanelTiles;
        private RichTextBox progressTextBox;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblCallsProcessed;
        private Label lblLastProcessed;
        private ProgressBar progressBarSAP;
        private Label labelVatFormCount;
        private Button button1;
        private Label label2;
        private Label label1;
        private Class.Design.Custom_Items.RoundedPanel roundedPanel1;
        private Class.Design.Custom_Items.RoundedPanel roundedPanel2;
        private Class.Design.Custom_Items.RoundedPanel roundedPanel3;
        private Class.Design.Custom_Items.RoundedPanel roundedPanel4;
        private Label lblDifference;
        private Label lblLatestTotal;
        private Label lblLastRunTime;
        private Label label4;
        private DataGridView dataGridViewCompanies;
        private TableLayoutPanel tableLayoutPanel3;
        private Class.Design.Custom_Items.RoundedPanel roundedPanel7;
        private Label label12;
    }
}