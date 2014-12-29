namespace ImageProcessor.Plugins.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Plugins.Effects.Imaging;
    using ImageProcessor.Processors;

    /// <summary>
    /// Substitutes a color for another
    /// http://softwarebydefault.com/2013/03/16/bitmap-color-substitution/
    /// </summary>
    public class ColorSubstitution : ProcessorBase
    {
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

            byte minValue = 0;
            byte maxValue = 255;

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

                        if (resultBlue > maxValue)
                        {
                            resultBlue = maxValue;
                        }
                        else if (resultBlue < minValue)
                        {
                            resultBlue = minValue;
                        }

                        resultGreen = greenFilter - sourceGreen + newGreenValue;

                        if (resultGreen > maxValue)
                        {
                            resultGreen = maxValue;
                        }
                        else if (resultGreen < minValue)
                        {
                            resultGreen = minValue;
                        }

                        resultRed = redFilter - sourceRed + newRedValue;

                        if (resultRed > maxValue)
                        {
                            resultRed = maxValue;
                        }
                        else if (resultRed < minValue)
                        {
                            resultRed = minValue;
                        }

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