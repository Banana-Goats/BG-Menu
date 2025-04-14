using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class CommandAutoUpdateForm : Form
    {
        public CommandAutoUpdateForm()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadCommandsData();
        }

        private void SetupDataGridView()
        {
            // Clear any existing columns
            dataGridViewCommands.Columns.Clear();

            // 1. Hidden ID column (for updates)
            DataGridViewTextBoxColumn colID = new DataGridViewTextBoxColumn();
            colID.Name = "ID";
            colID.DataPropertyName = "ID";
            colID.Visible = false;
            dataGridViewCommands.Columns.Add(colID);

            // 2. Description column
            DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn();
            colDescription.Name = "Description";
            colDescription.HeaderText = "Description";
            colDescription.DataPropertyName = "Description";
            colDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCommands.Columns.Add(colDescription);

            // 3. Value column
            DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
            colValue.Name = "Value";
            colValue.HeaderText = "Value";
            colValue.DataPropertyName = "Value";
            colValue.Width = 150;
            dataGridViewCommands.Columns.Add(colValue);

            // 4. Type column
            DataGridViewTextBoxColumn colType = new DataGridViewTextBoxColumn();
            colType.Name = "Type";
            colType.HeaderText = "Type";
            colType.DataPropertyName = "Type";
            colType.Width = 100;
            dataGridViewCommands.Columns.Add(colType);

            // 5. AutoUpdate column as a checkbox
            DataGridViewCheckBoxColumn colAutoUpdate = new DataGridViewCheckBoxColumn();
            colAutoUpdate.Name = "AutoUpdate";
            colAutoUpdate.HeaderText = "AutoUpdate";
            colAutoUpdate.DataPropertyName = "AutoUpdate";
            colAutoUpdate.TrueValue = "Yes";
            colAutoUpdate.FalseValue = "No";
            colAutoUpdate.Width = 100;
            dataGridViewCommands.Columns.Add(colAutoUpdate);
        }

        private void LoadCommandsData()
        {
            // Build a query to pull the desired columns from the Commands table.
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
            string query = "SELECT ID, Description, Value, Type, AutoUpdate FROM Commands";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the result to the DataGridView.
                    dataGridViewCommands.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Commands data: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Loop through each row in the grid and update the AutoUpdate value.
                    foreach (DataGridViewRow row in dataGridViewCommands.Rows)
                    {
                        // Get the ID (primary key) for the row.
                        int id = Convert.ToInt32(row.Cells["ID"].Value);

                        // Retrieve the value of the AutoUpdate cell.
                        // The cell value may be a Boolean or a string, so we handle both.
                        bool autoUpdateChecked = false;
                        if (row.Cells["AutoUpdate"].Value != null)
                        {
                            if (row.Cells["AutoUpdate"].Value is bool)
                            {
                                autoUpdateChecked = (bool)row.Cells["AutoUpdate"].Value;
                            }
                            else
                            {
                                string val = row.Cells["AutoUpdate"].Value.ToString();
                                autoUpdateChecked = val.Equals("Yes", StringComparison.OrdinalIgnoreCase);
                            }
                        }
                        string autoUpdateValue = autoUpdateChecked ? "Yes" : "No";

                        // Update the Commands table for this row.
                        string updateQuery = "UPDATE Commands SET AutoUpdate = @AutoUpdate WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@AutoUpdate", autoUpdateValue);
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Changes saved successfully.");

                // After saving, call UpdateColumnToYes("Commands") from your Updater form.
                // (For this to work, ensure that UpdateColumnToYes is declared public in the Updater form.)
                Updater parentForm = Application.OpenForms["Updater"] as Updater;
                if (parentForm != null)
                {
                    parentForm.UpdateColumnToYes("Commands");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Updater form not found. Please ensure it is open.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }
    }
}
