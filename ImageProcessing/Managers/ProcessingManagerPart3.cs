using System.Drawing;
using System.Threading.Tasks;

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

                            if (mask[a + 1, b + 1] != bitmap.GetPixel(x + a, y + b).R / 255)
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

        public Bitmap ApplyM1Operation(Bitmap bitmap1, Bitmap bitmap2, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap1.Width, bitmap1.Height);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap1.Width - 1; x++)
            {
                for (int y = 1; y < bitmap1.Height - 1; y++)
                {
                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == -1)
                            {
                                continue;
                            }

                            Color pixel2 = bitmap2.GetPixel(x + a, y + b);

                            int redColorValue2 = pixel2.R;
                            int greenColorValue2 = pixel2.G;
                            int blueColorValue2 = pixel2.B;

                            if (redColorValue2 == 0 || greenColorValue2 == 0 || blueColorValue2 == 0)
                            {
                                redColorValue2 = 1;
                                greenColorValue2 = 1;
                                blueColorValue2 = 1;
                            }

                            Color pixel1 = bitmap1.GetPixel(x + a, y + b);

                            int redColorValue1 = pixel1.R;
                            int greenColorValue1 = pixel1.G;
                            int blueColorValue1 = pixel1.B;

                            int newRedColorValue = redColorValue1 / redColorValue2;
                            int newGreenColorValue = greenColorValue1 / greenColorValue2;
                            int newBlueColorValue = blueColorValue1 / blueColorValue2;

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

        public Bitmap ApplyMerging(Bitmap bitmap, int x, int y, int threshold)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            Stack<Point> pointStack = new Stack<Point>();

            bool[,] checkedPoints = new bool[bitmap.Width, bitmap.Height];
            int maxPixelValueR = Math.Clamp(bitmap.GetPixel(x, y).R + threshold, 0, 255);
            int maxPixelValueG = Math.Clamp(bitmap.GetPixel(x, y).G + threshold, 0, 255);
            int maxPixelValueB = Math.Clamp(bitmap.GetPixel(x, y).B + threshold, 0, 255);
            int minPixelValueR = Math.Clamp(bitmap.GetPixel(x, y).R - threshold, 0, 255);
            int minPixelValueG = Math.Clamp(bitmap.GetPixel(x, y).R - threshold, 0, 255);
            int minPixelValueB = Math.Clamp(bitmap.GetPixel(x, y).R - threshold, 0, 255);

            pointStack.Push(new Point(x, y));

            while (true)
            {
                if (pointStack.Count == 0)
                {
                    break;
                }

                Point point = pointStack.Pop();

                if (point.X >= 0 && point.X < bitmap.Width &&
                    point.Y >= 0 && point.Y < bitmap.Height &&
                    !checkedPoints[point.X, point.Y])
                {
                    checkedPoints[point.X, point.Y] = true;

                    if (bitmap.GetPixel(point.X, point.Y).R >= minPixelValueR &&
                        bitmap.GetPixel(point.X, point.Y).G >= minPixelValueG &&
                        bitmap.GetPixel(point.X, point.Y).B >= minPixelValueB &&
                        bitmap.GetPixel(point.X, point.Y).R < maxPixelValueR &&
                        bitmap.GetPixel(point.X, point.Y).G < maxPixelValueG &&
                        bitmap.GetPixel(point.X, point.Y).B < maxPixelValueB)
                    {
                        newBitmap.SetPixel(point.X, point.Y, Color.FromArgb(255, 255, 255));

                        pointStack.Push(new Point(point.X, point.Y - 1));
                        pointStack.Push(new Point(point.X, point.Y + 1));
                        pointStack.Push(new Point(point.X - 1, point.Y));
                        pointStack.Push(new Point(point.X + 1, point.Y));
                    }
                }
                else
                {
                    bitmap.SetPixel(point.X, point.Y, Color.FromArgb(0, 0, 0));
                }
            }

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
                //XI
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
                        {  1,  1,  1 },
                        { -1,  0, -1 },
                        { -1, -1, -1 }
                    };
                case 13:
                    return new int[,]
                    {
                        { -1, -1,  1 },
                        { -1,  0,  1 },
                        { -1, -1,  1 }
                    };
                case 14:
                    return new int[,]
                    {
                        { -1, -1, -1 },
                        { -1,  0, -1 },
                        {  1,  1,  1 }
                    };
                //XII
                case 15:
                    return new int[,]
                    {
                        {  0,  0,  0 },
                        { -1,  1, -1 },
                        {  1,  1,  1 }
                    };
                case 16:
                    return new int[,]
                    {
                        { -1,  0,  0 },
                        {  1,  1,  0 },
                        {  1,  1, -1 }
                    };
                case 17:
                    return new int[,]
                    {
                        { 1,  -1,  0 },
                        { 1,   1,  0 },
                        { 1,  -1,  0 }
                    };
                case 18:
                    return new int[,]
                    {
                        {  1,  1, -1 },
                        {  1,  1,  0 },
                        { -1,  0,  0 }
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(maskNumber));
            }
        }
        #endregion
    }
}

