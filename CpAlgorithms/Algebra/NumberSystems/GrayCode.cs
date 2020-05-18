namespace CpAlgorithms.Algebra.NumberSystems
{
    public readonly struct GrayCode
    {
        public uint Code { get; }
        
        public GrayCode(uint num) => Code = num ^ (num >> 1);

        public uint ToInteger()
        {
            var result = 0U;
            for (var current = Code; current != 0; current >>= 1)
                result ^= current;
            return result;
        }

        public uint ToIntegerFast()
        {
            var result = Code;
            result ^= result >> 16;
            result ^= result >> 8;
            result ^= result >> 4;
            result ^= result >> 2;
            result ^= result >> 1;
            return result;
        }
    }
}