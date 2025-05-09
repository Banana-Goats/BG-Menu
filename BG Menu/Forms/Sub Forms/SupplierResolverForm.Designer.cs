namespace BG_Menu.Forms.Sub_Forms
{
    partial class SupplierResolverForm
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
            richTextBox1 = new RichTextBox();
            cmbSuppliers = new ComboBox();
            panel1 = new Panel();
            txtGLName = new TextBox();
            cboVATCode = new ComboBox();
            cboGLAccount = new ComboBox();
            btnSave = new Button();
            txtTotalRegex = new TextBox();
            btnCaptureTotalExample = new Button();
            btnCaptureKeyword = new Button();
            txtNewSupplierName = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(958, 550);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // cmbSuppliers
            // 
            cmbSuppliers.FormattingEnabled = true;
            cmbSuppliers.Location = new Point(6, 12);
            cmbSuppliers.Name = "cmbSuppliers";
            cmbSuppliers.Size = new Size(182, 23);
            cmbSuppliers.TabIndex = 1;
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtGLName);
            panel1.Controls.Add(cboVATCode);
            panel1.Controls.Add(cboGLAccount);
            panel1.Controls.Add(btnSave);
            panel1.Controls.Add(txtTotalRegex);
            panel1.Controls.Add(btnCaptureTotalExample);
            panel1.Controls.Add(btnCaptureKeyword);
            panel1.Controls.Add(txtNewSupplierName);
            panel1.Controls.Add(cmbSuppliers);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(958, 0);
            panel1.MaximumSize = new Size(200, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 550);
            panel1.TabIndex = 2;
            // 
            // txtGLName
            // 
            txtGLName.Location = new Point(6, 295);
            txtGLName.Name = "txtGLName";
            txtGLName.ReadOnly = true;
            txtGLName.Size = new Size(182, 23);
            txtGLName.TabIndex = 13;
            // 
            // cboVATCode
            // 
            cboVATCode.FormattingEnabled = true;
            cboVATCode.Location = new Point(6, 324);
            cboVATCode.Name = "cboVATCode";
            cboVATCode.Size = new Size(182, 23);
            cboVATCode.TabIndex = 8;
            // 
            // cboGLAccount
            // 
            cboGLAccount.FormattingEnabled = true;
            cboGLAccount.Location = new Point(6, 266);
            cboGLAccount.Name = "cboGLAccount";
            cboGLAccount.Size = new Size(182, 23);
            cboGLAccount.TabIndex = 7;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(6, 524);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(182, 23);
            btnSave.TabIndex = 6;
            btnSave.Text = "button2";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtTotalRegex
            // 
            txtTotalRegex.Location = new Point(6, 128);
            txtTotalRegex.Multiline = true;
            txtTotalRegex.Name = "txtTotalRegex";
            txtTotalRegex.Size = new Size(182, 132);
            txtTotalRegex.TabIndex = 5;
            // 
            // btnCaptureTotalExample
            // 
            btnCaptureTotalExample.Location = new Point(6, 99);
            btnCaptureTotalExample.Name = "btnCaptureTotalExample";
            btnCaptureTotalExample.Size = new Size(182, 23);
            btnCaptureTotalExample.TabIndex = 4;
            btnCaptureTotalExample.Text = "Total";
            btnCaptureTotalExample.UseVisualStyleBackColor = true;
            btnCaptureTotalExample.Click += btnCaptureTotalExample_Click;
            // 
            // btnCaptureKeyword
            // 
            btnCaptureKeyword.Location = new Point(6, 70);
            btnCaptureKeyword.Name = "btnCaptureKeyword";
            btnCaptureKeyword.Size = new Size(182, 23);
            btnCaptureKeyword.TabIndex = 3;
            btnCaptureKeyword.Text = "Keyword";
            btnCaptureKeyword.UseVisualStyleBackColor = true;
            btnCaptureKeyword.Click += btnCaptureKeyword_Click;
            // 
            // txtNewSupplierName
            // 
            txtNewSupplierName.Location = new Point(6, 41);
            txtNewSupplierName.Name = "txtNewSupplierName";
            txtNewSupplierName.PlaceholderText = "Supplier";
            txtNewSupplierName.Size = new Size(182, 23);
            txtNewSupplierName.TabIndex = 2;
            txtNewSupplierName.TextAlign = HorizontalAlignment.Center;
            // 
            // SupplierResolverForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1158, 550);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Name = "SupplierResolverForm";
            Text = "Supplier Builder";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private ComboBox cmbSuppliers;
        private Panel panel1;
        private Button btnCaptureTotalExample;
        private Button btnCaptureKeyword;
        private TextBox txtNewSupplierName;
        private Button btnSave;
        private TextBox txtTotalRegex;
        private ComboBox cboVATCode;
        private ComboBox cboGLAccount;
        private TextBox txtGLName;
    }
}