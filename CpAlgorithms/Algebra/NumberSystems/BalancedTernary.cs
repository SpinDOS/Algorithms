using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpAlgorithms.Algebra.NumberSystems
{
    public readonly struct BalancedTernary
    {
        private readonly sbyte[] ReversedDigits;

        public BalancedTernary(int decimalNumber)
        {
            if (decimalNumber == 0)
            {
                ReversedDigits = null;
                return;
            }
            
            var negative = decimalNumber < 0;
            if (negative)
                decimalNumber = -decimalNumber;
            
            var reversedTernary = new List<sbyte>();
            for (; decimalNumber > 0; decimalNumber /= 3)
                reversedTernary.Add((sbyte)(decimalNumber % 3));
            
            ReversedDigits = new sbyte[reversedTernary.Count + 1];
            var index = 0;
            
            sbyte addNext = 0;
            foreach (var ternaryDigit in reversedTernary)
            {
                var realTernaryDigit = (sbyte)(ternaryDigit + addNext);
                switch (realTernaryDigit)
                {
                    case 0:
                    case 1:
                        ReversedDigits[index++] = realTernaryDigit;
                        addNext = 0;
                        break;
                    case 2:
                    case 3:
                        ReversedDigits[index++] = (sbyte)(realTernaryDigit - 3);
                        addNext = 1;
                        break;
                    default:
                        throw new NotImplementedException($"Unknown real ternary digit {realTernaryDigit}");
                }
            }

            ReversedDigits[index] = addNext;
            
            if (!negative)
                return;
            
            for (var i = 0; i < ReversedDigits.Length; i++)
                ReversedDigits[i] = (sbyte)-ReversedDigits[i];
        }

        public int ToInteger()
        {
            if (ReversedDigits == null)
                return 0;
            
            var result = 0;
            var currentPowOf3 = 1;
            
            foreach (var digit in ReversedDigits)
            {
                result += digit * currentPowOf3;
                currentPowOf3 *= 3;
            }

            return result;
        }

        public override string ToString()
        {
            if (ReversedDigits == null)
                return "0";
            
            var result = new StringBuilder(ReversedDigits.Length);
            var greatestDigitIsZero = ReversedDigits.Last() == 0;
            
            foreach (var digit in ReversedDigits.Reverse().Skip(greatestDigitIsZero ? 1 : 0))
                result.Append(ToChar(digit));
            
            return result.ToString();
        }

        private static char ToChar(sbyte digit)
        {
            switch (digit)
            {
                case -1:
                    return 'Z';
                case 0:
                    return '0';
                case 1:
                    return '1';
                default:
                    throw new NotImplementedException($"Unknown digit {digit}");
            }
        }
    }
}