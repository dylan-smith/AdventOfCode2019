using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day01
    {
        public static string PartOne(string input)
        {
            return input.Doubles().Sum(x => Math.Floor(x / 3) - 2).ToString();
        }

        public static string PartTwo(string input)
        {
            var total = 0.0;

            foreach (var m in input.Doubles())
            {
                var fuel = Math.Floor(m / 3) - 2;
                total += fuel;

                while (fuel > 0)
                {
                    fuel = Math.Floor(fuel / 3) - 2;
                    total += fuel > 0 ? fuel : 0;
                }
            }

            return total.ToString();
        }
    }
}