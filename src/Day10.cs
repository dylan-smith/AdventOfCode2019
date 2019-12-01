using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day10
    {
        public static string PartOne(string input)
        {
            var points = input.Lines().Select(x => ParseLine(x)).ToList();
            var lastArea = long.MaxValue;

            while (true)
            {
                var area = GetArea(points);

                if (area > lastArea)
                {
                    points = MoveBackward(points);
                    ShowPoints(points);
                    return @"Look in C:\AoC\Day10.txt for the answer";
                }

                points = MoveForward(points);
                lastArea = area;
            }
        }

        private static long GetArea(List<(Point location, Point velocity)> points)
        {
            var minX = points.Min(p => p.location.X);
            var maxX = points.Max(p => p.location.X);
            var minY = points.Min(p => p.location.Y);
            var maxY = points.Max(p => p.location.Y);

            return (long)(maxX - minX + 1) * (long)(maxY - minY + 1);
        }

        private static void ShowPoints(List<(Point location, Point velocity)> points)
        {
            var result = new StringBuilder();

            for (var y = points.Min(p => p.location.Y); y <= points.Max(p => p.location.Y); y++)
            {
                for (var x = points.Min(p => p.location.X); x <= points.Max(p => p.location.X); x++)
                {
                    if (points.Any(p => p.location.X == x && p.location.Y == y))
                    {
                        result.Append('X');
                    }
                    else
                    {
                        result.Append(' ');
                    }
                }

                result.Append(Environment.NewLine);
            }

            File.WriteAllText(@"C:\AoC\Day10.txt", result.ToString());
        }

        private static List<(Point location, Point velocity)> MoveForward(List<(Point location, Point velocity)> points)
        {
            return points.Select(p => (new Point(p.location.X + p.velocity.X, p.location.Y + p.velocity.Y), p.velocity)).ToList();
        }

        private static List<(Point location, Point velocity)> MoveBackward(List<(Point location, Point velocity)> points)
        {
            return points.Select(p => (new Point(p.location.X - p.velocity.X, p.location.Y - p.velocity.Y), p.velocity)).ToList();
        }

        private static (Point location, Point velocity) ParseLine(string input)
        {
            var g = Regex.Match(input, @"position=\< *(.+), *(.+)\> velocity=\< *(.*), *(.+)\>").Groups;

            return (new Point(int.Parse(g[1].Value), int.Parse(g[2].Value)), new Point(int.Parse(g[3].Value), int.Parse(g[4].Value)));
        }

        public static string PartTwo(string input)
        {
            var points = input.Lines().Select(x => ParseLine(x)).ToList();
            var seconds = 0;
            var lastArea = long.MaxValue;

            while (true)
            {
                var area = GetArea(points);

                if (area > lastArea)
                {
                    points = MoveBackward(points);
                    ShowPoints(points);
                    return (seconds - 1).ToString();
                }

                points = MoveForward(points);
                lastArea = area;
                seconds++;
            }
        }
    }
}