using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem17
    {
        public static void Solve()
        {
            var containerSizes = System.IO.File.ReadAllLines("17Input.txt").Select(l => Int32.Parse(l)).ToArray();

            var combinationCount = 0;
            var minimumContainersUsed = Int32.MaxValue;
            var countOfMinimumCombinations = 0;
            for (var i = 0; i < Math.Pow(2, containerSizes.Length); ++i)
            {
                var t = i;
                var containersUsed = 0;
                var total = 0;
                var place = 0;
                while (t > 0)
                {
                    if ((t & 1) == 1)
                    {
                        containersUsed += 1;
                        total += containerSizes[place];
                    }
                    t >>= 1;
                    place += 1;
                }

                if (total == 150)
                {
                    combinationCount += 1;

                    if (containersUsed == minimumContainersUsed)
                        countOfMinimumCombinations += 1;
                    else if (containersUsed < minimumContainersUsed)
                    {
                        countOfMinimumCombinations = 1;
                        minimumContainersUsed = containersUsed;
                    }
                }
            }

            Console.WriteLine("Part 1: {0}", combinationCount);
            Console.WriteLine("Part 2: {0}", countOfMinimumCombinations);

        }
    }
}