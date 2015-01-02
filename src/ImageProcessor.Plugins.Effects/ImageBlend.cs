namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Processors;

    /// <summary>
    /// Image blending filter
    /// http://softwarebydefault.com/2013/03/10/bitmap-blending/
    /// </summary>
    public class ImageBlend : IGraphicsProcessor
    {
        /// <summary>
        /// Gets or sets the filter parameter
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        [Obsolete("Not used")]
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            ImageBlendParameters parameters = this.DynamicParameter;

            Bitmap sourceBitmap = new Bitmap(factory.Image);
            Bitmap overlayImage = new Bitmap(parameters.Overlay);

            try
            {
                BitmapData baseImageData = sourceBitmap.LockBits(
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format32bppArgb);
                byte[] baseImageBuffer = new byte[baseImageData.Stride * baseImageData.Height];

                Marshal.Copy(baseImageData.Scan0, baseImageBuffer, 0, baseImageBuffer.Length);

                BitmapData overlayImageData = overlayImage.LockBits(
                    new Rectangle(0, 0, overlayImage.Width, overlayImage.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);
                byte[] overlayImageBuffer = new byte[overlayImageData.Stride * overlayImageData.Height];

                Marshal.Copy(overlayImageData.Scan0, overlayImageBuffer, 0, overlayImageBuffer.Length);

                for (int k = 0; k < baseImageBuffer.Length && k < overlayImageBuffer.Length; k += 4)
                {
                    float sourceBlue = (parameters.SourceBlueEnabled ? baseImageBuffer[k] * parameters.SourceBlueLevel : 0);
                    float sourceGreen = (parameters.SourceGreenEnabled ? baseImageBuffer[k + 1] * parameters.SourceGreenLevel : 0);
                    float sourceRed = (parameters.SourceRedEnabled ? baseImageBuffer[k + 2] * parameters.SourceRedLevel : 0);

                    float overlayBlue = (parameters.OverlayBlueEnabled ? overlayImageBuffer[k] * parameters.OverlayBlueLevel : 0);
                    float overlayGreen = (parameters.OverlayGreenEnabled ? overlayImageBuffer[k + 1] * parameters.OverlayGreenLevel : 0);
                    float overlayRed = (parameters.OverlayRedEnabled ? overlayImageBuffer[k + 2] * parameters.OverlayRedLevel : 0);

                    baseImageBuffer[k] = CalculateColorComponentBlendValue(sourceBlue, overlayBlue, parameters.BlendTypeBlue);
                    baseImageBuffer[k + 1] = CalculateColorComponentBlendValue(sourceGreen, overlayGreen, parameters.BlendTypeGreen);
                    baseImageBuffer[k + 2] = CalculateColorComponentBlendValue(sourceRed, overlayRed, parameters.BlendTypeRed);
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);
                BitmapData resultImageData = resultBitmap.LockBits(
                    new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);

                Marshal.Copy(baseImageBuffer, 0, resultImageData.Scan0, baseImageBuffer.Length);

                resultBitmap.UnlockBits(resultImageData);
                sourceBitmap.UnlockBits(baseImageData);
                overlayImage.UnlockBits(overlayImageData);

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
        /// Calculates the color component blended value
        /// </summary>
        /// <param name="source">The source value</param>
        /// <param name="overlay">The overlay value</param>
        /// <param name="blendType">The blend type</param>
        /// <returns>The calculated value</returns>
        private static byte CalculateColorComponentBlendValue(float source, float overlay, ImageBlendParameters.ColorComponentBlendType blendType)
        {
            float resultValue = 0;
            byte resultByte = 0;

            if (blendType == ImageBlendParameters.ColorComponentBlendType.Add)
            {
                resultValue = source + overlay;
            }
            else if (blendType == ImageBlendParameters.ColorComponentBlendType.Substract)
            {
                resultValue = source - overlay;
            }
            else if (blendType == ImageBlendParameters.ColorComponentBlendType.Average)
            {
                resultValue = (source + overlay) / 2.0f;
            }
            else if (blendType == ImageBlendParameters.ColorComponentBlendType.AscendingOrder)
            {
                resultValue = (source > overlay ? overlay : source);
            }
            else if (blendType == ImageBlendParameters.ColorComponentBlendType.DescendingOrder)
            {
                resultValue = (source < overlay ? overlay : source);
            }

            if (resultValue > 255)
            {
                resultByte = 255;
            }
            else if (resultValue < 0)
            {
                resultByte = 0;
            }
            else
            {
                resultByte = (byte)resultValue;
            }

            return resultByte;
        }
    }
}