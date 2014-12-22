namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Represents a pixel in the image
    /// http://softwarebydefault.com/2013/06/30/stained-glass-image-filter/
    /// </summary>
    internal class Pixel
    {
        private byte blue = 0;

        private byte green = 0;

        private byte red = 0;

        private int xOffset = 0;

        private int yOffset = 0;

        public byte Blue
        {
            get
            {
                return this.blue;
            }
            set
            {
                this.blue = value;
            }
        }

        public byte Green
        {
            get
            {
                return this.green;
            }
            set
            {
                this.green = value;
            }
        }

        public byte Red
        {
            get
            {
                return this.red;
            }
            set
            {
                this.red = value;
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
    }
}