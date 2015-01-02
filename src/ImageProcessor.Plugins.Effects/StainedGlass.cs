namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using ImageProcessor.Plugins.Effects.Imaging;

    /// <summary>
    /// Stained glass-style filter processor
    /// http://softwarebydefault.com/2013/06/30/stained-glass-image-filter/
    /// </summary>
    public class StainedGlass : ProcessorBase
    {
        /// <summary>
        /// Defines a list of square roots (so as to not calculate them every time)
        /// </summary>
        private static readonly Dictionary<int, int> SquareRoots = new Dictionary<int, int>();

        /// <summary>
        /// Post-processes the result bitmap
        /// </summary>
        /// <param name="resultBitmap">The result bitmap to post-process</param>
        /// <returns>The processed bitmap</returns>
        protected override Bitmap PostProcess(Bitmap resultBitmap)
        {
            StainedGlassParameters parameters = this.DynamicParameter;

            if (parameters.Edges)
            {
                return resultBitmap.GradientBasedEdgeDetectionFilter(parameters.EdgesColor, parameters.EdgesThreshold);
            }

            return resultBitmap;
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
            StainedGlassParameters parameters = this.DynamicParameter;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            int neighbourHoodTotal = 0;
            int sourceOffset = 0;
            int resultOffset = 0;
            int currentPixelDistance = 0;
            int nearestPixelDistance = 0;
            int nearesttPointIndex = 0;

            Random randomizer = new Random();
            List<VoronoiPoint> randomPointList = new List<VoronoiPoint>();
            int blockSize = parameters.Size;

            for (int row = 0; row < sourceHeight - blockSize; row += blockSize)
            {
                for (int col = 0; col < sourceWidth - blockSize; col += blockSize)
                {
                    sourceOffset = (row * sourceStride) + (col * 4);
                    neighbourHoodTotal = 0;

                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            resultOffset = sourceOffset + (y * sourceStride) + (x * 4);
                            neighbourHoodTotal += pixelBuffer[resultOffset];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 1];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 2];
                        }
                    }

                    randomizer = new Random(neighbourHoodTotal);
                    VoronoiPoint randomPoint = new VoronoiPoint();
                    randomPoint.XOffset = randomizer.Next(0, blockSize) + col;
                    randomPoint.YOffset = randomizer.Next(0, blockSize) + row;
                    randomPointList.Add(randomPoint);
                }
            }

            int rowOffset = 0;
            int colOffset = 0;

            for (int bufferOffset = 0; bufferOffset < pixelBuffer.Length - 4; bufferOffset += 4)
            {
                rowOffset = bufferOffset / sourceStride;
                colOffset = (bufferOffset % sourceStride) / 4;
                currentPixelDistance = 0;
                nearestPixelDistance = blockSize * 4;
                nearesttPointIndex = 0;
                List<VoronoiPoint> pointSubset = new List<VoronoiPoint>();

                pointSubset.AddRange(from t in randomPointList
                                     where
                                          rowOffset >= t.YOffset - (blockSize * 2) &&
                                          rowOffset <= t.YOffset + (blockSize * 2)
                                     select t);

                for (int k = 0; k < pointSubset.Count; k++)
                {
                    if (parameters.DistanceFormula == StainedGlassParameters.Formula.Euclidean)
                    {
                        currentPixelDistance = CalculateDistanceEuclidean(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }
                    else if (parameters.DistanceFormula == StainedGlassParameters.Formula.Manhattan)
                    {
                        currentPixelDistance = CalculateDistanceManhattan(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }
                    else if (parameters.DistanceFormula == StainedGlassParameters.Formula.Chebyshev)
                    {
                        currentPixelDistance = CalculateDistanceChebyshev(pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }

                    if (currentPixelDistance <= nearestPixelDistance)
                    {
                        nearestPixelDistance = currentPixelDistance;
                        nearesttPointIndex = k;

                        if (nearestPixelDistance <= blockSize / parameters.Factor)
                        {
                            break;
                        }
                    }
                }

                Pixel tmpPixel = new Pixel();
                tmpPixel.XOffset = colOffset;
                tmpPixel.YOffset = rowOffset;
                tmpPixel.Blue = pixelBuffer[bufferOffset];
                tmpPixel.Green = pixelBuffer[bufferOffset + 1];
                tmpPixel.Red = pixelBuffer[bufferOffset + 2];
                pointSubset[nearesttPointIndex].AddPixel(tmpPixel);
            }

            for (int k = 0; k < randomPointList.Count; k++)
            {
                randomPointList[k].CalculateAverages();

                for (int i = 0; i < randomPointList[k].PixelCollection.Count; i++)
                {
                    resultOffset = (randomPointList[k].PixelCollection[i].YOffset * sourceStride) + (randomPointList[k].PixelCollection[i].XOffset * 4);
                    resultBuffer[resultOffset] = (byte)randomPointList[k].BlueAverage;
                    resultBuffer[resultOffset + 1] = (byte)randomPointList[k].GreenAverage;
                    resultBuffer[resultOffset + 2] = (byte)randomPointList[k].RedAverage;
                    resultBuffer[resultOffset + 3] = 255;
                }
            }

            return resultBuffer;
        }

        /// <summary>
        /// Calculates a distance using the Chebyshev method
        /// </summary>
        /// <param name="x1">The starting X</param>
        /// <param name="x2">The ending X</param>
        /// <param name="y1">The starting Y</param>
        /// <param name="y2">The ending Y</param>
        /// <returns>The calculated distance</returns>
        private static int CalculateDistanceChebyshev(int x1, int x2, int y1, int y2)
        {
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }

        /// <summary>
        /// Calculates a distance using the Euclidian method
        /// </summary>
        /// <param name="x1">The starting X</param>
        /// <param name="x2">The ending X</param>
        /// <param name="y1">The starting Y</param>
        /// <param name="y2">The ending Y</param>
        /// <returns>The calculated distance</returns>
        private static int CalculateDistanceEuclidean(int x1, int x2, int y1, int y2)
        {
            int square = ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2));

            if (!SquareRoots.ContainsKey(square))
            {
                SquareRoots.Add(square, (int)Math.Sqrt(square));
            }

            return SquareRoots[square];
        }

        /// <summary>
        /// Calculates a distance using the Manhattan method
        /// </summary>
        /// <param name="x1">The starting X</param>
        /// <param name="x2">The ending X</param>
        /// <param name="y1">The starting Y</param>
        /// <param name="y2">The ending Y</param>
        /// <returns>The calculated distance</returns>
        private static int CalculateDistanceManhattan(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}