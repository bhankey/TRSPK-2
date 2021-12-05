using System;
using System.Collections.Generic;

namespace LucasKanade
{
    public static class LucasKanade
    {
        private const int BoxSize = 30;

        public static int GetBoxSize()
        {
            return BoxSize;
        }

        public static List<double[]> GetOpticalFlow(double[,] firstImage, double[,] secondImage)
        {
            var opticalFlow = new List<double[]>();
            
            if (MatrixOperation.GetRowsCount(firstImage) != MatrixOperation.GetRowsCount(secondImage)
                || MatrixOperation.GetColumnsCount(firstImage) != MatrixOperation.GetColumnsCount(secondImage))
            {
                throw new ArgumentException("image must be same size");
            }

            var x = 0;
            var y = 0;
            
            for (;x + BoxSize < MatrixOperation.GetRowsCount(firstImage); x += BoxSize)
            {
                y = 0;
                for (;y + BoxSize < MatrixOperation.GetColumnsCount(firstImage); y += BoxSize)
                {
                    var changesByX = GetIntensityChangesByX(firstImage, y, x);
                    var changesByY = GetIntensityChangesByY(firstImage, y, x);
                    var changesT = GetIntensityChangesByTime(firstImage, secondImage, y, x); // TODO think to change y, x order
                    
                    var flattenChangesByX = MatrixOperation.FlattenInRows(changesByX);
                    
                    MatrixOperation.Print2DMatrix(flattenChangesByX);
                    var flattenChangesByY = MatrixOperation.FlattenInRows(changesByY);
                    
                    var flattenChangesT = MatrixOperation.FlattenInRows(changesT);

                    var matrixS = MatrixOperation.ConcatenateByXAxis(flattenChangesByX, flattenChangesByY);

                    var transposeMatrixS = MatrixOperation.Transpose(matrixS);

                    var stS = MatrixOperation.MatrixMultiplier(transposeMatrixS, matrixS);

                    if (stS[0,0]*stS[1,1]-stS[0,1]*stS[1,0] == 0)
                    {
                        var tmp = new[] {0.0, 0.0};
                        opticalFlow.Add(tmp);
                        continue;
                    }

                    var stSInv = MatrixOperation.MatrixInverse(stS);
                    var tempMatrix = MatrixOperation.MatrixMultiplier(stSInv, transposeMatrixS);

                    var matrixVector = MatrixOperation.MatrixMultiplier(tempMatrix, flattenChangesT);
                    
                    opticalFlow.Add(MatrixOperation.GetRow(matrixVector, 0));
                }
            }

            return opticalFlow;
        }


        private static double[,] GetIntensityChangesByX(double[,] image, int x, int y)
        {
            var result = new double[BoxSize, BoxSize];

            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    var nextValue = 0.0;
                    var prevValue = 0.0;

                    if (j + 1 > BoxSize - 1)
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

                    result[i, j] = (nextValue - prevValue) / 2.0;
                }
            }

            return result;
        }
        
        private static double[,] GetIntensityChangesByY(double[,] image, int x, int y)
        {
            var result = new double[BoxSize, BoxSize];

            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    var nextValue = 0.0;
                    var prevValue = 0.0;
                    var currentValue = image[x + i, y + j];

                    if (i + 1 > BoxSize - 1)
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

                    result[i, j] = (nextValue - prevValue) / 2.0;
                }
            }

            return result;
        }
        
        private static double[,] GetIntensityChangesByTime(double[,] firstImage, double[,] secondImage, int x, int y)
        {
            var result = new double[BoxSize, BoxSize];

            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    result[i, j] = -(secondImage[x + i, y + j] - firstImage[x + i, y + j]);
                }
            }

            return result;
        }
    }
}