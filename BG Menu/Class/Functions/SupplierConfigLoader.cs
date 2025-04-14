using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace BG_Menu.Class.Functions
{

    public static class SupplierConfigLoader
    {
        private static readonly object SuppliersLock = new object();
        public static List<Supplier> Suppliers { get; private set; } = new List<Supplier>();

        public static void LoadSuppliers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQL"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Database connection string 'SQL' is missing or empty.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage("Connection string 'SQL' is missing or empty.");
                return;
            }

            string combinedQuery = @"
                SELECT 
                    s.SupplierID, s.Name, s.GLAccount, s.GLName, s.VATCode, s.DescriptionPattern, s.FilenamePattern, 
                    s.TotalExtractionRegex, -- Include the regex column
                    sk.Keyword
                FROM 
                    Suppliers s
                LEFT JOIN 
                    SupplierDetectionKeywords sk ON s.SupplierID = sk.SupplierID";

            try
            {
                lock (SuppliersLock)
                {
                    Suppliers.Clear();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(combinedQuery, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int supplierID = reader.GetInt32(reader.GetOrdinal("SupplierID"));

                                // Check if supplier already exists in the list
                                Supplier supplier = Suppliers.FirstOrDefault(s => s.SupplierID == supplierID);
                                if (supplier == null)
                                {
                                    // Initialize a new Supplier
                                    supplier = new Supplier
                                    {
                                        SupplierID = supplierID,
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        GLAccount = reader.GetString(reader.GetOrdinal("GLAccount")),
                                        GLName = reader.GetString(reader.GetOrdinal("GLName")),
                                        VATCode = reader.GetString(reader.GetOrdinal("VATCode")),
                                        DescriptionPattern = reader.GetString(reader.GetOrdinal("DescriptionPattern")),
                                        FilenamePattern = reader.GetString(reader.GetOrdinal("FilenamePattern")),
                                        DetectionKeywords = new List<string>(),
                                        TotalExtractionRegexes = reader.IsDBNull(reader.GetOrdinal("TotalExtractionRegex"))
                                        ? new List<string>()
                                        : reader.GetString(reader.GetOrdinal("TotalExtractionRegex")).Split(';').ToList() // Handle multiple regex patterns
                                    };

                                    Suppliers.Add(supplier);
                                    LogMessage($"Loaded Supplier: ID={supplier.SupplierID}, Name={supplier.Name}, GLAccount={supplier.GLAccount}, GLName={supplier.GLName}");
                                }

                                // Add Detection Keyword if exists
                                if (!reader.IsDBNull(reader.GetOrdinal("Keyword")))
                                {
                                    string keyword = reader.GetString(reader.GetOrdinal("Keyword"));
                                    supplier.DetectionKeywords.Add(keyword);
                                    LogMessage($"Added Keyword for Supplier ID={supplier.SupplierID}: {keyword}");
                                }
                            }
                        }
                    }

                    LogMessage($"Successfully loaded {Suppliers.Count} suppliers from the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading supplier configurations from SQL Server:\n{ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Error loading suppliers from database: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Logs messages to a text file for auditing and troubleshooting.
        /// </summary>
        /// <param name="message">Message to log.</param>
        private static void LogMessage(string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportLog.txt");
            try
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch
            {
                // If logging fails, silently ignore to prevent user disruption.
            }
        }
    }
}
