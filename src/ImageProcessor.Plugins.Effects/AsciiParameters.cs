namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Defines the parameters for the ASCII filter
    /// </summary>
    public class AsciiParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsciiParameters"/> class
        /// </summary>
        public AsciiParameters()
        {
            this.CharacterCount = 26;
            this.FontSize = 10;
            this.PixelPerCharacter = 5;
            this.Zoom = 100;
        }

        /// <summary>
        /// Gets or sets the unique characters count
        /// </summary>
        public int CharacterCount { get; set; }

        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// Gets or sets the number of pixels per character
        /// </summary>
        public int PixelPerCharacter { get; set; }

        /// <summary>
        /// Gets or sets the zoom level
        /// </summary>
        public int Zoom { get; set; }
    }
}