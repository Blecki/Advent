using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem23
    {
        private static int DecodeOffset(String Str)
        {
            var r = Int32.Parse(Str.Substring(1));
            if (Str[0] == '-') return (r * -1);
            return r;
        }

        public static void Solve()
        {
            var lines = System.IO.File.ReadAllLines("23Input.txt");


            Console.WriteLine("Part 1 - b: {0}", Emulate(0, lines));
            Console.WriteLine("Part 2 - b: {0}", Emulate(1, lines));

        }

        private static UInt32 Emulate(UInt32 a, String[] Lines)
        {
            UInt32 b = 0;
            var IP = 0;
            var running = true;

            #region Emulate

            while (running)
            {
                if (IP < 0 || IP >= Lines.Length)
                    break;

                var instruction = Lines[IP];
                var parts = instruction.Split(' ');
                switch (parts[0])
                {
                    // hlf r sets register r to half its current value, then continues with the next instruction.
                    case "hlf":
                        if (parts[1] == "a") a = a / 2;
                        else b = b / 2;
                        IP += 1;
                        break;

                    //tpl r sets register r to triple its current value, then continues with the next instruction.
                    case "tpl":
                        if (parts[1] == "a") a = a * 3;
                        else b = b * 3;
                        IP += 1;
                        break;

                    //inc r increments register r, adding 1 to it, then continues with the next instruction.
                    case "inc":
                        if (parts[1] == "a") a = a + 1;
                        else b = b + 1;
                        IP += 1;
                        break;

                    //jmp offset is a jump; it continues with the instruction offset away relative to itself.
                    case "jmp":
                        IP = IP + DecodeOffset(parts[1]);
                        break;

                    //jie r, offset is like jmp, but only jumps if register r is even ("jump if even").
                    case "jie":
                        if (parts[1] == "a," && (a % 2) == 0) IP = IP + DecodeOffset(parts[2]);
                        else if (parts[1] == "b," && (b % 2) == 0) IP = IP + DecodeOffset(parts[2]);
                        else IP = IP + 1;
                        break;

                    //jio r, offset is like jmp, but only jumps if register r is 1 ("jump if one", not odd).
                    case "jio":
                        if (parts[1] == "a," && a == 1) IP = IP + DecodeOffset(parts[2]);
                        else if (parts[1] == "b," && b == 1) IP = IP + DecodeOffset(parts[2]);
                        else IP = IP + 1;
                        break;

                    default:
                        running = false;
                        break;
                }

            }

            #endregion

            return b;
        }
    }
}