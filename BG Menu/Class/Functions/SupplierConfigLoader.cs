using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BG_Menu.Class.Sales_Summary;
using Microsoft.Data.SqlClient;

namespace BG_Menu.Class.Functions
{
    public static class SupplierConfigLoader
    {
        private static readonly object SuppliersLock = new object();

        /// <summary>
        /// The in-memory cache of supplier configs for the app to use.
        /// </summary>
        public static List<Supplier> Suppliers { get; private set; } = new List<Supplier>();

        /// <summary>
        /// Reloads the Suppliers list from the single Suppliers table.
        /// Expects two pipe-delimited columns: DetectionKeywords and TotalExtractionRegexes.
        /// </summary>
        public static void LoadSuppliers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show(
                    "Database connection string 'SQL' is missing or empty.",
                    "Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LogMessage("Connection string 'SQL' is missing or empty.");
                return;
            }

            const string sql = @"
                SELECT
                    SupplierID,
                    Name,
                    GLAccount,
                    GLName,
                    VATCode,
                    DescriptionPattern,
                    FilenamePattern,
                    DetectionKeywords,
                    TotalExtractionRegexes
                  FROM Suppliers";

            try
            {
                // Execute the query into a DataTable
                DataTable dt = GlobalInstances.SalesRepository.ExecuteSqlQuery(sql);

                // Build a new list from the results
                var newList = new List<Supplier>();
                foreach (DataRow row in dt.Rows)
                {
                    var sup = new Supplier
                    {
                        SupplierID = (int)row["SupplierID"],
                        Name = row.Field<string>("Name"),
                        GLAccount = row.Field<string>("GLAccount"),
                        GLName = row.Field<string>("GLName"),
                        VATCode = row.Field<string>("VATCode"),
                        DescriptionPattern = row.Field<string>("DescriptionPattern"),
                        FilenamePattern = row.Field<string>("FilenamePattern"),
                        DetectionKeywords = (row.Field<string>("DetectionKeywords") ?? "")
                                                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .ToList(),
                        TotalExtractionRegexes = (row.Field<string>("TotalExtractionRegexes") ?? "")
                                                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .ToList()
                    };

                    newList.Add(sup);
                    LogMessage($"Loaded Supplier: ID={sup.SupplierID}, Name='{sup.Name}'");
                }

                // Swap in the new list under lock
                lock (SuppliersLock)
                {
                    Suppliers.Clear();
                    Suppliers.AddRange(newList);
                }

                LogMessage($"Successfully loaded {Suppliers.Count} suppliers from the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading supplier configurations:\n{ex.Message}",
                    "Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LogMessage($"Error loading suppliers: {ex}");
            }
        }

        /// <summary>
        /// Appends a line to ImportLog.txt for audit and troubleshooting.
        /// </summary>
        private static void LogMessage(string message)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportLog.txt");
                File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}");
            }
            catch
            {
                // Swallow logging errors
            }
        }

        public static void AddOrUpdateSupplier(
    string name,
    string keyword,
    string totalRegex,
    string glAccount,
    string glName,
    string vatCode)
        {
            // 1) Grab your connection string
            var connectionString = ConfigurationManager.ConnectionStrings["SQL"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Missing 'SQL' connection string.");

            // 2) Define your MERGE (upsert) statement
            const string upsert = @"
    MERGE INTO Suppliers AS target
    USING (SELECT @Name AS Name) AS src
      ON target.Name = src.Name
    WHEN NOT MATCHED THEN
      INSERT (Name, GLAccount, GLName, VATCode, DescriptionPattern, FilenamePattern, DetectionKeywords, TotalExtractionRegexes)
      VALUES (@Name, @GLAccount, @GLName, @VATCode,
              '{SupplierName} {Number}', '{Description}.pdf',
              @Keyword, @Regex)
    WHEN MATCHED THEN
      UPDATE
         SET GLAccount              = @GLAccount,
             GLName                 = @GLName,
             VATCode                = @VATCode,
             DetectionKeywords      = CASE WHEN ISNULL(DetectionKeywords,'') = '' THEN @Keyword ELSE DetectionKeywords      + '|' + @Keyword END,
             TotalExtractionRegexes = CASE WHEN ISNULL(TotalExtractionRegexes,'') = '' THEN @Regex   ELSE TotalExtractionRegexes + '|' + @Regex   END;";

            // 3) Open the connection & execute
            using (var conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(upsert, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@GLAccount", glAccount);
                cmd.Parameters.AddWithValue("@GLName", glName);
                cmd.Parameters.AddWithValue("@VATCode", vatCode);
                cmd.Parameters.AddWithValue("@Keyword", keyword);
                cmd.Parameters.AddWithValue("@Regex", totalRegex);

                cmd.ExecuteNonQuery();
            }

            // 4) Log for diagnostics
            LogMessage($"Upserted supplier '{name}' (GL: {glAccount}/{glName}, VAT: {vatCode}), added keyword '{keyword}' and regex '{totalRegex}'.");
        }
    }
}
