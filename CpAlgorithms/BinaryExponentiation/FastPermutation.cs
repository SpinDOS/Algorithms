using System;

namespace CpAlgorithms.BinaryExponentiation
{
    public static class FastPermutation
    {
        public static void ApplyPermutation<T>(T[] arr, int[] permutation, uint k)
        {
            // O(arr.Length * ln k)
            // Apply permutation in-place
            // Changes arr and permutation

            ValidateArguments(arr, permutation);
            while (k > 0)
            {
                if ((k & 1) != 0)
                    ApplyPermutationOnce(arr, permutation);
                
                ApplyPermutationOnce(permutation, permutation);
                k >>= 1;
            }
        }

        private static void ApplyPermutationOnce<T>(T[] arr, int[] permutation)
        {
            var index = 0;
            while (index < permutation.Length)
            {
                var loopStartIndex = index;
                var loopStartValue = arr[index];

                while (true)
                {
                    var nextIndex = permutation[index];
                    if (nextIndex == loopStartIndex)
                        break;
                    
                    arr[index] = arr[nextIndex];
                    ReplaceWithNegative(ref permutation[index]);
                    index = nextIndex;
                }
                
                arr[index] = loopStartValue;
                ReplaceWithNegative(ref permutation[index]);
                index = loopStartIndex + 1;

                while (index < permutation.Length && permutation[index] < 0)
                    ++index;
            }

            for (var i = 0; i < permutation.Length; i++)
                ReplaceWithNegative(ref permutation[i]);
        }

        private static void ReplaceWithNegative(ref int value) => value = -value - 1;

        private static void ValidateArguments<T>(T[] arr, int[] permutation)
        {
            arr = arr ?? throw new ArgumentNullException(nameof(arr));
            permutation = permutation ?? throw new ArgumentNullException(nameof(permutation));
            if (permutation.Length != arr.Length)
                throw new ArgumentException("permutation.Length must be equal to arr.Length");
        }
    }
}