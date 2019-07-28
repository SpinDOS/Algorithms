namespace CpAlgorithms
{
    public static class CommonHelper
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            var c = a;
            a = b;
            b = c;
        }
    }
}