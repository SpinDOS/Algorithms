using System;

namespace CpAlgorithms.BinaryExponentiation
{
    public static class FastArithmetics
    {
        public static long Pow(long x, uint y)
        {
            // O(ln y)
            var result = 1L;
            while (y > 0)
            {
                if ((y & 1) != 0)
                    result *= x;

                x *= x;
                y >>= 1;
            }

            return result;
        }

        public static long ModuloPow(long x, uint y, long m)
        {
            // O(ln y)
            if (m == 1)
                return 0;
            
            var result = 1L;
            x %= m;
            
            while (y > 0)
            {
                if ((y & 1) != 0)
                    result = (result * x) % m;

                x = (x * x) % m;
                y >>= 1;
            }

            return result;
        }

        public static long ModuloBigMultiply(long x, long y, long m)
        {
            if (m > long.MaxValue / 2)
                throw new ArgumentOutOfRangeException(nameof(m), "Modulo is not safe to use");

            var result = 0L;
            x %= m;
            y %= m;
            
            while (y > 0)
            {
                if ((y & 1) != 0)
                    result = (result + x) % m;

                x = (x << 1) % m;
                y >>= 1;
            }

            return result;
        }
    }
}