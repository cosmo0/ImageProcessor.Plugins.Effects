namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Provides a convolution filter
    /// <see href="http://softwarebydefault.com/" />
    /// </summary>
    public class Convolution : ProcessorBase
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
            byte[] resultBuffer = new byte[sourceStride * sourceHeight];

            ConvolutionParameters parameters = this.DynamicParameter;
            double[,] filterMatrix = parameters.Matrix;

            int filterWidth = filterMatrix.GetLength(1);
            int filterOffset = (filterWidth - 1) / 2;

            for (int offsetY = filterOffset; offsetY < sourceHeight - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceWidth - filterOffset; offsetX++)
                {
                    double blue = 0.0;
                    double green = 0.0;
                    double red = 0.0;

                    int byteOffset = (offsetY * sourceStride) + (offsetX * 4);

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            int calcOffset = byteOffset + (filterX * 4) + (filterY * sourceStride);
                            blue += (double)pixelBuffer[calcOffset] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            green += (double)pixelBuffer[calcOffset + 1] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                            red += (double)pixelBuffer[calcOffset + 2] * filterMatrix[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    blue = (parameters.Factor * blue) + parameters.Bias;
                    green = (parameters.Factor * green) + parameters.Bias;
                    red = (parameters.Factor * red) + parameters.Bias;

                    blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
                    green = green > 255 ? 255 : (green < 0 ? 0 : green);
                    red = red > 255 ? 255 : (red < 0 ? 0 : red);

                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }

            return resultBuffer;
        }
    }
}