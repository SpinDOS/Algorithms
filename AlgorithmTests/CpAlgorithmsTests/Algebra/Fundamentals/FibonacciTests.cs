using System;
using CpAlgorithms.Algebra.Fundamentals;
using NUnit.Framework;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.Fundamentals
{
    public class FibonacciTests
    {
        [Test]
        public void FibonacciByIndexTest()
        {
            ulong prev0 = 0, prev1 = 1;
            for (var n = 0U; prev1 >= prev0; n++)
            {
                Assert.AreEqual(prev0, FibonacciNumbers.SimpleFibonacci(n).fn, 
                    $"{n}'th Fibonacci number");
                Assert.AreEqual(prev0, FibonacciNumbers.FibonacciRecursive(n).fn, 
                    $"{n}'th Fibonacci number");
                // too long
                // Assert.AreEqual(prev0, FibonacciNumbers.FibonacciRecursiveBad(n), 
                //     $"{n}'th Fibonacci number");
                Assert.AreEqual(prev0, FibonacciNumbers.FibonacciByBinaryExponentiation(n).fn, 
                    $"{n}'th Fibonacci number");
                Assert.AreEqual(prev0, FibonacciNumbers.FibonacciByMatrixPow(n).fn, 
                    $"{n}'th Fibonacci number");
                
//              Assert.AreEqual(prev0, FibonacciNumbers.FibonacciByDouble(n), 
//                  $"{n}'th Fibonacci number") 
//              fails on 71: 308061521170130 instead of 308061521170129
                
                var next = unchecked(prev0 + prev1);
                prev0 = prev1;
                prev1 = next;
            }
        }
        
        [Test]
        public void FibonacciEncodeDecodeTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            var buffer = new bool[100];

            for (var i = 0; i < 10000; i++)
            {
                var num = Math.Max(rnd.NextULong(ulong.MaxValue / 2), 1);
                FibonacciNumbers.FibonacciEncode(num, buffer);
                Assert.AreEqual(num, FibonacciNumbers.FibonacciDecode(buffer));
            }
        }
    }
}