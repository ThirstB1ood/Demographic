using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using Demographic.FileOperations;
using MathNet.Numerics.Distributions;
using System.Threading;

namespace Demographic.WinForms
{
    public partial class Form1 : Form
    {

        private Controller controller = new Controller();
        private Series series;
        private Series seriesFemale;
        private Series seriesMale;

        public Form1()
        {
            InitializeComponent();
            controller.YearTick += AddPoints;
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            series = chart1.Series.Add("Total");
            seriesFemale = chart1.Series.Add("Female");
            seriesMale = chart1.Series.Add("Male");
            chart2.Titles.Add("Men");
            chart3.Titles.Add("Women");
        }

        private void AddPoints(List<Person> population, int currentTime)
        {
            series.Points.AddXY(currentTime, population.Count(p => p.Status == Alive.alive));
            seriesFemale.Points.AddXY(currentTime, population.Count(p => p is Female && p.Status == Alive.alive));
            seriesMale.Points.AddXY(currentTime, population.Count(p => p is Male && p.Status == Alive.alive));
            series.ChartType = SeriesChartType.Spline;
            seriesFemale.ChartType = SeriesChartType.Spline;
            seriesMale.ChartType = SeriesChartType.Spline;

            Axis ax = new Axis
            {
                Title = "Year",
            };
            chart1.ChartAreas[0].AxisX = ax;

            Axis ay = new Axis();
            ay.Title = "Population  (thousands)";
            chart1.ChartAreas[0].AxisY = ay;
        }

        private void OutputError(string error)
        {
            chart1.Series.Clear();
            errors.Text = error;
        }

        private void FileSelect(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv" };

            if (fileDialog.ShowDialog() != DialogResult.Cancel && File.Exists(fileDialog.FileName))
            {
                FileName.Text = Path.GetFileName(fileDialog.FileName);
                controller.LoadData(fileDialog.FileName);
            }
            else
            {
                OutputError("No file selected");
            }
        }

        private void Draw(object sender, EventArgs e)
        {
            try
            {
                controller.SetValues(textBox1.Text, textBox2.Text, textBox3.Text);
                controller.DataToDouble();
                DrawBar(controller.GetMale(), controller.GetFemale());
            }
            catch (Exception exception)
            {
                OutputError(exception.Message);
            }
        }

        private void DrawBar(int[] countMale, int[] countFemale)
        {
            chart2.Series.Clear();
            chart3.Series.Clear();


            Axis ax = new Axis
            {
                Title = "",
            };
            ax.IsMarginVisible = false;
            chart2.ChartAreas[0].AxisX = ax;
            chart3.ChartAreas[0].AxisX = ax;


            Axis ay = new Axis
            {
                Title = "Population  (thousands)"
                
            };
            
            chart2.ChartAreas[0].AxisY = ay;
            chart3.ChartAreas[0].AxisY = ay;


            chart2.Series.Clear();
            string[] ages = { "0 - 18", "19 - 45", "46 - 65", "65 - 100" };


            for (int i = 0; i < countMale.Length; i++)
            {
                Series series = chart2.Series.Add(ages[i]);
                series.Points.Add(countMale[i]);
            }



            chart3.Series.Clear();


            for (int i = 0; i < countFemale.Length; i++)
            {
                Series series = chart3.Series.Add(ages[i]);
                series.Points.Add(countFemale[i]);
            }
        }
    }
}
