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
            var planets = BuildPlanetList(input);

            return planets.Sum(p => p.CountOrbits()).ToString();
        }

        private IEnumerable<Planet> BuildPlanetList(string input)
        {
            var lines = input.Lines().ToList();
            var planets = new List<Planet>();

            foreach (var line in lines)
            {
                var left = line.Substring(0, 3);
                var right = line.Substring(4);

                var leftPlanet = planets.SingleOrDefault(p => p.Name == left);
                var rightPlanet = planets.SingleOrDefault(p => p.Name == right);

                if (leftPlanet == null)
                {
                    leftPlanet = new Planet(left);
                    planets.Add(leftPlanet);
                }

                if (rightPlanet == null)
                {
                    rightPlanet = new Planet(right);
                    planets.Add(rightPlanet);
                }

                rightPlanet.Orbits = leftPlanet;
                leftPlanet.Orbiters.Add(rightPlanet);
            }

            return planets;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }

        private class Planet
        {
            public List<Planet> Orbiters { get; set; } = new List<Planet>();
            public Planet Orbits { get; set; }
            public string Name { get; set; }

            public Planet(string name)
            {
                Name = name;
            }

            public int CountOrbits()
            {
                if (Orbits == null) return 0;

                return Orbits.CountOrbits() + 1;
            }
        }
    }
}
