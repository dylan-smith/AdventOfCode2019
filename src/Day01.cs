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
            return input.Doubles().Sum(m =>
            {
                var total = 0.0;

                while (m > 0)
                {
                    m = Math.Floor(m / 3) - 2;
                    total += m > 0 ? m : 0;
                }

                return total;
            }).ToString();
        }
    }
}