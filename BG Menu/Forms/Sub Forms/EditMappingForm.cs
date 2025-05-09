using BG_Menu.Class.Design;
using BG_Menu.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class EditMappingForm : Form
    {
        private readonly string _machine;
        private readonly SalesRepository _repo;
        private RoundedCorners roundedCorners;

        public EditMappingForm(string machine, SalesRepository repo)
        {
            InitializeComponent();

            _machine = machine;
            _repo = repo;
            Text = $"Edit mapping for {_machine}";
            txtMachine.Text = _machine;

            var roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var Form = new Draggable(this, this);

            PopulateCompanyLookup();
            LoadCurrentValues();
        }

        private void PopulateCompanyLookup()
        {
            var dtC = _repo.ExecuteSqlQuery(
                "SELECT DISTINCT Company FROM Sales.SalesSheetMapping WHERE Company IS NOT NULL",
                null);
            foreach (DataRow r in dtC.Rows)
                cmbCompany.Items.Add(r["Company"].ToString());

            cmbCompany.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void LoadCurrentValues()
        {
            var dt = _repo.ExecuteSqlQuery(
                "SELECT Company, Mapping FROM Sales.SalesSheetMapping WHERE Machine = @m",
                new Dictionary<string, object> { { "@m", _machine } });

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            var r = dt.Rows[0];
            cmbCompany.Text = r["Company"].ToString();
            txtMapping.Text = r["Mapping"].ToString();   // <-- textbox instead of combobox
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            const string sql = @"
            UPDATE Sales.SalesSheetMapping
            SET Company = @company,
                Mapping = @mapping
            WHERE Machine = @m;
        ";

            var parms = new Dictionary<string, object>
        {
            {"@company", cmbCompany.Text.Trim()},
            {"@mapping", txtMapping.Text.Trim()},   // <-- textbox value
            {"@m",       _machine}
        };

            if (_repo.ExecuteSqlNonQuery(sql, parms) == 1)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
