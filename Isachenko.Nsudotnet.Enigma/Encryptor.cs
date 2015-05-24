using System;
using System.IO;
using System.Security.Cryptography;

namespace Isachenko.Nsudotnet.Enigma
{
    public class Encryptor
    {
        public void EncryptFile(string src, string method, string dest)
        {
            SymmetricAlgorithm algorithm;

            var srcInfo = new FileInfo(Path.GetFullPath(src));
            var keysFilePath = Path.GetFullPath(String.Concat(dest, ".key.txt"));

            switch (method)
            {
                case "aes":
                    algorithm = new AesManaged();
                    break;
                case "des":
                    algorithm = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    algorithm = new RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    algorithm = new RijndaelManaged();
                    break;
                default:
                    Console.WriteLine("Unknown algorithm: {0}", method);
                    return;
            }

            algorithm.GenerateIV();
            algorithm.GenerateKey();

            using (var inStream = srcInfo.OpenRead())
            {
                using (var outStream = new FileStream(Path.GetFullPath(dest), FileMode.OpenOrCreate))
                {
                    using (
                        var cryptoStream = new CryptoStream(outStream, algorithm.CreateEncryptor(),
                            CryptoStreamMode.Write))
                    {                        
                        inStream.CopyTo(cryptoStream);
                        cryptoStream.Flush();
                    }
                }
            }

            string[] temp = {Convert.ToBase64String(algorithm.IV), Convert.ToBase64String(algorithm.Key)};
            File.WriteAllLines(keysFilePath, temp);
        }
    }
}