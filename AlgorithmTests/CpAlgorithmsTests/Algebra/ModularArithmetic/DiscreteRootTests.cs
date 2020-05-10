using CpAlgorithms.Algebra.Fundamentals.BinaryExponentiation;
using CpAlgorithms.Algebra.ModularArithmetic;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.ModularArithmetic
{
    public class DiscreteRootTests
    {
        [Test]
        [TestCase(0U, 10U, 7U)]
        [TestCase(1U, 0U, 7U)]
        [TestCase(1U, 2U, 7U)]
        [TestCase(110U, 5U, 7U)]
        public void DiscreteRootTest(uint a, uint k, uint m)
        {
            var discreteRoot = DiscreteRoot.CalculateDiscreteRoot(a, k, m);
            Assert.NotNull(discreteRoot);
            Assert.AreEqual(a % m, (uint) FastArithmetics.ModuloPow(discreteRoot.Value, k, m));
            
            var count = 0U;
            foreach (var discreteRootN in DiscreteRoot.CalculateDiscreteRoots(a, k, m))
            {
                Assert.AreEqual(a % m, (uint)FastArithmetics.ModuloPow(discreteRootN, k, m), 
                    $"{count}'th discrete root is wrong");
                ++count;
            }
            
            Assert.NotZero(count, "No discrete roots found");
        }

        [Test]
        [TestCase(2U, 0U, 7U)]
        [TestCase(2U, 2U, 5U)]
        public void NoDiscreteRootTest(uint a, uint k, uint m)
        {
            Assert.Null(DiscreteRoot.CalculateDiscreteRoot(a, k, m));
            Assert.IsEmpty(DiscreteRoot.CalculateDiscreteRoots(a, k, m));
        }
    }
}
