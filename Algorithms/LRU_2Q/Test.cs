using System;
using System.Diagnostics;

namespace Algorithms.LRU_2Q
{
    public static class Test
    {
        public static void Run()
        {
            CheckCachedAndRemoved();

            CheckReclaimFromA1In(true);
            CheckReclaimFromA1In(false);
            
            CheckReclaimFromAm(true);
            CheckReclaimFromAm(false);
            
            CheckA1InUnupdatable(true);
            CheckA1InUnupdatable(false);
                        
            CheckAmUpdatable(true);
            CheckAmUpdatable(false);

            CheckA1OutOverflow(true);
            CheckA1OutOverflow(false);

            CheckClear();
            
            CheckTimeIsOConst();
        }

        private static void CheckCachedAndRemoved()
        {
            var cache = CreateCache(2);
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));

            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            
            CheckEqual(2, cache.Get(2));
            CheckNotEqual(0, cache.Get(0));
        }

        private static void CheckReclaimFromA1In(bool doReclaim)
        {
            var cache = CreateCache(3);
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            CheckEqual(3, cache.Get(3));
            // A1In: 3, 2, 1; A1Out: 0
            
            // now reclaim from A1In
            CheckEqual(4, cache.Get(0));
            // A1In: 3, 2; A1Out: 1; Am: 0

            // now reclaim from A1In
            CheckEqual(5, cache.Get(4));
            // A1In: 4, 3; A1Out: 2; Am: 0
            
            CheckEqual(4, cache.Get(0));
            
            if (doReclaim)
                CheckEqual(6, cache.Get(2));
            // A1In: 4; A1Out: 3; Am: 2, 0
            // or
            // A1In: 4, 3; A1Out: 2; Am: 0

            CheckEqual(4, cache.Get(0));
            CheckEqual(5, cache.Get(4));
            
            if (doReclaim)
                CheckNotEqual(3, cache.Get(3));
            else
                CheckEqual(3, cache.Get(3));
        }

        private static void CheckReclaimFromAm(bool doReclaim)
        {
            var cache = CreateCache(2);
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            // A1In: 2, 1; A1Out: 0
            
            CheckEqual(3, cache.Get(0));
            // A1In: 2; A1Out: 1; Am: 0

            if (doReclaim)
                CheckEqual(4, cache.Get(3));
            // A1In: 3, 2; A1Out: 1;
            // or
            // A1In: 2; A1Out: 1; Am: 0
            
            CheckEqual(2, cache.Get(2));
            if (doReclaim)
                CheckNotEqual(3, cache.Get(0));
            else
                CheckEqual(3, cache.Get(0));
        }

        private static void CheckA1InUnupdatable(bool doUselessRefresh)
        {
            var cache = CreateCache(3);
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            
            if (doUselessRefresh)
                CheckEqual(0, cache.Get(0));
            // always A1In: 2, 1, 0
            
            CheckEqual(3, cache.Get(3));
            // A1In: 3, 2, 1; A1Out: 0
            CheckNotEqual(0, cache.Get(0));
        }
        
        private static void CheckAmUpdatable(bool refreshKey1)
        {
            var cache = CreateCache(4);
            cache.KOut = 10;
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            CheckEqual(3, cache.Get(3));
            
            CheckEqual(4, cache.Get(4));
            CheckEqual(5, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 1, 0
            
            CheckEqual(6, cache.Get(1));
            CheckEqual(7, cache.Get(0));
            // A1In: 5, 4; A1Out: 3, 2; Am: 0, 1
            CheckEqual(8, cache.Get(2));
            // A1In: 5; A1Out: 4, 3; Am: 2, 0, 1

            if (refreshKey1)
                CheckEqual(6, cache.Get(1));
            // A1In: 5; A1Out: 4, 3; Am: 1, 2, 0
            // or
            // A1In: 5; A1Out: 4, 3; Am: 2, 0, 1

            CheckEqual(9, cache.Get(6));
            // A1In: 6, 5; A1Out: 4, 3; Am: 1, 2
            // or
            // A1In: 6, 5; A1Out: 4, 3; Am: 2, 0

            if (refreshKey1)
            {
                CheckEqual(6, cache.Get(1));
                CheckNotEqual(7, cache.Get(0));
            }
            else
            {
                CheckEqual(7, cache.Get(0));
                CheckNotEqual(6, cache.Get(1));
            }
        }

        private static void CheckA1OutOverflow(bool doOverflow)
        {
            var local = 0;
            var cache = new LRU_2Q_Cache<int, int>(3, key => key == 5 ? 100: local++);
            cache.KOut = 2;
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            // A1In: 2, 1, 0
           
            if (doOverflow)
                CheckEqual(100, cache.Get(5));
            
            CheckEqual(3, cache.Get(3));
            CheckEqual(4, cache.Get(4));
            // A1In: 4, 3, 5; A1Out: 2, 1
            // or
            // A1In: 4, 3, 2; A1Out: 1, 0

            // place 0 to A1In or Am
            CheckEqual(5, cache.Get(0));
            // A1In: 0, 4, 3; A1Out: 5, 2
            // or
            // A1In: 4, 3; A1Out: 2, 1; Am: 0

            CheckEqual(6, cache.Get(2));
            // A1In: 0, 4; A1Out: 3, 5; Am: 2
            // or
            // A1In: 4; A1Out: 3, 1; Am: 2, 0
            
            CheckEqual(7, cache.Get(6));
            // A1In: 6, 0; A1Out: 4, 3; Am: 2
            // or
            // A1In: 6, 4; A1Out: 3, 1; Am: 2

            if (doOverflow)
            {
                CheckEqual(5, cache.Get(0));
                CheckNotEqual(4, cache.Get(4));
            }
            else
            {
                CheckEqual(4, cache.Get(4));
                CheckNotEqual(5, cache.Get(0));
            }
        }

        private static void CheckClear()
        {
            var cache = CreateCache(4);
            cache.KOut = 10;
            
            CheckEqual(0, cache.Get(0));
            CheckEqual(1, cache.Get(1));
            CheckEqual(2, cache.Get(2));
            CheckEqual(3, cache.Get(3));
            CheckEqual(4, cache.Get(4));
            CheckEqual(5, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 1, 0

            CheckEqual(6, cache.Get(0));
            // A1In: 5, 4, 3; A1Out: 2, 1; Am: 0
            CheckEqual(7, cache.Get(2));
            // A1In: 5, 4; A1Out: 3, 1; Am: 2, 0

            cache.Clear();
            
            // get values from all potential queues (they must be empty)
            CheckEqual(8, cache.Get(0));
            CheckEqual(9, cache.Get(2));
            CheckEqual(10, cache.Get(3));
            CheckEqual(11, cache.Get(4));
            // A1In: 4, 3, 2, 0
            
            CheckEqual(8, cache.Get(0)); // not updated
            CheckEqual(12, cache.Get(5));
            // A1In: 5, 4, 3, 2; A1Out: 0
            CheckNotEqual(8, cache.Get(0));
        }

        private static void CheckTimeIsOConst()
        {
            var avgSmall = GetAverageTimeOfGet(100);
            var avgBig = GetAverageTimeOfGet(100000);
            if (Math.Abs(avgSmall - avgBig) > 5)
                throw new Exception("Time of Get() is not constant and depends on size of cache");
        }

        private static decimal GetAverageTimeOfGet(int cacheSize)
        {
            var rnd = new Random();
            var cache = new LRU_2Q_Cache<int, int>(cacheSize, key => key);

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

        private static LRU_2Q_Cache<int, int> CreateCache(int size)
        {
            var local = 0;
            return new LRU_2Q_Cache<int, int>(size, key => local++);
        }

        private static void CheckEqual(int a, int b)
        {
            if (a != b)
                throw new Exception($"Cache error, expected {a}, but was {b}");
        }
        
        private static void CheckNotEqual(int a, int b)
        {
            if (a == b)
                throw new Exception($"Cache error, expected not equal: {a}");
        }
    }
}