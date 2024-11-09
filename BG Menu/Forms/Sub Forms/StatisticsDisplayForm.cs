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
    public partial class StatisticsDisplayForm : Form
    {
        private DataGridView statsDataGridView;

        public StatisticsDisplayForm()
        {
            this.Text = "Network Statistics";
            this.Size = new System.Drawing.Size(400, 300);

            statsDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ColumnCount = 2,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false
            };

            statsDataGridView.Columns[0].Name = "Metric";
            statsDataGridView.Columns[1].Name = "Count";

            this.Controls.Add(statsDataGridView);

            // Initialize the rows for each metric
            statsDataGridView.Rows.Add("Connected Clients", 0);
            statsDataGridView.Rows.Add("Last Alert Sent Times", 0);
            statsDataGridView.Rows.Add("Client Machine Names", 0);
            statsDataGridView.Rows.Add("Machine Name to Row", 0);
        }

        public void UpdateStatistics(int connectedClientsCount, int lastAlertSentTimesCount, int clientMachineNamesCount, int machineNameToRowCount)
        {
            if (statsDataGridView.InvokeRequired)
            {
                statsDataGridView.BeginInvoke(new Action(() => UpdateStatistics(connectedClientsCount, lastAlertSentTimesCount, clientMachineNamesCount, machineNameToRowCount)));
                return;
            }

            statsDataGridView.Rows[0].Cells[1].Value = connectedClientsCount;
            statsDataGridView.Rows[1].Cells[1].Value = lastAlertSentTimesCount;
            statsDataGridView.Rows[2].Cells[1].Value = clientMachineNamesCount;
            statsDataGridView.Rows[3].Cells[1].Value = machineNameToRowCount;
        }
    }
}

