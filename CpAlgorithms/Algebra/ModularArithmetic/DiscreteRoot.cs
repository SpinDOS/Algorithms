using System.Collections.Generic;
using System.Linq;
using CpAlgorithms.Algebra.Fundamentals;
using CpAlgorithms.Algebra.Fundamentals.BinaryExponentiation;

namespace CpAlgorithms.Algebra.ModularArithmetic
{
    public static class DiscreteRoot
    {
        public static uint? CalculateDiscreteRoot(uint a, uint k, uint m)
        {
            if (k == 0)
                return a == 1 ? (uint?) 1 : null;
            if (a == 0)
                return 0;
            return CalculateDiscreteRootImpl(a, k, m)?.x0;
        }
        
        public static IEnumerable<uint> CalculateDiscreteRoots(uint a, uint k, uint m)
        {
            if (k == 0)
                return a == 1 
                    ? Enumerable.Range(1, (int)m).Select(x => (uint)x) 
                    : Enumerable.Empty<uint>();
            if (a == 0)
                return Enumerable.Range(0, (int) m).Select(x => (uint) x * m);

            return CalculateDiscreteRootsImpl(a, k, m);
        }
        
        private static (uint primeRoot, uint x0)? CalculateDiscreteRootImpl(uint a, uint k, uint m)
        {
            // m must be primitive
            var primeRoot = PrimitiveRoot.CalculatePrimitiveRootForPrimeModulo(m);

            var gPowK = (uint)FastArithmetics.ModuloPow(primeRoot, k, m);
            var y0 = DiscreteLogarithm.OptimizedSolve(gPowK, a, m);
            if (y0 == null)
                return null;
            return (primeRoot, (uint)FastArithmetics.ModuloPow(primeRoot, y0.Value, m));
        }
        
        private static IEnumerable<uint> CalculateDiscreteRootsImpl(uint a, uint k, uint m)
        {
            var discreteRootImplResult = CalculateDiscreteRootImpl(a, k, m);
            if (discreteRootImplResult == null)
                yield break;

            var phi = m - 1;
            var step = phi / (uint)EuclideanGcdAlgorithm.Gcd(phi, k);
            var multiplier = FastArithmetics.ModuloPow(discreteRootImplResult.Value.primeRoot, step, m);
            
            long ans = discreteRootImplResult.Value.x0;
            for (var i = 0; i < m; i++)
            {
                yield return (uint)ans;
                ans = (ans * multiplier) % m;
            }
        }
    }
}
