using CpAlgorithms.Algebra.Fundamentals;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.Fundamentals
{
    public class DiophantineTests
    {
        [Test]
        public void SolveTest()
        {
            for (var i = 0; i < 100; i++)
                CheckSolvable(2, 6, i, i % 2 == 0);
            
            for (var i = 0; i < 100; i++)
                CheckSolvable(3, 6, i, i % 3 == 0);
            
            for (var i = 0; i < 100; i++)
                CheckSolvable(12, -6, i, i % 6 == 0);
            
            for (var i = 0; i < 100; i++)
                CheckSolvable(-1, 6, i, true);
            
            for (var i = 0; i < 100; i++)
                CheckSolvable(2, -3, i, true);
            
            for (var i = 0; i < 100; i++)
                CheckSolvable(0, -3, i, i % 3 == 0);
            
            CheckSolvable(0, 0, 0, true);
            CheckSolvable(0, 0, 1, false);
        }

        [Test]
        public void SolutionCountTest()
        {
            CheckSolutionCount(4, 6, 10, (1, 10), (-10, 1), 4);
            CheckSolutionCount(4, 6, 10, (1, 10), (-1, 10), 2);
            CheckSolutionCount(4, 6, 10, (0, 10), (2, 10), 0);
            CheckSolutionCount(4, 6, 5, (0, 10), (-10, 1), 0);
            
            CheckSolutionCount(2, 3, 10, (1, 10), (-10, 1), 2);
            CheckSolutionCount(2, 3, 10, (1, 10), (-1, 10), 2);
            CheckSolutionCount(2, 3, 10, (0, 10), (2, 10), 1);
            CheckSolutionCount(2, 3, 5, (0, 10), (-10, 0), 3);
            
            CheckSolutionCount(0, 2, 4, (-10, 10), (-10, 10), 21);
            CheckSolutionCount(0, 2, 5, (-10, 10), (-10, 10), 0);
            CheckSolutionCount(0, 2, 4, (-10, 10), (-10, 1), 0);
        }
        
        private void CheckSolvable(long a, long b, long c, bool solvable)
        {
            var equation = $"{a} * x + {b} * y = {c}";
            Assert.AreEqual(solvable, DiophantineSolver.TrySolve(a, b, c, out var x, out var y, out _), equation);
            if (solvable)
                Assert.AreEqual(c, a * x + b * y, equation);
        }

        private void CheckSolutionCount(long a, long b, long c, 
            (long minX, long maxX) intervalX,
            (long minY, long maxY) intervalY, 
            ulong solutionCount)
        {
            var equation = $"{a} * x + {b} * y = {c}, x [{intervalX.minX}, {intervalX.maxX}], y [{intervalY.minY}, {intervalY.maxY}]";
            Assert.AreEqual(solutionCount, DiophantineSolver.FindSolutionsCount(a, b, c, intervalX, intervalY), equation);
        }
    }
}