using Image_processing.Managers;
using Image_processing.Models;

namespace Image_processing
{
    public class Program
    {
        private static BitmapManager? bitmapManager;
        private static CommandManager? commandManager;

        static void Main(string[] args)
        {
            Console.WriteLine("All available commands with description are available under \"--help\" command");
            Console.Write("Please enter the command with the following syntax\n");
            Console.Write("filename --operation [-argument=value]\n");

            while (true)
            {
                try
                {
                    Console.Write("Input: ");

                    bitmapManager = new BitmapManager();
                    commandManager = new CommandManager(bitmapManager);

                    string? input = Console.ReadLine().Trim(' ');
                    Command command = commandManager.GetInputCommandFromConsole(input);

                    commandManager.ExecuteCommand(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}