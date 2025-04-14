using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            var label2Draggable = new Draggable(networkTestListView, this);


            pictureBox3.MouseDown += pictureBox1_MouseClickToggle;
            pictureBox3.MouseUp += pictureBox1_MouseClickToggle;

            this.KeyPreview = true;

            this.KeyDown += new KeyEventHandler(YourForm_KeyDown);

            Task.Run(() => PingVPNsAsync());

        }

        private async Task PingVPNsAsync()
        {
            await PingVPNAsync("able-fs03", hoVpnStatusLabel, "HO VPN Online", "HO VPN");
            await PingVPNAsync("10.100.230.6", sapVpnStatusLabel, "SAP VPN Online", "SAP VPN");
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

        private async Task RunNetworkTestsAsync()
        {

            networkTestListView.Items.Clear();

            ListViewItem itemNicStatus = new ListViewItem();
            networkTestListView.Items.Add(itemNicStatus);
            bool nicStatus = IsNetworkInterfaceUp();
            UpdateListViewItem(itemNicStatus, nicStatus, "Network adapter is operational", "No active network adapter found");


            ListViewItem itemIpConfig = new ListViewItem();
            networkTestListView.Items.Add(itemIpConfig);
            bool validIpConfig = HasValidIPConfiguration();
            UpdateListViewItem(itemIpConfig, validIpConfig, "IP configuration is valid", "IP configuration is invalid (possible APIPA or no IP)");


            string defaultGateway = GetDefaultGatewayIP();
            if (!string.IsNullOrEmpty(defaultGateway))
            {
                ListViewItem itemGateway = new ListViewItem();
                networkTestListView.Items.Add(itemGateway);
                bool gatewayStatus = await TestPingAsync(defaultGateway);
                UpdateListViewItem(itemGateway, gatewayStatus, $"Default Gateway ({defaultGateway}) is reachable", $"Default Gateway ({defaultGateway}) is unreachable");
            }
            else
            {
                ListViewItem itemGateway = new ListViewItem();
                networkTestListView.Items.Add(itemGateway);
                UpdateListViewItem(itemGateway, false, "Default Gateway not found", "Default Gateway not found");
            }

            ListViewItem itemDC = new ListViewItem();
            networkTestListView.Items.Add(itemDC);
            bool dcStatus = await TestPingAsync("able-dc01");
            UpdateListViewItem(itemDC, dcStatus, "Domain Controller is Online", "Domain Controller is Offline");

            ListViewItem itemVM1 = new ListViewItem();
            networkTestListView.Items.Add(itemVM1);
            bool vm1Status = await TestPingAsync("192.168.0.26");
            UpdateListViewItem(itemVM1, vm1Status, "VM Host 1 is Online", "VM Host 1 is Offline");

            ListViewItem itemVM2 = new ListViewItem();
            networkTestListView.Items.Add(itemVM2);
            bool vm2Status = await TestPingAsync("192.168.0.25");
            UpdateListViewItem(itemVM2, vm2Status, "VM Host 2 is Online", "VM Host 2 is Offline");

            ListViewItem itemDnsPing = new ListViewItem();
            networkTestListView.Items.Add(itemDnsPing);
            bool dnsServerPingStatus = await TestPingAsync("8.8.8.8");
            UpdateListViewItem(itemDnsPing, dnsServerPingStatus, "DNS Server is reachable", "DNS Server is unreachable");

            string[] websites = { "www.google.com", "www.microsoft.com", "www.facebook.com" };
            Task<bool>[] dnsTasks = websites.Select(site => TestDnsResolutionAsync(site)).ToArray();
            bool[] dnsResults = await Task.WhenAll(dnsTasks);
            bool allDnsWorking = dnsResults.All(result => result);

            ListViewItem itemDnsResolve = new ListViewItem();
            networkTestListView.Items.Add(itemDnsResolve);
            UpdateListViewItem(itemDnsResolve, allDnsWorking, "ISP DNS is Working", "ISP DNS is Not Working");

            ListViewItem itemInternetPing = new ListViewItem();
            networkTestListView.Items.Add(itemInternetPing);
            bool internetPingStatus = await TestPingAsync("8.8.8.8");
            UpdateListViewItem(itemInternetPing, internetPingStatus, "Internet connectivity (Ping) is Active", "Internet connectivity (Ping) is Inactive");

            ListViewItem itemHttpTest = new ListViewItem();
            networkTestListView.Items.Add(itemHttpTest);
            bool httpStatus = await TestHttpConnectivityAsync("https://www.google.com");
            UpdateListViewItem(itemHttpTest, httpStatus, "HTTP/HTTPS access is working", "HTTP/HTTPS access is blocked or failing");
        }
        private bool IsNetworkInterfaceUp()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            return interfaces.Any(nic => nic.OperationalStatus == OperationalStatus.Up);
        }


        private bool HasValidIPConfiguration()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up);

            foreach (var nic in interfaces)
            {
                var props = nic.GetIPProperties();
                foreach (var addr in props.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork &&
                        !addr.Address.ToString().StartsWith("169.254.") &&
                        !addr.Address.Equals(IPAddress.Any) &&
                        !addr.Address.Equals(IPAddress.None))
                    {
                        return true; // Found a valid IPv4 address
                    }
                }
            }
            return false;
        }


        private string GetDefaultGatewayIP()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up);

            foreach (var nic in interfaces)
            {
                var ipProps = nic.GetIPProperties();
                foreach (var gateway in ipProps.GatewayAddresses)
                {
                    // Skip 0.0.0.0 addresses
                    if (gateway.Address.AddressFamily == AddressFamily.InterNetwork &&
                        !gateway.Address.ToString().Equals("0.0.0.0"))
                    {
                        return gateway.Address.ToString();
                    }
                }
            }
            return null;
        }

        private async Task<bool> TestPingAsync(string host)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync(host, 3000);
                    return reply.Status == IPStatus.Success;
                }
                catch
                {
                    return false;
                }
            }
        }

        private async Task<bool> TestDnsResolutionAsync(string hostname)
        {
            try
            {
                var hostEntry = await Dns.GetHostEntryAsync(hostname);
                return hostEntry.AddressList.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> TestHttpConnectivityAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // HEAD request to avoid downloading full content
                    var request = new HttpRequestMessage(HttpMethod.Head, url);
                    HttpResponseMessage response = await client.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateListViewItem(ListViewItem item, bool success, string successText, string failureText)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateListViewItem(item, success, successText, failureText)));
            }
            else
            {
                item.Text = success ? successText : failureText;
                item.ForeColor = success ? Color.Green : Color.Red;
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            await RunNetworkTestsAsync();
        }
    }
}
