namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Lists the possible smoothing filters
    /// </summary>
    public enum SmoothingFilterType
    {
        /// <summary>
        /// No smoothing
        /// </summary>
        None,

        /// <summary>
        /// Gaussian 3x3 smoothing
        /// </summary>
        Gaussian3x3,

        /// <summary>
        /// Gaussian 5x5 smoothing
        /// </summary>
        Gaussian5x5,

        /// <summary>
        /// Gaussian 7x7 smoothing
        /// </summary>
        Gaussian7x7,

        /// <summary>
        /// Median 3x3 smoothing
        /// </summary>
        Median3x3,

        /// <summary>
        /// Median 5x5 smoothing
        /// </summary>
        Median5x5,

        /// <summary>
        /// Median 7x7 smoothing
        /// </summary>
        Median7x7,

        /// <summary>
        /// Median 9x9 smoothing
        /// </summary>
        Median9x9,

        /// <summary>
        /// Mean 3x3 smoothing
        /// </summary>
        Mean3x3,

        /// <summary>
        /// Mean 5x5 smoothing
        /// </summary>
        Mean5x5,

        /// <summary>
        /// Low pass 3x3 smoothing
        /// </summary>
        LowPass3x3,

        /// <summary>
        /// Low pass 5x5 smoothing
        /// </summary>
        LowPass5x5,

        /// <summary>
        /// Sharpen 3x3
        /// </summary>
        Sharpen3x3,
    }
}