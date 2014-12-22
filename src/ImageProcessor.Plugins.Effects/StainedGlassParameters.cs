using System.Drawing;

namespace ImageProcessor.Plugins.Effects
{
    public class StainedGlassParameters
    {
        public StainedGlassParameters()
        {
            this.DistanceFormula = Formula.Euclidean;
            this.Edges = false;
            this.Factor = 1;
            this.Size = 10;
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

        public Formula DistanceFormula { get; set; }

        public bool Edges { get; set; }

        public Color EdgesColor { get; set; }

        public byte EdgesThreshold { get; set; }

        public float Factor { get; set; }

        public int Size { get; set; }
    }
}