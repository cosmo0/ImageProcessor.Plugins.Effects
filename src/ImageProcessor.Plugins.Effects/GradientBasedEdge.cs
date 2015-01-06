namespace ImageProcessor.Plugins.Effects
{
    using System;

    /// <summary>
    /// Provides a gradient-based edge detection filter
    /// <see href="http://softwarebydefault.com/" />
    /// </summary>
    public class GradientBasedEdge : ProcessorBase
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
            GradientBasedEdgeParameters parameters = this.DynamicParameter;
            byte threshold = parameters.Threshold;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];
            Array.Copy(pixelBuffer, resultBuffer, resultBuffer.Length);

            for (int offsetY = 1; offsetY < sourceHeight - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceWidth - 1; offsetX++)
                {
                    int sourceOffset = (offsetY * sourceStride) + (offsetX * 4);
                    int gradientValue = 0;

                    // Horizontal Gradient
                    bool exceedsThreshold = CheckThreshold(
                        pixelBuffer,
                        sourceOffset - 4,
                        sourceOffset + 4,
                        ref gradientValue,
                        threshold,
                        2);

                    // Vertical Gradient
                    exceedsThreshold = CheckThreshold(
                        pixelBuffer,
                        sourceOffset - sourceStride,
                        sourceOffset + sourceStride,
                        ref gradientValue,
                        threshold,
                        2);

                    if (!exceedsThreshold)
                    {
                        gradientValue = 0;

                        // Horizontal Gradient
                        exceedsThreshold = CheckThreshold(
                            pixelBuffer,
                            sourceOffset - 4,
                            sourceOffset + 4,
                            ref gradientValue,
                            threshold);

                        if (!exceedsThreshold)
                        {
                            gradientValue = 0;

                            // Vertical Gradient
                            exceedsThreshold = CheckThreshold(
                                pixelBuffer,
                                sourceOffset - sourceStride,
                                sourceOffset + sourceStride,
                                ref gradientValue,
                                threshold);

                            if (!exceedsThreshold)
                            {
                                gradientValue = 0;

                                // Diagonal Gradient : NW-SE
                                CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - 4 - sourceStride,
                                    sourceOffset + 4 + sourceStride,
                                    ref gradientValue,
                                    threshold,
                                    2);

                                // Diagonal Gradient : NE-SW
                                exceedsThreshold = CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - sourceStride + 4,
                                    sourceOffset - 4 + sourceStride,
                                    ref gradientValue,
                                    threshold,
                                    2);

                                if (!exceedsThreshold)
                                {
                                    gradientValue = 0;

                                    // Diagonal Gradient : NW-SE
                                    exceedsThreshold = CheckThreshold(
                                        pixelBuffer,
                                        sourceOffset - 4 - sourceStride,
                                        sourceOffset + 4 + sourceStride,
                                        ref gradientValue,
                                        threshold);

                                    if (!exceedsThreshold)
                                    {
                                        gradientValue = 0;

                                        // Diagonal Gradient : NE-SW
                                        exceedsThreshold = CheckThreshold(
                                            pixelBuffer,
                                            sourceOffset - sourceStride + 4,
                                            sourceOffset + sourceStride - 4,
                                            ref gradientValue,
                                            threshold);
                                    }
                                }
                            }
                        }
                    }

                    if (exceedsThreshold)
                    {
                        resultBuffer[sourceOffset] = parameters.EdgeColor.B;
                        resultBuffer[sourceOffset + 1] = parameters.EdgeColor.G;
                        resultBuffer[sourceOffset + 2] = parameters.EdgeColor.R;
                    }

                    resultBuffer[sourceOffset + 3] = 255;
                }
            }

            return resultBuffer;
        }

        /// <summary>
        /// Checks the threshold of a pixel
        /// </summary>
        /// <param name="pixelBuffer">The pixel values</param>
        /// <param name="offset1">The first offset</param>
        /// <param name="offset2">The second offset</param>
        /// <param name="gradientValue">The gradient value</param>
        /// <param name="threshold">The threshold</param>
        /// <param name="divideBy">The division value</param>
        /// <returns>Whether the pixel matches the threshold</returns>
        private static bool CheckThreshold(byte[] pixelBuffer, int offset1, int offset2, ref int gradientValue, byte threshold, int divideBy = 1)
        {
            gradientValue += Math.Abs(pixelBuffer[offset1] - pixelBuffer[offset2]) / divideBy;
            gradientValue += Math.Abs(pixelBuffer[offset1 + 1] - pixelBuffer[offset2 + 1]) / divideBy;
            gradientValue += Math.Abs(pixelBuffer[offset1 + 2] - pixelBuffer[offset2 + 2]) / divideBy;

            return gradientValue >= threshold;
        }
    }
}