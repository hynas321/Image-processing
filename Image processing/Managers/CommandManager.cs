using Image_processing.Models;
using Image_processing.Records;

namespace Image_processing.Managers
{
    public class CommandManager
    {
        private readonly BitmapManager bitmapManager;

        public CommandManager(BitmapManager bitmapManager)
        {
            this.bitmapManager = bitmapManager;
        }

        public Command GetInputCommandFromConsole(string input)
        {
            string[] inputArray = input.Split(' ');

            if (inputArray.Length >= 3)
            {
                return new Command(inputArray[0], inputArray[1], inputArray[2]);
            }
            if (inputArray.Length == 2)
            {
                return new Command(inputArray[0], inputArray[1], null);
            }
            if (inputArray.Length == 1)
            {
                return new Command(inputArray[0], null, null);
            }

            return null;
        }

        public void ExecuteCommand(Command command)
        {
            if (command == null)
            {
                DisplayIncorrectCommandMessage(command);
                return;
            }

            if (command.ImageFile == Operations.Help)
            {
                DisplayHelpMessage();
            }
            else if (command.Operation == Operations.BrightnessModification)
            {
                ManageBrightnessModification();
            }
            else if (command.Operation == Operations.ContrastModification)
            {
                ManageContrastModification();
            }
            else if (command.Operation == Operations.Negative)
            {
                ManageNegative();
            }
            else if (command.Operation == Operations.HorizontalFlip)
            {
                ManageHorizontalFlip();
            }
            else if (command.Operation == Operations.VerticalFlip)
            {
                ManageVerticalFlip();
            }
            else if (command.Operation == Operations.DiagonalFlip)
            {
                ManageDiagonalFlip();
            }
            else if (command.Operation == Operations.ImageShrinking)
            {
                ManageImageShrinking();
            }
            else if (command.Operation == Operations.ImageEnlargement)
            {
                ManageImageEnlargement();
            }
            else if (command.Operation == Operations.MeanSquareError)
            {
                ManageMeanSquareError();
            }
            else if (command.Operation == Operations.PeakMeanSquareError)
            {
                ManagePeakMeanSquareError();
            }
            else if (command.Operation == Operations.MidpointFilter)
            {
                ManageMidpointFilter();
            }
            else if (command.Operation == Operations.ArithmeticMeanFilter)
            {
                ManageArithmeticMeanFilter();
            }
            else
            {
                DisplayIncorrectCommandMessage(command);
                return;
            }

            DisplayCommandExecutedMessage(command);
        }

        #region Task 1

        private void DisplayCommandExecutedMessage(Command command)
        {
            Console.WriteLine($"Command {command} has been executed.");
        }

        private void DisplayIncorrectCommandMessage(Command command)
        {
            Console.WriteLine($"Command {command} is invalid.");
        }

        private void DisplayHelpMessage()
        {

        }

        private void ManageBrightnessModification()
        {

        }

        private void ManageContrastModification()
        {

        }

        private void ManageNegative()
        {

        }

        private void ManageHorizontalFlip()
        {

        }

        private void ManageVerticalFlip()
        {

        }

        private void ManageDiagonalFlip()
        {

        }

        private void ManageImageShrinking()
        {

        }

        private void ManageImageEnlargement()
        {

        }

        private void ManageMidpointFilter()
        {

        }

        private void ManageArithmeticMeanFilter()
        {

        }

        private void ManageMeanSquareError()
        {

        }

        private void ManagePeakMeanSquareError()
        {

        }

        #endregion
    }
}
