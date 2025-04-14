using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.WinForms;
using BG_Menu.Class.Sales_Summary; // for StoreSales

namespace BG_Menu.Forms.Sales_Summary
{
    public partial class SalesChartForm : Form
    {
        private CartesianChart cartesianChart;
        private string storeName;
        private List<int> selectedWeeks;

        // Sales data for different datasets.
        private List<StoreSales> currentSales; // Current (SAP HANA) data
        private List<StoreSales> sales2023;
        private List<StoreSales> sales2022;
        private List<StoreSales> sales2021;
        private List<StoreSales> sales2020;

        public SalesChartForm(
            string storeName,
            List<int> selectedWeeks,
            List<StoreSales> currentSales,
            List<StoreSales> sales2023,
            List<StoreSales> sales2022,
            List<StoreSales> sales2021,
            List<StoreSales> sales2020)
        {
            InitializeComponent(); // If you're using the WinForms Designer, otherwise remove
            this.storeName = storeName;
            this.selectedWeeks = selectedWeeks;
            this.currentSales = currentSales;
            this.sales2023 = sales2023;
            this.sales2022 = sales2022;
            this.sales2021 = sales2021;
            this.sales2020 = sales2020;

            // Create the LiveCharts2 CartesianChart and dock it to fill the form.
            cartesianChart = new CartesianChart
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(cartesianChart);

            this.Text = $"Weekly Sales for {storeName}";
            this.Width = 800;
            this.Height = 600;

            BuildChart();
        }

        private void BuildChart()
        {
            // For each dataset, convert the data into arrays of double 
            // that LiveCharts2 can handle as an IReadOnlyList<double>.
            var currentValues = GetWeekValues(currentSales);
            var values2023 = GetWeekValues(sales2023);
            var values2022 = GetWeekValues(sales2022);
            var values2021 = GetWeekValues(sales2021);
            var values2020 = GetWeekValues(sales2020);

            // Create column series for each dataset
            // NOTE: Convert List<double> to .ToArray() so it matches 
            // the required type (IReadOnlyList<double>).
            var seriesCurrent = new ColumnSeries<double>
            {
                Name = "Current",
                Values = currentValues.ToArray()
            };
            var series2023 = new ColumnSeries<double>
            {
                Name = "2023",
                Values = values2023.ToArray()
            };
            var series2022 = new ColumnSeries<double>
            {
                Name = "2022",
                Values = values2022.ToArray()
            };
            var series2021 = new ColumnSeries<double>
            {
                Name = "2021",
                Values = values2021.ToArray()
            };
            var series2020 = new ColumnSeries<double>
            {
                Name = "2020",
                Values = values2020.ToArray()
            };

            // Assign the chart's series collection
            cartesianChart.Series = new ISeries[]
            {
                seriesCurrent,
                series2023,
                series2022,
                series2021,
                series2020
            };

            // Configure X-axis to show the week labels
            var weekLabels = selectedWeeks
                .OrderBy(w => w)
                .Select(w => $"Week {w}")
                .ToArray();

            cartesianChart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = weekLabels
                }
            };

            // Simple numeric Y-axis
            cartesianChart.YAxes = new Axis[]
            {
                new Axis
                {
                    Labeler = value => value.ToString("N0")
                }
            };

            // Show the legend at the top
            cartesianChart.LegendPosition = LegendPosition.Top;
        }

        private IList<double> GetValuesForDataset(List<StoreSales> salesData, string storeName, List<int> selectedWeeks)
        {
            var values = new List<double>();

            // For each selected week (ordered), sum up sales for that store.
            foreach (var week in selectedWeeks.OrderBy(w => w))
            {
                double sum = salesData
                    .Where(s => s.Store.Equals(storeName, StringComparison.OrdinalIgnoreCase) && s.Week == week)
                    .Sum(s => (double)s.Sales);
                values.Add(sum);
            }

            return values;
        }

        private List<double> GetWeekValues(List<StoreSales> salesData)
        {
            var results = new List<double>();

            // For each selected week (in ascending order),
            // sum the sales for that store/week, and add it to the results list.
            foreach (int w in selectedWeeks.OrderBy(x => x))
            {
                double sum = salesData
                    .Where(s => s.Store.Equals(storeName, StringComparison.OrdinalIgnoreCase)
                                && s.Week == w)
                    .Sum(s => (double)s.Sales);
                results.Add(sum);
            }

            return results;
        }
    }
}
