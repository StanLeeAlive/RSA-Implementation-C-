using System;
using System.Numerics;

namespace CryptoLAB_RSA
{
    public static class Autotests
    {
        public static void RunAll()
        {
            Console.WriteLine("=== ЗАПУСК АВТОТЕСТОВ ===\n");
            int passed = 0;
            int total = 0;


            total++;
            if (TestExtendedGCD()) { passed++; Console.WriteLine("[PASS] Расширенный Евклид"); }
            else { Console.WriteLine("[FAIL] Расширенный Евклид"); }


            total++;
            if (TestModPow()) { passed++; Console.WriteLine("[PASS] ModPow"); }
            else { Console.WriteLine("[FAIL] ModPow"); }


            total++;
            if (TestPrimality()) { passed++; Console.WriteLine("[PASS] Проверка простоты"); }
            else { Console.WriteLine("[FAIL] Проверка простоты"); }


            total++;
            if (TestKeyGeneration()) { passed++; Console.WriteLine("[PASS] Генерация ключей"); }
            else { Console.WriteLine("[FAIL] Генерация ключей"); }

 
            total++;
            if (TestEncryptionCycle()) { passed++; Console.WriteLine("[PASS] Полный цикл шифрования"); }
            else { Console.WriteLine("[FAIL] Полный цикл шифрования"); }

            Console.WriteLine($"\n=== РЕЗУЛЬТАТ: {passed}/{total} тестов пройдено ===");
            if (passed != total) throw new Exception("Не все тесты пройдены!");
        }

        private static bool TestExtendedGCD()
        {
            BigInteger a = 240;
            BigInteger b = 46;
            var (gcd, x, y) = a.ExtendedGCD(b);

            return gcd == 2 && (a * x + b * y) == gcd;
        }

        private static bool TestModPow()
        {

            BigInteger ourRes = ((BigInteger)3).ModPow(10, 1000); 
    
            // Сравнение со встроенной реализацией .NET для верификации
            BigInteger sysRes = BigInteger.ModPow(3, 10, 1000);
    
            return ourRes == sysRes; // Должно быть true (49 == 49)
        }
        private static bool TestPrimality()
        {
            if (!PrimeHelper.IsPrimeMillerRabin(101, 10)) return false;
            if (!PrimeHelper.IsPrimeMillerRabin(104729, 10)) return false;
            if (PrimeHelper.IsPrimeMillerRabin(100, 10)) return false;
            return true;
        }

        private static bool TestKeyGeneration()
        {
            try
            {
                // Для теста уменьшим длину, чтобы не ждать долго
                var rsa = new RSAImplementation(512); 
                var keys = rsa.GenerateKeys();
                return keys.N > 0 && keys.E > 0 && keys.D > 0 && (keys.E * keys.D) % ((keys.P - 1) * (keys.Q - 1)) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TestEncryptionCycle()
        {
            try
            {
                var rsa = new RSAImplementation(512);
                var keys = rsa.GenerateKeys();
                
                string originalMessage = "Hello RSA Lab 4!";
                BigInteger m = RSAImplementation.StringToBigInteger(originalMessage);
                
                if (m >= keys.N) return false;

                BigInteger c = rsa.Encrypt(m, keys.E, keys.N);
                BigInteger decryptedM = rsa.Decrypt(c, keys.D, keys.N);
                
                string decryptedMessage = RSAImplementation.BigIntegerToString(decryptedM);
                
                return originalMessage == decryptedMessage;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}