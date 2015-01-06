namespace ImageProcessor.Plugins.Effects
{
    /// <summary>
    /// Provides parameters to the convolution filter
    /// </summary>
    public class ConvolutionParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvolutionParameters"/> class
        /// </summary>
        public ConvolutionParameters()
        {
            this.Matrix = Imaging.Matrix.Gaussian5x5;
            this.Factor = 1;
            this.Bias = 0;
        }

        /// <summary>
        /// Gets or sets the convolution bias
        /// </summary>
        public int Bias { get; set; }

        /// <summary>
        /// Gets or sets the convolution factor
        /// </summary>
        public double Factor { get; set; }

        /// <summary>
        /// Gets or sets the smoothing matrix
        /// </summary>
        public double[,] Matrix { get; set; }
    }
}