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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
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
            tableLayoutPanel2.SuspendLayout();
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
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 2, 0);
            tableLayoutPanel1.Controls.Add(roundedPanel3, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1536, 648);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
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
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
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
    }
}