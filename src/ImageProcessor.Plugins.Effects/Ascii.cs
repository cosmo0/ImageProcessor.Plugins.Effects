namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using ImageProcessor.Processors;

    /// <summary>
    /// ASCII-style filter processor
    /// http://softwarebydefault.com/2013/07/14/image-ascii-art/
    /// </summary>
    public class Ascii : IGraphicsProcessor
    {
        /// <summary>
        /// Gets or sets the processor parameter
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image through the filter
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}