using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem13
    {
        public class LineGrammar : Ancora.Grammar
        {
            public LineGrammar()
            {
                var number = Token(c => "0123456789".Contains(c)).Ast("VALUE");
                var name = Token(c => c != ' ' && c != '.').Ast("NAME");
                var ws = Maybe(Token(c => " \r\n\t".Contains(c))).WithMutator(Discard());

                var line = ws + name + ws +  Keyword("would").WithMutator(Discard()) + ws + name + ws + number + ws + Keyword("happiness units by sitting next to").WithMutator(Discard()) + ws + name + Character('.').WithMutator(Discard());

                Root = line;
            }
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list)
        {
            return GetPermutations(list, list.Count());
        }

        private static IEnumerable<Tuple<T, T>> EnumerateSequencedPairs<T>(IEnumerable<T> List)
        {
            var first = List.First();
            foreach (var item in List.Skip(1))
            {
                yield return Tuple.Create(first, item);
                first = item;
            }

            yield return Tuple.Create(first, List.First());
        }


        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("13Input.txt");
            var lineGrammar = new LineGrammar();
            var happinessTable = new Dictionary<String, Dictionary<String, int>>();
            foreach (var line in input)
            {
                var iter = new Ancora.StringIterator(line);
                var parsedLine = lineGrammar.Root.Parse(iter);
                if (parsedLine.ResultType != Ancora.ResultType.Success) throw new InvalidProgramException();

                var subject = parsedLine.Node.Children[0].Value.ToString();
                var objekt = parsedLine.Node.Children[3].Value.ToString();
                var points = Int32.Parse(parsedLine.Node.Children[2].Value.ToString());
                var sign = parsedLine.Node.Children[1].Value.ToString();

                if (sign == "lose") points = -points;

                if (!happinessTable.ContainsKey(subject)) happinessTable.Add(subject, new Dictionary<string, int>());
                if (!happinessTable[subject].ContainsKey(objekt)) happinessTable[subject].Add(objekt, points);
            }

            Console.WriteLine("Part 1: {0}", MaxHappiness(happinessTable));

            //Add myself.
            var myself = new Dictionary<String, int>();

            foreach (var person in happinessTable)
            {
                person.Value.Add("ME", 0);
                myself.Add(person.Key, 0);
            }

            happinessTable.Add("ME", myself);

            Console.WriteLine("Part 2: {0}", MaxHappiness(happinessTable));
            
        }

        private static int MaxHappiness(Dictionary<String, Dictionary<String, int>> HappinessTable)
        {
            var permutations = GetPermutations(HappinessTable.Select(p => p.Key));
            var maxHappiness = int.MinValue;
            foreach (var perm in permutations)
            {
                var happinessSum = EnumerateSequencedPairs(perm).Select(p => HappinessTable[p.Item1][p.Item2] + HappinessTable[p.Item2][p.Item1]).Sum();
                if (happinessSum > maxHappiness)
                    maxHappiness = happinessSum;
            }

            return maxHappiness;
        }
    }
}