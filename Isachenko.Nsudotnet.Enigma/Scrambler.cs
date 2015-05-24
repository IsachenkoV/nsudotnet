using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Isachenko.Nsudotnet.Enigma
{
    public class Scrambler
    {
        public SymmetricAlgorithm GetAlgorithm(string method)
        {
            method = method.ToLower();
            switch (method)
            {
                case "aes":
                    return new AesManaged();
                case "des":
                    return new DESCryptoServiceProvider();
                case "rc2":
                    return new RC2CryptoServiceProvider();
                case "rijndael":
                    return new RijndaelManaged();
                default:
                    throw new Exception(string.Format("Unknown algorithm: {0}", method));
            }
        }
    }
}
