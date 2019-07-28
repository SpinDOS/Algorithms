using System;
using System.Threading;
using LRU_K;
using NUnit.Framework;

namespace AlgorithmTests.AlgorithmsTests
{
    public class LRU_KTests
    {
        [Test]
        public void CachedTest()
        {
            var cache = CreateCache();
            
            Sleep();
            
            Assert.AreEqual(0, cache.Get(0));
            Assert.AreEqual(1, cache.Get(1));
            Assert.AreEqual(2, cache.Get(2));
            
            Sleep();

            Assert.AreEqual(0, cache.Get(0));
            Assert.AreEqual(1, cache.Get(1));
            Assert.AreEqual(2, cache.Get(2));
        }

        [Test]
        public void RemoveByTimeoutTest()
        {
            var cache = CreateCache();
            
            Sleep();
            
            Assert.AreEqual(2, cache.Get(2));
            Assert.AreEqual(3, cache.Get(3));
            
            Sleep();

            Assert.AreNotEqual(0, cache.Get(0));
        }
        
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void K2Test(bool stripDuplicate3)
        {
            var cache = CreateCache(); // Keys
            
            Sleep();
            Assert.AreEqual(2, cache.Get(2)); // 0, 1, 2
            Sleep();
            Assert.AreEqual(3, cache.Get(3)); // 1, 2, 3

            if (!stripDuplicate3)
                Sleep();

            Assert.AreEqual(3, cache.Get(3)); // 1, 2, 3
            
            Sleep();
            Assert.AreEqual(4, cache.Get(0)); // 2, 3, 0

            Sleep();
            Assert.AreEqual(5, cache.Get(4)); // 4, 3, 0

            Sleep();
            Assert.AreEqual(6, cache.Get(5)); // 5, 3, 0 or 3, 5, 0

            Sleep();
            Assert.AreEqual(7, cache.Get(6)); // 6, 3, 0 or 5, 6, 0
            
            Sleep();
            Assert.AreEqual(7, cache.Get(6));
            Assert.AreEqual(4, cache.Get(0));

            if (!stripDuplicate3)
            {
                Assert.AreEqual(3, cache.Get(3));
                Assert.AreNotEqual(6, cache.Get(5));
            }
            else
            {
                Assert.AreEqual(6, cache.Get(5));
                Assert.AreNotEqual(3, cache.Get(3));
            }
        }
        
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ImmutableDuringTimeoutTest(bool doTimeout)
        {
            var cache = CreateCache();
            Assert.AreEqual(2, cache.Get(2));

            Sleep();
            Assert.AreEqual(0, cache.Get(0));
            Sleep();
            Assert.AreEqual(1, cache.Get(1));
            Sleep();
            Assert.AreEqual(2, cache.Get(2));
            
            Sleep();
            Assert.AreEqual(3, cache.Get(3));

            if (doTimeout)
                Sleep();
            
            Assert.AreEqual(4, cache.Get(4));
            
            if (!doTimeout)
                Assert.AreEqual(3, cache.Get(3));
            else
                Assert.AreNotEqual(3, cache.Get(3));
        }
        
        [Test]
        public void ClearTest()
        {
            var cache = CreateCache();
            Assert.AreEqual(2, cache.Get(2));
            
            Sleep();
            Assert.AreEqual(0, cache.Get(0));
            Assert.AreEqual(1, cache.Get(1));
            Assert.AreEqual(2, cache.Get(2));
            Sleep();

            Assert.AreNotEqual(0, cache.Count);
            cache.Clear();
            Assert.AreEqual(0, cache.Count);
            
            Sleep();
            Assert.AreEqual(3, cache.Get(0));
            Assert.AreEqual(4, cache.Get(2));
            Assert.AreEqual(5, cache.Get(5));
            
            Sleep();
            Assert.AreEqual(3, cache.Get(0));
            Assert.AreEqual(4, cache.Get(2));
            Assert.AreEqual(5, cache.Get(5));
        }
        
        private LRU_K_Cache<int, int> CreateCache()
        {
            var local = 0;
            var cache = new LRU_K_Cache<int, int>(2, 3, i => local++)
            {
                CorrelationPeriod = TimeSpan.FromMilliseconds(200)
            };
            
            Assert.AreEqual(0, cache.Get(0));
            Sleep();
            Assert.AreEqual(1, cache.Get(1));
            return cache;
        }

        private static void Sleep() => Thread.Sleep(500);
    }
}