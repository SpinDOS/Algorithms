using System;
using System.Collections.Generic;

namespace CpAlgorithms
{
    public static class IntegerFactorization
    {
        private static readonly long[] OptimizedFactors = new long[] {2, 3, 5};
        
        // prime numbers steps from 7 (first prime) to 37 (7 + lcm (2, 3, 5))
        // 7, 11, 13, 17, 19, 23, 29, 31, 37
        private static readonly long[] OtherFactorsSteps = new long[] {4, 2, 4, 2, 4, 6, 2, 6};
        
        public static List<long> SimpleFactorize(long num)
        {
            // O (sqrt n)
            num = Math.Abs(num);
            
            var result = new List<long>();
            for (var i = 2L; i * i <= num; i++)
                AddFactors(ref num, i, result);
            
            if (num > 1)
                result.Add(num);

            return result;
        }

        public static List<long> OptimizedFactorize(long num)
        {
            // O (sqrt n)
            num = Math.Abs(num);
            
            var result = new List<long>();
            foreach (var factor in OptimizedFactors)
                AddFactors(ref num, factor, result);
            
            var stepIndex = 0;
            for (var factor = 7L; factor * factor <= num; factor += OtherFactorsSteps[stepIndex++])
            {
                AddFactors(ref num, factor, result);
                if (stepIndex == OtherFactorsSteps.Length)
                    stepIndex = 0;
            }
            
            if (num > 1)
                result.Add(num);

            return result;
        }

        public static List<long> EratosthenesSieveFactorize(long num)
        {
            // complexity is same as EratosthenesSieve complexity: O (n * ln (ln n)) or O (n)
            num = Math.Abs(num);
            
            var result = new List<long>();
            var primes = EratosthenesSieve.ClassicSieve((int) Math.Ceiling(Math.Sqrt(num)));
            foreach (var factor in primes)
                AddFactors(ref num, factor, result);

            if (num > 1)
                result.Add(num);

            return result;
        }

        private static void AddFactors(ref long num, long factor, List<long> factors)
        {
            while (num % factor == 0)
            {
                factors.Add(factor);
                num /= factor;
            }
        }
    }
}