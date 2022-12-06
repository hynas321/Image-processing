using ScottPlot;
using System.Drawing;

namespace Image_processing.Managers
{
    public static class ProcessingManager
    {
        #region Task 1
        public static Bitmap ManageBrightnessModification(this Bitmap bitmap, int value)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialPixel = bitmap.GetPixel(x, y);

                    int red = TruncateColorValue(initialPixel.R + value);
                    int green = TruncateColorValue(initialPixel.G + value);
                    int blue = TruncateColorValue(initialPixel.B + value);

                    Color changedPixel = Color.FromArgb(red, green, blue);

                    bitmap.SetPixel(x, y, changedPixel);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageContrastModification(this Bitmap bitmap, double alphaValue)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    double red = 255 * Math.Pow((double)pixel.R / 255, alphaValue);
                    double blue = 255 * Math.Pow((double)pixel.B / 255, alphaValue);
                    double green = 255 * Math.Pow((double)pixel.G / 255, alphaValue);
                        
                    Color changedPixel = Color.FromArgb(
                        TruncateColorValue((int)red),
                        TruncateColorValue((int)green),
                        TruncateColorValue((int)blue)
                    );

                    bitmap.SetPixel(x, y, changedPixel);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageNegative(this Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    int red = 255 - initialColor.R;
                    int green = 255 - initialColor.G;
                    int blue = 255 - initialColor.B;

                    Color reversedColor = Color.FromArgb(red, green, blue);

                    bitmap.SetPixel(x, y, reversedColor);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageHorizontalFlip(this Bitmap bitmap)
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width / 2; x++)
                {
                    Color pixel1 = bitmap.GetPixel(bitmap.Width - 1 - x, y);
                    Color pixel2 = bitmap.GetPixel(x, y);

                    bitmap.SetPixel(bitmap.Width - 1 - x, y, pixel2);
                    bitmap.SetPixel(x, y, pixel1);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageVerticalFlip(this Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height / 2; y++)
                {
                    Color pixel1 = bitmap.GetPixel(x, bitmap.Height - 1 - y);
                    Color pixel2 = bitmap.GetPixel(x, y);

                    bitmap.SetPixel(x, bitmap.Height - 1 - y, pixel2);
                    bitmap.SetPixel(x, y, pixel1);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageDiagonalFlip(this Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height / 2; y++)
                {
                    Color pixel1 = bitmap.GetPixel(bitmap.Width - 1 - x, bitmap.Height - 1 - y);
                    Color pixel2 = bitmap.GetPixel(x, y);

                    bitmap.SetPixel(bitmap.Width - 1 - x, bitmap.Height - 1 - y, pixel2);
                    bitmap.SetPixel(x, y, pixel1);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageImageShrinking(this Bitmap bitmap, int value)
        {
            int shrunkWidth = bitmap.Width / value;
            int shrunkHeight = bitmap.Height / value;

            Bitmap enlargedBitmap = new Bitmap(shrunkWidth, shrunkHeight);

            for (int x = 0; x < shrunkWidth; x++)
            {
                for (int y = 0; y < shrunkHeight; y++)
                {
                    Color pixel = bitmap.GetPixel(x * value, y * value);

                    enlargedBitmap.SetPixel(x, y, pixel);
                }
            }

            return enlargedBitmap;
        }

        public static Bitmap ManageImageEnlargement(this Bitmap bitmap, int value)
        {
            int enlargedWidth = bitmap.Width * value;
            int enlargedHeight = bitmap.Height * value;

            Bitmap enlargedBitmap = new Bitmap(enlargedWidth, enlargedHeight);

            for (int x = 0; x < enlargedWidth; x++)
            {
                for (int y = 0; y < enlargedHeight; y++)
                {
                    Color pixel = bitmap.GetPixel(x / value, y / value);

                    enlargedBitmap.SetPixel(x, y, pixel);
                }
            }

            return enlargedBitmap;
        }

        public static Bitmap ManageMidpointFilter(this Bitmap bitmap, int scope)
        {
            Bitmap maxFilteredBitmap = ManageMaxFilter(bitmap, scope);
            Bitmap minFilteredBitmap = ManageMinFilter(bitmap, scope);
            Bitmap midpointFilteredBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color maxPixel = maxFilteredBitmap.GetPixel(x, y);
                    Color minPixel = minFilteredBitmap.GetPixel(x, y);

                    int redColor = (int)(0.5 * (maxPixel.R + minPixel.R));
                    int greenColor = (int)(0.5 * (maxPixel.G + minPixel.G));
                    int blueColor = (int)(0.5 * (maxPixel.B + minPixel.B));

                    Color midpointPixel = Color.FromArgb(redColor, greenColor, blueColor);

                    midpointFilteredBitmap.SetPixel(x, y, midpointPixel);
                }
            }

            return midpointFilteredBitmap;
        }

        public static Bitmap ManageArithmeticMeanFilter(this Bitmap bitmap, int scope)
        {
            Bitmap filteredBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int y = scope; y < bitmap.Height - scope; y++)
            {
                for (int x = scope; x < bitmap.Width - scope; x++)
                {
                    Color filteredPixel = GetArithmeticMeanPixel(bitmap, x, y, scope);

                    filteredBitmap.SetPixel(x, y, filteredPixel);
                }
            }

            return filteredBitmap;
        }

