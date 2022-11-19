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

        #region Task 2
        public const string Histogram = "--histogram";

        #region H (histogram calculation algorithm)
        public const string RaleighFinalProbabilityDensityFunction = "--hraleigh";
        #endregion

        #region C (image characteristics)
        public const string Mean = "--cmean";
        public const string Variance = "--cvariance";
        public const string StandardDeviation = "--cstdev";
        public const string VariationCoefficientI = "--cvarcoi";
        public const string VariationCoefficientII = "--cvarcoii";
        public const string InformationSourceEntropy = "--centropy";
        #endregion

        #region S (linear image filtration algorithm in spatial domain basing on convolution)
        public const string ExtractionOfDetailsI = "--sexdeti";
        #endregion

        #region O (non-linear image filtration algorithm in spatial domain)
        public const string RosenfeldOperator = "--orosenfeld";
        #endregion
        #endregion

        public static Dictionary<string, string> OperationsDictionary { get; } = new Dictionary<string, string>()
        {
            {Help, "Displays all available commands with description"},
            {$"filename {BrightnessModification} intValue", "Modifies image's brightness by the given value"},
            {$"filename {ContrastModification} intValue", "Modifies image's contrast by the given value"},
            {$"filename {Negative}", "Negates an image"},
            {$"filename {HorizontalFlip}", "Flips an image horizontally"},
            {$"filename {VerticalFlip}", "Flips an image vertically"},
            {$"filename {DiagonalFlip}", "Flips an image diagonally"},
            {$"filename {ImageShrinking} intValue", "Shrinks an image x times"},
            {$"filename {ImageEnlargement} intValue", "Enlarges an image x times"},
            {$"filename {MidpointFilter} intValue", "Modifies image with the midpoint filter with scope equal to the given value"},
            {$"filename {ArithmeticMeanFilter} intValue", "Modifies image with the arithmetic mean filter with scope equal to the given value"},
            {$"filename filename {MeanSquareError}", "Calculates mean square error"},
            {$"filename filename {PeakMeanSquareError}", "Calculates peak mean square error"},
            {$"filename filename {SignalToNoiseRatio}", "Calculates signal to noise ratio"},
            {$"filename filename {PeakSignalToNoiseRatio}", "Calculates peak signal to noise ratio"},
            {$"filename filename {MaximumDifference}", "Calculates maximum difference"},
            {$"filename {Histogram} charValue (R, G or B)", ""},
            {$"filename {RaleighFinalProbabilityDensityFunction}", ""},
            {$"{Mean}", ""},
            {$"{Variance}", ""},
            {$"{StandardDeviation}", ""},
            {$"{VariationCoefficientI}", ""},
            {$"{VariationCoefficientII}", ""},
            {$"{InformationSourceEntropy}", ""},
            {$"{ExtractionOfDetailsI}", ""},
            {$"{RosenfeldOperator}", ""}
        };
    }
}