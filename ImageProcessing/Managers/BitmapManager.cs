using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Linq;

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

        public Bitmap LoadBitmapFile(string file)
        {
            if (File.Exists($@"{originalImagesFolderPath}\{file}") == false)
            {
                throw new FileNotFoundException(
                    $"File {file} does not exist in path {originalImagesFolderPath}"
                );
            }

            Bitmap bitmap = (Bitmap)Bitmap.FromFile($@"{originalImagesFolderPath}\{file}");

            return bitmap;
        }

        public void SaveBitmapFile(string file, Bitmap bitmap)
        {
            try
            {
                File.Copy(
                    $@"{originalImagesFolderPath}\{file}",
                    $@"{modifiedImagesFolderPath}\{file}",
                    true
                );

                bitmap.Save($@"{modifiedImagesFolderPath}\{file}");
            }
            catch
            {
                throw new FileNotFoundException($"File {file} could not be saved in {modifiedImagesFolderPath} location");
            }
        }
    }
}
