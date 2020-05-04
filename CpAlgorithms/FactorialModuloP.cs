using System;

namespace CpAlgorithms
{
    public static class FactorialModuloP
    {
        // calculate (n! / p^x) mod p, where x is power of p in n!
        // p must be prime
        public static uint CalculateFactorialModuloP(uint n, uint p)
        {
            ulong pMinus1 = p - 1;
                
            var result = 1UL;
            while (n > 1)
            {
                var powerOfPMinus1 = n / p;
                var isOddPowerOfPMinus1 = (powerOfPMinus1 & 1) == 1;
                var pMinus1InPower = isOddPowerOfPMinus1 ? pMinus1 : 1UL;
                
                result = (result * pMinus1InPower) % p;
                for (var i = 2UL; i <= n % p; i++)
                    result = (result * i) % p;
                
                n = powerOfPMinus1;
            }

            return (uint)result;
        }
    }
}