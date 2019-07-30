using System;

namespace CpAlgorithms
{
    public static class DiophantineSolver
    {
        public static bool TrySolve(long a, long b, long c, out long x, out long y, out long gcd)
        {
            // solve a * x + b * y = c
            if (a == 0 && b == 0)
            {
                x = y = gcd = 1;
                return c == 0;
            }

            gcd = EuclideanGcdAlgorithm.ExtendedGcd(Math.Abs(a), Math.Abs(b), out x, out y);
            if ((c % gcd) != 0)
                return false;

            x *= c / gcd * Math.Sign(a);
            y *= c / gcd * Math.Sign(b);
            return true;
        }

        public static ulong FindSolutionsCount(long a, long b, long c, 
            (long minX, long maxX) intervalX,
            (long minY, long maxY) intervalY)
        {
            if (a == 0 && b == 0)
                return c == 0 ? ulong.MaxValue : 0;
            
            if (!TrySolve(a, b, c, out var x, out var y, out var gcd))
                return 0;

            // minX <= x + n * b / gcd <= maxX
            // minY <= y - n * a / gcd <= maxY
            
            var (nxl, nxr) = FindLimitsForN(intervalX.minX, intervalX.maxX, x, b / gcd);
            var (nyl, nyr) = FindLimitsForN(intervalY.minY, intervalY.maxY, y, -a / gcd);

            var nl = Math.Max(nxl, nyl);
            var nr = Math.Min(nxr, nyr);
            return nr >= nl? (ulong)(nr - nl + 1) : 0;
        }

        private static (long, long) FindLimitsForN(long intervalL, long intervalR, long baseValue, long step)
        {
            if (step == 0)
            {
                return intervalL <= baseValue && baseValue <= intervalR?
                    (long.MinValue, long.MaxValue) : // no limits
                    (long.MaxValue, long.MinValue); // incompatible limits
            }
            
            intervalL -= baseValue;
            intervalR -= baseValue;
            
            if (step < 0)
                (intervalL, intervalR, step) = (-intervalR, -intervalL, -step);

            return ((intervalL + step - 1) / step, intervalR / step);
        }
    }
}