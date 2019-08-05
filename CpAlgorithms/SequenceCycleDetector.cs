using System;
using System.Collections.Generic;

namespace CpAlgorithms
{
    public static class SequenceCycleDetector
    {
        public static (int cycleStart, int period) Floyd(IEnumerable<int> sequence) =>
            ExecuteInTryFinally(sequence, FloydInternal);

        public static (int cycleStart, int period) Brent(IEnumerable<int> sequence) =>
            ExecuteInTryFinally(sequence, BrentInternal);
        
        private static (int cycleStart, int period) FloydInternal(IEnumerator<int> hare, IEnumerator<int> tortoise, IEnumerator<int> tortoise2)
        {
            do
            {
                tortoise.MoveNext();
                hare.MoveNext();
                hare.MoveNext();
            }
            while (tortoise.Current != hare.Current);

            var cycleStart = 0;
            while (tortoise.Current != tortoise2.Current)
            {
                cycleStart++;
                tortoise.MoveNext();
                tortoise2.MoveNext();
            }

            var period = 0;
            do
            {
                period++;
                tortoise2.MoveNext();
            }
            while (tortoise.Current != tortoise2.Current);

            return (cycleStart, period);
        }
        
        private static (int cycleStart, int period) BrentInternal(IEnumerator<int> hare, IEnumerator<int> tortoise, IEnumerator<int> tortoise2)
        {
            int period, tortoiseValue = hare.Current;
            hare.MoveNext();
            
            for (var powerOf2 = 1; true; powerOf2 <<= 1)
            {
                for (period = 1; period != powerOf2 && hare.Current != tortoiseValue; period++)
                    hare.MoveNext();

                if (hare.Current == tortoiseValue)
                    break;

                tortoiseValue = hare.Current;
                hare.MoveNext();
            }

            for (var i = 0; i < period; i++)
                tortoise.MoveNext();
            
            var cycleStart = 0;
            while (tortoise.Current != tortoise2.Current)
            {
                cycleStart++;
                tortoise.MoveNext();
                tortoise2.MoveNext();
            }

            return (cycleStart, period);
        }

        private static (int cycleStart, int period) ExecuteInTryFinally(
            IEnumerable<int> sequence,
            Func<IEnumerator<int>, IEnumerator<int>, IEnumerator<int>, (int cycleStart, int period)> solver)
        {
            IEnumerator<int> hare = null, tortoise = null, tortoise2 = null;
            try
            {
                hare = sequence.GetEnumerator();
                hare.MoveNext();
                tortoise = sequence.GetEnumerator();
                tortoise.MoveNext();
                tortoise2 = sequence.GetEnumerator();
                tortoise2.MoveNext();

                return solver(hare, tortoise, tortoise2);
            }
            finally
            {
                hare?.Dispose();
                tortoise?.Dispose();
                tortoise2?.Dispose();
            }
        }
    }
}