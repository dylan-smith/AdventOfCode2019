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
        public override string PartOne(string input)
        {
            var map = input.CreateCharGrid();

            var root = new KeyTree
            {
                Key = '@',
                Steps = 0,
                Pos = GetStartPos(map),
            };

            root.FindChildren(0, map, GetKeys(map));

            return FindBestPath(root).ToString();
        }

        private int FindBestPath(KeyTree tree)
        {
            if (tree.Children != null && tree.Children.Any())
            {
                return tree.Steps + tree.Children.Min(c => FindBestPath(c));
            }

            return tree.Steps;
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

        private class KeyTree
        {
            public char Key { get; set; }
            public int Steps { get; set; }
            public Point Pos { get; set; }
            public static int BEST = int.MaxValue;

            public List<KeyTree> Children { get; set; }

            public void FindChildren(int previousSteps, char[,] map, Dictionary<char, Point> keys)
            {
                if (!keys.Any())
                {
                    if (previousSteps + Steps < BEST)
                    {
                        BEST = previousSteps + Steps;
                        Debug.WriteLine(BEST);
                    }
                    return;
                }

                var paths = map.FindShortestPaths(c => c == '.', Pos).Where(p => keys.ContainsValue(p.Key)).ToList();
                paths.Sort((x, y) => x.Value.CompareTo(y.Value));

                Children = new List<KeyTree>();

                foreach (var p in paths)
                {
                    var child = new KeyTree
                    {
                        Key = keys.First(x => x.Value == p.Key).Key,
                        Steps = p.Value,
                        Pos = p.Key,
                    };

                    Children.Add(child);

                    var doors = map.GetPoints().Where(z => map[z.X, z.Y] == (char)(child.Key - 32)).ToList();
                    doors.ForEach(d => map[d.X, d.Y] = '.');
                    keys.Remove(child.Key);

                    child.FindChildren(previousSteps + Steps, map, keys);

                    doors.ForEach(d => map[d.X, d.Y] = (char)(child.Key - 32));
                    keys.Add(child.Key, child.Pos);
                }
            }
        }
    }
}
