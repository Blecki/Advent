using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem21
    {       
        private class Item
        {
            public int Cost;
            public int Damage;
            public int Armor;

            public Item(int Cost, int Damage, int Armor)
            {
                this.Cost = Cost;
                this.Damage = Damage;
                this.Armor = Armor;
            }
        }

        public static void Solve()
        {
            var weapons = new Item[] 
            {
                new Item(8,4,0), //Dagger        8     4       0
                new Item(10,5,0), //Shortsword   10     5       0
                new Item(25, 6,0), //Warhammer    25     6       0
                new Item(40, 7, 0), //Longsword    40     7       0
                new Item(74, 8, 0), //Greataxe     74     8       0
            };

            var armor = new Item[]
            {
                new Item(13, 0, 1), // Leather  13  0   1
                new Item(31, 0, 2), // Chainmail    31     0       2
                new Item(53, 0, 3), // Splintmail   53     0       3
                new Item(75, 0, 4), // Bandedmail   75     0       4
                new Item(102, 0, 5), //Platemail   102     0       5
            };

            var rings = new Item[]
            {
                new Item(25, 1, 0), // Damage + 1
                new Item(50, 2, 0), // Damage + 2
                new Item(100, 3, 0), // Damage + 3
                new Item(20, 0, 1), // Defense + 1
                new Item(40, 0, 2), // Defense + 2
                new Item(80, 0, 3), // Defense + 3
            };

            var playerHP = 100;

            var bossHP = 103;
            var bossDamage = 9;
            var bossArmor = 2;

            var cheapestWinningSet = Int32.MaxValue;
            var mostExpensiveLosingSet = 0;
            foreach (var itemSet in PermuteItems(weapons, armor, rings))
            {
                var bossDPT = Math.Max(1, bossDamage - itemSet.Sum(i => i.Armor));
                var playerDPT = Math.Max(1, itemSet.Sum(i => i.Damage) - bossArmor);
                var turnsUntilBossDeath = (bossHP / playerDPT) + (bossHP % playerDPT != 0 ? 1 : 0);
                var turnsUntilPlayerDeath = (playerHP / bossDPT) + (playerHP % bossDPT != 0 ? 1 : 0);
                if (turnsUntilBossDeath <= turnsUntilPlayerDeath)
                    cheapestWinningSet = Math.Min(cheapestWinningSet, itemSet.Sum(i => i.Cost));
                else
                    mostExpensiveLosingSet = Math.Max(mostExpensiveLosingSet, itemSet.Sum(i => i.Cost));
            }

            Console.WriteLine("Cheapest winning item set cost {0}", cheapestWinningSet);
            Console.WriteLine("Most expensive losing item set cost {0}", mostExpensiveLosingSet);
        }

        private static IEnumerable<IEnumerable<Item>> PermutePairs(IEnumerable<Item> Items)
        {
            yield return new Item[] { };
            foreach (var item in Items)
            {
                yield return new Item[] { item };
                foreach (var other in Items)
                    if (!Object.ReferenceEquals(item, other)) yield return new Item[] { item, other };
            }
        }

        private static IEnumerable<IEnumerable<Item>> PermuteArmors(IEnumerable<Item> Armors)
        {
            yield return new Item[] { };
            foreach (var armor in Armors)
                yield return new Item[] { armor };
        }

        private static IEnumerable<IEnumerable<Item>> PermuteItems(IEnumerable<Item> Weapons, IEnumerable<Item> Armors, IEnumerable<Item> Rings)
        {
            foreach (var weapon in Weapons)
                foreach (var armorSet in PermuteArmors(Armors))
                    foreach (var ringSet in PermutePairs(Rings))
                        yield return (new Item[] { weapon }).Concat(armorSet).Concat(ringSet);
        }

    }
}