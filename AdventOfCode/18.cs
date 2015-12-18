using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem18
    {
        private static void InitializeGrid(bool[] Grid, String[] Lines)
        {
            for (var y = 0; y < Lines.Length; ++y)
                for (var x = 0; x < Lines[y].Length; ++x)
                    Set(Grid, x, y, Lines[y][x] == '#');
        }

        private static void Set(bool[] Grid, int X, int Y, bool State)
        {
            Grid[(Y * 100) + X] = State;
        }

        private static bool Get(bool[] Grid, int X, int Y)
        {
            if (X < 0 || X >= 100 || Y < 0 || Y >= 100) return false;
            return Grid[(Y * 100) + X];
        }

        private static IEnumerable<bool> EnumerateNeighbors(bool[] Grid, int X, int Y)
        {
            yield return Get(Grid, X - 1, Y);
            yield return Get(Grid, X - 1, Y - 1);
            yield return Get(Grid, X, Y - 1);
            yield return Get(Grid, X + 1, Y - 1);
            yield return Get(Grid, X + 1, Y);
            yield return Get(Grid, X + 1, Y + 1);
            yield return Get(Grid, X, Y + 1);
            yield return Get(Grid, X - 1, Y + 1);
        }

        private static void Conway(bool[] OldGrid, bool[] NewGrid)
        {
            for (var y = 0; y < 100; ++y)
                for (var x = 0; x < 100; ++x)
                {
                    var liveNeighbors = EnumerateNeighbors(OldGrid, x, y).Count(b => b);
                    if (Get(OldGrid, x, y))
                        Set(NewGrid, x, y, liveNeighbors == 2 || liveNeighbors == 3);
                    else
                        Set(NewGrid, x, y, liveNeighbors == 3);
                }
        }

        public static void Solve()
        {
            var initialConfiguration = System.IO.File.ReadAllLines("18Input.txt");

            var states = new bool[2][];
            states[0] = new bool[100 * 100];
            states[1] = new bool[100 * 100];
            var parity = 0;

            InitializeGrid(states[parity], initialConfiguration);

            for (var i = 0; i < 100; ++i)
            {
                Conway(states[parity], states[(parity + 1) % 2]);
                parity = (parity + 1) % 2;
            }

            Console.WriteLine("Part 1: {0}", states[parity].Count(b => b));

            parity = 0;
            InitializeGrid(states[parity], initialConfiguration);
            Set(states[parity], 0, 0, true);
            Set(states[parity], 99, 0, true);
            Set(states[parity], 99, 99, true);
            Set(states[parity], 0, 99, true);

            for (var i = 0; i < 100; ++i)
            {
                Conway(states[parity], states[(parity + 1) % 2]);
                parity = (parity + 1) % 2;
                Set(states[parity], 0, 0, true);
                Set(states[parity], 99, 0, true);
                Set(states[parity], 99, 99, true);
                Set(states[parity], 0, 99, true);
            }

            Console.WriteLine("Part 2: {0}", states[parity].Count(b => b));

        }
    }
}