using ScottPlot;
using System.Drawing;

namespace Image_processing.Managers
{
    //Task 2
    public partial class ProcessingManager
    {
        public Plot CreateHistogramImage(Bitmap bitmap, char channel)
        {
            int[] colorValues = GetHistogramChannelValues(bitmap, channel);

            return PlotManager.CreatePlot(
                Array.ConvertAll<int, double>(colorValues, x => x), channel
            );
        }

        #region H (histogram calculation algorithm)
        public Bitmap ManageRaleigh(Bitmap bitmap, double alpha, int minBrightness)
        {
            int[] redColorHistogramValues = GetHistogramChannelValues(bitmap, 'R');
            int[] greenColorHistogramValues = GetHistogramChannelValues(bitmap, 'G');
            int[] blueColorHistogramValues = GetHistogramChannelValues(bitmap, 'B');

            int[] redColorNewBrightness = new int[256];
            int[] greenColorNewBrightness = new int[256];
            int[] blueColorNewBrightness = new int[256];

            for (int i = 0; i < 256; i++)
            {
                redColorNewBrightness[i] = CalculateMinInputBrightness(bitmap, i, alpha, redColorHistogramValues, minBrightness);
                greenColorNewBrightness[i] = CalculateMinInputBrightness(bitmap, i, alpha, greenColorHistogramValues, minBrightness);
                blueColorNewBrightness[i] = CalculateMinInputBrightness(bitmap, i, alpha, blueColorHistogramValues, minBrightness);
            }

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    Color newPixel = Color.FromArgb(
                        redColorNewBrightness[pixel.R],
                        greenColorNewBrightness[pixel.G],
                        blueColorNewBrightness[pixel.B]
                    );

                    bitmap.SetPixel(x, y, newPixel);
                }
            }

