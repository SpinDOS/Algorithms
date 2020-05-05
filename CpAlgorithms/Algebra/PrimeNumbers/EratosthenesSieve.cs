using System;
using System.Collections.Generic;

namespace CpAlgorithms.Algebra.PrimeNumbers
{
    public static class EratosthenesSieve
    {
        public static IEnumerable<int> ClassicSieve(int n)
        {
            // O (n * ln (ln n))
            var isComposite = new bool[n + 1];

            var sqrt = (int) Math.Ceiling(Math.Sqrt(n));
            for (var i = 2; i <= sqrt; i++)
            {
                if (isComposite[i])
                    continue;
                
                for (var j = i * i; j < isComposite.Length; j += i)
                    isComposite[j] = true;
            }
            
            for (var i = 2; i < isComposite.Length; i++)
                if (!isComposite[i])
                    yield return i;
        }
        
        public static IEnumerable<int> OddOptimizedSieve(int n)
        {
            // O (n * ln (ln n))

            if (n >= 2)
                yield return 2;
            
            if (n <= 2)
                yield break;

            var isComposite = new bool[(n + 1) / 2];
            var sqrt = (int) Math.Ceiling(Math.Sqrt(n));
            
            for (var i = 1; i <= sqrt / 2; i++)
            {
                if (isComposite[i])
                    continue;

                var doubleI = 2 * i;
                var step = doubleI + 1;
                
                // ( ((2 * i + 1) ^ 2) - 1 ) / 2 = (4 * i^2 + 4 * i + 1 - 1) / 2 = 2 * i^2 + 2 * i
                var start = doubleI * i + doubleI;
                
                for (var j = start; j < isComposite.Length; j += step)
                    isComposite[j] = true;
            }

            for (var i = 1; i < isComposite.Length; i++)
                if (!isComposite[i])
                    yield return 2 * i + 1;
        }

        public static List<int> LinearSieve(int n, out int[] smallestPrimeFactors)
        {
            // O(n)
            // Each number can be easily factorized with smallestPrimeFactors
            
            smallestPrimeFactors = new int[n + 1];
            smallestPrimeFactors[0] = 1;
            if (smallestPrimeFactors.Length >= 2)
                smallestPrimeFactors[1] = 1;
            
            var primes = new List<int>();

            for (var i = 2; i < smallestPrimeFactors.Length; i++)
            {
                var smallestPrimeFactor = smallestPrimeFactors[i];
                if (smallestPrimeFactor == 0)
                {
                    smallestPrimeFactors[i] = smallestPrimeFactor = i;
                    primes.Add(i);
                }

                foreach (var prime in primes)
                {
                    var newComposite = prime * i;
                    if (prime > smallestPrimeFactor || newComposite >= smallestPrimeFactors.Length)
                        break;
                    
                    smallestPrimeFactors[newComposite] = smallestPrimeFactor;
                }
            }

            return primes;
        }
    }
}