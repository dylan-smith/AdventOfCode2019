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
                var left = line.Split(')').First();
                var right = line.Split(')').Last();

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
            var planets = BuildPlanetList(input);

            var startPlanet = planets.Single(p => p.Name == "YOU").Orbits;
            var targetPlanet = planets.Single(p => p.Name == "SAN").Orbits;

            var distances = new Dictionary<Planet, int>();
            planets.ForEach(p => distances.Add(p, int.MaxValue));

            CalcDistance(startPlanet, 0, distances);

            return distances[targetPlanet].ToString();
        }

        private void CalcDistance(Planet planet, int distance, Dictionary<Planet, int> distances)
        {
            if (distances[planet] < distance) return;

            distances[planet] = distance;

            if (planet.Orbits != null)
            {
                CalcDistance(planet.Orbits, distance + 1, distances);
            }

            foreach (var p in planet.Orbiters)
            {
                CalcDistance(p, distance + 1, distances);
            }
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
