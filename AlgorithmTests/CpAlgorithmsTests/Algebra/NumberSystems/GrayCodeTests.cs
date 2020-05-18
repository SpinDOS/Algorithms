using CpAlgorithms.Algebra.NumberSystems;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.NumberSystems
{
    public class GrayCodeTests
    {
        private uint[] _grayCodes;
        
        [OneTimeSetUp]
        public void GenerateGrayCodes()
        {
            var grayCodes = new uint[2048];
            for (var i = 1U; i < grayCodes.Length; i <<= 1)
                for (var j = 0U; j < i; j++)
                    grayCodes[i + j] = i | grayCodes[i - j - 1];
            _grayCodes = grayCodes;
        }
        
        [Test]
        [TestCase(0U)]
        [TestCase(1U)]
        [TestCase(5U)]
        [TestCase(128U)]
        [TestCase((1U << 10) | (1U << 8) | 1)]
        [TestCase(2047U)]
        public void IsCorrectGrayCodeTest(uint num)
        {
            var grayCode = new GrayCode(num);
            Assert.AreEqual(_grayCodes[num], grayCode.Code);
        }
        
        [Test]
        [TestCase(0U)]
        [TestCase(1U)]
        [TestCase(15U)]
        [TestCase(1U << 28)]
        [TestCase((1U << 28) + 1)]
        [TestCase((1U << 28) - 1)]
        [TestCase((1U << 28) | (1U << 27))]
        [TestCase((1U << 28) | (1U << 15) | (1U << 2))]
        public void TransformTest(uint num)
        {
            var grayCode = new GrayCode(num);
            Assert.AreEqual(num, grayCode.ToInteger());
            Assert.AreEqual(num, grayCode.ToIntegerFast());
        }

        [Test]
        public void DefaultGrayCodeTest()
        {
            var defaultGrayCode = new GrayCode();
            Assert.AreEqual(0U, defaultGrayCode.Code);
            Assert.AreEqual(0U, defaultGrayCode.ToInteger());
            Assert.AreEqual(0U, defaultGrayCode.ToIntegerFast());
        }
    }
}