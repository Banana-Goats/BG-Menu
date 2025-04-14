using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Management;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;  // <-- TPL Dataflow
using System.Windows.Forms;
using System.Net.NetworkInformation;   // <-- For Ping

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class HO_Computers : Form
    {
        public HO_Computers()
        {
            InitializeComponent();

            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MachineName",
                HeaderText = "Machine Name",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "User",
                HeaderText = "Logged-In User",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "User Description",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CPU",
                HeaderText = "CPU",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RAM",
                HeaderText = "RAM",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HDD",
                HeaderText = "HDD ( Used Space )",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OS",
                HeaderText = "Operating System",
                SortMode = DataGridViewColumnSortMode.Automatic
            });
            dataGridViewPCInfo.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Build",
                HeaderText = "Build Number",
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            PopulateDataGrid();
        }

        private async void PopulateDataGrid()
        {
            dataGridViewPCInfo.Rows.Clear(); // Clear existing rows before adding new data

            var patternConfigurations = new List<(string pattern, int start, int end)>
            {
                ("WS-{0:D2}", 1, 80) // WS-01 to WS-80
            };

            var generatedMachineNames = new List<string>();
            foreach (var (pattern, start, end) in patternConfigurations)
            {
                for (int i = start; i <= end; i++)
                {
                    generatedMachineNames.Add(string.Format(pattern, i));
                }
            }

            var staticMachines = new List<string> { 
                "LT-71",
                "WS-54-Primary"
            };
            var allMachineNames = new List<string>();
            allMachineNames.AddRange(generatedMachineNames);
            allMachineNames.AddRange(staticMachines);

            var getInfoBlock = new TransformBlock<string, (string, Dictionary<string, string>)>(
                async machineName =>
                {
                    if (!IsMachineReachable(machineName, 1000)) return (machineName, null);
                    var info = GetMachineInfo(machineName);
                    return (machineName, info);
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10 }
            );

            var processInfoBlock = new ActionBlock<(string, Dictionary<string, string>)>(
                async tuple =>
                {
                    var (machineName, info) = tuple;
                    if (info == null || info["CPU"] == "N/A") return;

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
                    }));

                    await SaveToDatabaseAsync(
                        machineName,
                        info["User"],
                        info["CPU"],
                        info["RAM"],
                        info["HDD"],
                        info["OS"],
                        info["Build"]
                    );
                },
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
            );

            getInfoBlock.LinkTo(processInfoBlock, new DataflowLinkOptions { PropagateCompletion = true });

            foreach (var machineName in allMachineNames)
            {
                await getInfoBlock.SendAsync(machineName);
            }

            getInfoBlock.Complete();
            await processInfoBlock.Completion;

            dataGridViewPCInfo.Sort(
                dataGridViewPCInfo.Columns["MachineName"],
                System.ComponentModel.ListSortDirection.Ascending
            );

            MessageBox.Show(
                "All machines have been processed!",
                "Finished",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private async Task SaveToDatabaseAsync(string machineName, string location, string cpu, string ram,
                                       string storageInfo, string os, string buildNumber)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            string query = @"
        MERGE INTO HOPC AS target
        USING (SELECT @MachineName AS Name) AS source
        ON target.Name = source.Name
        WHEN MATCHED THEN
            UPDATE SET
                Store = @Location,
                CPU = @CPU,
                Ram = @RAM,
                HHD = @StorageInfo,
                OS = @OS,
                OS_Version = @BuildNumber
        WHEN NOT MATCHED THEN
            INSERT (Name, Store, CPU, Ram, HHD, OS, OS_Version)
            VALUES (@MachineName, @Location, @CPU, @RAM, @StorageInfo, @OS, @BuildNumber);
    ";

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

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, string> GetMachineInfo(string machineName)
        {
            var machineInfo = new Dictionary<string, string>
            {
                ["CPU"] = "N/A",
                ["RAM"] = "N/A",
                ["HDD"] = "N/A",
                ["OS"] = "N/A",
                ["Build"] = "N/A",
                ["User"] = "N/A",
                ["Description"] = "N/A"
            };

            try
            {
                // Connect to WMI
                ConnectionOptions options = new ConnectionOptions();
                // If desired, you can shorten the WMI timeout here:
                // options.Timeout = new TimeSpan(0,0,2);

                ManagementScope scope = new ManagementScope($@"\\{machineName}\root\cimv2", options);
                scope.Connect();

                // CPU
                machineInfo["CPU"] = GetCPUInfo(scope);

                // RAM
                machineInfo["RAM"] = GetRamInfo(scope);

                // Storage
                machineInfo["HDD"] = GetStorageInfo(scope);

                // OS
                machineInfo["OS"] = GetWindowsOS(scope);

                // Build
                machineInfo["Build"] = GetBuildNumber(scope);

                // User
                var (user, description) = GetLoggedInUser(scope);
                machineInfo["User"] = user;
                machineInfo["Description"] = description;
            }
            catch
            {
                // If we fail to connect, we keep "N/A" values
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
                        // AD usernames often look like DOMAIN\username
                        string[] parts = username.Split('\\');
                        string userNameOnly = (parts.Length > 1) ? parts[1] : username;

                        (loggedInUser, userDescription) = GetFullNameAndDescriptionFromAD(userNameOnly);
                    }
                    break; // Just assume one user
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving logged-in user information: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        fullName = $"{user.GivenName} {user.Surname}".Trim();
                        description = user.Description ?? "No description available";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user information from Active Directory: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (fullName, description);
        }

        private string GetCPUInfo(ManagementScope scope)
        {
            try
            {
                var query = new ObjectQuery("SELECT Name FROM Win32_Processor");
                var searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    string cpuInfo = obj["Name"].ToString();
                    // Optionally parse the CPU info to something friendlier
                    if (cpuInfo.Contains("Intel"))
                    {
                        cpuInfo = "Intel " + ExtractIntelModel(cpuInfo);
                    }
                    else if (cpuInfo.Contains("AMD"))
                    {
                        cpuInfo = "AMD " + ExtractAMDModel(cpuInfo);
                    }
                    return cpuInfo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving CPU information: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "N/A";
        }

        private string ExtractIntelModel(string cpuName)
        {
            var parts = cpuName.Split(' ');
            foreach (string part in parts)
            {
                // e.g. i5-8350U, i7-10700, etc.
                if (part.StartsWith("i") && part.Contains("-"))
                {
                    return part;
                }
            }
            return "Unknown Intel Model";
        }

        private string ExtractAMDModel(string cpuName)
        {
            var parts = cpuName.Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                // e.g. AMD Ryzen 7 3700U
                if (parts[i].Equals("Ryzen", StringComparison.OrdinalIgnoreCase))
                {
                    // Safely handle index
                    if (i + 2 < parts.Length)
                        return $"{parts[i]} {parts[i + 1]} {parts[i + 2]}";
                }
            }
            return "Unknown AMD Model";
        }

        private string GetRamInfo(ManagementScope scope)
        {
            try
            {
                var query = new ObjectQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (ulong.TryParse(obj["TotalPhysicalMemory"].ToString(), out ulong totalMemory))
                    {
                        return $"{(totalMemory / (1024 * 1024 * 1024)):0}GB";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving RAM information: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "N/A";
        }

        private string GetStorageInfo(ManagementScope scope)
        {
            long totalSize = 0;
            long availableSpace = 0;

            try
            {
                var query = new ObjectQuery("SELECT Size, FreeSpace FROM Win32_LogicalDisk WHERE DriveType=3");
                var searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject obj in searcher.Get())
                {
                    if (long.TryParse(obj["Size"]?.ToString(), out long sizeVal))
                        totalSize += sizeVal;

                    if (long.TryParse(obj["FreeSpace"]?.ToString(), out long freeVal))
                        availableSpace += freeVal;
                }

                long usedSpace = totalSize - availableSpace;
                return $"{(usedSpace / (1024 * 1024 * 1024)):0}/{(totalSize / (1024 * 1024 * 1024)):0}GB";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving storage information: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "N/A";
        }

        private string GetWindowsOS(ManagementScope scope)
        {
            try
            {
                var query = new ObjectQuery("SELECT Caption FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject os in searcher.Get())
                {
                    string osName = os["Caption"].ToString();
                    if (osName.StartsWith("Microsoft"))
                    {
                        osName = osName.Replace("Microsoft", "").Trim();
                    }
                    return osName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving OS information: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "Unknown OS";
        }

        private string GetBuildNumber(ManagementScope scope)
        {
            try
            {
                var query = new ObjectQuery("SELECT Version FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string versionStr = obj["Version"]?.ToString();
                    if (!string.IsNullOrEmpty(versionStr))
                    {
                        var parts = versionStr.Split('.');
                        // Return the last segment of the version string
                        return parts[parts.Length - 1];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving build number from remote machine: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return "Unknown Build";
        }

        private bool IsMachineReachable(string machineName, int timeoutMs = 200)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(machineName, timeoutMs);
                    return (reply != null && reply.Status == IPStatus.Success);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
