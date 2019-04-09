using System;

namespace KormenAlgorithms
{
    public static class CommonHelper
    {
        public static readonly Random GlobalRandom = new Random();
        
        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}