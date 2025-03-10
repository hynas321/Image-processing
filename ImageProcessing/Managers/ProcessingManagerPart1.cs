using System.Drawing;

namespace Image_processing.Managers
{
    //Task 1
    public partial class ProcessingManager
    {
        private const int colorRange = 256;
        private const int maxColorValue = 255;

        #region B (elementary operations)
        public Bitmap ApplyBrightnessModification(Bitmap bitmap, int value)
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

        public Bitmap ApplyContrastModification(Bitmap bitmap, double alphaValue)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    double red = maxColorValue * Math.Pow((double)pixel.R / maxColorValue, alphaValue);
                    double blue = maxColorValue * Math.Pow((double)pixel.B / maxColorValue, alphaValue);
                    double green = maxColorValue * Math.Pow((double)pixel.G / maxColorValue, alphaValue);
                        
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

        public Bitmap ApplyNegative(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    int red = maxColorValue - initialColor.R;
                    int green = maxColorValue - initialColor.G;
                    int blue = maxColorValue - initialColor.B;

                    Color reversedColor = Color.FromArgb(red, green, blue);

                    bitmap.SetPixel(x, y, reversedColor);
                }
            }

            return bitmap;
        }

        #endregion

        #region G (geometric operations)
        public Bitmap ApplyHorizontalFlip(Bitmap bitmap)
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

        public Bitmap ApplyVerticalFlip(Bitmap bitmap)
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

        public Bitmap ApplyDiagonalFlip(Bitmap bitmap)
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

        public Bitmap ApplyImageShrinking(Bitmap bitmap, int value)
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

        public Bitmap ApplyImageEnlargement(Bitmap bitmap, int value)
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

        #endregion

        #region N (noise removal)
        public Bitmap ApplyMidpointFilter(Bitmap bitmap, int scope)
        {
            Bitmap maxFilteredBitmap = ApplyMaxFilter(bitmap, scope);
            Bitmap minFilteredBitmap = ApplyMinFilter(bitmap, scope);
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

        public Bitmap ApplyArithmeticMeanFilter(Bitmap bitmap, int scope)
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
        #endregion

        #region E (analysis)
        public double CalculateMeanSquareError(Bitmap bitmap1, Bitmap bitmap2)
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

        public double CalculatePeakMeanSquareError(Bitmap bitmap1, Bitmap bitmap2)
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

        public double CalculateSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
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

        public double CalculatePeakSignalToNoiseRatio(Bitmap bitmap1, Bitmap bitmap2)
        {
            return 10 * Math.Log10(Math.Pow(maxColorValue, 2) / CalculateSignalToNoiseRatio(bitmap1, bitmap2));
        }

        public double CalculateMaximumDifference(Bitmap bitmap1, Bitmap bitmap2)
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
        private int TruncateColorValue(int colorValue)
        {
            if (colorValue > maxColorValue)
            {
                return maxColorValue;
            }
            else if (colorValue < 0)
            {
                return 0;
            }

            return colorValue;
        }

        private static int ToRgb(Color color)
        {
            return color.R + color.G + color.B;
        }

        private Bitmap ApplyMaxFilter(Bitmap bitmap, int scope)
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

        private Bitmap ApplyMinFilter(Bitmap bitmap, int scope)
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

        private Color FilterPixelMinimum(Bitmap bitmap, int x, int y, int scope)
        {
            Color pixelMin = bitmap.GetPixel(x, y);

            for (int a = x - scope; a <= x + scope; a++)
            {
                for (int b = y - scope; b <= y + scope; b++)
                {
                    Color pixel = bitmap.GetPixel(a, b);

                    int pixelRGB = ToRgb(pixel);
                    int pixelMinRGB = ToRgb(pixel);

                    if (pixelRGB < pixelMinRGB)
                    {
                        pixelMin = pixel;
                    }
                }
            }

            return pixelMin;
        }

        private Color FilterPixelMaximum(Bitmap bitmap, int x, int y, int scope)
        {
            Color pixelMax = bitmap.GetPixel(x, y);

            for (int a = x - scope; a <= x + scope; a++)
            {
                for (int b = y - scope; b <= y + scope; b++)
                {
                    Color pixel = bitmap.GetPixel(a, b);

                    int pixelRGB = ToRgb(pixel);
                    int pixelMaxRGB = ToRgb(pixelMax);

                    if (pixelRGB > pixelMaxRGB)
                    {
                        pixelMax = pixel;
                    }
                }
            }

            return pixelMax;
        }
        private Color GetArithmeticMeanPixel(Bitmap bitmap, int x, int y, int scope)
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
    }
}
