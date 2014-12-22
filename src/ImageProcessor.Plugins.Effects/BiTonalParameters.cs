namespace ImageProcessor.Plugins.Effects
{
    using System.Drawing;

    /// <summary>
    /// Defines the parameters for the bi-tonal filter
    /// </summary>
    public class BiTonalParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiTonalParameters"/> class
        /// </summary>
        public BiTonalParameters()
        {
            this.DarkColor = Color.Black;
            this.LightColor = Color.White;
            this.Threshold = 155;
        }

        /// <summary>
        /// Gets or sets the dark color
        /// </summary>
        public Color DarkColor { get; set; }

        /// <summary>
        /// Gets or sets the light color
        /// </summary>
        public Color LightColor { get; set; }

        /// <summary>
        /// Gets or sets the threshold
        /// </summary>
        public int Threshold { get; set; }
    }
}