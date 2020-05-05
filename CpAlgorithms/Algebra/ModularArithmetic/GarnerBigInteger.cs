using System;
using System.Linq;
using System.Numerics;
using CpAlgorithms.Algebra.PrimeNumbers;

namespace CpAlgorithms.Algebra.ModularArithmetic
{
    public readonly struct GarnerBigInteger
    {
        private const int PrimesCount = 1000;
        private static readonly object Sync = new object();
        
        private static BigInteger _primesProduct;
        private static BigInteger _maxValue;
        private static uint[] _primes;
        private static uint[,] _inverses;

        private readonly uint[] _a;
        
        public static void InitPrimes() 
        {
            if (_primes != null)
                return;

            lock (Sync)
                if (_primes == null)
                {
                    _primes = GetPrimes(PrimesCount);
                    _inverses = GetInverses(_primes);
                    _primesProduct = MultiplyPrimes(_primes);
                    _maxValue = _primesProduct >> 1;
                }
        }

        private static uint[] GetPrimes(int primesCount)
        {
            var primes = new uint[primesCount];
            
            var i = 0;
            for (var num = 1_000_000_001L; num < uint.MaxValue && i < primesCount; num += 2)
                if (Primality.MillerRabin(num))
                    primes[i++] = (uint) num;

            return primes;
        }

        private static uint[,] GetInverses(uint[] primes)
        {
            var inverses = new uint[primes.Length, primes.Length];
            for (var i = 0; i < primes.Length; i++)
            for (var j = i + 1; j < primes.Length; j++)
            {
                if (!ModularInverse.EuclideanModularInverse(primes[i], primes[j], out var inverse))
                    throw new ArgumentException($"Cannot find modular inverse for prime {primes[i]} modulo {primes[j]}");
                inverses[i, j] = (uint) inverse;
            }

            return inverses;
        }

        private static BigInteger MultiplyPrimes(uint[] primes) =>
            primes.Aggregate(new BigInteger(1), (result, prime) => result * prime);

        private GarnerBigInteger(uint[] a)
        {
            _a = a;
        }

        public GarnerBigInteger(long num) : this(new BigInteger(num))
        {
        }
        
        public GarnerBigInteger(BigInteger num)
        {
            InitPrimes();
            
            var absNum = num.Sign < 0 ? -num : num;
            if (absNum > _maxValue)
                throw new ArgumentOutOfRangeException($"Too large num. Max absolute value is {_maxValue}, got {num}");
            
            if (num.Sign < 0)
                num += _primesProduct;
            
            _a = new uint[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
                _a[i] = (uint) (num % _primes[i]);
        }

        public BigInteger ToBigInteger()
        {
            var result = new BigInteger(0);
            var primesMultiply = new BigInteger(1);
            var x = new long[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
            {
                x[i] = _a[i];
                for (var j = 0; j < i; j++)
                {
                    x[i] = (_inverses[j, i] * (x[i] - x[j])) % _primes[i];
                    if (x[i] < 0)
                        x[i] += _primes[i];
                }

                result += x[i] * primesMultiply;
                primesMultiply *= _primes[i];
            }

            return result >= _maxValue ? result - _primesProduct : result;
        }

        public override bool Equals(object obj) => 
            obj is GarnerBigInteger other && this == other;

        public override int GetHashCode() =>
            unchecked((int)_a.Aggregate(0u, (hash, num) => hash ^ num));

        public override string ToString() => ToBigInteger().ToString();

        public static bool operator ==(GarnerBigInteger lhs, GarnerBigInteger rhs) =>
            lhs._a.SequenceEqual(rhs._a);

        public static bool operator !=(GarnerBigInteger lhs, GarnerBigInteger rhs) => !(lhs == rhs);

        public static GarnerBigInteger operator-(GarnerBigInteger num)
        {
            var result = new uint[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
                result[i] = _primes[i] - num._a[i];
            return new GarnerBigInteger(result);
        }

        public static GarnerBigInteger operator+(GarnerBigInteger lhs, GarnerBigInteger rhs)
        {
            var result = new uint[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
                result[i] = (lhs._a[i] + rhs._a[i]) % _primes[i];
            return new GarnerBigInteger(result);
        }

        public static GarnerBigInteger operator-(GarnerBigInteger lhs, GarnerBigInteger rhs)
        {
            var result = new uint[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
                result[i] = (_primes[i] + lhs._a[i] - rhs._a[i]) % _primes[i];
            return new GarnerBigInteger(result);
        }

        public static GarnerBigInteger operator*(GarnerBigInteger lhs, GarnerBigInteger rhs)
        {
            var result = new uint[PrimesCount];
            for (var i = 0; i < PrimesCount; i++)
                result[i] = (uint)(((ulong)lhs._a[i] * (ulong)rhs._a[i]) % _primes[i]);
            return new GarnerBigInteger(result);
        }
        
        public static implicit operator GarnerBigInteger(long num) => new GarnerBigInteger(num);
    }
}
