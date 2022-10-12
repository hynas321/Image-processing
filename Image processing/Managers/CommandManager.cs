using Image_processing.Models;
using Image_processing.Records;
using System.Text.RegularExpressions;

namespace Image_processing.Managers
{
    public class CommandManager
    {
        private readonly ProcessingManager processingManager;
        private readonly Command command;

        public CommandManager(ProcessingManager processingManager, Command command)
        {
            this.processingManager = processingManager;
            this.command = command;
        }

        public static Command GetInputCommandFromConsole(string input)
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

        public void ExecuteCommand()
        {
            if (command.FileName == null || command.FileName == string.Empty)
            {
                Console.WriteLine("Incorrect command");
                return;
            }

            if (command.FileName == Operations.Help)
            {
                DisplayHelpMessage();
                return;
            }

            if (CheckIfFilenameIsCorrect(command.FileName) == false)
            {
                Console.WriteLine($"Invalid filename {command.FileName}");
                return;
            }

            if (command.ArgumentValue != null &&
                CheckIfArgumentValueIsCorrect(command.ArgumentValue) == false)
            {
                Console.WriteLine($@"Invalid argument {command.ArgumentValue}");
                return;
            }

            switch (command.Operation)
            {
                case Operations.BrightnessModification:
                    processingManager.ManageBrightnessModification();
                    break;
                case Operations.ContrastModification:
                    processingManager.ManageContrastModification();
                    break;
                case Operations.Negative:
                    processingManager.ManageNegative();
                    break;
                case Operations.HorizontalFlip:
                    processingManager.ManageHorizontalFlip();
                    break;
                case Operations.VerticalFlip:
                    processingManager.ManageVerticalFlip();
                    break;
                case Operations.DiagonalFlip:
                    processingManager.ManageDiagonalFlip();
                    break;
                case Operations.ImageShrinking:
                    processingManager.ManageImageShrinking();
                    break;
                case Operations.ImageEnlargement:
                    processingManager.ManageImageEnlargement();
                    break;
                case Operations.MeanSquareError:
                    processingManager.ManageMeanSquareError();
                    break;
                case Operations.PeakMeanSquareError:
                    processingManager.ManagePeakMeanSquareError();
                    break;
                case Operations.MidpointFilter:
                    processingManager.ManageMidpointFilter();
                    break;
                case Operations.ArithmeticMeanFilter:
                    processingManager.ManageArithmeticMeanFilter();
                    break;
                default:
                    if (command.Operation != null)
                    {
                        Console.WriteLine($"Invalid operation {command.Operation}");
                    }
                    else
                    {
                        Console.WriteLine("No operation parameter has been given");
                    }
                    return;
            }

            Console.WriteLine($"Command {command} has been executed successfully");
        }

        private void DisplayHelpMessage()
        {
            Console.WriteLine("*Help message*");
        }

        private bool CheckIfFilenameIsCorrect(string filename)
        {
            string suffix = ".bmp";

            return filename.Contains(suffix);
        }

        private bool CheckIfArgumentValueIsCorrect(string argumentValue)
        {
            string prefix = "-argument=";

            return Regex.Match(argumentValue, "-argument=[0-9]").Success;
        }
    }
}
