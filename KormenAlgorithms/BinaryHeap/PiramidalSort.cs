using System.Collections.Generic;

namespace KormenAlgorithms.BinaryHeap
{
    public static class PiramidalSort
    {
        public static void Sort<T>(T[] arr, IComparer<T> comparer = null)
        {
            var heap = BinaryHeap<T>.BuildHeap(arr, comparer);

            while (heap.HeapSize > 1)
            {
                CommonHelper.Swap(ref heap.Items[0], ref heap.Items[--heap.HeapSize]);
                heap.MaxHeapify(0);
            }
        }
    }
}