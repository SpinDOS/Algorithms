using CpAlgorithms.Algebra.PrimeNumbers;

namespace CpAlgorithms.Algebra.NumberTheoreticFunctions
{
    public static class EulerTotientFunction
    {
        public static long Phi(long n)
        {
            // O (sqrt n)
            var result = n;
            
            foreach (var prime in IntegerFactorization.FactorizeToPowers(n).Keys)
                result -= result / prime;

            return result;
        }
    }
}