namespace ImageProcessor.Plugins.Effects
{
    using System.Drawing;

    /// <summary>
    /// Provides parameters for the gradient-based edge detection filter
    /// <see href="http://softwarebydefault.com/" />
    /// </summary>
    public class GradientBasedEdgeParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientBasedEdgeParameters"/> class
        /// </summary>
        public GradientBasedEdgeParameters()
        {
            this.EdgeColor = Color.Black;
            this.Threshold = 0;
        }

        /// <summary>
        /// Gets or sets the edge color
        /// </summary>
        public Color EdgeColor { get; set; }

        /// <summary>
        /// Gets or sets the threshold
        /// </summary>
        public byte Threshold { get; set; }
    }
}