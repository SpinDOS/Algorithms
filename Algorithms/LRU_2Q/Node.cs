using System.Collections.Generic;

namespace Algorithms.LRU_2Q
{
    internal class Node<TKey>
    {
        public Node(TKey key) { Key = key; }

        public readonly TKey Key;
        public int Index;
        public Location Location;
        public Node<TKey> Prev, Next;
    }
    
    internal class NodeEqualityComparer<TKey> : IEqualityComparer<Node<TKey>>
    {
        private readonly IEqualityComparer<TKey> _keyComparer;
        
        public NodeEqualityComparer(IEqualityComparer<TKey> keyComparer = null)
        {
            _keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
        }

        public bool Equals(Node<TKey> x, Node<TKey> y) => _keyComparer.Equals(x.Key, y.Key);
        public int GetHashCode(Node<TKey> x) => _keyComparer.GetHashCode(x.Key);
    }

    internal enum Location : byte
    {
        A1In,
        A1Out,
        Am,
    }
}