using CpAlgorithms.Algebra.NumberTheoreticFunctions;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.NumberTheoreticFunctions
{
    public class EulerTotientFunctionTests
    {
        [Test]
        public void EulerTotientFunctionTest()
        {
            // https://oeis.org/A000010
            var correctValues = new long[]
            {
                1, 1, 2, 2, 4, 2, 6, 4, 6, 4, 10, 4, 12, 6, 8, 8, 16, 6, 18, 8, 12,
                10, 22, 8, 20, 12, 18, 12, 28, 8, 30, 16, 20, 16, 24, 12, 36, 18,
                24, 16, 40, 12, 42, 20, 24, 22, 46, 16, 42, 20, 32, 24, 52, 18,
                40, 24, 36, 28, 58, 16, 60, 30, 36, 32, 48, 20, 66, 32, 44,
            };

            for (var n = 1; n <= correctValues.Length; n++)
                Assert.AreEqual(correctValues[n - 1], EulerTotientFunction.Phi(n), $"Phi({n})");
        }
    }
}