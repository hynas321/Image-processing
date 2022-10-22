using System.Drawing;
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

            Image image = Image.FromFile($@"{originalImagesFolderPath}\{file}");
            Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.DrawImage(image, 0, 0);
            }

            return bitmap;
        }

        public void SaveBitmapFile(string file, Bitmap bitmap)
        {
            File.Copy(
                $@"{originalImagesFolderPath}\{file}",
                $@"{modifiedImagesFolderPath}\{file}",
                true
            );

            bitmap.Save($@"{modifiedImagesFolderPath}\{file}");
        }
    }
}
