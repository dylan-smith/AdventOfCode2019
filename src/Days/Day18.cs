using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 18)]
    public class Day18 : BaseDay
    {
        private int BEST = int.MaxValue;
        private Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>> _pathsByStart;

        public override string PartOne(string input)
        {
            var map = input.CreateCharGrid();

            var startPos = GetStartPos(map);
            var keyMap = GetKeyMap(map);

            _pathsByStart = GetPaths(map, keyMap, startPos);

            FindPath(0, startPos, new HashSet<Point>(keyMap.Select(k => k.Key)));

            return BEST.ToString();
        }

        private Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>> GetPaths(char[,] map, Dictionary<Point, Point> keyMap, Point startPos)
        {
            var keyPoints = keyMap.Select(k => k.Key).Append(startPos).ToList();

            return GetPaths(map, keyMap, keyPoints);
        }

        private Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>> GetPaths(char[,] map, Dictionary<Point, Point> keyMap, Point[] startPos)
        {
            var keyPoints = keyMap.Select(k => k.Key).Concat(startPos).ToList();

            return GetPaths(map, keyMap, keyPoints);
        }

        private Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>> GetPaths(char[,] map, Dictionary<Point, Point> keyMap, List<Point> keyPoints)
        {
            var result = new Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>>();
            Log(map.GetString());

            foreach (var combo in keyPoints.GetCombinations(2))
            {
                var path = GetShortestPath(map, combo.First(), combo.Last(), new HashSet<Point>());

                if (path != null)
                {
                    var doors = path.Where(p => keyMap.Any(k => k.Value == p)).Select(p => keyMap.Single(k => k.Value == p).Key).ToList();
                    var keys = path.Where(p => keyMap.Any(k => k.Key == p)).Where(p => p != combo.First() && p != combo.Last()).ToList();
                    var dict = new Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>();

                    if (!result.ContainsKey(combo.First()))
                    {
                        result.Add(combo.First(), new Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>());
                    }

                    result[combo.First()].Add(combo.Last(), (path.Count - 1, new HashSet<Point>(doors), new HashSet<Point>(keys)));
                }
            }

            return result;
        }

        private List<Point> GetShortestPath(char[,] map, Point a, Point b, HashSet<Point> visited)
        {
            var result = new List<Point>() { a };

            if (a == b)
            {
                return result;
            }

            var neighbors = map.GetNeighborPoints(a.X, a.Y).Where(c => c.c == '.' && !visited.Contains(c.point)).ToList();

            visited.Add(a);
            var paths = neighbors.Select(n => GetShortestPath(map, n.point, b, visited)).Where(p => p != null).ToList();
            visited.Remove(a);

            if (paths.Any())
            {
                result.AddRange(paths.WithMin(p => p.Count));
                return result;
            }

            return null;
        }

        private Dictionary<Point, Point> GetKeyMap(char[,] map)
        {
            var result = new Dictionary<Point, Point>();

            var keys = map.GetPoints().Where(p => map[p.X, p.Y] >= 'a' && map[p.X, p.Y] <= 'z').ToList();

            foreach (var k in keys)
            {
                var door = map.GetPoints().Single(p => map[p.X, p.Y] == (char)(map[k.X, k.Y] - 32));
                result.Add(k, door);
                map[k.X, k.Y] = '.';
                map[door.X, door.Y] = '.';
            }

            return result;
        }

        public void FindPath(int steps, Point pos, HashSet<Point> keysLeft)
        {
            if (steps >= BEST) return;

            if (!keysLeft.Any())
            {
                if (steps < BEST)
                {
                    BEST = steps;
                    Log(BEST.ToString());
                }
                return;
            }

            if (keysLeft.Count == 23)
            {
                Log($"{keysLeft.Count} - {steps}");
            }

            var paths = keysLeft.Select(k => (dest: k, details: _pathsByStart[pos][k]))
                                .Where(p => !p.details.doors.Any(d => keysLeft.Contains(d)))
                                .OrderBy(p => p.details.distance)
                                .ToList();

            foreach (var (dest, details) in paths)
            {
                keysLeft.Remove(dest);
                var removedKeys = keysLeft.Where(k => details.keys.Contains(k)).ToList();
                removedKeys.ForEach(r => keysLeft.Remove(r));

                FindPath(steps + details.distance, dest, keysLeft);

                keysLeft.Add(dest);
                keysLeft.AddRange(removedKeys);
            }
        }

        public void FindPath(int steps, Point[] pos, HashSet<Point> keysLeft)
        {
            if (steps >= BEST) return;

            if (!keysLeft.Any())
            {
                if (steps < BEST)
                {
                    BEST = steps;
                    Log(BEST.ToString());
                }
                return;
            }

            if (keysLeft.Count == 23)
            {
                Log($"{keysLeft.Count} - {steps}");
            }

            for (var i = 0; i <= 3; i++)
            {
                var paths = _pathsByStart[pos[i]].Where(p => keysLeft.Contains(p.Key))
                                    .Where(p => !p.Value.doors.Any(d => keysLeft.Contains(d)))
                                    .OrderBy(p => p.Value.distance)
                                    .ToList();

                foreach (var (dest, details) in paths)
                {
                    keysLeft.Remove(dest);
                    var removedKeys = keysLeft.Where(k => details.keys.Contains(k)).ToList();
                    removedKeys.ForEach(r => keysLeft.Remove(r));
                    var oldPos = pos[i];
                    pos[i] = dest;

                    FindPath(steps + details.distance, pos, keysLeft);

                    keysLeft.Add(dest);
                    keysLeft.AddRange(removedKeys);
                    pos[i] = oldPos;
                }
            }
        }

        private Point GetStartPos(char[,] map)
        {
            var result = map.GetPoints().Single(p => map[p.X, p.Y] == '@');

            map[result.X, result.Y] = '.';

            return result;
        }

        private Point[] GetStartPos2(char[,] map)
        {
            var result = map.GetPoints().Where(p => map[p.X, p.Y] == '@').ToList();

            result.ForEach(r => map[r.X, r.Y] = '.');

            return result.ToArray();
        }

        public override string PartTwo(string input)
        {
            var map = input.CreateCharGrid();

            map[39, 39] = '@';
            map[39, 40] = '#';
            map[39, 41] = '@';
            map[40, 39] = '#';
            map[40, 40] = '#';
            map[40, 41] = '#';
            map[41, 39] = '@';
            map[41, 40] = '#';
            map[41, 41] = '@';

            var startPos = GetStartPos2(map);
            var keyMap = GetKeyMap(map);

            _pathsByStart = GetPaths(map, keyMap, startPos);

            FindPath(0, startPos, new HashSet<Point>(keyMap.Select(k => k.Key)));

            return BEST.ToString();
        }
    }
}
