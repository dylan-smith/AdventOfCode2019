using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            FindPath(0, GetStartPos(map), map, GetKeys(map));

            return BEST.ToString();
        }

        public void FindPath(int steps, Point pos, char[,] map, Dictionary<char, Point> keys)
        {
            if (!keys.Any())
            {
                if (steps < BEST)
                {
                    BEST = steps;
                    Debug.WriteLine(BEST);
                }
                return;
            }

            var paths = map.FindShortestPaths(c => c == '.', pos).Where(p => keys.ContainsValue(p.Key)).ToList();
            paths.Sort((x, y) => x.Value.CompareTo(y.Value));

            foreach (var p in paths)
            {
                var key = keys.First(x => x.Value == p.Key).Key;

                var doors = map.GetPoints().Where(z => map[z.X, z.Y] == (char)(key - 32)).ToList();
                doors.ForEach(d => map[d.X, d.Y] = '.');
                keys.Remove(key);

                FindPath(steps + p.Value, p.Key, map, keys);

                doors.ForEach(d => map[d.X, d.Y] = (char)(key - 32));
                keys.Add(key, p.Key);
            }
        }

        private Point GetStartPos(char[,] map)
        {
            var result = map.GetPoints().Single(p => map[p.X, p.Y] == '@');

            map[result.X, result.Y] = '.';

            return result;
        }

        private Dictionary<char, Point> GetKeys(char[,] map)
        {
            var result = new Dictionary<char, Point>();

            var keys = map.GetPoints().Where(p => map[p.X, p.Y] >= 'a' && map[p.X, p.Y] <= 'z').ToList();

            foreach (var k in keys)
            {
                result.Add(map[k.X, k.Y], k);
                map[k.X, k.Y] = '.';
            }

            return result;
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
