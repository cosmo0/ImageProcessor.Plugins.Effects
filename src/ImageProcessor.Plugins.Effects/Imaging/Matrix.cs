namespace ImageProcessor.Plugins.Effects.Imaging
{
    /// <summary>
    /// Stores smoothing matrixes
    /// </summary>
    public static class Matrix
    {
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