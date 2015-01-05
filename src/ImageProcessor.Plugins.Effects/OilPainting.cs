namespace ImageProcessor.Plugins.Effects
{
    using System;

    /// <summary>
    /// Oil painting filter
    /// <see href="http://softwarebydefault.com/2013/06/29/oil-painting-cartoon-filter/" />
    /// </summary>
    public class OilPainting : ProcessorBase
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
            OilPaintingParameters parameters = this.DynamicParameter;

            int levels = parameters.Levels;

            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            levels = levels - 1;

            int filterOffset = (parameters.FilterSize - 1) / 2;

            for (int offsetY = filterOffset; offsetY < sourceHeight - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceWidth - filterOffset; offsetX++)
                {
                    int maxIntensity = 0;
                    int maxIndex = 0;

                    int[] intensityBin = new int[levels + 1];
                    int[] blueBin = new int[levels + 1];
                    int[] greenBin = new int[levels + 1];
                    int[] redBin = new int[levels + 1];

                    int byteOffset = offsetY * sourceStride + offsetX * 4;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            int calcOffset = byteOffset + (filterX * 4) + (filterY * sourceStride);

                            int currentIntensity = (int)Math.Round(((double)(pixelBuffer[calcOffset] + pixelBuffer[calcOffset + 1] + pixelBuffer[calcOffset + 2]) / 3.0 * (levels)) / 255.0);

                            intensityBin[currentIntensity] += 1;
                            blueBin[currentIntensity] += pixelBuffer[calcOffset];
                            greenBin[currentIntensity] += pixelBuffer[calcOffset + 1];
                            redBin[currentIntensity] += pixelBuffer[calcOffset + 2];

                            if (intensityBin[currentIntensity] > maxIntensity)
                            {
                                maxIntensity = intensityBin[currentIntensity];
                                maxIndex = currentIntensity;
                            }
                        }
                    }

                    double blue = blueBin[maxIndex] / maxIntensity;
                    double green = greenBin[maxIndex] / maxIntensity;
                    double red = redBin[maxIndex] / maxIntensity;

                    resultBuffer[byteOffset] = Guard(blue);
                    resultBuffer[byteOffset + 1] = Guard(green);
                    resultBuffer[byteOffset + 2] = Guard(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            return resultBuffer;
        }
    }
}