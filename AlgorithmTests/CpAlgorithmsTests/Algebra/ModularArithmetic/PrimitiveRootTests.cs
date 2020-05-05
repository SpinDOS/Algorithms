using System.Collections.Generic;
using CpAlgorithms.Algebra.ModularArithmetic;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.ModularArithmetic
{
    public class PrimitiveRootTests
    {
        [Test]
        public void IsPrimitiveRootTest()
        {
            const uint m = 26U;
            var primitiveRootsOfM = new HashSet<uint>() {7, 11, 15, 19,};
            for (var i = 1U; i < m; i++)
            {
                if (primitiveRootsOfM.Contains(i))
                    Assert.True(PrimitiveRoot.IsPrimitiveRoot(i, m), $"{i} must be primitive root of {m}");
                else
                    Assert.False(PrimitiveRoot.IsPrimitiveRoot(i, m), $"{i} must not be primitive root of {m}");
            }
        }
        
        [Test]
        [TestCase(1U)]
        [TestCase(2U)]
        [TestCase(3U)]
        [TestCase(4U)]
        [TestCase(6U)]
        [TestCase(98U)]
        [TestCase(243U)]
        public void PrimitiveRootTest(uint m)
        {
            var primitiveRoot = PrimitiveRoot.CalculatePrimitiveRoot(m);
            Assert.NotNull(primitiveRoot);
            Assert.True(PrimitiveRoot.IsPrimitiveRoot(primitiveRoot.Value, m), m.ToString());
        }

        [Test]
        [TestCase(12U)]
        [TestCase(21U)]
        [TestCase(75U)]
        [TestCase(92U)]
        public void NoPrimitiveRootTest(uint m)
        {
            Assert.Null(PrimitiveRoot.CalculatePrimitiveRoot(m));
        }
    }
}
