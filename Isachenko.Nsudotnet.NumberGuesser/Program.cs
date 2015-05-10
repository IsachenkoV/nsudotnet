using System;

namespace Isachenko.Nsudotnet.NumberGuesser
{
    class Program
    {
        private const string Welcome = "Приветствую, путник!\nДа ведет тебя дорога приключений!\nКак тебя зовут?";

        private const string Anon = "Ок, тогда я буду называть тебя {0}";

        private const string Usage = "{0}, слабо угадать число от 0 до 100?\n" + "Если слабо, то введи 'q'";

        private static readonly string[] DerogatoryPhrases = {"Здесь должна быть шутка про блондинок.", "{0}, слушай, кажется уже даже мой вай-фай модуль догадался.", 
                                                              "Британские учёные доказали, что {0} почти угадал число", "{0}, ты пьян? Иди домой", "Слоупок превратился в слоубро, пока {0} угадывал число",
                                                              "Псс, парень, не хочешь немного мозгов?", "{0}, угадай число, нутыжпрограммист! Или нет?" };
        static void Main(string[] args)
        {
            var name = "Анонимус";
            Console.WriteLine(Welcome);
            string s = Console.ReadLine();
            if (string.IsNullOrEmpty(s))
            {
                Console.WriteLine(Anon, name);
            }
            else
            {
                name = s;
            }

            Console.WriteLine(Usage, name);

            Random randomExample = new Random();
            int number = randomExample.Next(0, 100);
            bool numberGuessed = false;
            int attempts = 0;
            int[] statistics = new int[1000];
            DateTime startTime = DateTime.Now;

            while (true)
            {
                var str = Console.ReadLine();
                if (str.Equals("q"))
                {
                    Console.WriteLine("Эта задачка оказалась тебе не по силам :(");
                    break;
                }

                var current = -1;
                try
                {
                    current = int.Parse(str);
                }
                catch (Exception)
                {
                    Console.WriteLine("Не балуйся с клавиатурой и введи уже число!");
                    continue;
                }

                if (attempts >= 1000)
                    attempts = 0;
                statistics[attempts] = current;

                attempts++;
                if (current == number)
                {
                    Console.WriteLine("Поздравляю, ты угадал число!");
                    numberGuessed = true;
                    break;
                }
                else
                {
                    Console.WriteLine(current < number
                        ? "Твое число меньше загаданного"
                        : "Твое число больше загаданного");
                    if (attempts % 4 == 0)
                    {
                        int numberOfFunnyPhrase = randomExample.Next(0, DerogatoryPhrases.Length - 1);
                        Console.WriteLine(DerogatoryPhrases[numberOfFunnyPhrase], name);
                    }
                }
            }

            if (numberGuessed)
            {
                Console.WriteLine("{0} совершил {1} попыток", name, attempts);
                for (int i = 0; i < attempts; i++)
                {
                    Console.WriteLine(statistics[i] < number
                        ? "{0} <"
                        : "{0} >", statistics[i]);
                }
                TimeSpan timeWasted = DateTime.Now.Subtract(startTime);
                Console.WriteLine("Времени потрачено: {0}:{1}", timeWasted.Minutes, timeWasted.Seconds);
            }

            Console.WriteLine("Заходи еще :)");
            Console.Read();
        }
    }
}
