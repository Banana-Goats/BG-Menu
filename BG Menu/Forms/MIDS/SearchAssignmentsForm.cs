using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class SearchAssignmentsForm : Form
    {

        private string connectionString;
        public string Result { get; private set; }

        public SearchAssignmentsForm(string connString)
        {
            InitializeComponent();
            connectionString = connString;
            SetDateTimePickerToSixAM();
        }

        private void SetDateTimePickerToSixAM()
        {
            DateTime selectedDate = dtpTargetDateTime.Value.Date;
            dtpTargetDateTime.Value = selectedDate.AddHours(19); // Sets time to 6:00 AM
        }

        private void dtpTargetDateTime_ValueChanged(object sender, EventArgs e)
        {
            SetDateTimePickerToSixAM();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Retrieve user inputs
            DateTime targetDateTime = dtpTargetDateTime.Value;
            string pPTID = string.IsNullOrWhiteSpace(txtPTID.Text) ? null : txtPTID.Text.Trim();
            string pTID = string.IsNullOrWhiteSpace(txtTID.Text) ? null : txtTID.Text.Trim();
            string pMerchantID = string.IsNullOrWhiteSpace(txtMerchantID.Text) ? null : txtMerchantID.Text.Trim();

            label1.Text = $"Search DateTime: {targetDateTime.ToString("yyyy-MM-dd HH:mm:ss")}";


            // Execute the query and get the DataTable
            DataTable resultsTable = ExecuteSearchDataTable(targetDateTime, pPTID, pTID, pMerchantID);

            if (resultsTable.Rows.Count > 0)
            {
                dataGridViewResults.DataSource = resultsTable;
                FormatDataGridView();
            }
            else
            {
                MessageBox.Show("No records found matching the provided criteria.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridViewResults.DataSource = null;
            }
        }

        private DataTable ExecuteSearchDataTable(DateTime targetDateTime, string pPTID, string pTID, string pMerchantID)
        {
            DataTable resultTable = new DataTable();
            string query = @"
        WITH LatestChanges AS (
            SELECT
                pa.IndexID,
                pa.FieldName,
                pa.NewValue,
                pa.ChangeDate,
                ROW_NUMBER() OVER (PARTITION BY pa.IndexID, pa.FieldName ORDER BY pa.ChangeDate DESC) AS rn
            FROM
                PaymentDevicesAudit pa
            WHERE
                pa.ChangeDate <= @TargetDateTime
                AND pa.FieldName IN ('AssignedUser', 'PTID', 'TID', 'MerchantID')
        ),
        ReconstructedState AS (
            SELECT
                lc.IndexID,
                MAX(CASE WHEN lc.FieldName = 'AssignedUser' AND lc.rn = 1 THEN lc.NewValue END) AS AssignedUser,
                MAX(CASE WHEN lc.FieldName = 'PTID' AND lc.rn = 1 THEN lc.NewValue END) AS PTID,
                MAX(CASE WHEN lc.FieldName = 'TID' AND lc.rn = 1 THEN lc.NewValue END) AS TID,
                MAX(CASE WHEN lc.FieldName = 'MerchantID' AND lc.rn = 1 THEN lc.NewValue END) AS MerchantID
            FROM
                LatestChanges lc
            GROUP BY
                lc.IndexID
        )
        SELECT
            rs.IndexID,
            rs.AssignedUser,
            rs.PTID,
            rs.TID,
            rs.MerchantID
        FROM
            ReconstructedState rs
        WHERE
            (@PTID IS NULL OR rs.PTID = @PTID)
            AND (@TID IS NULL OR rs.TID = @TID)
            AND (@MerchantID IS NULL OR rs.MerchantID = @MerchantID)
        ORDER BY
            rs.IndexID;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Define parameters
                cmd.Parameters.Add("@TargetDateTime", SqlDbType.DateTime).Value = targetDateTime;
                cmd.Parameters.Add("@PTID", SqlDbType.VarChar, 50).Value = (object)pPTID ?? DBNull.Value;
                cmd.Parameters.Add("@TID", SqlDbType.VarChar, 50).Value = (object)pTID ?? DBNull.Value;
                cmd.Parameters.Add("@MerchantID", SqlDbType.VarChar, 50).Value = (object)pMerchantID ?? DBNull.Value;

                try
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(resultTable);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return resultTable;
        }

        private void FormatDataGridView()
        {           

            // Set column headers
            dataGridViewResults.Columns["IndexID"].HeaderText = "Index ID";
            dataGridViewResults.Columns["AssignedUser"].HeaderText = "Assigned User";
            dataGridViewResults.Columns["PTID"].HeaderText = "PTID";
            dataGridViewResults.Columns["TID"].HeaderText = "TID";
            dataGridViewResults.Columns["MerchantID"].HeaderText = "Merchant ID";

            // Optionally, set column widths or other styles
            dataGridViewResults.Columns["IndexID"].Width = 60;
            dataGridViewResults.Columns["AssignedUser"].Width = 120;
            dataGridViewResults.Columns["PTID"].Width = 100;
            dataGridViewResults.Columns["TID"].Width = 100;
            dataGridViewResults.Columns["MerchantID"].Width = 120;

            // Set row headers to not display
            dataGridViewResults.RowHeadersVisible = false;

            // Make the DataGridView read-only
            dataGridViewResults.ReadOnly = true;

            // Disable adding and deleting rows
            dataGridViewResults.AllowUserToAddRows = false;
            dataGridViewResults.AllowUserToDeleteRows = false;

            // Enable full row selection
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
