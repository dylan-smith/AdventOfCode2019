using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day04
    {
        private static int _start = 138241;
        private static int _count = 674034 - 138241 + 1;
        private static List<string> _passwords = Enumerable.Range(_start, _count).Select(x => x.ToString()).ToList();

        public static string PartOne(string input)
        {
            return _passwords.Count(x => CheckAdjacent(x) && CheckIncreasingDigits(x)).ToString();
        }

        public static string PartTwo(string input)
        {
            return _passwords.Count(x => CheckTwoAdjacent(x) && CheckIncreasingDigits(x)).ToString();
        }

        private static bool CheckAdjacent(string pwd)
        {
            return pwd.GroupBy(x => x).Any(g => g.Count() >= 2);
        }

        private static bool CheckTwoAdjacent(string pwd)
        {
            return pwd.GroupBy(x => x).Any(g => g.Count() == 2);
        }

        private static bool CheckIncreasingDigits(string pwd)
        {
            return pwd.OrderBy(c => c).SequenceEqual(pwd);
        }
    }
}