namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Parameters for the oil painting effect
    /// </summary>
    public class OilPaintingParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OilPaintingParameters"/> class
        /// </summary>
        public OilPaintingParameters()
        {
            this.FilterSize = 15;
            this.Levels = 10;
        }

        /// <summary>
        /// Gets or sets the filter size (higher value results in "flatter" image)
        /// </summary>
        public int FilterSize { get; set; }

        /// <summary>
        /// Gets or sets the intensity levels (higher value results in higher number of color intensities)
        /// </summary>
        public int Levels { get; set; }
    }
}