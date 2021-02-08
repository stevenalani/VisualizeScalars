using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using VisualizeScalars.DataQuery;
using CartesianChart = LiveCharts.WinForms.CartesianChart;


namespace VisualizeScalars
{
    public partial class DetailForm : Form
    {
        private CartesianChart cartesianChart1;
        private BaseGridCell cell;

        public DetailForm(BaseGridCell cell)
        {
            this.cell = cell;
            InitializeComponent();
            initChart();
        }

        private void initChart()
        {
            cartesianChart1.Series = new SeriesCollection()
            {
                new RowSeries{
                Title = "Messwerte",
                Values = new ChartValues<float> {},
                DataLabels = true,
            }
            };
            foreach (var cellScalar in cell.Scalars)
            {
                var min = Form1.Model.DataGrid.Min(cellScalar.Key);
                var max = Form1.Model.DataGrid.Max(cellScalar.Key);
                cartesianChart1.Series[0].Values.Add((float)((cellScalar.Value - min)/(max - min)));
            }
            
            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Skalar-Name",
                Labels = cell.Scalars.Keys.ToArray() 
            });

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Normalisierter Wert",
                LabelFormatter = value => value.ToString("N")
            });


        }

        private void DetailForm_Load(object sender, EventArgs e)
        {
        }
        
    }
}
