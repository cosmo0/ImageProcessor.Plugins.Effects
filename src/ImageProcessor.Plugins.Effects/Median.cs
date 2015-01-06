namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a median filter
    /// <see href="http://softwarebydefault.com/" />
    /// </summary>
    public class Median : ProcessorBase
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
            MedianParameters parameters = this.DynamicParameter;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            int filterOffset = (parameters.MatrixSize - 1) / 2;
            int calcOffset = 0;
            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY < sourceHeight - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceWidth - filterOffset; offsetX++)
                {
                    byteOffset = (offsetY * sourceStride) + (offsetX * 4);
                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 4) + (filterY * sourceStride);
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

            return resultBuffer;
        }
    }
}