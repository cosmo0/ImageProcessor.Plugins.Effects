namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Stores smoothing matrixes
    /// </summary>
    public static class Matrix
    {
        /// <summary>
        /// Gets a 3x3 gaussian matrix
        /// </summary>
        public static double[,] Gaussian3x3
        {
            get
            {
                return new double[,]
                 { { 1, 2, 1, },
                   { 2, 4, 2, },
                   { 1, 2, 1, }, };
            }
        }

        /// <summary>
        /// Gets a 5x5 gaussian matrix
        /// </summary>
        public static double[,] Gaussian5x5
        {
            get
            {
                return new double[,]
                 { { 2, 04, 05, 04, 2  },
                   { 4, 09, 12, 09, 4  },
                   { 5, 12, 15, 12, 5  },
                   { 4, 09, 12, 09, 4  },
                   { 2, 04, 05, 04, 2  }, };
            }
        }

        /// <summary>
        /// Gets a 7x7 gaussian matrix
        /// </summary>
        public static double[,] Gaussian7x7
        {
            get
            {
                return new double[,]
                 { { 1,  1,  2,  2,  2,  1,  1, },
                   { 1,  2,  2,  4,  2,  2,  1, },
                   { 2,  2,  4,  8,  4,  2,  2, },
                   { 2,  4,  8, 16,  8,  4,  2, },
                   { 2,  2,  4,  8,  4,  2,  2, },
                   { 1,  2,  2,  4,  2,  2,  1, },
                   { 1,  1,  2,  2,  2,  1,  1, }, };
            }
        }

        /// <summary>
        /// Gets a 3x3 mean matrix
        /// </summary>
        public static double[,] Mean3x3
        {
            get
            {
                return new double[,]
                 { { 1, 1, 1, },
                   { 1, 1, 1, },
                   { 1, 1, 1, }, };
            }
        }

        /// <summary>
        /// Gets a 5x5 mean matrix
        /// </summary>
        public static double[,] Mean5x5
        {
            get
            {
                return new double[,]
                 { { 1, 1, 1, 1, 1, },
                   { 1, 1, 1, 1, 1, },
                   { 1, 1, 1, 1, 1, },
                   { 1, 1, 1, 1, 1, },
                   { 1, 1, 1, 1, 1, }, };
            }
        }

        /// <summary>
        /// Gets a 3x3 low pass matrix
        /// </summary>
        public static double[,] LowPass3x3
        {
            get
            {
                return new double[,]
                 { { 1, 2, 1, },
                   { 2, 4, 2, },
                   { 1, 2, 1, }, };
            }
        }

        /// <summary>
        /// Gets a 5x5 low pass matrix
        /// </summary>
        public static double[,] LowPass5x5
        {
            get
            {
                return new double[,]
                 { { 1, 1,  1, 1, 1, },
                   { 1, 4,  4, 4, 1, },
                   { 1, 4, 12, 4, 1, },
                   { 1, 4,  4, 4, 1, },
                   { 1, 1,  1, 1, 1, }, };
            }
        }

        /// <summary>
        /// Gets a 3x3 sharpen matrix
        /// </summary>
        public static double[,] Sharpen3x3
        {
            get
            {
                return new double[,]
                 { { -1, -2, -1, },
                   {  2,  4,  2, },
                   {  1,  2,  1, }, };
            }
        }
    }
}