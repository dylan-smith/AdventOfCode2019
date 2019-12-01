using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day24
    {
        public static string PartOne(string input)
        {
            var armies = CreateArmies().ToList();

            while (armies.Any(x => x.ArmyType == ArmyType.Infection && x.Units > 0) && armies.Any(x => x.ArmyType == ArmyType.Immune && x.Units > 0))
            {
                Fight(armies);
            }

            return armies.Sum(x => x.Units).ToString();
        }

        private static void Fight(List<Army> armies)
        {
            var targetsChosen = ChooseTargets(armies).OrderByDescending(x => x.attacker.Initiative).ToList();

            foreach (var battle in targetsChosen)
            {
                battle.attacker.Attack(battle.defender);
            }

            armies.RemoveAll(x => x.Units <= 0);
        }

        private static List<(Army attacker, Army defender)> ChooseTargets(List<Army> armies)
        {
            armies = armies.OrderByDescending(x => x.GetEffectivePower() * 100 + x.Initiative).ToList();
            var availableTargets = new List<Army>(armies);
            var targetsChosen = new List<(Army attacker, Army defender)>();

            foreach (var army in armies)
            {
                var mostDamageTargets = new List<Army>();
                var mostDamage = int.MinValue;

                foreach (var target in availableTargets.Where(x => x.ArmyType != army.ArmyType).ToList())
                {
                    var damage = army.GetDamageDealt(target);

                    if (damage == mostDamage)
                    {
                        mostDamageTargets.Add(target);
                    }

                    if (damage > mostDamage)
                    {
                        mostDamage = damage;
                        mostDamageTargets.Clear();
                        mostDamageTargets.Add(target);
                    }
                }

                if (mostDamage > 0)
                {
                    var finalTarget = mostDamageTargets.OrderByDescending(x => x.GetEffectivePower() * 100 + x.Initiative).First();

                    targetsChosen.Add((army, finalTarget));
                    availableTargets.Remove(finalTarget);
                }
            }

            return targetsChosen;
        }

        private static IEnumerable<Army> CreateArmies()
        {
            // Infection Army
            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 3401,
                Hp = 31843,
                Weaknesses = new List<AttackType>() { AttackType.Cold, AttackType.Fire },
                Immunities = new List<AttackType>(),
                Ap = 16,
                AttackType = AttackType.Slashing,
                Initiative = 19
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 1257,
                Hp = 10190,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>(),
                Ap = 16,
                AttackType = AttackType.Cold,
                Initiative = 8
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 2546,
                Hp = 49009,
                Weaknesses = new List<AttackType>() { AttackType.Bludgeoning, AttackType.Radiation },
                Immunities = new List<AttackType>() { AttackType.Cold },
                Ap = 38,
                AttackType = AttackType.Bludgeoning,
                Initiative = 6
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 2593,
                Hp = 12475,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>(),
                Ap = 9,
                AttackType = AttackType.Cold,
                Initiative = 1
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 2194,
                Hp = 25164,
                Weaknesses = new List<AttackType>() { AttackType.Bludgeoning },
                Immunities = new List<AttackType>() { AttackType.Cold },
                Ap = 18,
                AttackType = AttackType.Bludgeoning,
                Initiative = 14
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 8250,
                Hp = 40519,
                Weaknesses = new List<AttackType>() { AttackType.Bludgeoning, AttackType.Radiation },
                Immunities = new List<AttackType>() { AttackType.Slashing },
                Ap = 8,
                AttackType = AttackType.Bludgeoning,
                Initiative = 16
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 1793,
                Hp = 51817,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>() { AttackType.Bludgeoning },
                Ap = 46,
                AttackType = AttackType.Radiation,
                Initiative = 3
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 288,
                Hp = 52213,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>() { AttackType.Bludgeoning },
                Ap = 339,
                AttackType = AttackType.Fire,
                Initiative = 4
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 22,
                Hp = 38750,
                Weaknesses = new List<AttackType>() { AttackType.Fire },
                Immunities = new List<AttackType>(),
                Ap = 3338,
                AttackType = AttackType.Slashing,
                Initiative = 5
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Infection,
                Units = 2365,
                Hp = 25468,
                Weaknesses = new List<AttackType>() { AttackType.Radiation, AttackType.Cold },
                Immunities = new List<AttackType>(),
                Ap = 20,
                AttackType = AttackType.Fire,
                Initiative = 12
            };

            // Immune Army
            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 89,
                Hp = 11269,
                Weaknesses = new List<AttackType>() { AttackType.Fire, AttackType.Radiation },
                Immunities = new List<AttackType>(),
                Ap = 1018,
                AttackType = AttackType.Slashing,
                Initiative = 7
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 371,
                Hp = 8033,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>(),
                Ap = 204,
                AttackType = AttackType.Bludgeoning,
                Initiative = 15
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 86,
                Hp = 12112,
                Weaknesses = new List<AttackType>() { AttackType.Cold },
                Immunities = new List<AttackType>() { AttackType.Slashing, AttackType.Bludgeoning },
                Ap = 1110,
                AttackType = AttackType.Slashing,
                Initiative = 18
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 4137,
                Hp = 10451,
                Weaknesses = new List<AttackType>() { AttackType.Slashing },
                Immunities = new List<AttackType>() { AttackType.Radiation },
                Ap = 20,
                AttackType = AttackType.Slashing,
                Initiative = 11
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 3374,
                Hp = 6277,
                Weaknesses = new List<AttackType>() { AttackType.Slashing, AttackType.Cold },
                Immunities = new List<AttackType>(),
                Ap = 13,
                AttackType = AttackType.Cold,
                Initiative = 10
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 1907,
                Hp = 1530,
                Weaknesses = new List<AttackType>() { AttackType.Radiation },
                Immunities = new List<AttackType>() { AttackType.Fire, AttackType.Bludgeoning },
                Ap = 7,
                AttackType = AttackType.Fire,
                Initiative = 9
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 1179,
                Hp = 6638,
                Weaknesses = new List<AttackType>() { AttackType.Slashing, AttackType.Bludgeoning },
                Immunities = new List<AttackType>() { AttackType.Radiation },
                Ap = 49,
                AttackType = AttackType.Fire,
                Initiative = 20
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 4091,
                Hp = 7627,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>(),
                Ap = 17,
                AttackType = AttackType.Bludgeoning,
                Initiative = 17
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 6318,
                Hp = 7076,
                Weaknesses = new List<AttackType>(),
                Immunities = new List<AttackType>(),
                Ap = 8,
                AttackType = AttackType.Bludgeoning,
                Initiative = 2
            };

            yield return new Army()
            {
                ArmyType = ArmyType.Immune,
                Units = 742,
                Hp = 1702,
                Weaknesses = new List<AttackType>() { AttackType.Radiation },
                Immunities = new List<AttackType>() { AttackType.Slashing },
                Ap = 22,
                AttackType = AttackType.Radiation,
                Initiative = 13
            };
        }

        public static string PartTwo(string input)
        {
            var armies = CreateArmies().ToList();
            var boost = 0;

            do
            {
                armies = CreateArmies().ToList();
                boost++;
                armies.Where(x => x.ArmyType == ArmyType.Immune).ForEach(x => x.Ap += boost);
                var unitCount = 0;

                while (armies.Any(x => x.ArmyType == ArmyType.Infection && x.Units > 0) && armies.Any(x => x.ArmyType == ArmyType.Immune && x.Units > 0) && armies.Sum(x => x.Units) != unitCount)
                {
                    unitCount = armies.Sum(x => x.Units);
                    Fight(armies);
                }
            } while (armies.Any(x => x.ArmyType == ArmyType.Infection && x.Units > 0));

            return armies.Sum(x => x.Units).ToString();
        }
    }

    public class Army
    {
        public ArmyType ArmyType { get; set; }
        public int Units { get; set; }
        public int Hp { get; set; }
        public List<AttackType> Weaknesses { get; set; }
        public List<AttackType> Immunities { get; set; }
        public int Ap { get; set; }
        public AttackType AttackType { get; set; }
        public int Initiative { get; set; }

        public int GetEffectivePower()
        {
            return Units * Ap;
        }

        public int GetDamageDealt(Army target)
        {
            if (target.Immunities.Contains(AttackType))
            {
                return 0;
            }

            if (target.Weaknesses.Contains(AttackType))
            {
                return GetEffectivePower() * 2;
            }

            return GetEffectivePower();
        }

        public void Attack(Army defender)
        {
            defender.TakeDamage(GetDamageDealt(defender));
        }

        public void TakeDamage(int damage)
        {
            Units -= damage / Hp;

            if (Units < 0)
            {
                Units = 0;
            }
        }
    }

    public enum ArmyType
    {
        Infection,
        Immune
    }

    public enum AttackType
    {
        Fire,
        Radiation,
        Slashing,
        Bludgeoning,
        Cold
    }
}