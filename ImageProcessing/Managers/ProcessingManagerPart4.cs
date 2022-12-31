using System.Drawing;
using System.Numerics;

namespace Image_processing.Managers
{
    public partial class ProcessingManager
    {
        public Bitmap ApplyFourierSpectrumVisualization(List<List<Complex>> image)
        {
            Bitmap visualizationImage = new Bitmap(image[0].Count, image.Count);

            for (int x = 0; x < image[0].Count; x++)
            {
                for (int y = 0; y < image.Count; y++)
                {
                    int calculatedColor = (int)Math.Clamp(Math.Log(Math.Abs(image[y][x].Magnitude)) * 10, 0, 255);
                    Color pixel = Color.FromArgb(1, calculatedColor, calculatedColor, calculatedColor);

                    visualizationImage.SetPixel(x, y, pixel);
                }
            }

            return visualizationImage;
        }

        public (List<List<Complex>> complexNumbers, Bitmap bitmap) ApplyDft(Bitmap bitmap)
        {
            Complex[,] spatialDomain1 = new Complex[bitmap.Height, bitmap.Width];
            Complex[,] spatialDomain2 = new Complex[bitmap.Height, bitmap.Width];
            List<List<Complex>> frequencyDomainOutput = new List<List<Complex>>();

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

            for (int row = 0; row < spatialDomain2.GetLength(0); row++)
            {
                List<Complex> rowList = new List<Complex>();

                for (int column = 0; column < spatialDomain2.GetLength(1); column++)
                {
                    rowList.Add(spatialDomain2[row, column]);
                }

                frequencyDomainOutput.Add(rowList);
            }

            List<List<Complex>> swappedQuarters = ApplyQuartersSwap(frequencyDomainOutput);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(swappedQuarters);

            (List<List<Complex>> complexNumbers, Bitmap bitmap) returnedValues = (frequencyDomainOutput, newBitmap);

            return returnedValues;
        }

