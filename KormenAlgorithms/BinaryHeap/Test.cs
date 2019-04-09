using System;

namespace KormenAlgorithms.BinaryHeap
{
    public static class Test
    {
        public static void TestPiramidalSort()
        {
            var rnd = CommonHelper.GlobalRandom;
            
            var arr = new int[rnd.Next(10, 300)];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = rnd.Next(arr.Length + 100);
            
            PiramidalSort.Sort(arr);
            
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                    throw new Exception($"Invalid sort (index {i}): " + Environment.NewLine + string.Join(" ", arr));
            }
        }

        public static void TestPriorityQueue()
        {
            var rnd = CommonHelper.GlobalRandom;
            
            var queue = new PriorityQueue<int>(5);
            
            var count = rnd.Next(10, 300);
            for (int i = 0; i < count; i++)
                queue.Insert(rnd.Next(count + 100));

            var prev = queue.GetMax();
            for (int i = 0; i < count; i++)
            {
                var cur = queue.ExtractMax();
                if (prev < cur)
                    throw new Exception($"Invalid priority queue: {cur} after {prev}");
                
                prev = cur;
            }
            
            if (queue.Count != 0)
                throw new Exception($"Invalid pririty queue count after all extracts: {queue.Count}");
        }
        
    }
}