        public static double CalculateMeanSquareError(Bitmap bitmap1, Bitmap bitmap2)
        {
            double meanSquareErrorResult = 0;

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap2.Height; y++)
                {
                    Color pixel1 = bitmap1.GetPixel(x, y);
                    Color pixel2 = bitmap2.GetPixel(x, y);

                    double redColorDifference = pixel1.R - pixel2.R;
                    double greenColorDifference = pixel1.G - pixel2.G;
                    double blueColorDifference = pixel1.B - pixel2.B;

                    meanSquareErrorResult +=
                        (Math.Pow(redColorDifference, 2) +
                        Math.Pow(greenColorDifference, 2) +
                        Math.Pow(blueColorDifference, 2)) / 3;
                }
            }

            return meanSquareErrorResult / (bitmap1.Width * bitmap2.Height);
        }

        public static double CalculatePeakMeanSquareError(Bitmap bitmap1, Bitmap bitmap2)
        {
            double peakMeanSquareErrorResult = 0;
            double maxRedColorValue = 0;
            double maxGreenColorValue = 0;
            double maxBlueColorValue = 0;

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color pixel1 = bitmap1.GetPixel(j, i);
                    Color pixel2 = bitmap2.GetPixel(j, i);

                    if (pixel1.R > maxRedColorValue)
                    {
                        maxRedColorValue = pixel1.R;
                    }
                    if (pixel1.G > maxGreenColorValue)
                    {
                        maxGreenColorValue = pixel1.G;
                    }
                    if (pixel1.B > maxBlueColorValue)
                    {
                        maxBlueColorValue = pixel1.B;
                    }

                    double redColorDifference = pixel1.R - pixel2.R;
                    double greenColorDifference = pixel1.G - pixel2.G;
                    double blueColorDifference = pixel1.B - pixel2.B;

                    peakMeanSquareErrorResult +=
                        (Math.Pow(redColorDifference, 2) +
                        Math.Pow(greenColorDifference, 2) +
                        Math.Pow(blueColorDifference, 2)) / 3;
                }
            }

            return peakMeanSquareErrorResult /
                (bitmap1.Width * bitmap2.Height) /
                Math.Pow((maxRedColorValue + maxGreenColorValue + maxBlueColorValue) / 3, 2);
        }

        public static double CalculateSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
        {
            double redSignal = 0;
            double greenSignal = 0;
            double blueSignal = 0;

            double redNoise = 0;
            double greenNoise = 0;
            double blueNoise = 0;

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    Color pixel1 = bitmap1.GetPixel(x, y);
                    Color pixel2 = bitmap2.GetPixel(x, y);

                    redSignal += Math.Pow(pixel2.R, 2);
                    greenSignal += Math.Pow(pixel2.G, 2);
                    blueSignal += Math.Pow(pixel2.B, 2);

                    redNoise += Math.Pow(pixel2.R - pixel1.R, 2);
                    greenNoise += Math.Pow(pixel2.G - pixel1.G, 2);
                    blueNoise += Math.Pow(pixel2.B - pixel1.B, 2);
                }
            }
            return (10 * Math.Log10(redSignal / redNoise) +
                10 * Math.Log10(greenSignal / greenNoise) +
                10 * Math.Log10(blueSignal / blueNoise)) / 3;
        }

        public static double CalculatePeakSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
        {
            return 10 * Math.Log10(Math.Pow(255, 2) / CalculateSignalToNoiseRatio(bitmap1, bitmap2));
        }

        public static double CalculateMaximumDifference(Bitmap bitmap1, Bitmap bitmap2)
        {
            double maximumDifference = 0;

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    Color pixel1 = bitmap1.GetPixel(x, y);
                    Color pixel2 = bitmap2.GetPixel(x, y);

                    double redColorDifference = Math.Abs(pixel1.R - pixel2.R);
                    double greenColorDifference = Math.Abs(pixel1.G - pixel2.G);
                    double blueColorDifference = Math.Abs(pixel1.B - pixel2.B);

                   if (redColorDifference + greenColorDifference + blueColorDifference 
                        > maximumDifference * 3)
                   {
                        maximumDifference = 
                            (redColorDifference +
                            greenColorDifference +
                            blueColorDifference) / 3;
                   }
                }
            }

            return maximumDifference;
        }
        #endregion

        #region Task 1 private methods
        private static int TruncateColorValue(int colorValue)
        {
            if (colorValue > 255)
            {
                return 255;
            }
            else if (colorValue < 0)
            {
                return 0;
            }

            return colorValue;
        }

        private static int ToRgb(this Color color)
        {
            return color.R + color.G + color.B;
        }

        private static Bitmap ManageMaxFilter(this Bitmap bitmap, int scope)
        {
            Bitmap filteredBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = scope; x < bitmap.Width - scope; x++)
            {
                for (int y = scope; y < bitmap.Height - scope; y++)
                {
                    Color filteredPixel = FilterPixelMaximum(bitmap, x, y, scope);

                    filteredBitmap.SetPixel(x, y, filteredPixel);
                }
            }

            return filteredBitmap;
        }

        private static Bitmap ManageMinFilter(this Bitmap bitmap, int scope)
        {
            Bitmap filteredBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = scope; x < bitmap.Width - scope; x++)
            {
                for (int y = scope; y < bitmap.Height - scope; y++)
                {
                    Color filteredPixel = FilterPixelMinimum(bitmap, x, y, scope);

                    filteredBitmap.SetPixel(x, y, filteredPixel);
                }
            }

            return filteredBitmap;
        }

        private static Color FilterPixelMinimum(this Bitmap bitmap, int x, int y, int scope)
        {
            Color pixelMin = bitmap.GetPixel(x, y);

            for (int a = x - scope; a <= x + scope; a++)
            {
                for (int b = y - scope; b <= y + scope; b++)
                {
                    Color pixel = bitmap.GetPixel(a, b);

                    int pixelRGB = pixel.ToRgb();
                    int pixelMinRGB = pixelMin.ToRgb();

                    if (pixelRGB < pixelMinRGB)
                    {
                        pixelMin = pixel;
                    }
                }
            }

            return pixelMin;
        }

        private static Color FilterPixelMaximum(this Bitmap bitmap, int x, int y, int scope)
        {
            Color pixelMax = bitmap.GetPixel(x, y);

            for (int a = x - scope; a <= x + scope; a++)
            {
                for (int b = y - scope; b <= y + scope; b++)
                {
                    Color pixel = bitmap.GetPixel(a, b);

                    int pixelRGB = pixel.ToRgb();
                    int pixelMaxRGB = pixelMax.ToRgb();

                    if (pixelRGB > pixelMaxRGB)
                    {
                        pixelMax = pixel;
                    }
                }
            }

            return pixelMax;
        }
        private static Color GetArithmeticMeanPixel(Bitmap bitmap, int x, int y, int scope)
        {
            int redValueSum = 0;
            int greenValueSum = 0;
            int blueValueSum = 0;

            int iteration = 0;

            for (int a = y - scope; a < y + scope; a++)
            {
                for (int b = x - scope; b < x + scope; b++)
                {
                    Color pixel = bitmap.GetPixel(b, a);

                    redValueSum += pixel.R;
                    greenValueSum += pixel.G;
                    blueValueSum += pixel.B;

                    iteration++;
                }
            }

            return Color.FromArgb(
                redValueSum / iteration,
                greenValueSum / iteration,
                blueValueSum / iteration
            );
        }
        #endregion

        #region Task 2

        public static Plot CreateHistogramImage(this Bitmap bitmap, char channel)
        {
            int[] colorValues = GetHistogramChannelValues(bitmap, channel);

            return PlotManager.CreatePlot(
                Array.ConvertAll<int, double>(colorValues, x => x), channel
            );
        }

        #region H (histogram calculation algorithm)
        public static Bitmap ManageRaleigh(this Bitmap bitmap, double alpha, int minBrightness)
        {
            int[] redColorHistogramValues = GetHistogramChannelValues(bitmap, 'R');
            int[] greenColorHistogramValues = GetHistogramChannelValues(bitmap, 'G');
            int[] blueColorHistogramValues = GetHistogramChannelValues(bitmap, 'B');

            int[] redColorNewBrightness = new int[256];
            int[] greenColorNewBrightness = new int[256];
            int[] blueColorNewBrightness = new int[256];

            for (int i = 0; i < 256; i++)
            {
                redColorNewBrightness[i] = bitmap.CalculateMinInputBrightness(i, alpha, redColorHistogramValues, minBrightness);
                greenColorNewBrightness[i] = bitmap.CalculateMinInputBrightness(i, alpha, greenColorHistogramValues, minBrightness);
                blueColorNewBrightness[i] = bitmap.CalculateMinInputBrightness(i, alpha, blueColorHistogramValues, minBrightness);
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
        public static double CalculateMean(Bitmap bitmap, char channel)
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

        public static double CalculateVariance(Bitmap bitmap, char channel)
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

        public static double CalculateStandardDeviation(Bitmap bitmap, char channel)
        {
            return Math.Pow(CalculateVariance(bitmap, channel), 0.5);
        }

        public static double CalculateVariationCoefficientI(Bitmap bitmap, char channel)
        {
            double val = CalculateMean(bitmap, channel);
            double deviation = CalculateStandardDeviation(bitmap, channel);

            return deviation / val;
        }

        public static double CalculateAsymmetryCoefficient(Bitmap bitmap, char channel)
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

        public static double CalculateFlatteningCoefficient(Bitmap bitmap, char channel)
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

        public static double CalculateVariationCoefficientII(Bitmap bitmap, char channel)
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

        //Returns NaN
        public static double CalculateInformationSourceEntropy(Bitmap bitmap, char channel)
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
        public static Bitmap ManageExtractionOfDetailsI(this Bitmap bitmap, int mask)
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

        public static Bitmap ManageExtractionOfDetailsIOptimized(this Bitmap bitmap)
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

        #region O
        public static Bitmap ManageSobelOperator(this Bitmap bitmap)
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
        private static int[] GetHistogramChannelValues(Bitmap bitmap, char channel)
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

        private static int CalculateMinInputBrightness(this Bitmap bitmap, int f, double alpha, int[] histogramValues, int minBrightness)
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

        private static int[,] GetConvolutionMask(int mask)
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

        private static Color GetPixelAfterMasking(Bitmap bitmap, int x, int y, int[,] mask)
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

        #endregion
    }
}
