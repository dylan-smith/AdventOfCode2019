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

        public override string PartOne(string input)
        {
            var map = input.CreateCharGrid();

            var startPos = GetStartPos(map);
            var keyMap = GetKeyMap(map);

            var paths = map.FindShortestPaths(c => c == '.', startPos).ToList();

            ImageHelper.CreateBitmap(map.GetUpperBound(0) + 1, map.GetUpperBound(1) + 1, @"C:\AdventOfCode\Day18.bmp", (x,y) =>
            {
                if (keyMap.Any(k => k.Key.X == x && k.Key.Y == y))
                {
                    return Color.Red;
                }

                if (keyMap.Any(k => k.Value.X == x && k.Value.Y == y))
                {
                    return Color.Blue;
                }

                if (map[x, y] == '#')
                {
                    return Color.Black;
                }

                if (map[x,y] == '.')
                {
                    if (paths.Any(p => p.Key.X == x && p.Key.Y == y && p.Value >= 0))
                    {
                        return Color.Pink;
                    }

                    return Color.White;
                }

                return Color.Beige;
            });

            FindPath(0, startPos, map, GetKeyMap(map));

            return BEST.ToString();
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

        public void FindPath(int steps, Point pos, char[,] map, Dictionary<Point, Point> keys)
        {
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
                Log($"XXX - {steps}");
            }

            var paths = map.FindShortestPaths(c => c == '.', pos).Where(p => keys.ContainsKey(p.Key)).ToList();
            paths.Sort((x, y) => x.Value.CompareTo(y.Value));

            foreach (var p in paths)
            {
                var door = keys[p.Key];
                map[door.X, door.Y] = '.';
                keys.Remove(p.Key);

                FindPath(steps + p.Value, p.Key, map, keys);

                map[door.X, door.Y] = '#';
                keys.Add(p.Key, door);
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
