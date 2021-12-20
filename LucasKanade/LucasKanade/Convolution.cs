using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LucasKanade
{
    public class Convolution
    {
        
        // Можно играться с делением этой матрицы. Будут разные резы
        public static double[,] StandartCoreX = new double[,]
        {
            {-1, 1},
            {-1, 1}
        };
        
        // Можно играться с делением этой матрицы. Будут разные резы
        public static double[,] StandartCoreY = new double[,]
        {
            {-1, -1},
            {1, 1}
        };
        
        
        // Можно играться с делением этой матрицы. Будут разные резы
        public static double[,] StandartCoreXLol = new double[,]
        {
            {1, 1},
            {-1, -1}
        };
        
        // Можно играться с делением этой матрицы. Будут разные резы
        public static double[,] StandartCoreYLol = new double[,]
        {
            {1, -1},
            {1, -1}
        };
        
        public static double[,] SobelX = new double[,]
        {
            {1, 0, -1},
            {2, 0, -2},
            {1, 0, -1},
        };
        
        public static double[,] SobelY = new double[,]
        {
            {-1, -2, -1},
            {0, 0, 0},
            {1, 2, 1},
        };
        
        public static double[,] Smooth = new double[,]
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
        };

        public static double[,] EdgeDetection = new double[,]
        {
            {1, 0, -1},
            {0, 0, 0},
            {-1, 0, 1},
        };

        public static double[,] Smooth2x2 = new double[,]
        {
            {1, 1},
            {1, 1}
        };
        
        public static double[,] Smooth2x2Opposite = new double[,]
        {
            {-1, -1},
            {-1, -1}
        };
        
        
        // only same conv (just backup)
        public static double[,] GetConvolution(in double[,] matrix,in double[,] Kkernel)
        {
            //var kernel = MatrixOperation.MatrixDuplicate(Kkernel);
            
             var kernel = MatrixOperation.FlipLeftRight(Kkernel);
             kernel = MatrixOperation.FlipUpDown(kernel);

            var kernelRows = kernel.GetLength(0);
            var kernelColms = kernel.GetLength(1);

            var mRows = matrix.GetLength(0);
            var mColms = matrix.GetLength(1);

            var dx = (kernel.GetLength(0) - 1) / 2;
            var dy = (kernel.GetLength(1) - 1) / 2;
            
            var result = new double[mRows, mColms];
            
            for (int i = 0; i < mRows; i++)
            {
                for (int j = 0; j < mColms; j++)
                {
                    for (int k = 0; k < kernelRows; k++)
                    {
                        for (int l = 0; l < kernelColms; l++)
                        {
                            int x = i  + k - dx; // - dx
                            int y = j  + l - dy; // - dy

                            if (x >= 0 && x < mRows &&
                                y >= 0 && y < mColms)
                                result[i, j] += matrix[x, y] * kernel[k, l];
                        }
                    }
                }
            }
            
            return result;
        }
        
        // isValid == true, without zero padding calculations https://stackoverflow.com/questions/37674306/what-is-the-difference-between-same-and-valid-padding-in-tf-nn-max-pool-of-t
        public static double[,] GetConvolution(in double[,] matrix,in double[,] Kkernel, bool isValid, int c = 1)
        {
            //var kernel = MatrixOperation.MatrixDuplicate(Kkernel);
            var kernel = MatrixOperation.FlipLeftRight(Kkernel);
            kernel = MatrixOperation.FlipUpDown(kernel);

            var kernelRows = kernel.GetLength(0);
            var kernelColms = kernel.GetLength(1);

            var mRows = matrix.GetLength(0);
            var mColms = matrix.GetLength(1);

            var dx = (kernel.GetLength(0) - 1) / 2;
            var dy = (kernel.GetLength(1) - 1) / 2;

           var paddingX = 0;
           var paddingY = 0;
           var correctionX = 0;
           var correctionY = 0;
            if (isValid)
            { 
                correctionX = (kernel.GetLength(0)) % 2 == 0? 1 : 0;
                correctionY = (kernel.GetLength(1)) % 2 == 0? 1 : 0;
                
                paddingX = dx;
                paddingY = dy;
            }

            var result = new double[mRows - (paddingX * 2) - correctionX, mColms - (paddingX * 2) - correctionY];
            
            for (int i = paddingX; i < mRows - paddingX  - correctionX; i++)
            {
                for (int j = paddingY; j < mColms - paddingY  - correctionY; j++)
                {

                    var tmp = 0.0;
                    for (int k = 0; k < kernelRows; k++)
                    {
                        for (int l = 0; l < kernelColms; l++)
                        {
                            int x = i  + k - dx; 
                            int y = j  + l - dy;

                            if (x >= 0 && x < mRows &&
                                y >= 0 && y < mColms)
                            {
                                tmp += matrix[x, y] * kernel[k, l] / c;
                            }
                        }
                    }

                    result[i - paddingX, j - paddingY] = tmp;
                }
            }
            
            return result;
        }
        
        public static Image<Rgb24> GetConvolution(Image<Rgb24> image, in double[,] Kkernel, bool isValid, int c = 1)
        {
            //var kernel = MatrixOperation.MatrixDuplicate(Kkernel);
            
            var kernel = MatrixOperation.FlipLeftRight(Kkernel);
            kernel = MatrixOperation.FlipUpDown(kernel);

            var kernelRows = kernel.GetLength(0);
            var kernelColms = kernel.GetLength(1);

            var mRows = image.Width;
            var mColms = image.Height;

            var dx = (kernel.GetLength(0) - 1) / 2;
            var dy = (kernel.GetLength(1) - 1) / 2;

            var paddingX = 0;
            var paddingY = 0;
            var correctionX = 0;
            var correctionY = 0;
            if (isValid)
            { 
                correctionX = (kernel.GetLength(0)) % 2 == 0? 1 : 0;
                correctionY = (kernel.GetLength(1)) % 2 == 0? 1 : 0;
                
                paddingX = dx;
                paddingY = dy;
            }

            var result = new Image<Rgb24>(mRows, mColms);
            
            for (int i = paddingX; i < mRows - paddingX  - correctionX; i++)
            {
                for (int j = paddingY; j < mColms - paddingY  - correctionY; j++)
                {
                    var R = 0.0;
                    var G = 0.0;
                    var B = 0.0;
                    
                    for (int k = 0; k < kernelRows; k++)
                    {
                        for (int l = 0; l < kernelColms; l++)
                        {
                            int x = i  + k - dx; 
                            int y = j  + l - dy;

                            if (x >= 0 && x < mRows &&
                                y >= 0 && y < mColms)
                            {
                                R += image[x, y].R * kernel[k, l] / c;
                                G += image[x, y].G * kernel[k, l] / c;
                                B += image[x, y].B * kernel[k, l] / c;
                            }

                        }
                    }

                    result[i - paddingX, j - paddingY] = new Rgb24((byte)Math.Abs(R), (byte)Math.Abs(G), (byte)Math.Abs(B));
                }
            }
            
            return result;
        }
        
        // I know about unit testing, but time...
        public static void TestConv()
        {
            //var matrix = new double[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}}; 
            //var kernel = new double[,] {{-1, -2, -1}, {0, 0, 0},  {1, 2, 1}}; 
            /*
                -13   -20   -17
                -18   -24   -18
                13    20    17

             */
            
            
            var matrix = new double[,]
            {
                {3, 0, 1, 2, 7, 4},
                {1, 5, 8, 9, 3, 1},
                {2, 7, 2, 5, 1, 3},
                {0, 1, 3, 1, 7, 8},
                {4, 2, 1, 6, 2, 8},
                {2, 4,5,2,3,9}
            }; 
            var kernel = new double[,] {{1, 0, -1}, {1, 0, -1},  {1, 0, -1}}; 
            
            /*
                    5     4     0    -8
                    10     2    -2    -3
                    0     2     4     7
                    3     2     3    16
                    
                    
                         5     5     6     1    -6   -10
                        12     5     4     0    -8   -11
                        13    10     2    -2    -3   -11
                        10     0     2     4     7   -10
                        7     3     2     3    16   -12
                         6     0     2    -1     9    -5

             */
            
            var r  = Convolution.GetConvolution(matrix, kernel, false);
        }
    }
}