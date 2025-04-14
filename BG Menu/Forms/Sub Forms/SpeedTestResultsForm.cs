using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class SpeedTestResultsForm : Form
    {
        private System.Windows.Forms.Timer _refreshTimer;
        private DataGridView dataGridViewResults;

        public SpeedTestResultsForm()
        {
            InitializeComponent();
            SetupDataGridView();
            StartRefreshTimer();
        }

        private void SetupDataGridView()
        {
            
        }

        private void StartRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // Refresh every 1 second
            };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
                string query = "SELECT * FROM SpeedTestResults";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewResults.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error or display a message.
                MessageBox.Show("Error loading speed test results: " + ex.Message);
            }
        }
    }
}
