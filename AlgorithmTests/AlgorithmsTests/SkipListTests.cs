using System.Collections.Generic;
using System.Linq;
using Algorithms;
using NUnit.Framework;

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
                Assert.AreEqual(dict.TryAdd(val, i), skipList.Add(val, i), "SkipList.Add not equal to Dictionary.Add");
            }

            CheckTryGetValue(skipList, dict, count);

            var keys = dict.Keys.ToArray();
            for (var i = 0; i < count; i++)
            {
                var key = keys[rnd.Next(keys.Length)];
                
                Assert.AreEqual(
                    dict.Remove(key, out var expected), 
                    skipList.Remove(key, out var actual), 
                    "SkipList.Remove not equal to Dictionary.Remove");
                
                Assert.AreEqual(expected, actual, "SkipList.Remove returned wrong value");
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
                
                Assert.AreEqual(
                    dict.TryGetValue(key, out var expected), 
                    skipList.TryGetValue(key, out var actual), 
                    "SkipList.TryGetValue not equal to Dictionary.TryGetValue");
                
                Assert.AreEqual(expected, actual, "SkipList.TryGetValue returned wrong value");
            }
        }

        private void AddAndCheck(SkipList<int, int> skipList, int value)
        {
            Assert.AreEqual(0, skipList.MaxLevel, "Empty SkipList's MaxLevel is not zero");
            Assert.False(skipList.TryGetValue(1, out _), "Empty SkipList returns some value");
            Assert.True(skipList.Add(1, value), "SkipList Add returned false");
            Assert.AreNotEqual(0, skipList.MaxLevel, "Nonempty SkipList MaxLevel is zero");
            Assert.True(skipList.TryGetValue(1, out var x), "Could not find added value");
            Assert.AreEqual(value, x, "SkipList returned wrong value");
        }
    }
}