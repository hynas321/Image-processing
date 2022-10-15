using Image_processing.Models;
using System.Drawing;
using System.Text.RegularExpressions;

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
            Bitmap bitmap = bitmapManager.ReadBitmapFile(command.FileName);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    Color color = Color.FromArgb(
                        TruncateColorValue(initialColor.R + commandArgumentValue),
                        TruncateColorValue(initialColor.G + commandArgumentValue),
                        TruncateColorValue(initialColor.B + commandArgumentValue)
                    );

                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageContrastModification()
        {
            throw new NotImplementedException();
        }

        public void ManageNegative()
        {
            Bitmap bitmap = bitmapManager.ReadBitmapFile(command.FileName);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color initialColor = bitmap.GetPixel(x, y);

                    Color color = Color.FromArgb(
                        255 - initialColor.R,
                        255 - initialColor.G,
                        255 - initialColor.B
                    );
                    
                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageHorizontalFlip()
        {
            throw new NotImplementedException();
        }

        public void ManageVerticalFlip()
        {
            throw new NotImplementedException();
        }

        public void ManageDiagonalFlip()
        {
            throw new NotImplementedException();
        }

        public void ManageImageShrinking()
        {
            throw new NotImplementedException();
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
