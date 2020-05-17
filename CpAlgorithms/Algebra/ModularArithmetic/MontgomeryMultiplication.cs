using System.Numerics;

namespace CpAlgorithms.Algebra.ModularArithmetic.MontgomeryMultiplication
{
    public readonly struct MontgomeryNumber
    {
        public BigInteger X { get; }
        internal MontgomeryNumber(BigInteger x) => X = x;
    }
    
    public sealed class MontgomerySpace
    {
        private readonly BigInteger _n, _invN, _nMultiplyR, _modRMask, _r2;
        private readonly int _powerOfR;
        
        public MontgomerySpace(BigInteger modulo)
        {
            // modulo must be odd prime number
            _n = modulo;
            
            var r = new BigInteger(2);
            
            _powerOfR = 1;
            while (r < modulo)
            {
                r *= r;
                _powerOfR *= 2;
            }

            _invN = 1;
            for (var i = 0U; i < _powerOfR; i++)
                _invN *= 2 - _n * _invN;

            _nMultiplyR = _n * r;
            _modRMask = r - 1;

            var rModN = r % modulo;
            _r2 = (rModN * rModN) % modulo;
        }

        public MontgomeryNumber ToSpaceNaive(BigInteger x)
        {
            // we can cache r mod N, but ToSpaceNaive should not be used in real code
            var r = (_modRMask + 1) % _n;
            x %= _n;
            return new MontgomeryNumber((x * r) % _n);
        }

        public MontgomeryNumber ToSpace(BigInteger x)
        {
            x %= _n;
            for (var i = 0; i < _powerOfR; i++) {
                x <<= 1;
                if (x >= _n)
                    x -= _n;
            }
            
            return new MontgomeryNumber(x);
        }

        public MontgomeryNumber ToSpaceFast(BigInteger x)
        {
            x %= _n;
            return new MontgomeryNumber(Reduce(x * _r2));
        }

        public BigInteger FromSpace(MontgomeryNumber x) => Reduce(x.X);

        public MontgomeryNumber Add(MontgomeryNumber lhs, MontgomeryNumber rhs)
        {
            var newX = lhs.X + rhs.X;
            if (newX >= _n)
                newX -= _n;
            return new MontgomeryNumber(newX);
        }

        public MontgomeryNumber Subtract(MontgomeryNumber lhs, MontgomeryNumber rhs)
        {
            var newX = lhs.X - rhs.X;
            return new MontgomeryNumber(newX.Sign >= 0 ? newX : newX + _n);
        }

        public MontgomeryNumber Multiply(MontgomeryNumber lhs, MontgomeryNumber rhs) => 
            new MontgomeryNumber(Reduce(lhs.X * rhs.X));
        
        private BigInteger Reduce(BigInteger x)
        {
            var q = (x * _invN) & _modRMask;
            var a = x - q * _n;
            if (a < 0)
                a += _nMultiplyR;
            return a >> _powerOfR;
        }
    }
}