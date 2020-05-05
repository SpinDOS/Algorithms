using System.Linq;
using CpAlgorithms.Algebra.PrimeNumbers;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.PrimeNumbers
{
    public class EratosthenesSieveTests
    {
        [Test]
        public void EratosthenesSieveTest()
        {
            var nToCheck = new int[] {0, 1, 2, 3, 4, 1000, 1004, 1005, 1010, 1011, 1012, 1013};
            foreach (var n in nToCheck)
            {
                var expected = PrimeNumbersSequence.Primes.TakeWhile(it => it <= n).ToArray();
                Assert.AreEqual(expected, EratosthenesSieve.ClassicSieve(n).ToArray(), $"Classic sieve, {n}");
                Assert.AreEqual(expected, EratosthenesSieve.OddOptimizedSieve(n).ToArray(), $"Odd optimized sieve, {n}");
                Assert.AreEqual(expected, EratosthenesSieve.LinearSieve(n, out _).ToArray(), $"Linear sieve, {n}");
            }
        }
    }
}