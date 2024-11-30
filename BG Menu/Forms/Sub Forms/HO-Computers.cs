using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.Data.SqlClient;

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

            // Define patterns to match
            var patterns = new List<string>
    {
        "WS-{0:D2}",    // Matches WS-??
        "LT-{0:D2}",    // Matches LT-??
        "ABL000{0:D1}"    // Matches ABL0???
    };

            foreach (var pattern in patterns)
            {
                int start = pattern.Contains("D2") ? 1 : 0; // D2 pattern starts from 1; D3 pattern starts from 0
                int end = pattern.Contains("D2") ? 99 : 999; // D2 has 2 digits; D3 has 3 digits

                for (int i = start; i <= end; i++)
                {
                    string machineName = string.Format(pattern, i);

                    var task = Task.Run(async () =>
                    {
                        var info = GetMachineInfo(machineName);

                        // Check if the CPU field has valid data
                        if (!string.IsNullOrEmpty(info["CPU"]) && info["CPU"] != "N/A")
                        {
                            // Add data to the DataGridView
                            this.Invoke(new Action(() =>
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

                                // Sort the DataGridView by the MachineName column
                                dataGridViewPCInfo.Sort(dataGridViewPCInfo.Columns["MachineName"], System.ComponentModel.ListSortDirection.Ascending);
                            }));

                            // Save data to SQL Server
                            await SaveToDatabaseAsync(
                                machineName,
                                info["User"],  // Maps to Location
                                info["CPU"],
                                info["RAM"],
                                info["HDD"],   // Maps to StorageInfo
                                info["OS"],    // Maps to WindowsOS
                                info["Build"]  // Maps to BuildNumber
                            );
                        }
                    });

                    tasks.Add(task);
                }
            }

            await Task.WhenAll(tasks);
        }
               

        private async Task SaveToDatabaseAsync(string machineName, string location, string cpu, string ram, string storageInfo, string os, string buildNumber)
        {
            // Replace with your actual connection string
            string connectionString = "Server=bananagoats.co.uk;Database=Ableworld;User Id=Elliot;Password=1234;";

            string query = @"
        MERGE INTO MachineData AS target
        USING (SELECT @MachineName AS MachineName) AS source
        ON target.MachineName = source.MachineName
        WHEN MATCHED THEN
            UPDATE SET
                Location = @Location,
                CPUInfo = @CPU,
                RAMInfo = @RAM,
                StorageInfo = @StorageInfo,
                WindowsOS = @OS,
                BuildNumber = @BuildNumber
        WHEN NOT MATCHED THEN
            INSERT (MachineName, Location, CPUInfo, RAMInfo, StorageInfo, WindowsOS, BuildNumber)
            VALUES (@MachineName, @Location, @CPU, @RAM, @StorageInfo, @OS, @BuildNumber);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@MachineName", machineName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Location", location ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CPU", cpu ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RAM", ram ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StorageInfo", storageInfo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@OS", os ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BuildNumber", buildNumber ?? (object)DBNull.Value);

                    // Open the connection and execute the query
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            catch
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
