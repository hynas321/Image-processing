using Image_processing.Records;
using System.Drawing;

namespace Image_processing.Managers
{
    public static class ProcessingManager
    {
        public static void DisplayHelpMessage()
        {
            Dictionary<string, string> operationDictionary = Operations.OperationsDictionary;
            ConsoleColor defaultConsoleColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("***IMAGE PROCESSING APPLICATION***");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("In order to process an image, one should input parameters when running an application.");
            Console.WriteLine("Parameters have to be given in the correct order.");
            Console.WriteLine("The image has to be placed in the \"OriginalImages\" folder in exe file's location.");
            Console.WriteLine("The processed image is created in the \"ModifiedImages\" folder in exe file's location.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("PARAMETERS");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("filename ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("image file present in \"OriginalImages\" folder (example: lena.bmp)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("--(operation name) ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("type of image processing operation (example: --brightness)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("intValue ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("numeric value with the range specified in the command's description (example: 15)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("doubleValue ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("floating point numeric value with the range specified in the command's description (example: 15.5)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("COMMANDS");

            foreach (KeyValuePair<string, string> elem in operationDictionary)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{elem.Key} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{elem.Value}\n");
                Console.ForegroundColor = defaultConsoleColor;
            }
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
        #endregion
    }
}
