using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem08
    {
        public static void Solve()
        {
            var input = System.IO.File.ReadAllText("08Input.txt");

            var totalCodeCharacters = 0;
            var totalMemoryCharacters = 0;

            var iter = new Ancora.StringIterator(input);

            while (!iter.AtEnd)
            {
                if (" \n\r\t".Contains(iter.Next))
                {
                    iter = iter.Advance();
                    continue;
                }

                if (iter.Next == '\\')
                {
                    iter = iter.Advance();
                    if (iter.Next == 'x')
                    {
                        iter = iter.Advance(3);
                        totalCodeCharacters += 4;
                        totalMemoryCharacters += 1;
                    }
                    else
                    {
                        iter = iter.Advance();
                        totalCodeCharacters += 2;
                        totalMemoryCharacters += 1;
                    }
                }
                else if (iter.Next == '\"')
                {
                    iter = iter.Advance();
                    totalCodeCharacters += 1;
                }
                else
                {
                    iter = iter.Advance();
                    totalCodeCharacters += 1;
                    totalMemoryCharacters += 1;
                }
            }

            Console.WriteLine("Part 1: {0} - {1} = {2}", totalCodeCharacters, totalMemoryCharacters, totalCodeCharacters - totalMemoryCharacters);


            var specialCount = input.Count(c => "\\\"".Contains(c));
            var newLineCount = input.Count(c => c == '\n');

            Console.WriteLine("Part 2: {0}", specialCount + (newLineCount * 2));
            
        }
    }
}
