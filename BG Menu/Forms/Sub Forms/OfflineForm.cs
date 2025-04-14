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
    public partial class OfflineForm : Form
    {
        public OfflineForm()
        {
            // Remove window chrome and make it full screen
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.White;

            // Create a label for the message
            Label lblMessage = new Label
            {
                Text = "SQL Server Offline",
                ForeColor = Color.Red,
                Font = new Font("Arial", 48, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(lblMessage);
        }
    }
}
