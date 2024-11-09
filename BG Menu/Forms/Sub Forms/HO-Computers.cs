using System;
using System.Collections.Generic;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class HO_Computers : Form
    {
        public HO_Computers()
        {
            InitializeComponent();

            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "MachineName", HeaderText = "Machine Name", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "User", HeaderText = "Logged-In User", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "User Description", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "CPU", HeaderText = "CPU", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "RAM", HeaderText = "RAM", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "HDD", HeaderText = "HDD ( Used Space )", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "OS", HeaderText = "Operating System", SortMode = DataGridViewColumnSortMode.Automatic });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn { Name = "Build", HeaderText = "Build Number", SortMode = DataGridViewColumnSortMode.Automatic });

            PopulateDataGrid();
        }

        private async void PopulateDataGrid()
        {
            dataGridViewPCInfo.Rows.Clear(); // Clear existing rows before adding new data           

            var tasks = new List<Task>();

            for (int i = 1; i <= 99; i++)
            {
                string machineName = $"WS-{i.ToString("D2")}";

                var task = Task.Run(() =>
                {
                    var info = GetMachineInfo(machineName);
                    this.Invoke(new Action(() =>
                    {
                        if (info.Values.All(value => value != "N/A")) // Only add rows without "N/A"
                        {
                            dataGridViewPCInfo.Rows.Add(
                                machineName,
                                info["User"],
                                info["Description"],
                                info["CPU"],
                                info["RAM"],
                                info["HDD"],
                                info["OS"],
                                info["Build"]
                            );
                            // Sort the DataGridView by the MachineName column immediately after adding each row
                            dataGridViewPCInfo.Sort(dataGridViewPCInfo.Columns["MachineName"], System.ComponentModel.ListSortDirection.Ascending);
                        }
                    }));
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private Dictionary<string, string> GetMachineInfo(string machineName)
        {
            Dictionary<string, string> machineInfo = new Dictionary<string, string>();

            try
            {
                ConnectionOptions options = new ConnectionOptions();
                ManagementScope scope = new ManagementScope($@"\\{machineName}\root\cimv2", options);
                scope.Connect();

                // Get CPU Information
                machineInfo["CPU"] = GetCPUInfo(scope);

                // Get RAM Information
                machineInfo["RAM"] = GetRamInfo(scope);

                // Get HDD Information
                machineInfo["HDD"] = GetStorageInfo(scope);

                // Get OS Information
                machineInfo["OS"] = GetWindowsOS(scope);

                // Get Build Number
                machineInfo["Build"] = GetBuildNumber(scope);

                // Get Logged-In User and Description
                var (user, description) = GetLoggedInUser(scope);
                machineInfo["User"] = user;
                machineInfo["Description"] = description;
            }
            catch (Exception ex)
            {
                machineInfo["CPU"] = "N/A";
                machineInfo["RAM"] = "N/A";
                machineInfo["HDD"] = "N/A";
                machineInfo["OS"] = "N/A";
                machineInfo["Build"] = "N/A";
                machineInfo["User"] = "N/A";
                machineInfo["Description"] = "N/A";
            }

            return machineInfo;
        }

        private (string FullName, string Description) GetLoggedInUser(ManagementScope scope)
        {
            string loggedInUser = "Unknown User";
            string userDescription = "No description available";

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT UserName FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string username = obj["UserName"]?.ToString();
                    if (!string.IsNullOrEmpty(username))
                    {
                        // AD usernames are often in the format DOMAIN\username, so we split to get the actual username
                        string[] parts = username.Split('\\');
                        if (parts.Length > 1)
                        {
                            (loggedInUser, userDescription) = GetFullNameAndDescriptionFromAD(parts[1]);
                        }
                        else
                        {
                            (loggedInUser, userDescription) = GetFullNameAndDescriptionFromAD(username);
                        }
                    }
                    break; // Assuming single user
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving logged-in user information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (loggedInUser, userDescription);
        }

        private (string FullName, string Description) GetFullNameAndDescriptionFromAD(string username)
        {
            string fullName = username;
            string description = "No description available";

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);

                    if (user != null)
                    {
                        fullName = $"{user.GivenName} {user.Surname}";
                        description = user.Description ?? "No description available";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user information from Active Directory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (fullName, description);
        }

        private string GetCPUInfo(ManagementScope scope)
        {
            string cpuInfo = string.Empty;

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Processor");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    cpuInfo = obj["Name"].ToString();
                    break; // Assuming single CPU
                }

                // Extract the manufacturer and model
                if (cpuInfo.Contains("Intel"))
                {
                    cpuInfo = "Intel " + ExtractIntelModel(cpuInfo);
                }
                else if (cpuInfo.Contains("AMD"))
                {
                    cpuInfo = "AMD " + ExtractAMDModel(cpuInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving CPU information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return cpuInfo;
        }

        private string ExtractIntelModel(string cpuName)
        {
            string[] parts = cpuName.Split(' ');
            foreach (string part in parts)
            {
                if (part.StartsWith("i") && part.Contains("-"))
                {
                    return part;
                }
            }
            return "Unknown Model";
        }

        private string ExtractAMDModel(string cpuName)
        {
            string[] parts = cpuName.Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Equals("Ryzen", StringComparison.OrdinalIgnoreCase))
                {
                    return $"{parts[i]} {parts[i + 1]} {parts[i + 2]}";
                }
            }
            return "Unknown Model";
        }

        private string GetRamInfo(ManagementScope scope)
        {
            ulong totalMemory = 0;

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    totalMemory = (ulong)obj["TotalPhysicalMemory"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving RAM information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return $"{(totalMemory / (1024 * 1024 * 1024)):0}GB";
        }

        private string GetStorageInfo(ManagementScope scope)
        {
            long totalSize = 0;
            long availableSpace = 0;

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT Size, FreeSpace FROM Win32_LogicalDisk WHERE DriveType=3"); // 3 indicates a local disk
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    totalSize += Convert.ToInt64(obj["Size"]);
                    availableSpace += Convert.ToInt64(obj["FreeSpace"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving storage information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            long usedSpace = totalSize - availableSpace;
            return $"{(usedSpace / (1024 * 1024 * 1024)):0}/{(totalSize / (1024 * 1024 * 1024)):0}GB";
        }

        private string GetWindowsOS(ManagementScope scope)
        {
            string osName = string.Empty;

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT Caption FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject os in searcher.Get())
                {
                    osName = os["Caption"].ToString();
                    break; // Assuming single OS instance
                }

                if (osName.StartsWith("Microsoft"))
                {
                    osName = osName.Replace("Microsoft", "").Trim();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving OS information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                osName = "Unknown OS";
            }

            return osName;
        }

        private string GetBuildNumber(ManagementScope scope)
        {
            string buildNumber = "Unknown Build";

            try
            {
                ObjectQuery query = new ObjectQuery("SELECT Version FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    buildNumber = obj["Version"].ToString();
                    break; // Assuming single OS instance
                }

                // The build number is typically the part after the third dot in the version string.
                var buildParts = buildNumber.Split('.');
                if (buildParts.Length >= 4)
                {
                    buildNumber = buildParts[3]; // Extract the build number part
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving build number from remote machine: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return buildNumber;
        }

    }
}
