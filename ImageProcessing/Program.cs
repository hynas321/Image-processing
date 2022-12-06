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
                        case Operations.ExtractionOfDetailsIOptimized:
                            bitmap = bitmap.ManageExtractionOfDetailsIOptimized();
                            break;
                        case Operations.SobelOperator:
                            bitmap = bitmap.ManageSobelOperator();
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
                //filename --operation intValue
                else if (args.Length == 3 && args[1].StartsWith("--")
                    && int.TryParse(args[2], out int value))
                {
                    string filename = args[0];
                    string operation = args[1];

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);

                    switch (operation)
                    {
                        case Operations.BrightnessModification:
                            bitmap = bitmap.ManageBrightnessModification((int)value);
                            break;
                        case Operations.ContrastModification:
                            bitmap = bitmap.ManageContrastModification(value);
                            break;
                        case Operations.ImageShrinking:
                            bitmap = bitmap.ManageImageShrinking((int)value);
                            break;
                        case Operations.ImageEnlargement:
                            bitmap = bitmap.ManageImageEnlargement((int)value);
                            break;
                        case Operations.MidpointFilter:
                            bitmap = bitmap.ManageMidpointFilter((int)value);
                            break;
                        case Operations.ArithmeticMeanFilter:
                            bitmap = bitmap.ManageArithmeticMeanFilter((int)value);
                            break;
                        case Operations.ExtractionOfDetailsI:
                            bitmap = bitmap.ManageExtractionOfDetailsI(value);
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
                //filename --operation charValue
                else if (args.Length == 3 && args[1].StartsWith("--") 
                    && char.TryParse(args[2], out char color))
                {
                    string filename = args[0];
                    string operation = args[1];

                    double resultToDisplay = -1;
                    bool histogramPlot = false;

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);
                    Plot plot = new Plot();

                    switch (operation)
                    {
                        case Operations.Histogram:
                            plot = ProcessingManager.CreateHistogramImage(bitmap, color);
                            histogramPlot = true;
                            break;
                        case Operations.Mean:
                            resultToDisplay = ProcessingManager.CalculateMean(bitmap, color);
                            break;
                        case Operations.Variance:
                            resultToDisplay = ProcessingManager.CalculateVariance(bitmap, color);
                            break;
                        case Operations.StandardDeviation:
                            resultToDisplay = ProcessingManager.CalculateStandardDeviation(bitmap, color);
                            break;
                        case Operations.VariationCoefficientI:
                            resultToDisplay = ProcessingManager.CalculateVariationCoefficientI(bitmap, color);
                            break;
                        case Operations.AsymmetryCoefficient:
                            resultToDisplay = ProcessingManager.CalculateAsymmetryCoefficient(bitmap, color);
                            break;
                        case Operations.FlatteningCoefficient:
                            resultToDisplay = ProcessingManager.CalculateFlatteningCoefficient(bitmap, color);
                            break;
                        case Operations.VariationCoefficientII:
                            resultToDisplay = ProcessingManager.CalculateVariationCoefficientII(bitmap, color);
                            break;
                        case Operations.InformationSourceEntropy:
                            resultToDisplay = ProcessingManager.CalculateInformationSourceEntropy(bitmap, color);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    if (histogramPlot)
                    {
                        fileManager.SaveHistogram(filename, plot, color);
                        ConsoleManager.DisplayCommandExecutedSuccesfullyMessage(command);
                    }
                    else
                    {
                        ConsoleManager.WriteLineWithForegroundColor(
                            $"Operation's \"{operation}\" result: {resultToDisplay}",
                            ConsoleColor.Green
                        );
                    }
                }
                //filename --operation doubleValue intValue
                else if (args.Length == 4 && args[1].StartsWith("--") 
                    && double.TryParse(args[2], out double alpha)
                    && int.TryParse(args[3], out int minBrightness))
                {
                    string filename = args[0];
                    string operation = args[1];

                    Bitmap bitmap = fileManager.LoadBitmapFile(filename);

                    switch (operation)
                    {
                        case Operations.RaleighFinalProbabilityDensityFunction:
                            bitmap = bitmap.ManageRaleigh(alpha, minBrightness);
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