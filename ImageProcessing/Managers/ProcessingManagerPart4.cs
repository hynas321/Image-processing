using System.Drawing;
using System.Numerics;

namespace Image_processing.Managers
{
    public partial class ProcessingManager
    {
        #region DFT

        public (Complex[,] complexNumbers, Bitmap bitmap) ApplyDft(Bitmap bitmap)
        {
            Complex[,] spatialDomain1 = new Complex[bitmap.Height, bitmap.Width];
            Complex[,] spatialDomain2 = new Complex[bitmap.Height, bitmap.Width];

            //columns
            //Dft 1D
            for (int a = 0; a < bitmap.Height; a++)
            {
                for (int k = 0; k < bitmap.Width; k++)
                {
                    Complex sum = new Complex(0, 0);

                    for (int n = 0; n < bitmap.Width; n++)
                    {
                        Complex W = new Complex(
                            Math.Cos(2 * Math.PI * n * k / bitmap.Width),
                            (-1) * Math.Sin(2 * Math.PI * n * k / bitmap.Width));

                        sum += bitmap.GetPixel(n, a).R * W;
                    }

                    spatialDomain1[a, k] = sum;
                }
            }

            //rows
            //Dft 1D
            for (int b = 0; b < bitmap.Width; b++)
            {
                for (int k = 0; k < bitmap.Height; k++)
                {
                    Complex sum = new Complex(0, 0);

                    for (int n = 0; n < bitmap.Height; n++)
                    {
                        Complex W = new Complex(
                            Math.Cos(2 * Math.PI * n * k / bitmap.Height),
                            (-1) * Math.Sin(2 * Math.PI * n * k / bitmap.Height)
                        );

                        sum += spatialDomain1[n, b] * W;
                    }

                    spatialDomain2[k, b] = sum;
                }
            }

            Complex[,] swappedQuarters = ApplyQuartersSwap(spatialDomain2);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(swappedQuarters);

            (Complex[,] complexNumbers, Bitmap bitmap) returnedValues = (spatialDomain2, newBitmap);

            return returnedValues;
        }

        public Bitmap ApplyInverseDft(Bitmap bitmap)
        {
            (Complex[,] complexNumbers, Bitmap bitmap) frequencyDomain = ApplyDft(bitmap);
            Bitmap newBitmap = new Bitmap(frequencyDomain.complexNumbers.GetLength(1), frequencyDomain.complexNumbers.Length);
            Complex[,] spatialDomain = new Complex[newBitmap.Height, newBitmap.Width];

            //columns
            for (int a = 0; a < newBitmap.Width; a++)
            {
                for (int k = 0; k < newBitmap.Height; k++)
                {
                    Complex sum = new Complex(0, 0);

                    for (int n = 0; n < newBitmap.Height; n++)
                    {
                        Complex W = new Complex(
                            Math.Cos(2 * Math.PI * n * k / newBitmap.Height),
                            (-1) * Math.Sin(2 * Math.PI * n * k / newBitmap.Height)
                        );

                        sum += frequencyDomain.complexNumbers[n,a] * W;
                    }

                    spatialDomain[k, a] = sum / newBitmap.Height;
                }
            }

            //rows
            for (int b = 0; b < newBitmap.Height; b++)
            {
                for (int k = 0; k < newBitmap.Width; k++)
                {
                    Complex sum = new Complex(0, 0);

                    for (int n = 0; n < newBitmap.Width; n++)
                    {
                        Complex W = new Complex(
                            Math.Cos(2 * Math.PI * n * k / newBitmap.Width),
                            -Math.Sin(2 * Math.PI * n * k / newBitmap.Width)
                        );

                        sum += spatialDomain[b, n] * W;
                    }

                    int calculatedColor = (int)Math.Clamp(sum.Magnitude / newBitmap.Width, 0, 255);

                    newBitmap.SetPixel(
                        (k - 1 + newBitmap.Width) % newBitmap.Width,
                        (b - 1 + newBitmap.Height) % newBitmap.Height,
                        Color.FromArgb( calculatedColor, calculatedColor, calculatedColor)
                    );
                }
            }

            return newBitmap;
        }


        #endregion

        #region Fourier Transform Helper Methods

        private Complex[,] ApplyQuartersSwap(Complex[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            Complex[,] complexNumbersResult = new Complex[rows, columns];
            Array.Copy(array, complexNumbersResult, array.Length);

            for (int x = 0; x < rows / 2; x++)
            {
                for (int y = 0; y < columns / 2; y++)
                {
                    Complex temp = complexNumbersResult[x, y];
                    complexNumbersResult[x, y] = complexNumbersResult[rows / 2 + x, columns / 2 + y];
                    complexNumbersResult[rows / 2 + x, columns / 2 + y] = temp;

                    temp = complexNumbersResult[rows / 2 + x, y];
                    complexNumbersResult[rows / 2 + x, y] = complexNumbersResult[x, columns / 2 + y];
                    complexNumbersResult[x, columns / 2 + y] = temp;
                }
            }
            return complexNumbersResult;
        }
        
        
        public Bitmap ApplyFourierSpectrumVisualization(Complex[,] image)
        {
            int height = image.GetLength(0);
            int width = image.GetLength(1);
            Bitmap visualizationImage = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int calculatedColor = (int)Math.Clamp(Math.Log(Math.Abs(image[y, x].Magnitude)) * 10, 0, 255);
                    Color pixel = Color.FromArgb(calculatedColor, calculatedColor, calculatedColor);

                    visualizationImage.SetPixel(x, y, pixel);
                }
            }

