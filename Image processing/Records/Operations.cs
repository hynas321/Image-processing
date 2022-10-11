namespace Image_processing.Records
{
    public record Operations
    {
        public static string Help { get; } = "--help";

        #region Task 1
        #region B (elementary operations)
        public static string BrightnessModification { get; } = "--brightness";
        public static string ContrastModification { get; } = "--contrast";
        public static string Negative { get; } = "--negative";
        #endregion

        #region G (geometric operations)
        public static string HorizontalFlip { get; } = "--hflip";
        public static string VerticalFlip { get; } = "--vflip";
        public static string DiagonalFlip { get; } = "--dflip";
        public static string ImageShrinking { get; } = "--shrink";
        public static string ImageEnlargement { get; } = "--enlarge";
        #endregion

        #region N (noise removal operations)
        public static string MidpointFilter { get; } = "--mid";
        public static string ArithmeticMeanFilter { get; } = "--amean";
        #endregion

        #region E (analysis of obtained results)
        public static string MeanSquareError { get; } = "--mse";
        public static string PeakMeanSquareError { get; } = "--pmse";
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