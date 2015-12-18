using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem16
    {
        enum PropType
        {
            less,
            exact,
            greater,
        }

        public class LineGrammar : Ancora.Grammar
        {
            public LineGrammar()
            {
                //    0    1            2   3     4  5        6
                //Sue 500: pomeranians: 10, cats: 3, vizslas: 5
                
                var number = Token(c => "-0123456789".Contains(c)).Ast("VALUE");
                var thing = Token(c => c != ':').Ast("THING");
                var name = Token(c => c != ' ' && c != ':').Ast("NAME");
                var ws = Maybe(Token(c => " \r\n\t".Contains(c))).WithMutator(Discard());

                var line = Keyword("Sue ").WithMutator(Discard()) + number + Keyword(": ").WithMutator(Discard())
                    + thing + Keyword(": ").WithMutator(Discard()) + number + Keyword(", ").WithMutator(Discard())
                    + thing + Keyword(": ").WithMutator(Discard()) + number + Keyword(", ").WithMutator(Discard())
                    + thing + Keyword(": ").WithMutator(Discard()) + number;

                Root = line;
            }
        }

        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("16Input.txt");
            var lineGrammar = new LineGrammar();
            var Aunts = new List<Dictionary<String, int>>();

            foreach (var line in input)
            {
                var iter = new Ancora.StringIterator(line);
                var parsedLine = lineGrammar.Root.Parse(iter);
                if (parsedLine.ResultType != Ancora.ResultType.Success) throw new InvalidProgramException();

                var aunt = new Dictionary<String, int>();

                for (int i = 1; i < 6; i += 2)
                    aunt.Add(parsedLine.Node.Children[i].Value.ToString(), Int32.Parse(parsedLine.Node.Children[i + 1].Value.ToString()));

                Aunts.Add(aunt);
            }

            var knownItems = new Dictionary<String, Tuple<int, PropType>>();
            knownItems.Add("children", Tuple.Create(3, PropType.exact));
            knownItems.Add("cats", Tuple.Create(7, PropType.greater));
            knownItems.Add("samoyeds", Tuple.Create(2, PropType.exact));
            knownItems.Add("pomeranians", Tuple.Create(3, PropType.less));
            knownItems.Add("akitas", Tuple.Create(0, PropType.exact));
            knownItems.Add("vizslas", Tuple.Create(0, PropType.exact));
            knownItems.Add("goldfish", Tuple.Create(5, PropType.less));
            knownItems.Add("trees", Tuple.Create(3, PropType.greater));
            knownItems.Add("cars", Tuple.Create(2, PropType.exact));
            knownItems.Add("perfumes", Tuple.Create(1, PropType.exact));

            for (var i = 0; i < Aunts.Count; ++i)
            {
                var rejectAunt = false;
                foreach (var property in Aunts[i])
                    if (knownItems[property.Key].Item1 != property.Value) rejectAunt = true;
                if (!rejectAunt)
                    Console.WriteLine("Part 1 matched Aunt {0}", i + 1);

                rejectAunt = false;
                foreach (var property in Aunts[i])
                    switch (knownItems[property.Key].Item2)
                    {
                        case PropType.exact:
                            if (knownItems[property.Key].Item1 != property.Value) rejectAunt = true;
                            break;
                        case PropType.greater:
                            if (property.Value <= knownItems[property.Key].Item1) rejectAunt = true;
                            break;
                        case PropType.less:
                            if (property.Value >= knownItems[property.Key].Item1) rejectAunt = true;
                            break;
                    }
                if (!rejectAunt)
                    Console.WriteLine("Part 2 matched Aunt {0}", i + 1);

            }

        }
    }
}