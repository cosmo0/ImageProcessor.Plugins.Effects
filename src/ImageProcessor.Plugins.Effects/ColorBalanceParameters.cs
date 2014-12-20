namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Stores parameters for the color balance effect
    /// </summary>
    public class ColorBalanceParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBalanceParameters"/> class
        /// </summary>
        public ColorBalanceParameters()
        {
            Blue = 255;
            Green = 255;
            Red = 255;
        }

        /// <summary>
        /// Gets or sets the blue value
        /// </summary>
        public float Blue { get; set; }

        /// <summary>
        /// Gets or sets the red value
        /// </summary>
        public float Red { get; set; }

        /// <summary>
        /// Gets or sets the green value
        /// </summary>
        public float Green { get; set; }
    }
}