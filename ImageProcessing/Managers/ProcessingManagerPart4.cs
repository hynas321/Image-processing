using System.Drawing;
using System.Numerics;

namespace Image_processing.Managers
{
    public partial class ProcessingManager
    {
        public (List<List<Complex>> complexNumbers, Bitmap bitmap) ApplySlowFourierTransform(Bitmap bitmap)
        {
            Complex[,] complexNumbers1 = new Complex[bitmap.Height, bitmap.Width];
            Complex[,] complexNumbers2 = new Complex[bitmap.Height, bitmap.Width];
            List<List<Complex>> slowFourierTransformOutput = new List<List<Complex>>();

            //columns
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

                    complexNumbers1[a, k] = sum;
                }
            }

            //rows
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

                        sum += complexNumbers1[n, b] * W;
                    }

                    complexNumbers2[k, b] = sum;
                }
            }

            for (int row = 0; row < complexNumbers2.GetLength(0); row++)
            {
                List<Complex> rowList = new List<Complex>();

                for (int column = 0; column < complexNumbers2.GetLength(1); column++)
                {
                    rowList.Add(complexNumbers2[row, column]);
                }

                slowFourierTransformOutput.Add(rowList);
            }

            List<List<Complex>> swappedQuarters = ApplyQuartersSwap(slowFourierTransformOutput);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(slowFourierTransformOutput);

            (List<List<Complex>> complexNumbers, Bitmap bitmap) returnedValues = (slowFourierTransformOutput, newBitmap);

            return returnedValues;
        }

        public Bitmap ApplyInverseSlowFourierTransform(Bitmap bitmap)
        {
            List<List<Complex>> fourierTransformComplexNumbers = ApplySlowFourierTransform(bitmap).complexNumbers;
            Bitmap newBitmap = new Bitmap(fourierTransformComplexNumbers[0].Count, fourierTransformComplexNumbers.Count);
            Complex[,] complexNumbers = new Complex[newBitmap.Height, newBitmap.Width];

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

                        sum += fourierTransformComplexNumbers[n][a] * W;
                    }

                    complexNumbers[k, a] = sum / newBitmap.Height;
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

                        sum += complexNumbers[b, n] * W;
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


        public (List<List<Complex>> complexNumbers, Bitmap bitmap) ApplyFastFourierTransform(Bitmap bitmap)
        {
            List<List<Complex>> fastFourierTransformOutput = new List<List<Complex>>();
            List<List<Complex>> complexNumbers = new List<List<Complex>>();

            //rows
            for (int a = 0; a < bitmap.Height; a++)
            {
                List<Complex> rows = new List<Complex>();

                for (int x = 0; x < bitmap.Width; x++)
                {
                    rows.Add(bitmap.GetPixel(x, a).R);
                }

                complexNumbers.Add(ApplyFastFourierTransform1D(rows));
            }

            //columns
            for (int b = 0; b < bitmap.Width; b++)
            {
                List<Complex> columns = new List<Complex>();

                for (int y = 0; y < bitmap.Height; y++)
                {
                    columns.Add(complexNumbers[y][b]);
                }

                fastFourierTransformOutput.Add(ApplyFastFourierTransform1D(columns));
            }

            List<List<Complex>> swappedQuarters = ApplyQuartersSwap(fastFourierTransformOutput);
            Bitmap newBitmap = ApplyFourierSpectrumVisualization(swappedQuarters);

            newBitmap = ApplyVerticalFlip(newBitmap);
            newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

            (List<List<Complex>> complexNumbers, Bitmap bitmap) returnedValues = (fastFourierTransformOutput, newBitmap);

            return returnedValues;
        }

        public Bitmap ApplyInverseFastFourierTransform(Bitmap bitmap)
        {
            List<List<Complex>> fourierTransformComplexNumbers = ApplyFastFourierTransform(bitmap).complexNumbers;
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

                rows = ApplyInverseFastFourierTransform1D(rows);

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

                columns = ApplyInverseFastFourierTransform1D(columns);

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

        private List<Complex> ApplyInverseFastFourierTransform1D(List<Complex> list)
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

            oddComplexNumbers = ApplyInverseFastFourierTransform1D(oddComplexNumbers);
            evenComplexNumbers = ApplyInverseFastFourierTransform1D(evenComplexNumbers);

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

        private List<Complex> ApplyFastFourierTransform1D(List<Complex> list)
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

            oddComplexNumbers = ApplyFastFourierTransform1D(oddComplexNumbers);
            evenComplexNumbers = ApplyFastFourierTransform1D(evenComplexNumbers);

            for (int i = 0; i < list.Count; i++)
            {
                outputResult.Add(0);
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
                List<Complex> column = new List<Complex>();

                for (int y = 0; y < list[0].Count; y++)
                {
                    column.Add(list[x][y]);
                }

                complexNumbersResult.Add(column);
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
    }
}
