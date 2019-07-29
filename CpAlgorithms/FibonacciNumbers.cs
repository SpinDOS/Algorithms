using System;
using CpAlgorithms.BinaryExponentiation;

namespace CpAlgorithms
{
    public static class FibonacciNumbers
    {
        public static (long fn, long fnPlus1) FibonacciByMatrixPow(uint n)
        {
            // O (ln n)
            // Return F_n and F_(n + 1)
            
            var matrix = new long[2, 2]
            {
                {0, 1,},
                {1, 1,},
            };

            matrix = FastArithmetics.MatrixPow(matrix, n);
            return (matrix[1, 0], matrix[1, 1]);
        }

        public static (long fn, long fnPlus1) FibonacciByBinaryExponentiation(uint n)
        {
            // O (ln n)

            if (n == 0)
                return (0, 1);
            
            var (fHalfN, fHalfNPlus1) = FibonacciByBinaryExponentiation(n >> 1);
            var c = fHalfN * (2 * fHalfNPlus1 - fHalfN);
            var d = fHalfN * fHalfN + fHalfNPlus1 * fHalfNPlus1;
            return (n & 1) == 0? (c, d) : (d, c + d);
        }

//        public static long FibonacciByDouble(uint n)
//        {
//            // O (1)
//
//            var part1 = Math.Pow((1 + Math.Sqrt(5)) / 2, n);
//            var part2 = Math.Pow((1 - Math.Sqrt(5)) / 2, n);
//            var result = (part1 - part2) / Math.Sqrt(5);
//            return (long) Math.Round(result);
//        }

        public static void FibonacciEncode(ulong x, bool[] buffer)
        {
            var n = FindFibonacciIndex(x, out var fib);
            buffer[n - 1] = true;
            Array.Clear(buffer, 0, n - 2);
            
            while (true)
            {
                buffer[n - 2] = true;
                x -= fib;
                if (x == 0)
                    return;
                
                n = FindFibonacciIndex(x, out fib);
            }
        }

        public static ulong FibonacciDecode(bool[] code)
        {
            ulong prev0 = 0, prev1 = 1, result = 0;
            for (var i = 0; true; i++)
            {
                var next = prev0 + prev1;
                if (code[i])
                {
                    result += next;
                    if (code[i + 1])
                        return result;
                }

                prev0 = prev1;
                prev1 = next;
            }
        }

        private static int FindFibonacciIndex(ulong x, out ulong fib)
        {
            ulong prev0 = 0, prev1 = 1;
            for (var i = 1; true; i++)
            {
                var next = prev0 + prev1;
                if (next > x)
                {
                    fib = prev1;
                    return i;
                }

                prev0 = prev1;
                prev1 = next;
            }
        }
    }
}