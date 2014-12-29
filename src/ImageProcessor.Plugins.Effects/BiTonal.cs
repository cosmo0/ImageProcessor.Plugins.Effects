namespace ImageProcessor.Plugins.Effects
{
    using System.Drawing;

    /// <summary>
    /// Bi-Tonal bitmap filter
    /// http://softwarebydefault.com/2013/04/12/bitonal-bitmaps/
    /// </summary>
    public class BiTonal : ProcessorBase
    {
        /// <summary>
        /// Process the specified image using a pixel buffer.
        /// </summary>
        /// <param name="pixelBuffer">The pixel buffer.</param>
        protected override byte[] Process(byte[] pixelBuffer, int sourceWidth, int sourceHeight, int sourceStride)
        {
            BiTonalParameters parameters = this.DynamicParameter;

            Color darkColor = parameters.DarkColor;
            Color lightColor = parameters.LightColor;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                if (pixelBuffer[k] + pixelBuffer[k + 1] +
                    pixelBuffer[k + 2] <= parameters.Threshold)
                {
                    pixelBuffer[k] = darkColor.B;
                    pixelBuffer[k + 1] = darkColor.G;
                    pixelBuffer[k + 2] = darkColor.R;
                }
                else
                {
                    pixelBuffer[k] = lightColor.B;
                    pixelBuffer[k + 1] = lightColor.G;
                    pixelBuffer[k + 2] = lightColor.R;
                }
            }

            return pixelBuffer;
        }
    }
}