using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    static class Power
    {
        public static BigInteger SuperPow(BigInteger a, int n)
        {
            BigInteger result = 1;
            for (int byter = 1; byter <= n; byter <<= 1)
            {
                if ((byter & n) != 0)
                    result *= a;
                a *= a;
            }
            return result;
        }

        public static BigInteger SimplePow(BigInteger a, int n)
        {
            BigInteger result = 1;
            for (int i = 0; i < n; i++)
                result *= a;
            return result;
        }
    }
}
