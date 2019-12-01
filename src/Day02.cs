using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day02
    {
        public static string PartOne(string input)
        {
            return (input.Lines().Count(x => ContainsLetterCount(x, 2)) * input.Lines().Count(x => ContainsLetterCount(x, 3))).ToString();
        }

        private static bool ContainsLetterCount(string box, int count)
        {
            return box.Distinct().Any(l => box.Count(c => c == l) == count);
        }

        public static string PartTwo(string input)
        {
            foreach (var c in input.Lines().ToList().GetCombinations(2))
            {
                if (c.First().Overlap(c.Last()).Length == c.First().Length - 1)
                {
                    return c.First().Overlap(c.Last());
                }
            }

            throw new Exception();
        }
    }
}