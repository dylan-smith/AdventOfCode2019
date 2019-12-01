using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    public class Day23
    {
        public static string PartOne(string input)
        {
            var bots = GetBots(input);

            return BotsInRange(bots.WithMax(b => b.range), bots).ToString();
        }

        private static int BotsInRange((Point3D location, int range) bot, List<(Point3D location, int range)> bots)
        {
            return bots.Count(b => bot.location.GetManhattanDistance(b.location) <= bot.range);
        }

        private static List<(Point3D location, int range)> GetBots(string input, int divideBy = 1)
        {
            var result = new List<(Point3D location, int range)>();

            foreach (var line in input.Lines())
            {
                var words = line.Split(new string[] { "pos=<", ",", " ", ">", "r=" }, StringSplitOptions.RemoveEmptyEntries);
                result.Add((new Point3D(int.Parse(words[0]) / divideBy, int.Parse(words[1]) / divideBy, int.Parse(words[2]) / divideBy), int.Parse(words[3]) / divideBy));
            }

            return result;
        }

        public static string PartTwo(string input)
        {
            var start = new Point3D(0, 0, 0);
            var best = start;

            for (var divideBy = 1000000; divideBy >= 1; divideBy /= 10)
            {
                var bots = GetBots(input, divideBy);
                var maxBots = int.MinValue;
                var minDistance = long.MaxValue;
                var botCount = 0;

                var reachable = new List<Point3D>();
                reachable.Add(start);

                for (var i = 1; i < 100; i++)
                {
                    Debug.WriteLine($"Distance: {i - 1} [{reachable.Count}]");
                    var newReachable = new List<Point3D>();

                    foreach (var r in reachable)
                    {
                        botCount = bots.Count(b => b.location.GetManhattanDistance(r) <= b.range);

                        if (botCount == maxBots)
                        {
                            if (r.GetManhattanDistance() < minDistance)
                            {
                                best = r;
                                minDistance = r.GetManhattanDistance();
                                Debug.WriteLine($"Bots: {maxBots} - [{r.X}, {r.Y}, {r.Z}] ({minDistance})");
                            }
                        }

                        if (botCount > maxBots)
                        {
                            best = r;
                            maxBots = botCount;
                            minDistance = r.GetManhattanDistance();
                            Debug.WriteLine($"Bots: {maxBots} - [{r.X}, {r.Y}, {r.Z}] ({minDistance})");
                        }

                        newReachable.AddRange(r.GetNeighbors().Where(n => n.GetManhattanDistance(start) == i));
                    }

                    reachable = newReachable.Distinct().ToList();
                }

                start = new Point3D(best.X * 10, best.Y * 10, best.Z * 10);
            }

            return best.GetManhattanDistance().ToString();
        }
    }
}