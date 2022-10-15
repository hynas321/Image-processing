using Image_processing.Managers;
using Image_processing.Models;

namespace Image_processing
{
    public class Program
    {
        private static BitmapManager? bitmapManager;
        private static ProcessingManager? processingManager;
        private static CommandManager? commandManager;

        static void Main(string[] args)
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

            while (true)
            {
                try
                {
                    Console.Write("Input: ");

                    bitmapManager = new BitmapManager(originalImagesFolderPath, modifiedImagesFolderPath);

                    string? input = Console.ReadLine()?.Trim(' ');
                    Command command = CommandManager.GetInputCommandFromConsole(input);

                    processingManager = new ProcessingManager(bitmapManager, command);
                    commandManager = new CommandManager(processingManager, command);

                    commandManager.ExecuteCommand();
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
}