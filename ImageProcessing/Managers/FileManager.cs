using ScottPlot;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Image_processing.Managers
{
    public class FileManager
    {
        private readonly string originalImagesFolderPath;
        private readonly string modifiedImagesFolderPath;
        private readonly string imagePlotsFolderPath;

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
            if (File.Exists($@"{originalImagesFolderPath}\{file}") == true)
            {
                return new Bitmap((Bitmap)Image.FromFile($@"{originalImagesFolderPath}\{file}"));
            }

            if (File.Exists($@"{modifiedImagesFolderPath}\{file}") == true)
            {
                return new Bitmap(Image.FromFile($@"{modifiedImagesFolderPath}\{file}"));
            }

            throw new FileNotFoundException($"File {file} does not exist in path {originalImagesFolderPath}");
        }

        public void SaveBitmapFile(string file, Bitmap bitmap, string operation, [Optional] double? parameter1, [Optional] double? parameter2)
        {
            try
            {
                string savedFile;

                if (parameter1 != null && parameter2 == null)
                {
                    savedFile = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{operation.TrimStart('-')}_param_{parameter1}_{file}";
                }
                else if (parameter1 != null && parameter2 != null)
                {
                    savedFile = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{operation.TrimStart('-')}_param1_{parameter1}_param2_{parameter2}_{file}";
                }
                else
                {
                    savedFile = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{operation.TrimStart('-')}_{file}";
                }

                bitmap.Save($@"{modifiedImagesFolderPath}\{file}", ImageFormat.Bmp);

                FileInfo fInfo = new FileInfo($@"{modifiedImagesFolderPath}\{file}");
                fInfo.MoveTo($@"{modifiedImagesFolderPath}\{savedFile}", false);
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
                string colorName = color switch
                {
                    'R' => "red_color",
                    'G' => "green_color",
                    'B' => "blue_color",
                    _ => "no_color",
                };

                string savedFile = $"{DateTime.Now:dd-MM-yy_HH-mm-ss}_{colorName}_histogram_{file}";

                plot.SaveFig($@"{imagePlotsFolderPath}\{savedFile}");
            }
            catch
            {
                throw new Exception($"File {file} could not be saved in location {imagePlotsFolderPath}");
            }
        }
    }
}
