using ScottPlot;
using System.Drawing;

namespace Image_processing.Managers
{
    public class FileManager
    {
        private string originalImagesFolderPath;
        private string modifiedImagesFolderPath;
        private string imagePlotsFolderPath;

        public FileManager(
            string originalImagesFolderPath,
            string modifiedImagesFolderPath,
            string imagePlotsFolderPath
        )
        {
            this.originalImagesFolderPath = originalImagesFolderPath;
            this.modifiedImagesFolderPath = modifiedImagesFolderPath;
            this.imagePlotsFolderPath = imagePlotsFolderPath;
        }

        public Bitmap LoadBitmapFile(string file)
        {
            Bitmap bitmap;

            if (File.Exists($@"{originalImagesFolderPath}\{file}") == true)
            {
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
                    = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{operation.TrimStart('-')}_{file}";

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

        public void SaveHistogram(string file, Plot plot, char color)
        {
            try
            {
                string colorName;

                switch (color)
                {
                    case 'R':
                        colorName = "red_color";
                        break;
                    case 'G':
                        colorName = "green_color";
                        break;
                    case 'B':
                        colorName = "blue_color";
                        break;
                    default:
                        colorName = "no_color";
                        break;
                }

                string savedFile
                    = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{colorName}_histogram_{file}";

                plot.SaveFig($@"{imagePlotsFolderPath}\{savedFile}");
            }
            catch
            {
                throw new Exception($"File {file} could not be saved in location {imagePlotsFolderPath}");
            }
        }
    }
}
