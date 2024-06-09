using System.Collections.Generic;
using System.Numerics;
using CpAlgorithms.Algebra.Fundamentals;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.Fundamentals
{
    public class EuclideanGcdAlgorithmTests
    {
        [Test]
        [TestCaseSource(nameof(GcdTestCaseData))]
        public void GcdTest(long a, long b, long gcd)
        {
            ClassicAssert.AreEqual(gcd, EuclideanGcdAlgorithm.Gcd(a, b));
            ClassicAssert.AreEqual(gcd, EuclideanGcdAlgorithm.Gcd(b, a));
        }

        [Test]
        [TestCaseSource(nameof(GcdTestCaseData))]
        public void ExtendedGcdTest(long a, long b, long gcd)
        {
            ClassicAssert.AreEqual(gcd, EuclideanGcdAlgorithm.ExtendedGcd(a, b, out var x, out var y));
            ClassicAssert.AreEqual(gcd, a * x + b * y);

            ClassicAssert.AreEqual(gcd, EuclideanGcdAlgorithm.ExtendedGcd(b, a, out x, out y));
            ClassicAssert.AreEqual(gcd, b * x + a * y);
        }

        [Test]
        [TestCaseSource(nameof(GcdTestCaseData))]
        public void LcmTest(long a, long b, long gcd)
        {
            var lcm = (new BigInteger(a) * b) / gcd;
            if (lcm > long.MaxValue)
                Assert.Ignore("Least common multiple is too large for long");

            ClassicAssert.AreEqual((long)lcm, EuclideanGcdAlgorithm.Lcm(a, b));
            ClassicAssert.AreEqual((long)lcm, EuclideanGcdAlgorithm.Lcm(b, a));
        }

        private static IEnumerable<TestCaseData> GcdTestCaseData()
        {
            const long Prime1 = 1_000_000_016_531;
            const long Prime2 = 1_000_000_016_347;

            yield return new TestCaseData(0, 2, 2);
            yield return new TestCaseData(10, 1, 1);
            yield return new TestCaseData(10, 10, 10);
            yield return new TestCaseData(12, 6, 6);
            yield return new TestCaseData(36, 48, 12);
            yield return new TestCaseData(Prime1, Prime2, 1);
            yield return new TestCaseData(Prime1, Prime1, Prime1);
            yield return new TestCaseData(Prime1 * 2, Prime2, 1);
            yield return new TestCaseData(Prime1 * 2, Prime2 * 6, 2);
            yield return new TestCaseData(Prime1 * 2, Prime2 * 3, 1);

        }
    }
}
