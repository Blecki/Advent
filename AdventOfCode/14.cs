using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem14
    {
        public class LineGrammar : Ancora.Grammar
        {
            public LineGrammar()
            {
                //0             1          2                                 3
                //Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.

                var number = Token(c => "0123456789".Contains(c)).Ast("VALUE");
                var name = Token(c => c != ' ' && c != '.').Ast("NAME");
                var ws = Maybe(Token(c => " \r\n\t".Contains(c))).WithMutator(Discard());

                var line = name + Keyword(" can fly ").WithMutator(Discard()) + number +
                    Keyword(" km/s for ").WithMutator(Discard()) + number +
                    Keyword(" seconds, but then must rest for ").WithMutator(Discard()) + number +
                    Keyword(" seconds.").WithMutator(Discard());
 
                Root = line;
            }
        }

        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("14Input.txt");
            var lineGrammar = new LineGrammar();

            var maxFlight = 0;

            foreach (var line in input)
            {
                var iter = new Ancora.StringIterator(line);
                var parsedLine = lineGrammar.Root.Parse(iter);
                if (parsedLine.ResultType != Ancora.ResultType.Success) throw new InvalidProgramException();

                var distance = Int32.Parse(parsedLine.Node.Children[1].Value.ToString());
                var flyTime = Int32.Parse(parsedLine.Node.Children[2].Value.ToString());
                var restTime = Int32.Parse(parsedLine.Node.Children[3].Value.ToString());

                var flightDistance = ReindeerFlightDistance(2503, distance, flyTime, restTime);
                maxFlight = Math.Max(flightDistance, maxFlight);
            }

            Console.WriteLine("Part 1: {0}", maxFlight);


            // Unfortunately, the calculated distance method does not work for part two.

            var reindeer = new List<Reindeer>();

            foreach (var line in input)
            {
                var iter = new Ancora.StringIterator(line);
                var parsedLine = lineGrammar.Root.Parse(iter);
                if (parsedLine.ResultType != Ancora.ResultType.Success) throw new InvalidProgramException();

                var distance = Int32.Parse(parsedLine.Node.Children[1].Value.ToString());
                var flyTime = Int32.Parse(parsedLine.Node.Children[2].Value.ToString());
                var restTime = Int32.Parse(parsedLine.Node.Children[3].Value.ToString());

                reindeer.Add(new Reindeer { Points = 0, Speed = distance, Flytime = flyTime, Resttime = restTime });
            }

            for (var t = 1; t <= 2503; ++t)
            {
                var maxDistance = 0;
                foreach (var deer in reindeer)
                {
                    deer.Distance = ReindeerFlightDistance(t, deer.Speed, deer.Flytime, deer.Resttime);
                    maxDistance = Math.Max(maxDistance, deer.Distance);
                }

                foreach (var deer in reindeer.Where(d => d.Distance == maxDistance))
                    deer.Points += 1;
            }

            Console.WriteLine("Part 2: {0}", reindeer.Max(d => d.Points));
        }

        class Reindeer
        {
            public int Distance;
            public int Points;
            public int Speed;
            public int Flytime;
            public int Resttime;
        };

        private static int ReindeerFlightDistance(int Time, int Distance, int Flytime, int Resttime)
        {
            var cycleLength = Flytime + Resttime;
            var completeCycles = Time / cycleLength;
            var partialCycle = Time % cycleLength;

            if (partialCycle >= Flytime) return (Distance * Flytime * completeCycles) + (Distance * Flytime);
            else return (Distance * Flytime * completeCycles) + (Distance * partialCycle);
        }
    }
}