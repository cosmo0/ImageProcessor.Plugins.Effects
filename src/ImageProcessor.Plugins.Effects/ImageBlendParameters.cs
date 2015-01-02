namespace ImageProcessor.Plugins.Effects
{
    public class ImageBlendParameters
    {
        private const float DefaultLevelValue = 0.5F;

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

        public enum ColorComponentBlendType
        {
            Add,

            Average,

            DescendingOrder,

            AscendingOrder,

            Substract
        }

        public ColorComponentBlendType BlendTypeBlue { get; set; }

        public ColorComponentBlendType BlendTypeGreen { get; set; }

        public ColorComponentBlendType BlendTypeRed { get; set; }

        public string Overlay { get; set; }

        public bool OverlayBlueEnabled { get; set; }

        public float OverlayBlueLevel { get; set; }

        public bool OverlayGreenEnabled { get; set; }

        public float OverlayGreenLevel { get; set; }

        public bool OverlayRedEnabled { get; set; }

        public float OverlayRedLevel { get; set; }

        public bool SourceBlueEnabled { get; set; }

        public float SourceBlueLevel { get; set; }

        public bool SourceGreenEnabled { get; set; }

        public float SourceGreenLevel { get; set; }

        public bool SourceRedEnabled { get; set; }

        public float SourceRedLevel { get; set; }
    }
}