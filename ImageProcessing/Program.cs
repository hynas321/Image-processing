using Image_processing.Managers;
using Image_processing.Records;
using System.Diagnostics;
using System.Drawing;

namespace Image_processing
{
    public class Program
    {
        private static BitmapManager? bitmapManager;

        static void Main(string[] args)
        {
            try
            {
                string workingDirectory = Environment.CurrentDirectory;

                string originalImagesFolder = "OriginalImages";
                string modifiedImagesFolder = "ModifiedImages";

                string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
                string originalImagesFolderPath = $@"{projectDirectory}\{originalImagesFolder}";
                string modifiedImagesFolderPath = $@"{projectDirectory}\{modifiedImagesFolder}";

                Console.WriteLine("Image processing app");
                Console.WriteLine("All available commands with description are available under \"--help\" command");
                Console.Write("Please enter the command with the following syntax\n");
                Console.Write("filename --operation [-argument=value]\n");

                bitmapManager = new BitmapManager(originalImagesFolderPath, modifiedImagesFolderPath);

                if (args.Length == 0)
                {

                }
                //--help
                else if (args[0] == Operations.Help)
                {
                    ProcessingManager.DisplayHelpMessage();
                }
                //filename --operation
                else if (args.Length == 2)
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
                }
                else if (args.Length == 4)
                {

                }
                else
                {

                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}