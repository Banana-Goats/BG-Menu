using System;
using System.Drawing;
using System.Windows.Forms;
using BG_Menu.Class.Design;
using BG_Menu.Forms.Sales_Summary;
using BG_Menu.Forms.Sub_Forms;
using Google.Cloud.Firestore;



namespace BG_Menu
{
    public partial class Main : Form
    {
        private readonly string currentUsername;
        private RoundedCorners roundedCorners;
        private Form currentForm;
        private FullscreenToggle fullscreenToggle;
        private TrayMinimizer trayMinimizer;
        private List<string> userPermissions;
        private FirestoreDb firestoreDb;

        public Main(string username, List<string> permissions, string rank, FirestoreDb db)
        {

            InitializeComponent();

            this.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BG Menu.ico"));

            Userlbl.Text = $"{username}";
            Accountlbl.Text = $"{rank}";

            this.DoubleBuffered = true;

            var labelDraggable = new Draggable(Pagelbl, this);
            var panelDraggable = new Draggable(panel3, this);

            var roundedCorners = new RoundedCorners(this, 70);

            fullscreenToggle = new FullscreenToggle(this, roundedCorners);

            trayMinimizer = new TrayMinimizer(this, roundedCorners, fullscreenToggle);

            currentUsername = username;
            userPermissions = permissions;
            firestoreDb = db;

            btnSalesSummary.MouseUp += btnSalesSummary_MouseUp;

            SetButtonVisibility();

        }

        #region Overrides
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Reapply docking and size adjustments if a form is loaded
            if (currentForm != null)
            {
                currentForm.Dock = DockStyle.Fill;
                currentForm.Size = FormLoader.ClientSize;
            }
        }



        #endregion



        #region Button Events

        private void HidePanels()
        {
            if (pnlActiveDirectory.Visible == true)
                pnlActiveDirectory.Visible = false;

            if (pnlServiceTools.Visible == true)
                pnlServiceTools.Visible = false;

            if (pnlFinance.Visible == true)
                pnlFinance.Visible = false;
        }

        private void ShowPanels(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                HidePanels();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "DashBoard";
            LoadFormInPanel(new DashBoard());
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Credit Card Report";
            LoadFormInPanel(new Forms.Sub_Forms.NewCreditCard());

        }

