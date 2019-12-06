using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 6)]
    public class Day06 : BaseDay
    {
        public override string PartOne(string input)
        {
            //input = "X)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L";

            var com = new Planet("COM");

            var orbits = input.Lines().ToList();

            while (orbits.Any())
            {
                AddOrbits(com, orbits);
            }

            return com.CountOrbits().ToString();
        }

        private void AddOrbits(Planet com, List<string> orbits)
        {
            var orbitsCopy = orbits.Select(x => x).ToList();

            foreach (var x in orbitsCopy)
            {
                var left = x.Substring(0, 3);
                var right = x.Substring(4);

                var planet = com.Find(left);

                if (planet != null)
                {
                    planet.AddOrbit(right);
                    orbits.Remove(x);
                }
            }
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }

        private class Planet
        {
            private List<Planet> _orbits = new List<Planet>();
            public string Name { get; set; }

            public Planet(string name)
            {
                Name = name;
            }

            public void AddOrbit(string newPlanet)
            {
                var p = new Planet(newPlanet);

                _orbits.Add(p);
            }

            public int CountOrbits()
            {
                var result = 0;

                foreach (var p in _orbits)
                {
                    result += p.CountOrbits() + p.CountPlanets();
                }

                //System.Diagnostics.Debug.WriteLine($"{Name} = {result}");
                return result;
            }

            private int CountPlanets()
            {
                var result = 1;

                foreach (var p in _orbits)
                {
                    result += p.CountPlanets();
                }

                //System.Diagnostics.Debug.WriteLine($"{Name} = {result} - COUNT");
                return result;
            }

            public Planet Find(string name)
            {
                if (Name == name) return this;

                foreach (var x in _orbits)
                {
                    var result = x.Find(name);

                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }
        }
    }
}
