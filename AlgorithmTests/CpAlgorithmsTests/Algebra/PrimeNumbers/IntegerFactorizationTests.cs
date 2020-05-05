using System.Linq;
using CpAlgorithms.Algebra.PrimeNumbers;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.PrimeNumbers
{
    public class FactorizationTests
    {
        [Test]
        [TestCase(5, 10_000)]
        [TestCase(20, 200)]
        public void FactorizationTest(int possiblePrimesCount, int iterations)
        {
            var rnd = TestContext.CurrentContext.Random;
            for (var i = 0; i < iterations; i++)
            {
                var factors = Enumerable.Range(0, rnd.Next(1, 10))
                    .Select(it => (long)PrimeNumbersSequence.Primes[rnd.Next(possiblePrimesCount)])
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