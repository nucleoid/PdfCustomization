using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using PdfCustomization.Models;

namespace PdfCustomization.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult LineChart()
        {
            var prices = new List<Price>
            {
                new Price {Mode = Mode.Magic, UnitPrice = 34.76m, TotalPrice = 278.08m},
                new Price {Mode = Mode.Magic, UnitPrice = 25.75m, TotalPrice = 231.75m},
                new Price {Mode = Mode.Offline, UnitPrice = 80m, TotalPrice = 560m},
                new Price {Mode = Mode.Offline, UnitPrice = 98.45m, TotalPrice = 590.7m},
                new Price {Mode = Mode.Online, UnitPrice = 12.45m, TotalPrice = 161.85m},
                new Price {Mode = Mode.Online, UnitPrice = 10.75m, TotalPrice = 32.25m},
                new Price {Mode = Mode.Online, UnitPrice = 57.89m, TotalPrice = 926.24m}
            };

            var chart = new Chart { BackColor = Color.Transparent, Width = Unit.Pixel(800), Height = Unit.Pixel(400)};

            var ca1 = new ChartArea("ca1") {BackColor = Color.Transparent, AxisX = {Title = "Entry"}, AxisY = {Title = "Price"}};
            chart.ChartAreas.Add(ca1);

            var font = new Font("Trebuchet MS", 8.25f);
            var series1 = new Series("Unit Price") { ChartType = SeriesChartType.Line, ChartArea = "ca1", IsValueShownAsLabel = true, Font = font };
            var series2 = new Series("Total Price") { ChartType = SeriesChartType.Line, ChartArea = "ca1", IsValueShownAsLabel = true, Font = font };
            foreach (var unitPrice in prices.Select(x => x.UnitPrice))
                series1.Points.AddY(unitPrice);
            foreach (var totalPrice in prices.Select(x => x.TotalPrice))
                series2.Points.AddY(totalPrice);
            chart.Series.Add(series1);
            chart.Series.Add(series2);
            chart.Legends.Add("Legend1");

            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms.ToArray(), "image/png", "mychart.png");
            }
        }
    }
}
