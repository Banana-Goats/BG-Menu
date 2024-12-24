namespace BG_Menu.Forms.Sub_Forms
{
    partial class PaymentDeviceManager
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
            txtMerchantID = new TextBox();
            cmbMerchant = new ComboBox();
            txtTID = new TextBox();
            txtPTID = new TextBox();
            txtDevice = new TextBox();
            txtSerialNumber = new TextBox();
            txtCompany = new TextBox();
            txtAssignedUser = new TextBox();
            txtDepartmentStore = new TextBox();
            txtPCIDSSVersion = new TextBox();
            txtPCIDSSPassword = new TextBox();
            btnSave = new Button();
            dtpPCIDSSDate = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            btnDelete = new Button();
            txtChangedBy = new TextBox();
            txtChangeDate = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // txtMerchantID
            // 
            txtMerchantID.Location = new Point(179, 32);
            txtMerchantID.Name = "txtMerchantID";
            txtMerchantID.Size = new Size(200, 23);
            txtMerchantID.TabIndex = 0;
            // 
            // cmbMerchant
            // 
            cmbMerchant.FormattingEnabled = true;
            cmbMerchant.Items.AddRange(new object[] { "Elavon", "Worldpay" });
            cmbMerchant.Location = new Point(179, 61);
            cmbMerchant.Name = "cmbMerchant";
            cmbMerchant.Size = new Size(200, 23);
            cmbMerchant.TabIndex = 1;
            // 
            // txtTID
            // 
            txtTID.Location = new Point(179, 90);
            txtTID.Name = "txtTID";
            txtTID.Size = new Size(200, 23);
            txtTID.TabIndex = 2;
            // 
            // txtPTID
            // 
            txtPTID.Location = new Point(179, 119);
            txtPTID.Name = "txtPTID";
            txtPTID.Size = new Size(200, 23);
            txtPTID.TabIndex = 3;
            // 
            // txtDevice
            // 
            txtDevice.Location = new Point(179, 148);
            txtDevice.Name = "txtDevice";
            txtDevice.Size = new Size(200, 23);
            txtDevice.TabIndex = 4;
            // 
            // txtSerialNumber
            // 
            txtSerialNumber.Location = new Point(179, 177);
            txtSerialNumber.Name = "txtSerialNumber";
            txtSerialNumber.Size = new Size(200, 23);
            txtSerialNumber.TabIndex = 5;
            // 
            // txtCompany
            // 
            txtCompany.Location = new Point(179, 206);
            txtCompany.Name = "txtCompany";
            txtCompany.Size = new Size(200, 23);
            txtCompany.TabIndex = 6;
            // 
            // txtAssignedUser
            // 
            txtAssignedUser.Location = new Point(179, 235);
            txtAssignedUser.Name = "txtAssignedUser";
            txtAssignedUser.Size = new Size(200, 23);
            txtAssignedUser.TabIndex = 7;
            // 
            // txtDepartmentStore
            // 
            txtDepartmentStore.Location = new Point(179, 264);
            txtDepartmentStore.Name = "txtDepartmentStore";
            txtDepartmentStore.Size = new Size(200, 23);
            txtDepartmentStore.TabIndex = 8;
            // 
            // txtPCIDSSVersion
            // 
            txtPCIDSSVersion.Location = new Point(179, 322);
            txtPCIDSSVersion.Name = "txtPCIDSSVersion";
            txtPCIDSSVersion.Size = new Size(200, 23);
            txtPCIDSSVersion.TabIndex = 9;
            // 
            // txtPCIDSSPassword
            // 
            txtPCIDSSPassword.Location = new Point(179, 351);
            txtPCIDSSPassword.Name = "txtPCIDSSPassword";
            txtPCIDSSPassword.Size = new Size(200, 23);
            txtPCIDSSPassword.TabIndex = 10;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(85, 380);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(93, 30);
            btnSave.TabIndex = 12;
            btnSave.Text = "Add Record";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // dtpPCIDSSDate
            // 
            dtpPCIDSSDate.Format = DateTimePickerFormat.Short;
            dtpPCIDSSDate.Location = new Point(179, 293);
            dtpPCIDSSDate.Name = "dtpPCIDSSDate";
            dtpPCIDSSDate.Size = new Size(200, 23);
            dtpPCIDSSDate.TabIndex = 13;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(95, 35);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 14;
            label1.Text = "Merchant ID :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.Control;
            label2.Location = new Point(106, 64);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 15;
            label2.Text = "Merchant  :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.Control;
            label3.Location = new Point(135, 122);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 17;
            label3.Text = "PTID :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.Control;
            label4.Location = new Point(142, 93);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 16;
            label4.Text = "TID :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = SystemColors.Control;
            label5.Location = new Point(85, 180);
            label5.Name = "label5";
            label5.Size = new Size(88, 15);
            label5.TabIndex = 19;
            label5.Text = "Serial Number :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.Control;
            label6.Location = new Point(125, 151);
            label6.Name = "label6";
            label6.Size = new Size(48, 15);
            label6.TabIndex = 18;
            label6.Text = "Device :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = SystemColors.Control;
            label7.Location = new Point(66, 354);
            label7.Name = "label7";
            label7.Size = new Size(107, 15);
            label7.TabIndex = 25;
            label7.Text = "PCI DSS Password :";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.Control;
            label8.Location = new Point(78, 325);
            label8.Name = "label8";
            label8.Size = new Size(95, 15);
            label8.TabIndex = 24;
            label8.Text = "PCI DSS Version :";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = SystemColors.Control;
            label9.Location = new Point(95, 299);
            label9.Name = "label9";
            label9.Size = new Size(78, 15);
            label9.TabIndex = 23;
            label9.Text = "PCI DSS Date:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = SystemColors.Control;
            label10.Location = new Point(59, 267);
            label10.Name = "label10";
            label10.Size = new Size(114, 15);
            label10.TabIndex = 22;
            label10.Text = "Department / Store :";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = SystemColors.Control;
            label11.Location = new Point(86, 238);
            label11.Name = "label11";
            label11.Size = new Size(87, 15);
            label11.TabIndex = 21;
            label11.Text = "Assigned User :";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.ForeColor = SystemColors.Control;
            label12.Location = new Point(108, 209);
            label12.Name = "label12";
            label12.Size = new Size(65, 15);
            label12.TabIndex = 20;
            label12.Text = "Company :";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(187, 380);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(93, 30);
            btnDelete.TabIndex = 26;
            btnDelete.Text = "Delete Record";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtChangedBy
            // 
            txtChangedBy.Location = new Point(50, 416);
            txtChangedBy.Name = "txtChangedBy";
            txtChangedBy.ReadOnly = true;
            txtChangedBy.Size = new Size(161, 23);
            txtChangedBy.TabIndex = 27;
            // 
            // txtChangeDate
            // 
            txtChangeDate.Location = new Point(218, 416);
            txtChangeDate.Name = "txtChangeDate";
            txtChangeDate.ReadOnly = true;
            txtChangeDate.Size = new Size(161, 23);
            txtChangeDate.TabIndex = 28;
            // 
            // button1
            // 
            button1.Location = new Point(286, 380);
            button1.Name = "button1";
            button1.Size = new Size(93, 30);
            button1.TabIndex = 29;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PaymentDeviceManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(428, 479);
            Controls.Add(button1);
            Controls.Add(txtChangeDate);
            Controls.Add(txtChangedBy);
            Controls.Add(btnDelete);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(label11);
            Controls.Add(label12);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dtpPCIDSSDate);
            Controls.Add(btnSave);
            Controls.Add(txtPCIDSSPassword);
            Controls.Add(txtPCIDSSVersion);
            Controls.Add(txtDepartmentStore);
            Controls.Add(txtAssignedUser);
            Controls.Add(txtCompany);
            Controls.Add(txtSerialNumber);
            Controls.Add(txtDevice);
            Controls.Add(txtPTID);
            Controls.Add(txtTID);
            Controls.Add(cmbMerchant);
            Controls.Add(txtMerchantID);
            FormBorderStyle = FormBorderStyle.None;
            Name = "PaymentDeviceManager";
            Text = "PaymentDeviceManager";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtMerchantID;
        private ComboBox cmbMerchant;
        private TextBox txtTID;
        private TextBox txtPTID;
        private TextBox txtDevice;
        private TextBox txtSerialNumber;
        private TextBox txtCompany;
        private TextBox txtAssignedUser;
        private TextBox txtDepartmentStore;
        private TextBox txtPCIDSSVersion;
        private TextBox txtPCIDSSPassword;
        private Button btnSave;
        private DateTimePicker dtpPCIDSSDate;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Button btnDelete;
        private TextBox txtChangedBy;
        private TextBox txtChangeDate;
        private Button button1;
    }
}