using System.Numerics;
using CpAlgorithms.Algebra.ModularArithmetic;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.ModularArithmetic
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
            ClassicAssert.AreEqual(bigInt, garnerBigInt.ToBigInteger());
        }

        [Test]
        [TestCaseSource(nameof(SimpleNumsForTest))]
        public void NegateOperatorTest(long num)
        {
            var bigInt = -new BigInteger(num);
            var garnerBigInt = -new GarnerBigInteger(num);
            var garnerBigInt2 = new GarnerBigInteger(-num);
            ClassicAssert.AreEqual(bigInt, garnerBigInt.ToBigInteger());
            ClassicAssert.AreEqual(bigInt, garnerBigInt2.ToBigInteger());
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
            ClassicAssert.AreEqual(lhs + rhs,
                (long)(new GarnerBigInteger(lhs) + new GarnerBigInteger(rhs)).ToBigInteger(), "Operator +");
            ClassicAssert.AreEqual(lhs - rhs,
                (long)(new GarnerBigInteger(lhs) - new GarnerBigInteger(rhs)).ToBigInteger(), "Operator -");
            ClassicAssert.AreEqual(lhs * rhs,
                (long)(new GarnerBigInteger(lhs) * new GarnerBigInteger(rhs)).ToBigInteger(), "Operator *");
        }

        [Test]
        public void ComparisonOperatorsTest()
        {
            var one = new GarnerBigInteger(1);
            var one2 = new GarnerBigInteger(1);
            var minusOne = new GarnerBigInteger(-1);

            ClassicAssert.True(one == one2, "operator ==");
            ClassicAssert.False(one == minusOne, "operator ==");

            ClassicAssert.False(one != one2, "operator !=");
            ClassicAssert.True(one != minusOne, "operator !=");

            ClassicAssert.True(one.Equals(one2), "Equals");
            ClassicAssert.False(one.Equals(minusOne), "Equals");

            ClassicAssert.AreEqual(one.GetHashCode(), one2.GetHashCode(), "GetHashCode");
            ClassicAssert.AreNotEqual(one.GetHashCode(), minusOne.GetHashCode(), "GetHashCode");
        }
    }
}
