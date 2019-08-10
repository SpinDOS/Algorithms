namespace CpAlgorithms
{
    public static class MatrixHelper
    {
        public static long[,] Multiply(long[,] x, long[,] y)
        {
            // O (n1 * n2 * n3)
            var result = new long[x.GetLength(0), y.GetLength(1)];
            Multiply(x, y, result);
            return result;
        }
        
        public static void Multiply(long[,] x, long[,] y, long[,] result)
        {
            // O (n1 * n2 * n3)

            var xDim0 = x.GetLength(0);
            var xDim1 = x.GetLength(1); // equals to y.GetLength(0)
            var yDim1 = y.GetLength(1);
            
            for (var i = 0; i < xDim0; i++)
                for (var j = 0; j < yDim1; j++)
                {
                    var sum = 0L;
                    for (var k = 0; k < xDim1; k++)
                        sum += x[i, k] * y[k, j];
                    
                    result[i, j] = sum;
                }
        }

        public static long[,] OneMatrix(int n)
        {
            // O (n^2)
            var result = new long[n, n];
            for (var i = 0; i < n; i++)
                result[i, i] = 1L;
            return result;
        }
    }
}