using System;
using System.Collections.Generic;

namespace KormenAlgorithms.BinaryHeap
{
    public class PriorityQueue<T>
    {
        private BinaryHeap<T> _heap;

        public PriorityQueue(int size = 0, IComparer<T> comparer = null)
        {
            if (size <= 0)
                size = 100;

            _heap = new BinaryHeap<T>(new T[size], comparer) { HeapSize = 0 };
        }

        public int Count => _heap.HeapSize;

        public T GetMax()
        {
            CheckNotEmpty();
            return _heap.Items[0];
        }

        public T ExtractMax()
        {
            CheckNotEmpty();

            var result = _heap.Items[0];
            _heap.Items[0] = _heap.Items[--_heap.HeapSize];
            _heap.MaxHeapify(0);

            return result;
        }

        public void Insert(T item)
        {
            if (_heap.HeapSize == _heap.Items.Length)
            {
                var heap = new BinaryHeap<T>(new T[_heap.HeapSize * 2], _heap.Comparer) { HeapSize = _heap.HeapSize };
                Array.Copy(_heap.Items, heap.Items, _heap.Items.Length);
                _heap = heap;
            }

            _heap.Items[_heap.HeapSize] = item;
            IncreaseKey(_heap.HeapSize++, item);
        }

        private void IncreaseKey(int index, T value)
        {
            while (index > 0)
            {
                var parentIndex = (index - 1) / 2;
                var parent = _heap.Items[parentIndex];

                if (_heap.Comparer.Compare(parent, value) >= 0)
                    return;
                
                CommonHelper.Swap(ref _heap.Items[index], ref _heap.Items[parentIndex]);
                index = parentIndex;
            }
        }

        private void CheckNotEmpty()
        {
            if (_heap.HeapSize == 0)
                throw new ArgumentOutOfRangeException("Priority queue is empty");
        }
    }
}