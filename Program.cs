using System;
using System.Numerics;


namespace CryptoLAB_RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                Autotests.RunAll();


                Console.WriteLine("Демонстрация работы с модулем 4096 бит");
                
     
                var rsa = new RSAImplementation(4096);
                var keys = rsa.GenerateKeys();

     
                rsa.SaveKeys(keys, "public_key.json", "private_key.json");


                string message = "Secret Message for Lab 4";
                BigInteger m = RSAImplementation.StringToBigInteger(message);
                

                if (m >= keys.N)
                {
                    Console.WriteLine("Сообщение слишком длинное для данного модуля.");
                }
                else
                {
                    Console.WriteLine($"Исходное сообщение: {message}");
                    Console.WriteLine("Шифрование:");
                    BigInteger c = rsa.Encrypt(m, keys.E, keys.N);
                    Console.WriteLine($"Шифртекст (первые 50 символов): {c.ToString().Substring(0, Math.Min(50, c.ToString().Length))}...");

                    Console.WriteLine("Расшифрование:");
                    BigInteger decryptedM = rsa.Decrypt(c, keys.D, keys.N);
                    string decryptedText = RSAImplementation.BigIntegerToString(decryptedM);
                    Console.WriteLine($"Расшифрованное сообщение: {decryptedText}");

                    if (message == decryptedText)
                        Console.WriteLine("\nУСПЕХ: Decr(Encr(m)) == m");
                    else
                        Console.WriteLine("\nОШИБКА: Сообщения не совпадают!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}