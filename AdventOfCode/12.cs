using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem12
    {
        public class JSONGrammar : Ancora.Grammar
        {
            public JSONGrammar()
            {
                var array = LateBound();
                var obj = LateBound();
                var ws = Maybe(Token(c => " \r\n\t".Contains(c))).WithMutator(Discard());
                var number = Identifier(c => "-0123456789".Contains(c), c => "-0123456789".Contains(c)).Ast("NUMBER");
                var str = (Character('"') + Token(c => c != '"') + '"').Ast("STRING").WithMutator(n =>
                    {
                        n.Value = n.Children[1].Value;
                        n.Children.Clear();
                        return n;
                    });
                var property = (str + ws + Character(':').WithMutator(Discard()) + ws + (str | number | array | obj).WithMutator(Collapse())).Ast("PROPERTY");
                var propList = DelimitedList(property, (ws + ',' + ws).WithMutator(Discard())).WithMutator(n =>
                    {
                        var r = new Ancora.AstNode { NodeType = "OBJECT" };
                        r.Children.Add(n.Children[0]);
                        foreach (var sub in n.Children[1].Children)
                            r.Children.Add(sub.Children[0]);
                        return r;
                    });

                array.SetSubParser((Character('[').WithMutator(Discard()) + ws + Maybe(DelimitedList((number | str | array | obj).WithMutator(Collapse()), (ws + Character(',').WithMutator(Discard()) + ws).WithMutator(Discard())).WithMutator(n =>
                {
                    var r = new Ancora.AstNode { NodeType = "ARRAY" };
                    r.Children.Add(n.Children[0]);
                    foreach (var sub in n.Children[1].Children)
                        r.Children.Add(sub.Children[0]);
                    return r;
                })).WithMutator(Collapse()) + ws + Character(']').WithMutator(Discard())).WithMutator(Collapse()).Ast("ARRAY"));
                obj.SetSubParser((Character('{').WithMutator(Discard()) + ws + Maybe(propList).WithMutator(Collapse()) + ws + Character('}').WithMutator(Discard())).Ast("OBJECT").WithMutator(Collapse()));

                Root = obj;
            }
        }

        private static int Sum(Ancora.AstNode Node)
        {
            if (Node.NodeType == "NUMBER") return Int32.Parse(Node.Value.ToString());
            else if (Node.NodeType == "PROPERTY") return Sum(Node.Children[1]);
            else if (Node.NodeType == "ARRAY" || Node.NodeType == "OBJECT")
                return Node.Children.Sum(n => Sum(n));
            else return 0;
        }

        private static int SumSansRed(Ancora.AstNode Node)
        {
            if (Node.NodeType == "NUMBER") return Int32.Parse(Node.Value.ToString());
            else if (Node.NodeType == "PROPERTY") return SumSansRed(Node.Children[1]);
            else if (Node.NodeType == "ARRAY") return Node.Children.Sum(n => SumSansRed(n));
            else if (Node.NodeType == "OBJECT")
            {
                var redPropIndex = Node.Children.FindIndex(c =>
                    {
                        if (c.NodeType != "PROPERTY") return false;
                        if (c.Children[1].NodeType != "STRING") return false;
                        return c.Children[1].Value.ToString() == "red";
                    });
                if (redPropIndex >= 0) return 0;
                else return Node.Children.Sum(n => SumSansRed(n));
            }
            else return 0;
        }

        public static void Solve()
        {
            var input = System.IO.File.ReadAllText("12Input.txt");

            //while (true)
            //{
            //    input = Console.ReadLine();
                var itr = new Ancora.StringIterator(input);

                var iter = new Ancora.StringIterator(input);
                var grammar = new JSONGrammar();

                var r = grammar.Root.Parse(iter);
                if (r.ResultType == Ancora.ResultType.Success && !r.After.AtEnd && r.FailReason == null)
                    r.FailReason = new Ancora.Failure(grammar.Root, "Did not consume all input.");

                if (r.ResultType == Ancora.ResultType.Success)
                {
                    Console.WriteLine("Parsed.");

                    pause = 30;
                    //EmitAst(r.Node, 0);
                    Console.WriteLine("Part 1: {0}", Sum(r.Node));
                    Console.WriteLine("Part 2: {0}", SumSansRed(r.Node));
                
                }
                else
                {
                    Console.WriteLine("Failed.");
                    if (r.FailReason != null)
                        Console.WriteLine(r.FailReason.Message);
                    else
                        Console.WriteLine("No fail reason specified.");
                }
            //}
        }

        static int pause;

        static void EmitAst(Ancora.AstNode Node, int Depth)
        {
            pause -= 1;
            if (pause == 0)
            {
                Console.ReadKey();
                pause = 30;
            }

            Console.WriteLine(new String(' ', Depth) + Node.NodeType + " : " + (Node.Value == null ? "null" : "\"" + Node.Value.ToString() + "\""));

            foreach (var child in Node.Children)
                EmitAst(child, Depth + 1);
        }
    }
}