namespace CpAlgorithms
{
    public static class EulerTotientFunction
    {
        public static long Phi(long n)
        {
            var result = n;

            var lastPrime = 0L;
            foreach (var prime in IntegerFactorization.SimpleFactorize(n))
            {
                if (prime == lastPrime)
                    continue;

                lastPrime = prime;
                result -= result / prime;
            }

            return result;
        }
    }
}