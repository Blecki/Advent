using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem15
    {
        public class LineGrammar : Ancora.Grammar
        {
            public LineGrammar()
            {
                //0               1             2         3          4            5
                //Sugar: capacity 3, durability 0, flavor 0, texture -3, calories 2


                var number = Token(c => "-0123456789".Contains(c)).Ast("VALUE");
                var name = Token(c => c != ' ' && c != ':').Ast("NAME");
                var ws = Maybe(Token(c => " \r\n\t".Contains(c))).WithMutator(Discard());

                var line = name + 
                    Keyword(": capacity ").WithMutator(Discard()) + number +
                    Keyword(", durability ").WithMutator(Discard()) + number +
                    Keyword(", flavor ").WithMutator(Discard()) + number +
                    Keyword(", texture ").WithMutator(Discard()) + number +
                    Keyword(", calories ").WithMutator(Discard()) + number;
 
                Root = line;
            }
        }

        private class Ingredient
        {
            public String Name;
            public int[] Properties;
        }

        private static IEnumerable<IEnumerable<int>> EnumerateCombinations(int Total, int Count)
        {
            if (Count == 1) yield return new int[] { Total };
            else
            {
                for (var i = 0; i <= Total; ++i)
                    foreach (var sequence in EnumerateCombinations(Total - i, Count - 1))
                        yield return (new int[] { i }).Concat(sequence);
            }
        }

        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("15Input.txt");
            var lineGrammar = new LineGrammar();
            var ingredients = new List<Ingredient>();

            foreach (var line in input)
            {
                var iter = new Ancora.StringIterator(line);
                var parsedLine = lineGrammar.Root.Parse(iter);
                if (parsedLine.ResultType != Ancora.ResultType.Success) throw new InvalidProgramException();

                ingredients.Add(new Ingredient
                {
                    Name = parsedLine.Node.Children[0].Value.ToString(),
                    Properties = parsedLine.Node.Children.Skip(1).Select(c => Int32.Parse(c.Value.ToString())).ToArray()
                });
            }

            var part1MaxValue = 0;
            var part2MaxValue = 0;
            foreach (var sequence in EnumerateCombinations(100, 4))
            {
                if (sequence.Sum() != 100) throw new InvalidOperationException();
                var recipeValue = MeasureRecipe(sequence.ToArray(), ingredients);

                part1MaxValue = Math.Max(part1MaxValue, recipeValue);

                var calories = MeasureProperty(sequence.ToArray(), ingredients, 4);
                if (calories == 500) part2MaxValue = Math.Max(part2MaxValue, recipeValue);
            }

            Console.WriteLine("Part 1: {0}", part1MaxValue);
            Console.WriteLine("Part 2: {0}", part2MaxValue);
        }

        private static int MeasureRecipe(int[] IngredientAmounts, List<Ingredient> Ingredients)
        {
            var totals = new int[] { 0, 0, 0, 0 };
            
            for (var propertyIndex = 0; propertyIndex < 4; ++propertyIndex)
                totals[propertyIndex] = MeasureProperty(IngredientAmounts, Ingredients, propertyIndex);

            var product = 1;
            foreach (var sum in totals) product *= sum;
            return product;
        }

        private static int MeasureProperty(int[] IngredientAmounts, List<Ingredient> Ingredients, int PropertyIndex)
        {
            var total = 0;
            for (var ingredientIndex = 0; ingredientIndex < Ingredients.Count; ++ingredientIndex)
                total += Ingredients[ingredientIndex].Properties[PropertyIndex] * IngredientAmounts[ingredientIndex];
            return Math.Max(total, 0);
        }
    }
}