using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem24
    {        
        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("24Input.txt").Select(l => Int32.Parse(l)).Reverse();

            var sum = input.Sum();
            var sectionTotal = sum / 4;
            var count = input.Count();

            Console.WriteLine("Total weight: {0} Section weight: {1}", sum, sectionTotal);

            var considered = 0;
            var etq = UInt64.MaxValue;

            for (var i = 4; i < count; ++i)
            {
                foreach (var ordering in GetPermutations(input, i))
                {
                    considered += 1;
                    var partitianSum = ordering.Sum();
                    if (partitianSum == sectionTotal)
                    {
                        var thisEtq = Product(ordering);
                        if (thisEtq < etq)
                        {
                            foreach (var x in ordering) Console.Write("{0} ", x);
                            Console.WriteLine("= {0} ETQ: {1}", partitianSum, thisEtq);
                            etq = thisEtq;
                        }
                    }
                }
            }

            Console.WriteLine("Considered {0} partitians", considered);            
        }

        private static UInt64 Product(IEnumerable<int> Input)
        {
            UInt64 x = 1;
            foreach (var i in Input.Select(i => (ulong)i)) x *= i;
            return x;
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

    }
}