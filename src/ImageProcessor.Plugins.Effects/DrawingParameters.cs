namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Defines parameters for the drawing filter
    /// </summary>
    public class DrawingParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingParameters"/> class
        /// </summary>
        public DrawingParameters()
        {
            this.FilterType = EdgeFilterType.Sharpen;
            this.Threshold = 150;

            this.BlueFactor = 0;
            this.RedFactor = 0;
            this.GreenFactor = 0;
        }

        /// <summary>
        /// Defines the possible edge filters
        /// </summary>
        public enum EdgeFilterType
        {
            /// <summary>
            /// No edge filter
            /// </summary>
            None,

            /// <summary>
            /// Mono-color edge filter
            /// </summary>
            EdgeDetectMono,

            /// <summary>
            /// Gradient-based edge filter
            /// </summary>
            EdgeDetectGradient,

            /// <summary>
            /// Sharpen edge filter
            /// </summary>
            Sharpen,

            /// <summary>
            /// Sharpen gradient edge filter
            /// </summary>
            SharpenGradient,
        }

        /// <summary>
        /// Gets or sets the blue factor
        /// </summary>
        public float BlueFactor { get; set; }

        /// <summary>
        /// Gets or sets the filter type
        /// </summary>
        public EdgeFilterType FilterType { get; set; }

        /// <summary>
        /// Gets or sets the green factor
        /// </summary>
        public float GreenFactor { get; set; }

        /// <summary>
        /// Gets or sets the red factor
        /// </summary>
        public float RedFactor { get; set; }

        /// <summary>
        /// Gets or sets the threshold
        /// </summary>
        public byte Threshold { get; set; }
    }
}