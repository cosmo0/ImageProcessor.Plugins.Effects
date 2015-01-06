namespace ImageProcessor.Plugins.Effects.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Extension methods for bitmaps
    /// </summary>
    /// <remarks>
    /// Copied from samples on <see href="http://softwarebydefault.com/" />
    /// </remarks>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Applies a convolution filter to a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="filterMatrix">The filter matrix</param>
        /// <param name="factor">The convolution factor</param>
        /// <param name="bias">The convolution bias</param>
        /// <returns>The processed bitmap</returns>
        public static Bitmap ConvolutionFilter(this Bitmap sourceBitmap, double[,] filterMatrix, double factor = 1, int bias = 0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            int filterWidth = filterMatrix.GetLength(1);
            int filterOffset = (filterWidth - 1) / 2;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    double blue = 0.0;
                    double green = 0.0;
                    double red = 0.0;

                    int byteOffset = (offsetY * sourceData.Stride) + (offsetX * 4);

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            int calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            blue += (double)pixelBuffer[calcOffset] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            green += (double)pixelBuffer[calcOffset + 1] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            red += (double)pixelBuffer[calcOffset + 2] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blue = (factor * blue) + bias;
                    green = (factor * green) + bias;
                    red = (factor * red) + bias;

                    blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
                    green = green > 255 ? 255 : (green < 0 ? 0 : green);
                    red = red > 255 ? 255 : (red < 0 ? 0 : red);

                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
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
        /// Converts a bitmap to a 32-bit ARGB bitmap
        /// </summary>
        /// <param name="sourceBitmap">The bitmap to convert</param>
        /// <returns>The converted bitmap</returns>
        public static Bitmap Format32bppArgbCopy(this Bitmap sourceBitmap)
        {
            Bitmap copyBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphicsObject = Graphics.FromImage(copyBitmap))
            {
                graphicsObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicsObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                graphicsObject.DrawImage(
                    sourceBitmap,
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    GraphicsUnit.Pixel);
            }

            return copyBitmap;
        }

        /// <summary>
        /// Calculates gradient-based edge detection on an image
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="edgeColour">The color of the edges</param>
        /// <param name="threshold">The gradient threshold</param>
        /// <returns>The modified image</returns>
        public static Bitmap GradientBasedEdgeDetectionFilter(this Bitmap sourceBitmap, Color edgeColour, byte threshold = 0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            for (int offsetY = 1; offsetY < sourceBitmap.Height - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < sourceBitmap.Width - 1; offsetX++)
                {
                    int sourceOffset = (offsetY * sourceData.Stride) + (offsetX * 4);
                    int gradientValue = 0;
                    bool exceedsThreshold = true;

                    // Horizontal Gradient
                    CheckThreshold(
                        pixelBuffer,
                        sourceOffset - 4,
                        sourceOffset + 4,
                        ref gradientValue,
                        threshold,
                        2);

                    // Vertical Gradient
                    exceedsThreshold = CheckThreshold(
                        pixelBuffer,
                        sourceOffset - sourceData.Stride,
                        sourceOffset + sourceData.Stride,
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
                                sourceOffset - sourceData.Stride,
                                sourceOffset + sourceData.Stride,
                                ref gradientValue,
                                threshold);

                            if (!exceedsThreshold)
                            {
                                gradientValue = 0;

                                // Diagonal Gradient : NW-SE
                                CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - 4 - sourceData.Stride,
                                    sourceOffset + 4 + sourceData.Stride,
                                    ref gradientValue,
                                    threshold,
                                    2);

                                // Diagonal Gradient : NE-SW
                                exceedsThreshold = CheckThreshold(
                                    pixelBuffer,
                                    sourceOffset - sourceData.Stride + 4,
                                    sourceOffset - 4 + sourceData.Stride,
                                    ref gradientValue,
                                    threshold,
                                    2);

                                if (!exceedsThreshold)
                                {
                                    gradientValue = 0;

                                    // Diagonal Gradient : NW-SE
                                    exceedsThreshold = CheckThreshold(
                                        pixelBuffer,
                                        sourceOffset - 4 - sourceData.Stride,
                                        sourceOffset + 4 + sourceData.Stride,
                                        ref gradientValue,
                                        threshold);

                                    if (!exceedsThreshold)
                                    {
                                        gradientValue = 0;

                                        // Diagonal Gradient : NE-SW
                                        exceedsThreshold = CheckThreshold(
                                            pixelBuffer,
                                            sourceOffset - sourceData.Stride + 4,
                                            sourceOffset + sourceData.Stride - 4,
                                            ref gradientValue,
                                            threshold);
                                    }
                                }
                            }
                        }
                    }

                    if (exceedsThreshold)
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
        /// Applies a median filter to a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="matrixSize">The matrix size</param>
        /// <returns>The modified bitmap</returns>
        public static Bitmap MedianFilter(this Bitmap sourceBitmap, int matrixSize)
        {
            BitmapData sourceData = sourceBitmap.LockBits(
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = (offsetY * sourceData.Stride) + (offsetX * 4);
                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceData.Stride);
                            neighbourPixels.Add(BitConverter.ToInt32(pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();
                    middlePixel = BitConverter.GetBytes(neighbourPixels[filterOffset]);

                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
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
        /// Scales a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The bitmap to scale</param>
        /// <param name="factor">The scale factor (0-1)</param>
        /// <returns>The scaled bitmap</returns>
        public static Bitmap ScaleBitmap(this Bitmap sourceBitmap, float factor)
        {
            Bitmap resultBitmap = new Bitmap(
                (int)(sourceBitmap.Width * factor),
                (int)(sourceBitmap.Height * factor),
                PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(resultBitmap);

            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            graphics.DrawImage(
                sourceBitmap,
                new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                GraphicsUnit.Pixel);

            return resultBitmap;
        }

        /// <summary>
        /// Applies a smoothing filter to a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="smoothFilter">The filter to apply</param>
        /// <returns>The smoothed bitmap</returns>
        public static Bitmap SmoothingFilter(this Bitmap sourceBitmap, SmoothingFilterType smoothFilter = SmoothingFilterType.None)
        {
            Bitmap inputBitmap = null;

            switch (smoothFilter)
            {
                case SmoothingFilterType.None:
                    inputBitmap = sourceBitmap;
                    break;

                case SmoothingFilterType.Gaussian3x3:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Gaussian3x3, 1.0 / 16.0, 0);
                    break;

                case SmoothingFilterType.Gaussian5x5:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Gaussian5x5, 1.0 / 159.0, 0);
                    break;

                case SmoothingFilterType.Gaussian7x7:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Gaussian7x7, 1.0 / 136.0, 0);
                    break;

                case SmoothingFilterType.Median3x3:
                    inputBitmap = sourceBitmap.MedianFilter(3);
                    break;

                case SmoothingFilterType.Median5x5:
                    inputBitmap = sourceBitmap.MedianFilter(5);
                    break;

                case SmoothingFilterType.Median7x7:
                    inputBitmap = sourceBitmap.MedianFilter(7);
                    break;

                case SmoothingFilterType.Median9x9:
                    inputBitmap = sourceBitmap.MedianFilter(9);
                    break;

                case SmoothingFilterType.Mean3x3:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Mean3x3, 1.0 / 9.0, 0);
                    break;

                case SmoothingFilterType.Mean5x5:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Mean5x5, 1.0 / 25.0, 0);
                    break;

                case SmoothingFilterType.LowPass3x3:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.LowPass3x3, 1.0 / 16.0, 0);
                    break;

                case SmoothingFilterType.LowPass5x5:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.LowPass5x5, 1.0 / 60.0, 0);
                    break;

                case SmoothingFilterType.Sharpen3x3:
                    inputBitmap = sourceBitmap.ConvolutionFilter(Matrix.Sharpen3x3, 1.0 / 8.0, 0);
                    break;
            }

            return inputBitmap;
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