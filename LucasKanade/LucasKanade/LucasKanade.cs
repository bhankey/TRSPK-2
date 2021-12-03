using System;

namespace LucasKanade
{
    public class LucasKanade
    {
        private const int boxSize = 30;

        public double[] GetOpticalFlow(double[,] firstImage, double[,] secondImage)
        {
            if (MatrixOperation.GetRowsCount(firstImage) != MatrixOperation.GetRowsCount(secondImage)
                || MatrixOperation.GetColumnsCount(firstImage) != MatrixOperation.GetColumnsCount(secondImage))
            {
                throw new ArgumentException("image must be same size");
            }

            var x = 0;
            var y = 0;

            var velocityVector = new double[] {0, 0};
            while (x + boxSize < MatrixOperation.GetRowsCount(firstImage))
            {
                while (y + boxSize < MatrixOperation.GetColumnsCount(firstImage))
                {
                    
                }
            }
        }


        private double[][] GetIntensityChangesByX(double[,] image, int x, int y)
        {
            var result = MatrixOperation.MatrixCreate<double>(boxSize, boxSize);

            for (int i = 0; i < boxSize; i++)
            {
                for (int j = 0; j < boxSize; j++)
                {
                    var nextValue = 0.0;
                    var prevValue = 0.0;

                    if (j + 1 > boxSize - 1)
                    {
                        nextValue = image[x + i, y + j];
                    }
                    else
                    {
                        nextValue = image[x + i, y + j + 1];
                    }

                    if (j - 1 < 0)
                    {
                        prevValue = image[x + i, y + j];
                    }
                    else
                    {
                        prevValue = image[x + i, y + j - 1];
                    }

                    result[i][j] = (nextValue - prevValue) / 2.0;
                }
            }

            return result;
        }
        
        private double[][] GetIntensityChangesByY(double[,] image, int x, int y)
        {
            var result = MatrixOperation.MatrixCreate<double>(boxSize, boxSize);

            for (int i = 0; i < boxSize; i++)
            {
                for (int j = 0; j < boxSize; j++)
                {
                    var nextValue = 0.0;
                    var prevValue = 0.0;
                    var currentValue = image[x + i, y + j];

                    if (i + 1 > boxSize - 1)
                    {
                        nextValue = currentValue;
                    }
                    else
                    {
                        nextValue = image[x + i + 1, y + j];
                    }

                    if (i - 1 < 0)
                    {
                        prevValue = currentValue;
                    }
                    else
                    {
                        prevValue = image[x + i - 1, y + j];
                    }

                    result[i][j] = (nextValue - prevValue) / 2.0;
                }
            }

            return result;
        }
        
        private double[][] GetIntensityChangesByTime(double[,] firstImage, double[,] secondImage, int x, int y)
        {
            var result = MatrixOperation.MatrixCreate<double>(boxSize, boxSize);

            for (int i = 0; i < boxSize; i++)
            {
                for (int j = 0; j < boxSize; j++)
                {
                    result[i][j] = -(secondImage[x + i, y + j] - firstImage[x + i, y + j]);
                }
            }

            return result;
        }
    }
}