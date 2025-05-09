using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BG_Menu.Class.Functions;
using BG_Menu.Class.Sales_Summary;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class SupplierResolverForm : Form
    {
        public string SelectedSupplierName { get; private set; }
        public string NewDetectionKeyword { get; private set; }
        public string NewTotalRegex { get; private set; }
        public string SelectedGLAccount { get; private set; }
        public string SelectedGLName { get; private set; }
        public string SelectedVATCode { get; private set; }

        // Map of GLAccount code to GLName
        private Dictionary<string, string> _glMap;

        public SupplierResolverForm(string extractedText, List<Supplier> existingSuppliers)
        {
            InitializeComponent();

            // Display the raw invoice text
            richTextBox1.Text = extractedText;

            // Populate supplier dropdown
            cmbSuppliers.Items.AddRange(existingSuppliers.Select(s => s.Name).ToArray());
            cmbSuppliers.Items.Add("<New Supplier…>");
            cmbSuppliers.SelectedIndex = 0;
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            txtNewSupplierName.Enabled = false;

            // Build GLAccount -> GLName map
            _glMap = existingSuppliers
                .GroupBy(s => s.GLAccount)
                .ToDictionary(g => g.Key, g => g.First().GLName);

            // Populate GL Account dropdown
            cboGLAccount.Items.AddRange(_glMap.Keys.ToArray());
            cboGLAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGLAccount.SelectedIndexChanged += cboGLAccount_SelectedIndexChanged;
            if (cboGLAccount.Items.Count > 0) cboGLAccount.SelectedIndex = 0;

            // Populate VAT Code dropdown
            cboVATCode.Items.AddRange(existingSuppliers
                .Select(s => s.VATCode)
                .Distinct()
                .ToArray());
            cboVATCode.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cboVATCode.Items.Count > 0) cboVATCode.SelectedIndex = 0;
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isNew = cmbSuppliers.SelectedItem.ToString() == "<New Supplier…>";
            txtNewSupplierName.Enabled = isNew;
            TryEnableSave();
        }

        private void cboGLAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGLAccount.SelectedItem is string acct && _glMap.TryGetValue(acct, out var name))
            {
                txtGLName.Text = name;
            }
            else
            {
                txtGLName.Text = string.Empty;
            }
            TryEnableSave();
        }

        private void btnCaptureKeyword_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength == 0)
            {
                MessageBox.Show("Please highlight the supplier name first.", "Capture Keyword", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewDetectionKeyword = richTextBox1.SelectedText.Trim();
            MessageBox.Show($"Captured keyword: '{NewDetectionKeyword}'", "Keyword Captured", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TryEnableSave();
        }

        private void btnCaptureTotalExample_Click(object sender, EventArgs e)
        {
            var sel = richTextBox1.SelectedText;
            if (string.IsNullOrWhiteSpace(sel))
            {
                MessageBox.Show("Please highlight an example total first.", "Capture Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var numMatch = Regex.Match(sel, "\\d+(?:[.,]\\d+)*");
            if (!numMatch.Success)
            {
                MessageBox.Show("Couldn't find a numeric value in your selection.", "Capture Total", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var prefix = sel.Substring(0, numMatch.Index);
            var numeric = numMatch.Value;
            var suffix = sel.Substring(numMatch.Index + numeric.Length);

            var pattern = new StringBuilder();
            pattern
                .Append(Regex.Escape(prefix))
                .Append("(\\d{1,3}(?:[.,]\\d{3})*(?:\\.\\d{1,2})?)")
                .Append(Regex.Escape(suffix));

            NewTotalRegex = pattern.ToString();
            txtTotalRegex.Text = NewTotalRegex;
            MessageBox.Show($"Built regex: {NewTotalRegex}", "Total Regex", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TryEnableSave();
        }

        private void TryEnableSave()
        {
            bool hasSupplier = cmbSuppliers.SelectedItem.ToString() != "<New Supplier…>" || !string.IsNullOrEmpty(txtNewSupplierName.Text.Trim());

            btnSave.Enabled = hasSupplier
                && !string.IsNullOrEmpty(NewDetectionKeyword)
                && !string.IsNullOrEmpty(NewTotalRegex)
                && cboGLAccount.SelectedItem != null
                && cboVATCode.SelectedItem != null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SelectedSupplierName = cmbSuppliers.SelectedItem.ToString() == "<New Supplier…>"
                ? txtNewSupplierName.Text.Trim()
                : cmbSuppliers.SelectedItem.ToString();

            if (string.IsNullOrEmpty(SelectedSupplierName))
            {
                MessageBox.Show("Please enter a Supplier name.", "Save Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedGLAccount = cboGLAccount.SelectedItem.ToString();
            SelectedGLName = txtGLName.Text;
            SelectedVATCode = cboVATCode.SelectedItem.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}