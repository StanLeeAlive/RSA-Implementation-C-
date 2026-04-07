using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.Json;


namespace CryptoLAB_RSA
{
    public class RSAKeyPair
    {
        public BigInteger N { get; set; } 
        public BigInteger E { get; set; } 
        public BigInteger D { get; set; } 

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
    }

    public class RSAImplementation
    {
        private readonly int _keyLength;

        public RSAImplementation(int keyLength = 4096)
        {
            _keyLength = keyLength;
        }


        public RSAKeyPair GenerateKeys()
        {
            Console.WriteLine($"Генерация ключей длиной {_keyLength} бит...");
            

            int primeBits = _keyLength / 2;
            BigInteger p = PrimeHelper.GeneratePrime(primeBits);
            BigInteger q = PrimeHelper.GeneratePrime(primeBits);

            while (p == q)
            {
                q = PrimeHelper.GeneratePrime(primeBits);
            }

            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);


            BigInteger e = 65537;


            if (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {

                throw new Exception("e и phi не взаимно просты (крайне редко).");
            }


            BigInteger d = e.ModInverse(phi);

            Console.WriteLine("Ключи сгенерированы.");

            return new RSAKeyPair
            {
                N = n,
                E = e,
                D = d,
                P = p, 
                Q = q
            };
        }


        public BigInteger Encrypt(BigInteger message, BigInteger publicKeyE, BigInteger modulusN)
        {
            if (message >= modulusN)
                throw new ArgumentException("Сообщение должно быть меньше модуля N.");
            
            return message.ModPow(publicKeyE, modulusN);
        }


        public BigInteger Decrypt(BigInteger cipher, BigInteger privateKeyD, BigInteger modulusN)
        {
            return cipher.ModPow(privateKeyD, modulusN);
        }


        public void SaveKeys(RSAKeyPair keys, string pubPath, string privPath)
        {
            var pubObj = new { N = keys.N.ToString(), E = keys.E.ToString() };
            var privObj = new { N = keys.N.ToString(), D = keys.D.ToString() };

            File.WriteAllText(pubPath, JsonSerializer.Serialize(pubObj, new JsonSerializerOptions { WriteIndented = true }));
            File.WriteAllText(privPath, JsonSerializer.Serialize(privObj, new JsonSerializerOptions { WriteIndented = true }));
            
            Console.WriteLine($"Ключи сохранены: {pubPath}, {privPath}");
        }
        

        public static BigInteger StringToBigInteger(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] bigBytes = new byte[bytes.Length + 1];
            Array.Copy(bytes, bigBytes, bytes.Length);
            return new BigInteger(bigBytes);
        }
        
        
        public static string BigIntegerToString(BigInteger number)
        {
            byte[] bytes = number.ToByteArray();
            if (bytes[bytes.Length - 1] == 0 && bytes.Length > 1)
            {
                byte[] newBytes = new byte[bytes.Length - 1];
                Array.Copy(bytes, newBytes, newBytes.Length);
                bytes = newBytes;
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}