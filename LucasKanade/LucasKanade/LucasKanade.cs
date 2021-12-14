using System;
using System.Collections.Generic;
using System.Linq;


namespace LucasKanade
{
    public class LucasKanade
    {
        private const int BoxSize = 9;

        private double[,] changesByX;
        private double[,] changesByY;
        private double[,] changesT;
        
        private double[,] flattenChangesByX;
        private double[,] flattenChangesByY;
        private double[,] flattenChangesT;

        private double[,] matrixS;
        private double[,] transposeMatrixS;
        private double[,] tempMatrix;

        public LucasKanade(int imageHeight, int imageWidth)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            changesByX = new double[BoxSize, BoxSize];
            changesByY = new double[BoxSize, BoxSize];
            changesT = new double[BoxSize, BoxSize];

            flattenChangesByX = new double[BoxSize * BoxSize, 1];
            flattenChangesByY = new double[BoxSize * BoxSize, 1];
            flattenChangesT = new double[BoxSize * BoxSize, 1];

            matrixS = new double[MatrixOperation.GetRowsCount(flattenChangesByX),
                MatrixOperation.GetColumnsCount(flattenChangesByX) +
                MatrixOperation.GetColumnsCount(flattenChangesByY)];

            transposeMatrixS = new double[MatrixOperation.GetColumnsCount(matrixS), MatrixOperation.GetRowsCount(matrixS)];

            tempMatrix = new double[2, transposeMatrixS.GetLength(1)];
        }

        public int ImageWidth { get; }

        public int ImageHeight { get; }

        public static int GetBoxSize()
        {
            return BoxSize;
        }

        public List<List<double[]>> GetOpticalFlow(double[,] firstImage, double[,] secondImage)
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
                    SetIntensityChangesByX(firstImage, x, y);
                    SetIntensityChangesByY(firstImage, x, y);
                    SetIntensityChangesByTime(firstImage, secondImage, x, y);
                    
                    MatrixOperation.FillFlattenInRows(changesByX, flattenChangesByX);
                    
                    MatrixOperation.FillFlattenInRows(changesByY, flattenChangesByY);
                    
                    MatrixOperation.FillFlattenInRows(changesT, flattenChangesT);
                    
                    MatrixOperation.FillConcatenateByXAxis(flattenChangesByX, flattenChangesByY, matrixS);

                    MatrixOperation.FillTranspose(matrixS, transposeMatrixS);

                    var stS = MatrixOperation.MatrixMultiplier(transposeMatrixS, matrixS);

                    if (stS[0,0]*stS[1,1]-stS[0,1]*stS[1,0] == 0)
                    {
                        continue;
                    }

                    var stSInv = MatrixOperation.MatrixInverse(stS);
                    MatrixOperation.FillMatrixMultiplier(stSInv, transposeMatrixS, tempMatrix);

                    var matrixVector = MatrixOperation.MatrixMultiplier(tempMatrix, flattenChangesT);
                    
                    opticalFlow[opticalFlowX][ opticalFlowY] = (MatrixOperation.GetRow(matrixVector, 0));
                }
            }

            return opticalFlow;
        }


        private void SetIntensityChangesByX(double[,] image, int x, int y)
        {
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

                    changesByX[i, j] = (nextValue - prevValue) / 2.0;
                }
            }
        }
        
        private void SetIntensityChangesByY(double[,] image, int x, int y)
        {
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

                    changesByY[i, j] = (nextValue - prevValue) / 2.0;
                }
            }
        }
        
        private void SetIntensityChangesByTime(double[,] firstImage, double[,] secondImage, int x, int y)
        {
            for (int i = 0; i < BoxSize; i++)
            {
                for (int j = 0; j < BoxSize; j++)
                {
                    changesT[i, j] = -(secondImage[x + i, y + j] - firstImage[x + i, y + j]);
                }
            }
        }
    }
}