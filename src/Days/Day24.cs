using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 24)]
    public class Day24 : BaseDay
    {
        public override string PartOne(string input)
        {
            var grid = input.CreateCharGrid();

            var diversity = GetBiodiversityRating(grid);

            var seen = new HashSet<int>();

            while (!seen.Contains(diversity))
            {
                seen.Add(diversity);

                grid = NextMinute(grid);
                diversity = GetBiodiversityRating(grid);
            }

            return diversity.ToString();
        }

        private char[,] NextMinute(char[,] grid)
        {
            var result = grid.Clone(c => c);

            foreach (var p in grid.GetPoints())
            {
                var bugCount = grid.GetNeighbors(p.X, p.Y, false).Count(c => c == '#');

                if (grid[p.X, p.Y] == '#')
                {
                    result[p.X, p.Y] = bugCount == 1 ? '#' : '.';
                }

                if (grid[p.X, p.Y] == '.')
                {
                    result[p.X, p.Y] = bugCount == 1 || bugCount == 2 ? '#' : '.';
                }
            }

            return result;
        }

        private int GetBiodiversityRating(char[,] grid)
        {
            return (int)grid.GetPoints('#').Sum(p => Math.Pow(2, (p.X) + (p.Y * 5)));
        }

        public override string PartTwo(string input)
        {
            throw new NotImplementedException();
        }
    }
}
