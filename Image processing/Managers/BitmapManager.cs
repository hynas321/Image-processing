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
            File.Copy(
                $@"{originalImagesFolderPath}\{filename}",
                $@"{modifiedImagesFolderPath}\{filename}",
                true
            );

            bitmap.Save($@"{modifiedImagesFolderPath}\{filename}");
        }
    }
}
