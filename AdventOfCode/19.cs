using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem19
    {
        private static IEnumerable<int> EnumerateIndicies(String Str, String SubString)
        {
            var i = Str.IndexOf(SubString);
            while (i >= 0)
            {
                yield return i;
                i = Str.IndexOf(SubString, i + 1);
            }
        }

        private static String ReplaceSubString(String Str, int Index, int Length, String With)
        {
            return Str.Substring(0, Index) + With + Str.Substring(Index + Length);
        }

        public static void Solve()
        {
            var replacements = new List<Tuple<String, String>>();
            var lines = System.IO.File.ReadAllLines("19Input.txt");
            var input = "";

            foreach (var line in lines)
            {
                if (!String.IsNullOrEmpty(line) && line.IndexOf("=>") < 0)
                {
                    input = line;
                    break;
                }

                var pieces = line.Split(' ');
                if (pieces.Length != 3) continue;
                replacements.Add(Tuple.Create(pieces[0], pieces[2]));
            }

            {
                var molecules = GetPossibleReplacements(input, replacements);
                Console.WriteLine("Part 1: {0}", molecules.Distinct().Count());
            }

            {
                var flippedReplacements = replacements.Select(r => Tuple.Create(r.Item2, r.Item1)).OrderBy(t => t.Item1.Length).ToList();

                // While this 'greedy' solution works for my input, peeking at the advent subreddit tells me that it
                // won't work for all possible inputs. It's possible to reach a dead end - where no substitution is 
                // possible. If that happens, the algorithm would have to back track. 

                var steps = 0;
                var molecule = input;

                while (molecule != "e")
                {
                    steps += 1;
                    var molecules = GetPossibleReplacements(molecule, flippedReplacements);
                    molecule = molecules.First();
                    foreach (var mol in molecules)
                        if (mol.Length < molecule.Length)
                            molecule = mol;
                }

                Console.WriteLine("Part 2: {0}", steps);
            }
        }

        private static IEnumerable<String> GetPossibleReplacements(String Input, List<Tuple<String,String>> Replacements)
        {
            return Replacements.SelectMany(r =>
               EnumerateIndicies(Input, r.Item1).Select(i => ReplaceSubString(Input, i, r.Item1.Length, r.Item2)));
        }
    }
}