﻿namespace ImageProcessor.Plugins.Effects.Imaging
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
            ConvolutionParameters parameters = new ConvolutionParameters
            {
                Bias = bias,
                Factor = factor,
                Matrix = filterMatrix
            };

            Convolution filter = new Convolution();
            filter.DynamicParameter = parameters;
            return filter.ProcessBitmap(sourceBitmap);
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
            GradientBasedEdgeParameters parameters = new GradientBasedEdgeParameters
            {
                EdgeColor = edgeColour,
                Threshold = threshold
            };

            GradientBasedEdge filter = new GradientBasedEdge();
            filter.DynamicParameter = parameters;
            return filter.ProcessBitmap(sourceBitmap);
        }

        /// <summary>
        /// Applies a median filter to a bitmap
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap</param>
        /// <param name="matrixSize">The matrix size</param>
        /// <returns>The modified bitmap</returns>
        public static Bitmap MedianFilter(this Bitmap sourceBitmap, int matrixSize)
        {
            Median filter = new Median();
            filter.DynamicParameter = new MedianParameters { MatrixSize = matrixSize };

            return filter.ProcessBitmap(sourceBitmap);
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
    }
}