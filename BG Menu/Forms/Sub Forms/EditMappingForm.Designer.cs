namespace BG_Menu.Forms.Sub_Forms
{
    partial class EditMappingForm
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
            cmbCompany = new ComboBox();
            label5 = new Label();
            txtMachine = new TextBox();
            button2 = new Button();
            button1 = new Button();
            label3 = new Label();
            label1 = new Label();
            txtMapping = new TextBox();
            SuspendLayout();
            // 
            // cmbCompany
            // 
            cmbCompany.FormattingEnabled = true;
            cmbCompany.Location = new Point(158, 70);
            cmbCompany.Name = "cmbCompany";
            cmbCompany.Size = new Size(187, 23);
            cmbCompany.TabIndex = 25;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(92, 34);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 23;
            label5.Text = "Machine :";
            // 
            // txtMachine
            // 
            txtMachine.Location = new Point(157, 31);
            txtMachine.Name = "txtMachine";
            txtMachine.Size = new Size(188, 23);
            txtMachine.TabIndex = 22;
            // 
            // button2
            // 
            button2.Location = new Point(239, 177);
            button2.Name = "button2";
            button2.Size = new Size(96, 33);
            button2.TabIndex = 21;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnCancel_Click;
            // 
            // button1
            // 
            button1.Location = new Point(78, 177);
            button1.Name = "button1";
            button1.Size = new Size(96, 33);
            button1.TabIndex = 20;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnSave_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(86, 73);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 18;
            label3.Text = "Company :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(88, 114);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 27;
            label1.Text = "Mapping :";
            // 
            // txtMapping
            // 
            txtMapping.Location = new Point(158, 111);
            txtMapping.Name = "txtMapping";
            txtMapping.Size = new Size(188, 23);
            txtMapping.TabIndex = 26;
            // 
            // EditMappingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(397, 267);
            Controls.Add(label1);
            Controls.Add(txtMapping);
            Controls.Add(cmbCompany);
            Controls.Add(label5);
            Controls.Add(txtMachine);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EditMappingForm";
            Text = "EditMappingForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbCompany;
        private Label label5;
        private TextBox txtMachine;
        private Button button2;
        private Button button1;
        private Label label3;
        private Label label1;
        private TextBox txtMapping;
    }
}