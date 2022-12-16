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
        private static ProcessingManager? processingManager;

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

                processingManager = new ProcessingManager();

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
                            bitmap = processingManager.ApplyNegative(bitmap);
                            break;
                        case Operations.HorizontalFlip:
                            bitmap = processingManager.ApplyHorizontalFlip(bitmap);
                            break;
                        case Operations.VerticalFlip:
                            bitmap = processingManager.ApplyVerticalFlip(bitmap);
                            break;
                        case Operations.DiagonalFlip:
                            bitmap = processingManager.ApplyDiagonalFlip(bitmap);
                            break;
                        case Operations.ExtractionOfDetailsIOptimized:
                            bitmap = processingManager.ApplyExtractionOfDetailsIOptimized(bitmap);
                            break;
                        case Operations.SobelOperator:
                            bitmap = processingManager.ApplySobelOperator(bitmap);
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
                            bitmap = processingManager.ApplyBrightnessModification(bitmap, value);
                            break;
                        case Operations.ContrastModification:
                            bitmap = processingManager.ApplyContrastModification(bitmap, value);
                            break;
                        case Operations.ImageShrinking:
                            bitmap = processingManager.ApplyImageShrinking(bitmap, value);
                            break;
                        case Operations.ImageEnlargement:
                            bitmap = processingManager.ApplyImageEnlargement(bitmap, value);
                            break;
                        case Operations.MidpointFilter:
                            bitmap = processingManager.ApplyMidpointFilter(bitmap, value);
                            break;
                        case Operations.ArithmeticMeanFilter:
                            bitmap = processingManager.ApplyArithmeticMeanFilter(bitmap, value);
                            break;
                        case Operations.ExtractionOfDetailsI:
                            bitmap = processingManager.ApplyExtractionOfDetailsI(bitmap, value);
                            break;
                        case Operations.Dilation:
                            bitmap = processingManager.ApplyDilation(bitmap, value);
                            break;
                        case Operations.Erosion:
                            bitmap = processingManager.ApplyErosion(bitmap, value);
                            break;
                        case Operations.Opening:
                            bitmap = processingManager.ApplyOpening(bitmap, value);
                            break;
                        case Operations.Closing:
                            bitmap = processingManager.ApplyClosing(bitmap, value);
                            break;
                        case Operations.Hmt:
                            bitmap = processingManager.ApplyHmt(bitmap, value);
                            break;
                        case Operations.M1Operation1:
                            bitmap = processingManager.ApplyM1Operation1(bitmap, value);
                            break;
                        case Operations.M1Operation2:
                            bitmap = processingManager.ApplyM1Operation2(bitmap, value);
                            break;
                        case Operations.M1Operation3:
                            bitmap = processingManager.ApplyM1Operation3(bitmap, value);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    fileManager.SaveBitmapFile(args[0], bitmap, operation, value);

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
                            resultToDisplay = processingManager.CalculateMeanSquareError(bitmap1, bitmap2);
                            break;
                        case Operations.PeakMeanSquareError:
                            resultToDisplay = processingManager.CalculatePeakMeanSquareError(bitmap1, bitmap2);
                            break;
                        case Operations.SignalToNoiseRatio:
                            resultToDisplay = processingManager.CalculateSignalToNoiseRatio(bitmap1, bitmap2);
                            break;
                        case Operations.PeakSignalToNoiseRatio:
                            resultToDisplay = processingManager.CalculatePeakSignalToNoiseRatio(bitmap1, bitmap2);
                            break;
                        case Operations.MaximumDifference:
                            resultToDisplay = processingManager.CalculateMaximumDifference(bitmap1, bitmap2);
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
                            plot = processingManager.CreateHistogramImage(bitmap, color);
                            histogramPlot = true;
                            break;
                        case Operations.Mean:
                            resultToDisplay = processingManager.CalculateMean(bitmap, color);
                            break;
                        case Operations.Variance:
                            resultToDisplay = processingManager.CalculateVariance(bitmap, color);
                            break;
                        case Operations.StandardDeviation:
                            resultToDisplay = processingManager.CalculateStandardDeviation(bitmap, color);
                            break;
                        case Operations.VariationCoefficientI:
                            resultToDisplay = processingManager.CalculateVariationCoefficientI(bitmap, color);
                            break;
                        case Operations.AsymmetryCoefficient:
                            resultToDisplay = processingManager.CalculateAsymmetryCoefficient(bitmap, color);
                            break;
                        case Operations.FlatteningCoefficient:
                            resultToDisplay = processingManager.CalculateFlatteningCoefficient(bitmap, color);
                            break;
                        case Operations.VariationCoefficientII:
                            resultToDisplay = processingManager.CalculateVariationCoefficientII(bitmap, color);
                            break;
                        case Operations.InformationSourceEntropy:
                            resultToDisplay = processingManager.CalculateInformationSourceEntropy(bitmap, color);
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
                            bitmap = processingManager.ApplyRaleigh(bitmap, alpha, minBrightness);
                            break;
                        default:
                            throw new CommandException(
                                $"Command {command} is incorrect\n" +
                                $"Run program with \"--help\" parameter to see all available commands with description"
                            );
                    }

                    fileManager.SaveBitmapFile(args[0], bitmap, operation, alpha, minBrightness);

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