            return ManageNegative(bitmap);
        }
        #endregion

        #region C (image characteristics)
        public double CalculateMean(Bitmap bitmap, char channel)
        {
            double val = 0;
            double N = bitmap.Width * bitmap.Height;
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                val += m * histogramChannelValues[m];
            }

            return 1 / N * val;
        }

        public double CalculateVariance(Bitmap bitmap, char channel)
        {
            double val = 0;
            double N = bitmap.Width * bitmap.Height;
            double mean = CalculateMean(bitmap, channel);
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                val += Math.Pow(m - mean, 2) * histogramChannelValues[m];
            }

            return 1 / N * val;
        }

        public double CalculateStandardDeviation(Bitmap bitmap, char channel)
        {
            return Math.Pow(CalculateVariance(bitmap, channel), 0.5);
        }

        public double CalculateVariationCoefficientI(Bitmap bitmap, char channel)
        {
            double val = CalculateMean(bitmap, channel);
            double deviation = CalculateStandardDeviation(bitmap, channel);

            return deviation / val;
        }

        public double CalculateAsymmetryCoefficient(Bitmap bitmap, char channel)
        {
            double val = 0;
            double N = bitmap.Width * bitmap.Height;
            double mean = CalculateMean(bitmap, channel);
            double standardDeviation = CalculateStandardDeviation(bitmap, channel);
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                val += Math.Pow(m - mean, 3) * histogramChannelValues[m];
            }

            return 1 / Math.Pow(standardDeviation, 3) *
                1 / N * val;
        }

        public double CalculateFlatteningCoefficient(Bitmap bitmap, char channel)
        {
            double val = 0;
            double N = bitmap.Width * bitmap.Height;
            double b = CalculateMean(bitmap, channel);
            double σ = CalculateStandardDeviation(bitmap, channel);
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                val += Math.Pow(m - b, 4) * histogramChannelValues[m];
            }

            return 1 / Math.Pow(σ, 4) * (1 / N * val) - 3;
        }

        public double CalculateVariationCoefficientII(Bitmap bitmap, char channel)
        {
            double val = 0;
            double N = bitmap.Width * bitmap.Height;
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                val += Math.Pow(histogramChannelValues[m], 2);
            }

            return Math.Pow(1 / N, 2) * val;
        }

        public double CalculateInformationSourceEntropy(Bitmap bitmap, char channel)
        {
            double val = 0;
            int N = bitmap.Width * bitmap.Height;
            int[] histogramChannelValues = GetHistogramChannelValues(bitmap, channel);

            for (int m = 0; m < 256; m++)
            {
                if (histogramChannelValues[m] > 0)
                {
                    val += histogramChannelValues[m] * Math.Log2((double)histogramChannelValues[m] / N);
                }
            }

            return (-1.0 / (double)N) * val;
        }
        #endregion

        #region S (linear image filtration)
        public Bitmap ManageExtractionOfDetailsI(Bitmap bitmap, int mask)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int[,] maskArray = GetConvolutionMask(mask);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    Color newPixel = GetPixelAfterMasking(bitmap, x, y, maskArray);
                    newBitmap.SetPixel(x, y, newPixel);
                }
            }

            return newBitmap;
        }

        public Bitmap ManageExtractionOfDetailsIOptimized(Bitmap bitmap)
        {
            //Mask N
            //{ 1,  1,  1 },
            //{ 1, -2,  1 },
            //{ -1, -1, -1 }

            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    int redColorValue = 0;
                    int greenColorValue = 0;
                    int blueColorValue = 0;

                    redColorValue = bitmap.GetPixel(x - 1, y - 1).R + bitmap.GetPixel(x - 1, y).R + bitmap.GetPixel(x - 1, y + 1).R +
                        bitmap.GetPixel(x, y - 1).R + (-2) * bitmap.GetPixel(x, y).R + bitmap.GetPixel(x, y + 1).R +
                        (-1) * bitmap.GetPixel(x + 1, y - 1).R + (-1) * bitmap.GetPixel(x + 1, y).R + (-1) * bitmap.GetPixel(x + 1, y + 1).R;

                    greenColorValue = bitmap.GetPixel(x - 1, y - 1).G + bitmap.GetPixel(x - 1, y).G + bitmap.GetPixel(x - 1, y + 1).G +
                        bitmap.GetPixel(x, y - 1).G + (-2) * bitmap.GetPixel(x, y).G + bitmap.GetPixel(x, y + 1).G +
                        (-1) * bitmap.GetPixel(x + 1, y - 1).G + (-1) * bitmap.GetPixel(x + 1, y).G + (-1) * bitmap.GetPixel(x + 1, y + 1).G;

                    blueColorValue = bitmap.GetPixel(x - 1, y - 1).B + bitmap.GetPixel(x - 1, y).B + bitmap.GetPixel(x - 1, y + 1).B +
                        bitmap.GetPixel(x, y - 1).B + (-2) * bitmap.GetPixel(x, y).B + bitmap.GetPixel(x, y + 1).B +
                        (-1) * bitmap.GetPixel(x + 1, y - 1).B + (-1) * bitmap.GetPixel(x + 1, y).B + (-1) * bitmap.GetPixel(x + 1, y + 1).B;

                    Color newPixel = Color.FromArgb(
                        TruncateColorValue(redColorValue),
                        TruncateColorValue(greenColorValue),
                        TruncateColorValue(blueColorValue)
                    );

                    newBitmap.SetPixel(x, y, newPixel);
                }
            }

            return newBitmap;
        }
        #endregion

        #region O (non-linear image filtration)
        public Bitmap ManageSobelOperator(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    //A1 x, y+1
                    //A3 x+1, y
                    //A5 x,   y-1
                    //A7 x-1, y
                    double redColorValueX =
                        (bitmap.GetPixel(x + 1, y + 1).R + 2 * bitmap.GetPixel(x + 1, y).R + bitmap.GetPixel(x + 1, y - 1).R) -
                        (bitmap.GetPixel(x - 1, y + 1).R + 2 * bitmap.GetPixel(x - 1, y).R + bitmap.GetPixel(x - 1, y - 1).R);

                    double greenColorValueX =
                        (bitmap.GetPixel(x + 1, y + 1).G + 2 * bitmap.GetPixel(x + 1, y).G + bitmap.GetPixel(x + 1, y - 1).G) -
                        (bitmap.GetPixel(x - 1, y + 1).G + 2 * bitmap.GetPixel(x - 1, y).G + bitmap.GetPixel(x - 1, y - 1).G);

                    double blueColorValueX =
                        (bitmap.GetPixel(x + 1, y + 1).G + 2 * bitmap.GetPixel(x + 1, y).G + bitmap.GetPixel(x + 1, y - 1).G) -
                        (bitmap.GetPixel(x - 1, y + 1).G + 2 * bitmap.GetPixel(x - 1, y).G + bitmap.GetPixel(x - 1, y - 1).G);

                    double redColorValueY =
                        (bitmap.GetPixel(x - 1, y + 1).R + 2 * bitmap.GetPixel(x, y + 1).R + bitmap.GetPixel(x + 1, y + 1).R) -
                        (bitmap.GetPixel(x - 1, y - 1).R + 2 * bitmap.GetPixel(x, y - 1).R + bitmap.GetPixel(x + 1, y - 1).R);

                    double greenColorValueY =
                        (bitmap.GetPixel(x - 1, y + 1).G + 2 * bitmap.GetPixel(x, y + 1).G + bitmap.GetPixel(x + 1, y + 1).G) -
                        (bitmap.GetPixel(x - 1, y - 1).G + 2 * bitmap.GetPixel(x, y - 1).G + bitmap.GetPixel(x + 1, y - 1).G);

                    double blueColorValueY =
                        (bitmap.GetPixel(x - 1, y + 1).B + 2 * bitmap.GetPixel(x, y + 1).B + bitmap.GetPixel(x + 1, y + 1).B) -
                        (bitmap.GetPixel(x - 1, y - 1).B + 2 * bitmap.GetPixel(x, y - 1).B + bitmap.GetPixel(x + 1, y - 1).B);

                    double redColorValue = Math.Sqrt(Math.Pow(redColorValueX, 2) + Math.Pow(redColorValueY, 2));
                    double greenColorValue = Math.Sqrt(Math.Pow(greenColorValueX, 2) + Math.Pow(greenColorValueY, 2));
                    double blueColorValue = Math.Sqrt(Math.Pow(blueColorValueX, 2) + Math.Pow(blueColorValueY, 2));

                    Color newPixel = Color.FromArgb(
                        TruncateColorValue((int)redColorValue),
                        TruncateColorValue((int)greenColorValue),
                        TruncateColorValue((int)blueColorValue)
                    );

                    newBitmap.SetPixel(x, y, newPixel);
                }
            }

            return newBitmap;
        }
        #endregion 

        #region Task 2 private methods
        private int[] GetHistogramChannelValues(Bitmap bitmap, char channel)
        {
            int[] colorValues = new int[256];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    switch (channel)
                    {
                        case 'R':
                            colorValues[pixel.R] += 1;
                            break;
                        case 'G':
                            colorValues[pixel.G] += 1;
                            break;
                        case 'B':
                            colorValues[pixel.B] += 1;
                            break;
                        default:
                            break;
                    }
                }
            }

            return colorValues;
        }

        private int CalculateMinInputBrightness(Bitmap bitmap, int f, double alpha, int[] histogramValues, int minBrightness)
        {
            int histogramValuesSum = 0;
            int resolution = bitmap.Width * bitmap.Height;

            for (int N = 0; N < f; N++)
            {
                histogramValuesSum += histogramValues[N];
            }

            int value = (int)Math.Pow(
                (double)(2 * Math.Pow(alpha, 2) * Math.Log(1 / ((double)1 / (resolution) * histogramValuesSum))),
                0.5
             );

            return TruncateColorValue(minBrightness + value);
        }

        private int[,] GetConvolutionMask(int mask)
        {
            switch (mask)
            {
                //N
                case 1:
                    return new int[,]
                    {
                        { 1, 1, 1 },
                        { 1, -2, 1 },
                        { -1, -1, -1 }
                    };
                //NE
                case 2:
                    return new int[,]
                    {
                        { 1, 1, 1 },
                        { -1, -2, 1 },
                        { -1, -1, 1 }
                    };
                //E
                case 3:
                    return new int[,]
                    {
                        { -1, 1, 1 },
                        { -1, -2, 1 },
                        { -1, 1, 1 },
                    };
                //SE
                case 4:
                    return new int[,]
                    {
                        { -1, -1, 1 },
                        { -1, -2, 1 },
                        { 1, 1, 1 },
                    };
                default:
                    return new int[,]
                    {
                        { 0, 0, 0 },
                        { 0, 0, 0 },
                        { 0, 0, 0 },
                    };
            }
        }

        private Color GetPixelAfterMasking(Bitmap bitmap, int x, int y, int[,] mask)
        {
            int firstIndex = 0;
            int redColorSum = 0;
            int blueColorSum = 0;
            int greenColorSum = 0;

            for (int a = x - 1; a <= x + 1; a++)
            {
                int secondIndex = 0;
                for (int b = y - 1; b <= y + 1; b++)
                {
                    Color pixel = bitmap.GetPixel(a, b);

                    redColorSum += mask[firstIndex, secondIndex] * pixel.R;
                    blueColorSum += mask[firstIndex, secondIndex] * pixel.B;
                    greenColorSum += mask[firstIndex, secondIndex] * pixel.G;
                    secondIndex++;
                }
                firstIndex++;
            }

            return Color.FromArgb(
                TruncateColorValue(redColorSum),
                TruncateColorValue(blueColorSum),
                TruncateColorValue(greenColorSum)
            );
        }
        #endregion
    }
}
