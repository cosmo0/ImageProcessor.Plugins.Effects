namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Lists the possible smoothing filters
    /// </summary>
    public enum SmoothingFilterType
    {
        None,

        Gaussian3x3,

        Gaussian5x5,

        Gaussian7x7,

        Median3x3,

        Median5x5,

        Median7x7,

        Median9x9,

        Mean3x3,

        Mean5x5,

        LowPass3x3,

        LowPass5x5,

        Sharpen3x3,
    }
}