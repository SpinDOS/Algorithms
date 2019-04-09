using System;
using System.Collections;
using System.Collections.Generic;

namespace SkipList
{
    public sealed class SkipList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> 
        where TKey : IComparable<TKey>
    {
        private const int DefaultMaxPossibleLevel = 16;
        private static readonly Random Rnd = new Random();

        private Node _head = new Node(default(TKey), default(TValue), DefaultMaxPossibleLevel);
        private int _maxLevel = -1;
        private double _probability = 0.5;

        public double LevelProbability
        {
            get => _probability;
            set
            {
                if (value <= 0 || value >= 1) 
                    throw new ArgumentOutOfRangeException(nameof(value), "New level probability must be a number 0 to 1 non inclusive");
                _probability = value;
            }
        }

        public int MaxPossibleLevel
        {
            get => _head.NextNodes.Length;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Max possible level must be positive number");

                if (value == MaxPossibleLevel)
                    return;
                
                var newHead = new Node(default(TKey), default(TValue), value);
                Array.Copy(_head.NextNodes, newHead.NextNodes, Math.Min(value, MaxLevel));
                _head = newHead;
            }

        }

        public int MaxLevel => _maxLevel + 1;
        
        public int Count { get; private set; }

        public bool Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
                return false;
            
            AddInternal(key, value);
            return true;
        }

        public bool Remove(TKey key, out TValue value)
        {
            var current = _head;
            var level = _maxLevel;

            while (true)
            {
                current = FindAtLevel(key, current, level, out var found);
                if (found)
                    break;
                
                if (--level < 0)
                {
                    value = default(TValue);
                    return false;
                }
            }

            var target = current.NextNodes[level];
            for (; level >= 0; level--)
            {
                current = FindAtLevel(key, current, level, out _);
                current.NextNodes[level] = target.NextNodes[level];
            }

            while (_head.NextNodes[_maxLevel] == null && --_maxLevel >= 0) 
                { }

            value = target.Value;
            Count--;
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var node = Find(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            
            value = node.Value;
            return true;
        }

        public TValue this[TKey key]
        {
            get => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
            set
            {
                var found = Find(key);
                if (found == null)
                    AddInternal(key, value);
                else
                    found.Value = value;
            }
        }

        public bool ContainsKey(TKey key) => Find(key) != null;

        public void Clear()
        {
            Array.Fill(_head.NextNodes, null, 0, MaxLevel);
            _maxLevel = -1;
            Count = 0;
        }

        private void AddInternal(TKey key, TValue value)
        {
            var level = GetRandomLevel();
            _maxLevel = Math.Max(_maxLevel, level);

            var newNode = new Node(key, value, level + 1);

            var current = _head;
            for (; level >= 0; level--)
            {
                current = FindAtLevel(key, current, level, out _);
                newNode.NextNodes[level] = current.NextNodes[level];
                current.NextNodes[level] = newNode;
            }

            Count++;
        }

        private Node Find(TKey key)
        {
            var current = _head;
            for (var level = _maxLevel; level >= 0; level--)
            {
                current = FindAtLevel(key, current, level, out var found);
                if (found)
                    return current.NextNodes[level];
            }
            
            return null;
        }

        private int GetRandomLevel()
        {
            var level = 0;
            while (Rnd.NextDouble() < _probability)
                level++;

            return Math.Min(level, MaxPossibleLevel - 1);
        }

        private static Node FindAtLevel(TKey key, Node current, int level, out bool nextIsTarget)
        {
            while (true)
            {
                var next = current.NextNodes[level];
                
                var compareTo = -1;
                if (next != null)
                    compareTo = key.CompareTo(next.Key);
                
                if (compareTo <= 0)
                {
                    nextIsTarget = compareTo == 0;
                    return current;
                }
                
                current = next;
            }
        }
        
        private class Node
        {
            public readonly Node[] NextNodes;
            public readonly TKey Key;
            public TValue Value;

            public Node(TKey key, TValue value, int nodesCount)
            {
                Key = key;
                Value = value;
                NextNodes = new Node[nodesCount];
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new SkipListEnumerator(_head);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private class SkipListEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private Node _current;
            public SkipListEnumerator(Node current) { _current = current; }
            public bool MoveNext() { return (_current = _current.NextNodes[0]) != null; }
            public KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
            object IEnumerator.Current => Current;
            public void Dispose() { _current = null; }
            public void Reset() => throw new NotSupportedException();
        }

    }
}