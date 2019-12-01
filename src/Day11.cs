using System;

namespace AdventOfCode
{
    public class Day11
    {
        private const int GRID_SERIAL = 2568;
        private const int GRID_SIZE = 300;

        public static string PartOne(string input)
        {
            var grid = BuildGrid();
            int maxPower = int.MinValue, maxX = 0, maxY = 0;

            for (var y = 1; y <= GRID_SIZE - 2; y++)
            {
                for (var x = 1; x <= GRID_SIZE - 2; x++)
                {
                    var power = GetSquarePower(x, y, grid, 3);

                    if (power > maxPower)
                    {
                        maxPower = power;
                        maxX = x;
                        maxY = y;
                    }
                }
            }

            return $"{maxX},{maxY}";
        }

        private static int GetPowerLevel(int x, int y)
        {
            var rackId = (x + 10);
            var result = ((rackId * y) + GRID_SERIAL) * rackId;
            return ((result / 100) % 10) - 5;
        }

        private static int GetSquarePower(int x, int y, (int power, int square)[,] grid, int size)
        {
            return grid[x - 1, y - 1].square + 
                grid[x + size - 1, y + size - 1].square - 
                grid[x - 1, y + size - 1].square - 
                grid[x + size - 1, y - 1].square;
        }

        public static string PartTwo(string input)
        {
            var grid = BuildGrid();
            int maxPower = int.MinValue, maxX = 0, maxY = 0, maxSize = 0;

            for (var y = 1; y <= GRID_SIZE; y++)
            {
                for (var x = 1; x <= GRID_SIZE; x++)
                {
                    var sizeLimit = Math.Min(GRID_SIZE - x + 1, GRID_SIZE - y + 1);

                    for (var size = 1; size <= sizeLimit; size++)
                    {
                        var power = GetSquarePower(x, y, grid, size);

                        if (power > maxPower)
                        {
                            maxPower = power;
                            maxX = x;
                            maxY = y;
                            maxSize = size;
                        }
                    }
                }
            }

            return $"{maxX},{maxY},{maxSize}";
        }

        private static (int power, int square)[,] BuildGrid()
        {
            var grid = new (int power, int square)[GRID_SIZE + 1, GRID_SIZE + 1];

            for (var y = 1; y <= GRID_SIZE; y++)
            {
                for (var x = 1; x <= GRID_SIZE; x++)
                {
                    grid[x, y] = GetGridEntry(x, y, grid);
                }
            }

            return grid;
        }

        private static (int power, int square) GetGridEntry(int x, int y, (int power, int square)[,] grid)
        {
            var power = GetPowerLevel(x, y);
            return (power, grid[x - 1, y].square + grid[x, y - 1].square - grid[x - 1, y - 1].square + power);
        }
    }
}