namespace ImageProcessor.Plugins.Effects
{
    using System.Collections.Generic;
    using System.Drawing;
    using ImageProcessor.Processors;

    /// <summary>
    /// Provides color balance modifications
    /// </summary>
    public class ColorBalance : IGraphicsProcessor
    {
        public Image ProcessImage(ImageFactory factory)
        {
            throw new System.NotImplementedException();
        }

        public dynamic DynamicParameter { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }
}