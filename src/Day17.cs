using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day17
    {
        private static int _minY;

        public static string PartOne(string input)
        {
            var veins = GetVeins(input).ToList();
            var grid = MakeGrid(veins);

            FlowWater(grid, 500, 0);

            return (grid.Count('|') + grid.Count('~') - _minY).ToString();
        }

        private static char[,] MakeGrid(List<(char axis, int axisValue, int offAxisStart, int offAxisEnd)> veins)
        {
            var maxX = Math.Max(veins.Where(v => v.axis == 'x').Max(v => v.axisValue), veins.Where(v => v.axis == 'y').Max(v => v.offAxisEnd));
            var maxY = Math.Max(veins.Where(v => v.axis == 'y').Max(v => v.axisValue), veins.Where(v => v.axis == 'x').Max(v => v.offAxisEnd));
            _minY = Math.Min(veins.Where(v => v.axis == 'y').Min(v => v.axisValue), veins.Where(v => v.axis == 'x').Min(v => v.offAxisStart));

            var grid = new char[maxX + 1, maxY + 1];
            grid.Replace(default(char), '.');

            ApplyVeins(grid, veins);

            return grid;
        }

        private static IEnumerable<(char axis, int axisValue, int offAxisStart, int offAxisEnd)> GetVeins(string input)
        {
            foreach (var line in input.Lines())
            {
                var words = line.Words().ToList();
                var axis = line[0];
                var axisValue = int.Parse(words[0].Substring(2));
                var offAxisStart = int.Parse(words[1].Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries)[0].ShaveLeft(2));
                var offAxisEnd = int.Parse(words[1].Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries)[1]);

                yield return (axis, axisValue, offAxisStart, offAxisEnd);
            }
        }

        private static void ApplyVeins(char[,] grid, List<(char axis, int axisValue, int offAxisStart, int offAxisEnd)> veins)
        {
            foreach (var (axis, axisValue, offAxisStart, offAxisEnd) in veins)
            {
                if (axis == 'x')
                {
                    for (var y = offAxisStart; y <= offAxisEnd; y++)
                    {
                        grid[axisValue, y] = '#';
                    }
                }

                if (axis == 'y')
                {
                    for (var x = offAxisStart; x <= offAxisEnd; x++)
                    {
                        grid[x, axisValue] = '#';
                    }
                }
            }
        }

        private static void FlowWater(char[,] grid, int waterX, int waterY)
        {
            var curX = waterX;
            var curY = waterY;

            while (grid[curX, curY] == '.' || grid[curX, curY] == '|')
            {
                grid[curX, curY] = '|';
                curY += 1;

                if (curY > grid.GetUpperBound(1))
                {
                    return;
                }
            }

            curY -= 1;

            var clayLeft = int.MaxValue;
            var clayRight = int.MaxValue;

            for (var x = curX; x >= 0; x--)
            {
                if (grid[x, curY] == '#')
                {
                    clayLeft = x;
                    break;
                }

                if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                {
                    break;
                }
            }

            for (var x = curX; x <= grid.GetUpperBound(0); x++)
            {
                if (grid[x, curY] == '#')
                {
                    clayRight = x;
                    break;
                }

                if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                {
                    break;
                }
            }

            if (clayLeft != int.MaxValue && clayRight != int.MaxValue)
            {
                for (var x = clayLeft + 1; x < clayRight; x++)
                {
                    grid[x, curY] = '~';
                }

                FlowWater(grid, curX, curY - 1);
            }

            if (clayLeft == int.MaxValue && clayRight != int.MaxValue)
            {
                for (var x = clayRight - 1; x >= 0; x--)
                {
                    grid[x, curY] = '|';

                    if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                    {
                        FlowWater(grid, x, curY);
                        break;
                    }
                }
            }

            if (clayLeft != int.MaxValue && clayRight == int.MaxValue)
            {
                for (var x = clayLeft + 1; x <= grid.GetUpperBound(0); x++)
                {
                    grid[x, curY] = '|';

                    if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                    {
                        FlowWater(grid, x, curY);
                        break;
                    }
                }
            }

            if (clayLeft == int.MaxValue && clayRight == int.MaxValue)
            {
                for (var x = curX; x <= grid.GetUpperBound(0); x++)
                {
                    grid[x, curY] = '|';

                    if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                    {
                        FlowWater(grid, x, curY);
                        break;
                    }
                }

                for (var x = curX; x >= 0; x--)
                {
                    if (grid[x, curY] == '~')
                    {
                        break;
                    }

                    grid[x, curY] = '|';

                    if (grid[x, curY + 1] == '.' || grid[x, curY + 1] == '|')
                    {
                        FlowWater(grid, x, curY);
                        break;
                    }
                }
            }
        }

        public static string PartTwo(string input)
        {
            var veins = GetVeins(input).ToList();
            var grid = MakeGrid(veins);

            FlowWater(grid, 500, 0);

            return grid.ToList().Count(x => x == '~').ToString();
        }
    }
}