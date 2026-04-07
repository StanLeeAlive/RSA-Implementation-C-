using System;
using System.Numerics;
using System.Security.Cryptography;

namespace CryptoLAB_RSA
{
    public static class PrimeHelper
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();


        public static BigInteger GenerateRandomBits(int bits)
        {
            int bytes = bits / 8 + 1; 
            byte[] data = new byte[bytes];
            Rng.GetBytes(data);
            

            int excessBits = (bytes * 8) - bits;
            if (excessBits > 0)
            {

            }

            BigInteger result = new BigInteger(data);
            if (result.Sign < 0) result = -result;
            
            // Убедимся, что битовая длина корректна
            while (result.BitLength() < bits)
            {
                result = (result << 1) | 1;
            }
            

            if (result.BitLength() > bits)
            {
                result = result & ((BigInteger.One << bits) - 1);

                result = result | (BigInteger.One << (bits - 1));
            }

            return result;
        }

  
        public static int BitLength(this BigInteger i)
        {
            if (i == 0) return 0;
            if (i < 0) throw new ArgumentException("Negative numbers not supported for bit length");
            

            byte[] bytes = i.ToByteArray();
            int bits = bytes.Length * 8;
            

            for (int j = 7; j >= 0; j--)
            {
                if ((bits - (7 - j)) > 0 && ((i >> (bits - (7 - j))) == 0))
                {
   
                }
            }

            return (int)Math.Floor(BigInteger.Log(i, 2)) + 1;
        }


        public static BigInteger GeneratePrime(int bits)
        {
            while (true)
            {
                BigInteger candidate = GenerateRandomBits(bits);
                
                candidate = candidate | 1;
                candidate = candidate | (BigInteger.One << (bits - 1));

                if (IsPrimeMillerRabin(candidate, 100))
                {
                    return candidate;
                }
            }
        }


        public static bool IsPrimeMillerRabin(BigInteger n, int rounds)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;


            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            byte[] randomBytes = new byte[n.ToByteArray().Length];
            
            for (int i = 0; i < rounds; i++)
            {

                BigInteger a;
                do
                {
                    Rng.GetBytes(randomBytes);
                    a = new BigInteger(randomBytes);
                    if (a.Sign < 0) a = -a;
                    a = a % (n - 3) + 2;
                } while (a <= 1 || a >= n - 1);

                BigInteger x = a.ModPow(d, n);

                if (x == 1 || x == n - 1)
                    continue;

                bool composite = true;
                for (int r = 1; r < s; r++)
                {
                    x = (x * x) % n;
                    if (x == n - 1)
                    {
                        composite = false;
                        break;
                    }
                }

                if (composite)
                    return false;
            }

            return true; 
        }
    }
}