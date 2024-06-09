using System.Linq;
using CpAlgorithms.Algebra.Fundamentals;
using CpAlgorithms.Algebra.ModularArithmetic;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.ModularArithmetic
{
    public class ModularInverseTests
    {
        [Test]
        [Repeat(10_000)]
        public void ModularInverseTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            var a = rnd.Next(1, 100_000);
            var m = rnd.Next(2, 100_000);
            var inverseExists = EuclideanGcdAlgorithm.Gcd(a, m) == 1;
            ClassicAssert.AreEqual(inverseExists, ModularInverse.EuclideanModularInverse(a, m, out var x));
            if (inverseExists)
                AssertModularInverse(a, x, m);
        }

        [Test]
        public void ModularInverseForPrimeTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            foreach (var prime in PrimeNumbersSequence.Primes)
            {
                var coprimes = Enumerable.Range(0, int.MaxValue)
                    .Select(_ => rnd.Next(1, 100_000))
                    .Where(a => a % prime != 0)
                    .Take(10);

                foreach (var a in coprimes)
                {
                    var inverse = ModularInverse.BinaryExponentiationModularInverse(a, prime);
                    AssertModularInverse(a, inverse, prime);
                }
            }
        }

        [Test]
        public void ModularInverseForRangeTest()
        {
            foreach (var prime in PrimeNumbersSequence.Primes)
            {
                var inverses = ModularInverse.ModularInverseForRange(prime);
                for (var i = 1; i < inverses.Length; i++)
                    AssertModularInverse(i, inverses[i], prime);
            }
        }

        private void AssertModularInverse(long a, long x, long m)
        {
            ClassicAssert.AreEqual(1, (a * x) % m, $"({a} * {x}) % {m}");
        }
    }
}
