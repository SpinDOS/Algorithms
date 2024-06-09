using System.Collections.Generic;
using System.Linq;
using Algorithms;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.AlgorithmsTests
{
    public class SkipListTests
    {
        [Test]
        [Repeat(100)]
        public void AddRemoveTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            var count = rnd.Next(1000, 2000);

            var dict = new Dictionary<int, int>();
            var skipList = new SkipList<int, int>();

            for (var i = 0; i < count; i++)
            {
                var val = rnd.Next(count * 2);
                ClassicAssert.AreEqual(dict.TryAdd(val, i), skipList.Add(val, i), "SkipList.Add not equal to Dictionary.Add");
            }

            CheckTryGetValue(skipList, dict, count);

            var keys = dict.Keys.ToArray();
            for (var i = 0; i < count; i++)
            {
                var key = keys[rnd.Next(keys.Length)];

                ClassicAssert.AreEqual(
                    dict.Remove(key, out var expected),
                    skipList.Remove(key, out var actual),
                    "SkipList.Remove not equal to Dictionary.Remove");

                ClassicAssert.AreEqual(expected, actual, "SkipList.Remove returned wrong value");
            }

            CheckTryGetValue(skipList, dict, count);
        }

        [Test]
        public void ClearTest()
        {
            var skipList = new SkipList<int, int>();
            AddAndCheck(skipList, 2);
            skipList.Clear();
            AddAndCheck(skipList,3);
        }

        private void CheckTryGetValue(SkipList<int, int> skipList, Dictionary<int, int> dict, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var key = TestContext.CurrentContext.Random.Next(count * 2);

                ClassicAssert.AreEqual(
                    dict.TryGetValue(key, out var expected),
                    skipList.TryGetValue(key, out var actual),
                    "SkipList.TryGetValue not equal to Dictionary.TryGetValue");

                ClassicAssert.AreEqual(expected, actual, "SkipList.TryGetValue returned wrong value");
            }
        }

        private void AddAndCheck(SkipList<int, int> skipList, int value)
        {
            ClassicAssert.AreEqual(0, skipList.MaxLevel, "Empty SkipList's MaxLevel is not zero");
            ClassicAssert.False(skipList.TryGetValue(1, out _), "Empty SkipList returns some value");
            ClassicAssert.True(skipList.Add(1, value), "SkipList Add returned false");
            ClassicAssert.AreNotEqual(0, skipList.MaxLevel, "Nonempty SkipList MaxLevel is zero");
            ClassicAssert.True(skipList.TryGetValue(1, out var x), "Could not find added value");
            ClassicAssert.AreEqual(value, x, "SkipList returned wrong value");
        }
    }
}
