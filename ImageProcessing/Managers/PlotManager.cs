using ScottPlot;
using ScottPlot.Plottable;
using System.Drawing;

namespace Image_processing.Managers
{
    public class PlotManager
    {
        public static Plot CreatePlot(double[] values, char color)
        {
            Plot plot = new Plot(1280, 720);

            BarPlot bar = plot.AddBar(values);

            switch (color)
            {
                case 'R':
                    bar.FillColor = Color.Red;
                    plot.XLabel("Red color value");
                    break;
                case 'G':
                    bar.FillColor = Color.Green;
                    plot.XLabel("Green color value");
                    break;
                case 'B':
                    bar.FillColor = Color.Blue;
                    plot.XLabel("Blue color value");
                    break;
                default:
                    break;
            }

            plot.SetAxisLimits(yMin: 0);
            plot.YLabel("Count");

            return plot;
        }
    }
}
