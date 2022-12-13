using System.Drawing;
using System.Threading.Tasks;

namespace Image_processing.Managers
{
    //Task 3
    public partial class ProcessingManager
    {
        #region Masks

        //inactive - 0
        //black - 1
        //white - 2
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
                            if (mask[a + 1, b + 1] == 0)
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
                            if (mask[a + 1, b + 1] == 0)
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
            return ApplyDilation(ApplyErosion(bitmap, maskNumber), maskNumber);
        }

        public Bitmap ApplyClosing(Bitmap bitmap, int maskNumber)
        {
            return ApplyErosion(ApplyDilation(bitmap, maskNumber), maskNumber);
        }

        public Bitmap ApplyHmt(Bitmap bitmap, int maskNumber)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            int[,] mask = GetMask(maskNumber);

            for (int x = 1; x < bitmap.Width - 1; x++)
            {
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    bool Hmt = true;

                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if (mask[a + 1, b + 1] == 0)
                            {
                                continue;
                            }

                            if (mask[a + 1, b + 1] == 2 && bitmap.GetPixel(x + a, y + b) != Color.FromArgb(255, 255, 255))
                            {
                                Hmt = false;
                            }
                        }
                    }

                    if (Hmt)
                    {
                        newBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        newBitmap.SetPixel(x, y, bitmap.GetPixel(x, y));
                    }

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
                        {0, 0, 0},
                        {0, 1, 1},
                        {0, 0, 0}
                    };
                case 2:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {0, 1, 0},
                        {0, 1, 0}
                    };
                case 3:
                    return new int[,]
                    {
                        {1, 1, 1},
                        {1, 1, 1},
                        {1, 1, 1}
                    };
                case 4:
                    return new int[,]
                    {
                        {0, 1, 0},
                        {1, 1, 1},
                        {0, 1, 0}
                    };
                case 5:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {0, 1, 1},
                        {0, 1, 0}
                    };
                case 6:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {0, 2, 1},
                        {0, 1, 0}
                    };
                case 7:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {1, 1, 1},
                        {0, 0, 0}
                    };
                case 8:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {1, 2, 1},
                        {0, 0, 0}
                    };
                case 9:
                    return new int[,]
                    {
                        {0, 0, 0},
                        {1, 1, 0},
                        {1, 0, 0}
                    };
                case 10:
                    return new int[,]
                    {
                        {0, 1, 1},
                        {0, 1, 0},
                        {0, 0, 0}
                    };
                case 11:
                    return new int[,]
                    {
                        {1, 0, 0},
                        {1, 2, 0},
                        {1, 0, 0}
                    };
                case 12:
                    return new int[,]
                    {
                        {2, 2, 2},
                        {0, 1, 0},
                        {1, 1, 1}
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(maskNumber));
            }
        }
        #endregion
    }
}

