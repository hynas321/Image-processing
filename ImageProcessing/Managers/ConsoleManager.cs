namespace Image_processing.Managers
{
    public class ConsoleManager
    {
        private static ConsoleColor defaultColor = Console.ForegroundColor;

        public static void WriteLineWithForegroundColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = defaultColor;
        }
    }
}
