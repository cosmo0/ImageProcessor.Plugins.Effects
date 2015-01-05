namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Represents a pixel in the image
    /// <see href="http://softwarebydefault.com/2013/06/30/stained-glass-image-filter/" />
    /// </summary>
    internal class Pixel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> class
        /// </summary>
        public Pixel()
        {
            this.Blue = 0;
            this.Green = 0;
            this.Red = 0;
            this.XOffset = 0;
            this.YOffset = 0;
        }

        /// <summary>
        /// Gets or sets the blue value
        /// </summary>
        public byte Blue { get; set; }

        /// <summary>
        /// Gets or sets the green value
        /// </summary>
        public byte Green { get; set; }

        /// <summary>
        /// Gets or sets the red value
        /// </summary>
        public byte Red { get; set; }

        /// <summary>
        /// Gets or sets the X offset
        /// </summary>
        public int XOffset { get; set; }

        /// <summary>
        /// Gets or sets the Y offset
        /// </summary>
        public int YOffset { get; set; }
    }
}