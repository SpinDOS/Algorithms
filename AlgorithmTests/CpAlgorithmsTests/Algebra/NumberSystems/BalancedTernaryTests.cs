using CpAlgorithms.Algebra.NumberSystems;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.NumberSystems
{
    public class BalancedTernaryTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-3)]
        [TestCase(38)]
        [TestCase(-3838)]
        public void TransformTest(int num)
        {
            var balancedTernary = new BalancedTernary(num);
            Assert.AreEqual(num, balancedTernary.ToInteger());
        }

        [Test]
        [TestCase(0, "0")]
        [TestCase(2, "1Z")]
        [TestCase(-2, "Z1")]
        [TestCase(3, "10")]
        [TestCase(-3, "Z0")]
        [TestCase(4, "11")]
        [TestCase(-4, "ZZ")]
        public void ToStringTest(int num, string expected)
        {
            Assert.AreEqual(expected, new BalancedTernary(num).ToString());
        }

        [Test]
        public void DefaultBalancedTernaryTest()
        {
            var defaultBalancedTernary = new BalancedTernary();
            Assert.AreEqual(0, defaultBalancedTernary.ToInteger());
            Assert.AreEqual("0", defaultBalancedTernary.ToString());
        }
    }
}