using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class YeaStarConfig : Form
    {

        private static readonly HttpClient httpClient = new HttpClient();

        private bool isClearingSelection = false;

        private Dictionary<string, string> phoneDirectory = new Dictionary<string, string>();

        private readonly HashSet<string> excludedMenus = new HashSet<string>
        {
            "2. Store Landline UK" // Add more Menu Names here if needed
        };

        public YeaStarConfig()
        {
            InitializeComponent();

            FetchData();

            ConfigureDataGridView(dgvExpansionModule1, "Expansion Module 1 (1-20)");

            // Configure DataGridView2 (Keys 21-40)
            ConfigureDataGridView(dgvExpansionModule2, "Expansion Module 2 (21-40)");

            // Configure DataGridView3 (Keys 41-60)
            ConfigureDataGridView(dgvExpansionModule3, "Expansion Module 3 (41-60)");

            dgvExpansionModule1.ClearSelection();
            dgvExpansionModule2.ClearSelection();
            dgvExpansionModule3.ClearSelection();

            dgvExpansionModule1.SelectionChanged += DataGridView_SelectionChanged;
            dgvExpansionModule2.SelectionChanged += DataGridView_SelectionChanged;
            dgvExpansionModule3.SelectionChanged += DataGridView_SelectionChanged;

            txtSearchUser.KeyDown += TxtSearchUser_KeyDown;
            txtSearchUser.Leave += TxtSearchUser_Leave;
        }

               

        private void TxtSearchUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the 'ding' sound
                UpdateSelectedCell();
            }
        }

        private void TxtSearchUser_Leave(object sender, EventArgs e)
        {
            UpdateSelectedCell();
        }

        private void UpdateSelectedCell()
        {
            string selectedName = txtSearchUser.Text.Trim();

            if (string.IsNullOrEmpty(selectedName))
            {
                // Do nothing if no search is performed
                return;
            }

            // Check if the selected name exists in the phoneDirectory
            if (!phoneDirectory.ContainsKey(selectedName))
            {
                MessageBox.Show("User not found in the directory.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string phoneNumber = phoneDirectory[selectedName];

            // Check if any DataGridView has a selected cell
            DataGridView selectedGrid = null;
            DataGridViewCell selectedCell = null;

            if (dgvExpansionModule1.SelectedCells.Count > 0)
            {
                selectedGrid = dgvExpansionModule1;
                selectedCell = dgvExpansionModule1.SelectedCells[0];
            }
            else if (dgvExpansionModule2.SelectedCells.Count > 0)
            {
                selectedGrid = dgvExpansionModule2;
                selectedCell = dgvExpansionModule2.SelectedCells[0];
            }
            else if (dgvExpansionModule3.SelectedCells.Count > 0)
            {
                selectedGrid = dgvExpansionModule3;
                selectedCell = dgvExpansionModule3.SelectedCells[0];
            }

            if (selectedGrid == null || selectedCell == null)
            {
                MessageBox.Show("Please select a cell in one of the DataGridViews to update.", "No Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update the selected cell with the user's name and phone number
            selectedCell.Value = $"{selectedName}{Environment.NewLine}{phoneNumber}";

            // Optionally, clear the search TextBox after updating
            txtSearchUser.Clear();
        }

        #region Dictonary
        private async void FetchData()
        {

            // Define the HTTP URLs
            string[] urls = new string[]
            {
                "http://ableworldphone.co.uk/Phones/Head-Office.xml",
                "http://ableworldphone.co.uk/Phones/Store-Extension.xml",
                "http://ableworldphone.co.uk/Phones/Engineers Drivers SDMs.xml"
            };

            phoneDirectory.Clear();

            foreach (var url in urls)
            {
                try
                {
                    await FetchDataAsync(url, phoneDirectory);
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show($"HTTP Error fetching data from {url}:\r\n{httpEx.Message}", "HTTP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching data from {url}:\r\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            PopulateAutocompleteSource();


        }


        private async Task FetchDataAsync(string url, Dictionary<string, string> phoneDirectory)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseData = await response.Content.ReadAsStringAsync();

            ProcessXmlData(responseData, phoneDirectory);
        }

        private void ProcessXmlData(string xmlData, Dictionary<string, string> phoneDirectory)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlData);

                foreach (var menu in doc.Descendants("Menu"))
                {
                    string menuName = menu.Attribute("Name")?.Value.Trim() ?? string.Empty;

                    if (excludedMenus.Contains(menuName))
                    {
                        continue;
                    }

                    foreach (var unit in menu.Descendants("Unit"))
                    {
                        string unitName = unit.Attribute("Name")?.Value.Trim() ?? "Unnamed Unit";
                        string phone1 = unit.Attribute("Phone1")?.Value.Trim() ?? "N/A";

                        if (phoneDirectory.ContainsKey(unitName))
                        {
                            phoneDirectory[unitName] += $", {phone1}";
                        }
                        else
                        {
                            phoneDirectory.Add(unitName, phone1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of any parsing errors, throw an exception to be caught in the caller
                throw new Exception($"Error processing XML data: {ex.Message}");
            }
        }

        #endregion


        private void PopulateAutocompleteSource()
        {
            var autoComplete = new AutoCompleteStringCollection();
            foreach (var user in phoneDirectory.Keys)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    autoComplete.Add(user);
                }
            }
            txtSearchUser.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearchUser.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSearchUser.AutoCompleteCustomSource = autoComplete;
        }

        private void btnImportConfig_Click(object sender, EventArgs e)
        {
            string configText = txtConfig.Text;

            if (string.IsNullOrWhiteSpace(configText))
            {
                MessageBox.Show("Please paste the configuration into the textbox before importing.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse the config and extract label-value pairs for keys 1-60
            List<(string Label, string Value)> expansionData = ParseExpansionModule(configText, 60);

            if (expansionData.Count == 0)
            {
                MessageBox.Show("No expansion_module data found in the configuration.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Split the data into three parts: 1-20, 21-40, 41-60
            var firstSet = expansionData.GetRange(0, Math.Min(20, expansionData.Count));
            var secondSet = expansionData.Count > 20 ? expansionData.GetRange(20, Math.Min(20, expansionData.Count - 20)) : new List<(string Label, string Value)>();
            var thirdSet = expansionData.Count > 40 ? expansionData.GetRange(40, Math.Min(20, expansionData.Count - 40)) : new List<(string Label, string Value)>();

            // Populate each DataGridView
            PopulateDataGridView(dgvExpansionModule1, firstSet);
            PopulateDataGridView(dgvExpansionModule2, secondSet);
            PopulateDataGridView(dgvExpansionModule3, thirdSet);

            // Notify the user upon successful import
            MessageBox.Show("Configuration imported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<(string Label, string Value)> ParseExpansionModule(string config, int maxKeys)
        {
            var result = new List<(string Label, string Value)>();

            // Split the config into lines
            var lines = config.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Dictionary to hold key-value pairs
            var dict = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                // Ignore comments and empty lines
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    continue;

                // Split the line into key and value
                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                    continue; // Invalid line format

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();

                // Add to dictionary
                dict[key] = value;
            }

            // Extract expansion_module.1.key.{n}.label and expansion_module.1.key.{n}.value for n=1 to maxKeys
            for (int n = 1; n <= maxKeys; n++)
            {
                string labelKey = $"expansion_module.1.key.{n}.label";
                string valueKey = $"expansion_module.1.key.{n}.value";

                // Assign empty strings if keys are missing
                string label = dict.ContainsKey(labelKey) ? dict[labelKey] : string.Empty;
                string value = dict.ContainsKey(valueKey) ? dict[valueKey] : string.Empty;

                result.Add((label, value));
            }

            return result;
        }

        private void PopulateDataGridView(DataGridView dgv, List<(string Label, string Value)> data)
        {
            // Clear existing data
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["Label"].Value = string.Empty;
                row.Cells["Value"].Value = string.Empty;
            }

            // Populate the DataGridView in the specified order
            for (int i = 0; i < data.Count && i < 20; i++)
            {
                int row = i / 2; // Determine the row index (0 to 9)
                int col = i % 2; // Determine the column index (0 or 1)

                // Check if both label and value are empty
                if (string.IsNullOrEmpty(data[i].Label) && string.IsNullOrEmpty(data[i].Value))
                {
                    dgv[col, row].Value = string.Empty; // Leave cell blank
                }
                else
                {
                    // Assign the label and value to the respective cell with a line break
                    // Only include non-empty parts
                    string cellContent = string.Empty;
                    if (!string.IsNullOrEmpty(data[i].Label))
                    {
                        cellContent += data[i].Label;
                    }
                    if (!string.IsNullOrEmpty(data[i].Value))
                    {
                        if (!string.IsNullOrEmpty(cellContent))
                        {
                            cellContent += Environment.NewLine; // Add line break if label exists
                        }
                        cellContent += data[i].Value;
                    }

                    dgv[col, row].Value = cellContent;
                }
            }

            // Refresh the DataGridView to ensure updates are visible
            dgv.Refresh();
        }

        private void ConfigureDataGridView(DataGridView dgv, string moduleName)
        {
            dgv.ColumnCount = 2;
            dgv.Columns[0].Name = "Label";
            dgv.Columns[1].Name = "Value";

            // Enable text wrapping
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Automatically adjust row heights to fit content
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Initialize empty rows
            InitializeDataGridViewRows(dgv, 10);

            StyleDataGridView(dgv);

            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect; // Allow cell selection
            dgv.MultiSelect = false; // Disable multi-select
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            // Center-align the text vertically and horizontally
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Optionally, set a different font or style
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (isClearingSelection)
                return;

            try
            {
                isClearingSelection = true;

                DataGridView currentGrid = sender as DataGridView;

                if (currentGrid == null)
                    return;

                // Determine which grids to clear
                List<DataGridView> otherGrids = new List<DataGridView>();

                if (currentGrid == dgvExpansionModule1)
                {
                    otherGrids.Add(dgvExpansionModule2);
                    otherGrids.Add(dgvExpansionModule3);
                }
                else if (currentGrid == dgvExpansionModule2)
                {
                    otherGrids.Add(dgvExpansionModule1);
                    otherGrids.Add(dgvExpansionModule3);
                }
                else if (currentGrid == dgvExpansionModule3)
                {
                    otherGrids.Add(dgvExpansionModule1);
                    otherGrids.Add(dgvExpansionModule2);
                }

                // If current grid has any selected cells, clear selection in other grids
                if (currentGrid.SelectedCells.Count > 0)
                {
                    foreach (var grid in otherGrids)
                    {
                        grid.ClearSelection();
                    }
                }
            }
            finally
            {
                isClearingSelection = false;
            }
        }

        private void InitializeDataGridViewRows(DataGridView dgv, int rowCount)
        {
            dgv.Rows.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                dgv.Rows.Add();
            }
        }

        private void btnExportConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate the expansion module section from DataGridView content
                string expansionModuleSection = GenerateExpansionModuleSection();

                // Replace the placeholder in the base config with the generated expansion module section
                string baseConfig = GetCurrentBaseConfig();
                string updatedConfig = baseConfig.Replace("{EXPANSION_MODULE_SECTION}", expansionModuleSection);

                // Update the TextBox with the updated configuration
                txtConfig.Text = updatedConfig;

                MessageBox.Show("Configuration exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during export:\r\n{ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCurrentBaseConfig()
        {
            string selectedDevice = cmbDeviceType.SelectedItem.ToString();

            if (selectedDevice == "T46S")
            {
                return GetBaseConfigT46S();
            }
            else if (selectedDevice == "T46U")
            {
                return GetBaseConfigT46U();
            }
            else
            {
                throw new InvalidOperationException("Unknown device type selected.");
            }
        }

        private string GenerateExpansionModuleSection()
        {
            var sb = new System.Text.StringBuilder();
            int keyNumber = 1;

            // Iterate through all DataGridViews and their rows
            foreach (var dgv in new List<DataGridView> { dgvExpansionModule1, dgvExpansionModule2, dgvExpansionModule3 })
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row placeholder

                    // Process each cell in the row (both columns: Label and Value)
                    for (int col = 0; col < dgv.ColumnCount; col++)
                    {
                        // Retrieve the cell content, split into label and value
                        string cellContent = row.Cells[col].Value?.ToString() ?? string.Empty;
                        string[] lines = cellContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                        string label = lines.Length > 0 ? lines[0].Trim() : string.Empty;
                        string value = lines.Length > 1 ? lines[1].Trim() : string.Empty;

                        // Only include keys that have a Label or Value
                        if (!string.IsNullOrEmpty(label) || !string.IsNullOrEmpty(value))
                        {
                            // Add each line for the current key in the specified format
                            sb.AppendLine($"expansion_module.1.key.{keyNumber}.extension = *04");
                            sb.AppendLine($"expansion_module.1.key.{keyNumber}.label = {label}");
                            sb.AppendLine($"expansion_module.1.key.{keyNumber}.line = 1");
                            sb.AppendLine($"expansion_module.1.key.{keyNumber}.type = 16");
                            sb.AppendLine($"expansion_module.1.key.{keyNumber}.value = {value}");

                            sb.AppendLine(); // Add an empty line for readability

                            keyNumber++;
                        }
                    }
                }
            }

            // Remove the last appended empty line to prevent extra empty lines after the expansion module
            return sb.ToString().TrimEnd();
        }



        private void CmbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDevice = cmbDeviceType.SelectedItem.ToString();

            if (selectedDevice == "T46S")
            {
                txtConfig.Text = GetBaseConfigT46S();
            }
            else if (selectedDevice == "T46U")
            {
                txtConfig.Text = GetBaseConfigT46U();
            }
        }

        // Define methods to return base configurations

        private string GetBaseConfigT46S()
        {
            return @"#!version:1.0.0.1
##File header ""#!version:1.0.0.1"" can not be edited or deleted, and must be placed in the first line.##
lang.wui = {{.PhoneWebLanguage}}
lang.gui = {{.PhoneLanguage}}
voice.tone.country = {{.Tones}}
call_waiting.enable = {{.CallWaiting}}
static.security.user_password = {{.PhoneUser}}:{{.PhonePassword}}
local_time.time_zone = {{.TimeZone}}
local_time.time_zone_name = {{.TimeZoneName}}
local_time.summer_time = {{.DaylightSavingTime}}
local_time.ntp_server1 = {{.PrimaryNtpServer}}
local_time.ntp_server2 = {{.SecondaryNtpServer}}
local_time.time_format =  {{.TimeFormat}}
local_time.date_format =  {{.DateFormat}}
transfer.dsskey_deal_type = {{.TransferModeViaDsskey}}
features.dtmf.hide = {{.SuppressDtmfDisplay}}
features.dtmf.hide_delay = 1
features.intercom.led.enable = 1
features.intercom.subscribe.enable = 1
features.csta_control.enable = {{.EnableUacsta}}

remote_phonebook.data.1.url =  https://ableworldphone.co.uk/Phones/Head-Office.xml
remote_phonebook.data.1.name = Head Office
remote_phonebook.data.2.url =  https://ableworldphone.co.uk/Phones/Store-Extension.xml
remote_phonebook.data.2.name = Stores
remote_phonebook.data.3.url =  https://ableworldphone.co.uk/Phones/Engineers%20Drivers%20SDMs.xml
remote_phonebook.data.3.name = Engineers, Drivers, SDMs

features.remote_phonebook.enable=1

directory_setting.local_directory.enable=0
directory_setting.local_directory.priority=1
directory_setting.remote_phone_book.enable=1
directory_setting.remote_phone_book.priority=2

search_in_dialing.local_directory.enable=1
search_in_dialing.local_directory.priority=1
search_in_dialing.history.enable=1
search_in_dialing.history.priority=2
search_in_dialing.remote_phone_book.enable=1
search_in_dialing.remote_phone_book.priority=3

static.auto_provision.server.url = {{.AutoProvisionServerUrl}}/{{.ProvisioningFile}}
static.firmware.url = {{.FirmwareUrl}}/{{.FirmwareFile}}

ldap.enable = {{.EnableLdap}}
ldap.customize_label = {{.LdapName}}
ldap.tls_mode = {{.LdapMode}}
ldap.host = {{.LdapHost}}
ldap.name_filter = {{.LdapNameFilter}}
ldap.number_filter = {{.LdapNumFilter}}
ldap.name_attr = {{.LdapNameAttr}}
ldap.numb_attr = {{.LdapNumAttr}}
ldap.display_name = {{.LdapDisplayName}}
ldap.max_hits = {{.LdapMaxHit}}
ldap.call_in_lookup = {{.LdapIncomingLookup}}
ldap.call_out_lookup = {{.LdapDialLookup}}
ldap.ldap_sort = {{.LdapSort}}
ldap.version = 3
ldap.port = {{.LdapPort}}
ldap.base = {{.LdapBase}}
ldap.user = {{.LdapUser}}
ldap.password = {{.LdapPassword}}
ldap.incoming_call_special_search.enable = 1
ldap.numb_display_mode=1

phone_setting.lcd_logo.mode = 0
#lcd_logo.url = ${AUTOPLOG_URL}/autoplogo/yealink/yealinkt41.dob

features.dnd.off_code = *91
features.dnd.on_code = *93

programablekey.2.label = Directory
programablekey.2.line = 0
programablekey.2.type = 47

sip.call_fail_use_reason.enable = 0

static.network.vlan.internet_port_enable = 1
static.network.vlan.internet_port_vid = 30

{EXPANSION_MODULE_SECTION}

expansion_module.1.enable = 1
static.phone.reboot = 1
";
        }

        private string GetBaseConfigT46U()
        {
            return @"#!version:1.0.0.1
##File header ""#!version:1.0.0.1"" can not be edited or deleted, and must be placed in the first line.##
lang.wui = {{.PhoneWebLanguage}}
lang.gui = {{.PhoneLanguage}}
voice.tone.country = {{.Tones}}
call_waiting.enable = {{.CallWaiting}}
static.security.user_password = {{.PhoneUser}}:{{.PhonePassword}}
local_time.time_zone = {{.TimeZone}}
local_time.time_zone_name = {{.TimeZoneName}}
local_time.summer_time = {{.DaylightSavingTime}}
local_time.ntp_server1 = {{.PrimaryNtpServer}}
local_time.ntp_server2 = {{.SecondaryNtpServer}}
local_time.time_format =  {{.TimeFormat}}
local_time.date_format =  {{.DateFormat}}
transfer.dsskey_deal_type = {{.TransferModeViaDsskey}}
features.dtmf.hide = {{.SuppressDtmfDisplay}}
features.dtmf.hide_delay = 1
features.intercom.led.enable = 1
features.intercom.subscribe.enable = 1
features.csta_control.enable = {{.EnableUacsta}}

remote_phonebook.data.1.url = https://ableworldphone.co.uk/Phones/Head-Office.xml
remote_phonebook.data.1.name = Head Office
remote_phonebook.data.2.url = https://ableworldphone.co.uk/Phones/Store-Extension.xml
remote_phonebook.data.2.name = Stores
remote_phonebook.data.3.url = https://ableworldphone.co.uk/Phones/Engineers%20Drivers%20SDMs.xml
remote_phonebook.data.3.name = Engineers, Drivers, SDMs

features.remote_phonebook.enable=1

directory_setting.local_directory.enable=0
directory_setting.local_directory.priority=1
directory_setting.remote_phone_book.enable=1
directory_setting.remote_phone_book.priority=2

search_in_dialing.local_directory.enable=1
search_in_dialing.local_directory.priority=1
search_in_dialing.history.enable=1
search_in_dialing.history.priority=2
search_in_dialing.remote_phone_book.enable=1
search_in_dialing.remote_phone_book.priority=3

static.auto_provision.server.url = {{.AutoProvisionServerUrl}}/{{.ProvisioningFile}}
static.firmware.url = {{.FirmwareUrl}}/{{.FirmwareFile}}

distinctive_ring_tones.alert_info.1.text = {{.AlertInfoText_1}}
distinctive_ring_tones.alert_info.1.ringer = {{.AlertInfoRingTone_1}}
distinctive_ring_tones.alert_info.2.text = {{.AlertInfoText_2}}
distinctive_ring_tones.alert_info.2.ringer = {{.AlertInfoRingTone_2}}
distinctive_ring_tones.alert_info.3.text = {{.AlertInfoText_3}}
distinctive_ring_tones.alert_info.3.ringer = {{.AlertInfoRingTone_3}}
distinctive_ring_tones.alert_info.4.text = {{.AlertInfoText_4}}
distinctive_ring_tones.alert_info.4.ringer = {{.AlertInfoRingTone_4}}
distinctive_ring_tones.alert_info.5.text = {{.AlertInfoText_5}}
distinctive_ring_tones.alert_info.5.ringer = {{.AlertInfoRingTone_5}}
distinctive_ring_tones.alert_info.6.text = {{.AlertInfoText_6}}
distinctive_ring_tones.alert_info.6.ringer = {{.AlertInfoRingTone_6}}
distinctive_ring_tones.alert_info.7.text = {{.AlertInfoText_7}}
distinctive_ring_tones.alert_info.7.ringer = {{.AlertInfoRingTone_7}}
distinctive_ring_tones.alert_info.8.text = {{.AlertInfoText_8}}
distinctive_ring_tones.alert_info.8.ringer = {{.AlertInfoRingTone_8}}
distinctive_ring_tones.alert_info.9.text = {{.AlertInfoText_9}}
distinctive_ring_tones.alert_info.9.ringer = {{.AlertInfoRingTone_9}}
distinctive_ring_tones.alert_info.10.text = {{.AlertInfoText_10}}
distinctive_ring_tones.alert_info.10.ringer = {{.AlertInfoRingTone_10}}

#ACCOUNT
account.{{.Account}}.enable = {{.EnbAccount}}
account.{{.Account}}.label = {{.AccountLabel}}
account.{{.Account}}.display_name = {{.AccountDisplayName}}
account.{{.Account}}.auth_name = {{.AccountRegistrationName}}
account.{{.Account}}.user_name = {{.AccountRegistrationExtNumber}}
account.{{.Account}}.password = {{.AccountRegistrationPassword}}
account.{{.Account}}.sip_server.1.address = {{.AccountSipServerAddr}}
account.{{.Account}}.sip_server.1.port = {{.AccountSipServerPort}}
account.{{.Account}}.sip_server.1.transport_type = {{.AccountSipServerTransportType}}
account.{{.Account}}.sip_server.1.expires = 600
account.{{.Account}}.register_mac = 1
account.{{.Account}}.unregister_on_reboot = 1
account.{{.Account}}.auto_answer = {{.AutoAnswer}}
voice_mail.number.{{.Account}} = {{.CheckVoicemail}}
account.{{.Account}}.codec.pcmu.enable = {{.AccountCodecPcmu}}
account.{{.Account}}.codec.pcmu.priority = {{.AccountCodecPcmu_Priority}}
account.{{.Account}}.codec.pcma.enable = {{.AccountCodecPcma}}
account.{{.Account}}.codec.pcma.priority = {{.AccountCodecPcma_Priority}}
account.{{.Account}}.codec.ilbc_15_2kbps.enable = {{.AccountCodecIlbc_15_2_Kbps}}
account.{{.Account}}.codec.ilbc_15_2kbps.priority = {{.AccountCodecIlbc_15_2_Kbps_Priority}}
account.{{.Account}}.codec.ilbc_13_33kbps.enable = {{.AccountCodecIlbc_13_33_Kbps}}
account.{{.Account}}.codec.ilbc_13_33kbps.priority = {{.AccountCodecIlbc_13_33_Kbps_Priority}}
account.{{.Account}}.codec.g722.enable = {{.AccountCodecG722}}
account.{{.Account}}.codec.g722.priority = {{.AccountCodecG722_Priority}}
account.{{.Account}}.codec.g729.enable = {{.AccountCodecG729}}
account.{{.Account}}.codec.g729.priority = {{.AccountCodecG729_Priority}}
account.{{.Account}}.codec.g726_32.enable = {{.AccountCodecG726_32}}
account.{{.Account}}.codec.g726_32.priority = {{.AccountCodecG726_32_Priority}}
account.{{.Account}}.codec.g726_16.enable = 0
account.{{.Account}}.codec.g726_16.priority = 0
account.{{.Account}}.codec.g726_24.enable = 0
account.{{.Account}}.codec.g726_24.priority = 0
account.{{.Account}}.codec.g726_40.enable = 0
account.{{.Account}}.codec.g726_40.priority = 0
account.{{.Account}}.codec.g722_1c_48kpbs.enable = 0
account.{{.Account}}.codec.g722_1c_48kpbs.priority = 0
account.{{.Account}}.codec.g722_1c_32kpbs.enable = 0
account.{{.Account}}.codec.g722_1c_32kpbs.priority = 0
account.{{.Account}}.codec.g722_1c_24kpbs.enable = 0
account.{{.Account}}.codec.g722_1c_24kpbs.priority = 0
account.{{.Account}}.codec.g722_1_24kpbs.enable = 0
account.{{.Account}}.codec.g722_1_24kpbs.priority = 0
account.{{.Account}}.codec.opus.enable ={{.AccountCodecOpus}}
account.{{.Account}}.codec.opus.priority ={{.AccountCodecOpus_Priority}}
account.{{.Account}}.codec.g723_53.enable = 0
account.{{.Account}}.codec.g723_53.priority = 0
account.{{.Account}}.codec.g723_63.enable = 0
account.{{.Account}}.codec.g723_63.priority = 0

ldap.enable = {{.EnableLdap}}
ldap.customize_label = {{.LdapName}}
ldap.tls_mode = {{.LdapMode}}
ldap.host = {{.LdapHost}}
ldap.name_filter = {{.LdapNameFilter}}
ldap.number_filter = {{.LdapNumFilter}}
ldap.name_attr = {{.LdapNameAttr}}
ldap.numb_attr = {{.LdapNumAttr}}
ldap.display_name = {{.LdapDisplayName}}
ldap.max_hits = {{.LdapMaxHit}}
ldap.call_in_lookup = {{.LdapIncomingLookup}}
ldap.call_out_lookup = {{.LdapDialLookup}}
ldap.ldap_sort = {{.LdapSort}}
ldap.version = 3
ldap.port = {{.LdapPort}}
ldap.base = {{.LdapBase}}
ldap.user = {{.LdapUser}}
ldap.password = {{.LdapPassword}}
ldap.incoming_call_special_search.enable = 1
ldap.numb_display_mode=1

expansion_module.1.enable = 1
expansion_module.1.type = 43

{EXPANSION_MODULE_SECTION}

features.dnd.off_code = *91
features.dnd.on_code = *93

static.network.vlan.internet_port_enable = 1
static.network.vlan.internet_port_vid = 30

{{.FunctionkeySyntax}}";
        }


    }
}