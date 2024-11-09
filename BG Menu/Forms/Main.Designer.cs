namespace BG_Menu
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnFileServer = new Button();
            pnlServiceTools = new Panel();
            button3 = new Button();
            btnFSM = new Button();
            btnBudgetsExtract = new Button();
            btnSalesSheets = new Button();
            btnTools = new Button();
            btnAdminSettings = new Button();
            btnSettings = new Button();
            btnSalesSummary = new Button();
            BtnSpacerPnl = new Panel();
            btnStoreManagement = new Button();
            pnlActiveDirectory = new Panel();
            button5 = new Button();
            button4 = new Button();
            btnActiveDirector = new Button();
            button2 = new Button();
            button1 = new Button();
            panel7 = new Panel();
            panel2 = new Panel();
            Userlbl = new Label();
            Accountlbl = new Label();
            pictureBox1 = new PictureBox();
            panel8 = new Panel();
            panel3 = new Panel();
            Pagelbl = new Label();
            panel5 = new Panel();
            btnMinimise = new Button();
            btnScreenSize = new Button();
            btnClose = new Button();
            FormLoader = new Panel();
            panel6 = new Panel();
            panel1.SuspendLayout();
            pnlServiceTools.SuspendLayout();
            pnlActiveDirectory.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.BackColor = Color.FromArgb(24, 30, 54);
            panel1.Controls.Add(btnFileServer);
            panel1.Controls.Add(pnlServiceTools);
            panel1.Controls.Add(btnTools);
            panel1.Controls.Add(btnAdminSettings);
            panel1.Controls.Add(btnSettings);
            panel1.Controls.Add(btnSalesSummary);
            panel1.Controls.Add(BtnSpacerPnl);
            panel1.Controls.Add(btnStoreManagement);
            panel1.Controls.Add(pnlActiveDirectory);
            panel1.Controls.Add(btnActiveDirector);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 660);
            panel1.TabIndex = 0;
            // 
            // btnFileServer
            // 
            btnFileServer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFileServer.BackgroundImageLayout = ImageLayout.Stretch;
            btnFileServer.Dock = DockStyle.Bottom;
            btnFileServer.FlatAppearance.BorderSize = 0;
            btnFileServer.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnFileServer.FlatStyle = FlatStyle.Flat;
            btnFileServer.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFileServer.ForeColor = Color.FromArgb(158, 161, 176);
            btnFileServer.Image = Properties.Resources.Folder;
            btnFileServer.ImageAlign = ContentAlignment.MiddleLeft;
            btnFileServer.Location = new Point(0, 790);
            btnFileServer.Margin = new Padding(2);
            btnFileServer.Name = "btnFileServer";
            btnFileServer.RightToLeft = RightToLeft.No;
            btnFileServer.Size = new Size(203, 55);
            btnFileServer.TabIndex = 14;
            btnFileServer.Text = "File Server";
            btnFileServer.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnFileServer.UseVisualStyleBackColor = true;
            btnFileServer.Click += button3_Click_1;
            // 
            // pnlServiceTools
            // 
            pnlServiceTools.AutoSize = true;
            pnlServiceTools.Controls.Add(button3);
            pnlServiceTools.Controls.Add(btnFSM);
            pnlServiceTools.Controls.Add(btnBudgetsExtract);
            pnlServiceTools.Controls.Add(btnSalesSheets);
            pnlServiceTools.Dock = DockStyle.Top;
            pnlServiceTools.Location = new Point(0, 570);
            pnlServiceTools.Name = "pnlServiceTools";
            pnlServiceTools.Size = new Size(203, 220);
            pnlServiceTools.TabIndex = 13;
            pnlServiceTools.Visible = false;
            // 
            // button3
            // 
            button3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.Dock = DockStyle.Top;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.Silver;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.FromArgb(158, 161, 176);
            button3.Image = Properties.Resources.Phone;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(0, 165);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.RightToLeft = RightToLeft.No;
            button3.Size = new Size(203, 55);
            button3.TabIndex = 8;
            button3.Text = "PBX Configs";
            button3.TextImageRelation = TextImageRelation.ImageBeforeText;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_2;
            // 
            // btnFSM
            // 
            btnFSM.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnFSM.BackgroundImageLayout = ImageLayout.Stretch;
            btnFSM.Dock = DockStyle.Top;
            btnFSM.FlatAppearance.BorderSize = 0;
            btnFSM.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnFSM.FlatStyle = FlatStyle.Flat;
            btnFSM.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFSM.ForeColor = Color.FromArgb(158, 161, 176);
            btnFSM.Image = Properties.Resources.User___Small;
            btnFSM.ImageAlign = ContentAlignment.MiddleLeft;
            btnFSM.Location = new Point(0, 110);
            btnFSM.Margin = new Padding(2);
            btnFSM.Name = "btnFSM";
            btnFSM.RightToLeft = RightToLeft.No;
            btnFSM.Size = new Size(203, 55);
            btnFSM.TabIndex = 7;
            btnFSM.Text = "FSM Users";
            btnFSM.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnFSM.UseVisualStyleBackColor = true;
            btnFSM.Click += btnFSM_Click;
            // 
            // btnBudgetsExtract
            // 
            btnBudgetsExtract.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnBudgetsExtract.BackgroundImageLayout = ImageLayout.Stretch;
            btnBudgetsExtract.Dock = DockStyle.Top;
            btnBudgetsExtract.FlatAppearance.BorderSize = 0;
            btnBudgetsExtract.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnBudgetsExtract.FlatStyle = FlatStyle.Flat;
            btnBudgetsExtract.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBudgetsExtract.ForeColor = Color.FromArgb(158, 161, 176);
            btnBudgetsExtract.Image = Properties.Resources.BudgetsExtract;
            btnBudgetsExtract.ImageAlign = ContentAlignment.MiddleLeft;
            btnBudgetsExtract.Location = new Point(0, 55);
            btnBudgetsExtract.Margin = new Padding(2);
            btnBudgetsExtract.Name = "btnBudgetsExtract";
            btnBudgetsExtract.RightToLeft = RightToLeft.No;
            btnBudgetsExtract.Size = new Size(203, 55);
            btnBudgetsExtract.TabIndex = 6;
            btnBudgetsExtract.Text = "Budgets Extract";
            btnBudgetsExtract.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnBudgetsExtract.UseVisualStyleBackColor = true;
            btnBudgetsExtract.Click += btnBudgetsExtract_Click;
            // 
            // btnSalesSheets
            // 
            btnSalesSheets.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSalesSheets.BackgroundImageLayout = ImageLayout.Stretch;
            btnSalesSheets.Dock = DockStyle.Top;
            btnSalesSheets.FlatAppearance.BorderSize = 0;
            btnSalesSheets.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnSalesSheets.FlatStyle = FlatStyle.Flat;
            btnSalesSheets.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalesSheets.ForeColor = Color.FromArgb(158, 161, 176);
            btnSalesSheets.Image = Properties.Resources.Sales_Sheet;
            btnSalesSheets.ImageAlign = ContentAlignment.MiddleLeft;
            btnSalesSheets.Location = new Point(0, 0);
            btnSalesSheets.Margin = new Padding(2);
            btnSalesSheets.Name = "btnSalesSheets";
            btnSalesSheets.RightToLeft = RightToLeft.No;
            btnSalesSheets.Size = new Size(203, 55);
            btnSalesSheets.TabIndex = 5;
            btnSalesSheets.Text = "Sales Sheets";
            btnSalesSheets.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSalesSheets.UseVisualStyleBackColor = true;
            btnSalesSheets.Click += btnSalesSheets_Click;
            // 
            // btnTools
            // 
            btnTools.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnTools.BackgroundImageLayout = ImageLayout.Stretch;
            btnTools.Dock = DockStyle.Top;
            btnTools.FlatAppearance.BorderSize = 0;
            btnTools.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnTools.FlatStyle = FlatStyle.Flat;
            btnTools.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTools.ForeColor = Color.FromArgb(158, 161, 176);
            btnTools.Image = Properties.Resources.Tools;
            btnTools.ImageAlign = ContentAlignment.MiddleLeft;
            btnTools.Location = new Point(0, 515);
            btnTools.Margin = new Padding(2);
            btnTools.Name = "btnTools";
            btnTools.RightToLeft = RightToLeft.No;
            btnTools.Size = new Size(203, 55);
            btnTools.TabIndex = 12;
            btnTools.Text = "Service Tools";
            btnTools.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTools.UseVisualStyleBackColor = true;
            btnTools.Click += btnTools_Click;
            // 
            // btnAdminSettings
            // 
            btnAdminSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAdminSettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnAdminSettings.Dock = DockStyle.Bottom;
            btnAdminSettings.FlatAppearance.BorderSize = 0;
            btnAdminSettings.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnAdminSettings.FlatStyle = FlatStyle.Flat;
            btnAdminSettings.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdminSettings.ForeColor = Color.FromArgb(158, 161, 176);
            btnAdminSettings.Image = Properties.Resources.Admin;
            btnAdminSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdminSettings.Location = new Point(0, 845);
            btnAdminSettings.Margin = new Padding(2);
            btnAdminSettings.Name = "btnAdminSettings";
            btnAdminSettings.RightToLeft = RightToLeft.No;
            btnAdminSettings.Size = new Size(203, 55);
            btnAdminSettings.TabIndex = 8;
            btnAdminSettings.Text = "Admin Settings";
            btnAdminSettings.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAdminSettings.UseVisualStyleBackColor = true;
            btnAdminSettings.Click += btnAdminSettings_Click;
            // 
            // btnSettings
            // 
            btnSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnSettings.Dock = DockStyle.Bottom;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSettings.ForeColor = Color.FromArgb(158, 161, 176);
            btnSettings.Image = Properties.Resources.Settings;
            btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
            btnSettings.Location = new Point(0, 900);
            btnSettings.Margin = new Padding(2);
            btnSettings.Name = "btnSettings";
            btnSettings.RightToLeft = RightToLeft.No;
            btnSettings.Size = new Size(203, 55);
            btnSettings.TabIndex = 7;
            btnSettings.Text = "Settings";
            btnSettings.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnSalesSummary
            // 
            btnSalesSummary.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnSalesSummary.BackgroundImageLayout = ImageLayout.Stretch;
            btnSalesSummary.Dock = DockStyle.Top;
            btnSalesSummary.FlatAppearance.BorderSize = 0;
            btnSalesSummary.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnSalesSummary.FlatStyle = FlatStyle.Flat;
            btnSalesSummary.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalesSummary.ForeColor = Color.FromArgb(158, 161, 176);
            btnSalesSummary.Image = Properties.Resources.Finance;
            btnSalesSummary.ImageAlign = ContentAlignment.MiddleLeft;
            btnSalesSummary.Location = new Point(0, 460);
            btnSalesSummary.Margin = new Padding(2);
            btnSalesSummary.Name = "btnSalesSummary";
            btnSalesSummary.RightToLeft = RightToLeft.No;
            btnSalesSummary.Size = new Size(203, 55);
            btnSalesSummary.TabIndex = 6;
            btnSalesSummary.Text = "Sales Summary";
            btnSalesSummary.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSalesSummary.UseMnemonic = false;
            btnSalesSummary.UseVisualStyleBackColor = true;
            btnSalesSummary.Click += btnSalesSummary_Click;
            // 
            // BtnSpacerPnl
            // 
            BtnSpacerPnl.Dock = DockStyle.Bottom;
            BtnSpacerPnl.Location = new Point(0, 955);
            BtnSpacerPnl.Name = "BtnSpacerPnl";
            BtnSpacerPnl.Size = new Size(203, 5);
            BtnSpacerPnl.TabIndex = 10;
            // 
            // btnStoreManagement
            // 
            btnStoreManagement.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnStoreManagement.BackgroundImageLayout = ImageLayout.Stretch;
            btnStoreManagement.Dock = DockStyle.Top;
            btnStoreManagement.FlatAppearance.BorderSize = 0;
            btnStoreManagement.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnStoreManagement.FlatStyle = FlatStyle.Flat;
            btnStoreManagement.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStoreManagement.ForeColor = Color.FromArgb(158, 161, 176);
            btnStoreManagement.Image = Properties.Resources.Store;
            btnStoreManagement.ImageAlign = ContentAlignment.MiddleLeft;
            btnStoreManagement.Location = new Point(0, 405);
            btnStoreManagement.Margin = new Padding(2);
            btnStoreManagement.Name = "btnStoreManagement";
            btnStoreManagement.RightToLeft = RightToLeft.No;
            btnStoreManagement.Size = new Size(203, 55);
            btnStoreManagement.TabIndex = 5;
            btnStoreManagement.Text = "Store Management";
            btnStoreManagement.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnStoreManagement.UseVisualStyleBackColor = true;
            btnStoreManagement.Click += btnStoreManagement_Click;
            // 
            // pnlActiveDirectory
            // 
            pnlActiveDirectory.AutoSize = true;
            pnlActiveDirectory.Controls.Add(button5);
            pnlActiveDirectory.Controls.Add(button4);
            pnlActiveDirectory.Dock = DockStyle.Top;
            pnlActiveDirectory.Location = new Point(0, 295);
            pnlActiveDirectory.Name = "pnlActiveDirectory";
            pnlActiveDirectory.Size = new Size(203, 110);
            pnlActiveDirectory.TabIndex = 11;
            pnlActiveDirectory.Visible = false;
            // 
            // button5
            // 
            button5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button5.BackgroundImageLayout = ImageLayout.Stretch;
            button5.Dock = DockStyle.Top;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatAppearance.MouseOverBackColor = Color.Silver;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.ForeColor = Color.FromArgb(158, 161, 176);
            button5.Image = Properties.Resources.Computer;
            button5.ImageAlign = ContentAlignment.MiddleLeft;
            button5.Location = new Point(0, 55);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.RightToLeft = RightToLeft.No;
            button5.Size = new Size(203, 55);
            button5.TabIndex = 6;
            button5.Text = "PC Management";
            button5.TextImageRelation = TextImageRelation.ImageBeforeText;
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button4.Dock = DockStyle.Top;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.Silver;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.ForeColor = Color.FromArgb(158, 161, 176);
            button4.Image = Properties.Resources.User___Small;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(0, 0);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.RightToLeft = RightToLeft.No;
            button4.Size = new Size(203, 55);
            button4.TabIndex = 5;
            button4.Text = "User Management";
            button4.TextImageRelation = TextImageRelation.ImageBeforeText;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // btnActiveDirector
            // 
            btnActiveDirector.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnActiveDirector.BackgroundImageLayout = ImageLayout.Stretch;
            btnActiveDirector.Dock = DockStyle.Top;
            btnActiveDirector.FlatAppearance.BorderSize = 0;
            btnActiveDirector.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnActiveDirector.FlatStyle = FlatStyle.Flat;
            btnActiveDirector.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnActiveDirector.ForeColor = Color.FromArgb(158, 161, 176);
            btnActiveDirector.Image = Properties.Resources.Active_Directory;
            btnActiveDirector.ImageAlign = ContentAlignment.MiddleLeft;
            btnActiveDirector.Location = new Point(0, 240);
            btnActiveDirector.Margin = new Padding(2);
            btnActiveDirector.Name = "btnActiveDirector";
            btnActiveDirector.RightToLeft = RightToLeft.No;
            btnActiveDirector.Size = new Size(203, 55);
            btnActiveDirector.TabIndex = 4;
            btnActiveDirector.Text = "Active Directory";
            btnActiveDirector.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnActiveDirector.UseVisualStyleBackColor = true;
            btnActiveDirector.Click += button3_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Dock = DockStyle.Top;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.Silver;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(158, 161, 176);
            button2.Image = Properties.Resources.Credit_Card;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(0, 185);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.RightToLeft = RightToLeft.No;
            button2.Size = new Size(203, 55);
            button2.TabIndex = 3;
            button2.Text = "Credit Card";
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Top;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.Silver;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(158, 161, 176);
            button1.Image = Properties.Resources.Home;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(0, 130);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.RightToLeft = RightToLeft.No;
            button1.Size = new Size(203, 55);
            button1.TabIndex = 2;
            button1.Text = "Home";
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel7
            // 
            panel7.BackColor = Color.White;
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 129);
            panel7.Margin = new Padding(2);
            panel7.Name = "panel7";
            panel7.Size = new Size(203, 1);
            panel7.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(24, 30, 54);
            panel2.Controls.Add(Userlbl);
            panel2.Controls.Add(Accountlbl);
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new Size(203, 129);
            panel2.TabIndex = 1;
            // 
            // Userlbl
            // 
            Userlbl.Dock = DockStyle.Bottom;
            Userlbl.Font = new Font("Yu Gothic", 12F, FontStyle.Bold);
            Userlbl.ForeColor = Color.FromArgb(158, 161, 176);
            Userlbl.Location = new Point(0, 79);
            Userlbl.Margin = new Padding(2, 0, 2, 0);
            Userlbl.Name = "Userlbl";
            Userlbl.Size = new Size(203, 23);
            Userlbl.TabIndex = 2;
            Userlbl.Text = "Not Logged In";
            Userlbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Accountlbl
            // 
            Accountlbl.Dock = DockStyle.Bottom;
            Accountlbl.Font = new Font("Yu Gothic", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Accountlbl.ForeColor = Color.FromArgb(158, 161, 176);
            Accountlbl.Location = new Point(0, 102);
            Accountlbl.Margin = new Padding(2, 0, 2, 0);
            Accountlbl.Name = "Accountlbl";
            Accountlbl.Size = new Size(203, 27);
            Accountlbl.TabIndex = 3;
            Accountlbl.Text = "Null";
            Accountlbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(24, 30, 54);
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Image = Properties.Resources.Account;
            pictureBox1.InitialImage = Properties.Resources.Account;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(203, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.WaitOnLoad = true;
            // 
            // panel8
            // 
            panel8.BackColor = Color.White;
            panel8.Dock = DockStyle.Left;
            panel8.Location = new Point(220, 0);
            panel8.Margin = new Padding(2);
            panel8.Name = "panel8";
            panel8.Size = new Size(1, 660);
            panel8.TabIndex = 10;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(46, 51, 73);
            panel3.Controls.Add(Pagelbl);
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(btnMinimise);
            panel3.Controls.Add(btnScreenSize);
            panel3.Controls.Add(btnClose);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(221, 0);
            panel3.Margin = new Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new Size(989, 51);
            panel3.TabIndex = 1;
            // 
            // Pagelbl
            // 
            Pagelbl.Dock = DockStyle.Left;
            Pagelbl.Font = new Font("Yu Gothic", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Pagelbl.ForeColor = Color.FromArgb(158, 161, 176);
            Pagelbl.Location = new Point(13, 0);
            Pagelbl.Margin = new Padding(2, 0, 0, 0);
            Pagelbl.Name = "Pagelbl";
            Pagelbl.Size = new Size(359, 51);
            Pagelbl.TabIndex = 0;
            Pagelbl.Text = "Home";
            Pagelbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Left;
            panel5.Location = new Point(0, 0);
            panel5.Margin = new Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new Size(13, 51);
            panel5.TabIndex = 10;
            // 
            // btnMinimise
            // 
            btnMinimise.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnMinimise.BackgroundImage = Properties.Resources.Minimize;
            btnMinimise.BackgroundImageLayout = ImageLayout.Zoom;
            btnMinimise.Dock = DockStyle.Right;
            btnMinimise.FlatAppearance.BorderSize = 0;
            btnMinimise.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnMinimise.FlatStyle = FlatStyle.Flat;
            btnMinimise.Font = new Font("Yu Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMinimise.ForeColor = Color.FromArgb(158, 161, 176);
            btnMinimise.Location = new Point(836, 0);
            btnMinimise.Margin = new Padding(2);
            btnMinimise.Name = "btnMinimise";
            btnMinimise.RightToLeft = RightToLeft.No;
            btnMinimise.Size = new Size(51, 51);
            btnMinimise.TabIndex = 9;
            btnMinimise.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnMinimise.UseMnemonic = false;
            btnMinimise.UseVisualStyleBackColor = true;
            btnMinimise.Click += button8_Click;
            // 
            // btnScreenSize
            // 
            btnScreenSize.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnScreenSize.BackgroundImage = Properties.Resources.Full_Screen;
            btnScreenSize.BackgroundImageLayout = ImageLayout.Zoom;
            btnScreenSize.Dock = DockStyle.Right;
            btnScreenSize.FlatAppearance.BorderSize = 0;
            btnScreenSize.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnScreenSize.FlatStyle = FlatStyle.Flat;
            btnScreenSize.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnScreenSize.ForeColor = Color.FromArgb(158, 161, 176);
            btnScreenSize.Location = new Point(887, 0);
            btnScreenSize.Margin = new Padding(2);
            btnScreenSize.Name = "btnScreenSize";
            btnScreenSize.RightToLeft = RightToLeft.No;
            btnScreenSize.Size = new Size(51, 51);
            btnScreenSize.TabIndex = 8;
            btnScreenSize.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnScreenSize.UseMnemonic = false;
            btnScreenSize.UseVisualStyleBackColor = true;
            btnScreenSize.Click += button10_Click;
            // 
            // btnClose
            // 
            btnClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnClose.BackgroundImage = Properties.Resources.Close;
            btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.Silver;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(158, 161, 176);
            btnClose.Location = new Point(938, 0);
            btnClose.Margin = new Padding(2);
            btnClose.Name = "btnClose";
            btnClose.RightToLeft = RightToLeft.No;
            btnClose.Size = new Size(51, 51);
            btnClose.TabIndex = 7;
            btnClose.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnClose.UseMnemonic = false;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += button9_Click;
            // 
            // FormLoader
            // 
            FormLoader.Dock = DockStyle.Fill;
            FormLoader.Location = new Point(221, 52);
            FormLoader.Margin = new Padding(0);
            FormLoader.Name = "FormLoader";
            FormLoader.Padding = new Padding(10);
            FormLoader.Size = new Size(989, 608);
            FormLoader.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(221, 51);
            panel6.Margin = new Padding(2);
            panel6.Name = "panel6";
            panel6.Size = new Size(989, 1);
            panel6.TabIndex = 3;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(1210, 660);
            Controls.Add(FormLoader);
            Controls.Add(panel6);
            Controls.Add(panel3);
            Controls.Add(panel8);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            Name = "Main";
            Text = "BG Menu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            pnlServiceTools.ResumeLayout(false);
            pnlActiveDirectory.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel8;
        private Panel panel7;
        private PictureBox pictureBox1;
        private Panel panel3;
        private Button button1;
        private Button btnSalesSummary;
        private Button btnStoreManagement;
        private Button btnActiveDirector;
        private Button button2;
        private Label Userlbl;
        private Button btnSettings;
        private Button btnAdminSettings;
        private Label Accountlbl;
        private Label Pagelbl;
        private Panel FormLoader;
        private Button btnScreenSize;
        private Button btnClose;
        private Button btnMinimise;
        private Panel panel5;
        private Panel panel6;
        private Panel BtnSpacerPnl;
        private Panel pnlActiveDirectory;
        private Button button5;
        private Button button4;
        private Button btnTools;
        private Panel pnlServiceTools;
        private Button btnBudgetsExtract;
        private Button btnSalesSheets;
        private Button btnFSM;
        private Button btnFileServer;
        private Button button3;
    }
}
