using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem10
    {
        public static void Solve()
        {
            var input = System.IO.File.ReadAllText("10Input.txt");

            var line = input;
            for (var i = 0; i < 40; ++i)
            {
                line = String.Concat(LookAndSay(line).Select(v => v.ToString()));
            }

            Console.WriteLine("Part 1: {0}", line.Length);

            for (var i = 0; i < 10; ++i)
            {
                line = String.Concat(LookAndSay(line).Select(v => v.ToString()));
            }

            Console.WriteLine("Part 2: {0}", line.Length);

        }
                
        public static IEnumerable<int> LookAndSay(String Data)
        {
            var currentDigit = Data[0];
            int count = 1;
            int place = 1;            

            while (place < Data.Length)
            {
                if (Data[place] == currentDigit)
                    count += 1;
                else
                {
                    yield return count;
                    yield return currentDigit - '0';
                    currentDigit = Data[place];
                    count = 1;
                }

                place += 1;
            }

            yield return count;
            yield return currentDigit - '0';
        }
    }
}
