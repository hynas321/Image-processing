namespace Image_processing.Records
{
    public record Operations
    {
        public const string Help = "--help";

        #region Task 1
        #region B (elementary operations)
        public const string BrightnessModification = "--brightness";
        public const string ContrastModification = "--contrast";
        public const string Negative = "--negative";
        #endregion

        #region G (geometric operations)
        public const string HorizontalFlip = "--hflip";
        public const string VerticalFlip = "--vflip";
        public const string DiagonalFlip = "--dflip";
        public const string ImageShrinking = "--shrink";
        public const string ImageEnlargement = "--enlarge";
        #endregion

        #region N (noise removal operations)
        public const string MidpointFilter = "--mid";
        public const string ArithmeticMeanFilter = "--amean";
        #endregion

        #region E (analysis of obtained results)
        public const string MeanSquareError = "--mse";
        public const string PeakMeanSquareError = "--pmse";
        public const string SignalToNoiseRatio = "--snr";
        public const string PeakSignalToNoiseRatio = "--psnr";
        public const string MaximumDifference = "--md";
        #endregion
        #endregion

        public static Dictionary<string, string> OperationsDictionary { get; } = new Dictionary<string, string>()
        {
            {Help, "Displays all available commands with description"},
            {$"filename {BrightnessModification} intValue", "Modifies image's brightness"},
            {$"filename {ContrastModification} intValue", "Modifies image's contrast"},
            {$"filename {Negative}", "Negates an image"},
            {$"filename {HorizontalFlip}", "Flips an image horizontally"},
            {$"filename {VerticalFlip}", "Flips an image vertically"},
            {$"Filename {DiagonalFlip}", "Flips an image diagonally"},
            {ImageShrinking, "Shrinks an image"},
            {ImageEnlargement, "Enlarges an image"},
            {MidpointFilter, ""},
            {ArithmeticMeanFilter, ""},
            {MeanSquareError, ""},
            {PeakMeanSquareError, ""},
            {SignalToNoiseRatio, ""},
            {PeakSignalToNoiseRatio, ""},
            {MaximumDifference, ""}

        };
    }
}