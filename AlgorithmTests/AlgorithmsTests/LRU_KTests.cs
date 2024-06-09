using System;
using System.Threading;
using LRU_K;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.AlgorithmsTests
{
    public class LRU_KTests
    {
        [Test]
        public void CachedTest()
        {
            var cache = CreateCache();

            Sleep();

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));

            Sleep();

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
        }

        [Test]
        public void RemoveByTimeoutTest()
        {
            var cache = CreateCache();

            Sleep();

            ClassicAssert.AreEqual(2, cache.Get(2));
            ClassicAssert.AreEqual(3, cache.Get(3));

            Sleep();

            ClassicAssert.AreNotEqual(0, cache.Get(0));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void K2Test(bool stripDuplicate3)
        {
            var cache = CreateCache(); // Keys

            Sleep();
            ClassicAssert.AreEqual(2, cache.Get(2)); // 0, 1, 2
            Sleep();
            ClassicAssert.AreEqual(3, cache.Get(3)); // 1, 2, 3

            if (!stripDuplicate3)
                Sleep();

            ClassicAssert.AreEqual(3, cache.Get(3)); // 1, 2, 3

            Sleep();
            ClassicAssert.AreEqual(4, cache.Get(0)); // 2, 3, 0

            Sleep();
            ClassicAssert.AreEqual(5, cache.Get(4)); // 4, 3, 0

            Sleep();
            ClassicAssert.AreEqual(6, cache.Get(5)); // 5, 3, 0 or 3, 5, 0

            Sleep();
            ClassicAssert.AreEqual(7, cache.Get(6)); // 6, 3, 0 or 5, 6, 0

            Sleep();
            ClassicAssert.AreEqual(7, cache.Get(6));
            ClassicAssert.AreEqual(4, cache.Get(0));

            if (!stripDuplicate3)
            {
                ClassicAssert.AreEqual(3, cache.Get(3));
                ClassicAssert.AreNotEqual(6, cache.Get(5));
            }
            else
            {
                ClassicAssert.AreEqual(6, cache.Get(5));
                ClassicAssert.AreNotEqual(3, cache.Get(3));
            }
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void ImmutableDuringTimeoutTest(bool doTimeout)
        {
            var cache = CreateCache();
            ClassicAssert.AreEqual(2, cache.Get(2));

            Sleep();
            ClassicAssert.AreEqual(0, cache.Get(0));
            Sleep();
            ClassicAssert.AreEqual(1, cache.Get(1));
            Sleep();
            ClassicAssert.AreEqual(2, cache.Get(2));

            Sleep();
            ClassicAssert.AreEqual(3, cache.Get(3));

            if (doTimeout)
                Sleep();

            ClassicAssert.AreEqual(4, cache.Get(4));

            if (!doTimeout)
                ClassicAssert.AreEqual(3, cache.Get(3));
            else
                ClassicAssert.AreNotEqual(3, cache.Get(3));
        }

        [Test]
        public void ClearTest()
        {
            var cache = CreateCache();
            ClassicAssert.AreEqual(2, cache.Get(2));

            Sleep();
            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            Sleep();

            ClassicAssert.AreNotEqual(0, cache.Count);
            cache.Clear();
            ClassicAssert.AreEqual(0, cache.Count);

            Sleep();
            ClassicAssert.AreEqual(3, cache.Get(0));
            ClassicAssert.AreEqual(4, cache.Get(2));
            ClassicAssert.AreEqual(5, cache.Get(5));

            Sleep();
            ClassicAssert.AreEqual(3, cache.Get(0));
            ClassicAssert.AreEqual(4, cache.Get(2));
            ClassicAssert.AreEqual(5, cache.Get(5));
        }

        private LRU_K_Cache<int, int> CreateCache()
        {
            var local = 0;
            var cache = new LRU_K_Cache<int, int>(2, 3, i => local++)
            {
                CorrelationPeriod = TimeSpan.FromMilliseconds(200)
            };

            ClassicAssert.AreEqual(0, cache.Get(0));
            Sleep();
            ClassicAssert.AreEqual(1, cache.Get(1));
            return cache;
        }

        private static void Sleep() => Thread.Sleep(500);
    }
}
