using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class Problem22
    {
        private enum Spells
        {
            Missile,
            Drain,
            Shield,
            Poison,
            Recharge
        }

        private enum StateType
        {
            Normal,
            Win,
            Lose,
        }

        private class State
        {
            public int BossHP;
            public int Mana;
            public int HP;
            public int ShieldTimer;
            public int PoisonTimer;
            public int RechargeTimer;
            public int ManaSpent;
            public StateType Type;

            public State BossTurn()
            {
                var r = new State
                {
                    BossHP = BossHP,
                    Mana = Mana,
                    HP = HP,
                    ShieldTimer = ShieldTimer,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = RechargeTimer,
                    ManaSpent = ManaSpent,
                    Type = StateType.Normal
                };

                var armor = ShieldTimer == 0 ? 0 : 7;

                r.HP -= (10 - armor);
                if (r.HP <= 0) r.Type = StateType.Lose;
                return r;
            }


            public State ApplyEffects(int PlayerDamage = 0)
            {
                var r = new State
                {
                    BossHP = BossHP,
                    Mana = Mana,
                    HP = HP - PlayerDamage,
                    ShieldTimer = ShieldTimer,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = RechargeTimer,
                    ManaSpent = ManaSpent,
                    Type = StateType.Normal
                };

                if (r.HP <= 0)
                {
                    r.Type = StateType.Lose;
                    return r;
                }

                if (r.PoisonTimer > 0)
                {
                    r.BossHP -= 3;
                    r.PoisonTimer -= 1;
                }

                if (r.RechargeTimer > 0)
                {
                    r.Mana += 101;
                    r.RechargeTimer -= 1;
                }

                if (r.ShieldTimer != 0)
                {
                    r.ShieldTimer -= 1;
                }

                if (r.BossHP <= 0)
                    r.Type = StateType.Win;

                return r;
            }

            public State CastMissile()
            {
                return new State
                {
                    BossHP = BossHP - 4,
                    Mana = Mana - 53,
                    HP = HP,
                    ShieldTimer = ShieldTimer,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = RechargeTimer,
                    ManaSpent = ManaSpent + 53,
                    Type = (BossHP - 4) <= 0 ? StateType.Win : StateType.Normal,
                };
            }

            public State CastDrain()
            {
                return new State
                {
                    BossHP = BossHP - 2,
                    Mana = Mana - 73,
                    HP = HP + 2,
                    ShieldTimer = ShieldTimer,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = RechargeTimer,
                    ManaSpent = ManaSpent + 73,
                    Type = (BossHP - 2) <= 0 ? StateType.Win : StateType.Normal,
                };
            }

            public State CastShield()
            {
                return new State
                {
                    BossHP = BossHP,
                    Mana = Mana - 113,
                    HP = HP,
                    ShieldTimer = 6,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = RechargeTimer,
                    ManaSpent = ManaSpent + 113,
                    Type = StateType.Normal,
                };
            }

            public State CastPosion()
            {
                return new State
                    {
                        BossHP = BossHP,
                        Mana = Mana - 173,
                        HP = HP,
                        ShieldTimer = ShieldTimer,
                        PoisonTimer = 6,
                        RechargeTimer = RechargeTimer,
                        ManaSpent = ManaSpent + 173,
                        Type = StateType.Normal,
                    };
            }

            public State CastRecharge()
            {
                return new State
                {
                    BossHP = BossHP,
                    Mana = Mana - 229,
                    HP = HP,
                    ShieldTimer = ShieldTimer,
                    PoisonTimer = PoisonTimer,
                    RechargeTimer = 5,
                    ManaSpent = ManaSpent + 229,
                    Type = StateType.Normal,
                };
            }
        }

        private static IEnumerable<State> EnumerateTransitionStates(State StartingState)
        {
            /*    Magic Missile costs 53 mana. It instantly does 4 damage.
    Drain costs 73 mana. It instantly does 2 damage and heals you for 2 hit points.
    Shield costs 113 mana. It starts an effect that lasts for 6 turns. While it is active, your armor is increased by 7.
    Poison costs 173 mana. It starts an effect that lasts for 6 turns. At the start of each turn while it is active, it deals the boss 3 damage.
    Recharge costs 229 mana. It starts an effect that lasts for 5 turns. At the start of each turn while it is active, it gives you 101 new mana.
*/
            if (StartingState.Mana >= 53)
                yield return StartingState.CastMissile();
            if (StartingState.Mana >= 73)
                yield return StartingState.CastDrain();
            if (StartingState.Mana >= 113 && StartingState.ShieldTimer == 0)
                yield return StartingState.CastShield();
            if (StartingState.Mana >= 173 && StartingState.PoisonTimer == 0)
                yield return StartingState.CastPosion();
            if (StartingState.Mana >= 229 && StartingState.RechargeTimer == 0)
                yield return StartingState.CastRecharge();
        }

        public static void Solve()
        {
            var startingState = new State
            {
                BossHP = 71,
                HP = 50,
                Mana = 500,
                ManaSpent = 0,
                ShieldTimer = 0,
                PoisonTimer = 0,
                RechargeTimer = 0,
                Type = StateType.Normal
            };

            var openStates = new List<State>(new State[] { startingState });
            var cheapestWin = Int32.MaxValue;

            //while (openStates.Count != 0)
            //{
            //    Console.WriteLine("Open States: {0} Cheapest: {1}", openStates.Count, cheapestWin);
            //    var nextStates = new List<State>();
            //    foreach (var state in openStates)
            //    {
            //        var effectsApplied = state.ApplyEffects();
            //        if (effectsApplied.Type == StateType.Win)
            //        {
            //            cheapestWin = Math.Min(cheapestWin, effectsApplied.ManaSpent);
            //            continue;
            //        }

            //        foreach (var playerTurn in EnumerateTransitionStates(effectsApplied))
            //        {
            //            if (playerTurn.ManaSpent >= cheapestWin) continue;

            //            if (playerTurn.Type == StateType.Win)
            //            {
            //                cheapestWin = playerTurn.ManaSpent;
            //                continue;
            //            }

            //            var postBossTurn = playerTurn.ApplyEffects().BossTurn();
            //            if (postBossTurn.Type == StateType.Win)
            //            {
            //                cheapestWin = Math.Min(cheapestWin, postBossTurn.ManaSpent);
            //                continue;
            //            }
            //            else if (postBossTurn.Type == StateType.Lose) continue;

            //            nextStates.Add(postBossTurn);
            //        }
            //    }

            //    openStates = nextStates;
            //}

            //Console.WriteLine("Part 1: {0}", cheapestWin);

            openStates = new List<State>(new State[] { startingState });
            cheapestWin = Int32.MaxValue;

            while (openStates.Count != 0)
            {
                Console.WriteLine("Open States: {0} Cheapest: {1}", openStates.Count, cheapestWin);
                var nextStates = new List<State>();
                foreach (var state in openStates)
                {
                    var effectsApplied = state.ApplyEffects(1);
                    if (effectsApplied.Type == StateType.Win)
                    {
                        cheapestWin = Math.Min(cheapestWin, effectsApplied.ManaSpent);
                        continue;
                    }
                    else if (effectsApplied.Type == StateType.Lose)
                        continue;

                    foreach (var playerTurn in EnumerateTransitionStates(effectsApplied))
                    {
                        if (playerTurn.ManaSpent >= cheapestWin) continue;

                        if (playerTurn.Type == StateType.Win)
                        {
                            cheapestWin = playerTurn.ManaSpent;
                            continue;
                        }

                        var postBossEffects = playerTurn.ApplyEffects(0);
                        if (postBossEffects.Type == StateType.Win)
                        {
                            cheapestWin = Math.Min(cheapestWin, postBossEffects.ManaSpent);
                            continue;
                        }

                        var postBossTurn = postBossEffects.BossTurn();
                        if (postBossTurn.Type == StateType.Win)
                        {
                            cheapestWin = Math.Min(cheapestWin, postBossTurn.ManaSpent);
                            continue;
                        }
                        else if (postBossTurn.Type == StateType.Lose) continue;

                        nextStates.Add(postBossTurn);
                    }
                }

                openStates = nextStates;
            }

            Console.WriteLine("Part 2: {0}", cheapestWin);

        }
    }
}