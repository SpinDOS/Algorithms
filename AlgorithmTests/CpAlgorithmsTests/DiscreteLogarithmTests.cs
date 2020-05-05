using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class DiscreteLogarithmTests
    {
        [Test]
        [TestCase(2U, 3U, 5U)]
        [TestCase(21U, 6U, 8U)]
        [TestCase(21U, 7U, 13U)]
        public void TestDiscreteLogarithm(uint a, uint x, uint m)
        {
            var b = (uint)CpAlgorithms.BinaryExponentiation.FastArithmetics.ModuloPow(a, x, m);

            var x1 = DiscreteLogarithm.SimpleSolve(a, b, m);
            Assert.NotNull(x1, "SimpleSolve");
            var b1 = (uint)CpAlgorithms.BinaryExponentiation.FastArithmetics.ModuloPow(a, x1.Value, m);
            Assert.AreEqual(b, b1, "SimpleSolve");

            var x2 = DiscreteLogarithm.OptimizedSolve(a, b, m);
            Assert.NotNull(x2, "OptimizedSolve");
            var b2 = (uint)CpAlgorithms.BinaryExponentiation.FastArithmetics.ModuloPow(a, x2.Value, m);
            Assert.AreEqual(b, b2, "OptimizedSolve");
        }

        [Test]
        [TestCase(1U, 2U, 3U)]
        [TestCase(2U, 3U, 7U)]
        public void TestNoSolution(uint a, uint b, uint m)
        {
            Assert.Null(DiscreteLogarithm.SimpleSolve(a, b, m), "SimpleSolve");
            Assert.Null(DiscreteLogarithm.OptimizedSolve(a, b, m), "OptimizedSolve");
        }
    }
}