namespace ImageProcessor.Plugins.Effects.Imaging
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Extension methods for edge detection
    /// </summary>
    public static class EdgeDetection
    {
        /// <summary>
        /// Calculates gradient-based edge detection on an image
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="edgeColour">The color of the edges</param>
        /// <param name="threshold">The gradient threshold</param>
        /// <returns>The modified image</returns>
        public static Bitmap GradientBasedEdgeDetectionFilter(this Bitmap sourceBitmap, Color edgeColour, byte threshold = 0)
        {
            BitmapData sourceData =
                sourceBitmap.LockBits(new Rectangle(0, 0,
                    sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            int sourceOffset = 0, gradientValue = 0;
            bool exceedsThreshold = false;

            for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                {
                    sourceOffset = offsetY * sourceData.Stride + offsetX * 4;
                    gradientValue = 0;
                    exceedsThreshold = true;

                    // Horizontal Gradient 
                    CheckThreshold(
                        pixelBuffer,
                        sourceOffset - 4,
                        sourceOffset + 4,
                        ref gradientValue, threshold, 2);

                    // Vertical Gradient 
                    exceedsThreshold = CheckThreshold(
                        pixelBuffer,
                        sourceOffset - sourceData.Stride,
                        sourceOffset + sourceData.Stride,
                        ref gradientValue, threshold, 2);

                    if (exceedsThreshold == false)
                    {
                        gradientValue = 0;

                        // Horizontal Gradient 
                        exceedsThreshold = CheckThreshold(
                            pixelBuffer,
                            sourceOffset - 4,
                            sourceOffset + 4,
                            ref gradientValue, threshold);

                        if (exceedsThreshold == false)
                        {
                            gradientValue = 0;

                            // Vertical Gradient 
                            exceedsThreshold = CheckThreshold(
                                pixelBuffer,
                                sourceOffset - sourceData.Stride,
                                sourceOffset + sourceData.Stride,
                                ref gradientValue, threshold);

                            if (exceedsThreshold == false)
                            {
                                gradientValue = 0;

                                // Diagonal Gradient : NW-SE 
                                CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - 4 - sourceData.Stride,
                                    sourceOffset + 4 + sourceData.Stride,
                                    ref gradientValue, threshold, 2);

                                // Diagonal Gradient : NE-SW 
                                exceedsThreshold = CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - sourceData.Stride + 4,
                                    sourceOffset - 4 + sourceData.Stride,
                                    ref gradientValue, threshold, 2);

                                if (exceedsThreshold == false)
                                {
                                    gradientValue = 0;

                                    // Diagonal Gradient : NW-SE 
                                    exceedsThreshold = CheckThreshold(
                                        pixelBuffer,
                                        sourceOffset - 4 - sourceData.Stride,
                                        sourceOffset + 4 + sourceData.Stride,
                                        ref gradientValue, threshold);

                                    if (exceedsThreshold == false)
                                    {
                                        gradientValue = 0;

                                        // Diagonal Gradient : NE-SW 
                                        exceedsThreshold = CheckThreshold(
                                            pixelBuffer,
                                            sourceOffset - sourceData.Stride + 4,
                                            sourceOffset + sourceData.Stride - 4,
                                            ref gradientValue, threshold);
                                    }
                                }
                            }
                        }
                    }

                    if (exceedsThreshold == true)
                    {
                        resultBuffer[sourceOffset] = edgeColour.B;
                        resultBuffer[sourceOffset + 1] = edgeColour.G;
                        resultBuffer[sourceOffset + 2] = edgeColour.R;
                    }

                    resultBuffer[sourceOffset + 3] = 255;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
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

            return (gradientValue >= threshold);
        }
    }
}