            return visualizationImage;
        }
        
        #region FFT

        public (Complex[,] frequencyDomain, Bitmap bitmap) ApplyFft(Bitmap bitmap)
        {
            Complex[,] frequencyDomain = new Complex[bitmap.Width, bitmap.Height];
            Complex[][] spatialDomain = new Complex[bitmap.Height][];

            //rows
            for (int a = 0; a < bitmap.Height; a++)
            {
                Complex[] rows = new Complex[bitmap.Width];

                for (int x = 0; x < bitmap.Width; x++)
                {
                    rows[x] = bitmap.GetPixel(x, a).R;
                }

                spatialDomain[a] = ApplyFft1D(rows);
            }

            //columns
            for (int b = 0; b < bitmap.Width; b++)
            {
                Complex[] columns = new Complex[bitmap.Height];

                for (int y = 0; y < bitmap.Height; y++)
                {
                    columns[y] = spatialDomain[y][b];
                }

                Complex[] colFft = ApplyFft1D(columns);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    frequencyDomain[b, y] = colFft[y];
                }
            }

            Complex[,] swappedQuarters = ApplyQuartersSwap(frequencyDomain);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(swappedQuarters);

            newBitmap = ApplyVerticalFlip(newBitmap);
            newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            (Complex[,] complexNumbers, Bitmap bitmap) returnedValues = (frequencyDomain, newBitmap);

