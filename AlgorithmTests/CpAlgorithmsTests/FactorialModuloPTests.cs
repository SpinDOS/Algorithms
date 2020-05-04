using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class FactorialModuloPTests
    {
        [Test]
        [TestCase(7U, 11U)]
        [TestCase(11U, 7U)]
        [TestCase(12U, 7U)]
        [TestCase(12U, 11U)]
        public void FactorialModuloPTest(uint n, uint p)
        {
            Assert.AreEqual(
                NaiveFactorialModuloP(n, p),
                FactorialModuloP.CalculateFactorialModuloP(n, p));
        }

        private static uint NaiveFactorialModuloP(uint n, uint p)
        {
            var result = 1UL;
            for (var i = 2UL; i <= n; i++)
            {
                result *= i;
                while (result % p == 0)
                    result /= p;
                result %= p;
            }

            return (uint)result;
        }
    }
}