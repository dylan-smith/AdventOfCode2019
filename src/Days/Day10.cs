using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 10)]
    public class Day10 : BaseDay
    {
        private char[,] _grid;

        public override string PartOne(string input)
        {
            _grid = input.CreateCharGrid();

            var station = _grid.GetPoints(x => _grid[x.X, x.Y] == '#').WithMax(x => CountVisibleAsteroids(x));

            return (CountVisibleAsteroids(station) - 1).ToString();
        }

        private int CountVisibleAsteroids(Point from)
        {
            return _grid.GetPoints().Count(p => _grid[p.X, p.Y] == '#' && IsVisible(p, from));
        }

        private bool IsVisible(Point p, Point from)
        {
            var slope = CalcSlope(p, from);
            var distance = CalcDistance(p, from);

            var maybeBlockers = _grid.GetPoints().Where(x => _grid[x.X, x.Y] == '#' &&
                                        CalcSlope(x, from) == slope &&
                                        CalcDistance(x, from) < distance).ToList();

            foreach (var b in maybeBlockers)
            {
                if (p.X > from.X && b.X > from.X)
                {
                    return false;
                }

                if (p.X < from.X && b.X < from.X)
                {
                    return false;
                }

                if (p.X == from.X)
                {
                    if (p.Y > from.Y && b.Y > from.Y)
                    {
                        return false;
                    }

                    if (p.Y < from.Y && b.Y < from.Y)
                    {
                        return false;
                    }
                }
            }


            //if (_grid.GetPoints().Any(x => _grid[x.X, x.Y] == '#' && 
            //                            CalcSlope(x, from) == slope && 
            //                            CalcDistance(x, from) < distance))
            //{
            //    var blocker = _grid.GetPoints().First(x => _grid[x.X, x.Y] == '#' && CalcSlope(x, from) == slope && CalcDistance(x, from) < distance);

                
            //    return false;
            //}

            return true;
        }

        private double CalcDistance(Point p, Point from)
        {
            return Math.Sqrt(Math.Abs(p.X - from.X) * Math.Abs(p.X - from.X) + Math.Abs(p.Y - from.Y) * Math.Abs(p.Y - from.Y));
        }

        private double CalcSlope(Point p, Point from)
        {
            return (double)(p.X - from.X) / (double)(p.Y - from.Y);
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
