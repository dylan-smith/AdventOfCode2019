using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day01
    {
        public static string PartOne(string input)
        {
            return input.Integers().Sum(x => Math.Floor((double)x / 3.0) - 2).ToString();
        }

        public static string PartTwo(string input)
        {
            var modules = input.Integers();
            var total = 0.0;

            foreach (var m in modules)
            {
                var fuel = Math.Floor((double)m / 3.0) - 2;
                var moduleFuel = fuel;

                while (fuel > 0)
                {
                    fuel = Math.Floor(fuel / 3) - 2;
                    moduleFuel += fuel > 0 ? fuel : 0;
                }

                total += moduleFuel;
            }

            return total.ToString();
        }
    }
}