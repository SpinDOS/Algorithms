using System;
using System.Collections.Generic;
using CpAlgorithms.Algebra.Fundamentals.BinaryExponentiation;

namespace CpAlgorithms.Algebra.ModularArithmetic
{
    public static class DiscreteLogarithm
    {
        public static uint? SimpleSolve(uint a, uint b, uint m)
        {
            // a and n must be relatively prime
            var n = (uint) Math.Sqrt(m) + 1;

            var leftFunction = new Dictionary<uint, uint>((int)n);
            for (var p = 1U; p <= n; p++)
                leftFunction.TryAdd((uint) FastArithmetics.ModuloPow(a, n * p, m), p);

            for (var q = 0U; q <= n; q++)
            {
                var val = (b * FastArithmetics.ModuloPow(a, q, m)) % m;
                if (leftFunction.TryGetValue((uint) val, out var p))
                    return n * p - q;
            }

            return null;
        }
        
        public static uint? OptimizedSolve(uint a, uint b, uint m)
        {
            // a and n must be relatively prime
            var n = (uint) Math.Sqrt(m) + 1;
            
            var leftFunction = new Dictionary<long, uint>((int)n);
            
            var an = FastArithmetics.ModuloPow(a, n, m);
            var leftVal = an;
            for (var p = 1U; p <= n; p++)
            {
                leftFunction.TryAdd(leftVal, p);
                leftVal = (leftVal * an) % m;
            }

            long rightVal = b % m;
            for (var q = 0U; q <= n; q++)
            {
                if (leftFunction.TryGetValue(rightVal, out var p))
                    return n * p - q;
                rightVal = (rightVal * a) % m;
            }

            return null;
        }
    }
}