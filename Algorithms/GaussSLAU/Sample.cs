using System;
using System.Diagnostics;

namespace GaussSLAU
{
    static class Sample
    {
        public static void Run()
        {
            Console.Write("Enter n: ");
            var n = int.Parse(Console.ReadLine());

            var a = ReadMatrix(n);
            var y = ReadRightPart(n);

            Console.WriteLine();
            Console.WriteLine("Solving");
            Console.WriteLine();

            var sw = Stopwatch.StartNew();
            var x = GaussSolver.Solve(a, y);
            sw.Stop();

            Console.WriteLine($"Got solution in {sw.ElapsedMilliseconds} milliseconds");

            Console.WriteLine();
            PrintProblem(a, y);
            Console.WriteLine();
            PrintSolution(x);

            Console.ReadKey(true);
        }

        private static decimal[,] ReadMatrix(int n)
        {
            var a = new decimal[n, n];
            
            for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                Console.Write($"a[{i}][{j}] = ");
                a[i, j] = decimal.Parse(Console.ReadLine());
            }

            return a;
        }

        private static decimal[] ReadRightPart(int n)
        {
            var y = new decimal[n];
            
            for (int i = 0; i < n; i++)
            {
                Console.Write($"y[{i}] = ");
                y[i] = decimal.Parse(Console.ReadLine());
            }

            return y;
        }

        private static void PrintProblem(decimal[,] a, decimal[] y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < y.Length; j++)
                {
                    Console.Write($"{a[i, j]} * x{j}");
                    if (j + 1 != y.Length)
                        Console.Write(" + ");
                }

                Console.WriteLine($" = {y[i]}");
            }
        }

        private static void PrintSolution(decimal[] x)
        {
            for (int i = 0; i < x.Length; i++)
                Console.WriteLine($"x{i} = " + x[i]);
        }
    }
}