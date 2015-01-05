namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Drawing;
    using ImageProcessor.Plugins.Effects.Imaging;

    /// <summary>
    /// Provides a cartoon or comics filter
    /// <see href="http://softwarebydefault.com/2013/06/02/cartoon-effect-image/" />
    /// </summary>
    public class Comics : ProcessorBase
    {
        /// <summary>
        /// Pre-processes the source bitmap
        /// </summary>
        /// <returns>The processed bitmap</returns>
        /// <param name="sourceBitmap">The source bitmap.</param>
        protected override Bitmap PreProcess(Bitmap sourceBitmap)
        {
            ComicsParameters parameters = this.DynamicParameter;
            return sourceBitmap.SmoothingFilter(parameters.Smoothing);
        }

        /// <summary>
        /// Processes the image using a pixel buffer
        /// </summary>
        /// <param name="pixelBuffer">The pixel buffer to use</param>
        /// <param name="sourceWidth">The source image width</param>
        /// <param name="sourceHeight">The source image height</param>
        /// <param name="sourceStride">The source data stride</param>
        /// <returns>The processed pixel buffer</returns>
        protected override byte[] Process(byte[] pixelBuffer, int sourceWidth, int sourceHeight, int sourceStride)
        {
            ComicsParameters parameters = this.DynamicParameter;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            int byteOffset = 0;
            int blueGradient, greenGradient, redGradient = 0;
            double blue = 0, green = 0, red = 0;

            bool exceedsThreshold = false;

            for (int offsetY = 1; offsetY < sourceHeight - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceWidth - 1; offsetX++)
                {
                    byteOffset = (offsetY * sourceStride) + (offsetX * 4);
                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);
                    byteOffset++;
                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);
                    byteOffset++;
                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);

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
                            blueGradient = Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);
                            byteOffset++;
                            greenGradient = Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);
                            byteOffset++;
                            redGradient = Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]);

                            if (blueGradient + greenGradient + redGradient > parameters.Threshold)
                            {
                                exceedsThreshold = true;
                            }
                            else
                            {
                                byteOffset -= 2;
                                blueGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]);
                                blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]);
                                byteOffset++;
                                greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]);
                                greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]);
                                byteOffset++;
                                redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]);
                                redGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]);

                                exceedsThreshold = blueGradient + greenGradient + redGradient > parameters.Threshold;
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

                    blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
                    green = green > 255 ? 255 : (green < 0 ? 0 : green);
                    red = red > 255 ? 255 : (red < 0 ? 0 : red);

                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            return resultBuffer;
        }
    }
}