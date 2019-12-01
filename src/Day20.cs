using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day20
    {
        public static string PartOne(string input)
        {
            var map = new char[2000, 2000];
            map.Replace(default(char), '?');

            var start = new Point(1000, 1000);
            map[start.X, start.Y] = '.';

            ProcessDirections(input.Trim().Shave(1), map, start);
            map.Replace('?', '#');

            return (map.FindShortestPaths(c => c == '.' || c == '|' || c == '-', start).Max(x => x.Value) / 2).ToString();
        }

        private static (List<Point> points, int chars) ProcessDirections(string directions, char[,] map, Point pos)
        {
            var startPos = pos;
            var branches = new List<List<Point>>() { new List<Point>() { pos } };
            var curBranch = 0;

            for (var i = 0; i < directions.Length; i++)
            {
                switch (directions[i])
                {
                    case 'E':
                        for (var p = 0; p < branches[curBranch].Count; p++)
                        {
                            map[branches[curBranch][p].X + 1, branches[curBranch][p].Y] = '|';
                            map[branches[curBranch][p].X + 2, branches[curBranch][p].Y] = '.';
                            map[branches[curBranch][p].X + 1, branches[curBranch][p].Y + 1] = '#';
                            map[branches[curBranch][p].X + 1, branches[curBranch][p].Y - 1] = '#';
                            branches[curBranch][p] = new Point(branches[curBranch][p].X + 2, branches[curBranch][p].Y);
                        }
                        break;
                    case 'W':
                        for (var p = 0; p < branches[curBranch].Count; p++)
                        {
                            map[branches[curBranch][p].X - 1, branches[curBranch][p].Y] = '|';
                            map[branches[curBranch][p].X - 2, branches[curBranch][p].Y] = '.';
                            map[branches[curBranch][p].X - 1, branches[curBranch][p].Y + 1] = '#';
                            map[branches[curBranch][p].X - 1, branches[curBranch][p].Y - 1] = '#';
                            branches[curBranch][p] = new Point(branches[curBranch][p].X - 2, branches[curBranch][p].Y);
                        }
                        break;
                    case 'N':
                        for (var p = 0; p < branches[curBranch].Count; p++)
                        {
                            map[branches[curBranch][p].X, branches[curBranch][p].Y - 1] = '-';
                            map[branches[curBranch][p].X, branches[curBranch][p].Y - 2] = '.';
                            map[branches[curBranch][p].X + 1, branches[curBranch][p].Y - 1] = '#';
                            map[branches[curBranch][p].X - 1, branches[curBranch][p].Y - 1] = '#';
                            branches[curBranch][p] = new Point(branches[curBranch][p].X, branches[curBranch][p].Y - 2);
                        }
                        break;
                    case 'S':
                        for (var p = 0; p < branches[curBranch].Count; p++)
                        {
                            map[branches[curBranch][p].X, branches[curBranch][p].Y + 1] = '-';
                            map[branches[curBranch][p].X, branches[curBranch][p].Y + 2] = '.';
                            map[branches[curBranch][p].X + 1, branches[curBranch][p].Y + 1] = '#';
                            map[branches[curBranch][p].X - 1, branches[curBranch][p].Y + 1] = '#';
                            branches[curBranch][p] = new Point(branches[curBranch][p].X, branches[curBranch][p].Y + 2);
                        }
                        break;
                    case ')':
                        return (branches.SelectMany(b => b).Distinct().ToList(), i + 1);
                    case '|':
                        branches.Add(new List<Point>() { startPos });
                        curBranch++;
                        break;
                    case '(':
                        var childDirections = directions.ShaveLeft(i + 1);
                        var (childPoints, childChars) = ProcessDirections(childDirections, map, branches[curBranch][0]);
                        i += childChars;

                        for (var p = 1; p < branches[curBranch].Count; p++)
                        {
                            childPoints.AddRange(ProcessDirections(childDirections, map, branches[curBranch][p]).points);
                        }

                        branches[curBranch] = childPoints.Distinct().ToList();
                        break;
                    default:
                        throw new System.Exception("should never happen");
                }
            }

            return (branches.SelectMany(b => b).ToList(), directions.Length);
        }

        public static string PartTwo(string input)
        {
            var map = new char[2000, 2000];
            map.Replace(default(char), '?');

            var start = new Point(1000, 1000);
            map[start.X, start.Y] = '.';

            ProcessDirections(input.Trim().Shave(1), map, start);
            map.Replace('?', '#');

            return map.FindShortestPaths(c => c == '.' || c == '|' || c == '-', start).Count(x => map[x.Key.X, x.Key.Y] == '.' && x.Value >= 2000).ToString();
        }
    }
}