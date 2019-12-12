using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 12)]
    public class Day12 : BaseDay
    {
        private List<Moon> _moons;
        private List<List<Moon>> _seen;

        public override string PartOne(string input)
        {
            _moons = input.Lines().Select(x => new Moon(x)).ToList();

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
            var combos = GetMoonCombos();

            foreach (var c in combos)
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
                m.Position = m.Position + m.Velocity;
            }
        }

        public override string PartTwo(string input)
        {
            _moons = input.Lines().Select(x => new Moon(x)).ToList();
            _seen = new List<List<Moon>>();

            var steps = 0;

            while (!SeenState())
            {
                _seen.Add(GetMoonState());
                ProcessGravity();
                steps++;

                if (steps % 10000 == 0)
                {
                    Log(steps.ToString());
                }
            }

            return steps.ToString();
        }

        private bool SeenState()
        {
            foreach (var s in _seen)
            {
                var match = true;

                for (var i = 0; i < 4; i++)
                {
                    if (s[i].Position != _moons[i].Position)
                    {
                        match = false;
                        break;
                    }

                    if (s[i].Velocity != _moons[i].Velocity)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return true;
                }
            }

            return false;
        }

        private List<Moon> GetMoonState()
        {
            return _moons.Select(m => m.Clone()).ToList();
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