            return returnedValues;
        }
        
        private Complex[] ApplyFft1D(Complex[] list)
        {
            Complex[] outputResult = new Complex[list.Length];
            Complex[] oddComplexNumbers = new Complex[list.Length / 2];
            Complex[] evenComplexNumbers = new Complex[list.Length / 2];
            //if input data consits of only one sample
            if (list.Length == 1)
            {
                return list;
            }

            //input data divided into two sets: even and odd
            for (int i = 0; i < list.Length / 2; i++)
            {
                oddComplexNumbers[i] = list[2 * i + 1];
                evenComplexNumbers[i] = list[2 * i];
            }

            //recursion applied to both sets
            oddComplexNumbers = ApplyFft1D(oddComplexNumbers);
            evenComplexNumbers = ApplyFft1D(evenComplexNumbers);

            //butterfly operations
            for (int i = 0; i < list.Length / 2; i++)
            {
                Complex number = new Complex(
                    Math.Cos(2 * Math.PI * i / list.Length),
                    (-1) * Math.Sin(2 * Math.PI * i / list.Length)
                );

                outputResult[i] = evenComplexNumbers[i] + number * oddComplexNumbers[i];
                outputResult[i + list.Length / 2] = evenComplexNumbers[i] - number * oddComplexNumbers[i];
            }

            return outputResult;
        }

        public Bitmap ApplyInverseFft(Bitmap bitmap)
        {
            Complex[,] fourierTransformComplexNumbers = ApplyFft(bitmap).frequencyDomain;
            Bitmap newBitmap = new Bitmap(fourierTransformComplexNumbers.GetLength(1), 
                fourierTransformComplexNumbers.GetLength(0));
            Complex[,] values = new Complex[newBitmap.Height, newBitmap.Width];
            for (int y = 0; y < newBitmap.Height; y++)
            {
                Complex[] rows = new Complex[newBitmap.Width];

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    rows[x] = fourierTransformComplexNumbers[y, x];
                }

                rows = ApplyInverseFft1D(rows);

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    values[y, x] = rows[x] / newBitmap.Width;
                }
            }

            for (int x = 0; x < newBitmap.Width; x++)
            {
                Complex[] columns = new Complex[newBitmap.Height];

                for (int y = 0; y < newBitmap.Height; y++)
                {
                    columns[y] = values[y, x];
                }

                columns = ApplyInverseFft1D(columns);

                for (int y = 0; y < newBitmap.Height; y++)
                {
                    int color = (int)Math.Clamp(Math.Abs(columns[y].Magnitude / newBitmap.Width), 0, 255);
                    Color pixel = Color.FromArgb( color, color, color);

                    newBitmap.SetPixel(x, y, pixel);
                }
            }

            newBitmap = ApplyVerticalFlip(newBitmap);
            newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            return newBitmap;
        }

        public Bitmap ApplyInverseFft(Complex[,] fourierTransformComplexNumbers)
        {
            Bitmap newBitmap = new Bitmap(fourierTransformComplexNumbers.GetLength(1),
                fourierTransformComplexNumbers.GetLength(0));
            Complex[,] values = new Complex[newBitmap.Height, newBitmap.Width];
            for (int y = 0; y < newBitmap.Height; y++)
            {
                Complex[] rows = new Complex[newBitmap.Width];

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    rows[x] = fourierTransformComplexNumbers[y, x];
                }

                rows = ApplyInverseFft1D(rows);

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    values[y, x] = rows[x] / newBitmap.Width;
                }
            }

            for (int x = 0; x < newBitmap.Width; x++)
            {
                Complex[] columns = new Complex[newBitmap.Height];

                for (int y = 0; y < newBitmap.Height; y++)
                {
                    columns[y] = values[y, x];
                }

                columns = ApplyInverseFft1D(columns);

                for (int y = 0; y < newBitmap.Height; y++)
                {
                    int color = (int)Math.Clamp(Math.Abs(columns[y].Magnitude / newBitmap.Width), 0, 255);
                    Color pixel = Color.FromArgb(1, color, color, color);

                    newBitmap.SetPixel(x, y, pixel);
                }
            }

            newBitmap = ApplyVerticalFlip(newBitmap);
            newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            return newBitmap;
        }

        private Complex[] ApplyInverseFft1D(Complex[] fourierTransformComplexNumbers)
        {
            Complex[] outputResult = new Complex[fourierTransformComplexNumbers.Length];
            Complex[] oddComplexNumbers = new Complex[fourierTransformComplexNumbers.Length / 2];
            Complex[] evenComplexNumbers = new Complex[fourierTransformComplexNumbers.Length / 2];

            if (fourierTransformComplexNumbers.Length == 1)
            {
                return fourierTransformComplexNumbers;
            }

            for (int i = 0; i < fourierTransformComplexNumbers.Length / 2; i++)
            {
                oddComplexNumbers[i] = fourierTransformComplexNumbers[2 * i + 1];
                evenComplexNumbers[i] = fourierTransformComplexNumbers[2 * i];
            }

            oddComplexNumbers = ApplyInverseFft1D(oddComplexNumbers);
            evenComplexNumbers = ApplyInverseFft1D(evenComplexNumbers);

            for (int i = 0; i < fourierTransformComplexNumbers.Length / 2; i++)
            {
                Complex number = new Complex(
                    Math.Cos(2 * Math.PI * i / fourierTransformComplexNumbers.Length),
                    Math.Sin(2 * Math.PI * i / fourierTransformComplexNumbers.Length)
                );
                outputResult[i] = evenComplexNumbers[i] + number * oddComplexNumbers[i];
                outputResult[i + fourierTransformComplexNumbers.Length / 2] = evenComplexNumbers[i] - number * oddComplexNumbers[i];

            }

            return outputResult;
        }

        #endregion

        #endregion

        #region filters

        public Bitmap ApplyLowPassFilter(Bitmap bitmap, int cutOff, char preservePhase)
        {
            Complex[,] frequencyDomain = ApplyFft(bitmap).frequencyDomain;
            int width = frequencyDomain.GetLength(0);
            int height = frequencyDomain.GetLength(1);
            frequencyDomain = ApplyQuartersSwap(frequencyDomain);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double distance = Math.Sqrt(
                        Math.Pow((x - width / 2), 2) +
                        Math.Pow((y - height / 2), 2)
                    );

                    if (distance > cutOff)
                    {
                        if (preservePhase == 'y')
                        {
                            double phase = frequencyDomain[x, y].Phase;
                            frequencyDomain[x, y] = new Complex(0, phase);
                        }
                        else
                        {
                            frequencyDomain[x, y] = new Complex(0, 0);
                        }
                    }
                }
            }

            if (preservePhase == 'y')
            {
                return ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
            }
            else
            {

                return ApplyInverseFft(frequencyDomain);
            }
        }

   /*     public Bitmap ApplyHighPassFilter(Bitmap bitmap, int cutOff, bool preservePhase)
        {
            Complex[,] frequencyDomain = ApplyFft(bitmap).frequencyDomain;
            int width = frequencyDomain.GetLength(0);
            int height = frequencyDomain.GetLength(1);
            Complex dc = frequencyDomain[width / 2][height / 2];

            frequencyDomain = ApplyQuartersSwap(frequencyDomain);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double distance = Math.Sqrt(
                        Math.Pow((x - width / 2), 2) +
                        Math.Pow((y - height / 2), 2)
                    );

                    if (distance < cutOff)
                    {
                        if (preservePhase)
                        {
                            double phase = frequencyDomain[x][y].Phase;
                            frequencyDomain[x][y] = new Complex(0, phase);
                        }
                        else
                        {
                            frequencyDomain[x][y] = new Complex(0, 0);
                        }
                    }
                }
            }

            frequencyDomain[width / 2][height / 2] = dc;

            if (preservePhase)
            {
                return ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
            }

            return ApplyInverseFft(frequencyDomain);
        }


*/



        #endregion
        

    }
}