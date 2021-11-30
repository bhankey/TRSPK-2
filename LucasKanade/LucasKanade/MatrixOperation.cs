using System;

namespace LucasKanade
{
    // TODO написать вычисление обратной матрицы
    public static class MatrixOperation
    {
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
            var result = new T[rowsCount,columnsCount];
            
            for (int rows = 0; rows < rowsCount; rows++)
            {
                var columns = 0;
                for (; columns < first.GetLength(1); columns++)
                {
                    result[rows, columns] = first[rows, columns];
                }
                for (int secondMatrixColumns = 0; secondMatrixColumns < second.GetLength(1); secondMatrixColumns++, columns++)
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
                        result[i,j] += first[i,k] * second[k,j];
                    }
                }
            }

            return result;
        }
    }
}