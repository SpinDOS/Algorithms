using System;
using System.Diagnostics;

namespace LRU_K
{
    internal static class Test
    {
        public static void Run()
        {
            CheckCached();
            CheckRemoving();
            
            CheckK2(false);
            CheckK2(true);

            CheckImmutableDuringTimeout(false);
            CheckImmutableDuringTimeout(true);

            CheckClear();
        }

        private static decimal GetAverageTimeOfGet(int cacheSize)
        {
            var rnd = new Random();
            var cache = new LRU_K_Cache<int, int>(2, cacheSize, key => key);

            decimal sum = 0;
            var count = cacheSize * 100;
            for (var i = 0; i < count; i++)
            {
                var sw = Stopwatch.StartNew();
                cache.Get(rnd.Next());
                sum += sw.ElapsedTicks;
            }

            return sum / count;
        }

        private static void CheckCached()
        {
            var cache = TestHelper.CreateCache();
            
            TestHelper.Sleep();
            
            TestHelper.CheckEqual(0, cache.Get(0));
            TestHelper.CheckEqual(1, cache.Get(1));
            TestHelper.CheckEqual(2, cache.Get(2));
            
            TestHelper.Sleep();

            TestHelper.CheckEqual(0, cache.Get(0));
            TestHelper.CheckEqual(1, cache.Get(1));
            TestHelper.CheckEqual(2, cache.Get(2));
        }

        private static void CheckRemoving()
        {
            var cache = TestHelper.CreateCache();
            
            TestHelper.Sleep();
            
            TestHelper.CheckEqual(2, cache.Get(2));
            TestHelper.CheckEqual(3, cache.Get(3));
            
            TestHelper.Sleep();

            TestHelper.CheckNotEqual(0, cache.Get(0));
        }

        private static void CheckK2(bool stripDuplicate3)
        {
            var cache = TestHelper.CreateCache(); // Keys
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(2, cache.Get(2)); // 0, 1, 2
            TestHelper.Sleep();
            TestHelper.CheckEqual(3, cache.Get(3)); // 1, 2, 3

            if (!stripDuplicate3)
                TestHelper.Sleep();

            TestHelper.CheckEqual(3, cache.Get(3)); // 1, 2, 3
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(4, cache.Get(0)); // 2, 3, 0

            TestHelper.Sleep();
            TestHelper.CheckEqual(5, cache.Get(4)); // 4, 3, 0

            TestHelper.Sleep();
            TestHelper.CheckEqual(6, cache.Get(5)); // 5, 3, 0 or 3, 5, 0

            TestHelper.Sleep();
            TestHelper.CheckEqual(7, cache.Get(6)); // 6, 3, 0 or 5, 6, 0
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(7, cache.Get(6));
            TestHelper.CheckEqual(4, cache.Get(0));

            if (!stripDuplicate3)
            {
                TestHelper.CheckEqual(3, cache.Get(3));
                TestHelper.CheckNotEqual(6, cache.Get(5));
            }
            else
            {
                TestHelper.CheckEqual(6, cache.Get(5));
                TestHelper.CheckNotEqual(3, cache.Get(3));
            }
        }

        private static void CheckImmutableDuringTimeout(bool doTimeout)
        {
            var cache = TestHelper.CreateCache();
            TestHelper.CheckEqual(2, cache.Get(2));

            TestHelper.Sleep();
            TestHelper.CheckEqual(0, cache.Get(0));
            TestHelper.Sleep();
            TestHelper.CheckEqual(1, cache.Get(1));
            TestHelper.Sleep();
            TestHelper.CheckEqual(2, cache.Get(2));
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(3, cache.Get(3));

            if (doTimeout)
                TestHelper.Sleep();
            
            TestHelper.CheckEqual(4, cache.Get(4));
            
            if (!doTimeout)
                TestHelper.CheckEqual(3, cache.Get(3));
            else
                TestHelper.CheckNotEqual(3, cache.Get(3));
        }

        private static void CheckClear()
        {
            var cache = TestHelper.CreateCache();
            TestHelper.CheckEqual(2, cache.Get(2));
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(0, cache.Get(0));
            TestHelper.CheckEqual(1, cache.Get(1));
            TestHelper.CheckEqual(2, cache.Get(2));
            TestHelper.Sleep();

            TestHelper.CheckNotEqual(0, cache.Count);
            cache.Clear();
            TestHelper.CheckEqual(0, cache.Count);
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(3, cache.Get(0));
            TestHelper.CheckEqual(4, cache.Get(2));
            TestHelper.CheckEqual(5, cache.Get(5));
            
            TestHelper.Sleep();
            TestHelper.CheckEqual(3, cache.Get(0));
            TestHelper.CheckEqual(4, cache.Get(2));
            TestHelper.CheckEqual(5, cache.Get(5));
        }
    }
}