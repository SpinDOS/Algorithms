using System.Collections.Generic;
using System.Numerics;
using CpAlgorithms;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class GarnerBigIntegerTests
    {
        private static readonly long[] SimpleNumsForTest = new long[] { 0, -1, 127, 128, int.MinValue };

        [OneTimeSetUp]
        public void InitGarnerBigInteger()
        {
            GarnerBigInteger.InitPrimes();
        }
        
        [Test]
        [TestCaseSource(nameof(SimpleNumsForTest))]
        public void ParseBigIntegerTest(long num)
        {
            var bigInt = new BigInteger(num);
            var garnerBigInt = new GarnerBigInteger(num);
            Assert.AreEqual(bigInt, garnerBigInt.ToBigInteger());
        }
        
        [Test]
        [TestCaseSource(nameof(SimpleNumsForTest))]
        public void NegateOperatorTest(long num)
        {
            var bigInt = -new BigInteger(num);
            var garnerBigInt = -new GarnerBigInteger(num);
            var garnerBigInt2 = new GarnerBigInteger(-num);
            Assert.AreEqual(bigInt, garnerBigInt.ToBigInteger());
            Assert.AreEqual(bigInt, garnerBigInt2.ToBigInteger());
        }

        [Test]
        [TestCase(128, 0)]
        [TestCase(-128, -1)]
        [TestCase(128, 255)]
        [TestCase(128, -255)]
        [TestCase(-128, -255)]
        [TestCase(-128, 255)]
        public void ArithmeticOperatorsTest(long lhs, long rhs)
        {
            Assert.AreEqual(lhs + rhs, 
                (long)(new GarnerBigInteger(lhs) + new GarnerBigInteger(rhs)).ToBigInteger(), "Operator +");
            Assert.AreEqual(lhs - rhs, 
                (long)(new GarnerBigInteger(lhs) - new GarnerBigInteger(rhs)).ToBigInteger(), "Operator -");
            Assert.AreEqual(lhs * rhs, 
                (long)(new GarnerBigInteger(lhs) * new GarnerBigInteger(rhs)).ToBigInteger(), "Operator *");
        }

        [Test]
        public void ComparisonOperatorsTest()
        {
            var one = new GarnerBigInteger(1);
            var one2 = new GarnerBigInteger(1);
            var minusOne = new GarnerBigInteger(-1);
            
            Assert.True(one == one2, "operator ==");
            Assert.False(one == minusOne, "operator ==");
            
            Assert.False(one != one2, "operator !=");
            Assert.True(one != minusOne, "operator !=");
            
            Assert.True(one.Equals(one2), "Equals");
            Assert.False(one.Equals(minusOne), "Equals");
            
            Assert.AreEqual(one.GetHashCode(), one2.GetHashCode(), "GetHashCode");
            Assert.AreNotEqual(one.GetHashCode(), minusOne.GetHashCode(), "GetHashCode");
        }
    }
}
