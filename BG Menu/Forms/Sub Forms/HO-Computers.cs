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
using System.ComponentModel;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class HO_Computers : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isDomainAvailable;


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

            _isDomainAvailable = CheckDomainAvailability();
            PopulateDataGridAsync(_cts.Token);
        }

        private bool CheckDomainAvailability()
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    return context.ConnectedServer != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task PopulateDataGridAsync(CancellationToken ct)
        {
            dataGridViewPCInfo.Rows.Clear();

            var allMachineNames = Enumerable.Range(1, 80)
                .Select(i => $"WS-{i:D2}")
                .Concat(new[] { "LT-71", "WS-54-Primary" })
                .ToList();

            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = ct
            };

            var getInfoBlock = new TransformBlock<string, (string Machine, Dictionary<string, string> Info)>(
                async machine =>
                {
                    if (ct.IsCancellationRequested) return (machine, null);
                    if (!await PingAsync(machine, 500, ct)) return (machine, null);
                    var info = await Task.Run(() => GetMachineInfo(machine), ct);
                    return (machine, info);
                }, options
            );

            var processInfoBlock = new ActionBlock<(string Machine, Dictionary<string, string> Info)>(
                async tuple =>
                {
                    if (tuple.Info == null || ct.IsCancellationRequested) return;

                    Invoke((Action)(() =>
                    {
                        dataGridViewPCInfo.Rows.Add(
                            tuple.Machine,
                            tuple.Info["User"],
                            tuple.Info["Description"],
                            tuple.Info["CPU"],
                            tuple.Info["RAM"],
                            tuple.Info["HDD"],
                            tuple.Info["OS"],
                            tuple.Info["Build"]
                        );
                    }));

                    await SaveToDatabaseAsync(tuple.Machine, tuple.Info, ct);
                }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2, CancellationToken = ct });

            getInfoBlock.LinkTo(processInfoBlock, new DataflowLinkOptions { PropagateCompletion = true });

            foreach (var machineName in allMachineNames)
            {
                if (ct.IsCancellationRequested) break;
                await getInfoBlock.SendAsync(machineName, ct);
            }

            getInfoBlock.Complete();
            await processInfoBlock.Completion;

            if (!ct.IsCancellationRequested)
            {
                Invoke((Action)(() =>
                {
                    dataGridViewPCInfo.Sort(dataGridViewPCInfo.Columns["MachineName"], ListSortDirection.Ascending);
                    MessageBox.Show("All machines processed!", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }

        private async Task SaveToDatabaseAsync(string machineName, Dictionary<string, string> info, CancellationToken ct)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            const string query = @"
        MERGE INTO HOPC AS target
        USING (SELECT @MachineName AS Name) AS source
        ON target.Name = source.Name
        WHEN MATCHED THEN
            UPDATE SET
                Store = @Location, CPU = @CPU, Ram = @RAM, 
                HHD = @StorageInfo, OS = @OS, OS_Version = @BuildNumber
        WHEN NOT MATCHED THEN
            INSERT (Name, Store, CPU, Ram, HHD, OS, OS_Version)
            VALUES (@MachineName, @Location, @CPU, @RAM, @StorageInfo, @OS, @BuildNumber);";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MachineName", machineName);
                command.Parameters.AddWithValue("@Location", info["User"]);
                command.Parameters.AddWithValue("@CPU", info["CPU"]);
                command.Parameters.AddWithValue("@RAM", info["RAM"]);
                command.Parameters.AddWithValue("@StorageInfo", info["HDD"]);
                command.Parameters.AddWithValue("@OS", info["OS"]);
                command.Parameters.AddWithValue("@BuildNumber", info["Build"]);

                await connection.OpenAsync(ct);
                await command.ExecuteNonQueryAsync(ct);
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
                var scope = new ManagementScope($@"\\{machineName}\root\cimv2");
                scope.Connect();

                machineInfo["CPU"] = GetCPUInfo(scope);
                machineInfo["RAM"] = GetRamInfo(scope);
                machineInfo["HDD"] = GetStorageInfo(scope);
                machineInfo["OS"] = GetWindowsOS(scope);
                machineInfo["Build"] = GetBuildNumber(scope);

                var (user, desc) = _isDomainAvailable ? GetLoggedInUser(scope) : ("Domain Unavailable", "N/A");
                machineInfo["User"] = user;
                machineInfo["Description"] = desc;
            }
            catch
            {
                // Fail silently; already set to "N/A"
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

        private async Task<bool> PingAsync(string host, int timeout, CancellationToken ct)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(host, timeout);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _cts.Cancel();
        }
    }
}
