using System;
using System.Collections.Generic;
using System.Linq;


namespace LucasKanade
{
    public class LucasKanade
    {
        public int BoxSize { get; }

        private const int DefaultBoxSize = 9;
        
        public int ImageWidth { get; }

        public int ImageHeight { get; }

        private double[,] changesByX;
        private double[,] changesByY;
        private double[,] changesT;
        
        private double[,] flattenChangesByX;
        private double[,] flattenChangesByY;
        private double[,] flattenChangesT;

        private double[,] matrixS;
        private double[,] transposeMatrixS;
        private double[,] tempMatrix;

        private double[,] stS;
        private double[,] matrixVector;
        public LucasKanade(int boxSize,int imageHeight, int imageWidth)
        {
            BoxSize = boxSize;
            
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
            stS = new double[transposeMatrixS.GetLength(0), matrixS.GetLength(1)];
            matrixVector = new double[2, 1];
        }
        
        public LucasKanade(int imageHeight, int imageWidth)
        {
            BoxSize = DefaultBoxSize;
            
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
            stS = new double[transposeMatrixS.GetLength(0), matrixS.GetLength(1)];
            matrixVector = new double[2, 1];
        }

        private List<List<double[]>> AllocateOpticalFlowResult()
        {
            var opticalFlow = new List<List<double[]>>(ImageWidth / BoxSize + 1);
            for (int x = 0; x < ImageWidth / BoxSize + 1; x += 1)
            {
                opticalFlow.Add(new List<double[]>(ImageHeight / BoxSize + 1));
                for (int y = 0; y < ImageHeight / BoxSize + 1; y += 1)
                {
                    var tmp = new[] {0.0, 0.0};
                    opticalFlow.Last().Add(tmp);
                }
            }

            return opticalFlow;
        }



        public List<List<double[]>> GetOpticalFlow(double[,] firstImage, double[,] secondImage)
        {
            if (MatrixOperation.GetRowsCount(firstImage) != ImageWidth ||
                MatrixOperation.GetRowsCount(secondImage) != ImageWidth ||
                MatrixOperation.GetColumnsCount(firstImage) != ImageHeight ||
                MatrixOperation.GetColumnsCount(secondImage) != ImageHeight)
            {
                throw new ArgumentException("image must be same size");
            }

            var opticalFlow = AllocateOpticalFlowResult();

            for (int x = 0, opticalFlowX = 0;x + BoxSize < MatrixOperation.GetRowsCount(firstImage); x += BoxSize, opticalFlowX++)
            {
                for (int y = 0, opticalFlowY = 0;y + BoxSize < MatrixOperation.GetColumnsCount(firstImage); y += BoxSize, opticalFlowY++)
                {
                    SetIntensityChangesByX(firstImage, x, y);
                    SetIntensityChangesByY(firstImage, x, y);
                    SetIntensityChangesByTime(firstImage, secondImage, x, y);
                    
                    MatrixOperation.FlattenInRows(changesByX, flattenChangesByX);
                    
                    MatrixOperation.FlattenInRows(changesByY, flattenChangesByY);
                    
                    MatrixOperation.FlattenInRows(changesT, flattenChangesT);
                    
                    MatrixOperation.ConcatenateByXAxis(flattenChangesByX, flattenChangesByY, matrixS);

                    MatrixOperation.Transpose(matrixS, transposeMatrixS);

                    MatrixOperation.MatrixMultiplier(transposeMatrixS, matrixS, stS);

                    if (stS[0,0]*stS[1,1]-stS[0,1]*stS[1,0] == 0)
                    {
                        continue;
                    }

                    var stSInv = MatrixOperation.MatrixInverse(stS);
                    MatrixOperation.MatrixMultiplier(stSInv, transposeMatrixS, tempMatrix);
                    
                    MatrixOperation.MatrixMultiplier(tempMatrix, flattenChangesT, matrixVector);
                    
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
                    var currentValue = image[x + i, y + j];
                    
                    var nextValue = j + 1 > BoxSize - 1 ? currentValue : image[x + i, y + j + 1];
                    var prevValue = j - 1 < 0 ? currentValue : image[x + i, y + j - 1];

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
                    var currentValue = image[x + i, y + j];

                    var nextValue = i + 1 > BoxSize - 1 ? currentValue : image[x + i + 1, y + j];
                    var prevValue = i - 1 < 0 ? currentValue : image[x + i - 1, y + j];
                    
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