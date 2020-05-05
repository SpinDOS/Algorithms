using System;
using System.Linq;
using CpAlgorithms.Algebra.Fundamentals.BinaryExponentiation;
using CpAlgorithms.Algebra.PrimeNumbers;

namespace CpAlgorithms.Algebra.ModularArithmetic
{
    public static class PrimitiveRoot
    {
        public static uint CalculatePrimitiveRootForPrimeModulo(uint m)
        {
            if (m == 0)
                return 0;
            
            var phi = m - 1;
            
            if (m < 4)
                return phi;

            var factors = IntegerFactorization.FactorizeToPowers(phi).Keys;

            for (var ans = 2U; ans <= m; ans++)
                if (factors.All(factor => FastArithmetics.ModuloPow(ans, (ulong) (phi / factor), m) != 1))
                    return ans;

            throw new ArgumentException($"Primitive root for prime modulo must exist. Modulo {m} is not prime");
        }
        
        public static uint? CalculatePrimitiveRoot(uint m)
        {
            // https://math.stackexchange.com/questions/1416422/how-does-one-find-the-primitive-roots-of-a-non-prime-number
            if (m == 0)
                return 0;
            if (m <= 4)
                return m - 1;

            var isEvenSourceM = IsEven(m);
            if (isEvenSourceM)
                m >>= 1;

            if (IsEven(m))
                return null;
            
            var factors = IntegerFactorization.FactorizeToPowers(m);
            if (factors.Count > 1)
                return null;

            var prime = (uint)factors.Keys.Single();

            var primitiveRoot = CalculatePrimitiveRootForPrimeModulo(prime);

            if (m != prime && !IsPrimitiveRoot(primitiveRoot, prime * prime))
                primitiveRoot += prime;

            if (isEvenSourceM && IsEven(primitiveRoot))
                primitiveRoot += m;

            return primitiveRoot;
        }

        public static bool IsPrimitiveRoot(uint primitiveRoot, uint m)
        {
            var existingPowers = new bool[m];

            ulong curPower = primitiveRoot % m;
            for (var power = 1U; power < m; power++)
            {
                existingPowers[curPower] = true;
                curPower = (curPower * primitiveRoot) % m;
            }
            
            for (var i = 1U; i < m; i++)
                if (!existingPowers[i] && Primality.AreCoPrime(i, m))
                    return false;

            return true;
        }

        private static bool IsEven(uint m) => (m & 1) == 0;
    }
}
