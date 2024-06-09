using CpAlgorithms.Algebra.ModularArithmetic.MontgomeryMultiplication;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.ModularArithmetic
{
    public class MontgomeryMultiplicationTests
    {
        private const uint Modulo = 127;

        private MontgomerySpace _testSpace;

        [OneTimeSetUp]
        public void InitTestSpace()
        {
            _testSpace = new MontgomerySpace(Modulo);
        }

        [Test]
        [TestCase(0U)]
        [TestCase(1U)]
        [TestCase(2U)]
        [TestCase(4U)]
        [TestCase(15U)]
        [TestCase(17U)]
        [TestCase(32U)]
        [TestCase(135U)]
        [TestCase(Modulo)]
        public void TestTransformToFromSpace(uint x)
        {
            var naive = _testSpace.ToSpaceNaive(x);
            ClassicAssert.AreEqual(x % Modulo, (uint)_testSpace.FromSpace(naive));
            var simple = _testSpace.ToSpace(x);
            ClassicAssert.AreEqual(x % Modulo, (uint)_testSpace.FromSpace(simple));
            var fast = _testSpace.ToSpace(x);
            ClassicAssert.AreEqual(x % Modulo, (uint)_testSpace.FromSpace(fast));
        }

        [Test]
        [TestCase(100U, 35U)]
        [TestCase(35U, 100U)]
        [TestCase(Modulo, 2U)]
        [TestCase(Modulo - 1, 3U)]
        [TestCase(Modulo + 1, 3U)]
        public void TestOperations(uint lhs, uint rhs)
        {
            var montgomeryLhs = _testSpace.ToSpaceFast(lhs);
            var montgomeryRhs = _testSpace.ToSpaceFast(rhs);

            var montgomerySum = _testSpace.Add(montgomeryLhs, montgomeryRhs);
            var sum = (uint) _testSpace.FromSpace(montgomerySum);
            ClassicAssert.AreEqual((lhs + rhs) % Modulo, sum);

            var montgomerySubtract = _testSpace.Subtract(montgomeryLhs, montgomeryRhs);
            var subtract = (uint) _testSpace.FromSpace(montgomerySubtract);
            ClassicAssert.AreEqual(ModuloSubtract(lhs, rhs), subtract);

            var montgomeryProduct = _testSpace.Multiply(montgomeryLhs, montgomeryRhs);
            var product = (uint) _testSpace.FromSpace(montgomeryProduct);
            ClassicAssert.AreEqual((lhs * rhs) % Modulo, product);
        }

        private static uint ModuloSubtract(uint lhs, uint rhs) =>
            (lhs + Modulo - (rhs % Modulo)) % Modulo;
    }
}
