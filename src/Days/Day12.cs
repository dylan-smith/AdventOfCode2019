﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 12)]
    public class Day12 : BaseDay
    {
        private List<Moon> _moons;
        private List<List<Moon>> _combos;

        public override string PartOne(string input)
        {
            _moons = input.Lines().Select(x => new Moon(x)).ToList();
            _combos = GetMoonCombos();

            for (var i = 0; i < 1000; i++)
            {
                ProcessGravity();
            }

            return _moons.Sum(m => m.GetTotalEnergy()).ToString();
        }

        private List<List<Moon>> GetMoonCombos()
        {
            var result = new List<List<Moon>>();

            result.Add(new List<Moon>() { _moons[0], _moons[1] });
            result.Add(new List<Moon>() { _moons[0], _moons[2] });
            result.Add(new List<Moon>() { _moons[0], _moons[3] });
            result.Add(new List<Moon>() { _moons[1], _moons[2] });
            result.Add(new List<Moon>() { _moons[1], _moons[3] });
            result.Add(new List<Moon>() { _moons[2], _moons[3] });

            return result;
        }

        private void ProcessGravity()
        {
            foreach (var c in _combos)
            {
                if (c.First().Position.X != c.Last().Position.X)
                {
                    if (c.First().Position.X > c.Last().Position.X)
                    {
                        c.First().Velocity.X--;
                        c.Last().Velocity.X++;
                    }
                    else
                    {
                        c.First().Velocity.X++;
                        c.Last().Velocity.X--;
                    }
                }

                if (c.First().Position.Y != c.Last().Position.Y)
                {
                    if (c.First().Position.Y > c.Last().Position.Y)
                    {
                        c.First().Velocity.Y--;
                        c.Last().Velocity.Y++;
                    }
                    else
                    {
                        c.First().Velocity.Y++;
                        c.Last().Velocity.Y--;
                    }
                }

                if (c.First().Position.Z != c.Last().Position.Z)
                {
                    if (c.First().Position.Z > c.Last().Position.Z)
                    {
                        c.First().Velocity.Z--;
                        c.Last().Velocity.Z++;
                    }
                    else
                    {
                        c.First().Velocity.Z++;
                        c.Last().Velocity.Z--;
                    }
                }
            }

            foreach (var m in _moons)
            {
                m.Position += m.Velocity;
            }
        }

        public override string PartTwo(string input)
        {
            var seen = new List<long[]>[3];
            var steps = new long[3];

            seen[0] = new List<long[]>();
            seen[1] = new List<long[]>();
            seen[2] = new List<long[]>();

            _moons = input.Lines().Select(x => new Moon(x)).ToList();
            _combos = GetMoonCombos();
            var value = GetSeenX();

            Log("Processing X...");
            while (!seen[0].Any(x => x.SequenceEqual(value)))
            {
                seen[0].Add(value);
                ProcessGravity();
                steps[0]++;
                value = GetSeenX();

                if (steps[0] % 10000 == 0)
                {
                    Log(steps[0].ToString());
                }
            }

            _moons = input.Lines().Select(x => new Moon(x)).ToList();
            _combos = GetMoonCombos();
            value = GetSeenY();

            Log("Processing Y...");
            while (!seen[1].Any(x => x.SequenceEqual(value)))
            {
                seen[1].Add(value);
                ProcessGravity();
                steps[1]++;
                value = GetSeenY();

                if (steps[1] % 10000 == 0)
                {
                    Log(steps[1].ToString());
                }
            }

            _moons = input.Lines().Select(x => new Moon(x)).ToList();
            _combos = GetMoonCombos();
            value = GetSeenZ();

            Log("Processing Z...");
            while (!seen[2].Any(x => x.SequenceEqual(value)))
            {
                seen[2].Add(value);
                ProcessGravity();
                steps[2]++;
                value = GetSeenZ();

                if (steps[2] % 10000 == 0)
                {
                    Log(steps[2].ToString());
                }
            }

            // TODO: Punch these 3 numbers into an LCM calculator
            return $"{steps[0]} {steps[1]} {steps[2]}";
        }

        private long[] GetSeenX()
        {
            var result = new long[8];

            result[0] = _moons[0].Position.X;
            result[1] = _moons[0].Velocity.X;
            result[2] = _moons[1].Position.X;
            result[3] = _moons[1].Velocity.X;
            result[4] = _moons[2].Position.X;
            result[5] = _moons[2].Velocity.X;
            result[6] = _moons[3].Position.X;
            result[7] = _moons[3].Velocity.X;

            return result;
        }

        private long[] GetSeenY()
        {
            var result = new long[8];

            result[0] = _moons[0].Position.Y;
            result[1] = _moons[0].Velocity.Y;
            result[2] = _moons[1].Position.Y;
            result[3] = _moons[1].Velocity.Y;
            result[4] = _moons[2].Position.Y;
            result[5] = _moons[2].Velocity.Y;
            result[6] = _moons[3].Position.Y;
            result[7] = _moons[3].Velocity.Y;

            return result;
        }

        private long[] GetSeenZ()
        {
            var result = new long[8];

            result[0] = _moons[0].Position.Z;
            result[1] = _moons[0].Velocity.Z;
            result[2] = _moons[1].Position.Z;
            result[3] = _moons[1].Velocity.Z;
            result[4] = _moons[2].Position.Z;
            result[5] = _moons[2].Velocity.Z;
            result[6] = _moons[3].Position.Z;
            result[7] = _moons[3].Velocity.Z;

            return result;
        }

        private class Moon
        {
            public Point3D Position { get; set; }
            public Point3D Velocity { get; set; }

            public Moon(string input)
            {
                var a = input.ShaveLeft(1).ShaveRight(1).Split(',', StringSplitOptions.RemoveEmptyEntries);

                Position = new Point3D();
                Velocity = new Point3D();

                Position.X = int.Parse(a[0].Trim().ShaveLeft(2));
                Position.Y = int.Parse(a[1].Trim().ShaveLeft(2));
                Position.Z = int.Parse(a[2].Trim().ShaveLeft(2));
            }

            public Moon()
            {
                Position = new Point3D();
                Velocity = new Point3D();
            }

            public long GetTotalEnergy()
            {
                var potential = Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
                var kinetic = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

                return potential * kinetic;
            }

            public Moon Clone()
            {
                var result = new Moon();

                result.Position.X = Position.X;
                result.Position.Y = Position.Y;
                result.Position.Z = Position.Z;

                result.Velocity.X = Velocity.X;
                result.Velocity.Y = Velocity.Y;
                result.Velocity.Z = Velocity.Z;

                return result;
            }
        }
    }
}
