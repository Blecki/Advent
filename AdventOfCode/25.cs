using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem25
    {
        public static UInt64 NextCode(UInt64 Code)
        {
            var t = Code * 252533;
            return t % 33554393;
        }

        public static void Solve()
        {
            UInt64 targetRow = 2947, targetColumn = 3029;
            UInt64 startCode = 20151125;

            //  1 2 3 4  5
            //1 1 3 6 10
            //2 2 5 9
            //3 4 8
            //4 7
            //5

            UInt64 r = 1;
            UInt64 c = 1;
            UInt64 code = startCode;

            while (true)
            {
                r -= 1;
                c += 1;

                if (r == 0)
                {
                    r = c;
                    c = 1;
                }

                code = NextCode(code);
                if (r == targetRow && c == targetColumn)
                    break;
            }

            Console.WriteLine("Part 1: {0}", code);
        }
    }
}