namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Substitutes a color for another
    /// http://softwarebydefault.com/2013/03/16/bitmap-color-substitution/
    /// </summary>
    public class ColorSubstitution : ProcessorBase
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
            ColorSubstitutionParameters parameters = this.DynamicParameter;

            byte sourceRed = 0, sourceGreen = 0, sourceBlue = 0, sourceAlpha = 0;
            int resultRed = 0, resultGreen = 0, resultBlue = 0;

            byte newRedValue = parameters.ReplaceWith.R;
            byte newGreenValue = parameters.ReplaceWith.G;
            byte newBlueValue = parameters.ReplaceWith.B;

            byte redFilter = parameters.ToReplace.R;
            byte greenFilter = parameters.ToReplace.G;
            byte blueFilter = parameters.ToReplace.B;

            for (int k = 0; k < pixelBuffer.Length; k += 4)
            {
                sourceAlpha = pixelBuffer[k + 3];

                if (sourceAlpha != 0)
                {
                    sourceBlue = pixelBuffer[k];
                    sourceGreen = pixelBuffer[k + 1];
                    sourceRed = pixelBuffer[k + 2];

                    if ((sourceBlue < blueFilter + parameters.Threshold &&
                         sourceBlue > blueFilter - parameters.Threshold) &&
                        (sourceGreen < greenFilter + parameters.Threshold &&
                         sourceGreen > greenFilter - parameters.Threshold) &&
                        (sourceRed < redFilter + parameters.Threshold &&
                         sourceRed > redFilter - parameters.Threshold))
                    {
                        resultBlue = blueFilter - sourceBlue + newBlueValue;
                        resultBlue = Guard(resultBlue);

                        resultGreen = greenFilter - sourceGreen + newGreenValue;
                        resultGreen = Guard(resultGreen);

                        resultRed = redFilter - sourceRed + newRedValue;
                        resultRed = Guard(resultRed);

                        pixelBuffer[k] = (byte)resultBlue;
                        pixelBuffer[k + 1] = (byte)resultGreen;
                        pixelBuffer[k + 2] = (byte)resultRed;
                        pixelBuffer[k + 3] = sourceAlpha;
                    }
                }
            }

            return pixelBuffer;
        }
    }
}