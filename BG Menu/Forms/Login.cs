using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BG_Menu.Class.Design;
using Google.Cloud.Firestore;

namespace BG_Menu
{

    public partial class Login : Form
    {
        private RoundedCorners roundedCorners;
        private FirestoreDb firestoreDb;
        private AuthService authService;
        private UserSession userSession;

        public Login()
        {
            InitializeComponent();                       

            InitializeFirebase();

            this.DoubleBuffered = true;
            var roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var LoginDraggable = new Draggable(this, this);
            var labelDraggable = new Draggable(sapVpnStatusLabel, this);
            var label1Draggable = new Draggable(hoVpnStatusLabel, this);


            pictureBox3.MouseDown += pictureBox1_MouseClickToggle;
            pictureBox3.MouseUp += pictureBox1_MouseClickToggle;

            this.KeyPreview = true;
            
            this.KeyDown += new KeyEventHandler(YourForm_KeyDown);

            Task.Run(() => PingVPNsAsync());
        }

        private async Task PingVPNsAsync()
        {
            await PingVPNAsync("able-fs03", hoVpnStatusLabel, "HO VPN Online", "HO VPN Offline");
            await PingVPNAsync("10.100.230.6", sapVpnStatusLabel, "SAP VPN Online", "SAP VPN Offline");           
        }

        private async Task PingVPNAsync(string ipAddress, Label statusLabel, string onlineMessage, string offlineMessage)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = await ping.SendPingAsync(ipAddress);

                if (reply.Status == IPStatus.Success)
                {
                    // Ping successful, update UI with Online message in green
                    statusLabel.Invoke((Action)(() =>
                    {
                        statusLabel.Text = onlineMessage;
                        statusLabel.ForeColor = Color.Green;
                    }));
                }
                else
                {
                    // Ping failed, update UI with Offline message in red
                    statusLabel.Invoke((Action)(() =>
                    {
                        statusLabel.Text = offlineMessage;
                        statusLabel.ForeColor = Color.Red;
                    }));
                }
            }
            catch (Exception)
            {
                // Exception occurred, update UI with Offline message in red
                statusLabel.Invoke((Action)(() =>
                {
                    statusLabel.Text = offlineMessage;
                    statusLabel.ForeColor = Color.Red;
                }));
            }
        }

        private void pictureBox1_MouseClickToggle(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Button == MouseButtons.Left)
            {
                // Toggle the UseSystemPasswordChar property
                passwordTextBox.UseSystemPasswordChar = !passwordTextBox.UseSystemPasswordChar;
            }
        }

        private void InitializeFirebase()
        {
            try
            {
                // Use the singleton instance to get the FirestoreDb
                firestoreDb = FirebaseServiceInitializer.Instance.GetFirestoreDb();

                if (firestoreDb != null)
                {
                    authService = new AuthService(firestoreDb);  // Initialize authService here
                    userSession = new UserSession(authService);
                }
                else
                {
                    MessageBox.Show("Failed to initialize FirestoreDb.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize Firebase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            loginButton.Enabled = false;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            // Attempt to log in using UserSession
            bool isAuthenticated = await userSession.LoginAsync(username, password);
            loginButton.Enabled = true;
            if (isAuthenticated)
            {
                // If login is successful, open the Main form and close the Login form
                Main mainForm = new Main(userSession.Username, userSession.Permissions, userSession.Rank, firestoreDb);
                mainForm.Show();  // Show the Main form
                this.Hide();  // Hide the Login form
            }
        }

        private void SetPasswordButton_Click(object sender, EventArgs e)
        {
            /*
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await authService.SetPasswordAsync(username, password);
                MessageBox.Show($"Password for user {username} has been set successfully.");
            }
            else
            {
                MessageBox.Show("Username and password fields cannot be empty.");
            }
            */
            Application.Exit();
            Application.Exit();
        }        

        private async void YourForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                loginButton.Enabled = false;
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;

                // Attempt to log in using UserSession
                bool isAuthenticated = await userSession.LoginAsync(username, password);
                loginButton.Enabled = true;
                if (isAuthenticated)
                {
                    // If login is successful, open the Main form and close the Login form
                    Main mainForm = new Main(userSession.Username, userSession.Permissions, userSession.Rank, firestoreDb);
                    mainForm.Show();  // Show the Main form
                    this.Hide();  // Hide the Login form
                }
                e.Handled = true;
            }
        }
    }

}
