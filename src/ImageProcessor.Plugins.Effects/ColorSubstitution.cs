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
    public class ColorSubstitution : IGraphicsProcessor
    {
        public dynamic DynamicParameter { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public Image ProcessImage(ImageFactory factory)
        {
            Bitmap sourceBitmap = (new Bitmap(factory.Image)).Format32bppArgbCopy();
            ColorSubstitutionParameters parameters = this.DynamicParameter;

            try
            {
                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);

                BitmapData sourceData = sourceBitmap.LockBits(
                    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);
                BitmapData resultData = resultBitmap.LockBits(
                    new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppArgb);

                byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];
                Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);

                sourceBitmap.UnlockBits(sourceData);

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

                for (int k = 0; k < resultBuffer.Length; k += 4)
                {
                    sourceAlpha = resultBuffer[k + 3];

                    if (sourceAlpha != 0)
                    {
                        sourceBlue = resultBuffer[k];
                        sourceGreen = resultBuffer[k + 1];
                        sourceRed = resultBuffer[k + 2];

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

                            resultBuffer[k] = (byte)resultBlue;
                            resultBuffer[k + 1] = (byte)resultGreen;
                            resultBuffer[k + 2] = (byte)resultRed;
                            resultBuffer[k + 3] = sourceAlpha;
                        }
                    }
                }

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                sourceBitmap = resultBitmap;
            }
            catch (Exception ex)
            {
                if (sourceBitmap != null)
                {
                    sourceBitmap.Dispose();
                }

                throw new ImageProcessingException("Error processing image with " + this.GetType().Name, ex);
            }

            return sourceBitmap;
        }
    }
}