using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem09
    {
        private class Connection
        {
            public int Destination;
            public int Distance;
        }

        private class Planet
        {
            public String Name;
            public List<Connection> Connections = new List<Connection>();
        }

        private static IEnumerable<IEnumerable<int>> PermuteIndicies(int Start, int Count)
        {
            return GetPermutations(Enumerable.Range(0, Count), Count);
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static void Solve()
        {
            var input = System.IO.File.ReadAllLines("09Input.txt");

            var planets = new List<Planet>();

            foreach (var line in input)
            {
                var parts = line.Split(' ');
                var source = parts[0];
                var dest = parts[2];
                var distance = Int32.Parse(parts[4]);

                var sIndex = planets.FindIndex(p => p.Name == source);
                var dIndex = planets.FindIndex(p => p.Name == dest);

                if (sIndex < 0)
                {
                    sIndex = planets.Count;
                    planets.Add(new Planet { Name = source });
                }

                if (dIndex < 0)
                {
                    dIndex = planets.Count;
                    planets.Add(new Planet { Name = dest });
                }

                planets[sIndex].Connections.Add(new Connection { Destination = dIndex, Distance = distance });
                planets[dIndex].Connections.Add(new Connection { Destination = sIndex, Distance = distance });
            }

            var shortestDistance = Int32.MaxValue;
            IEnumerable<int> shortestPath = null;

            var longestDistance = 0;
            IEnumerable<int> longestPath = null;

            foreach (var ordering in PermuteIndicies(0, planets.Count))
            {
                var distance = 0;
                if (MeasurePath(ordering, planets, out distance))
                {
                    if (distance < shortestDistance)
                    {
                        shortestPath = ordering;
                        shortestDistance = distance;
                    }

                    if (distance > longestDistance)
                    {
                        longestPath = ordering;
                        longestDistance = distance;
                    }

                    //foreach (var i in ordering)
                    //    Console.Write(planets[i].Name + " -> ");
                    //Console.WriteLine(distance);
                }
                //else
                //    Console.WriteLine("Path failed");
            }

            Console.WriteLine("Shortest path: {0} light years.", shortestDistance);
            foreach (var i in shortestPath)
                Console.WriteLine(planets[i].Name);
            Console.WriteLine();
            Console.WriteLine("Longest path: {0} light years.", longestDistance);
            foreach (var i in longestPath)
                Console.WriteLine(planets[i].Name);
        }

        private static bool MeasurePath(IEnumerable<int> Ordering, List<Planet> Planets, out int Distance)
        {
            Distance = 0;
            var path = Ordering.ToArray();
            for (var i = 0; i < Ordering.Count() - 1; ++i)
            {
                var source = Planets[path[i]];
                var connectionIndex = source.Connections.FindIndex(c => c.Destination == path[i + 1]);
                if (connectionIndex < 0) 
                    return false;
                Distance += source.Connections[connectionIndex].Distance;
            }
            return true;
        }
    }
}
