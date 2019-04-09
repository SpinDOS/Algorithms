using System.Collections.Generic;

namespace KormenAlgorithms.BinaryHeap
{
    internal class BinaryHeap<T>
    {
        public T[] Items { get; }
        public IComparer<T> Comparer { get; }
        public int HeapSize { get; set; }

        public BinaryHeap(T[] arr, IComparer<T> comparer = null)
        {
            Items = arr;
            HeapSize = arr.Length;
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public static BinaryHeap<T> BuildHeap(T[] arr, IComparer<T> comparer = null)
        {
            var heap = new BinaryHeap<T>(arr, comparer);
            for (var i = (arr.Length - 1) / 2; i >= 0; i--)
                heap.MaxHeapify(i);
            return heap;
        }

        public void MaxHeapify(int index)
        {
            var largestIndex = index;

            for (var i = 1; i <= 2; i++)
            {
                var childIndex = 2 * index + i;
                if (childIndex >= HeapSize)
                    break;

                var largest = Items[largestIndex];
                var child = Items[childIndex];
                if (Comparer.Compare(largest, child) < 0)
                    largestIndex = childIndex;
            }

            if (largestIndex == index)
                return;

            CommonHelper.Swap(ref Items[index], ref Items[largestIndex]);
            MaxHeapify(largestIndex);
        }
    }
}