using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day06
    {
        public static string PartOne(string input)
        {
            var coords = input.Lines().Select(x => new Point(int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1]))).ToList();

            var maxY = coords.Max(x => x.Y) + 5;
            var maxX = coords.Max(x => x.X) + 5;

            var grid = new Point[maxX + 1, maxY + 1];

            for (var x = 0; x <= grid.GetUpperBound(0); x++)
            {
                for (var y = 0; y <= grid.GetUpperBound(1); y++)
                {
                    grid[x, y] = FindClosestCoord(x, y, coords);
                }
            }

            var outsidePoints = new List<Point>();

            for (var x = 0; x < maxX; x++)
            {
                outsidePoints.Add(grid[x, 0]);
                outsidePoints.Add(grid[x, maxY]);
            }

            for (var y = 0; y < maxY; y++)
            {
                outsidePoints.Add(grid[0, y]);
                outsidePoints.Add(grid[maxX, y]);
            }

            var candidates = coords.Where(c => !outsidePoints.Any(p => p == c));

            var counts = grid.ToList().GroupBy(x => x)
                .Where(x => candidates.Any(c => c == x.Key))
                .Select(x => x.Count())
                .OrderByDescending(x => x).ToList();

            return counts.First().ToString();
        }

        private static Point FindClosestCoord(int x, int y, List<Point> coords)
        {
            var distances = coords.Select(c => c.ManhattanDistance(new Point(x, y))).ToList();

            var minDistance = distances.Min();

            if (distances.Count(d => d == minDistance) == 1)
            {
                return coords.First(c => c.ManhattanDistance(new Point(x, y)) == distances.Min());
            }

            return default(Point);
        }

        public static string PartTwo(string input)
        {
            var coords = input.Lines().Select(x => new Point(int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1]))).ToList();

            var maxY = coords.Max(c => c.Y) + 300;
            var maxX = coords.Max(c => c.X) + 300;

            var safeCount = 0;
            const int SAFE_DISTANCE = 10000;

            for (var x = -300; x <= maxX; x++)
            {
                for (var y = -300; y <= maxY; y++)
                {
                    if (FindTotalDistance(x, y, coords) < SAFE_DISTANCE)
                    {
                        safeCount++;
                    }
                }
            }

            return safeCount.ToString();
        }

        private static int FindTotalDistance(int x, int y, List<Point> coords)
        {
            return coords.Sum(c => c.ManhattanDistance(new Point(x, y)));
        }
    }
}