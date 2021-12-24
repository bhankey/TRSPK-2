using System;

namespace LucasKanade
{
    public static class MatrixOperation
    {
        public static T[,] MatrixCreate<T>(int rows, int cols)
        {
            return new T[rows, cols];
        }

        public static void SetEmpty<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    matrix[i, j] = default;
                }
            }
        }

      

        public static T[,] MatrixDuplicate<T>(T[,] matrix)
        {
            var result = MatrixCreate<T>(matrix.GetLength(0), matrix.GetLength(1));
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    result[i, j] = matrix[i, j];
                }
            }

            return result;
        }

        public static int GetRowsCount<T>(T[,] m)
        {
            return m.GetLength(0);
        }

        public static int GetColumnsCount<T>(T[,] m)
        {
            return m.GetLength(1);
        }
        
        public static void Transpose<T>(T[,] matrix, T[,] buffer)
        {
            var rowsCount = GetRowsCount(matrix);
            var columnsCount = GetColumnsCount(matrix);

            for (int rows = 0; rows < rowsCount; rows++)
            {
                for (int columns = 0; columns < columnsCount; columns++)
                {
                    buffer[columns, rows] = matrix[rows, columns];
                }
            }
        }

        // Transpose - транспонирует матрицу
        public static T[,] Transpose<T>(T[,] matrix)
        {
            var rowsCount = GetRowsCount(matrix);
            var columnsCount = GetColumnsCount(matrix);

            var trans = new T[columnsCount, rowsCount];

            for (int rows = 0; rows < rowsCount; rows++)
            {
                for (int columns = 0; columns < columnsCount; columns++)
                {
                    trans[columns, rows] = matrix[rows, columns];
                }
            }

            return trans;
        }

        public static void Print2DMatrix<T>(T[,] matrix)
        {
            for (int rows = 0; rows < GetRowsCount(matrix); rows++)
            {
                for (int columns = 0; columns < GetColumnsCount(matrix); columns++)
                {
                    Console.Write($"{matrix[rows, columns].ToString().Replace(',','.' )} ");
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        public static void ConcatenateByXAxis<T>(T[,] first, T[,] second, T[,] buffer)
        {
            if (GetRowsCount(first) != GetRowsCount(second))
            {
                throw new ArgumentException("matrix must have same count of axis");
            }

            var rowsCount = GetRowsCount(first);

            for (int rows = 0; rows < rowsCount; rows++)
            {
                var columns = 0;
                for (; columns < GetColumnsCount(first); columns++)
                {
                    buffer[rows, columns] = first[rows, columns];
                }

                for (int secondMColumns = 0; secondMColumns < GetColumnsCount(second); secondMColumns++, columns++)
                {
                    buffer[rows, columns] = second[rows, secondMColumns];
                }
            }
        }

        // ConcatenateByXAxis конкатенирование массиво по оси x
        public static T[,] ConcatenateByXAxis<T>(T[,] first, T[,] second)
        {
            if (GetRowsCount(first) != GetRowsCount(second))
            {
                throw new ArgumentException("matrix must have same count of axis");
            }

            var rowsCount = GetRowsCount(first);
            var columnsCount = GetColumnsCount(first) + GetColumnsCount(second);

            var result = new T[rowsCount, columnsCount];

            for (int rows = 0; rows < rowsCount; rows++)
            {
                var columns = 0;
                for (; columns < GetColumnsCount(first); columns++)
                {
                    result[rows, columns] = first[rows, columns];
                }

                for (int secondMColumns = 0; secondMColumns < GetColumnsCount(second); secondMColumns++, columns++)
                {
                    result[rows, columns] = second[rows, secondMColumns];
                }
            }

            return result;
        }


        public static double[,] MatrixMultiplier(double[,] first, double[,] second)
        {
            if (first.GetLength(1) != second.GetLength(0))
            {
                throw new ArgumentException("Images must be the same size ");
            }

            var result = new double[first.GetLength(0), second.GetLength(1)];

            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    for (int k = 0; k < second.GetLength(0); k++)
                    {
                        result[i, j] += first[i, k] * second[k, j];
                    }
                }
            }

            return result;
        }

        public static void MatrixMultiplier(double[,] first, double[,] second, double[,] buffer)
        {
            if (first.GetLength(1) != second.GetLength(0))
            {
                throw new ArgumentException("Images must be the same size ");
            }

            SetEmpty(buffer);

            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < second.GetLength(1); j++)
                {
                    for (int k = 0; k < second.GetLength(0); k++)
                    {
                        buffer[i, j] += first[i, k] * second[k, j];
                    }
                }
            }
        }

        public static double[,] MatrixInverse(double[,] matrix)
        {
            var result = new double[,]
            {
                {matrix[1, 1], -1 * matrix[0, 1]},
                {matrix[1, 0] * -1, matrix[0, 0]}
            };

            var det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            MatrixMult(result, 1 / det);

            return result;
        }

        public static void FlattenInRows<T>(T[,] matrix, T[,] buffer)
        {
            var k = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    buffer[k++, 0] = matrix[i, j];
                }
            }
        }

        public static T[,] FlattenInRows<T>(T[,] matrix)
        {
            var flattenMatrix = new T[matrix.GetLength(0) * matrix.GetLength(1), 1];

            var k = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    flattenMatrix[k++, 0] = matrix[i, j];
                }
            }

            return flattenMatrix;
        }


        public static void Swap2DRows<T>(T[,] a, int indexOne, int indexTwo)
        {
            for (int i = 0; i < GetRowsCount(a); i++)
            {
                (a[indexOne, i], a[indexTwo, i]) = (a[indexTwo, i], a[indexOne, i]);
            }
        }

        public static void Swap2DColons<T>(T[,] a, int indexOne, int indexTwo)
        {
            for (int i = 0; i < GetColumnsCount(a); i++)
            {
                (a[i, indexOne], a[i, indexTwo]) = (a[i, indexTwo], a[i, indexOne]);
            }
        }



        public static T[,] FlipLeftRight<T>(in T[,] matrix)
        {
            var result = MatrixDuplicate(matrix);

            for (int i = 0; i < GetColumnsCount(matrix) / 2; i++)
            {
                Swap2DColons(result, i, GetColumnsCount(matrix) - i - 1);
            }

            return result;
        }

        public static T[,] FlipUpDown<T>(T[,] matrix)
        {
            var result = MatrixDuplicate(matrix);

            for (int i = 0; i < GetRowsCount(matrix) / 2; i++)
            {
                Swap2DRows(result, i, GetRowsCount(matrix) - i - 1);
            }

            return result;
        }

        public static void GetSubMatrix<T>(T[,] matrix, int x, int y, int x1, int y1, T[,] buffer)
        {
            for (int i = x, l = 0; i < x1; i++, l++)
            {
                for (int j = y, k = 0; j < y1; j++, k++)
                {
                    buffer[l, k] = matrix[i, j];
                }
            }
        }
       
        public static void MatrixMinus(double[,] first, double[,] second)
        {
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < first.GetLength(1); j++)
                {
                    first[i, j] = first[i, j] - second[i, j];
                }
            }
        }

        public static void MatrixPlus(double[,] first, double[,] second)
        {
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < first.GetLength(1); j++)
                {
                    first[i, j] += second[i, j];
                }
            }
        }

        public static void MatrixDiv(double[,] first, double div)
        {
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < first.GetLength(1); j++)
                {
                    first[i, j] /= div;
                }
            }
        }

        public static void MatrixMult(double[,] first, double m)
        {
            for (int i = 0; i < first.GetLength(0); i++)
            {
                for (int j = 0; j < first.GetLength(1); j++)
                {
                    first[i, j] *= m;
                }
            }
        }
    }
}