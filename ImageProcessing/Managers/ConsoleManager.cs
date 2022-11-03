using Image_processing.Records;

namespace Image_processing.Managers
{
    public class ConsoleManager
    {
        public static void WriteLineWithForegroundColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteWithForegroundColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void DisplayCommandExecutedSuccesfullyMessage(string command, string locationPath)
        {
            ConsoleManager.WriteLineWithForegroundColor(
                $"Command \"{command}\" has been executed successfully.",
                ConsoleColor.Green
            );
        }

        public static void DisplayHelpInformationMessage()
        {
            ConsoleManager.WriteLineWithForegroundColor(
                $"Run program with \"--help\" parameter to see all available commands with description",
                ConsoleColor.Yellow
            );
        }

        public static void DisplayHelpMessage()
        {
            Dictionary<string, string> operationDictionary = Operations.OperationsDictionary;

            WriteLineWithForegroundColor("***IMAGE PROCESSING APPLICATION***", ConsoleColor.Cyan);
            WriteLineWithForegroundColor(
                "In order to process an image, one should input parameters when running an application.\n" +
                "Parameters have to be given in the correct order.\n" +
                "Original, unmodified images should be kept in the \"OriginalImages\" folder in exe file's location.\n" +
                "The processed image is created in the \"ModifiedImages\" folder in exe file's location.",
                ConsoleColor.White
            );
            WriteLineWithForegroundColor("PARAMETERS", ConsoleColor.Cyan);
            WriteWithForegroundColor("filename ", ConsoleColor.Yellow);
            WriteLineWithForegroundColor("image file present in \"OriginalImages\" or \"ModifiedImages\" folder (example: lena.bmp)", ConsoleColor.White);
            WriteWithForegroundColor("--(operation name) ", ConsoleColor.Yellow);
            WriteLineWithForegroundColor("type of the image processing operation (example: --brightness)", ConsoleColor.White);
            WriteWithForegroundColor("intValue ", ConsoleColor.Yellow);
            WriteLineWithForegroundColor("numeric value (example: 15)", ConsoleColor.White);
            WriteLineWithForegroundColor("COMMANDS", ConsoleColor.Cyan);

            foreach (KeyValuePair<string, string> elem in operationDictionary)
            {
                WriteWithForegroundColor($"{elem.Key} ", ConsoleColor.Yellow);
                WriteWithForegroundColor($"{elem.Value}\n", ConsoleColor.White);
            }
        }
    }
}
