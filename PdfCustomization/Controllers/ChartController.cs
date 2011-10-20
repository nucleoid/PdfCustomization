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

            var font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
            var chart = new Chart
                            {
                                Palette = ChartColorPalette.BrightPastel, 
                                BackColor = Color.DarkOrange, 
                                ImageType = ChartImageType.Png,
                                Width = Unit.Pixel(824),
                                Height = Unit.Pixel(592),
                                BorderlineDashStyle = ChartDashStyle.Solid,
                                BackGradientStyle = GradientStyle.TopBottom,
                                BorderWidth = 2,
                                BorderColor = Color.FromArgb(181, 64, 1),
                            };
            chart.Legends.Add("Legend1");
            chart.Legends[0].IsTextAutoFit = false;
            chart.Legends[0].BackColor = Color.Transparent;
            chart.Legends[0].Font = font;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            var ca1 = new ChartArea("ca1")
                          {
                              BorderColor = Color.FromArgb(64),
                              BorderDashStyle = ChartDashStyle.Solid,
                              BackSecondaryColor = Color.White,
                              BackColor = Color.OldLace,
                              ShadowColor = Color.Transparent,
                              BackGradientStyle = GradientStyle.TopBottom,
                              AxisX =
                                  {
                                      Title = "Entry", 
                                      LineColor = Color.FromArgb(64),
                                      LabelStyle = new LabelStyle {Font = font},
                                      MajorGrid = new Grid {LineColor = Color.FromArgb(64), Enabled = true}
                                  },
                              AxisY =
                                  {
                                      Title = "Price",
                                      LineColor = Color.FromArgb(64),
                                      LabelStyle = new LabelStyle { Font = font },
                                      MajorGrid = new Grid { LineColor = Color.FromArgb(64), Enabled = true}
                                  }, 
                          };
            chart.ChartAreas.Add(ca1);

            var series1 = new Series("Unit Price")
                              {
                                  MarkerSize = 8,
                                  BorderWidth = 3,
                                  XValueType = ChartValueType.Double,
                                  ChartType = SeriesChartType.Line, 
                                  MarkerStyle = MarkerStyle.Circle,
                                  ShadowColor = Color.Black,
                                  BorderColor = Color.Black,
                                  Color = Color.DarkTurquoise,
                                  ShadowOffset = 2,
                                  YValueType = ChartValueType.Double,
                                  ChartArea = "ca1", 
                                  IsValueShownAsLabel = false, 
                                  Font = font
                              };
            var series2 = new Series("Total Price")
                              {
                                  MarkerSize = 9,
                                  BorderWidth = 3,
                                  XValueType = ChartValueType.Double,
                                  ChartType = SeriesChartType.Line,
                                  MarkerStyle = MarkerStyle.Diamond,
                                  ShadowColor = Color.Black,
                                  BorderColor = Color.Black,
                                  Color = Color.Red,
                                  ShadowOffset = 2,
                                  YValueType = ChartValueType.Double,
                                  ChartArea = "ca1",
                                  IsValueShownAsLabel = false,
                                  Font = font
                              };
            foreach (var unitPrice in prices.Select(x => Convert.ToDouble(x.UnitPrice)))
                series1.Points.AddY(unitPrice);
            foreach (var totalPrice in prices.Select(x => Convert.ToDouble(x.TotalPrice)))
                series2.Points.AddY(totalPrice);
            chart.Series.Add(series1);
            chart.Series.Add(series2);
            
            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                return File(ms.ToArray(), "image/png", "mychart.png");
            }
        }
    }
}
