using System;

namespace LRU_K
{
    internal sealed class LastAccessInfo : IComparable<LastAccessInfo>
    {
        public LastAccessInfo (int k) => History = new DateTime[k];

        public int BufferIndex;
        public int QueueIndex;
        public DateTime Last;
        public readonly DateTime[] History;

        public int CompareTo(LastAccessInfo lastAccess)
        {
            for (var i = History.Length - 1; i >= 0; i--)
            {
                var compare = History[i].CompareTo(lastAccess.History[i]);
                if (compare != 0)
                    return compare;
            }
            
            return Last.CompareTo(lastAccess.Last);
        }
    }
}