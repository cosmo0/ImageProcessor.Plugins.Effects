namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Plugins.Effects.Imaging;
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
            ComicsParameters parameters = this.DynamicParameter;

            try
            {
                sourceBitmap = sourceBitmap.SmoothingFilter(parameters.Smoothing);

                BitmapData sourceData =
                    sourceBitmap.LockBits(
                        new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                        ImageLockMode.ReadOnly,
                        PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                int byteOffset = 0;
                int blueGradient, greenGradient, redGradient = 0;
                double blue = 0, green = 0, red = 0;

                bool exceedsThreshold = false;

                for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
                {
                    for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                    {
                        byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                        blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);
                        byteOffset++;
                        greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);
                        byteOffset++;
                        redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                        redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                        if (blueGradient + greenGradient + redGradient > parameters.Threshold)
                        {
                            exceedsThreshold = true;
                        }
                        else
                        {
                            byteOffset -= 2;
                            blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                            byteOffset++;
                            greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                            byteOffset++;
                            redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);

                            if (blueGradient + greenGradient + redGradient > parameters.Threshold)
                            {
                                exceedsThreshold = true;
                            }
                            else
                            {
                                byteOffset -= 2;
                                blueGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);
                                byteOffset++;
                                greenGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);
                                byteOffset++;
                                redGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                                if (blueGradient + greenGradient + redGradient > parameters.Threshold)
                                {
                                    exceedsThreshold = true;
                                }
                                else
                                {
                                    byteOffset -= 2;
                                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);
                                    byteOffset++;
                                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);
                                    byteOffset++;
                                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);

                                    if (blueGradient + greenGradient + redGradient > parameters.Threshold)
                                    {
                                        exceedsThreshold = true;
                                    }
                                    else
                                    {
                                        exceedsThreshold = false;
                                    }
                                }
                            }
                        }

                        byteOffset -= 2;

                        if (exceedsThreshold)
                        {
                            blue = 0;
                            green = 0;
                            red = 0;
                        }
                        else
                        {
                            blue = pixelBuffer[byteOffset];
                            green = pixelBuffer[byteOffset + 1];
                            red = pixelBuffer[byteOffset + 2];
                        }

                        blue = (blue > 255 ? 255 : (blue < 0 ? 0 : blue));
                        green = (green > 255 ? 255 : (green < 0 ? 0 : green));
                        red = (red > 255 ? 255 : (red < 0 ? 0 : red));

                        resultBuffer[byteOffset] = (byte)blue;
                        resultBuffer[byteOffset + 1] = (byte)green;
                        resultBuffer[byteOffset + 2] = (byte)red;
                        resultBuffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

                BitmapData resultData =
                    resultBitmap.LockBits(
                        new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppArgb);

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
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