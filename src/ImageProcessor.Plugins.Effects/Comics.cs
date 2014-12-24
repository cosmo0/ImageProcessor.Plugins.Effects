﻿namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Processors;

    /// <summary>
    /// Provides a cartoon or comics filter
    /// http://softwarebydefault.com/2013/06/02/cartoon-effect-image/
    /// Gaussian 7×7, Threshold X
    /// </summary>
    public class Comics : IGraphicsProcessor
    {
        public dynamic DynamicParameter { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = new Bitmap(factory.Image);
            var parameters = this.DynamicParameter;

            try
            {
                throw new NotImplementedException();
                //sourceBitmap = resultBitmap;
            }
            catch (Exception ex)
            {
                if (sourceBitmap != null)
                {
                    sourceBitmap.Dispose();
                }

                throw new ImageProcessingException("Error processing image with " + this.GetType().Name, ex);
            }

            return sourceBitmap;
        }
    }
}