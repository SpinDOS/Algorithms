using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    static class BullCow
    {
        public static void Start()
        {
            const int max = 10000;
            string[] vars = new string[max];
            bool[] resheto = new bool[max];
            for (int i = 0; i < max; i++)
            {
                vars[i] = $"{i:d4}";
                resheto[i] = IsOk(vars[i]);
            }
            string ans;
            Console.WriteLine("Придумайте число от 123 до 9876 без повторяющихся цифр");

            do
            {
                int next = FindNext(resheto, max);
                if (next == -1)
                {
                    Console.WriteLine("You are lying");
                    return;
                }
                Console.WriteLine($"Let's try {next:d4}");
                Console.Write("Enter bulls and cows: ");
                ans = Console.ReadLine();
                if (ans == "40")
                {
                    Console.WriteLine("That was easy");
                    return;
                }
                int bulls = int.Parse(ans.Remove(1));
                int cows = int.Parse(ans.Substring(1));
                for (int i = 0; i < max; i++)
                    if (resheto[i] && !Check($"{i:d4}", $"{next:d4}", bulls, cows))
                        resheto[i] = false;
            } while (ans != "40");

        }

        private static int FindNext(bool[] a, int max)
        {
            Random rnd = new Random();
            int r = rnd.Next(max);
            int l = r;
            while (l >= 0 || r < max)
            {
                if (r < max && a[r++])
                    return --r;
                if (l >= 0 && a[l--])
                    return ++l;
            }
            return -1;
        }

        private static bool Check(string cur, string template, int bulls, int cows)
        {
            int temp = 0;
            for (int i = 0; i < 4; i++)
                if (cur[i] == template[i])
                    temp++;
            if (temp != bulls)
                return false;
            temp = 0;
            for (int i = 0; i < 4; i++)
                if (cur[i] != template[i] && cur.Contains(template[i]))
                    temp++;
            if (temp != cows)
                return false;
            return true;
        }

        private static bool IsOk(string s)
        {
            for (int i = 0; i < 4; i++)
                if (s.Remove(i, 1).Contains(s[i]))
                    return false;
            return true;
        }
    }
}
