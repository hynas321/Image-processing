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
        public const string AsymmetryCoefficient = "--casyco";
        public const string FlatteningCoefficient = "--cfsyco";
        public const string VariationCoefficientII = "--cvarcoii";
        public const string InformationSourceEntropy = "--centropy";
        #endregion

        #region S (linear image filtration algorithm in spatial domain basing on convolution)
        public const string ExtractionOfDetailsI = "--sexdeti";
        public const string ExtractionOfDetailsIOptimized = "--sexdetio";
        #endregion

        #region O (non-linear image filtration algorithm in spatial domain)
        public const string SobelOperator = "--osobel";
        #endregion
        #endregion

        #region Task 3
        #region M (morphological operations)
        public const string Dilation = "--dilation";
        public const string Erosion = "--erosion";
        public const string Opening = "--opening";
        public const string Closing = "--closing";
        public const string Hmt = "--hmt";
        public const string M1Operation1 = "--m1op1";
        public const string M1Operation2 = "--m1op2";
        public const string M1Operation3 = "--m1op3";
        #endregion

        #region R (image segmentation)
        public const string Merging = "--merging";
        #endregion
        #endregion

        #region Task 4
        #region Fourier Transform
        public const string SlowFourierTransform = "--sft";
        public const string InverseSlowFourierTransform = "--isft";
        public const string FastFourierTransform = "--fft";
        public const string InverseFastFourierTransform = "--ifft";
        #endregion

        #region Filters in frequency domain
        public const string LowPassFilter = "--lowpass";
        public const string HighPassFilter = "--highpass";
        public const string BandPassFilter = "--bandpass";
        public const string BandCutFilter = "--bandcut";
        public const string HighPassWithEdgeDirection = "--edgehighpass";
        public const string PhaseModifyingFilter = "--phase";
        #endregion

        #endregion

        public static Dictionary<string, string> OperationsDictionary { get; } = new Dictionary<string, string>()
        {
            {Help, "Displays all available commands with description"},
            {$"*PART1*", ""},
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
            {$"*PART2*", ""},
            {$"filename {Histogram} charValue (R, G or B)", ""},
            {$"filename {RaleighFinalProbabilityDensityFunction} intValue (min brightness) intValue (max brightness)", ""},
            {$"filename {Mean} charValue (R, G or B)", "Calculates mean"},
            {$"filename {Variance} charValue (R, G or B)", "Calculates variance"},
            {$"filename {StandardDeviation} charValue (R, G or B)", "Calculates standard deviation"},
            {$"filename {VariationCoefficientI} charValue (R, G or B)", "Calculates variation coefficient I"},
            {$"filename {AsymmetryCoefficient} charValue (R, G or B)", "Calculates asymmetry coefficient"},
            {$"filename {FlatteningCoefficient} charValue (R, G or B)", "Calculates flattening coefficient" },
            {$"filename {VariationCoefficientII} charValue (R, G or B)", "Calculates variation coefficient II"},
            {$"filename {InformationSourceEntropy} charValue (R, G or B)", "Calculates information source entropy"},
            {$"filename {ExtractionOfDetailsI} intValue (1, 2, 3, 4), masks: N, NE, SE, S", "Applies extraction of details"},
            {$"filename {ExtractionOfDetailsIOptimized}", "Applies optimized extraction of details for the N mask"},
            {$"filename {SobelOperator}", "Applies sobel operator"},
            {$"*PART3*", ""},
            {$"filename {Dilation} intValue (1-18)", "Applies dilation operation for a chosen mask"},
            {$"filename {Erosion} intValue (1-18)", "Applies erosion operation for a chosen mask"},
            {$"filename {Opening} intValue (1-18)", "Applies opening operation for a chosen mask"},
            {$"filename {Closing} intValue (1-18)", "Applies closing operation for a chosen mask"},
            {$"filename {Hmt} intValue (1-18)", "Applies HMT operation for a chosen mask"},
            {$"filename {M1Operation1} intValue (1-18)", "Applies 1st morphological operation: dilated bitmap / bitmap"},
            {$"filename {M1Operation2} intValue (1-18)", "Applies 2nd morphological operation: bitmap / erosed bitmap"},
            {$"filename {M1Operation3} intValue (1-18)", "Applies 3rd morphological operation: dilated bitmap / erosed bitmap"},
            {$"filename {Merging} intValue (x coordinate) intValue (y coordinate) intValue (threshold)", "Applies merging"},
            {$"*PART4*", ""},
            {$"filename {SlowFourierTransform}", "Applies slow Fourier Transform in the spatial domain with its visualization"},
            {$"filename {InverseSlowFourierTransform}", "Applies slow inverse Fourier Transform in the spatial domain with its visualization"},
            {$"filename {FastFourierTransform}", "Applies Fast Fourier Transform in the spatial domain with its visualization"},
            {$"filename {InverseFastFourierTransform}", "Applies inverse Fast Fourier Transform in the spatial domain with its visualization"},
            {$"filename {LowPassFilter} intValue", "Applies low-pass filter for the chosen threshold"},
            {$"filename {HighPassFilter} intValue", "Applies high-pass filter for the chosen threshold"},
            {$"filename {BandPassFilter} intValue intValue", "Applies band-pass filter for the chosen min and max threshold"},
            {$"filename {BandCutFilter} intValue intValue", "Applies band-cut filter for the chosen min and max threshold"},
            {$"filename filename {HighPassWithEdgeDirection} intValue", "Applies high-pass filter for the chosen mask (the 2nd filename) and threshold"},
            {$"filename {PhaseModifyingFilter} intValue intValue", "Applies phase modifying filter for values k and l"}
        };
    }
}