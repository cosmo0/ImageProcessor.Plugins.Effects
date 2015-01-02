namespace ImageProcessor.Plugins.Effects
{
    using System.Drawing;

    /// <summary>
    /// Represents the parameters for the stained glass effect
    /// </summary>
    public class StainedGlassParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StainedGlassParameters"/> class
        /// </summary>
        public StainedGlassParameters()
        {
            this.DistanceFormula = Formula.Euclidean;
            this.Factor = 1;
            this.Size = 10;

            this.Edges = false;
            this.EdgesColor = Color.Black;
            this.EdgesThreshold = 155;
        }

        /// <summary>
        /// The distance formula type (the shapes)
        /// </summary>
        public enum Formula
        {
            Euclidean,

            Manhattan,

            Chebyshev
        }

        /// <summary>
        /// Gets or sets the distance formula
        /// </summary>
        public Formula DistanceFormula { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the edges
        /// </summary>
        public bool Edges { get; set; }

        /// <summary>
        /// Gets or sets the edges color
        /// </summary>
        public Color EdgesColor { get; set; }

        /// <summary>
        /// Gets or sets the edges threshold
        /// </summary>
        public byte EdgesThreshold { get; set; }

        /// <summary>
        /// Gets or sets the blocks factor
        /// </summary>
        public float Factor { get; set; }

        /// <summary>
        /// Gets or sets the blocks size
        /// </summary>
        public int Size { get; set; }
    }
}