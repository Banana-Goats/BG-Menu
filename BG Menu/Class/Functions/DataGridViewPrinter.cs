using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BG_Menu.Class.Functions
{
    /// <summary>
    /// Helper class to print DataGridView contents with support for multiple pages and custom column widths.
    /// </summary>
    public class DataGridViewPrinter
    {
        private DataGridView dgv;
        private PrintDocument printDoc;
        private bool isFirstPage;
        private bool isNewPage;
        private int totalWidth;
        private int rowPos;
        private List<int> columnWidths;
        private int headerHeight;
        private string title;
        private Font titleFont;
        private Color titleColor;
        private bool centerOnPage;

        // Declare yPos as a member variable
        private int yPos;

        // Define columns to adjust with specific shrink factors
        private Dictionary<string, double> columnShrinkFactors = new Dictionary<string, double>
        {
            { "ItemCode", 0.4224 },
            { "ItemName", 1.6024 },
            { "SuppSerial", 0.66 }    // Existing shrink factor
            // Add more columns here if needed
        };

        public DataGridViewPrinter(DataGridView grid, PrintDocument document, bool center, bool isLandscape, string reportTitle, Font titleFnt, Color titleClr, bool centerTitle)
        {
            dgv = grid;
            printDoc = document;
            centerOnPage = centerTitle;
            title = reportTitle;
            titleFont = titleFnt;
            titleColor = titleClr;

            if (isLandscape)
            {
                printDoc.DefaultPageSettings.Landscape = true;
            }
        }

        /// <summary>
        /// Initializes print settings before printing begins.
        /// </summary>
        public void BeginPrint(object sender, PrintEventArgs e)
        {
            try
            {
                isFirstPage = true;
                isNewPage = true;
                rowPos = 0;
                columnWidths = new List<int>();

                totalWidth = 0;
                foreach (DataGridViewColumn dgvCol in dgv.Columns)
                {
                    double factor = 1.0; // Default shrink factor

                    if (columnShrinkFactors.ContainsKey(dgvCol.Name))
                    {
                        factor = columnShrinkFactors[dgvCol.Name];
                    }

                    int colWidth = (int)(dgvCol.Width * factor);
                    columnWidths.Add(colWidth);
                    totalWidth += colWidth;
                }

                headerHeight = 0;
                foreach (DataGridViewRow dgvRow in dgv.Rows)
                {
                    headerHeight += dgvRow.Height;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during print initialization: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the PrintPage event to render each page.
        /// </summary>
        public void PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Set up the print area
                Rectangle marginBounds = e.MarginBounds;

                // Print title on the first page
                if (isFirstPage)
                {
                    if (!string.IsNullOrEmpty(title))
                    {
                        SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
                        int titleX = centerOnPage ? (marginBounds.Width - (int)titleSize.Width) / 2 : marginBounds.Left;
                        e.Graphics.DrawString(title, titleFont, new SolidBrush(titleColor), new PointF(titleX, marginBounds.Top - titleSize.Height - 10));
                    }

                    // Adjust the Description column's width to fill remaining space
                    int descriptionColumnIndex = dgv.Columns["Description"] != null ? dgv.Columns["Description"].Index : -1;
                    if (descriptionColumnIndex != -1)
                    {
                        int fixedWidth = 0;
                        for (int i = 0; i < columnWidths.Count; i++)
                        {
                            if (i != descriptionColumnIndex)
                                fixedWidth += columnWidths[i];
                        }

                        int remainingWidth = marginBounds.Width - fixedWidth;
                        if (remainingWidth > 0)
                        {
                            columnWidths[descriptionColumnIndex] = remainingWidth;
                        }
                    }

                    // Print column headers
                    PrintColumnHeaders(e.Graphics, marginBounds);
                    isFirstPage = false;
                    isNewPage = false;
                }
                else if (isNewPage)
                {
                    // Print column headers on new pages
                    PrintColumnHeaders(e.Graphics, marginBounds);
                    isNewPage = false;
                }

                // Initialize yPos if it's the first row to print
                if (yPos == 0)
                {
                    yPos = marginBounds.Top;
                }

                // Print rows
                while (rowPos < dgv.Rows.Count)
                {
                    DataGridViewRow row = dgv.Rows[rowPos];
                    if (row.IsNewRow) { rowPos++; continue; }

                    int xPosRow = marginBounds.Left;
                    int yPosRow = yPos;
                    int rowHeight = row.Height;

                    // Check if the row fits on the page
                    if (yPosRow + rowHeight > marginBounds.Bottom)
                    {
                        isNewPage = true;
                        e.HasMorePages = true;
                        return;
                    }

                    // Print each cell in the row
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        Rectangle cellBounds = new Rectangle(xPosRow, yPosRow, columnWidths[row.Cells.IndexOf(cell)], rowHeight);
                        e.Graphics.DrawRectangle(Pens.Black, cellBounds);
                        string cellValue = cell.Value?.ToString() ?? "";
                        e.Graphics.DrawString(
                            cellValue,
                            cell.InheritedStyle.Font,
                            new SolidBrush(cell.InheritedStyle.ForeColor),
                            cellBounds,
                            new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center }
                        );
                        xPosRow += columnWidths[row.Cells.IndexOf(cell)];
                    }

                    yPos += rowHeight;
                    rowPos++;
                }

                // No more pages
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during printing: " + ex.Message);
            }
        }

        /// <summary>
        /// Prints the column headers at the top of each page.
        /// </summary>
        private void PrintColumnHeaders(Graphics g, Rectangle marginBounds)
        {
            yPos = marginBounds.Top;
            int xPos = marginBounds.Left;
            int cellHeight = 0;

            foreach (DataGridViewColumn dgvCol in dgv.Columns)
            {
                Rectangle cellBounds = new Rectangle(xPos, yPos, columnWidths[dgv.Columns.IndexOf(dgvCol)], dgvCol.HeaderCell.Size.Height);
                g.FillRectangle(new SolidBrush(Color.LightGray), cellBounds);
                g.DrawRectangle(Pens.Black, cellBounds);
                g.DrawString(
                    dgvCol.HeaderText,
                    dgvCol.InheritedStyle.Font,
                    new SolidBrush(dgvCol.InheritedStyle.ForeColor),
                    cellBounds,
                    new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                );
                xPos += columnWidths[dgv.Columns.IndexOf(dgvCol)];
                cellHeight = cellBounds.Height;
            }

            yPos += cellHeight;
        }
    }
}
