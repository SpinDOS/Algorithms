using System;
using System.Threading;

namespace LRU_K
{
    internal static class TestHelper
    {
        public static LRU_K_Cache<int, int> CreateCache()
        {
            var local = 0;
            var cache = new LRU_K_Cache<int, int>(2, 3, i => local++)
            {
                CorrelationPeriod = TimeSpan.FromMilliseconds(200)
            };
            
            CheckEqual(0, cache.Get(0));
            Sleep();
            CheckEqual(1, cache.Get(1));

            return cache;
        }

        public static void CheckEqual(int a, int b)
        {
            if (a != b)
                throw new Exception($"Cache error, expected {a}, but was {b}");
        }
        
        public static void CheckNotEqual(int a, int b)
        {
            if (a == b)
                throw new Exception($"Cache error, expected not equal: {a}");
        }

        public static void Sleep() => Thread.Sleep(500);
    }
}