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
            return (double)(p.Y - from.Y) / (double)(p.X - from.X);
        }

        public override string PartTwo(string input)
        {
            _grid = input.CreateCharGrid();

            var station = _grid.GetPoints(x => _grid[x.X, x.Y] == '#').WithMax(x => CountVisibleAsteroids(x));

            var visible = _grid.GetPoints().Where(p => _grid[p.X, p.Y] == '#' && IsVisible(p, station) && p != station).ToList();

            var vaporized = 0;

            while (vaporized < 200)
            {
                var visibleN = visible.SingleOrDefault(p => p.X == station.X && p.Y < station.Y);
                var visibleNE = visible.Where(p => p.X > station.X && p.Y < station.Y).ToList();
                var visibleE = visible.SingleOrDefault(p => p.Y == station.Y && p.X > station.X);
                var visibleSE = visible.Where(p => p.X > station.X && p.Y > station.Y).ToList();
                var visibleS = visible.SingleOrDefault(p => p.X == station.X && p.Y > station.Y);
                var visibleSW = visible.Where(p => p.X < station.X && p.Y > station.Y).ToList();
                var visibleW = visible.SingleOrDefault(p => p.Y == station.Y && p.X < station.X);
                var visibleNW = visible.Where(p => p.X < station.X && p.Y < station.Y).ToList();

                if (visibleN != null)
                {
                    _grid[visibleN.X, visibleN.Y] = '*';
                    vaporized++;
                }

                var vapor = visibleNE.OrderBy(p => CalcSlope(p, station));

                foreach (var v in vapor)
                {
                    _grid[v.X, v.Y] = '*';
                    vaporized++;
                }

                if (visibleE != null)
                {
                    _grid[visibleE.X, visibleE.Y] = '*';
                    vaporized++;
                }

                vapor = visibleSE.OrderBy(p => CalcSlope(p, station));

                foreach (var v in vapor)
                {
                    _grid[v.X, v.Y] = '*';
                    vaporized++;
                }

                if (visibleS != null)
                {
                    _grid[visibleS.X, visibleS.Y] = '*';
                    vaporized++;
                }

                vapor = visibleSW.OrderBy(p => CalcSlope(p, station));

                foreach (var v in vapor)
                {
                    _grid[v.X, v.Y] = '*';
                    vaporized++;
                }

                if (visibleW != null)
                {
                    _grid[visibleW.X, visibleW.Y] = '*';
                    vaporized++;
                }

                vapor = visibleNW.OrderBy(p => CalcSlope(p, station));

                foreach (var v in vapor)
                {
                    _grid[v.X, v.Y] = '*';
                    vaporized++;
                }
            }

            return "blah";
        }
    }
}
