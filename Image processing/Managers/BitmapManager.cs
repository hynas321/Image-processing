using System.Drawing;

namespace Image_processing.Managers
{
    public class BitmapManager
    {
        private string originalImagesFolderPath;
        private string modifiedImagesFolderPath;

        public BitmapManager(
            string originalImagesFolderPath,
            string modifiedImagesFolderPath
        )
        {
            this.originalImagesFolderPath = originalImagesFolderPath;
            this.modifiedImagesFolderPath = modifiedImagesFolderPath;
        }

        public Bitmap ReadBitmapFile(string filename)
        {
            if (File.Exists($@"{originalImagesFolderPath}\{filename}") == false)
            {
                throw new FileNotFoundException(
                    $"File {filename} does not exist in path {originalImagesFolderPath}"
                );
            }

            return new Bitmap($@"{originalImagesFolderPath}\{filename}");
        }

        public void SaveBitmapFile(string filename, Bitmap bitmap)
        {
            if (File.Exists($@"{modifiedImagesFolderPath}\{filename}") == false)
            {
                File.Copy(
                    $@"{originalImagesFolderPath}\{filename}",
                    $@"{modifiedImagesFolderPath}\{filename}",
                    false
                );

                bitmap.Save($@"{modifiedImagesFolderPath}\{filename}");
            }
            else
            {
                Console.WriteLine(
                    $"File {filename} already exists in path {modifiedImagesFolderPath}, overwrite? [Y/N]"
                );

                ConsoleKey key = Console.ReadKey(false).Key;
                Console.WriteLine();

                switch (key)
                {
                    case ConsoleKey.Y:
                        File.Copy(
                            $@"{originalImagesFolderPath}\{filename}",
                            $@"{modifiedImagesFolderPath}\{filename}",
                            true
                        );

                        Console.WriteLine($"File {filename} has been overwritten");
                        break;
                    default:
                        Console.WriteLine($"File {filename} has not been overwritten");
                        break;
                }
            }
        }
    }
}
