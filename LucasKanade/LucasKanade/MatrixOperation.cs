using System;

namespace LucasKanade
{
    // TODO написать вычисление обратной матрицы
    public static class MatrixOperation
    {
        public static T[][] MatrixCreate<T>(int rows, int cols)
        {
            T[][] result = new T[rows][];
            for (int i = 0; i < rows; ++i)
            {
                result[i] = new T[cols];
            }

            return result;
        }
        
        public static T[][] MatrixDuplicate<T>(T[][] matrix)
        {
            var result = MatrixCreate<T>(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix[i].GetLength(0); ++j)
                {
                    result[i][j] = matrix[i][j];
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
        
        // Transpose - транспонирует матрицу
        public static T[,] Transpose<T>(T[,] matrix)
        {
            var rowsCount = matrix.GetLength(0);
            var columnsCount = matrix.GetLength(1);

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
            for (int rows = 0; rows < matrix.GetLength(0); rows++)
            {
                for (int columns = 0; columns < matrix.GetLength(1); columns++)
                {
                    Console.Write($"{matrix[rows, columns]} ");
                }

                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        // ConcatenateByXAxis конкатенирование массиво по оси x
        public static T[,] ConcatenateByXAxis<T>(T[,] first, T[,] second)
        {
            if (first.GetLength(0) != second.GetLength(0))
            {
                throw new ArgumentException("matrix must have same count of axis");
            }

            var rowsCount = first.GetLength(0);
            var columnsCount = first.GetLength(1) + second.GetLength(1);
            var result = new T[rowsCount, columnsCount];

            for (int rows = 0; rows < rowsCount; rows++)
            {
                var columns = 0;
                for (; columns < first.GetLength(1); columns++)
                {
                    result[rows, columns] = first[rows, columns];
                }

                for (int secondMatrixColumns = 0;
                    secondMatrixColumns < second.GetLength(1);
                    secondMatrixColumns++, columns++)
                {
                    result[rows, columns] = second[rows, secondMatrixColumns];
                }
            }

            return result;
        }

        public static int[,] MatrixMultiplier(int[,] first, int[,] second)
        {
            if (first.GetLength(1) != second.GetLength(0))
            {
                throw new ArgumentException("Can't calculate bla bla bla");
            }

            var result = new int[first.GetLength(0), second.GetLength(1)];

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

        public static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            var result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            var lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = helperSolve(lum, b);

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }

            return result;
        }

        private static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
        {
            var rows = matrix.Length;
            var cols = matrix[0].Length;

            if (rows != cols)
                throw new Exception("Attempt to decompose a non-square m");

            var n = rows; // convenience

            var result = MatrixDuplicate(matrix);

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i)
            {
                perm[i] = i;
            }

            toggle = 1; // toggle tracks row swaps.
            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                var colMax = Math.Abs(result[j][j]); // find largest val in col
                var pRow = j;
                //for (int i = j + 1; i < n; ++i)
                //{
                //  if (result[i][j] > colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    (result[pRow], result[j]) = (result[j], result[pRow]);

                    (perm[pRow], perm[j]) = (perm[j], perm[pRow]);

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    (result[goodRow], result[j]) = (result[j], result[goodRow]);

                    (perm[goodRow], perm[j]) = (perm[j], perm[goodRow]);

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }
            } // main j column loop

            return result;
        } // MatrixDecompose


        
        private static double[] helperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }
    }
}