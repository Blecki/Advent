using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem11
    {
        public static void Solve()
        {
            var input = System.IO.File.ReadAllText("11Input.txt");

            do
                input = IncrementPassword(input);
            while (!ValidatePassword(input));

            Console.WriteLine("Part 1: {0}", input);

            do
                input = IncrementPassword(input);
            while (!ValidatePassword(input));

            Console.WriteLine("Part 2: {0}", input);
        }

        private static String IncrementPassword(String Password)
        {
            var array = IncrementPlace(Password.ToCharArray(), Password.Length - 1);
            return String.Concat(array);
        }

        private static char[] IncrementPlace(char[] Password, int Place)
        {
            if (Place < 0) return Password;

            var c = Password[Place];
            if (c == 'z')
            {
                Password[Place] = 'a';
                return IncrementPlace(Password, Place - 1);
            }
            else
            {
                Password[Place] = (char)(c + 1);
                return Password;
            }
        }

        private static bool ValidatePassword(String Password)
        {
            var foundStraight = false;
            for (var i = 0; i < Password.Length - 2; ++i)
                if ((Password[i + 2] == Password[i + 1] + 1) && (Password[i + 1] == Password[i] + 1))
                    foundStraight = true;
            if (!foundStraight) return false;

            if (Password.Contains('i') || Password.Contains('l') || Password.Contains('o')) return false;

            if (!System.Text.RegularExpressions.Regex.IsMatch(Password, ".*(.)\\1+.*(.)\\2+.*")) return false;

            return true;
        }
    }
}
