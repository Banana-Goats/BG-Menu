namespace BG_Menu.Forms.Sub_Forms
{
    partial class EditComputerForm
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
            txtLocation = new TextBox();
            txtCompanyName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button1 = new Button();
            button2 = new Button();
            label5 = new Label();
            txtMachine = new TextBox();
            cmbType = new ComboBox();
            cmbCompany = new ComboBox();
            SuspendLayout();
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(144, 98);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(188, 23);
            txtLocation.TabIndex = 1;
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new Point(144, 168);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(188, 23);
            txtCompanyName.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(100, 67);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 4;
            label1.Text = "Type :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(79, 101);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 5;
            label2.Text = "Location :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(73, 137);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 6;
            label3.Text = "Company :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(38, 171);
            label4.Name = "label4";
            label4.Size = new Size(100, 15);
            label4.TabIndex = 7;
            label4.Text = "Company Name :";
            // 
            // button1
            // 
            button1.Location = new Point(75, 215);
            button1.Name = "button1";
            button1.Size = new Size(96, 33);
            button1.TabIndex = 8;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnSave_Click;
            // 
            // button2
            // 
            button2.Location = new Point(236, 215);
            button2.Name = "button2";
            button2.Size = new Size(96, 33);
            button2.TabIndex = 9;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnCancel_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(79, 33);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 11;
            label5.Text = "Machine :";
            // 
            // txtMachine
            // 
            txtMachine.Location = new Point(144, 30);
            txtMachine.Name = "txtMachine";
            txtMachine.Size = new Size(188, 23);
            txtMachine.TabIndex = 10;
            // 
            // cmbType
            // 
            cmbType.FormattingEnabled = true;
            cmbType.Location = new Point(145, 64);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(187, 23);
            cmbType.TabIndex = 12;
            // 
            // cmbCompany
            // 
            cmbCompany.FormattingEnabled = true;
            cmbCompany.Location = new Point(145, 134);
            cmbCompany.Name = "cmbCompany";
            cmbCompany.Size = new Size(187, 23);
            cmbCompany.TabIndex = 13;
            // 
            // EditComputerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(397, 280);
            Controls.Add(cmbCompany);
            Controls.Add(cmbType);
            Controls.Add(label5);
            Controls.Add(txtMachine);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtCompanyName);
            Controls.Add(txtLocation);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EditComputerForm";
            Text = "EditComputerForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtLocation;
        private TextBox txtCompanyName;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button button1;
        private Button button2;
        private Label label5;
        private TextBox txtMachine;
        private ComboBox cmbType;
        private ComboBox cmbCompany;
    }
}