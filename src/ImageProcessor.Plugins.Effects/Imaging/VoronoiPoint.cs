namespace ImageProcessor.Plugins.Effects.Imaging
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Voronoi point in the image
    /// http://softwarebydefault.com/2013/06/30/stained-glass-image-filter/
    /// </summary>
    internal class VoronoiPoint
    {
        private readonly List<Pixel> pixelCollection = new List<Pixel>();

        private int blueAverage = 0;

        private int blueTotal = 0;

        private int greenAverage = 0;

        private int greenTotal = 0;

        private int redAverage = 0;

        private int redTotal = 0;

        private int xOffset = 0;

        private int yOffset = 0;

        public int BlueAverage
        {
            get
            {
                return this.blueAverage;
            }
        }

        public int BlueTotal
        {
            get
            {
                return this.blueTotal;
            }
            set
            {
                this.blueTotal = value;
            }
        }

        public int GreenAverage
        {
            get
            {
                return this.greenAverage;
            }
        }

        public int GreenTotal
        {
            get
            {
                return this.greenTotal;
            }
            set
            {
                this.greenTotal = value;
            }
        }

        public List<Pixel> PixelCollection
        {
            get
            {
                return this.pixelCollection;
            }
        }

        public int RedAverage
        {
            get
            {
                return this.redAverage;
            }
        }

        public int RedTotal
        {
            get
            {
                return this.redTotal;
            }
            set
            {
                this.redTotal = value;
            }
        }

        public int XOffset
        {
            get
            {
                return this.xOffset;
            }
            set
            {
                this.xOffset = value;
            }
        }

        public int YOffset
        {
            get
            {
                return this.yOffset;
            }
            set
            {
                this.yOffset = value;
            }
        }

        public void AddPixel(Pixel pixel)
        {
            this.blueTotal += pixel.Blue;
            this.greenTotal += pixel.Green;
            this.redTotal += pixel.Red;

            this.pixelCollection.Add(pixel);
        }

        public void CalculateAverages()
        {
            if (this.pixelCollection.Count > 0)
            {
                this.blueAverage = this.blueTotal / this.pixelCollection.Count;
                this.greenAverage = this.greenTotal / this.pixelCollection.Count;
                this.redAverage = this.redTotal / this.pixelCollection.Count;
            }
        }
    }
}