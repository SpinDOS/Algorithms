using System.Numerics;
using CpAlgorithms.Algebra.Miscellaneous;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.Miscellaneous
{
    public class SubmaskEnumerationTests
    {
        [Test]
        [TestCase(0U)]
        [TestCase(1U)]
        [TestCase(2U)]
        [TestCase(15U)]
        [TestCase(16U)]
        [TestCase(17U)]
        public void EnumerateBitSubmasksDescendingTest(uint m)
        {
            var count = 0;
            var prev = m + 1;
            foreach (var submask in SubmaskEnumeration.EnumerateBitSubmasksDescending(m))
            {
                ClassicAssert.True(IsSubmask(submask, m));
                ClassicAssert.Less(submask, prev);
                prev = submask;
                ++count;
            }

            ClassicAssert.AreEqual(ExpectedSubmasksCount(m), count);
        }

        private static bool IsSubmask(uint submask, uint mask)
        {
            var differingBits = submask ^ mask;
            return (submask & differingBits) == 0;
        }

        private static int ExpectedSubmasksCount(uint m)
        {
            var numberOfEnabledBits = BitOperations.PopCount(m);
            return 1 << (int)numberOfEnabledBits;
        }
    }
}
