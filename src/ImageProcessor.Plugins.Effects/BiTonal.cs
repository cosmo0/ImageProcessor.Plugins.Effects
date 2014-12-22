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
    /// Bi-Tonal bitmap filter
    /// http://softwarebydefault.com/2013/04/12/bitonal-bitmaps/
    /// </summary>
    public class BiTonal : IGraphicsProcessor
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
        /// Proceses the image from the factory
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = new Bitmap(factory.Image);
            BiTonalParameters parameters = this.DynamicParameter;

            Color darkColor = parameters.DarkColor;
            Color lightColor = parameters.LightColor;

            try
            {
                BitmapData sourceData = sourceBitmap.LockBits(
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                {
                    if (pixelBuffer[k] + pixelBuffer[k + 1] +
                        pixelBuffer[k + 2] <= parameters.Threshold)
                    {
                        pixelBuffer[k] = darkColor.B;
                        pixelBuffer[k + 1] = darkColor.G;
                        pixelBuffer[k + 2] = darkColor.R;
                    }
                    else
                    {
                        pixelBuffer[k] = lightColor.B;
                        pixelBuffer[k + 1] = lightColor.G;
                        pixelBuffer[k + 2] = lightColor.R;
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
                BitmapData resultData = resultBitmap.LockBits(
                    new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);
                Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
                resultBitmap.UnlockBits(resultData); 

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
    }
}