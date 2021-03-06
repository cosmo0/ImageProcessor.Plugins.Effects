﻿namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Provides color balance modifications
    /// <see href="http://softwarebydefault.com/2013/04/11/bitmap-color-balance/" />
    /// </summary>
    public class ColorBalance : ProcessorBase
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
            ColorBalanceParameters parameters = this.DynamicParameter;

            float blueLevelFloat = parameters.Blue;
            float greenLevelFloat = parameters.Green;
            float redLevelFloat = parameters.Red;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                float blue = Guard(255.0f / blueLevelFloat * (float)pixelBuffer[k]);
                float green = Guard(255.0f / greenLevelFloat * (float)pixelBuffer[k + 1]);
                float red = Guard(255.0f / redLevelFloat * (float)pixelBuffer[k + 2]);

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            return pixelBuffer;
        }
    }
}