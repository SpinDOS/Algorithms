using System.Collections.Generic;

namespace CpAlgorithms.Algebra.Miscellaneous
{
    public static class SubmaskEnumeration
    {
        public static IEnumerable<uint> EnumerateBitSubmasksDescending(uint m)
        {
            for (var s = m; s != 0; s = (s - 1) & m)
                yield return s;
            yield return 0;
        }
    }
}