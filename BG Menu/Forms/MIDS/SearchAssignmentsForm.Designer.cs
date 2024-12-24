namespace BG_Menu.Forms.Sub_Forms
{
    partial class SearchAssignmentsForm
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
            dtpTargetDateTime = new DateTimePicker();
            txtPTID = new TextBox();
            txtTID = new TextBox();
            txtMerchantID = new TextBox();
            btnSearch = new Button();
            btnCancel = new Button();
            dataGridViewResults = new DataGridView();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            SuspendLayout();
            // 
            // dtpTargetDateTime
            // 
            dtpTargetDateTime.Location = new Point(12, 12);
            dtpTargetDateTime.Name = "dtpTargetDateTime";
            dtpTargetDateTime.Size = new Size(200, 23);
            dtpTargetDateTime.TabIndex = 0;
            dtpTargetDateTime.ValueChanged += dtpTargetDateTime_ValueChanged;
            // 
            // txtPTID
            // 
            txtPTID.Location = new Point(218, 12);
            txtPTID.Name = "txtPTID";
            txtPTID.PlaceholderText = "Personal Terminal ID";
            txtPTID.Size = new Size(200, 23);
            txtPTID.TabIndex = 1;
            // 
            // txtTID
            // 
            txtTID.Location = new Point(12, 41);
            txtTID.Name = "txtTID";
            txtTID.PlaceholderText = "Terminal ID";
            txtTID.Size = new Size(200, 23);
            txtTID.TabIndex = 2;
            // 
            // txtMerchantID
            // 
            txtMerchantID.Location = new Point(218, 41);
            txtMerchantID.Name = "txtMerchantID";
            txtMerchantID.PlaceholderText = "Merchant ID";
            txtMerchantID.Size = new Size(200, 23);
            txtMerchantID.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(12, 70);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(96, 23);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(116, 70);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(96, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Close";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Location = new Point(14, 104);
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.Size = new Size(406, 378);
            dataGridViewResults.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(218, 74);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 7;
            label1.Text = "label1";
            // 
            // SearchAssignmentsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(431, 498);
            Controls.Add(label1);
            Controls.Add(dataGridViewResults);
            Controls.Add(btnCancel);
            Controls.Add(btnSearch);
            Controls.Add(txtMerchantID);
            Controls.Add(txtTID);
            Controls.Add(txtPTID);
            Controls.Add(dtpTargetDateTime);
            Name = "SearchAssignmentsForm";
            Text = "SearchAssignmentsForm";
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpTargetDateTime;
        private TextBox txtPTID;
        private TextBox txtTID;
        private TextBox txtMerchantID;
        private Button btnSearch;
        private Button btnCancel;
        private DataGridView dataGridViewResults;
        private Label label1;
    }
}