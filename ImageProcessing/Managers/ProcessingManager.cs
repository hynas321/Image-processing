using ScottPlot;
using ScottPlot.Drawing;
using ScottPlot.Plottable;
using System.Drawing;
using System.Drawing.Text;

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

        public static Bitmap ManageContrastModification(this Bitmap bitmap, int value)
        {
            double contrast = Math.Pow((100.0 + value) / 100.0, 2);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialPixel = bitmap.GetPixel(x, y);

                    double red = ((((initialPixel.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    double green = ((((initialPixel.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    double blue = ((((initialPixel.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;

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

                    double redColorResult = Math.Pow(pixel1.R - pixel2.R, 2);
                    double greenColorResult = Math.Pow(pixel1.G - pixel2.G, 2);
                    double blueColorResult = Math.Pow(pixel1.B - pixel2.B, 2);

                    meanSquareErrorResult += redColorResult + greenColorResult + blueColorResult;
                }
            }
            
            return meanSquareErrorResult / (bitmap1.Width * bitmap2.Height);
        }

        public static double CalculatePeakMeanSquareError(Bitmap bitmap1, Bitmap bitmap2)
        {
            double value = 0;
            double maxValue = 0;

            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    Color pixel1 = bitmap1.GetPixel(j, i);
                    Color pixel2 = bitmap2.GetPixel(j, i);

                    double redColorDifference = pixel1.R - pixel2.R;
                    double greenColorDifference = pixel1.G - pixel2.G;
                    double blueColorDifference = pixel1.B - pixel2.B;

                    value += Math.Pow(redColorDifference, 2) +
                        Math.Pow(greenColorDifference, 2) +
                        Math.Pow(blueColorDifference, 2);

                    if (value > maxValue)
                    {
                        maxValue = value;
                    }
                }
            }

            return value / (bitmap1.Height * bitmap2.Width * maxValue);
        }

        public static double CalculateSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
        {
            double signal = 0;
            double noise = 0;

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    Color pixel1 = bitmap1.GetPixel(x, y);
                    Color pixel2 = bitmap2.GetPixel(x, y);

                    signal += 
                        Math.Pow(pixel1.R, 2) +
                        Math.Pow(pixel1.G, 2) +
                        Math.Pow(pixel1.B, 2);

                    noise +=
                        Math.Pow(pixel1.R - pixel2.R, 2) +
                        Math.Pow(pixel1.G - pixel2.G, 2) +
                        Math.Pow(pixel1.B - pixel2.B, 2);
                }
            }
            return 10 * Math.Log10(signal / noise);
        }

        public static double CalculatePeakSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
        {
            double signal = 0;
            double noise = 0;

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    Color pixel1 = bitmap1.GetPixel(x, y);
                    Color pixel2 = bitmap2.GetPixel(x, y);

                    if (pixel1.ToRgb() > signal)
                    {
                        signal = pixel1.ToRgb();
                    }

                    noise +=
                        Math.Pow(pixel1.R - pixel2.R, 2) +
                        Math.Pow(pixel1.G - pixel2.G, 2) +
                        Math.Pow(pixel1.B - pixel2.B, 2);
                }
            }

            return 10 * Math.Log10(Math.Pow(signal,2) / noise);
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

                    double rgbDifference =
                        redColorDifference + greenColorDifference + blueColorDifference;

                    if (rgbDifference > maximumDifference)
                    {
                        maximumDifference = rgbDifference;
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

        public static Bitmap ManageMaxFilter(this Bitmap bitmap, int scope)
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

        public static Bitmap ManageMinFilter(this Bitmap bitmap, int scope)
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
        public static Plot ManageHistogram(this Bitmap bitmap, char color)
        {
            double[] colorValues = new double[256];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    switch (color)
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

            return PlotManager.CreatePlot(colorValues, color);
        }
        #endregion

        #region Task 2 private methods

        #endregion
    }
}
