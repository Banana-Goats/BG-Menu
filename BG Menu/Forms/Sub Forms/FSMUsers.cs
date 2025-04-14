using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class FSMUsers : Form
    {
        // Path for storing the OAuth token temporarily.
        private readonly string tokenFilePath = Path.Combine(Path.GetTempPath(), "sap_fsm_token.txt");

        // OAuth endpoint details and client credentials.
        private const string TokenUrl = "https://eu.fsm.cloud.sap/api/oauth2/v2/token";
        private const string ClientId = "00016ccf-5e51-4e97-adcc-c5b42533cbe7";
        private const string ClientSecret = "568b27f0-7ff8-4c50-b156-98aab1f61e43";

        // Base API endpoint for users (without paging parameters).
        private const string UsersUrl = "https://eu.fsm.cloud.sap/api/user/v1/users?account=Ableworld_P1";

        // Class-level variable to hold the access token.
        private string _token = string.Empty;
        private DateTime _tokenExpiration = DateTime.MinValue;

        // Holds the complete filtered (active and planable) user list.
        private List<UserModel> _fullFilteredUserList = new List<UserModel>();

        public FSMUsers()
        {
            InitializeComponent();
            // Enable double buffering for smoother scrolling.
            SetDoubleBuffered(dataGridView1, true);
            SetDoubleBuffered(dataGridView2, true);

            // Wire the aggregation grid's selection changed event.
            dataGridView2.SelectionChanged += DataGridView2_SelectionChanged;

            // Wire the form load event to preload the token.
            this.Load += FSMUsers_Load;
        }

        // Form Load event handler that preloads the token.
        private async void FSMUsers_Load(object sender, EventArgs e)
        {
            try
            {
                _token = await GetAccessTokenAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error preloading token: " + ex.Message);
            }
        }

        // Event: When a row is selected in the aggregation grid, filter the detailed grid accordingly.
        private void DataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            // If no row selected, show full filtered list.
            if (dataGridView2.SelectedRows.Count == 0)
            {
                dataGridView1.DataSource = _fullFilteredUserList;
                return;
            }

            // Get the Company value from the selected row.
            var row = dataGridView2.SelectedRows[0].DataBoundItem as AggregationModel;
            if (row == null)
                return;

            string selectedCompany = row.Company;

            // If "Total" is selected, then remove company filtering.
            if (selectedCompany.Equals("Total", StringComparison.OrdinalIgnoreCase))
            {
                dataGridView1.DataSource = _fullFilteredUserList;
                return;
            }

            // Otherwise, filter the full list based on the rules:
            // For companies other than "UK", only show users that are NOT in multiple companies (IsMultiCompany false)
            // and whose companiesDisplay equals the selected company.
            // For "UK", show all users whose companiesDisplay equals "UK" (includes multi-company users).
            IEnumerable<UserModel> filtered;
            if (selectedCompany == "UK")
            {
                filtered = _fullFilteredUserList.Where(u => u.companiesDisplay == "UK");
            }
            else
            {
                filtered = _fullFilteredUserList.Where(u => !u.IsMultiCompany && u.companiesDisplay == selectedCompany);
            }

            dataGridView1.DataSource = filtered.ToList();
        }

        // Button click event: Retrieve user data and perform aggregation.
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Use the preloaded token if available; if not, get one.
                if (string.IsNullOrWhiteSpace(_token))
                {
                    _token = await GetAccessTokenAsync();
                }

                // Retrieve all pages of user data.
                List<UserModel> usersList = await GetAllUsersDataAsync(_token);

                // Define mapping: companyId -> Company Name.
                var companyMapping = new Dictionary<int, string>
                {
                    { 109076, "AMD" },
                    { 109697, "AWG" },
                    { 106565, "UK" },
                    { 109089, "GRMR" },
                    { 108953, "SML" },
                    { 108954, "JSCD" },
                    { 109094, "MGB" },
                    { 108827, "SJLK" }
                };

                // Process each user to compute computed fields.
                foreach (var user in usersList)
                {
                    // "planable": true if any company has a non-null groupId.
                    user.planable = user.companies != null && user.companies.Any(c => c.groupId.HasValue);

                    if (user.companies != null)
                    {
                        // Get the distinct mapped company names.
                        var mappedCompanies = user.companies
                            .Where(c => c.groupId.HasValue && companyMapping.ContainsKey(c.companyId))
                            .Select(c => companyMapping[c.companyId])
                            .Distinct()
                            .ToList();

                        // Flag if the user is in multiple companies.
                        user.IsMultiCompany = mappedCompanies.Count > 1;

                        // According to the rule:
                        // If in more than one, companiesDisplay is "UK".
                        // Otherwise, if exactly one, it's that company.
                        if (mappedCompanies.Count > 1)
                        {
                            user.companiesDisplay = "UK";
                        }
                        else if (mappedCompanies.Count == 1)
                        {
                            user.companiesDisplay = mappedCompanies.First();
                        }
                        else
                        {
                            user.companiesDisplay = string.Empty;
                        }
                    }
                    else
                    {
                        user.companiesDisplay = string.Empty;
                        user.IsMultiCompany = false;
                    }
                }

                // Filter the users: only those with active and planable true.
                var filteredUsers = usersList.Where(u => u.active && u.planable).ToList();
                _fullFilteredUserList = filteredUsers;

                // Bind the filtered user list to dataGridView1.
                SetupDataGridView();
                dataGridView1.DataSource = filteredUsers;

                // Aggregate data for the second grid.
                var aggregation = new Dictionary<string, int>();
                foreach (var user in filteredUsers)
                {
                    string companyToCount = user.companiesDisplay;
                    if (!string.IsNullOrEmpty(companyToCount))
                    {
                        if (!aggregation.ContainsKey(companyToCount))
                        {
                            aggregation[companyToCount] = 0;
                        }
                        aggregation[companyToCount]++;
                    }
                }

                // Add a "Total" row that sums all users.
                aggregation["Total"] = filteredUsers.Count;

                // Create a list of AggregationModel objects.
                var aggregationList = aggregation.Select(a => new AggregationModel
                {
                    Company = a.Key,
                    Count = a.Value
                }).ToList();

                // Bind the aggregation list to dataGridView2.
                SetupDataGridView2();
                dataGridView2.DataSource = aggregationList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Helper method: Sets double buffering on a control using reflection.
        private void SetDoubleBuffered(Control control, bool value)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(control, value, null);
        }

        // Retrieves the access token from file or calls the token endpoint.
        private async Task<string> GetAccessTokenAsync()
        {
            string token = string.Empty;

            if (File.Exists(tokenFilePath))
            {
                token = File.ReadAllText(tokenFilePath);
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                using (var client = new HttpClient())
                {
                    string authString = $"{ClientId}:{ClientSecret}";
                    string authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authBase64);

                    var formData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials")
                    });

                    HttpResponseMessage response = await client.PostAsync(TokenUrl, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic tokenData = JsonConvert.DeserializeObject(jsonResponse);
                        token = tokenData.access_token;
                        File.WriteAllText(tokenFilePath, token);
                    }
                    else
                    {
                        throw new Exception("Error retrieving token: " + response.ReasonPhrase);
                    }
                }
            }
            return token;
        }

        // Retrieves all pages of user data from the API.
        private async Task<List<UserModel>> GetAllUsersDataAsync(string accessToken)
        {
            List<UserModel> allUsers = new List<UserModel>();
            int currentPage = 0;
            int totalPages = 1;

            do
            {
                string pageUrl = UsersUrl + $"&page={currentPage}";
                string json = await GetUsersDataAsync(accessToken, pageUrl);
                UsersResponse response = JsonConvert.DeserializeObject<UsersResponse>(json);

                if (response?.content != null)
                {
                    allUsers.AddRange(response.content);
                }

                totalPages = response.totalPages;
                currentPage++;
            } while (currentPage < totalPages);

            return allUsers;
        }

        // Calls the Users API for a given URL (supports paging).
        private async Task<string> GetUsersDataAsync(string accessToken, string url = null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Client-ID", ClientId);
                client.DefaultRequestHeaders.Add("X-Client-Version", "0");

                string finalUrl = url ?? UsersUrl;
                HttpResponseMessage response = await client.GetAsync(finalUrl);

                // If unauthorized, clear both the cached token and the token file, then retry.
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _token = string.Empty; // clear the in-memory token

                    // Delete the token file if it exists.
                    if (File.Exists(tokenFilePath))
                    {
                        File.Delete(tokenFilePath);
                    }

                    // Get a new token.
                    accessToken = await GetAccessTokenAsync();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    response = await client.GetAsync(finalUrl);
                }

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("Error retrieving users: " + response.ReasonPhrase);
                }
            }
        }

        // Configures columns for dataGridView1 (detailed user grid).
        private void SetupDataGridView()
        {
            dataGridView1.Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = false;

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "name",
                    HeaderText = "Name"
                });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "firstName",
                    HeaderText = "First Name"
                });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "lastName",
                    HeaderText = "Last Name"
                });
                dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "active",
                    HeaderText = "Active",
                    TrueValue = true,
                    FalseValue = false,
                    ValueType = typeof(bool)
                });
                dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "planable",
                    HeaderText = "Planable",
                    TrueValue = true,
                    FalseValue = false,
                    ValueType = typeof(bool)
                });
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "companiesDisplay",
                    HeaderText = "Company"
                });
            });
        }

        // Configures columns for dataGridView2 (aggregation grid).
        private void SetupDataGridView2()
        {
            dataGridView2.Invoke((System.Windows.Forms.MethodInvoker)delegate
            {
                dataGridView2.Columns.Clear();
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Company",
                    HeaderText = "Company"
                });
                dataGridView2.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Count",
                    HeaderText = "Count"
                });
            });
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Check that a row is selected in dataGridView1.
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Please select a user from the detailed grid.");
                    return;
                }

                // Get the selected user.
                var selectedUser = dataGridView1.CurrentRow.DataBoundItem as UserModel;
                if (selectedUser == null)
                {
                    MessageBox.Show("Selected row is not valid.");
                    return;
                }

                // Use the user's "name" property as the user code.
                string userCode = selectedUser.name;

                // Build the SQL query string, replacing the hardcoded value with the selected user's code.
                string queryString = $"SELECT a.userName, t.name, r.remarks, r.startDate, r.endDate, r.createDateTime " +
                                     $"FROM Person a " +
                                     $"JOIN PersonReservation r ON a.id=r.person " +
                                     $"JOIN PersonReservationType t ON r.type=t.id " +
                                     $"WHERE a.userName IN ('{userCode}')";

                // URL-encode the query string.
                string encodedQuery = Uri.EscapeDataString(queryString);

                // Build the full API URL.
                string apiUrl = $"https://eu.fsm.cloud.sap/api/query/v1?account=Ableworld_P1&company=SBO_AWUK_NEWLIVE&dtos=Person.25;PersonReservation.21;PersonReservationType.17&query={encodedQuery}";

                // Use the preloaded token (_token). If not available, retrieve one.
                if (string.IsNullOrWhiteSpace(_token))
                {
                    _token = await GetAccessTokenAsync();
                }

                using (var client = new HttpClient())
                {
                    // Set up Bearer token authentication.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                    client.DefaultRequestHeaders.Add("X-Client-ID", ClientId);
                    client.DefaultRequestHeaders.Add("X-Client-Version", "0");

                    // Make the GET request.
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the JSON result.
                        string jsonResult = await response.Content.ReadAsStringAsync();

                        // Convert the JSON to CSV.
                        string csvData = ConvertJsonToCsv(jsonResult);

                        // Open a Save File Dialog so the user can choose where to save the CSV file.
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                            sfd.Title = "Save query result as CSV";
                            // Set the default file name to the user's code.
                            sfd.FileName = $"{userCode}.csv";

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                File.WriteAllText(sfd.FileName, csvData, Encoding.UTF8);
                                MessageBox.Show("CSV file saved successfully!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error retrieving query results: " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string ConvertJsonToCsv(string json)
        {
            // Parse the JSON into a JObject.
            JObject root = JObject.Parse(json);

            // Check for a "data" property that contains the array.
            JArray dataArray = root["data"] as JArray;
            if (dataArray == null)
            {
                // If no "data" property exists, assume the root is the array.
                dataArray = new JArray(root);
            }

            if (!dataArray.Any())
                return string.Empty;

            // Flatten the first record to extract CSV headers.
            var firstRecord = FlattenJsonObject((JObject)dataArray.First());
            var headers = firstRecord.Keys;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", headers));

            // Build rows for each record.
            foreach (JObject record in dataArray)
            {
                var flatRecord = FlattenJsonObject(record);
                List<string> fields = new List<string>();
                foreach (var header in headers)
                {
                    string field = flatRecord.ContainsKey(header) ? flatRecord[header] : "";
                    // Quote the field if necessary.
                    if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
                    {
                        field = "\"" + field.Replace("\"", "\"\"") + "\"";
                    }
                    fields.Add(field);
                }
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }

        private Dictionary<string, string> FlattenJsonObject(JObject jObj, string prefix = "")
        {
            var dict = new Dictionary<string, string>();
            foreach (var property in jObj.Properties())
            {
                string propertyName = string.IsNullOrEmpty(prefix) ? property.Name : prefix + "." + property.Name;
                if (property.Value is JObject nestedObj)
                {
                    var nestedDict = FlattenJsonObject(nestedObj, propertyName);
                    foreach (var kv in nestedDict)
                    {
                        dict[kv.Key] = kv.Value;
                    }
                }
                else if (property.Value is JArray array)
                {
                    // Convert arrays to a semicolon-separated string.
                    var values = array.Select(x => x.ToString()).ToArray();
                    dict[propertyName] = string.Join(";", values);
                }
                else
                {
                    dict[propertyName] = property.Value?.ToString() ?? "";
                }
            }
            return dict;
        }
    }

    // Wrapper class for the API JSON response.
    public class UsersResponse
    {
        public List<UserModel> content { get; set; }
        public object pageable { get; set; }
        public int totalPages { get; set; }
        public bool last { get; set; }
        public int totalElements { get; set; }
        public object sort { get; set; }
        public bool first { get; set; }
        public int size { get; set; }
        public int number { get; set; }
        public int numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    // Model class for a user.
    public class UserModel
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public List<string> roles { get; set; }
        public List<CompanyModel> companies { get; set; }
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime lastChanged { get; set; }
        // Computed properties.
        public bool planable { get; set; }
        public string companiesDisplay { get; set; }
        // Indicates if the user is mapped to more than one company.
        public bool IsMultiCompany { get; set; }
    }

    // Model class for a company in the user's companies array.
    public class CompanyModel
    {
        public int companyId { get; set; }
        public int? groupId { get; set; }
    }

    // Model class for aggregated results.
    public class AggregationModel
    {
        public string Company { get; set; }
        public int Count { get; set; }
    }
}
