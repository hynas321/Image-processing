using System.Drawing;

namespace Image_processing.Managers
{
    public static class ProcessingManager
    {
        public static void DisplayHelpMessage()
        {
            Console.WriteLine("Help!");
        }

        #region Task 1
        public static Bitmap ManageBrightnessModification(this Bitmap bitmap, int value)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    int red = TruncateColorValue(initialColor.R + value);
                    int green = TruncateColorValue(initialColor.G + value);
                    int blue = TruncateColorValue(initialColor.B + value);

                    Color color = Color.FromArgb(red, green, blue);

                    bitmap.SetPixel(x, y, color);
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
                    Color initialColor = bitmap.GetPixel(x, y);

                    double red = ((((initialColor.R / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    double green = ((((initialColor.G / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
                    double blue = ((((initialColor.B / 255.0) - 0.5) * contrast) + 0.5) * 255.0;

                    Color color = Color.FromArgb(
                        initialColor.A, 
                        TruncateColorValue((int)red),
                        TruncateColorValue((int)green),
                        TruncateColorValue((int)blue)
                    );

                    bitmap.SetPixel(x, y, color);
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

                    Color color = Color.FromArgb(red, green, blue);
                    
                    bitmap.SetPixel(x, y, color);
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
                    Color rightSidePixel = bitmap.GetPixel(bitmap.Width - 1 - x, y); 
                    Color leftSidePixel = bitmap.GetPixel(x, y); 
                                                                                       
                    bitmap.SetPixel(bitmap.Width - 1 - x, y, leftSidePixel);
                    bitmap.SetPixel(x, y, rightSidePixel);
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
                    Color rightSidePixel = bitmap.GetPixel(x, bitmap.Height - 1 - y);
                    Color leftSidePixel = bitmap.GetPixel(x, y);

                    bitmap.SetPixel(x, bitmap.Height - 1 - y, leftSidePixel);
                    bitmap.SetPixel(x, y, rightSidePixel);
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
                    Color rightSidePixel = bitmap.GetPixel(bitmap.Width - 1 - x, bitmap.Height - 1 - y);
                    Color leftSidePixel = bitmap.GetPixel(x, y);

                    bitmap.SetPixel(bitmap.Width - 1 - x, bitmap.Height - 1 - y, leftSidePixel);
                    bitmap.SetPixel(x, y, rightSidePixel);
                }
            }

            return bitmap;
        }

        public static Bitmap ManageImageShrinking(this Bitmap bitmap, double value)
        {
            return null;
        }

        public static Bitmap ManageImageEnlargement(this Bitmap bitmap, int value)
        {
            int newWidth = bitmap.Width * value;
            int newHeight = bitmap.Height * value;

            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    Color pixel = bitmap.GetPixel(x / value, y / value);
                    newBitmap.SetPixel(x, y, pixel);
                }
            }

            return bitmap;
        }

        public static void ManageMidpointFilter()
        {
            throw new NotImplementedException();
        }

        public static void ManageArithmeticMeanFilter()
        {
            throw new NotImplementedException();
        }

        public static void ManageMeanSquareError()
        {
            throw new NotImplementedException();
        }

        public static void ManagePeakMeanSquareError()
        {
            throw new NotImplementedException();
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

        internal static Bitmap ManageNegative()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