        private void button6_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Emails";
            LoadFormInPanel(new Forms.Sub_Forms.EmailManager());
        }

        private void btnStoreManagement_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Network Manager";
            LoadFormInPanel(new Forms.Sub_Forms.NetworkManagerDisplay());

        }

        private void btnSalesSummary_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Sales Summary";

            LoadFormInPanel(new Forms.Sales_Summary.SalesSummary());
        }

        private void btnSalesSummary_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Open SalesSummary in a new window and mark it as such.
                SalesSummary salesSummaryNew = new SalesSummary();
                salesSummaryNew.OpenedInNewWindow = true;
                salesSummaryNew.Show();
            }
            else if (e.Button == MouseButtons.Left)
            {
                // Original left-click behavior (loads form in the panel)
                Pagelbl.Text = "Sales Summary";
                LoadFormInPanel(new SalesSummary());
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            Pagelbl.Text = "File Server Manager";
            LoadFormInPanel(new Forms.Sub_Forms.FileClientForm());
        }

        private void btnAdminSettings_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Admin Settings";

            LoadFormInPanel(new Forms.Sub_Forms.UserManagement(firestoreDb));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Settings";
            LoadFormInPanel(new Forms.Sub_Forms.UserSettings(currentUsername, firestoreDb));
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Computer Management";
            LoadFormInPanel(new Forms.Sub_Forms.HO_Computers());
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "User Management";
            LoadFormInPanel(new Forms.Sub_Forms.ActiveDirectory());

        }

        private void btnSalesSheets_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Sales Sheet Creator";
            LoadFormInPanel(new Forms.Sub_Forms.SalesSheets());
        }

        private void btnBudgetsExtract_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "Budgets Extractor";
            LoadFormInPanel(new Forms.Sub_Forms.BudgetsExtract());
        }

        private void btnFSM_Click(object sender, EventArgs e)
        {

            Pagelbl.Text = "FSM Management";
            LoadFormInPanel(new Forms.Sub_Forms.FSMUsers());
        }

        private void button3_Click_2(object sender, EventArgs e)
        {

            Pagelbl.Text = "YeaStar PBX Custom Config Editor";
            LoadFormInPanel(new Forms.Sub_Forms.YeaStarConfig());
        }

        private void btnTools_Click(object sender, EventArgs e)
        {
            HidePanels();
            ShowPanels(pnlServiceTools);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HidePanels();
            ShowPanels(pnlActiveDirectory);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HidePanels();
            ShowPanels(pnlFinance);
        }

        private void paymentdevices_Click(object sender, EventArgs e)
        {
            Pagelbl.Text = "MIDS / TIDS";
            LoadFormInPanel(new Forms.Sub_Forms.PaymentDevicesApp(currentUsername));
        }

        private void btnRefunds_Click(object sender, EventArgs e)
        {
            Refunds Refunds = new Refunds();
            Refunds.Show();
        }

        // Tray Buttons

        private void button10_Click(object sender, EventArgs e)
        {
            fullscreenToggle.ToggleFullscreen();

            // Toggle the button image
            if (fullscreenToggle.IsFullscreen)
            {
                btnScreenSize.BackgroundImage = Properties.Resources.Full_Screen_Close;
            }
            else
            {
                btnScreenSize.BackgroundImage = Properties.Resources.Full_Screen;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Logout Functionality
            try
            {
                Login loginForm = new Login();
                loginForm.Show();

                // Close the Main form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during logout: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }



        #endregion



        // Permissions

        private void SetButtonVisibility()
        {
            // Admin Functions
            if (userPermissions.Contains("admin"))
            {
                btnAdminSettings.Visible = true;
            }
            else
            {
                btnAdminSettings.Visible = false;

            }

            if (userPermissions.Contains("fileserver"))
            {
                btnFileServer.Visible = true;

            }
            else
            {
                btnFileServer.Visible = false;
            }

            // Sales Summary Functions
            if (userPermissions.Contains("salesview"))
            {
                btnSalesSummary.Visible = true;
            }
            else
            {
                btnSalesSummary.Visible = false;
            }

            if (userPermissions.Contains("networkmonitor"))
            {
                btnStoreManagement.Visible = true;
            }
            else
            {
                btnStoreManagement.Visible = false;
            }

            if (userPermissions.Contains("servicetools"))
            {
                btnTools.Visible = true;
            }
            else
            {
                btnTools.Visible = false;
            }

            if (userPermissions.Contains("IT Department"))
            {
                paymentdevices.Visible = true;
                button1.Visible = true;
                btnCreditcard.Visible = true;
                btnActiveDirector.Visible = true;
                HidePanels();
            }
            else
            {
                paymentdevices.Visible = false;
                button1.Visible = false;
                btnCreditcard.Visible = false;
                btnActiveDirector.Visible = false;
            }

            if (userPermissions.Contains("mids/tids"))
            {
                paymentdevices.Visible = true;
            }
            else
            {
                paymentdevices.Visible = false;
            }

            if (userPermissions.Contains("Sales/Budgets"))
            {
                btnBudgetsExtract.Visible = true;
                btnSalesSheets.Visible = true;
            }
            else
            {
                btnBudgetsExtract.Visible = false;
                btnSalesSheets.Visible = false;
            }

            if (userPermissions.Contains("Refunds"))
            {
                btnRefunds.Visible = true;
            }
            else
            {
                btnRefunds.Visible = false;
            }

            if (userPermissions.Contains("Report"))
            {
                LoadFormInPanel(new Display());
            }
            else
            {
                
            }

            // Add more buttons and permission checks as needed
        }

        private void LoadFormInPanel(Form form)
        {
            if (currentForm != null)
            {
                // Remove the current form from the panel
                FormLoader.Controls.Remove(currentForm);

                // Dispose of the form and its resources
                currentForm.Dispose();
                currentForm = null;
            }

            // Clear the current form from the panel
            FormLoader.Controls.Clear();

            // Set the form's properties
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.AutoScaleMode = AutoScaleMode.None;
            form.AutoSize = false;
            form.MinimumSize = Size.Empty;
            form.MaximumSize = Size.Empty;
            form.WindowState = FormWindowState.Normal;
            form.StartPosition = FormStartPosition.Manual;

            // Add the form to the panel
            FormLoader.Controls.Add(form);

            // Dock the form to fill the entire panel
            form.Dock = DockStyle.Fill;

            // Show the form
            form.Show();

            // Store the reference to the current form
            currentForm = form;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Pagelbl.Text = "VAT Data Export";
            LoadFormInPanel(new VATData());
        }
    }
}
