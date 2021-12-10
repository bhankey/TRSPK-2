using System;
using System.Collections.Generic;
using System.Linq;


namespace LucasKanade
{
    public static class LucasKanade
    {
        private const int BoxSize = 9;
        
        public static int GetBoxSize()
        {
            return BoxSize;
        }

        public static List<List<double[]>> GetOpticalFlow(double[,] firstImage, double[,] secondImage)
        {
            if (MatrixOperation.GetRowsCount(firstImage) != MatrixOperation.GetRowsCount(secondImage)
                || MatrixOperation.GetColumnsCount(firstImage) != MatrixOperation.GetColumnsCount(secondImage))
            {
                throw new ArgumentException("image must be same size");
            }


            var opticalFlow = new List<List<double[]>>(MatrixOperation.GetRowsCount(firstImage) / BoxSize + 1);
            for (int x = 0; x < MatrixOperation.GetRowsCount(firstImage) / BoxSize; x += 1)
            {
                opticalFlow.Add(new List<double[]>(MatrixOperation.GetColumnsCount(firstImage) / BoxSize));
                for (int y = 0; y < MatrixOperation.GetColumnsCount(firstImage) / BoxSize; y += 1)
                {
                    var tmp = new[] {0.0, 0.0};
                    opticalFlow.Last().Add(tmp);
                }
            }

            for (int x = 0, opticalFlowX = 0;x + BoxSize < MatrixOperation.GetRowsCount(firstImage); x += BoxSize, opticalFlowX++)
            {
                for (int y = 0, opticalFlowY = 0;y + BoxSize < MatrixOperation.GetColumnsCount(firstImage); y += BoxSize, opticalFlowY++)
                {
                    var changesByX = GetIntensityChangesByX(firstImage, x, y);
                    var changesByY = GetIntensityChangesByY(firstImage, x, y);
                    var changesT = GetIntensityChangesByTime(firstImage, secondImage, x, y);
                    
                    var flattenChangesByX = MatrixOperation.FlattenInRows(changesByX);
                    
                    var flattenChangesByY = MatrixOperation.FlattenInRows(changesByY);
                    
                    var flattenChangesT = MatrixOperation.FlattenInRows(changesT);

                    var matrixS = MatrixOperation.ConcatenateByXAxis(flattenChangesByX, flattenChangesByY);

                    var transposeMatrixS = MatrixOperation.Transpose(matrixS);

                    var stS = MatrixOperation.MatrixMultiplier(transposeMatrixS, matrixS);

                    if (stS[0,0]*stS[1,1]-stS[0,1]*stS[1,0] == 0)
                    {
                        continue;
                    }

                    var stSInv = MatrixOperation.MatrixInverse(stS);
                    var tempMatrix = MatrixOperation.MatrixMultiplier(stSInv, transposeMatrixS);

                    var matrixVector = MatrixOperation.MatrixMultiplier(tempMatrix, flattenChangesT);
                    
                    opticalFlow[opticalFlowX][ opticalFlowY] = (MatrixOperation.GetRow(matrixVector, 0));
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