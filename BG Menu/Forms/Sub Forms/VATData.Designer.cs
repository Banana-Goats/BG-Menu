namespace BG_Menu.Forms.Sub_Forms
{
    partial class VATData
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
            button3 = new Button();
            labelCurrentPostcode = new Label();
            progressBarSAP = new ProgressBar();
            dateTimePickerEnd = new DateTimePicker();
            dateTimePickerStart = new DateTimePicker();
            button2 = new Button();
            textBox2 = new TextBox();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button3);
            panel1.Controls.Add(labelCurrentPostcode);
            panel1.Controls.Add(progressBarSAP);
            panel1.Controls.Add(dateTimePickerEnd);
            panel1.Controls.Add(dateTimePickerStart);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1473, 28);
            panel1.TabIndex = 0;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Left;
            button3.Location = new Point(650, 0);
            button3.Name = "button3";
            button3.Size = new Size(75, 28);
            button3.TabIndex = 7;
            button3.Text = "Generate";
            button3.UseVisualStyleBackColor = true;
            // 
            // labelCurrentPostcode
            // 
            labelCurrentPostcode.AutoSize = true;
            labelCurrentPostcode.Location = new Point(656, 8);
            labelCurrentPostcode.Name = "labelCurrentPostcode";
            labelCurrentPostcode.Size = new Size(38, 15);
            labelCurrentPostcode.TabIndex = 6;
            labelCurrentPostcode.Text = "label1";
            // 
            // progressBarSAP
            // 
            progressBarSAP.Dock = DockStyle.Left;
            progressBarSAP.Location = new Point(440, 0);
            progressBarSAP.Name = "progressBarSAP";
            progressBarSAP.Size = new Size(210, 28);
            progressBarSAP.TabIndex = 5;
            // 
            // dateTimePickerEnd
            // 
            dateTimePickerEnd.CalendarFont = new Font("Segoe UI", 9.75F);
            dateTimePickerEnd.Dock = DockStyle.Left;
            dateTimePickerEnd.Font = new Font("Segoe UI", 10F);
            dateTimePickerEnd.Location = new Point(295, 0);
            dateTimePickerEnd.Name = "dateTimePickerEnd";
            dateTimePickerEnd.Size = new Size(145, 25);
            dateTimePickerEnd.TabIndex = 4;
            dateTimePickerEnd.Value = new DateTime(2022, 1, 31, 0, 0, 0, 0);
            // 
            // dateTimePickerStart
            // 
            dateTimePickerStart.CalendarFont = new Font("Segoe UI", 9.75F);
            dateTimePickerStart.Dock = DockStyle.Left;
            dateTimePickerStart.Font = new Font("Segoe UI", 10F);
            dateTimePickerStart.Location = new Point(150, 0);
            dateTimePickerStart.Name = "dateTimePickerStart";
            dateTimePickerStart.Size = new Size(145, 25);
            dateTimePickerStart.TabIndex = 3;
            dateTimePickerStart.Value = new DateTime(2022, 1, 1, 8, 20, 0, 0);
            // 
            // button2
            // 
            button2.Dock = DockStyle.Left;
            button2.Location = new Point(75, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 28);
            button2.TabIndex = 2;
            button2.Text = "SAP Pull";
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Right;
            textBox2.Location = new Point(1291, 0);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(182, 23);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Left;
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(75, 28);
            button1.TabIndex = 0;
            button1.Text = "API Pull";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Size = new Size(1473, 570);
            dataGridView1.TabIndex = 1;
            // 
            // VATData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1473, 598);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            Name = "VATData";
            Text = "VATData";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private DataGridView dataGridView1;
        private TextBox textBox2;
        private Button button2;
        private DateTimePicker dateTimePickerEnd;
        private DateTimePicker dateTimePickerStart;
        private ProgressBar progressBarSAP;
        private Label labelCurrentPostcode;
        private Button button3;
    }
}