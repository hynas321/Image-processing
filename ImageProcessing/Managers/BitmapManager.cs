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

        public Bitmap LoadBitmapFile(string file)
        {
            Bitmap bitmap;

            if (File.Exists($@"{originalImagesFolderPath}\{file}") == true)
            {
                //bitmap = (Bitmap)Bitmap.FromFile($@"{originalImagesFolderPath}\{file}");
                bitmap = new Bitmap(Image.FromFile($@"{originalImagesFolderPath}\{file}"));
            }
            else if (File.Exists($@"{modifiedImagesFolderPath}\{file}") == true)
            {
                bitmap = new Bitmap(Image.FromFile($@"{modifiedImagesFolderPath}\{file}"));
            }
            else
            {
                throw new FileNotFoundException(
                    $"File {file} does not exist in path {originalImagesFolderPath}"
                );
            }

            return bitmap;
        }

        public void SaveBitmapFile(string file, Bitmap bitmap, string operation)
        {
            try
            {
                string savedFile
                    = $"{DateTime.Now.ToString("dd-MM-yy_HH-mm-ss")}_{operation.TrimStart('-')}_{file}";

                File.Copy(
                    $@"{originalImagesFolderPath}\{file}",
                    $@"{modifiedImagesFolderPath}\{file}",
                    true
                );

                bitmap.Save($@"{modifiedImagesFolderPath}\{file}");

                FileInfo fInfo = new FileInfo($@"{modifiedImagesFolderPath}\{file}");
                fInfo.MoveTo($@"{modifiedImagesFolderPath}\{savedFile}");
            }
            catch
            {
                throw new Exception($"File {file} could not be saved in location {modifiedImagesFolderPath}");
            }
        }
    }
}
