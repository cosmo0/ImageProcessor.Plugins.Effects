namespace ImageProcessor.Plugins.Effects
{
    using System;

    /// <summary>
    /// Applies a hand-drawing filter to an image
    /// http://softwarebydefault.com/2013/06/01/gradient-based-edge-detection/
    /// Edge Detect Sharpen, Second Derivative, Threshold 40, Black
    /// </summary>
    public class Drawing : ProcessorBase
    {
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
            DrawingParameters parameters = this.DynamicParameter;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            int derivative = 2;
            int byteOffset = 0;
            int blueGradient, greenGradient, redGradient = 0;
            double blue = 0, green = 0, red = 0;

            bool exceedsThreshold = false;

            for (int offsetY = 1; offsetY < sourceHeight - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceWidth - 1; offsetX++)
                {
                    byteOffset = (offsetY * sourceStride) + (offsetX * 4);
                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]) / derivative;
                    byteOffset++;
                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]) / derivative;
                    byteOffset++;
                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]) / derivative;
                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride] - pixelBuffer[byteOffset + sourceStride]) / derivative;

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
                                blueGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]) / derivative;
                                blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]) / derivative;
                                byteOffset++;
                                greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]) / derivative;
                                greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]) / derivative;
                                byteOffset++;
                                redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceStride] - pixelBuffer[byteOffset + 4 + sourceStride]) / derivative;
                                redGradient += Math.Abs(pixelBuffer[byteOffset - sourceStride + 4] - pixelBuffer[byteOffset + sourceStride - 4]) / derivative;

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
                        switch (parameters.FilterType)
                        {
                            case DrawingParameters.EdgeFilterType.EdgeDetectMono:
                                blue = green = red = 255;
                                break;
                            case DrawingParameters.EdgeFilterType.EdgeDetectGradient:
                                blue = blueGradient * parameters.BlueFactor;
                                green = greenGradient * parameters.GreenFactor;
                                red = redGradient * parameters.RedFactor;
                                break;
                            case DrawingParameters.EdgeFilterType.Sharpen:
                                blue = pixelBuffer[byteOffset] * parameters.BlueFactor;
                                green = pixelBuffer[byteOffset + 1] * parameters.GreenFactor;
                                red = pixelBuffer[byteOffset + 2] * parameters.RedFactor;
                                break;
                            case DrawingParameters.EdgeFilterType.SharpenGradient:
                                blue = pixelBuffer[byteOffset] + (blueGradient * parameters.BlueFactor);
                                green = pixelBuffer[byteOffset + 1] + (greenGradient * parameters.GreenFactor);
                                red = pixelBuffer[byteOffset + 2] + (redGradient * parameters.RedFactor);
                                break;
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

                    // thresholds checks and guards
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