using System.Collections.Generic;
using System.Linq;
using CpAlgorithms.Algebra.PrimeNumbers;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AlgorithmTests.CpAlgorithmsTests.Algebra.PrimeNumbers
{
    public class SequenceCycleDetectorTests
    {
        [Test]
        [Repeat(100)]
        public void SequenceCycleDetectorTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            var randomNumbers = Enumerable.Range(0, 10).Select(it => rnd.Next(9, 100));

            foreach (var start in new int[] { 0, 1, 2, 3, 5, 6, 7, 8, 15, 17, }.Concat(randomNumbers))
                foreach(var period in new int[] { 1, 2, 3, 6, 11, rnd.Next(4, 100), }.Concat(randomNumbers))
                    CheckSequenceCycleDetectors(start, period);
        }

        private void CheckSequenceCycleDetectors(int start, int period)
        {
            var arr = RandomizeArray(start + period);
            var sequence = GetInfiniteSequence(arr, start, period);
            var sequenceString = string.Join(", ", sequence.Take(start + 3 * period));

            var expected = (start, period);
            ClassicAssert.AreEqual(expected, SequenceCycleDetector.Floyd(sequence), $"Floyd ({start}, {period}): " + sequenceString);
            ClassicAssert.AreEqual(expected, SequenceCycleDetector.Brent(sequence), $"Brent ({start}, {period}): " + sequenceString);
        }

        private int[] RandomizeArray(int capacity)
        {
            var rnd = TestContext.CurrentContext.Random;

            var randomList = new List<int>(capacity);
            while (randomList.Count != randomList.Capacity)
            {
                var num = rnd.Next(randomList.Capacity * 5);
                if (!randomList.Contains(num))
                    randomList.Add(num);
            }

            return randomList.ToArray();
        }

        private IEnumerable<int> GetInfiniteSequence(int[] arr, int start, int period)
        {
            foreach (var index in Enumerable.Range(0, start))
                yield return arr[index];

            while (true)
            {
                foreach (var index in Enumerable.Range(0, period))
                    yield return arr[start + index];
            }
        }
    }
}
