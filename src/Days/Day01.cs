using System;
using System.Linq;

namespace AdventOfCode.Days
{
    [Day(2019, 1)]
    public class Day01 : BaseDay
    {
        public override string PartOne(string input)
        {
            return input.Doubles().Sum(x => Math.Floor(x / 3) - 2).ToString();
        }

        public override string PartTwo(string input)
        {
            return input.Doubles().Sum(m => CalcFuelForModule(m)).ToString();
        }

        private double CalcFuelForModule(double m)
        {
            var total = 0.0;

            while (m > 0)
            {
                m = Math.Floor(m / 3) - 2;
                total += m > 0 ? m : 0;
            }

            return total;
        }
    }
}
