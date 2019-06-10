using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Coreflow.Test.Performance
{
    public class PiCalculator
    {
        // Pi = 16 * arctan(1/5) - 4 * arctan(1/239)
        public BigInteger GetPi(int digits, int iterations)
        {
            return 16 * BigMath.ArcTan1OverX(5, digits).ElementAt(iterations)
                - 4 * BigMath.ArcTan1OverX(239, digits).ElementAt(iterations);
        }
    }
    public static class BigMath
    {
        //arctan(x) = x - x^3/3 + x^5/5 - x^7/7 + x^9/9 - ...
        public static IEnumerable<BigInteger> ArcTan1OverX(int x, int digits)
        {
            var mag = BigInteger.Pow(10, digits);
            var sum = BigInteger.Zero;
            bool sign = true;
            for (int i = 1; true; i += 2)
            {
                var cur = mag / (BigInteger.Pow(x, i) * i);
                if (sign)
                {
                    sum += cur;
                }
                else
                {
                    sum -= cur;
                }
                yield return sum;
                sign = !sign;
            }
        }
    }
}
