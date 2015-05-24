using System;
using System.IO;
using System.Security.Cryptography;

namespace Isachenko.Nsudotnet.Enigma
{
    public class Decryptor : Scrambler
    {

        public void DecryptFile(string src, string method, string keys, string dst)
        {
            SymmetricAlgorithm algorithm;
            var srcInfo = new FileInfo(Path.GetFullPath(src));
            var keysFilePath = Path.GetFullPath(keys);

            string[] temp = File.ReadAllLines(keysFilePath);
            using (algorithm = GetAlgorithm(method))
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