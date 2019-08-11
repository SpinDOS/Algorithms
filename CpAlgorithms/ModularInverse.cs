using CpAlgorithms.BinaryExponentiation;

namespace CpAlgorithms
{
    // solve a * x === 1 mod m
    public static class ModularInverse
    {
        public static bool EuclideanModularInverse(long a, long m, out long x)
        {
            // O (ln (min(a, m))
            var gcd = EuclideanGcdAlgorithm.ExtendedGcd(a, m, out x, out _);
            // x can be negative
            x = (x % m + m) % m;
            return gcd == 1;
        }

        public static long BinaryExponentiationModularInverse(long a, long m)
        {
            // O (ln m)
            // m is prime!
            var phiM = (ulong) m - 1;
            return FastArithmetics.ModuloPow(a, phiM - 1, m);
        }

        public static long[] ModularInverseForRange(long m)
        {
            // O (m)
            // m is prime!

            var result = new long[m];
            result[1] = 1;
            
            for (var i = 2; i < m; i++)
            {
                var negative = -(m / i) * result[m % i];
                result[i] = (m + (negative % m)) % m;
            }

            return result;
        }
    }
}