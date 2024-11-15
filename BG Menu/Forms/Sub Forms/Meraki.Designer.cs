namespace BG_Menu.Forms.Sub_Forms
{
    partial class Meraki
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
            textBoxProductNumber = new TextBox();
            dataGridViewProducts = new DataGridView();
            panel1 = new Panel();
            checkBoxShowDiscrepancies = new Class.Design.Custom_Items.ToggleSlider();
            label8 = new Label();
            checkBoxShowSerializedBatchItems = new Class.Design.Custom_Items.ToggleSlider();
            label6 = new Label();
            checkBoxShowAllProducts = new Class.Design.Custom_Items.ToggleSlider();
            label5 = new Label();
            comboBoxWarehouses = new ComboBox();
            comboBoxDatabases = new ComboBox();
            PrintDesc = new Button();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            buttonAmendScannedAmount = new Button();
            panel3 = new Panel();
            buttonExportMismatched = new Button();
            PrintSerial = new Button();
            label2 = new Label();
            label4 = new Label();
            textBoxLastScannedItemCode = new TextBox();
            label3 = new Label();
            textBoxLastScannedDescription = new TextBox();
            textBoxLastScannedQty = new TextBox();
            label1 = new Label();
            textBoxLastScannedOnHandQty = new TextBox();
            dataGridViewUnlistedProducts = new DataGridView();
            panel2 = new Panel();
            label7 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUnlistedProducts).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxProductNumber
            // 
            textBoxProductNumber.Dock = DockStyle.Left;
            textBoxProductNumber.Location = new Point(0, 0);
            textBoxProductNumber.Name = "textBoxProductNumber";
            textBoxProductNumber.Size = new Size(184, 23);
            textBoxProductNumber.TabIndex = 0;
            // 
            // dataGridViewProducts
            // 
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewProducts.Dock = DockStyle.Fill;
            dataGridViewProducts.Location = new Point(0, 0);
            dataGridViewProducts.Name = "dataGridViewProducts";
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.RowHeadersVisible = false;
            dataGridViewProducts.Size = new Size(577, 545);
            dataGridViewProducts.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(checkBoxShowDiscrepancies);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(checkBoxShowSerializedBatchItems);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(checkBoxShowAllProducts);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(comboBoxWarehouses);
            panel1.Controls.Add(comboBoxDatabases);
            panel1.Controls.Add(textBoxProductNumber);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(973, 24);
            panel1.TabIndex = 2;
            // 
            // checkBoxShowDiscrepancies
            // 
            checkBoxShowDiscrepancies.AutoSize = true;
            checkBoxShowDiscrepancies.Checked = true;
            checkBoxShowDiscrepancies.CheckState = CheckState.Checked;
            checkBoxShowDiscrepancies.Dock = DockStyle.Left;
            checkBoxShowDiscrepancies.Location = new Point(910, 0);
            checkBoxShowDiscrepancies.MinimumSize = new Size(45, 22);
            checkBoxShowDiscrepancies.Name = "checkBoxShowDiscrepancies";
            checkBoxShowDiscrepancies.OffBackColor = Color.Gray;
            checkBoxShowDiscrepancies.OffToggleColor = Color.Gainsboro;
            checkBoxShowDiscrepancies.OnBackColor = Color.MediumSlateBlue;
            checkBoxShowDiscrepancies.OnToggleColor = Color.WhiteSmoke;
            checkBoxShowDiscrepancies.Size = new Size(45, 24);
            checkBoxShowDiscrepancies.TabIndex = 7;
            checkBoxShowDiscrepancies.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Left;
            label8.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ButtonHighlight;
            label8.Location = new Point(813, 0);
            label8.Name = "label8";
            label8.Size = new Size(97, 24);
            label8.TabIndex = 8;
            label8.Text = "Discrep Only";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // checkBoxShowSerializedBatchItems
            // 
            checkBoxShowSerializedBatchItems.AutoSize = true;
            checkBoxShowSerializedBatchItems.Checked = true;
            checkBoxShowSerializedBatchItems.CheckState = CheckState.Checked;
            checkBoxShowSerializedBatchItems.Dock = DockStyle.Left;
            checkBoxShowSerializedBatchItems.Location = new Point(768, 0);
            checkBoxShowSerializedBatchItems.MinimumSize = new Size(45, 22);
            checkBoxShowSerializedBatchItems.Name = "checkBoxShowSerializedBatchItems";
            checkBoxShowSerializedBatchItems.OffBackColor = Color.Gray;
            checkBoxShowSerializedBatchItems.OffToggleColor = Color.Gainsboro;
            checkBoxShowSerializedBatchItems.OnBackColor = Color.MediumSlateBlue;
            checkBoxShowSerializedBatchItems.OnToggleColor = Color.WhiteSmoke;
            checkBoxShowSerializedBatchItems.Size = new Size(45, 24);
            checkBoxShowSerializedBatchItems.TabIndex = 4;
            checkBoxShowSerializedBatchItems.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Left;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = SystemColors.ButtonHighlight;
            label6.Location = new Point(677, 0);
            label6.Name = "label6";
            label6.Size = new Size(91, 24);
            label6.TabIndex = 6;
            label6.Text = "Hide Serials";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // checkBoxShowAllProducts
            // 
            checkBoxShowAllProducts.AutoSize = true;
            checkBoxShowAllProducts.Dock = DockStyle.Left;
            checkBoxShowAllProducts.Location = new Point(632, 0);
            checkBoxShowAllProducts.MinimumSize = new Size(45, 22);
            checkBoxShowAllProducts.Name = "checkBoxShowAllProducts";
            checkBoxShowAllProducts.OffBackColor = Color.Gray;
            checkBoxShowAllProducts.OffToggleColor = Color.Gainsboro;
            checkBoxShowAllProducts.OnBackColor = Color.MediumSlateBlue;
            checkBoxShowAllProducts.OnToggleColor = Color.WhiteSmoke;
            checkBoxShowAllProducts.Size = new Size(45, 24);
            checkBoxShowAllProducts.TabIndex = 3;
            checkBoxShowAllProducts.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Left;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonHighlight;
            label5.Location = new Point(554, 0);
            label5.Name = "label5";
            label5.Size = new Size(78, 24);
            label5.TabIndex = 5;
            label5.Text = "Show All";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBoxWarehouses
            // 
            comboBoxWarehouses.Dock = DockStyle.Left;
            comboBoxWarehouses.FormattingEnabled = true;
            comboBoxWarehouses.Location = new Point(305, 0);
            comboBoxWarehouses.Name = "comboBoxWarehouses";
            comboBoxWarehouses.Size = new Size(249, 23);
            comboBoxWarehouses.TabIndex = 2;
            // 
            // comboBoxDatabases
            // 
            comboBoxDatabases.Dock = DockStyle.Left;
            comboBoxDatabases.FormattingEnabled = true;
            comboBoxDatabases.Location = new Point(184, 0);
            comboBoxDatabases.Name = "comboBoxDatabases";
            comboBoxDatabases.Size = new Size(121, 23);
            comboBoxDatabases.TabIndex = 1;
            // 
            // PrintDesc
            // 
            PrintDesc.Dock = DockStyle.Left;
            PrintDesc.Location = new Point(0, 0);
            PrintDesc.Name = "PrintDesc";
            PrintDesc.Size = new Size(125, 25);
            PrintDesc.TabIndex = 7;
            PrintDesc.Text = "Print Discrepencies";
            PrintDesc.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridViewProducts);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(973, 545);
            splitContainer1.SplitterDistance = 577;
            splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(buttonAmendScannedAmount);
            splitContainer2.Panel1.Controls.Add(panel3);
            splitContainer2.Panel1.Controls.Add(label2);
            splitContainer2.Panel1.Controls.Add(label4);
            splitContainer2.Panel1.Controls.Add(textBoxLastScannedItemCode);
            splitContainer2.Panel1.Controls.Add(label3);
            splitContainer2.Panel1.Controls.Add(textBoxLastScannedDescription);
            splitContainer2.Panel1.Controls.Add(textBoxLastScannedQty);
            splitContainer2.Panel1.Controls.Add(label1);
            splitContainer2.Panel1.Controls.Add(textBoxLastScannedOnHandQty);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dataGridViewUnlistedProducts);
            splitContainer2.Panel2.Controls.Add(panel2);
            splitContainer2.Size = new Size(392, 545);
            splitContainer2.SplitterDistance = 185;
            splitContainer2.TabIndex = 8;
            // 
            // buttonAmendScannedAmount
            // 
            buttonAmendScannedAmount.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAmendScannedAmount.Location = new Point(301, 92);
            buttonAmendScannedAmount.Name = "buttonAmendScannedAmount";
            buttonAmendScannedAmount.Size = new Size(68, 29);
            buttonAmendScannedAmount.TabIndex = 9;
            buttonAmendScannedAmount.Text = "Override";
            buttonAmendScannedAmount.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(buttonExportMismatched);
            panel3.Controls.Add(PrintSerial);
            panel3.Controls.Add(PrintDesc);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 160);
            panel3.Name = "panel3";
            panel3.Size = new Size(392, 25);
            panel3.TabIndex = 8;
            // 
            // buttonExportMismatched
            // 
            buttonExportMismatched.Dock = DockStyle.Left;
            buttonExportMismatched.Location = new Point(250, 0);
            buttonExportMismatched.Name = "buttonExportMismatched";
            buttonExportMismatched.Size = new Size(125, 25);
            buttonExportMismatched.TabIndex = 9;
            buttonExportMismatched.Text = "Export";
            buttonExportMismatched.UseVisualStyleBackColor = true;
            // 
            // PrintSerial
            // 
            PrintSerial.Dock = DockStyle.Left;
            PrintSerial.Location = new Point(125, 0);
            PrintSerial.Name = "PrintSerial";
            PrintSerial.Size = new Size(125, 25);
            PrintSerial.TabIndex = 8;
            PrintSerial.Text = "Print Serialised";
            PrintSerial.UseVisualStyleBackColor = true;
            PrintSerial.Click += PrintSerial_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(22, 20);
            label2.Name = "label2";
            label2.Size = new Size(88, 21);
            label2.TabIndex = 5;
            label2.Text = "Item Code";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Location = new Point(22, 95);
            label4.Name = "label4";
            label4.Size = new Size(103, 21);
            label4.TabIndex = 7;
            label4.Text = "Stock Levels";
            // 
            // textBoxLastScannedItemCode
            // 
            textBoxLastScannedItemCode.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxLastScannedItemCode.Location = new Point(143, 17);
            textBoxLastScannedItemCode.Name = "textBoxLastScannedItemCode";
            textBoxLastScannedItemCode.ReadOnly = true;
            textBoxLastScannedItemCode.Size = new Size(226, 29);
            textBoxLastScannedItemCode.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(22, 57);
            label3.Name = "label3";
            label3.Size = new Size(98, 21);
            label3.TabIndex = 6;
            label3.Text = "Description";
            // 
            // textBoxLastScannedDescription
            // 
            textBoxLastScannedDescription.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxLastScannedDescription.Location = new Point(143, 54);
            textBoxLastScannedDescription.Name = "textBoxLastScannedDescription";
            textBoxLastScannedDescription.ReadOnly = true;
            textBoxLastScannedDescription.Size = new Size(226, 29);
            textBoxLastScannedDescription.TabIndex = 1;
            // 
            // textBoxLastScannedQty
            // 
            textBoxLastScannedQty.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxLastScannedQty.Location = new Point(143, 92);
            textBoxLastScannedQty.Name = "textBoxLastScannedQty";
            textBoxLastScannedQty.Size = new Size(61, 29);
            textBoxLastScannedQty.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(210, 95);
            label1.Name = "label1";
            label1.Size = new Size(17, 21);
            label1.TabIndex = 4;
            label1.Text = "/";
            // 
            // textBoxLastScannedOnHandQty
            // 
            textBoxLastScannedOnHandQty.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBoxLastScannedOnHandQty.Location = new Point(233, 92);
            textBoxLastScannedOnHandQty.Name = "textBoxLastScannedOnHandQty";
            textBoxLastScannedOnHandQty.ReadOnly = true;
            textBoxLastScannedOnHandQty.Size = new Size(61, 29);
            textBoxLastScannedOnHandQty.TabIndex = 3;
            // 
            // dataGridViewUnlistedProducts
            // 
            dataGridViewUnlistedProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUnlistedProducts.Dock = DockStyle.Fill;
            dataGridViewUnlistedProducts.Location = new Point(0, 27);
            dataGridViewUnlistedProducts.Name = "dataGridViewUnlistedProducts";
            dataGridViewUnlistedProducts.RowHeadersVisible = false;
            dataGridViewUnlistedProducts.Size = new Size(392, 329);
            dataGridViewUnlistedProducts.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(label7);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(392, 27);
            panel2.TabIndex = 0;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonHighlight;
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(392, 27);
            label7.TabIndex = 7;
            label7.Text = "Stock Not In SAP";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Meraki
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(973, 569);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Meraki";
            Text = "Meraki";
            ((System.ComponentModel.ISupportInitialize)dataGridViewProducts).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUnlistedProducts).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBoxProductNumber;
        private DataGridView dataGridViewProducts;
        private Panel panel1;
        private ComboBox comboBoxWarehouses;
        private ComboBox comboBoxDatabases;
        private Class.Design.Custom_Items.ToggleSlider checkBoxShowAllProducts;
        private SplitContainer splitContainer1;
        private Label label1;
        private TextBox textBoxLastScannedOnHandQty;
        private TextBox textBoxLastScannedQty;
        private TextBox textBoxLastScannedDescription;
        private TextBox textBoxLastScannedItemCode;
        private Label label4;
        private Label label3;
        private Label label2;
        private SplitContainer splitContainer2;
        private DataGridView dataGridViewUnlistedProducts;
        private Panel panel2;
        private Class.Design.Custom_Items.ToggleSlider checkBoxShowSerializedBatchItems;
        private Label label6;
        private Label label5;
        private Label label7;
        private Button PrintDesc;
        private Panel panel3;
        private Button PrintSerial;
        private Button buttonAmendScannedAmount;
        private Button buttonExportMismatched;
        private Class.Design.Custom_Items.ToggleSlider checkBoxShowDiscrepancies;
        private Label label8;
    }
}