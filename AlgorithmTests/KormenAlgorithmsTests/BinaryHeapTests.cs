using System.Collections.Generic;
using System.Linq;
using KormenAlgorithms.BinaryHeap;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.KormenAlgorithmsTests
{
    public class BinaryHeapTests
    {
        [Test]
        [Repeat(1000)]
        public void PiramidalSortTest()
        {
            var rnd = TestContext.CurrentContext.Random;

            var arr = new int[rnd.Next(10, 300)];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next(arr.Length + 100);

            var goodSort = arr.OrderBy(it => it).ToArray();
            PiramidalSort.Sort(arr);
            ClassicAssert.AreEqual(arr, goodSort, "Invalid piramidal sorting");
        }

        [Test]
        [Repeat(1000)]
        public void PriorityQueueTest()
        {
            var rnd = TestContext.CurrentContext.Random;

            var queue = new PriorityQueue<int>(5);
            var queueCopy = new List<int>();

            var count = rnd.Next(10, 300);
            for (int i = 0; i < count; i++)
            {
                var val = rnd.Next(count + 100);
                queueCopy.Add(val);
                queue.Insert(val);
            }

            ClassicAssert.AreEqual(count, queue.Count, "Count is wrong");

            var goodImpl = new Queue<int>(queueCopy.OrderByDescending(it => it));
            for (int i = 0; i < count; i++)
                ClassicAssert.AreEqual(goodImpl.Dequeue(), queue.ExtractMax(), "ExtractMax returned wrong value");

            ClassicAssert.AreEqual(0, queue.Count, "Priority queue is not empty after all extracts");
        }
    }
}
