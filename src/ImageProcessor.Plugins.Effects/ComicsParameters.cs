namespace ImageProcessor.Plugins.Effects
{
    using ImageProcessor.Plugins.Effects.Imaging;

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
            this.Smoothing = SmoothingFilterType.Mean5x5;
            this.Threshold = 100;
        }

        /// <summary>
        /// Gets or sets the smoothing filter
        /// </summary>
        public SmoothingFilterType Smoothing { get; set; }

        /// <summary>
        /// Gets or sets the threshold value
        /// </summary>
        public int Threshold { get; set; }
    }
}