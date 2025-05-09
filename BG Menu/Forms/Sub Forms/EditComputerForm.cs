using BG_Menu.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BG_Menu.Class.Design;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class EditComputerForm : Form
    {

        private readonly string _machineName;
        private readonly SalesRepository _repo;
        private RoundedCorners roundedCorners;

        public EditComputerForm(string machineName, SalesRepository repo)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            var roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var Form = new Draggable(this, this);

            _machineName = machineName;
            _repo = repo;

            Text = $"Edit {_machineName}";
            txtMachine.Text = _machineName;

            PopulateLookups();
            LoadCurrentValues();
        }

        private void PopulateLookups()
        {
            var dtCompanies = _repo.ExecuteSqlQuery(
                "SELECT DISTINCT Company FROM dbo.Computers WHERE Company IS NOT NULL",
                null);

            foreach (DataRow r in dtCompanies.Rows)
                cmbCompany.Items.Add(r["Company"].ToString());

            var dtTypes = _repo.ExecuteSqlQuery(
                "SELECT DISTINCT Type FROM dbo.Computers WHERE Type IS NOT NULL",
                null);

            foreach (DataRow r in dtTypes.Rows)
                cmbType.Items.Add(r["Type"].ToString());

            cmbCompany.DropDownStyle = ComboBoxStyle.DropDown;
            cmbType.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void LoadCurrentValues()
        {
            var dt = _repo.ExecuteSqlQuery(
                "SELECT Location, Company, Type, CompanyName FROM dbo.Computers WHERE MachineName = @name",
                new Dictionary<string, object> { { "@name", _machineName } });

            if (dt.Rows.Count == 0) { /* handle missing */ }
            var row = dt.Rows[0];

            txtLocation.Text = row["Location"].ToString();
            cmbCompany.SelectedItem = row["Company"].ToString();
            cmbType.SelectedItem = row["Type"].ToString();
            txtCompanyName.Text = row["CompanyName"].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            const string sql = @"
                UPDATE dbo.Computers
                SET Location    = @location,
                    Company     = @company,
                    Type        = @type,
                    CompanyName = @companyName
                WHERE MachineName = @name;
            ";

            var parms = new Dictionary<string, object>
            {
                {"@location",    txtLocation  .Text.Trim() },
                {"@company",     cmbCompany   .Text.Trim() },  // use .Text so new entries work
                {"@type",        cmbType      .Text.Trim() },
                {"@companyName", txtCompanyName.Text.Trim() },
                {"@name",        _machineName }
            };

            int updated = _repo.ExecuteSqlNonQuery(sql, parms);
            if (updated == 0)
            {
                MessageBox.Show("Update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }


}
