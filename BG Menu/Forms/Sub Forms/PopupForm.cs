using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Forms.Sub_Forms
{
    public partial class PopupForm : Form
    {
        public PopupForm()
        {
            InitializeComponent();
        }

        public void SetData(DataTable dataTable)
        {
            var dataGridView = Controls["dataGridView"] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = dataTable;
            }
        }
    }
}
