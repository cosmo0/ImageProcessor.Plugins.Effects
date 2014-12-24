namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Represents parameters for the comics filter
    /// </summary>
    public class ComicsParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComicsParameters"/> class
        /// </summary>
        public ComicsParameters()
        {
            this.Smoothing = SmoothingFilter.Mean5x5;
            this.Threshold = 50;
        }

        /// <summary>
        /// Lists the possible smoothing filters for the effect
        /// </summary>
        public enum SmoothingFilter
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

        /// <summary>
        /// Gets or sets the smoothing filter
        /// </summary>
        public SmoothingFilter Smoothing { get; set; }

        /// <summary>
        /// Gets or sets the threshold value
        /// </summary>
        public int Threshold { get; set; }
    }
}