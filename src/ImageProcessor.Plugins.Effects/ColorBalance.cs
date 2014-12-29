namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Provides color balance modifications
    /// </summary>
    /// <remarks>
    /// Shameless copypasta from http://softwarebydefault.com/2013/04/11/bitmap-color-balance/
    /// </remarks>
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
                float blue = 255.0f / blueLevelFloat * (float)pixelBuffer[k];
                float green = 255.0f / greenLevelFloat * (float)pixelBuffer[k + 1];
                float red = 255.0f / redLevelFloat * (float)pixelBuffer[k + 2];

                if (blue > 255)
                {
                    blue = 255;
                }
                else if (blue < 0)
                {
                    blue = 0;
                }

                if (green > 255)
                {
                    green = 255;
                }
                else if (green < 0)
                {
                    green = 0;
                }

                if (red > 255)
                {
                    red = 255;
                }
                else if (red < 0)
                {
                    red = 0;
                }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }

            return pixelBuffer;
        }
    }
}