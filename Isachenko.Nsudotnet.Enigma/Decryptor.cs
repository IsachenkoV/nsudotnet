using System;
using System.IO;
using System.Security.Cryptography;

namespace Isachenko.Nsudotnet.Enigma
{
    public class Decryptor
    {

        public void DecryptFile(string src, string method, string keys, string dst)
        {
            SymmetricAlgorithm algorithm;
            var srcInfo = new FileInfo(Path.GetFullPath(src));
            var keysFilePath = Path.GetFullPath(keys);

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

            string[] temp = File.ReadAllLines(keysFilePath);
            using (algorithm)
            {
                algorithm.IV = Convert.FromBase64String(temp[0]);
                algorithm.Key = Convert.FromBase64String(temp[1]);
                using (var inStream = srcInfo.OpenRead())
                {
                    using (var outStream = new FileStream(Path.GetFullPath(dst), FileMode.OpenOrCreate))
                    {
                        using (
                            var cryptoStream = new CryptoStream(outStream, algorithm.CreateDecryptor(),
                                CryptoStreamMode.Write))
                        {
                            inStream.CopyTo(cryptoStream);
                            cryptoStream.Flush();
                        }
                    }
                }
            }
        }
    }
}