﻿namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Processors;

    /// <summary>
    /// Provides a base processor class with common operations
    /// </summary>
    public abstract class ProcessorBase : IGraphicsProcessor
    {
        /// <summary>
        /// Gets or sets the effect parameter
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        [Obsolete("Not used")]
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The bitmap to process</param>
        /// <returns>The processed bitmap</returns>
        public Bitmap ProcessBitmap(Bitmap sourceBitmap)
        {
            try
            {
                sourceBitmap = this.PreProcess(sourceBitmap);

                BitmapData sourceData = sourceBitmap.LockBits(
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                // process the pixels buffer
                pixelBuffer = this.Process(pixelBuffer, sourceBitmap.Width, sourceBitmap.Height, sourceData.Stride);

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
                BitmapData resultData = resultBitmap.LockBits(
                    new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                // possible image post-process
                resultBitmap = this.PostProcess(resultBitmap);

                sourceBitmap = resultBitmap;
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

        /// <summary>
        /// Processes the image from the factory
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = new Bitmap(factory.Image);

            return this.ProcessBitmap(sourceBitmap);
        }

        /// <summary>
        /// Guards a color minimum (0) and maximum (255) values
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The guarded color value</returns>
        protected static float Guard(float color)
        {
            if (color > 255)
            {
                return 255;
            }

            return color < 0 ? 0 : color;
        }

        /// <summary>
        /// Guards a color minimum (0) and maximum (255) values
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The guarded color value</returns>
        protected static int Guard(int color)
        {
            if (color > 255)
            {
                return 255;
            }

            if (color < 0)
            {
                return 0;
            }

            return color;
        }

        /// <summary>
        /// Guards a color minimum (0) and maximum (255) values
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The guarded color value</returns>
        protected static byte Guard(double color)
        {
            return (byte)(color > 255 ? 255 : (color < 0 ? 0 : color));
        }

        /// <summary>
        /// Post-processes the result bitmap
        /// </summary>
        /// <param name="resultBitmap">The result bitmap to post-process</param>
        /// <returns>The processed bitmap</returns>
        protected virtual Bitmap PostProcess(Bitmap resultBitmap)
        {
            return resultBitmap;
        }

        /// <summary>
        /// Pre-processes the source bitmap
        /// </summary>
        /// <returns>The processed bitmap</returns>
        /// <param name="sourceBitmap">The source bitmap to pre-process</param>
        protected virtual Bitmap PreProcess(Bitmap sourceBitmap)
        {
            return sourceBitmap;
        }

        /// <summary>
        /// Processes the image using a pixel buffer
        /// </summary>
        /// <param name="pixelBuffer">The pixel buffer to use</param>
        /// <param name="sourceWidth">The source image width</param>
        /// <param name="sourceHeight">The source image height</param>
        /// <param name="sourceStride">The source data stride</param>
        /// <returns>The processed pixel buffer</returns>
        protected abstract byte[] Process(byte[] pixelBuffer, int sourceWidth, int sourceHeight, int sourceStride);
    }
}