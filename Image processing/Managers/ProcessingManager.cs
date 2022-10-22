using Image_processing.Models;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Image_processing.Managers
{
    public class ProcessingManager
    {
        private readonly BitmapManager bitmapManager;
        private readonly Command command;

        private readonly int commandArgumentValue;

        public ProcessingManager(BitmapManager bitmapManager, Command command)
        {
            this.bitmapManager = bitmapManager;
            this.command = command;

            if (command.ArgumentValue == null)
            {
                commandArgumentValue = 0;
                return;
            }

            Match match = Regex.Match(command.ArgumentValue, @"[+-]?\d+(\d+)?");

            if (match.Success && match.Value.Contains('-'))
            {
                commandArgumentValue =
                    int.Parse(string.Concat(command.ArgumentValue.Where(char.IsDigit))) * (-1);
            }
            else if (match.Success)
            {
                commandArgumentValue =
                    int.Parse(string.Concat(command.ArgumentValue.Where(char.IsDigit)));
            }
            else
            {
                commandArgumentValue = 0;
            }
        }

        #region Task 1
        public void ManageBrightnessModification()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    int red = TruncateColorValue(initialColor.R + commandArgumentValue);
                    int green = TruncateColorValue(initialColor.G + commandArgumentValue);
                    int blue = TruncateColorValue(initialColor.B + commandArgumentValue);

                    Color color = Color.FromArgb(red, green, blue);

                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageContrastModification()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

            double contrast = Math.Pow((100.0 + commandArgumentValue) / 100.0, 2);

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

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageNegative()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

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

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageHorizontalFlip()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

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

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageVerticalFlip()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

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

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageDiagonalFlip()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);

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

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageImageShrinking()
        {
            Bitmap bitmap = bitmapManager.LoadBitmapFile(command.FileName);
            Bitmap shrunkBitmap = bitmapManager.LoadBitmapFile(command.FileName);

            int shrunkWidth = bitmap.Width / 2;
            int shrunkHeight = bitmap.Height / 2;

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int posX =
                        (int)Math.Round((double)x / bitmap.Width / 2 * bitmap.Width);

                    int posY =
                        (int)Math.Round((double)y / bitmap.Height / 2 * bitmap.Height);

                    posX = Math.Min(posX, bitmap.Width - 1);
                    posY = Math.Min(posY, bitmap.Height - 1);

                    Color pixel = bitmap.GetPixel(x, y);
                    shrunkBitmap.SetPixel(posX, posY, pixel);
                }
            }

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageImageEnlargement()
        {
            throw new NotImplementedException();
        }

        public void ManageMidpointFilter()
        {
            throw new NotImplementedException();
        }

        public void ManageArithmeticMeanFilter()
        {
            throw new NotImplementedException();
        }

        public void ManageMeanSquareError()
        {
            throw new NotImplementedException();
        }

        public void ManagePeakMeanSquareError()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Task 1 private methods
        private int TruncateColorValue(int colorValue)
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
