using System.Linq;
using CpAlgorithms.BinaryExponentiation;

namespace CpAlgorithms
{
    public static class DivisorsCounter
    {
        public static ulong NumberOfDivisors(long num)
        {
            // O (sqrt num)
            return IntegerFactorization.FactorizeToPowers(num).Values
                .Aggregate(1UL, (acc, cur) => acc * (cur + 1));
        }

        public static ulong SumOfDivisors(long num)
        {
            // O (sqrt num)
            return IntegerFactorization.FactorizeToPowers(num)
                .Aggregate(1UL, (acc, cur) => acc * SumOfSingleFactor(cur.Key, cur.Value));
        }

        private static ulong SumOfSingleFactor(long factor, uint power)
        {
            var result = (FastArithmetics.Pow(factor, power + 1) - 1) / 
                         (factor - 1);
            
            return (ulong) result;
        }
        
    }
}