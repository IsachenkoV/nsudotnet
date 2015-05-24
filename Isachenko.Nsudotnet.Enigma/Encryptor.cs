using System;
using System.IO;
using System.Security.Cryptography;

namespace Isachenko.Nsudotnet.Enigma
{
    public class Encryptor : Scrambler
    {
        public void EncryptFile(string src, string method, string dest)
        {
            SymmetricAlgorithm algorithm;

            var srcInfo = new FileInfo(Path.GetFullPath(src));
            var keysFilePath = Path.GetFullPath(String.Concat(dest, ".key.txt"));

            using (algorithm = GetAlgorithm(method))
            {
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
}