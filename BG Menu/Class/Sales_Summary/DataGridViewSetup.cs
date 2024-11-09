using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BG_Menu.Class.Sales_Summary
{
    public static class DataGridViewSetup
    {

        // Simple location to handle the displays for all DataGridViews within the app. First attempt at using Class's

        public static void ClearCurrentCell(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    dataGridView.CurrentCell = null;
                }));
            }
        }

        public static void HideRowHeaders(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    dataGridView.RowHeadersVisible = false;
                }));
            }
        }

        public static void DisableAddRows(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    dataGridView.AllowUserToAddRows = false;
                }));
            }
        }

        public static void IncreaseColumnHeaderFontSize(float newSize, params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.Font = new Font(dataGridView.Font.FontFamily, newSize, FontStyle.Bold);
                    }
                }));
            }
        }

        public static void CenterAlignColumns(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }));
            }
        }

        public static void HideDataGridViews(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    dataGridView.Visible = false;
                }));
            }
        }

        public static void ShowDataGridViews(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    dataGridView.Visible = true;
                }));
            }
        }

        public static void DisableColumnSorting(params DataGridView[] dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
            {
                dataGridView?.Invoke((MethodInvoker)(() =>
                {
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }));
            }
        }

    }
}