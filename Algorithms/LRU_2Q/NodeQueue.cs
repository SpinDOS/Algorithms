using System;

namespace Algorithms.LRU_2Q
{
    internal class NodeQueue<TKey>
    {
        private Node<TKey> _head, _tail;

        public int Count { get; private set; } = 0;
        
        public void Enqueue(Node<TKey> node)
        {
            if (Count++ == 0)
            {
                _head = _tail = node;
                return;
            }
                
            node.Next = _head;
            _head.Prev = node;
            _head = node;
        }

        public Node<TKey> Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Queue is empty");;
            
            var result = _tail;
            
            if (--Count == 0)
            {
                _head = _tail = null;
                return result;
            }
            
            _tail = result.Prev;
            _tail.Next = null;
            result.Prev = null;
            
            return result;
        }

        public void Remove(Node<TKey> node)
        {
            Count--;
            
            var prev = node.Prev;
            var next = node.Next;

            if (prev != null)
                prev.Next = next;
            else
                _head = next;

            if (next != null)
                next.Prev = prev;
            else
                _tail = prev;

            node.Prev = node.Next = null;
        }

        public void MoveToHead(Node<TKey> node)
        {
            if (node == _head)
                return;
            
            var prev = node.Prev; // not null
            var next = node.Next;

            node.Prev = null;
            node.Next = _head;

            _head.Prev = node;
            _head = node;

            prev.Next = next;

            if (_tail == node)
                _tail = prev;
            else
                next.Prev = prev;
        }

        public void Clear()
        {
            _head = _tail = null;
            Count = 0;
        }
    }
}