using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 20)]
    public class Day20 : BaseDay
    {
        public override string PartOne(string input)
        {
            var map = input.CreateCharGrid();

            var portalPoints = GetPortalPoints(map).ToList();
            var portals = GetPortals(map, portalPoints);
            var paths = GetPaths(map, portalPoints);

            var startPos = portalPoints.Single(p => p.name == "AA").pos;
            var endPos = portalPoints.Single(p => p.name == "ZZ").pos;

            var result = FindBestPath(portals, paths, startPos, endPos);

            return result.ToString();
        }

        private int FindBestPath(Dictionary<Point, Point> portals, Dictionary<Point, Dictionary<Point, int>> paths, Point start, Point end)
        {
            var q = new SimplePriorityQueue<(Point pos, int steps), int>();

            q.Enqueue((start, 0), 0);

            var seen = new HashSet<Point>();

            while (q.Any())
            {
                var (pos, steps) = q.Dequeue();

                if (pos == end)
                {
                    return steps;
                }

                if (!seen.Contains(pos))
                {
                    seen.Add(pos);

                    foreach (var path in paths[pos])
                    {
                        q.Enqueue((path.Key, steps + path.Value), path.Value);
                    }
                }
            }

            throw new Exception("Path not found");
        }

        private Dictionary<Point, Dictionary<Point, int>> GetPaths(char[,] map, List<(string name, Point pos)> portalPoints)
        {
            var points = new HashSet<Point>(portalPoints.Select(p => p.pos));
            var result = new Dictionary<Point, Dictionary<Point, int>>();
            
            foreach (var p in points)
            {
                var paths = map.FindShortestPaths(c => c == '.', p)
                               .Where(x => points.Contains(x.Key) && x.Key != p)
                               .ToList();

                var dict = new Dictionary<Point, int>();
                paths.ForEach(x => dict.Add(x.Key, x.Value));
                
                var portalName = portalPoints.Single(x => x.pos == p).name;
                var otherPortal = portalPoints.FirstOrDefault(x => x.name == portalName && x.pos != p);

                if (otherPortal != default)
                {
                    dict.Add(otherPortal.pos, 1);
                }

                result.Add(p, dict);
            }

            return result;
        }

        private Dictionary<Point, Point> GetPortals(char[,] map, List<(string name, Point pos)> points)
        {
            var result = new Dictionary<Point, Point>();

            var groups = points.GroupBy(p => p.name).Where(g => g.Key != "AA" && g.Key != "ZZ").ToList();

            foreach (var g in groups)
            {
                result.Add(g.First().pos, g.Last().pos);
                result.Add(g.Last().pos, g.First().pos);
            }

            return result;
        }

        private IEnumerable<(string name, Point pos)> GetPortalPoints(char[,] map)
        {
            for (var x = 2; x <= map.GetUpperBound(0) - 2; x++)
            {
                if (map[x, 2] == '.')
                {
                    yield return ($"{map[x, 0]}{map[x, 1]}", new Point(x, 2));
                }

                if (map[x, map.GetUpperBound(1) - 2] == '.')
                {
                    yield return ($"{map[x, map.GetUpperBound(1) - 1]}{map[x, map.GetUpperBound(1)]}", new Point(x, map.GetUpperBound(1) - 2));
                }
            }

            for (var y = 2; y <= map.GetUpperBound(1) - 2; y++)
            {
                if (map[2, y] == '.')
                {
                    yield return ($"{map[0, y]}{map[1, y]}", new Point(2, y));
                }

                if (map[map.GetUpperBound(0) - 2, y] == '.')
                {
                    yield return ($"{map[map.GetUpperBound(0) - 1, y]}{map[map.GetUpperBound(0), y]}", new Point(map.GetUpperBound(0) - 2, y));
                }
            }

            // inner corners
            // 36, 36
            // 96, 36
            // 36, 90
            // 96, 90

            for (var x = 36; x <= 96; x++)
            {
                if (map[x, 36] == '.')
                {
                    yield return ($"{map[x, 37]}{map[x, 38]}", new Point(x, 36));
                }

                if (map[x, 90] == '.')
                {
                    yield return ($"{map[x, 88]}{map[x, 89]}", new Point(x, 90));
                }
            }

            for (var y = 36; y <= 90; y++)
            {
                if (map[36, y] == '.')
                {
                    yield return ($"{map[37, y]}{map[38, y]}", new Point(36, y));
                }

                if (map[96, y] == '.')
                {
                    yield return ($"{map[94, y]}{map[95, y]}", new Point(96, y));
                }
            }
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
