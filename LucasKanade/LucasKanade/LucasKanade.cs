using System;
using System.Collections.Generic;
using System.Linq;


namespace LucasKanade
{
    public class LucasKanade
    {
        public int BoxSize { get; }

        private const int DefaultBoxSize = 15;
        
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
        private double[,] changesByXImage;
        private double[,] changesByYImage;
        private double[,] changesByTImage;
        
        public LucasKanade(int imageHeight, int imageWidth, int boxSize = DefaultBoxSize)
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

            // changesByXImage = new double[ImageWidth, ImageHeight];
            // changesByTImage = new double[ImageWidth, ImageHeight];
            // changesByXImage = new double[ImageWidth, ImageHeight];
        }
        
        public List<FlowPoints> GetOpticalFlow(double[,] firstImage, double[,] secondImage, double threshold, int interval = 15)
        {
            if (MatrixOperation.GetRowsCount(firstImage) != ImageWidth ||
                MatrixOperation.GetRowsCount(secondImage) != ImageWidth ||
                MatrixOperation.GetColumnsCount(firstImage) != ImageHeight ||
                MatrixOperation.GetColumnsCount(secondImage) != ImageHeight)
            {
                throw new ArgumentException("image must be same size");
            }

            var opticalFlow = new List<FlowPoints>();
            
            double[,] changesByXImage = Convolution.GetConvolution(firstImage, Convolution.StandartCoreXLol, (bool)Registry.Get("valid_conv"), 4);

            double[,] changesByYImage  = Convolution.GetConvolution(firstImage, Convolution.StandartCoreYLol, (bool)Registry.Get("valid_conv"), 4);

            double[,] changesByTImage = Convolution.GetConvolution(secondImage, Convolution.Smooth2x2, (bool)Registry.Get("valid_conv"), 4);

            MatrixOperation.MatrixPlus(
                changesByTImage, 
                Convolution.GetConvolution(firstImage, Convolution.Smooth2x2Opposite, (bool)Registry.Get("valid_conv"), 4));

            MatrixOperation.MatrixDiv(firstImage, 255); // нормализуем изображение
            MatrixOperation.MatrixDiv(secondImage, 255); // нормализуем изображение
            
            for (int x = 0;x + BoxSize < MatrixOperation.GetRowsCount(firstImage); x += interval)
            {
                for (int y = 0;y + BoxSize < MatrixOperation.GetColumnsCount(firstImage); y += interval)
                {
                    if ((bool) Registry.Get("convolution"))
                    {
                      MatrixOperation.GetSubMatrix(changesByXImage, x, y, x + BoxSize, y + BoxSize, changesByX);
                        MatrixOperation.GetSubMatrix(changesByYImage, x, y, x + BoxSize, y + BoxSize, changesByY);

                        MatrixOperation.GetSubMatrix(changesByTImage, x, y, x + BoxSize, y + BoxSize, changesT);

                    }
                    else
                    {
                        SetIntensityChangesByX(firstImage, x, y);
                        SetIntensityChangesByY(firstImage, x, y);
                        SetIntensityChangesByTime(firstImage, secondImage, x, y);
                    }

                    MatrixOperation.MatrixMult(changesT, -1);
                    
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
                    
                    // Вычисление собственных значений для фильтрции, но у меня не прокатило
                    //var basis = (MatrixOperation.GetBasis(stS));

                    // if (Math.Max(Math.Abs(basis[0]), Math.Abs(basis[1])) <= 0.01)
                    // {
                    //     continue;
                    // }

                    var stSInv = MatrixOperation.MatrixInverse(stS);
                    
                    if (double.IsInfinity(stSInv[0, 0]) ||double.IsInfinity(stSInv[0, 1]) || double.IsInfinity(stSInv[1, 0]) ||double.IsInfinity(stSInv[1, 1]))
                    {
                        continue;
                    }
                    
                    MatrixOperation.MatrixMultiplier(stSInv, transposeMatrixS, tempMatrix);
                    
                    MatrixOperation.MatrixMultiplier(tempMatrix, flattenChangesT, matrixVector);

                    if (Math.Abs(matrixVector[0, 0]) < threshold || Math.Abs(matrixVector[1, 0]) < threshold)
                    {
                        continue;
                    }

                    opticalFlow.Add(new FlowPoints(x + BoxSize / 2, y + BoxSize / 2,  matrixVector[0, 0], matrixVector[1, 0]));
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