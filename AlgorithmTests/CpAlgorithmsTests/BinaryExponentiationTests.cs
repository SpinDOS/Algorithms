using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CpAlgorithms;
using CpAlgorithms.BinaryExponentiation;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests
{
    public class BinaryExponentiationTests
    {
        [Test]
        public void FastPowTest()
        {
            for (var x = -8L; x <= 8; x++)
                for (var y = 0U; y < 20; y++)
                {
                    var naivePow = (long) NaivePow(x, y);
                    var fastPow = FastArithmetics.Pow(x, y);
                    Assert.AreEqual(naivePow, fastPow, $"Wrong fast pow {x} ^ {y}");
                }
        }
        
        [Test]
        public void FastPowModuloTest()
        {
            var maxModuloWithoutOverflow = (long)Math.Sqrt(long.MaxValue);
            
            var modulos = Enumerable.Range(1, 10).Select(it => (long)it).ToList();
            for (var powOf10 = 1U; true; powOf10++)
            {
                var modulo = NaivePow(10, powOf10) + 1;
                if (modulo > maxModuloWithoutOverflow)
                    break;
                modulos.Add((long)modulo);
            }
            
            foreach (var m in modulos)
                for (var x = -8L; x <= 8; x++)
                    for (var y = 0U; y < 40; y++)
                    {
                        var naivePow = (long)(NaivePow(x, y) % m);
                        var fastPow = FastArithmetics.ModuloPow(x, y, m);
                        Assert.AreEqual(naivePow, fastPow, $"Wrong fast modulo pow ({x} ^ {y}) % {m}");
                    }
        }

        [Test]
        public void FastModuloBigMultiply()
        {
            foreach (var m in Get10LargeLongs(long.MaxValue / 2))
                foreach (var x in Get10LargeLongs(long.MaxValue).Concat(new [] {m}))
                    foreach (var y in Get10LargeLongs(long.MaxValue).Concat(new [] {m}))
                    {
                        var naiveMultiply = (long) ((new BigInteger(x) * y) % m);
                        var fastMultiply = FastArithmetics.ModuloBigMultiply(x, y, m);
                        Assert.AreEqual(naiveMultiply, fastMultiply, $"Wrong modulo multiply ({x} * {y}) % {m}");
                    }
        }

        [Test]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("aba")]
        [TestCase("abcdef")]
        public void FastPermutationTest(string stringForPermutation)
        {
            foreach (var permutation in GetPermutationsForTest(stringForPermutation.Length))
            {
                var expectedResult = stringForPermutation;
            
                for (var k = 0U; k < 100; k++)
                {
                    var sourceArray = stringForPermutation.ToCharArray();
                    FastPermutation.ApplyPermutation(sourceArray, permutation.ToArray(), k);
                    Assert.AreEqual(expectedResult, new string(sourceArray), 
                        $"Failed applying permutation '{string.Join(", ", permutation)}' {k} times");
                
                    expectedResult = new string(permutation.Select(i => expectedResult[i]).ToArray());
                }
            }
        }

        private BigInteger NaivePow(long x, uint y)
        {
            var result = new BigInteger(1);
            for (var i = 0; i < y; i++)
                result *= x;
            return result;
        }

        private IEnumerable<long> Get10LargeLongs(long maxValue)
        {
            return Enumerable.Range(0, 10)
                .Select(_ => TestContext.CurrentContext.Random.NextLong(long.MaxValue / 10, maxValue));
        }

        private List<int[]> GetPermutationsForTest(int length)
        {
            var result = new List<int[]>(Factorial(length));
            var initialArray = Enumerable.Range(0, length).ToArray();
            GenerateIndexPermutations(initialArray, 0, result);
            return result;
        }

        private int Factorial(int n)
        {
            var result = 1;
            for (; n > 1; n--)
                result *= n;
            return result;
        }

        private void GenerateIndexPermutations(int[] currentPermutation, int changeIndex, List<int[]> result)
        {
            if (changeIndex == currentPermutation.Length)
            {
                result.Add(currentPermutation.ToArray());
                return;
            }
            
            var nextIndex = changeIndex + 1;
            for (var i = changeIndex; i < currentPermutation.Length; i++)
            {
                CommonHelper.Swap(ref currentPermutation[i], ref currentPermutation[changeIndex]);
                GenerateIndexPermutations(currentPermutation, nextIndex, result);
                CommonHelper.Swap(ref currentPermutation[i], ref currentPermutation[changeIndex]);
            }
        }
    }
}