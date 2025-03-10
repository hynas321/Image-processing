# Image Processing Application

The command-line image processing application written in C# offers a set of image manipulation features. It allows to apply various operations starting from basic brightness and contrast adjustments to complex frequency-domain filters and morphological transformations on bitmap images.

## Overview

- **Input/Output Folders:**  
  - **OriginalImages:** Contains the original, unmodified images.  
  - **ModifiedImages:** Processed images are saved here.  
  - **PlotImages:** Generated analysis plots (e.g., histograms) are stored in this folder.

- **Command-Line Driven:**  
  The application is operated via command-line arguments. Running the application without arguments or with the `--help` flag displays detailed usage information.

## Commands and Usage Examples

### Display Help Information
```
ImageProcessing --help
```

## Features

Below are presented available operations, however without detailed information which is displayed after using the `--help` command. 

### Basic Operations
- **Brightness Modification** (`--brightness`)
- **Contrast Modification** (`--contrast`)
- **Negative** (`--negative`)

### Geometric Transformations
- **Flips:**  
  - Horizontal (`--hflip`)  
  - Vertical (`--vflip`)  
  - Diagonal (`--dflip`)
- **Scaling:**  
  - Image Shrinking (`--shrink`)  
  - Image Enlargement (`--enlarge`)

### Filtering Techniques
- **Noise Reduction:**  
  - Midpoint Filter (`--mid`)  
  - Arithmetic Mean Filter (`--amean`)
- **Detail Extraction:**  
  - Extraction of Details (`--sexdeti`) with customizable masks.  
  - Optimized Extraction of Details (`--sexdetio`)
- **Edge Detection:**  
  - Sobel Operator (`--osobel`)

### Statistical Analysis & Quality Metrics
- **Statistical Calculations:**  
  - Mean (`--cmean`)  
  - Variance (`--cvariance`)  
  - Standard Deviation (`--cstdev`)  
  - Variation Coefficient I (`--cvarcoi`)  
  - Asymmetry Coefficient (`--casyco`)  
  - Flattening Coefficient (`--cfsyco`)  
  - Variation Coefficient II (`--cvarcoii`)  
  - Information Source Entropy (`--centropy`)
- **Error Metrics (using two images):**  
  - Mean Square Error (`--mse`)  
  - Peak Mean Square Error (`--pmse`)  
  - Signal-to-Noise Ratio (`--snr`)  
  - Peak Signal-to-Noise Ratio (`--psnr`)  
  - Maximum Difference (`--md`)

### Morphological Operations
- **Basic Operations:**  
  - Dilation (`--dilation`)  
  - Erosion (`--erosion`)  
  - Opening (`--opening`)  
  - Closing (`--closing`)
- **Advanced Operations:**  
  - HMT (`--hmt`)  
  - M1 Operations (`--m1op1`, `--m1op2`, `--m1op3`)
- **Image Segmentation:**  
  - Merging (`--merging`)

### Frequency Domain Processing
- **Fourier Transforms:**  
  - Discrete Fourier Transform (`--dft`) and Inverse DFT (`--idft`)  
  - Fast Fourier Transform (`--fft`) and Inverse FFT (`--ifft`)
- **Frequency Domain Filters:**  
  - Low-pass Filter (`--lowpass`)  
  - High-pass Filter (`--highpass`) with optional phase preservation  
  - Band-pass Filter (`--bandpass`)  
  - Band-cut Filter (`--bandcut`)  
  - High-pass Filter with Edge Direction (`--edgehighpass`)  
  - Phase Modifying Filter (`--phase`)

### Histogram Plotting
- **Histogram Generation:**  
  Generates histograms for specific color channels (R, G, or B) using the ScottPlot library.

