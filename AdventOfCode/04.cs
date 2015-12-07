using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem04
    {
        public static void Solve()
        {
            var key = "bgvyzdsv";
            var md5 = System.Security.Cryptography.MD5.Create();

            var updateTime = 10000;
            var updateTimer = updateTime;
            var count = 0;
            
            while (true)
            {
                var candidate = key + count.ToString();
                var hash = String.Concat(md5.ComputeHash(Encoding.UTF8.GetBytes(candidate)).Select(i => i.ToString("X2")));

                updateTimer -= 1;
                if (updateTimer == 0)
                {
                    Console.WriteLine(candidate);
                    Console.WriteLine(hash);
                    updateTimer = updateTime;
                }

                if (hash.StartsWith("000000"))
                {
                    Console.WriteLine("Found hash at {0}", count);
                    Console.WriteLine(candidate);
                    Console.WriteLine(hash);
                    return;
                }

                count += 1;
            }
            
        }
    }
}
