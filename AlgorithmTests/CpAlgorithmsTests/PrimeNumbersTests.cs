using System;
using System.Linq;
using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class EratosthenesSieveTests
    {
        // https://oeis.org/A000040
        private static readonly int[] PrimeNumbers = new int[170]
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103,
            107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
            227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347,
            349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
            467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607,
            613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743,
            751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883,
            887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997, 1009, 1013,
        };

        [Test]
        public void EratosthenesSieveTest()
        {
            var nToCheck = new int[] {0, 1, 2, 3, 4, 1000, 1004, 1005, 1010, 1011, 1012, 1013};
            foreach (var n in nToCheck)
            {
                var expected = PrimeNumbers.TakeWhile(it => it <= n).ToArray();
                Assert.AreEqual(expected, EratosthenesSieve.ClassicSieve(n).ToArray(), $"Classic sieve, {n}");
                Assert.AreEqual(expected, EratosthenesSieve.OddOptimizedSieve(n).ToArray(), $"Odd optimized sieve, {n}");
                Assert.AreEqual(expected, EratosthenesSieve.LinearSieve(n, out _).ToArray(), $"Linear sieve, {n}");
            }
        }

        [Test]
        public void PrimalityTest()
        {
            foreach (var num in Enumerable.Range(0, PrimeNumbers.Max() + 1))
            {
                var isPrime = Array.BinarySearch(PrimeNumbers, num) >= 0;
                Assert.AreEqual(isPrime, Primality.IsPrimeSimple((ulong)num), $"Simple primality for {num}");
                Assert.AreEqual(isPrime, Primality.MillerRabin(num), $"MillerRabin for {num}");
            }
        }

        [Test]
        [TestCase(5, 10_000)]
        [TestCase(20, 200)]
        public void FactorizationTest(int possiblePrimesCount, int iterations)
        {
            var rnd = TestContext.CurrentContext.Random;
            for (var i = 0; i < iterations; i++)
            {
                var factors = Enumerable.Range(0, rnd.Next(1, 10))
                    .Select(it => (long)PrimeNumbers[rnd.Next(possiblePrimesCount)])
                    .OrderBy(it => it)
                    .ToList();

                var composite = factors.Aggregate(1L, (x, y) => x * y);
                Assert.AreEqual(factors, IntegerFactorization.SimpleFactorize(composite), $"Simple factorize for {composite}");
                Assert.AreEqual(factors, IntegerFactorization.OptimizedFactorize(composite), $"Optimized factorize for {composite}");
                Assert.AreEqual(factors, IntegerFactorization.EratosthenesSieveFactorize(composite), $"Eratosthenes sieve factorize for {composite}");

                var byPowers = factors.GroupBy(it => it).ToDictionary(g => g.Key, g => (uint)g.Count());
                Assert.AreEqual(byPowers, IntegerFactorization.FactorizeToPowers(composite), $"Factorization to factors with powers for {composite}");
            }
        }
    }
}