using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isachenko.Nsudotnet.Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "encrypt":
                    Encryptor encryptor = new Encryptor();
                    if (args.Length != 4)
                    {
                        Console.WriteLine("The number of args must be equal to 4!");
                        break;
                    }

                    try
                    {
                        encryptor.EncryptFile(args[1], args[2], args[3]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                    break;
                case "decrypt":
                    Decryptor decryptor = new Decryptor();
                    if (args.Length != 5)
                    {
                        Console.WriteLine("The number of args must be equal to 5!");
                        break;
                    }

                    try
                    {
                        decryptor.DecryptFile(args[1], args[2], args[3], args[4]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;
                default:
                    Console.WriteLine("Unknown command. This program can only encrypt and decrypt files.");
                    break;
            }

            Console.ReadKey();
        }
    }
}
