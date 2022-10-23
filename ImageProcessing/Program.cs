using Image_processing.Managers;
using Image_processing.Records;
using System.Diagnostics;
using System.Drawing;

namespace Image_processing
{
    public class Program
    {
        private static BitmapManager? bitmapManager;
        private static ConsoleColor defaultConsoleColor = Console.ForegroundColor;

        static void Main(string[] args)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;

                string originalImagesFolder = "OriginalImages";
                string modifiedImagesFolder = "ModifiedImages";

                string projectDirectory = Directory.GetParent(workingDirectory).FullName + "\\net6.0";
                string originalImagesFolderPath = $@"{projectDirectory}\{originalImagesFolder}";
                string modifiedImagesFolderPath = $@"{projectDirectory}\{modifiedImagesFolder}";

                bitmapManager = new BitmapManager(originalImagesFolderPath, modifiedImagesFolderPath);

                if (args.Length == 0)
                {
                    DisplayHelpInformationMessage();

                    Environment.Exit(0);
                }

                //--help
                if (args[0] == Operations.Help)
                {
                    ProcessingManager.DisplayHelpMessage();

                    Environment.Exit(0);
                }

                //filename --operation
                if (args.Length == 2)
                {
                    Bitmap bitmap = bitmapManager.LoadBitmapFile(args[0]);
                    string operation = args[1];

                    switch (operation)
                    {
                        case Operations.Negative:
                            bitmap = bitmap.ManageNegative();
                            break;
                        case Operations.HorizontalFlip:
                            bitmap = bitmap.ManageHorizontalFlip();
                            break;
                        case Operations.VerticalFlip:
                            bitmap = bitmap.ManageVerticalFlip();
                            break;
                        case Operations.DiagonalFlip:
                            bitmap = bitmap.ManageDiagonalFlip();
                            break;
                    }

                    bitmapManager.SaveBitmapFile(args[0], bitmap);

                    DisplayCommandExecutedSuccesfullyMessage(args);
                }
                //filename --operation value
                else if (args.Length == 3)
                {
                    Bitmap bitmap = bitmapManager.LoadBitmapFile(args[0]);
                    string operation = args[1];
                    double value = int.Parse(args[2]);

                    switch (operation)
                    {
                        case Operations.BrightnessModification:
                            bitmap = bitmap.ManageBrightnessModification((int) value);
                            break;
                        case Operations.ContrastModification:
                            bitmap = bitmap.ManageContrastModification((int) value);
                            break;
                        case Operations.ImageShrinking:
                            bitmap = bitmap.ManageImageShrinking(value);
                            break;
                        case Operations.ImageEnlargement:
                            bitmap = bitmap.ManageImageEnlargement((int) value);
                            break;
                    }

                    bitmapManager.SaveBitmapFile(args[0], bitmap);

                    DisplayCommandExecutedSuccesfullyMessage(args);
                }
                else if (args.Length == 4)
                {

                }
                else
                {
                    DisplayIncorrectCommandMessage(args);
                    DisplayHelpInformationMessage();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = defaultConsoleColor;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = defaultConsoleColor;
            }
        }

        public static void DisplayCommandExecutedSuccesfullyMessage(string[] args)
        {
            string command = string.Join(" ", args);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Command \"{command}\" has been executed successfully");
            Console.ForegroundColor = defaultConsoleColor;
        }

        public static void DisplayIncorrectCommandMessage(string[] args)
        {
            string command = string.Join(" ", args);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Incorrect command \"{command}\"");
            Console.ForegroundColor = defaultConsoleColor;
        }

        public static void DisplayHelpInformationMessage()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Run program with \"--help\" parameter to see all available commands with description");
            Console.ForegroundColor = defaultConsoleColor;
        }
    }
}