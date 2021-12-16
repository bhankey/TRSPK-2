using System.ComponentModel.DataAnnotations;

namespace LucasKanade
{
    public class Convolution
    {
        // isValid == true, without zero padding calculations https://stackoverflow.com/questions/37674306/what-is-the-difference-between-same-and-valid-padding-in-tf-nn-max-pool-of-t
        public static double[,] GetConvolution(in double[,] matrix, double[,] kernel, bool isValid)
        {
            kernel = MatrixOperation.FlipLeftRight(kernel);
            kernel = MatrixOperation.FlipUpDown(kernel);

            var dx = kernel.GetLength(0) / 2;
            var dy = kernel.GetLength(1) / 2;

            var paddingX = 0;
            var paddingY = 0;

            if (isValid)
            {
                paddingX = dx;
                paddingY = dy;
            }

            var correctionX = (kernel.GetLength(0) / 2) % 2 != 0 && isValid? 1 : 0;
            var correctionY = (kernel.GetLength(1) / 2) % 2 != 0 && isValid? 1 : 0;
            
            var result = new double[matrix.GetLength(0) - paddingX - correctionX, matrix.GetLength(1) - paddingY - correctionY];
            
            for (int i = paddingX; i < matrix.GetLength(0) - paddingX; i++)
            {
                for (int j = paddingY; j < matrix.GetLength(1) - paddingY; j++)
                {

                    var tmp = 0.0;
                    for (int k = 0; k < kernel.GetLength(1); k++)
                    {
                        for (int l = 0; l < kernel.GetLength(0); l++)
                        {
                            int x = j - dx + l;
                            int y = i - dy + k;

                            if (y >= 0 && y < matrix.GetLength(0) &&
                                x >= 0 && x < matrix.GetLength(1))
                                tmp += matrix[y, x] * kernel[k, l];
                        }
                    }

                    result[i - paddingX, j - paddingY] = tmp;

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
            
            
            var matrix = new double[,] {{3, 0, 1, 2, 7, 4}, {1, 5, 8, 9, 3, 1}, {2, 7, 2, 5, 1, 3}, {0, 1, 3, 1, 7, 8}, {4, 2, 1, 6, 2, 8} , {2, 4,5,2,3,9}}; 
            var kernel = new double[,] {{1, 0, -1}, {1, 0, -1},  {1, 0, -1}}; 
            
            /*
                    5     4     0    -8
                    10     2    -2    -3
                    0     2     4     7
                    3     2     3    16
             */
            
            var r  = Convolution.GetConvolution(matrix, kernel, true);
        }
    }
}