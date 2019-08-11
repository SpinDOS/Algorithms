using System;
using System.Linq;
using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class PrimalityTests
    {
        [Test]
        public void PrimalityTest()
        {
            foreach (var num in Enumerable.Range(0, PrimeNumbers.Primes.Max() + 1))
            {
                var isPrime = Array.BinarySearch(PrimeNumbers.Primes, num) >= 0;
                Assert.AreEqual(isPrime, Primality.IsPrimeSimple((ulong)num), $"Simple primality for {num}");
                Assert.AreEqual(isPrime, Primality.MillerRabin(num), $"MillerRabin for {num}");
            }
        }
    }
}