using Image_processing.Exceptions;
using Image_processing.Managers;
using Image_processing.Records;
using ScottPlot;
using System.Drawing;

namespace Image_processing
{
    public class Program
    {
        private static FileManager? fileManager;

        static void Main(string[] args)
        {
            try
            {
                string originalImagesFolder = "OriginalImages";
                string modifiedImagesFolder = "ModifiedImages";
                string plotImagesFolder = "PlotImages";
                string projectDirectory = Environment.CurrentDirectory;
                string originalImagesFolderPath = $@"{projectDirectory}\{originalImagesFolder}";
                string modifiedImagesFolderPath = $@"{projectDirectory}\{modifiedImagesFolder}";
                string plotImagesFolderPath = $@"{projectDirectory}\{plotImagesFolder}";
                string command = string.Join(" ", args);

                fileManager = new FileManager(
                    originalImagesFolderPath,
                    modifiedImagesFolderPath,
                    plotImagesFolderPath
                );

                if (args.Length == 0)
                {
                    ConsoleManager.DisplayHelpInformationMessage();
                    Console.ResetColor();

                    Environment.Exit(0);
                }

                //--help
                if (args[0] == Operations.Help)
                {
                    ConsoleManager.DisplayHelpMessage();

                    Environment.Exit(0);
                }

                //filename --operation
                if (args.Length == 2 && args[1].StartsWith("--"))
                {
                    string filename = args[0];
                    string operation = args[1];

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);

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
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    fileManager.SaveBitmapFile(args[0], bitmap, operation);

                    ConsoleManager.DisplayCommandExecutedSuccesfullyMessage(command);
                }
                //filename --operation value
                else if (args.Length == 3 && double.TryParse(args[2], out double result))
                {
                    string filename = args[0];
                    string operation = args[1];
                    int value = int.Parse(args[2]);

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);

                    switch (operation)
                    {
                        case Operations.BrightnessModification:
                            bitmap = bitmap.ManageBrightnessModification(value);
                            break;
                        case Operations.ContrastModification:
                            bitmap = bitmap.ManageContrastModification(value);
                            break;
                        case Operations.ImageShrinking:
                            bitmap = bitmap.ManageImageShrinking(value);
                            break;
                        case Operations.ImageEnlargement:
                            bitmap = bitmap.ManageImageEnlargement(value);
                            break;
                        case Operations.MidpointFilter:
                            bitmap = bitmap.ManageMidpointFilter(value);
                            break;
                        case Operations.ArithmeticMeanFilter:
                            bitmap = bitmap.ManageArithmeticMeanFilter(value);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    fileManager.SaveBitmapFile(args[0], bitmap, operation);

                    ConsoleManager.DisplayCommandExecutedSuccesfullyMessage(command);
                }
                //filename filename --operation
                else if (args.Length == 3 && args[2].StartsWith("--"))
                {
                    string filename1 = args[0];
                    string filename2 = args[1];
                    string operation = args[2];

                    Bitmap bitmap1 = fileManager.LoadBitmapFile(filename1);
                    Bitmap bitmap2 = fileManager.LoadBitmapFile(filename2);

                    double resultToDisplay = 0;

                    switch (operation)
                    {
                        case Operations.MeanSquareError:
                            resultToDisplay = ProcessingManager.CalculateMeanSquareError(bitmap1, bitmap2);
                            break;
                        case Operations.PeakMeanSquareError:
                            resultToDisplay = ProcessingManager.CalculatePeakMeanSquareError(bitmap1, bitmap2);
                            break;
                        case Operations.SignalToNoiseRatio:
                            resultToDisplay = ProcessingManager.CalculateSignalToNoiseRatio(bitmap1, bitmap2);
                            break;
                        case Operations.PeakSignalToNoiseRatio:
                            resultToDisplay = ProcessingManager.CalculatePeakSignalToNoiseRatio(bitmap1, bitmap2);
                            break;
                        case Operations.MaximumDifference:
                            resultToDisplay = ProcessingManager.CalculateMaximumDifference(bitmap1, bitmap2);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    ConsoleManager.WriteLineWithForegroundColor(
                        $"Operation's \"{operation}\" result: {resultToDisplay}",
                        ConsoleColor.Green
                    );
                }
                //filename --operation char
                else if (args.Length == 3)
                {
                    string filename = args[0];
                    string operation = args[1];
                    char color = char.Parse(args[2]);

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);
                    Plot plot;

                    switch (operation)
                    {
                        case Operations.Histogram:
                            plot = ProcessingManager.ManageHistogram(bitmap, color);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    fileManager.SaveHistogram(filename, plot, color);

                    ConsoleManager.DisplayCommandExecutedSuccesfullyMessage(command);
                }
                else
                {
                    throw new CommandException(
                        $"Command {command} is incorrect\n" +
                        $"Run program with \"--help\" parameter to see all available commands with description"
                    );
                }
            }
            catch (Exception ex)
            {
                if (ex is CommandException || ex is FileNotFoundException)
                {
                    ConsoleManager.WriteLineWithForegroundColor(ex.Message, ConsoleColor.Red);
                }
                else
                {
                    ConsoleManager.WriteLineWithForegroundColor(ex.ToString(), ConsoleColor.Red);
                }
            }

            Console.ResetColor();
        }
    }
}