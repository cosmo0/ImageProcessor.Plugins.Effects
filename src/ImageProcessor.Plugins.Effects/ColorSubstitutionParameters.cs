namespace ImageProcessor.Plugins.Effects
{
    using System.Drawing;

    /// <summary>
    /// Represents the parameters for the color substitution filter
    /// </summary>
    public class ColorSubstitutionParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSubstitutionParameters"/> class
        /// </summary>
        public ColorSubstitutionParameters()
        {
            this.Threshold = 50;
            this.ToReplace = Color.White;
            this.ReplaceWith = Color.Black;
        }

        /// <summary>
        /// Gets or sets the color to use in replacement
        /// </summary>
        public Color ReplaceWith { get; set; }

        /// <summary>
        /// Gets or sets the replacement threshold
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// Gets or sets the color to replace
        /// </summary>
        public Color ToReplace { get; set; }
    }
}