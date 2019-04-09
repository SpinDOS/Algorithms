using System;
using System.Collections.Generic;
using System.Linq;

namespace SkipList
{
    static class Test
    {
        public static void Run()
        {
            for (var i = 0; i < 1000; i++)
                RunInternal();
            CheckClear();
        }

        private static void RunInternal()
        {
            var r = new Random();
            var count = r.Next(1000, 2000);
            
            var dict = new Dictionary<int, int>();
            var skipList = new SkipList<int, int>();

            for (var i = 0; i < count; i++)
            {
                var x = r.Next(count * 2);
                if (dict.TryAdd(x, i) != skipList.Add(x, i))
                    throw new Exception("SkipList: Add error");
            }

            CheckTryGetValue();

            var keys = dict.Keys.ToArray();
            for (var i = 0; i < count; i++)
            {
                var key = keys[r.Next(keys.Length)];
                if (skipList.Remove(key, out var v1) != dict.Remove(key, out var v2) || v1 != v2)
                    throw new Exception("SkipList: Remove error");
            }

            CheckTryGetValue();

            void CheckTryGetValue()
            {
                for (var i = 0; i < count; i++)
                {
                    var key = r.Next(count * 2);
                    if (skipList.TryGetValue(key, out var v1) != dict.TryGetValue(key, out var v2) || v1 != v2)
                        throw new Exception("SkipList: TryGetValue error");
                }
                
            }
        }

        private static void CheckClear()
        {
            var skipList = new SkipList<int, int>();
            AddAndCheck(2);
            skipList.Clear();
            AddAndCheck(3);

            void AddAndCheck(int value)
            {
                Assert(skipList.MaxLevel == 0, "SkipList: MaxLevel Error");
                Assert(!skipList.TryGetValue(1, out _), "SkipList: TryGetValue error");
                skipList.Add(1, value);
                Assert(skipList.MaxLevel != 0, "SkipList: MaxLevel Error");
                Assert(skipList.TryGetValue(1, out var x) && x == value, "SkipList: TryGetValue error");
            }
        }

        private static void Assert(bool b, string message)
        {
            if (!b)
                throw new Exception(message);
        }
    }
}