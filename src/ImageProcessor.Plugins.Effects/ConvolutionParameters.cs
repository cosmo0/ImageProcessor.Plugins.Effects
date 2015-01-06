namespace ImageProcessor.Plugins.Effects
{
    internal class ConvolutionParameters
    {
        public ConvolutionParameters()
        {
            this.Matrix = Imaging.Matrix.Gaussian5x5;
            this.Factor = 1;
            this.Bias = 0;
        }

        public int Bias { get; set; }

        public double Factor { get; set; }

        public double[,] Matrix { get; set; }
    }
}