using Image_processing.Models;
using System.Drawing;

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
            }
            else
            {
                bool digitsAreInArgumentValue = int.TryParse(
                    string.Concat(command.ArgumentValue.Where(char.IsDigit)),
                    out commandArgumentValue
                );

                if (digitsAreInArgumentValue == false) {
                    commandArgumentValue = 0;
                }
            }
        }

        #region Task 1
        public void ManageBrightnessModification()
        {
            //Test, not a real implementation
            Bitmap bitmap = bitmapManager.ReadBitmapFile(command.FileName);

            for (int i = 0; i < commandArgumentValue; i++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }
                }
            }

            bitmapManager.SaveBitmapFile(command.FileName, bitmap);
        }

        public void ManageContrastModification()
        {

        }

        public void ManageNegative()
        {

        }

        public void ManageHorizontalFlip()
        {

        }

        public void ManageVerticalFlip()
        {

        }

        public void ManageDiagonalFlip()
        {

        }

        public void ManageImageShrinking()
        {

        }

        public void ManageImageEnlargement()
        {

        }

        public void ManageMidpointFilter()
        {

        }

        public void ManageArithmeticMeanFilter()
        {

        }

        public void ManageMeanSquareError()
        {

        }

        public void ManagePeakMeanSquareError()
        {

        }

        #endregion
    }
}
