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
        private Dictionary<(Point a, Point b), (int distance, HashSet<Point> doors, HashSet<Point> keys)> _paths;
        private Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>> _pathsByStart;

        public override string PartOne(string input)
        {
            var map = input.CreateCharGrid();

            var startPos = GetStartPos(map);
            var keyMap = GetKeyMap(map);

            //map = RestrictMap(map, keyMap, startPos);

            _paths = GetPaths(map, keyMap, startPos);

            var keyPoints = keyMap.Select(k => k.Key).Append(startPos).ToList();
            _pathsByStart = new Dictionary<Point, Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>>();

            foreach (var key in keyPoints)
            {
                var keyPaths = _paths.Where(p => p.Key.a == key).ToList();
                var keyDict = new Dictionary<Point, (int distance, HashSet<Point> doors, HashSet<Point> keys)>();

                keyPaths.ForEach(p => keyDict.Add(p.Key.b, p.Value));

                _pathsByStart.Add(key, keyDict);
            }

            Log(map.GetString());

            //var paths = newMap.FindShortestPaths(c => c == '.', startPos).ToList();

            //ImageHelper.CreateBitmap(newMap.GetUpperBound(0) + 1, newMap.GetUpperBound(1) + 1, @"C:\AdventOfCode\Day18New.bmp", (x,y) =>
            //{
            //    if (keyMap.Any(k => k.Key.X == x && k.Key.Y == y))
            //    {
            //        return Color.Red;
            //    }

            //    if (keyMap.Any(k => k.Value.X == x && k.Value.Y == y))
            //    {
            //        return Color.Blue;
            //    }

            //    if (newMap[x, y] == '#')
            //    {
            //        return Color.Black;
            //    }

            //    if (newMap[x,y] == '.')
            //    {
            //        if (paths.Any(p => p.Key.X == x && p.Key.Y == y && p.Value >= 0))
            //        {
            //            return Color.Pink;
            //        }

            //        return Color.White;
            //    }

            //    return Color.Beige;
            //});

            //FindPath(0, startPos, map, keyMap);
            FindPath2(0, startPos, new HashSet<Point>(keyMap.Select(k => k.Key).ToList()));

            return BEST.ToString();
        }

        private char[,] RestrictMap(char[,] map, Dictionary<Point, Point> keyMap, Point startPos)
        {
            var newMap = map.Clone(c => c);

            keyMap.ForEach(k => newMap[k.Value.X, k.Value.Y] = '.');

            var keyPoints = keyMap.Select(k => k.Key).Append(startPos).ToList();

            var points = GetValidPath(newMap, keyPoints);

            foreach (var p in newMap.GetPoints().ToList())
            {
                if (!points.Any(z => z == p))
                {
                    newMap[p.X, p.Y] = '#';
                }
            }

            keyMap.ForEach(k => newMap[k.Value.X, k.Value.Y] = '#');

            return newMap;
        }

        private Dictionary<(Point a, Point b), (int distance, HashSet<Point> doors, HashSet<Point> keys)> GetPaths(char[,] map, Dictionary<Point, Point> keyMap, Point startPos)
        {
            var newMap = map.Clone(c => c);

            keyMap.ForEach(k => newMap[k.Value.X, k.Value.Y] = '.');

            var keyPoints = keyMap.Select(k => k.Key).Append(startPos).ToList();

            var paths = GetPaths(newMap, keyPoints, keyMap);

            //keyMap.ForEach(k => newMap[k.Value.X, k.Value.Y] = '#');

            return paths;
        }

        private Dictionary<(Point a, Point b), (int distance, HashSet<Point> doors, HashSet<Point> keys)> GetPaths(char[,] map, List<Point> keyPoints, Dictionary<Point, Point> keyMap)
        {
            var result = new Dictionary<(Point a, Point b), (int distance, HashSet<Point> doors, HashSet<Point> keys)>();

            foreach (var combo in keyPoints.GetCombinations(2))
            {
                var path = GetShortestPath(map, combo.First(), combo.Last(), new HashSet<Point>());
                var doors = path.Where(p => keyMap.Any(k => k.Value == p)).Select(p => keyMap.Single(k => k.Value == p).Key).ToList();
                var keys = path.Where(p => keyMap.Any(k => k.Key == p)).Where(p => p != combo.First() && p != combo.Last()).ToList();

                result.Add((combo.First(), combo.Last()), (path.Count - 1, new HashSet<Point>(doors), new HashSet<Point>(keys)));
            }

            return result;
        }

        private HashSet<Point> GetValidPath(char[,] map, List<Point> keyPoints)
        {
            var result = new HashSet<Point>();

            foreach (var combo in keyPoints.GetCombinations(2))
            {
                var path = GetShortestPath(map, combo.First(), combo.Last(), new HashSet<Point>());
                result.AddRange(path);
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
                map[door.X, door.Y] = '#';
            }

            return result;
        }

        //public void FindPath(int steps, Point pos, chargot [,] map, Dictionary<Point, Point> keys)
        //{
        //    if (!keys.Any())
        //    {
        //        if (steps < BEST)
        //        {
        //            BEST = steps;
        //            Log(BEST.ToString());
        //        }
        //        return;
        //    }

        //    if (keys.Count == 23)
        //    {
        //        Log($"XXX - {steps}");
        //    }

        //    var paths = map.FindShortestPaths(c => c == '.', pos).Where(p => keys.ContainsKey(p.Key)).ToList();
        //    paths.Sort((x, y) => x.Value.CompareTo(y.Value));

        //    foreach (var p in paths)
        //    {
        //        var door = keys[p.Key];
        //        map[door.X, door.Y] = '.';
        //        keys.Remove(p.Key);

        //        FindPath(steps + p.Value, p.Key, map, keys);

        //        map[door.X, door.Y] = '#';
        //        keys.Add(p.Key, door);
        //    }
        //}

        public void FindPath2(int steps, Point pos, HashSet<Point> keys)
        {
            if (steps >= BEST) return;

            if (!keys.Any())
            {
                if (steps < BEST)
                {
                    BEST = steps;
                    Log(BEST.ToString());
                }
                return;
            }

            if (keys.Count == 23)
            {
                Log($"{keys.Count} - {steps}");
            }

            //var paths = _paths.Where(p => p.Key.a == pos)
            //                  .Where(p => keys.Contains(p.Key.b))
            //                  .Where(p => !p.Value.doors.Any(d => keys.Contains(d)))
            //                  .OrderBy(p => p.Value.distance)
            //                  .ToList();

            //var paths = _paths.Where(p => p.Key.a == pos).ToList();
            //var paths = _pathsByStart[pos].Where(p => keys.Contains(p.Key)).ToList();
            //var paths = keys.Select(k => (dest: k, details: _pathsByStart[pos][k])).ToList();
            //paths = paths.Where(p => !p.details.doors.Any(d => keys.Contains(d))).ToList();
            //paths = paths.OrderBy(p => p.details.distance).ToList();

            var paths = keys.Select(k => (dest: k, details: _pathsByStart[pos][k]))
                            .Where(p => !p.details.doors.Any(d => keys.Contains(d)))
                            .OrderBy(p => p.details.distance)
                            .ToList();

            foreach (var (dest, details) in paths)
            {
                keys.Remove(dest);
                var removedKeys = keys.Where(k => details.keys.Contains(k)).ToList();
                removedKeys.ForEach(r => keys.Remove(r));

                FindPath2(steps + details.distance, dest, keys);

                keys.Add(dest);
                keys.AddRange(removedKeys);
            }
        }

        private Point GetStartPos(char[,] map)
        {
            var result = map.GetPoints().Single(p => map[p.X, p.Y] == '@');

            map[result.X, result.Y] = '.';

            return result;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
