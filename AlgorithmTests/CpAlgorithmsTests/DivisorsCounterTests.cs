using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class DivisorsCounterTests
    {
        [Test]
        public void NumberOfDivisorsTest()
        {
            // https://oeis.org/A000005
            var correctValues = new uint[]
            {
                1, 2, 2, 3, 2, 4, 2, 4, 3, 4, 2, 6, 2, 4, 4, 5, 2, 6, 2, 6, 4, 4, 2, 8,
                3, 4, 4, 6, 2, 8, 2, 6, 4, 4, 4, 9, 2, 4, 4, 8, 2, 8, 2, 6, 6, 4, 2, 10,
                3, 6, 4, 6, 2, 8, 4, 8, 4, 4, 2, 12, 2, 4, 6, 7, 4, 8, 2, 6, 4, 8, 2, 12,
                2, 4, 6, 6, 4, 8, 2, 10, 5, 4, 2, 12, 4, 4, 4, 8, 2, 12, 4, 6, 4, 4, 4,
                12, 2, 6, 6, 9, 2, 8, 2, 8,
            };

            for (var n = 1; n <= correctValues.Length; n++)
                Assert.AreEqual(correctValues[n - 1], DivisorsCounter.NumberOfDivisors(n), n.ToString());
        }
        
        [Test]
        public void SumOfDivisorsTest()
        {
            // https://oeis.org/A001065
            var correctValues = new uint[]
            {
                0, 1, 1, 3, 1, 6, 1, 7, 4, 8, 1, 16, 1, 10, 9, 15, 1, 21, 1, 22, 11,
                14, 1, 36, 6, 16, 13, 28, 1, 42, 1, 31, 15, 20, 13, 55, 1, 22, 17,
                50, 1, 54, 1, 40, 33, 26, 1, 76, 8, 43, 21, 46, 1, 66, 17, 64, 23,
                32, 1, 108, 1, 34, 41, 63, 19, 78, 1, 58, 27, 74, 1, 123, 1, 40,
                49, 64, 19, 90, 1, 106,
            };

            for (var n = 1; n <= correctValues.Length; n++)
                Assert.AreEqual(correctValues[n - 1] + n, DivisorsCounter.SumOfDivisors(n), n.ToString());
        }
    }
}