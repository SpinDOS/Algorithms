namespace CpAlgorithms.Algebra.Fundamentals
{
    public static class EuclideanGcdAlgorithm
    {
        public static long Gcd(long a, long b)
        {
            // O (ln (min(x, y))
            while (b != 0)
            {
                var mod = a % b;
                a = b;
                b = mod;
            }

            return a;
        }

        public static long Lcm(long a, long b)
        {
            // O (ln (min(x, y))
            return (a / Gcd(a, b)) * b;
        }

        public static long ExtendedGcd(long a, long b, out long x, out long y)
        {
            // O (ln (min(x, y))
            // gcd(x, y) = a * x + b * y

            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            var gcd = ExtendedGcd(b, a % b, out var x1, out var y1);
            x = y1;
            y = x1 - y1 * (a / b);
            return gcd;
        }
    }
}