using System.Drawing;

namespace Image_processing.Managers
{
    //Task 3
    public partial class ProcessingManager
    {
        #region Masks

        //inactive: -1
        //black - 1
        //white - 0
        #endregion

        public Bitmap ApplyDilation(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int[,] mask = GetMask(maskNumber);

            for (int y = 1; y < bitmap.Height - 1; y++)
            {
                for (int x = 1; x < bitmap.Width - 1; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    int redColorValue = pixel.R;
                    int greenColorValue = pixel.G;
                    int blueColorValue = pixel.B;

                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel2 = bitmap.GetPixel(x + a, y + b);

                            redColorValue = Math.Max(redColorValue, pixel2.R);
                            greenColorValue = Math.Max(greenColorValue, pixel2.G);
                            blueColorValue = Math.Max(blueColorValue, pixel2.B);
                        }
                    }

                    newBitmap.SetPixel(x, y, Color.FromArgb(0 + redColorValue, 0 + greenColorValue, 0 + blueColorValue));
                }
            }

            return newBitmap;
        }

        public Bitmap ApplyErosion(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int[,] mask = GetMask(maskNumber);

            for (int y = 1; y < bitmap.Height - 1; y++)
            {
                for (int x = 1; x < bitmap.Width - 1; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    int redColorValue = pixel.R;
                    int greenColorValue = pixel.G;
                    int blueColorValue = pixel.B;

                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel2 = bitmap.GetPixel(x + a, y + b);

                            redColorValue = Math.Min(redColorValue, pixel2.R);
                            greenColorValue = Math.Min(greenColorValue, pixel2.G);
                            blueColorValue = Math.Min(blueColorValue, pixel2.B);
                        }
                    }

                    newBitmap.SetPixel(x, y, Color.FromArgb(0 + redColorValue, 0 + greenColorValue, 0 + blueColorValue));
                }
            }

            return newBitmap;
        }

        public Bitmap ApplyOpening(Bitmap bitmap, int maskNumber)
        {
            Bitmap erosedBitmap = ApplyErosion(bitmap, maskNumber);

            return ApplyDilation(erosedBitmap, maskNumber);
        }

        public Bitmap ApplyClosing(Bitmap bitmap, int maskNumber)
        {
            Bitmap dilatedBitmap = ApplyDilation(bitmap, maskNumber);

            return ApplyErosion(dilatedBitmap, maskNumber);
        }

        public Bitmap ApplyHmt(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    bool Hmt = true;

                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            if (mask[a + 1, b + 1] * 255 != bitmap.GetPixel(x + a, y + b).R)
                            {
                                Hmt = false;
                            }
                        }
                    }

                    if (Hmt)
                    {
                        newBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        newBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }

                }
            }

            return newBitmap;
        }

        public Bitmap ApplyM1Operation1(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Bitmap dilatedBitmap = ApplyDilation(bitmap, maskNumber);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel = bitmap.GetPixel(x + a, y + b);

                            int initialRedColorValue = pixel.R;
                            int initialGreenColorValue = pixel.G;
                            int initialBlueColorValue = pixel.B;

                            if (initialRedColorValue == 0 || initialGreenColorValue == 0 || initialBlueColorValue == 0)
                            {
                                initialRedColorValue = 1;
                                initialGreenColorValue = 1;
                                initialBlueColorValue = 1;
                            }

                            Color pixel2 = dilatedBitmap.GetPixel(x + a, y + b);

                            int dilatedRedColorValue = pixel2.R;
                            int dilatedGreenColorValue = pixel2.G;
                            int dilatedBlueColorValue = pixel2.B;

                            int newRedColorValue = dilatedRedColorValue / initialRedColorValue;
                            int newGreenColorValue = dilatedGreenColorValue / initialGreenColorValue;
                            int newBlueColorValue = dilatedBlueColorValue / initialBlueColorValue;

                            newBitmap.SetPixel(x, y, Color.FromArgb(
                                Math.Clamp(newRedColorValue, 0, 255),
                                Math.Clamp(newGreenColorValue, 0, 255),
                                Math.Clamp(newBlueColorValue, 0, 255)
                                )
                            );
                        }
                    }
                }
            }

            return newBitmap;

        }

        public Bitmap ApplyM1Operation2(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Bitmap erosedBitmap = ApplyErosion(bitmap, maskNumber);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel = erosedBitmap.GetPixel(x + a, y + b);

                            int erosedRedColorValue = pixel.R;
                            int erosedlGreenColorValue = pixel.G;
                            int erosedBlueColorValue = pixel.B;

                            if (erosedRedColorValue == 0 || erosedlGreenColorValue == 0 || erosedBlueColorValue == 0)
                            {
                                erosedRedColorValue = 1;
                                erosedlGreenColorValue = 1;
                                erosedBlueColorValue = 1;
                            }

                            Color pixel2 = bitmap.GetPixel(x + a, y + b);

                            int initialRedColorValue = pixel2.R;
                            int initialGreenColorValue = pixel2.G;
                            int initialBlueColorValue = pixel2.B;

                            int newRedColorValue = initialRedColorValue / erosedRedColorValue;
                            int newGreenColorValue = initialGreenColorValue / erosedlGreenColorValue;
                            int newBlueColorValue = initialBlueColorValue / erosedBlueColorValue;

                            newBitmap.SetPixel(x, y, Color.FromArgb(
                                Math.Clamp(newRedColorValue, 0, 255),
                                Math.Clamp(newGreenColorValue, 0, 255),
                                Math.Clamp(newBlueColorValue, 0, 255)
                                )
                            );
                        }
                    }
                }
            }

            return newBitmap;
        }

        public Bitmap ApplyM1Operation3(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Bitmap dilatedBitmap = ApplyDilation(bitmap, maskNumber);
            Bitmap erosedBitmap = ApplyErosion(bitmap, maskNumber);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel = erosedBitmap.GetPixel(x + a, y + b);

                            int erosedRedColorValue = pixel.R;
                            int erosedGreenColorValue = pixel.G;
                            int erosedBlueColorValue = pixel.B;

                            if (erosedRedColorValue == 0 || erosedGreenColorValue == 0 || erosedBlueColorValue == 0)
                            {
                                erosedRedColorValue = 1;
                                erosedGreenColorValue = 1;
                                erosedBlueColorValue = 1;
                            }

                            Color pixel2 = dilatedBitmap.GetPixel(x + a, y + b);

                            int dilatedRedColorValue = pixel2.R;
                            int dilatedGreenColorValue = pixel2.G;
                            int dilatedBlueColorValue = pixel2.B;

                            int newRedColorValue = dilatedRedColorValue / erosedRedColorValue;
                            int newGreenColorValue = dilatedGreenColorValue / erosedGreenColorValue;
                            int newBlueColorValue = dilatedBlueColorValue / erosedBlueColorValue;

                            newBitmap.SetPixel(x, y, Color.FromArgb(
                                Math.Clamp(newRedColorValue, 0, 255),
                                Math.Clamp(newGreenColorValue, 0, 255),
                                Math.Clamp(newBlueColorValue, 0, 255)
                                )
                            );
                        }
                    }
                }
            }

            return newBitmap;
        }

        public Bitmap ApplyMerging(Bitmap bitmap1, Bitmap bitmap2)
        {
            Bitmap newBitmap = new Bitmap(bitmap1.Width, bitmap1.Height);

            return newBitmap;
        }

        #region Task 3 private methods

        public int[,] GetMask(int maskNumber)
        {
            switch (maskNumber)
            {
                case 1:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        { -1,  1,  1 },
                        { -1, -1, -1 }
                    };
                case 2:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        { -1,  1, -1 },
                        { -1,  1, -1 }
                    };
                case 3:
                    return new int[,]
                    {
                        {  1,  1,  1 },
                        {  1,  1,  1 },
                        {  1,  1,  1 }
                    };
                case 4:
                    return new int[,]
                    {
                        { -1,  1, -1 },
                        {  1,  1,  1 },
                        { -1,  1, -1 }
                    };
                case 5:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        { -1,  1,  1 },
                        { -1,  1, -1 }
                    };
                case 6:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        { -1,  0,  1 },
                        { -1,  1, -1 }
                    };
                case 7:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        {  1,  1,  1 },
                        { -1, -1, -1 }  
                    };
                case 8:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        {  1,  0,  1 },
                        { -1, -1, -1 }
                    };
                case 9:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        {  1,  1, -1 },
                        {  1, -1, -1 }
                    };
                case 10:
                    return new int[,]
                    {
                        { -1,  1,  1 },
                        { -1,  1, -1 },
                        { -1, -1, -1 }
                    };
                case 11:
                    return new int[,]
                    {
                        {  1, -1, -1 },
                        {  1,  0, -1 },
                        {  1, -1, -1 }
                    };
                case 12:
                    return new int[,]
                    {
                        {  0,  0,  0 },
                        { -1,  1, -1 },
                        {  1,  1,  1 }
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(maskNumber));
            }
        }
        #endregion
    }
}

