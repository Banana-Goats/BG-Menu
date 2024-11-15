using BG_Menu.Class.Functions;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class Meraki : Form
    {
        private string connectionString = "Server=10.100.230.6:30015;UserID=ELLIOTRENNER;Password=Drop-Local-Poet-Knife-5";
        private BindingList<Product> productList = new BindingList<Product>();
        private BindingList<UnlistedProduct> unlistedProductList = new BindingList<UnlistedProduct>();
        private BindingSource productBindingSource = new BindingSource();
        private BindingSource unlistedProductBindingSource = new BindingSource();

        private Dictionary<string, string> databaseDictionary = new Dictionary<string, string>
        {
            { "UK Live", "SBO_AWUK_NEWLIVE" },
            { "SJLK", "SBO_SJLK_LIVE" },
            { "SML", "SBO_JCS_LIVE" },
            { "JSCD", "SBO_JSCD_LIVE" },
            { "AMD", "SBO_AMD_LIVE" },
            { "GRMR", "SBO_GRMR_LIVE" },
            { "MGB", "SBO_MGB_LIVE" },
            { "AWG", "SBO_AWG_LIVE" },
        };

        private string selectedDatabase;
        private string selectedWarehouse;

        private Dictionary<string, string> barcodeToProductDictionary = new Dictionary<string, string>();

        // Status Label for user feedback
        private Label labelStatus;

        private Dictionary<string, double> columnShrinkFactors = new Dictionary<string, double>
        {
            { "ItemCode", 0.4224 },   // Reduced by an additional 20% (from 0.528 to 0.4224)
            { "SuppSerial", 0.66 }     // Existing shrink factor
            // Add more columns here if needed
        };

        public Meraki()
        {
            string hanaClientPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HANA_Client_Dlls");
            Environment.SetEnvironmentVariable("PATH", hanaClientPath + ";" + Environment.GetEnvironmentVariable("PATH"));

            InitializeComponent();
            SetupStatusLabel();
            SetupDataGridView();
            PopulateDatabaseComboBox();
            SetupEventHandlers();

            productBindingSource.DataSource = productList;
            dataGridViewProducts.DataSource = productBindingSource;

            unlistedProductBindingSource.DataSource = unlistedProductList;
            dataGridViewUnlistedProducts.DataSource = unlistedProductBindingSource;
        }        

        private void SetupStatusLabel()
        {
            labelStatus = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Ready."
            };
            this.Controls.Add(labelStatus);
        }

        private void SetupEventHandlers()
        {
            checkBoxShowAllProducts.CheckedChanged += (s, e) => UpdateProductDisplay();
            checkBoxShowSerializedBatchItems.CheckedChanged += (s, e) => UpdateProductDisplay();
            checkBoxShowDiscrepancies.CheckedChanged += (s, e) => UpdateProductDisplay();
            comboBoxDatabases.SelectedIndexChanged += ComboBoxDatabases_SelectedIndexChanged;
            comboBoxWarehouses.SelectedIndexChanged += ComboBoxWarehouses_SelectedIndexChanged;
            textBoxProductNumber.KeyDown += TextBoxProductNumber_KeyDown;
            buttonAmendScannedAmount.Click += ButtonAmendScannedAmount_Click;
            buttonExportMismatched.Click += ButtonExportMismatched_Click;
        }

        private void SetupDataGridView()
        {
            // Configure dataGridViewProducts columns
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.ReadOnly = true;
            dataGridViewProducts.Columns.Clear();

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemCode",
                HeaderText = "Product Number",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Item Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ScannedAmount",
                HeaderText = "Amount",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "WarehouseStock",
                HeaderText = "On File",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Configure dataGridViewUnlistedProducts columns
            dataGridViewUnlistedProducts.AutoGenerateColumns = false;
            dataGridViewUnlistedProducts.AllowUserToAddRows = false;
            dataGridViewUnlistedProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUnlistedProducts.MultiSelect = false;
            dataGridViewUnlistedProducts.ReadOnly = true;
            dataGridViewUnlistedProducts.Columns.Clear();

            dataGridViewUnlistedProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemCode",
                HeaderText = "Item Code",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewUnlistedProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dataGridViewUnlistedProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ScannedAmount",
                HeaderText = "Scanned Amount",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
        }

        private void PopulateDatabaseComboBox()
        {
            comboBoxDatabases.DataSource = new BindingSource(databaseDictionary, null);
            comboBoxDatabases.DisplayMember = "Key";
            comboBoxDatabases.ValueMember = "Value";
        }

        private void ComboBoxDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDatabases.SelectedItem is KeyValuePair<string, string> selected)
            {
                selectedDatabase = selected.Value;
                comboBoxDatabases.Enabled = false;

                // Clear existing warehouses and load new ones
                comboBoxWarehouses.DataSource = null;
                comboBoxWarehouses.Items.Clear();
                LoadWarehousesForDatabase(selectedDatabase);
            }
        }

        private void ComboBoxWarehouses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxWarehouses.SelectedValue != null)
            {
                selectedWarehouse = comboBoxWarehouses.SelectedValue.ToString();

                // Clear existing data
                productList.Clear();
                unlistedProductList.Clear();

                // Load product data
                LoadProductsAsync();
            }
        }

        private void LoadWarehousesForDatabase(string databaseName)
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Updated SQL query with ORDER BY clause
                    string query = $@"
                SELECT ""WhsCode"", ""WhsName"" 
                FROM ""{databaseName}"".""OWHS""
                ORDER BY ""WhsName"" ASC"; // Sorting by Warehouse Name

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    comboBoxWarehouses.DataSource = dataTable;
                    comboBoxWarehouses.DisplayMember = "WhsName";
                    comboBoxWarehouses.ValueMember = "WhsCode";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouses: " + ex.Message);
                }
            }
        }

        private void LoadProductsAsync()
        {
            // Load products and barcodes asynchronously
            Task.Run(() =>
            {
                LoadMainProductsFromHana();
                LoadAdditionalBarcodesFromHana();

                // Update the UI on the main thread
                this.Invoke(new Action(() => UpdateProductDisplay()));
            });
        }

        private void LoadMainProductsFromHana()
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $@"
                        SELECT 
                            T0.""ItemCode"", 
                            T0.""ItemName"",
                            T0.""ManSerNum"",       
                            T1.""OnHand"" AS ""WarehouseStock""
                        FROM ""{selectedDatabase}"".""OITM"" T0
                        LEFT JOIN ""{selectedDatabase}"".""OITW"" T1 ON T0.""ItemCode"" = T1.""ItemCode""
                        WHERE T1.""WhsCode"" = '{selectedWarehouse}'";

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Product product = new Product
                        {
                            ItemCode = row["ItemCode"].ToString().Trim(),
                            Description = row["ItemName"].ToString().Trim(),
                            WarehouseStock = Convert.ToInt32(row["WarehouseStock"]),
                            IsSerializedOrBatchManaged = row["ManSerNum"].ToString().Trim().Equals("Y", StringComparison.OrdinalIgnoreCase),
                            ScannedAmount = 0
                        };

                        productList.Add(product);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading main products: " + ex.Message);
                }
            }
        }

        private void LoadAdditionalBarcodesFromHana()
        {
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $@"
                        SELECT 
                            T1.""BcdCode"", 
                            T0.""ItemCode""
                        FROM ""{selectedDatabase}"".""OITM"" T0
                        INNER JOIN ""{selectedDatabase}"".""OBCD"" T1 ON T0.""ItemCode"" = T1.""ItemCode""
                        WHERE T1.""BcdCode"" IS NOT NULL";

                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    barcodeToProductDictionary.Clear();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string aliasBarcode = row["BcdCode"].ToString().Trim();
                        string mainProductCode = row["ItemCode"].ToString().Trim();
                        barcodeToProductDictionary[aliasBarcode.ToUpper()] = mainProductCode;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading additional barcodes: " + ex.Message);
                }
            }
        }

        private void UpdateProductDisplay()
        {
            var filteredProducts = productList.AsEnumerable();

            // Exclude items with both ScannedAmount and WarehouseStock equal to zero
            filteredProducts = filteredProducts.Where(p => p.ScannedAmount > 0 || p.WarehouseStock > 0);

            if (!checkBoxShowAllProducts.Checked)
            {
                filteredProducts = filteredProducts.Where(p => p.ScannedAmount > 0);
            }

            if (checkBoxShowSerializedBatchItems.Checked)
            {
                filteredProducts = filteredProducts.Where(p => !p.IsSerializedOrBatchManaged);
            }

            if (checkBoxShowDiscrepancies.Checked)
            {
                filteredProducts = filteredProducts.Where(p => p.ScannedAmount != p.WarehouseStock);
            }

            productBindingSource.DataSource = new BindingList<Product>(filteredProducts.ToList());
            dataGridViewProducts.Refresh();
        }

        private void TextBoxProductNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string scannedCode = textBoxProductNumber.Text.Trim();
                if (string.IsNullOrEmpty(scannedCode))
                {
                    MessageBox.Show("Scanned code cannot be empty.");
                    return;
                }

                Product product = null;

                // Check if scanned code matches ItemCode (case-insensitive)
                product = productList.FirstOrDefault(p =>
                    string.Equals(p.ItemCode.Trim(), scannedCode, StringComparison.OrdinalIgnoreCase));

                // Check if scanned code matches an alias barcode (case-insensitive)
                if (product == null && barcodeToProductDictionary.TryGetValue(scannedCode.ToUpper(), out string itemCode))
                {
                    product = productList.FirstOrDefault(p =>
                        string.Equals(p.ItemCode.Trim(), itemCode, StringComparison.OrdinalIgnoreCase));
                }

                if (product != null)
                {
                    product.ScannedAmount++;

                    // Update last scanned product details
                    textBoxLastScannedItemCode.Text = product.ItemCode;
                    textBoxLastScannedDescription.Text = product.Description;
                    textBoxLastScannedQty.Text = product.ScannedAmount.ToString();
                    textBoxLastScannedOnHandQty.Text = product.WarehouseStock.ToString();

                    UpdateProductDisplay();

                    // Update status label
                    labelStatus.Text = $"Scanned: {product.ItemCode} - Total Scanned: {product.ScannedAmount}";
                }
                else
                {
                    // Handle unlisted products with case-insensitive and trimmed comparison
                    var unlistedProduct = unlistedProductList
                        .FirstOrDefault(p =>
                            string.Equals(p.ItemCode.Trim(), scannedCode, StringComparison.OrdinalIgnoreCase));

                    if (unlistedProduct != null)
                    {
                        unlistedProduct.ScannedAmount++;
                        // Update status label instead of MessageBox
                        labelStatus.Text = $"Incremented scanned amount for unlisted item: {unlistedProduct.ItemCode} - Total Scanned: {unlistedProduct.ScannedAmount}";
                    }
                    else
                    {
                        string description = PromptForDescription(scannedCode);
                        if (!string.IsNullOrEmpty(description))
                        {
                            unlistedProductList.Add(new UnlistedProduct
                            {
                                ItemCode = scannedCode.Trim(),
                                Description = description.Trim(),
                                ScannedAmount = 1
                            });

                            // Update status label for new unlisted item
                            labelStatus.Text = $"Added new unlisted item: {scannedCode.Trim()}";
                        }
                        else
                        {
                            labelStatus.Text = "Description was not provided for the scanned item.";
                        }
                    }
                }

                textBoxProductNumber.Clear();
                e.Handled = true;
            }
        }

        private string PromptForDescription(string itemCode)
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 350;
                prompt.Height = 180;
                prompt.Text = "Enter Product Description";
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;

                Label textLabel = new Label() { Left = 10, Top = 20, Text = $"Enter description for item code: {itemCode}", AutoSize = true };
                TextBox inputBox = new TextBox() { Left = 10, Top = 50, Width = 300 };

                Button confirmation = new Button() { Text = "OK", Left = 240, Width = 70, Top = 90, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text.Trim() : null;
            }
        }

        private void ButtonAmendScannedAmount_Click(object sender, EventArgs e)
        {
            // Retrieve the product number and new scanned amount from the TextBoxes
            string productNumber = textBoxLastScannedItemCode.Text.Trim();
            if (string.IsNullOrEmpty(productNumber))
            {
                MessageBox.Show("No product selected to amend.");
                return;
            }

            if (!int.TryParse(textBoxLastScannedQty.Text.Trim(), out int newScannedAmount))
            {
                MessageBox.Show("Invalid scanned amount. Please enter a valid number.");
                return;
            }

            var product = productList.FirstOrDefault(p => string.Equals(p.ItemCode, productNumber, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                product.ScannedAmount = newScannedAmount;
                UpdateProductDisplay();
                labelStatus.Text = $"Scanned amount for product {productNumber} has been updated to {newScannedAmount}.";
            }
            else
            {
                MessageBox.Show($"Product with Item Code {productNumber} not found.");
            }
        }

        private void ButtonExportMismatched_Click(object sender, EventArgs e)
        {
            // Ensure the selected warehouse is set
            if (string.IsNullOrEmpty(selectedWarehouse))
            {
                MessageBox.Show("Please select a warehouse before exporting.");
                return;
            }

            // Filter mismatched products where:
            // 1. ScannedAmount != WarehouseStock
            // 2. At least one of ScannedAmount or WarehouseStock is non-zero
            // 3. The product is not serialized or batch managed
            var mismatchedProducts = productList
                .Where(p => p.ScannedAmount != p.WarehouseStock
                            && (p.ScannedAmount != 0 || p.WarehouseStock != 0)
                            && !p.IsSerializedOrBatchManaged)
                .ToList();

            if (!mismatchedProducts.Any())
            {
                MessageBox.Show("No mismatched items found for export.");
                return;
            }

            // Prepare data table for export
            DataTable exportData = new DataTable();
            exportData.Columns.Add("ItemCode", typeof(string));
            exportData.Columns.Add("Description", typeof(string));
            exportData.Columns.Add("Freeze", typeof(string));
            exportData.Columns.Add("Warehouse", typeof(string));
            exportData.Columns.Add("OnFile", typeof(string));
            exportData.Columns.Add("Counted", typeof(string));
            exportData.Columns.Add("FakeCounted", typeof(string));
            exportData.Columns.Add("Scanned", typeof(int));

            foreach (var product in mismatchedProducts)
            {
                exportData.Rows.Add(
                    product.ItemCode,
                    product.Description,
                    "N",                   // Freeze
                    selectedWarehouse,     // Warehouse
                    "",                    // OnFile (leave blank for SAP auto-population)
                    "Y",                   // Counted
                    "",                    // FakeCounted (leave blank)
                    product.ScannedAmount  // Scanned
                );
            }

            // Create and configure a new form to display the export data
            Form exportForm = new Form
            {
                Text = $"Export Mismatched Items - Warehouse: {selectedWarehouse}",
                Size = new Size(1000, 600),
                StartPosition = FormStartPosition.CenterScreen
            };

            // Create and configure the DataGridView
            DataGridView dataGridViewExport = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = exportData,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = true // Enable auto-generation of columns
            };

            // Attach the DataBindingComplete event handler to set column headers safely
            dataGridViewExport.DataBindingComplete += (s, eArgs) =>
            {
                SetDataGridViewColumnHeaders(dataGridViewExport);
            };

            // Add the DataGridView to the form and display it
            exportForm.Controls.Add(dataGridViewExport);
            exportForm.ShowDialog();
        }

        // Helper method to set column headers safely
        private void SetDataGridViewColumnHeaders(DataGridView dgv)
        {
            // Define a mapping of DataTable column names to desired header texts
            var headerMappings = new Dictionary<string, string>
            {
                { "ItemCode", "Item No." },
                { "Description", "Item Description" },
                { "Freeze", "Freeze" },
                { "Warehouse", "Whse" },
                { "OnFile", "In-Whse Qty on Count Date" },
                { "Counted", "Counted" },
                { "FakeCounted", "UoM Counted Qty" },
                { "Scanned", "Counted Qty" }
            };

            foreach (var mapping in headerMappings)
            {
                if (dgv.Columns.Contains(mapping.Key))
                {
                    dgv.Columns[mapping.Key].HeaderText = mapping.Value;
                }
            }
        }

        private void PrintSerial_Click(object sender, EventArgs e)
        {
            // Ensure the selected warehouse is set
            if (string.IsNullOrEmpty(selectedWarehouse))
            {
                MessageBox.Show("Please select a warehouse before printing serial numbers.");
                return;
            }

            // Define the query to retrieve serial numbers
            string query = $@"
                            SELECT T1.""ItemCode"", T1.""ItemName"", T0.""SuppSerial""
                            FROM ""{selectedDatabase}"".""OSRI"" T0
                            INNER JOIN ""{selectedDatabase}"".""OITM"" T1 ON T0.""ItemCode"" = T1.""ItemCode""
                            WHERE T0.""Status"" = '0' AND T0.""WhsCode"" = '{selectedWarehouse}'
                            ORDER BY T1.""ItemName""";

            // Execute the query and get the data
            DataTable serialData = new DataTable();
            using (HanaConnection connection = new HanaConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    HanaDataAdapter adapter = new HanaDataAdapter(query, connection);
                    adapter.Fill(serialData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving serial numbers: " + ex.Message);
                    return;
                }
            }

            if (serialData.Rows.Count == 0)
            {
                MessageBox.Show("No serial numbers found for the selected warehouse.");
                return;
            }

            // Create and configure a new form to display the serial data
            Form printSerialForm = new Form
            {
                Text = $"Serial Numbers - Warehouse: {selectedWarehouse}",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterScreen
            };

            // Create and configure the DataGridView
            DataGridView dataGridViewSerial = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 500,
                DataSource = serialData,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = true
            };

            // Set column headers
            SetSerialDataGridViewColumnHeaders(dataGridViewSerial);

            // Create and configure the Print button
            Button buttonPrint = new Button
            {
                Text = "Print",
                Size = new Size(100, 30),
                Location = new Point((printSerialForm.ClientSize.Width - 100) / 2, 520)
            };
            buttonPrint.Click += (s, eArgs) =>
            {
                PrintSerialDataGridView(dataGridViewSerial);
            };

            // Add controls to the form
            printSerialForm.Controls.Add(dataGridViewSerial);
            printSerialForm.Controls.Add(buttonPrint);

            // Show the form
            printSerialForm.ShowDialog();
        }
        private void SetSerialDataGridViewColumnHeaders(DataGridView dgv)
        {
            // Define a mapping of DataTable column names to desired header texts
            var headerMappings = new Dictionary<string, string>
            {
                { "ItemCode", "Item No." },
                { "ItemName", "Item Description" },
                { "SuppSerial", "Serial Number" }
            };

            foreach (var mapping in headerMappings)
            {
                if (dgv.Columns.Contains(mapping.Key))
                {
                    dgv.Columns[mapping.Key].HeaderText = mapping.Value;
                }
            }
        }

        private void PrintSerialDataGridView(DataGridView dgv)
        {
            PrintDocument printDoc = new PrintDocument();

            // Set Paper Size to A4 Portrait
            printDoc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // Width=827, Height=1169 (hundredths of an inch)
            printDoc.DefaultPageSettings.Landscape = false; // Portrait orientation

            // Set Reduced Margins (e.g., 0.5 inches on each side)
            printDoc.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50); // Left, Right, Top, Bottom in hundredths of an inch

            PrintDialog printDlg = new PrintDialog
            {
                Document = printDoc
            };

            // Initialize the printer with adjusted column widths
            DataGridViewPrinter dgvPrinter = new DataGridViewPrinter(
                dgv,
                printDoc,
                true,  // center on page
                false, // isLandscape (already set to Portrait)
                "Serial Numbers",
                new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Point),
                Color.Black,
                true   // centerTitle
            );

            printDoc.PrintPage += dgvPrinter.PrintPage;
            printDoc.BeginPrint += dgvPrinter.BeginPrint;

            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error printing document: " + ex.Message);
                }
            }
        }

        // Data Models with INotifyPropertyChanged implementation
        public class Product : INotifyPropertyChanged
        {
            private string itemCode;
            private string description;
            private int warehouseStock;
            private bool isSerializedOrBatchManaged;
            private int scannedAmount;

            public string ItemCode
            {
                get => itemCode;
                set
                {
                    if (itemCode != value)
                    {
                        itemCode = value;
                        OnPropertyChanged(nameof(ItemCode));
                    }
                }
            }

            public string Description
            {
                get => description;
                set
                {
                    if (description != value)
                    {
                        description = value;
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            public int WarehouseStock
            {
                get => warehouseStock;
                set
                {
                    if (warehouseStock != value)
                    {
                        warehouseStock = value;
                        OnPropertyChanged(nameof(WarehouseStock));
                    }
                }
            }

            public bool IsSerializedOrBatchManaged
            {
                get => isSerializedOrBatchManaged;
                set
                {
                    if (isSerializedOrBatchManaged != value)
                    {
                        isSerializedOrBatchManaged = value;
                        OnPropertyChanged(nameof(IsSerializedOrBatchManaged));
                    }
                }
            }

            public int ScannedAmount
            {
                get => scannedAmount;
                set
                {
                    if (scannedAmount != value)
                    {
                        scannedAmount = value;
                        OnPropertyChanged(nameof(ScannedAmount));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class UnlistedProduct : INotifyPropertyChanged
        {
            private string itemCode;
            private string description;
            private int scannedAmount;

            public string ItemCode
            {
                get => itemCode;
                set
                {
                    if (itemCode != value)
                    {
                        itemCode = value;
                        OnPropertyChanged(nameof(ItemCode));
                    }
                }
            }

            public string Description
            {
                get => description;
                set
                {
                    if (description != value)
                    {
                        description = value;
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            public int ScannedAmount
            {
                get => scannedAmount;
                set
                {
                    if (scannedAmount != value)
                    {
                        scannedAmount = value;
                        OnPropertyChanged(nameof(ScannedAmount));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}