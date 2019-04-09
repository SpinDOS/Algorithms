using System;
using static System.Math;

namespace GaussSLAU
{
    public static class GaussSolver
    {
        public static decimal Epsilon { get; set; } = 0.00000000000001m;
        
        public static decimal[] Solve(decimal[,] a, decimal[] y)
        {
            CheckArguments(a, y);

            a = Copy(a);
            y = Copy(y);
            var n = y.Length;
            
            for (var i = 0; i < n; i++)
            {
                var maxRow = GetMaxRow(a, i, n);
                if (maxRow < 0)
                    continue;
                
                SwapRows(a, y, n, i, maxRow);

                DivideByFirstColumn(a, y, n, i, i);
                
                for (var bottomRow = i + 1; bottomRow < n; bottomRow++)
                    if (DivideByFirstColumn(a, y, n, bottomRow, i))
                        SubstractRowFromBottomRow(a, y, n, i, bottomRow);
            }
                
            return GetBackwardSolution(a, y, n);
        }

        private static int GetMaxRow(decimal[,] a, int row, int n)
        {
            var maxRow = row;
            var col = row;
            var max = Abs(a[maxRow, col]);
            
            for (var i = row + 1; i < n; i++)
            {
                var abs = Abs(a[i, col]);
                if (abs > max)
                {
                    max = abs;
                    maxRow = i;
                }
            }
            
            return max <= Epsilon ? -1 : maxRow;
        }
        
        private static void SwapRows(decimal[,] a, decimal[] y, int n, int row1, int row2)
        {
            if (row1 == row2)
                return;
            
            Swap(ref y[row1], ref y[row2]);
            
            //for (var col = row1; col < n; col++)
            for (var col = 0; col < n; col++)
                Swap(ref a[row1, col], ref a[row2, col]);
        }
        
        private static bool DivideByFirstColumn(decimal[,] a, decimal[] y, int n, int row, int col)
        {
            var divider = a[row, col];
            if (Abs(divider) <= Epsilon)
                return false;

            //for (var j = col; j < n; j++)
            for (var j = 0; j < n; j++)
                a[row, j] /= divider;
            
            y[row] /= divider;

            return true;
        }

        private static void SubstractRowFromBottomRow(decimal[,] a, decimal[] y, int n, int row, int bottomRow)
        {
            // for (var col = row; col < n; col++)
            for (var col = 0; col < n; col++)
                a[bottomRow, col] -= a[row, col];
            
            y[bottomRow] -= y[row];
        }

        private static decimal[] GetBackwardSolution(decimal[,] a, decimal[] y, int n)
        {
            var solution = new decimal[n];
            for (var row = n - 1; row >= 0; row--)
            {
                var x = solution[row] = y[row];
                var col = row;
                for (var upperRow = 0; upperRow < row; upperRow++)
                    y[upperRow] -= a[upperRow, col] * x;
            }

            return solution;
        }

        private static void CheckArguments(decimal[,] a, decimal[] y) 
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            if (a.GetLength(0) != a.GetLength(1) || y.Length != a.GetLength(0))
                throw new ArgumentException($"Invalid dimentions: {a.GetLength(0)} x {a.GetLength(1)} = {y.Length}");
            if (Epsilon <= 0)
                throw new ArgumentException(nameof(Epsilon) + " is too small");
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        private static T[] Copy<T>(T[] arr)
        {
            var result = new T[arr.Length];
            Array.Copy(arr, result, arr.Length);
            return result;
        }

        private static T[,] Copy<T>(T[,] matrix)
        {
            var result = new T[matrix.GetLength(0), matrix.GetLength(1)];
            Array.Copy(matrix, result, matrix.Length);
            return result;
        }
    }
}