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
        #endregion
        #endregion

        public string[] OperationsArray { get; } =
        {
            Help,
            BrightnessModification, ContrastModification, Negative,
            MeanSquareError, PeakMeanSquareError,
            HorizontalFlip, VerticalFlip, DiagonalFlip, ImageShrinking, ImageEnlargement,
            MidpointFilter, ArithmeticMeanFilter
        };
    }
}