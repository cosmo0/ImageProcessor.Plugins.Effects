namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Represents the parameters for the image blend effect
    /// </summary>
    public class ImageBlendParameters
    {
        /// <summary>
        /// Stores a default value for the levels
        /// </summary>
        private const float DefaultLevelValue = 0.5F;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBlendParameters"/> class with default values
        /// </summary>
        public ImageBlendParameters()
        {
            this.BlendTypeBlue = ColorComponentBlendType.Add;
            this.BlendTypeGreen = ColorComponentBlendType.Add;
            this.BlendTypeRed = ColorComponentBlendType.Add;

            this.SourceBlueEnabled = true;
            this.SourceGreenEnabled = true;
            this.SourceRedEnabled = true;

            this.SourceBlueLevel = DefaultLevelValue;
            this.SourceGreenLevel = DefaultLevelValue;
            this.SourceRedLevel = DefaultLevelValue;

            this.OverlayBlueEnabled = true;
            this.OverlayGreenEnabled = true;
            this.OverlayRedEnabled = true;

            this.OverlayBlueLevel = DefaultLevelValue;
            this.OverlayGreenLevel = DefaultLevelValue;
            this.OverlayRedLevel = DefaultLevelValue;
        }

        /// <summary>
        /// Represents the blend type for each color component
        /// </summary>
        public enum ColorComponentBlendType
        {
            /// <summary>
            /// Add colors
            /// </summary>
            Add,

            /// <summary>
            /// Color average
            /// </summary>
            Average,

            /// <summary>
            /// Descending order
            /// </summary>
            DescendingOrder,

            /// <summary>
            /// Ascending order
            /// </summary>
            AscendingOrder,

            /// <summary>
            /// Subtract colors
            /// </summary>
            Subtract
        }

        /// <summary>
        /// Gets or sets the blend type for blue
        /// </summary>
        public ColorComponentBlendType BlendTypeBlue { get; set; }

        /// <summary>
        /// Gets or sets the blend type for green
        /// </summary>
        public ColorComponentBlendType BlendTypeGreen { get; set; }

        /// <summary>
        /// Gets or sets the blend type for red
        /// </summary>
        public ColorComponentBlendType BlendTypeRed { get; set; }

        /// <summary>
        /// Gets or sets the path to the overlay image
        /// </summary>
        public string Overlay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the blue is enabled for the overlay
        /// </summary>
        public bool OverlayBlueEnabled { get; set; }

        /// <summary>
        /// Gets or sets the blue level for the overlay
        /// </summary>
        public float OverlayBlueLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the green is enabled for the overlay
        /// </summary>
        public bool OverlayGreenEnabled { get; set; }

        /// <summary>
        /// Gets or sets the green level for the overlay
        /// </summary>
        public float OverlayGreenLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the red is enabled for the overlay
        /// </summary>
        public bool OverlayRedEnabled { get; set; }

        /// <summary>
        /// Gets or sets the red level for the overlay
        /// </summary>
        public float OverlayRedLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the blue is enabled for the source
        /// </summary>
        public bool SourceBlueEnabled { get; set; }

        /// <summary>
        /// Gets or sets the blue level for the source
        /// </summary>
        public float SourceBlueLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the green is enabled for the source
        /// </summary>
        public bool SourceGreenEnabled { get; set; }

        /// <summary>
        /// Gets or sets the green level for the source
        /// </summary>
        public float SourceGreenLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the red is enabled for the source
        /// </summary>
        public bool SourceRedEnabled { get; set; }

        /// <summary>
        /// Gets or sets the red level for the source
        /// </summary>
        public float SourceRedLevel { get; set; }
    }
}