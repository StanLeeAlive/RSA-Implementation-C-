using System;
using System.Numerics;

namespace CryptoLAB_RSA
{

    public static class BigIntegerExtensions
    {

        public static (BigInteger gcd, BigInteger x, BigInteger y) ExtendedGCD(this BigInteger a, BigInteger b)
        {
            if (b == 0)
                return (a, 1, 0);

            var (gcd, x1, y1) = ExtendedGCD(b, a % b);
            BigInteger x = y1;
            BigInteger y = x1 - (a / b) * y1;

            return (gcd, x, y);
        }


        public static BigInteger ModInverse(this BigInteger a, BigInteger m)
        {
            var (gcd, x, _) = ExtendedGCD(a, m);
            if (gcd != 1)
                throw new ArgumentException("Modular inverse does not exist.");

            return (x % m + m) % m;
        }


        public static BigInteger ModPow(this BigInteger baseVal, BigInteger exp, BigInteger mod)
        {
            if (mod == 1) return 0;
            if (exp < 0) throw new ArgumentException("Exponent must be non-negative");
            
            BigInteger result = 1;
            baseVal = baseVal % mod;

            while (exp > 0)
            {

                if ((exp & 1) == 1)
                    result = (result * baseVal) % mod;


                exp >>= 1;
                baseVal = (baseVal * baseVal) % mod;
            }

            return result;
        }
    }
}
