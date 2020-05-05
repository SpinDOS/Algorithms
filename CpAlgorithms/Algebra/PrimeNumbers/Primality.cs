using System;
using CpAlgorithms.Algebra.Fundamentals;
using CpAlgorithms.Algebra.Fundamentals.BinaryExponentiation;

namespace CpAlgorithms.Algebra.PrimeNumbers
{
    public static class Primality
    {
        private static readonly uint[] MillerRabinBasesForInt = new uint[] { 2, 3, 5, 7 };
        
        private static readonly uint[] MillerRabinBasesForLong = new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };

        public static bool AreCoPrime(long x, long y) => EuclideanGcdAlgorithm.Gcd(x, y) == 1;

        public static bool IsPrimeSimple(ulong num)
        {
            // O (sqrt(n))
            
            if (num <= 1)
                return false;
            
            if ((num & 1) == 0)
                return num == 2;

            var sqrt = (ulong)Math.Ceiling(Math.Sqrt(num));

            for (var i = 3UL; i <= sqrt; i += 2)
                if (num % i == 0)
                    return false;

            return true;
        }

        // O (ln num)
        public static bool MillerRabin(int num) => MillerRabin(num, MillerRabinBasesForInt);
        
        // O (ln num)
        public static bool MillerRabin(long num) => MillerRabin(num, MillerRabinBasesForLong);

        private static bool MillerRabin(long num, uint[] basesToCheck)
        {
            // O (basesToCheck.Length * ln num)
            num = Math.Abs(num);

            if (num < 2)
                return false;

            var s = 0U;
            var d = (ulong)num - 1;
            while ((d & 1) == 0)
            {
                s++;
                d >>= 1;
            }
            
            // num = 2^s * d + 1

            foreach (var baseNum in basesToCheck)
            {
                if (baseNum == num)
                    return true;
                if (CheckComposite(num, baseNum, s, d))
                    return false;
            }

            return true;
        }

        private static bool CheckComposite(long num, uint baseNum, uint s, ulong d)
        {
            var minusOne = num - 1;
            
            var x = FastArithmetics.ModuloPow(baseNum, d, num);
            if (x == 1 || x == minusOne)
                return false;

            for (var i = 1U; i < s; i++)
            {
                x = (x * x) % num;
                if (x == minusOne)
                    return false;
            }

            return true;
        }
    }
}