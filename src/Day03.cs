using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
    public class Day03
    {
        public static string PartOne(string input)
        {
            var rects = input.Lines().Select(x => ParseInput(x)).ToList();
            return GetOverlapPoints(rects).Count.ToString();
        }

        private static HashSet<Point> GetOverlapPoints(IEnumerable<(int Id, Rectangle Rect)> rects)
        {
            var overlapPoints = new HashSet<Point>();

            foreach (var c in rects.GetCombinations(2))
            {
                if (c.First().Rect.IntersectsWith(c.Last().Rect))
                {
                    var intersect = Rectangle.Intersect(c.First().Rect, c.Last().Rect);
                    intersect.GetPoints().ForEach(p => overlapPoints.Add(p));
                }
            }

            return overlapPoints;
        }

        private static (int Id, Rectangle Rect) ParseInput(string line)
        {
            var (id, left, top, width, height) = line.Split(new char[] { '#', ' ', '@', ',', 'x', ':' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            return (id, new Rectangle(left, top, width, height));
        }

        public static string PartTwo(string input)
        {
            var rects = input.Lines().Select(x => ParseInput(x)).ToList();
            var overlapPoints = GetOverlapPoints(rects);

            return rects.First(r => !r.Rect.GetPoints().Any(p => overlapPoints.Contains(p))).Id.ToString();
        }
    }
}