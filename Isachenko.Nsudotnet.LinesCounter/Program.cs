using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Isachenko.Nsudotnet.LinesCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            long lineCount = 0;
            var extension = args[0];
            var directory = Directory.GetCurrentDirectory();
            Regex reg = new Regex("(/\\*[^\\*]*[^/]*/|\\/\\/[^\\n]*)");

            Stack<string> dirs = new Stack<string>();
            dirs.Push(directory);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                
                // getting subdirs
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                //getting files
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDir, extension);
                }
                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                // and processing
                foreach (string file in files)
                {
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        string line;
                        bool commentIsOn = false;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            line = reg.Replace(line, "");

                            if (commentIsOn)
                                line = string.Concat("/*", line);

                            line = reg.Replace(line, "");

                            if (line.IndexOf("/*") == -1)
                            {
                                commentIsOn = false;
                            }
                            else
                            {
                                commentIsOn = true;
                                line = string.Concat(line, " */");
                                line = reg.Replace(line, "");
                            }
                                
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            lineCount++;
                        }
                    }
                }

                foreach (string str in subDirs)
                    dirs.Push(str);
            }

            Console.WriteLine("Общее число строк кода в директории\n{0}\nв файлах с расширением {1} равно {2}", directory, extension, lineCount);
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
            Console.ReadKey();
        }
    }
}
