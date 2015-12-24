using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem20
    {       
        public static void Solve()
        {
            var limit = 29000000;
            
            var presents = new int[limit / 10];

            for (int i = 1; i < limit / 10; ++i)
            {
                for (int x = i; x < limit / 10; x += i)
                {
                    presents[x] += 10 * i;
                    
                }
            }

            for (int i = 0; i < limit / 10; i += 1)
                if (presents[i] > limit)
                {
                    Console.WriteLine("Part 1: House {0} - {1}", i, presents[i]);
                    break;
                }

            presents = new int[limit / 10];

            for (int i = 1; i < limit / 10; ++i)
            {
                for (int x = i, count = 0; x < limit / 10 && count < 50; x += i, count += 1)
                    presents[x] += 11 * i;
            }

            for (int i = 0; i < limit / 10; i += 1)
                if (presents[i] > limit)
                {
                    Console.WriteLine("Part 2: House {0} - {1}", i, presents[i]);
                    break;
                }
        }

    }
}