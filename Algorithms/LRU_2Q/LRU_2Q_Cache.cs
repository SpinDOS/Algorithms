using System;
using System.Collections.Generic;

namespace Algorithms.LRU_2Q
{
    /// <summary>
    /// LRU-2Q algorithm implementation
    /// http://www.vldb.org/conf/1994/P439.PDF
    /// </summary>
    /// <typeparam name="TKey">Cache key</typeparam>
    /// <typeparam name="TValue">Cached value</typeparam>
    public sealed class LRU_2Q_Cache<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _factory;

        private readonly TValue[] _buffer;
        private readonly HashSet<Node<TKey>> _hashSet;
        private readonly NodeQueue<TKey> _a1In, _a1Out, _am;
        private int _kIn, _kOut;
        
        public int KIn
        {
            get => _kIn;
            set
            {
                if (value <= 0) 
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(KIn)} must be a positive number");

                _kIn = value;
            }
        }

        public int KOut
        {
            get => _kOut;
            set
            {
                if (value <= 0) 
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(KOut)} must be a positive number");

                _kOut = value;
                while (_a1Out.Count > value)
                    _hashSet.Remove(_a1Out.Dequeue());
            }
        }

        public int Count => _am.Count + _a1In.Count;
        public int Capacity => _buffer.Length;
        public long Hits { get; private set; }

        public LRU_2Q_Cache(int size, Func<TKey, TValue> factory)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Cache size must be a positive number");
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _factory = factory;
            
            _buffer = new TValue[size];
            _hashSet = new HashSet<Node<TKey>>(new NodeEqualityComparer<TKey>());
            
            _a1In = new NodeQueue<TKey>();
            _a1Out = new NodeQueue<TKey>();
            _am = new NodeQueue<TKey>();

            _kIn = Math.Max(size / 4, 1);
            _kOut = Math.Max(size / 2, 2);
        }
        
        public TValue Get(TKey key)
        {
            var node = new Node<TKey>(key);
            if (!_hashSet.TryGetValue(node, out var existing))
                return PlaceToA1In(node);

            switch (existing.Location)
            {
                case Location.Am:
                    _am.MoveToHead(existing);
                    Hits++;
                    break;
                case Location.A1Out:
                    return MoveToAm(existing);
                case Location.A1In:
                    Hits++;
                    break;
                default:
                    throw new NotImplementedException($"Unexpected {nameof(Location)} enum value: " + existing.Location);
            }

            return _buffer[existing.Index];
        }

        public void Clear()
        {
            for (var i = 0; i < Count; i++)
                _buffer[i] = default(TValue);
            
            _a1In.Clear();
            _a1Out.Clear();
            _am.Clear();
            _hashSet.Clear();
            Hits = 0;
        }

        private int Reclaim()
        {
            if (Count < _buffer.Length)
                return Count;

            if (_a1In.Count <= _kIn) // CRP is not ended
            {
                var fromAm = _am.Dequeue();
                _hashSet.Remove(fromAm);
                return fromAm.Index;
            }

            var fromA1In = _a1In.Dequeue();
            fromA1In.Location = Location.A1Out;
            _a1Out.Enqueue(fromA1In);

            if (_a1Out.Count > _kOut)
                _hashSet.Remove(_a1Out.Dequeue());

            return fromA1In.Index;
        }

        private TValue PlaceToA1In(Node<TKey> node)
        {
            var result = _factory(node.Key);
            
            node.Index = Reclaim();
            node.Location = Location.A1In;
            
            _a1In.Enqueue(node);
            _hashSet.Add(node);
            
            return _buffer[node.Index] = result;
        }

        private TValue MoveToAm(Node<TKey> node)
        {
            var result = _factory(node.Key);

            _a1Out.Remove(node);
            
            node.Index = Reclaim();
            node.Location = Location.Am;
            
            _am.Enqueue(node);
            
            return _buffer[node.Index] = result;
        }
    }
}