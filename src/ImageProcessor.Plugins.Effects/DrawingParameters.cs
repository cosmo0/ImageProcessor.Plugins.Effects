namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Defines parameters for the drawing filter
    /// </summary>
    public class DrawingParameters
    {
        /// <summary>
        /// Gets or sets the blue factor
        /// </summary>
        internal float BlueFactor = 0;

        /// <summary>
        /// Gets or sets the green factor
        /// </summary>
        internal float GreenFactor = 0;

        /// <summary>
        /// Gets or sets the red factor
        /// </summary>
        internal float RedFactor = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingParameters"/> class
        /// </summary>
        public DrawingParameters()
        {
            this.FilterType = EdgeFilterType.Sharpen;
            this.Threshold = 150;
        }

        /// <summary>
        /// Defines the possible edge filters
        /// </summary>
        public enum EdgeFilterType
        {
            None,

            EdgeDetectMono,

            EdgeDetectGradient,

            Sharpen,

            SharpenGradient,
        }

        /// <summary>
        /// Gets or sets the filter type
        /// </summary>
        public EdgeFilterType FilterType { get; set; }

        /// <summary>
        /// Gets or sets the threshold
        /// </summary>
        public byte Threshold { get; set; }
    }
}