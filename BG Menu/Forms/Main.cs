using System;
using System.Drawing;
using System.Windows.Forms;
using BG_Menu.Class.Design;
using BG_Menu.Forms.Sub_Forms;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;


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
        private UdpListener udpListener;

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

            SetButtonVisibility();
            InitialiseFirebaseStorage();

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

        #region Firebase Storage
        private void InitialiseFirebaseStorage()
        {
            // Initialize the Firebase Storage client
            var firebaseStorageClient = StorageClient.Create();
            var firebaseStorage = new FirebaseStorage(firebaseStorageClient);

            // Initiate the download
            InitiateDownload(firebaseStorage);
        }

        private async void InitiateDownload(FirebaseStorage firebaseStorage)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDirectory = Path.Combine(appDataPath, "BudgetsReport");
            string filePath = Path.Combine(appDirectory, "Targets.db");

            // Check if the file exists, and delete it if it does
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine("Existing file deleted: " + filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete the existing file: " + ex.Message);
                    return;
                }
            }

            // Proceed with downloading the file
            string downloadedFilePath = await firebaseStorage.DownloadBudgetsAsync();

            if (downloadedFilePath != null)
            {
                Console.WriteLine("File Downloaded : " + downloadedFilePath);
            }
            else
            {
                MessageBox.Show("Download failed.");
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
            HidePanels();
            Pagelbl.Text = "DashBoard";
            LoadFormInPanel(new DashBoard());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Credit Card Report";
            LoadFormInPanel(new Forms.Sub_Forms.CreditCard());

        }        

        private void button6_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Emails";
            LoadFormInPanel(new Forms.Sub_Forms.EmailManager());
        }

        private void btnStoreManagement_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Network Manager";
            LoadFormInPanel(new Forms.Sub_Forms.NetworkManagerDisplay());

        }

        private void btnSalesSummary_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Sales Summary";

            LoadFormInPanel(new Forms.Sales_Summary.SalesSummary());
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "File Server Manager";
            LoadFormInPanel(new Forms.Sub_Forms.FileClientForm());
        }

        private void btnAdminSettings_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Admin Settings";

            LoadFormInPanel(new Forms.Sub_Forms.UserManagement(firestoreDb));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Settings";
            LoadFormInPanel(new Forms.Sub_Forms.UserSettings(currentUsername, firestoreDb));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Computer Management";
            LoadFormInPanel(new Forms.Sub_Forms.HO_Computers());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "User Management";
            LoadFormInPanel(new Forms.Sub_Forms.ActiveDirectory());

        }        

        private void btnSalesSheets_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Sales Sheet Creator";
            LoadFormInPanel(new Forms.Sub_Forms.SalesSheets());
        }

        private void btnBudgetsExtract_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "Budgets Extractor";
            LoadFormInPanel(new Forms.Sub_Forms.BudgetsExtract());
        }

        private void btnFSM_Click(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "FSM Management";
            LoadFormInPanel(new Forms.Sub_Forms.FSMUsers());
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            HidePanels();
            Pagelbl.Text = "YeaStar PBX Custom Config Editor";
            LoadFormInPanel(new Forms.Sub_Forms.YeaStarConfig());
        }

        private void btnTools_Click(object sender, EventArgs e)
        {
            ShowPanels(pnlServiceTools);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowPanels(pnlActiveDirectory);
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

                // Stop UDP Listener if it's running
                udpListener?.StopListening();

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

        private void StartUdpListener()
        {
            udpListener = new UdpListener(this);
        }

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
                button2.Visible = true;
                btnActiveDirector.Visible = true;
            }
            else
            {
                paymentdevices.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                btnActiveDirector.Visible = false;
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

        private void paymentdevices_Click(object sender, EventArgs e)
        {
            PaymentDevicesApp paymentDevicesApp = new PaymentDevicesApp(currentUsername);
            paymentDevicesApp.Show();
        }
    }
}
