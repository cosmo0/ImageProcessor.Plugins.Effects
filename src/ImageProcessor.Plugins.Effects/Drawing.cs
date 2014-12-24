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
    /// Applies a hand-drawing filter to an image
    /// http://softwarebydefault.com/2013/06/01/gradient-based-edge-detection/
    /// Edge Detect Sharpen, Second Derivative, Threshold 40, Black
    /// </summary>
    public class Drawing : IGraphicsProcessor
    {
        /// <summary>
        /// Gets or sets the filter parameter
        /// </summary>
        public dynamic DynamicParameter { get; set; }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        [Obsolete]
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Processes the image with the filter
        /// </summary>
        /// <param name="factory">The factory to use</param>
        /// <returns>The processed image</returns>
        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = new Bitmap(factory.Image);
            DrawingParameters parameters = this.DynamicParameter;

            try
            {
                BitmapData sourceData = sourceBitmap.LockBits(
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                int derivative = 2;
                int byteOffset = 0;
                int blueGradient, greenGradient, redGradient = 0;
                double blue = 0, green = 0, red = 0;

                bool exceedsThreshold = false;

                for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
                {
                    for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                    {
                        byteOffset = offsetY * sourceData.Stride + offsetX * 4;
                        blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                        blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]) / derivative;
                        byteOffset++;
                        greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                        greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]) / derivative;
                        byteOffset++;
                        redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                        redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]) / derivative;

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
                                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]) / derivative;
                                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]) / derivative;
                                    byteOffset++;
                                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]) / derivative;
                                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]) / derivative;
                                    byteOffset++;
                                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]) / derivative;
                                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]) / derivative;

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
                            if (parameters.FilterType == DrawingParameters.EdgeFilterType.EdgeDetectMono)
                            {
                                blue = green = red = 255;
                            }
                            else if (parameters.FilterType == DrawingParameters.EdgeFilterType.EdgeDetectGradient)
                            {
                                blue = blueGradient * parameters.BlueFactor;
                                green = greenGradient * parameters.GreenFactor;
                                red = redGradient * parameters.RedFactor;
                            }
                            else if (parameters.FilterType == DrawingParameters.EdgeFilterType.Sharpen)
                            {
                                blue = pixelBuffer[byteOffset] * parameters.BlueFactor;
                                green = pixelBuffer[byteOffset + 1] * parameters.GreenFactor;
                                red = pixelBuffer[byteOffset + 2] * parameters.RedFactor;
                            }
                            else if (parameters.FilterType == DrawingParameters.EdgeFilterType.SharpenGradient)
                            {
                                blue = pixelBuffer[byteOffset] + blueGradient * parameters.BlueFactor;
                                green = pixelBuffer[byteOffset + 1] + greenGradient * parameters.GreenFactor;
                                red = pixelBuffer[byteOffset + 2] + redGradient * parameters.RedFactor;
                            }
                        }
                        else
                        {
                            if (parameters.FilterType == DrawingParameters.EdgeFilterType.EdgeDetectMono ||
                                parameters.FilterType == DrawingParameters.EdgeFilterType.EdgeDetectGradient)
                            {
                                blue = green = red = 0;
                            }
                            else if (parameters.FilterType == DrawingParameters.EdgeFilterType.Sharpen ||
                                     parameters.FilterType == DrawingParameters.EdgeFilterType.SharpenGradient)
                            {
                                blue = pixelBuffer[byteOffset];
                                green = pixelBuffer[byteOffset + 1];
                                red = pixelBuffer[byteOffset + 2];
                            }
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