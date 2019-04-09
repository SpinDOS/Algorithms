using System;
using System.Collections.Generic;
using System.Linq;

namespace LRU_K
{
    /// <summary>
    /// LRU-K algorithm implementation
    /// http://www.cs.cmu.edu/~christos/courses/721-resources/p297-o_neil.pdf
    /// </summary>
    /// <typeparam name="TKey">Cache key</typeparam>
    /// <typeparam name="TValue">Cached value</typeparam>
    public sealed class LRU_K_Cache<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _factory;
        private readonly TValue[] _buffer;

        private readonly Dictionary<TKey, LastAccessInfo> _lastAccessInfos = new Dictionary<TKey, LastAccessInfo>();
        private readonly List<LastAccessInfo> _priorityQueue = new List<LastAccessInfo>();
        
        private TimeSpan _correlationPeriod = TimeSpan.FromSeconds(1);
        private TimeSpan _retainedInformationPeriod = TimeSpan.FromSeconds(200);

        public int K { get; }
        public int Count { get; private set; }
        public int Capacity => _buffer.Length;
        public long Hits { get; private set; }

        public TimeSpan CorrelationPeriod
        {
            get => _correlationPeriod;
            set
            {
                CheckPeriods(value, RetainedInformationPeriod, value, "Correlation period");
                _correlationPeriod = value;
            }
        }

        public TimeSpan RetainedInformationPeriod
        {
            get => _retainedInformationPeriod;
            set
            {
                CheckPeriods(CorrelationPeriod, value, value, "Retained information period");
                _retainedInformationPeriod = value;
            }
        }

        public LRU_K_Cache(int k, int size, Func<TKey, TValue> factory)
        {
            if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k), "K must be a positive number");
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Cache size must be a positive number");
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            K = k;
            _buffer = new TValue[size];
            _factory = factory;
        }

        public TValue Get(TKey key)
        {
            var now = DateTime.Now;
            
            if (!_lastAccessInfos.TryGetValue(key, out var lastAccess))
            {
                lastAccess = CreateNewAccessInfo(now, key);
                
                if (Count < _buffer.Length)
                {
                    CacheNewValue(lastAccess, now);
                    return _buffer[lastAccess.BufferIndex] = _factory(key);;
                }
            }
            else if (lastAccess.QueueIndex < Count)
            {
                Hits++;
                return ExtractValueFromBuffer(lastAccess, now);
            }

            var result = _factory(key);
            var victim = FindVictim(now);
            ReplaceVictim(lastAccess, victim, now);
            return _buffer[lastAccess.BufferIndex] = result;
        }
        
        public void Clear()
        {
            Array.Clear(_buffer, 0, Count);
            _priorityQueue.Clear();
            _lastAccessInfos.Clear();
            Count = 0;
            Hits = 0;
        }

        private void CacheNewValue(LastAccessInfo lastAccess, DateTime now)
        {
            lastAccess.BufferIndex = Count++;
            lastAccess.Last = lastAccess.History[0] = now;
            ShiftUp(lastAccess.QueueIndex);
        }

        private TValue ExtractValueFromBuffer(LastAccessInfo lastAccess, DateTime now)
        {
            if (now - lastAccess.Last > CorrelationPeriod)
            {
                var history = lastAccess.History;
                var correlatedReferencePeriod = lastAccess.Last - history[0];
                
                for (var i = history.Length - 1; i > 0; i--)
                    history[i] = history[i - 1] + correlatedReferencePeriod;

                history[0] = now;
            }
    
            lastAccess.Last = now;
            
            ShiftDown(lastAccess.QueueIndex);
            return _buffer[lastAccess.BufferIndex];
        }

        private LastAccessInfo FindVictim(DateTime now) => FindVictim(now, 0) ?? _priorityQueue[0];
        
        private LastAccessInfo FindVictim(DateTime now, int itemIndex)
        {
            if (itemIndex >= Count)
                return null;
            
            var item = _priorityQueue[itemIndex];
            if (now - item.Last > CorrelationPeriod)
                return item;
            
            var left = FindVictim(now, itemIndex * 2 + 1);
            var right = FindVictim(now, itemIndex * 2 + 2);

            if (left == null)
                return right;
            if (right == null)
                return left;

            return left.CompareTo(right) <= 0 ? left : right;
        }

        private void ReplaceVictim(LastAccessInfo newValue, LastAccessInfo victim, DateTime now)
        {
            newValue.BufferIndex = victim.BufferIndex;
            Swap(newValue.QueueIndex, victim.QueueIndex);
            
            if (K > 1 && newValue.Last != default(DateTime))
                Array.Copy(newValue.History, 0, newValue.History, 1, K - 1);
    
            newValue.Last = newValue.History[0] = now;
            
            if (!ShiftUp(newValue.QueueIndex))
                ShiftDown(newValue.QueueIndex);
        }

        private LastAccessInfo CreateNewAccessInfo(DateTime now, TKey key)
        {
            var result = new LastAccessInfo(K);
            if (_lastAccessInfos.Count > Count && TryFindHistoryVictim(now, out var removedKey, out var queueIndex))
            {
                _lastAccessInfos.Remove(removedKey);
                result.QueueIndex = queueIndex;
                _priorityQueue[queueIndex] = result;
            }
            else
            {
                result.QueueIndex = _priorityQueue.Count;
                _priorityQueue.Add(result);
            }
            
            _lastAccessInfos.Add(key, result);
            return result;
        }

        private bool TryFindHistoryVictim(DateTime now, out TKey key, out int queueIndex)
        {
            var historyVictim = _lastAccessInfos.FirstOrDefault(kv =>
                kv.Value.QueueIndex >= Count && now - kv.Value.Last > RetainedInformationPeriod);

            key = historyVictim.Key;
            queueIndex = historyVictim.Value?.QueueIndex ?? 0;
            return historyVictim.Value != null;
        }

        #region PriorityQueue elements

        private void ShiftDown(int index)
        {
            var left = index * 2 + 1;
            var right = index * 2 + 2;

            var smallest = index;

            if (left < Count && _priorityQueue[left].CompareTo(_priorityQueue[smallest]) < 0)
                smallest = left;
    
            if (right < Count && _priorityQueue[right].CompareTo(_priorityQueue[smallest]) < 0)
                smallest = right;

            if (smallest == index)
                return;

            Swap(smallest, index);
            ShiftDown(smallest);
        }

        private bool ShiftUp(int index)
        {
            var current = index;
            while (current > 0)
            {
                var parent = (current - 1) / 2;

                if (_priorityQueue[parent].CompareTo(_priorityQueue[current]) <= 0)
                    break;

                Swap(parent, current);
                current = parent;
            }

            return index != current;
        }

        private void Swap(int a, int b)
        {
            var item1 = _priorityQueue[a];
            var item2 = _priorityQueue[b];

            _priorityQueue[a] = item2;
            item2.QueueIndex = a;
            
            _priorityQueue[b] = item1;
            item1.QueueIndex = b;
        }
        
        #endregion

        private static void CheckPeriods(TimeSpan correlationPeriod, TimeSpan retainedInformationPeriod, TimeSpan value, string periodName)
        {
            if (value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(value), periodName + " must be non negative TimeSpan");
            if (correlationPeriod.Multiply(2) > retainedInformationPeriod)
                throw new ArgumentOutOfRangeException(nameof(value), "Retained information period must be at least twice longer than correlation period");
        }
    }
}