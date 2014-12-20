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
    /// Provides color balance modifications
    /// </summary>
    /// <remarks>
    /// Shameless copypasta from http://softwarebydefault.com/2013/04/11/bitmap-color-balance/
    /// </remarks>
    public class ColorBalance : IGraphicsProcessor
    {
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap bmp = new Bitmap(factory.Image);
            ColorBalanceParameters parameters = this.DynamicParameter;

            try
            {
                BitmapData sourceData = bmp.LockBits(
                                            new Rectangle(0, 0, bmp.Width, bmp.Height),  
                                            ImageLockMode.ReadOnly, 
                                            PixelFormat.Format32bppArgb); 

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height]; 

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length); 

                bmp.UnlockBits(sourceData); 

                float blue = 0; 
                float green = 0; 
                float red = 0; 

                float blueLevelFloat = parameters.Blue;
                float greenLevelFloat = parameters.Green;
                float redLevelFloat = parameters.Red;

                for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
                {
                    blue = 255.0f / blueLevelFloat * (float)pixelBuffer[k]; 
                    green = 255.0f / greenLevelFloat * (float)pixelBuffer[k + 1]; 
                    red = 255.0f / redLevelFloat * (float)pixelBuffer[k + 2]; 

                    if (blue > 255)
                    {
                        blue = 255;
                    }
                    else if (blue < 0)
                    {
                        blue = 0;
                    }

                    if (green > 255)
                    {
                        green = 255;
                    }
                    else if (green < 0)
                    {
                        green = 0;
                    }

                    if (red > 255)
                    {
                        red = 255;
                    }
                    else if (red < 0)
                    {
                        red = 0;
                    }

                    pixelBuffer[k] = (byte)blue; 
                    pixelBuffer[k + 1] = (byte)green; 
                    pixelBuffer[k + 2] = (byte)red; 
                }

                Bitmap resultBitmap = new Bitmap(bmp.Width, bmp.Height); 

                BitmapData resultData = resultBitmap.LockBits(
                                            new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),  
                                            ImageLockMode.WriteOnly, 
                                            PixelFormat.Format32bppArgb); 

                Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length); 
                resultBitmap.UnlockBits(resultData); 

                bmp = resultBitmap;
            }
            catch (Exception ex)
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                }

                throw new ImageProcessingException("Error processing image with " + this.GetType().Name, ex);
            }

            return bmp;
        }

        /// <summary>
        /// Gets or sets the <see cref="ColorBalanceParameters"/> for the effect
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        [Obsolete("This parameter is not used")]
        public Dictionary<string, string> Settings { get; set; }
    }
}