        public Bitmap ApplyInverseDft(Bitmap bitmap)
        {
            List<List<Complex>> frequencyDomain = ApplyDft(bitmap).complexNumbers;
            Bitmap newBitmap = new Bitmap(frequencyDomain[0].Count, frequencyDomain.Count);
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

                        sum += frequencyDomain[n][a] * W;
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
                        Color.FromArgb(1, calculatedColor, calculatedColor, calculatedColor)
                    );
                }
            }

            return ApplyDiagonalFlip(newBitmap);
        }

        public (List<List<Complex>> complexNumbers, Bitmap bitmap) ApplyFft(Bitmap bitmap)
        {
            List<List<Complex>> frequencyDomain = new List<List<Complex>>();
            List<List<Complex>> spatialDomain = new List<List<Complex>>();

            //rows
            for (int a = 0; a < bitmap.Height; a++)
            {
                List<Complex> rows = new List<Complex>();

                for (int x = 0; x < bitmap.Width; x++)
                {
                    rows.Add(bitmap.GetPixel(x, a).R);
                }

                spatialDomain.Add(ApplyFft1D(rows));
            }

            //columns
            for (int b = 0; b < bitmap.Width; b++)
            {
                List<Complex> columns = new List<Complex>();

                for (int y = 0; y < bitmap.Height; y++)
                {
                    columns.Add(spatialDomain[y][b]);
                }

                frequencyDomain.Add(ApplyFft1D(columns));
            }

            List<List<Complex>> swappedQuarters = ApplyQuartersSwap(frequencyDomain);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(swappedQuarters);

            newBitmap = ApplyVerticalFlip(newBitmap);
            newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            (List<List<Complex>> complexNumbers, Bitmap bitmap) returnedValues = (frequencyDomain, newBitmap);

            return returnedValues;
        }

        public Bitmap ApplyInverseFft(Bitmap bitmap)
        {
            List<List<Complex>> fourierTransformComplexNumbers = ApplyFft(bitmap).complexNumbers;
            Bitmap newBitmap = new Bitmap(fourierTransformComplexNumbers[0].Count, fourierTransformComplexNumbers.Count);
            List<List<Complex>> values = new List<List<Complex>>();

            for (int x = 0; x < newBitmap.Height; x++)
            {
                values.Add(new List<Complex>());
            }

            for (int y = 0; y < newBitmap.Height; y++)
            {
                List<Complex> rows = new List<Complex>();

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    rows.Add(fourierTransformComplexNumbers[y][x]);
                }

                rows = ApplyInverseFftID(rows);

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    values[y].Add(rows[x] / newBitmap.Width);
                }
            }

            for (int x = 0; x < newBitmap.Width; x++)
            {
                List<Complex> columns = new List<Complex>();

                for (int y = 0; y < newBitmap.Width; y++)
                {
                    columns.Add(values[y][x]);
                }

                columns = ApplyInverseFftID(columns);

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

        public Bitmap ApplyInverseFft(List<List<Complex>> fourierTransformComplexNumbers)
        {
            Bitmap newBitmap = new Bitmap(fourierTransformComplexNumbers[0].Count, fourierTransformComplexNumbers.Count);
            List<List<Complex>> values = new List<List<Complex>>();

            for (int x = 0; x < newBitmap.Height; x++)
            {
                values.Add(new List<Complex>());
            }

            for (int y = 0; y < newBitmap.Height; y++)
            {
                List<Complex> rows = new List<Complex>();

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    rows.Add(fourierTransformComplexNumbers[y][x]);
                }

                rows = ApplyInverseFftID(rows);

                for (int x = 0; x < newBitmap.Width; x++)
                {
                    values[y].Add(rows[x] / newBitmap.Width);
                }
            }

            for (int x = 0; x < newBitmap.Width; x++)
            {
                List<Complex> columns = new List<Complex>();

                for (int y = 0; y < newBitmap.Width; y++)
                {
                    columns.Add(values[y][x]);
                }

                columns = ApplyInverseFftID(columns);

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

        private List<Complex> ApplyInverseFftID(List<Complex> list)
        {
            List<Complex> outputResult = new List<Complex>(list.Count);
            List<Complex> oddComplexNumbers = new List<Complex>(list.Count / 2);
            List<Complex> evenComplexNumbers = new List<Complex>(list.Count / 2);

            if (list.Count == 1)
            {
                return list;
            }

            for (int i = 0; i < list.Count / 2; i++)
            {
                oddComplexNumbers.Add(list[2 * i + 1]);
                evenComplexNumbers.Add(list[2 * i]);
            }

            oddComplexNumbers = ApplyInverseFftID(oddComplexNumbers);
            evenComplexNumbers = ApplyInverseFftID(evenComplexNumbers);

            for (int i = 0; i < list.Count; i++)
            {
                outputResult.Add(0);
            }

            for (int i = 0; i < list.Count / 2; i++)
            {
                Complex number = new Complex(
                    Math.Cos(2 * Math.PI * i / list.Count),
                    Math.Sin(2 * Math.PI * i / list.Count)
                );

                outputResult[i] = evenComplexNumbers[i] + number * oddComplexNumbers[i];
                outputResult[i + list.Count / 2] = evenComplexNumbers[i] - number * oddComplexNumbers[i];
            }

            return outputResult;
        }

        private List<Complex> ApplyFft1D(List<Complex> list)
        {
            List<Complex> outputResult = new List<Complex>(list.Count);
            List<Complex> oddComplexNumbers = new List<Complex>(list.Count / 2);
            List<Complex> evenComplexNumbers = new List<Complex>(list.Count / 2);

            if (list.Count == 1)
            {
                return list;
            }

            for (int i = 0; i < list.Count / 2; i++)
            {
                oddComplexNumbers.Add(list[2 * i + 1]);
                evenComplexNumbers.Add(list[2 * i]);
            }

            oddComplexNumbers = ApplyFft1D(oddComplexNumbers);
            evenComplexNumbers = ApplyFft1D(evenComplexNumbers);

            //Added to fill the List<Complex> outputResult before assignment operations on elements
            for (int i = 0; i < list.Count; i++)
            {
                outputResult.Add(-1);
            }

            for (int i = 0; i < list.Count / 2; i++)
            {
                Complex number = new Complex(
                    Math.Cos(2 * Math.PI * i / list.Count),
                    (-1) * Math.Sin(2 * Math.PI * i / list.Count)
                );

                outputResult[i] = evenComplexNumbers[i] + number * oddComplexNumbers[i];
                outputResult[i + list.Count / 2] = evenComplexNumbers[i] - number * oddComplexNumbers[i];
            }

            return outputResult;
        }

        private List<List<Complex>> ApplyQuartersSwap(List<List<Complex>> list)
        {
            List<List<Complex>> complexNumbersResult = new List<List<Complex>>();

            for (int x = 0; x < list.Count; x++)
            {
                List<Complex> columns = new List<Complex>();

                for (int y = 0; y < list[0].Count; y++)
                {
                    columns.Add(list[x][y]);
                }

                complexNumbersResult.Add(columns);
            }

            for (int x = 0; x < list.Count / 2; x++)
            {
                for (int y = 0; y < list[0].Count / 2; y++)
                {
                    Complex temp = new Complex(complexNumbersResult[x][y].Real, complexNumbersResult[x][y].Imaginary);
                    complexNumbersResult[x][y] = complexNumbersResult[list.Count / 2 + x][list[0].Count / 2 + y];
                    complexNumbersResult[list.Count / 2 + x][list[0].Count / 2 + y] = temp;

                    temp = new Complex(complexNumbersResult[list.Count / 2 + x][y].Real, complexNumbersResult[list.Count / 2 + x][y].Imaginary);
                    complexNumbersResult[list.Count / 2 + x][y] = complexNumbersResult[x][list[0].Count / 2 + y];
                    complexNumbersResult[x][list[0].Count / 2 + y] = temp;
                }
            }

            return complexNumbersResult;
        }

        public Bitmap ApplyLowPassFilter(Bitmap bitmap, int cutOff, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;

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

            if (preservePhase)
            {
                return ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
            }

            return ApplyInverseFft(frequencyDomain);
        }

        public Bitmap ApplyHighPassFilter(Bitmap bitmap, int cutOff, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;
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
                            // Preserve the phase information of the sample
                            double phase = frequencyDomain[x][y].Phase;
                            // Set the magnitude to zero to attenuate the low-frequency component
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
                // Transform the frequency domain representation back to the spatial domain
                return ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
            }

            return ApplyInverseFft(frequencyDomain);
        }

        public Bitmap ApplyBandPassFilter(Bitmap bitmap, int maxThreshold, int minThreshold, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;
            Complex dc = frequencyDomain[width / 2][height / 2];

            frequencyDomain = ApplyQuartersSwap(frequencyDomain);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double value = Math.Sqrt(
                        Math.Pow(x - width / 2.0, 2) +
                        Math.Pow(y - height / 2.0, 2)
                    );

                    if ((value > minThreshold) || (value < maxThreshold))
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

        public Bitmap ApplyBandCutFilter(Bitmap bitmap, int minThreshold, int maxThreshold, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;

            frequencyDomain = ApplyQuartersSwap(frequencyDomain);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double value = Math.Sqrt(
                        Math.Pow(x - width / 2.0, 2) +
                        Math.Pow(y - height / 2.0, 2)
                    );

                    if ((value >= minThreshold) && (value <= maxThreshold))
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

            if (preservePhase)
            {
                return ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
            }

            return ApplyInverseFft(frequencyDomain);
        }

        public Bitmap ApplyHighPassEdgeDetectionFilter(Bitmap bitmap, Bitmap mask, int threshold, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mask.GetPixel(x, y).R == 0)
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
                    else if (Math.Sqrt(
                        Math.Pow(x - width / 2.0, 2) +
                        Math.Pow(y - height / 2.0, 2)
                    ) < threshold)
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

            if (preservePhase)
            {
                Bitmap result = ApplyInverseFft(ApplyFourierSpectrumVisualization(frequencyDomain));
                result.RotateFlip(RotateFlipType.Rotate90FlipNone);

                return result;
            }

            return ApplyInverseFft(frequencyDomain);
        }

        public Bitmap ApplyPhaseModifying(Bitmap bitmap, int k, int l, bool preservePhase)
        {
            List<List<Complex>> frequencyDomain = ApplyFft(bitmap).complexNumbers;
            int width = frequencyDomain.Count;
            int height = frequencyDomain[0].Count;
            Complex dc = frequencyDomain[width / 2][height / 2];

            frequencyDomain = ApplyQuartersSwap(frequencyDomain);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    frequencyDomain[x][y] = ApplyPhaseMask(frequencyDomain[x][y], x, y, k, l);
                }
            }

            frequencyDomain[width / 2][height / 2] = dc;

            return ApplyInverseFft(frequencyDomain);
        }

        private Complex ApplyPhaseMask(Complex number, int x, int y, int k, int l)
        {
            int j = 1;
            double formula = Math.Pow(
                Math.E,
                j * (((-1) * (x * k * 2 * Math.PI) / x) + (-1) * (y * l * 2 * Math.PI / y) + (k + l) * Math.PI)
            );

            return new Complex(number.Real * formula, number.Imaginary * formula);
        }
    }
}
