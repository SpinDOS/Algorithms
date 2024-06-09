using System;
using System.Diagnostics;
using Algorithms.LRU_2Q;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.AlgorithmsTests
{
    public class LRU_2QTests
    {
        [Test]
        public void CachedAndRemovedTest()
        {
            var cache = CreateCache(2);

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));

            ClassicAssert.AreEqual(2, cache.Get(2));
            ClassicAssert.AreNotEqual(0, cache.Get(0));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ReclaimFromA1InTest(bool doReclaim)
        {
            var cache = CreateCache(3);

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            ClassicAssert.AreEqual(3, cache.Get(3));
            // A1In: 3, 2, 1; A1Out: 0

            // now reclaim from A1In
            ClassicAssert.AreEqual(4, cache.Get(0));
            // A1In: 3, 2; A1Out: 1; Am: 0

            // now reclaim from A1In
            ClassicAssert.AreEqual(5, cache.Get(4));
            // A1In: 4, 3; A1Out: 2; Am: 0

            ClassicAssert.AreEqual(4, cache.Get(0));

            if (doReclaim)
                ClassicAssert.AreEqual(6, cache.Get(2));
            // A1In: 4; A1Out: 3; Am: 2, 0
            // or
            // A1In: 4, 3; A1Out: 2; Am: 0

            ClassicAssert.AreEqual(4, cache.Get(0));
            ClassicAssert.AreEqual(5, cache.Get(4));

            if (doReclaim)
                ClassicAssert.AreNotEqual(3, cache.Get(3));
            else
                ClassicAssert.AreEqual(3, cache.Get(3));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ReclaimFromAmTest(bool doReclaim)
        {
            var cache = CreateCache(2);

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            // A1In: 2, 1; A1Out: 0

            ClassicAssert.AreEqual(3, cache.Get(0));
            // A1In: 2; A1Out: 1; Am: 0

            if (doReclaim)
                ClassicAssert.AreEqual(4, cache.Get(3));
            // A1In: 3, 2; A1Out: 1;
            // or
            // A1In: 2; A1Out: 1; Am: 0

            ClassicAssert.AreEqual(2, cache.Get(2));
            if (doReclaim)
                ClassicAssert.AreNotEqual(3, cache.Get(0));
            else
                ClassicAssert.AreEqual(3, cache.Get(0));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void A1InUnupdatableTest(bool doUselessRefresh)
        {
            var cache = CreateCache(3);

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));

            if (doUselessRefresh)
                ClassicAssert.AreEqual(0, cache.Get(0));
            // always A1In: 2, 1, 0

            ClassicAssert.AreEqual(3, cache.Get(3));
            // A1In: 3, 2, 1; A1Out: 0
            ClassicAssert.AreNotEqual(0, cache.Get(0));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AmUpdatableTest(bool refreshKey1)
        {
            var cache = CreateCache(4);
            cache.KOut = 10;

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            ClassicAssert.AreEqual(3, cache.Get(3));

            ClassicAssert.AreEqual(4, cache.Get(4));
            ClassicAssert.AreEqual(5, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 1, 0

            ClassicAssert.AreEqual(6, cache.Get(1));
            ClassicAssert.AreEqual(7, cache.Get(0));
            // A1In: 5, 4; A1Out: 3, 2; Am: 0, 1
            ClassicAssert.AreEqual(8, cache.Get(2));
            // A1In: 5; A1Out: 4, 3; Am: 2, 0, 1

            if (refreshKey1)
                ClassicAssert.AreEqual(6, cache.Get(1));
            // A1In: 5; A1Out: 4, 3; Am: 1, 2, 0
            // or
            // A1In: 5; A1Out: 4, 3; Am: 2, 0, 1

            ClassicAssert.AreEqual(9, cache.Get(6));
            // A1In: 6, 5; A1Out: 4, 3; Am: 1, 2
            // or
            // A1In: 6, 5; A1Out: 4, 3; Am: 2, 0

            if (refreshKey1)
            {
                ClassicAssert.AreEqual(6, cache.Get(1));
                ClassicAssert.AreNotEqual(7, cache.Get(0));
            }
            else
            {
                ClassicAssert.AreEqual(7, cache.Get(0));
                ClassicAssert.AreNotEqual(6, cache.Get(1));
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void A1OutOverflowTest(bool doOverflow)
        {
            var local = 0;
            var cache = new LRU_2Q_Cache<int, int>(3, key => key == 5 ? 100: local++);
            cache.KOut = 2;

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            // A1In: 2, 1, 0

            if (doOverflow)
                ClassicAssert.AreEqual(100, cache.Get(5));

            ClassicAssert.AreEqual(3, cache.Get(3));
            ClassicAssert.AreEqual(4, cache.Get(4));
            // A1In: 4, 3, 5; A1Out: 2, 1
            // or
            // A1In: 4, 3, 2; A1Out: 1, 0

            // place 0 to A1In or Am
            ClassicAssert.AreEqual(5, cache.Get(0));
            // A1In: 0, 4, 3; A1Out: 5, 2
            // or
            // A1In: 4, 3; A1Out: 2, 1; Am: 0

            ClassicAssert.AreEqual(6, cache.Get(2));
            // A1In: 0, 4; A1Out: 3, 5; Am: 2
            // or
            // A1In: 4; A1Out: 3, 1; Am: 2, 0

            ClassicAssert.AreEqual(7, cache.Get(6));
            // A1In: 6, 0; A1Out: 4, 3; Am: 2
            // or
            // A1In: 6, 4; A1Out: 3, 1; Am: 2

            if (doOverflow)
            {
                ClassicAssert.AreEqual(5, cache.Get(0));
                ClassicAssert.AreNotEqual(4, cache.Get(4));
            }
            else
            {
                ClassicAssert.AreEqual(4, cache.Get(4));
                ClassicAssert.AreNotEqual(5, cache.Get(0));
            }
        }

        [Test]
        public void CheckClear()
        {
            var cache = CreateCache(4);
            cache.KOut = 10;

            ClassicAssert.AreEqual(0, cache.Get(0));
            ClassicAssert.AreEqual(1, cache.Get(1));
            ClassicAssert.AreEqual(2, cache.Get(2));
            ClassicAssert.AreEqual(3, cache.Get(3));
            ClassicAssert.AreEqual(4, cache.Get(4));
            ClassicAssert.AreEqual(5, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 1, 0

            ClassicAssert.AreEqual(6, cache.Get(0));
            // A1In: 5, 4, 3; A1Out: 2, 1; Am: 0
            ClassicAssert.AreEqual(7, cache.Get(2));
            // A1In: 5, 4; A1Out: 3, 1; Am: 2, 0

            cache.Clear();

            // get values from all potential queues (they must be empty)
            ClassicAssert.AreEqual(8, cache.Get(0));
            ClassicAssert.AreEqual(9, cache.Get(2));
            ClassicAssert.AreEqual(10, cache.Get(3));
            ClassicAssert.AreEqual(11, cache.Get(4));
            // A1In: 4, 3, 2, 0

            ClassicAssert.AreEqual(8, cache.Get(0)); // not updated
            ClassicAssert.AreEqual(12, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 0
            ClassicAssert.AreNotEqual(8, cache.Get(0));
        }

        [Test]
        [Ignore("Test fails. Should be fixed somehow later")]
        public void CheckTimeIsOConst()
        {
            // naive implementation
            var avgSmall = GetAverageTimeOfGet(100);
            var avgBig = GetAverageTimeOfGet(100000);
            ClassicAssert.LessOrEqual(Math.Abs(avgSmall - avgBig), 5m, "Time of Get() is not constant and depends on size of cache");
        }

        private LRU_2Q_Cache<int, int> CreateCache(int size)
        {
            var local = 0;
            return new LRU_2Q_Cache<int, int>(size, key => local++);
        }

        private decimal GetAverageTimeOfGet(int cacheSize)
        {
            var rnd = TestContext.CurrentContext.Random;
            var cache = new LRU_2Q_Cache<int, int>(cacheSize, key => key);

            decimal sum = 0;
            var count = cacheSize * 100;
            for (var i = 0; i < count; i++)
            {
                var key = rnd.Next();
                var sw = Stopwatch.StartNew();
                cache.Get(key);
                sum += sw.ElapsedTicks;
            }

            return sum / count;
        }
    }
}
