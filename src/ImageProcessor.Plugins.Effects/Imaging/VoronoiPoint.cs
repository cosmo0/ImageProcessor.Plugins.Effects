namespace ImageProcessor.Plugins.Effects.Imaging
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Voronoi point in the image
    /// http://softwarebydefault.com/2013/06/30/stained-glass-image-filter/
    /// </summary>
    internal class VoronoiPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoronoiPoint"/> class
        /// </summary>
        public VoronoiPoint()
        {
            this.PixelCollection = new List<Pixel>();
            this.BlueAverage = 0;
            this.BlueTotal = 0;
            this.GreenAverage = 0;
            this.GreenTotal = 0;
            this.RedAverage = 0;
            this.RedTotal = 0;
            this.XOffset = 0;
            this.YOffset = 0;
        }

        /// <summary>
        /// Gets or sets the blue average
        /// </summary>
        public int BlueAverage { get; private set; }

        /// <summary>
        /// Gets or sets the blue total
        /// </summary>
        public int BlueTotal { get; set; }

        /// <summary>
        /// Gets or sets the green average
        /// </summary>
        public int GreenAverage { get; private set; }

        /// <summary>
        /// Gets or sets the green total
        /// </summary>
        public int GreenTotal { get; set; }

        /// <summary>
        /// Gets or sets a list of pixels to use
        /// </summary>
        public List<Pixel> PixelCollection { get; private set; }

        /// <summary>
        /// Gets or sets the red average
        /// </summary>
        public int RedAverage { get; private set; }

        /// <summary>
        /// Gets or sets the red total
        /// </summary>
        public int RedTotal { get; set; }

        /// <summary>
        /// Gets or sets the X offset
        /// </summary>
        public int XOffset { get; set; }

        /// <summary>
        /// Gets or sets the Y offset
        /// </summary>
        public int YOffset { get; set; }

        /// <summary>
        /// Adds a pixel to the collection
        /// </summary>
        /// <param name="pixel">The pixel to add</param>
        public void AddPixel(Pixel pixel)
        {
            this.BlueTotal += pixel.Blue;
            this.GreenTotal += pixel.Green;
            this.RedTotal += pixel.Red;

            this.PixelCollection.Add(pixel);
        }

        /// <summary>
        /// Calculates the average for each color
        /// </summary>
        public void CalculateAverages()
        {
            if (this.PixelCollection.Count > 0)
            {
                this.BlueAverage = this.BlueTotal / this.PixelCollection.Count;
                this.GreenAverage = this.GreenTotal / this.PixelCollection.Count;
                this.RedAverage = this.RedTotal / this.PixelCollection.Count;
            }
        }
    }
}