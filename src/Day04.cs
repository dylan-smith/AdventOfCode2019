using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day04
    {
        public static string PartOne(string input)
        {
            var valid = new List<int>();

            for (var pwd = 138241; pwd <= 674034; pwd++)
            {
                if (CheckAdjacent(pwd) && CheckIncreasingDigits(pwd))
                {
                    valid.Add(pwd);
                }
            }

            return valid.Count.ToString();
        }

        private static bool CheckAdjacent(int pwd)
        {
            var text = pwd.ToString();
            var prev = text[0];
            text = text.ShaveLeft(1);

            foreach (var c in text)
            {
                if (c == prev)
                {
                    return true;
                }

                prev = c;
            }

            return false;
        }

        private static bool CheckIncreasingDigits(int pwd)
        {
            var text = pwd.ToString();
            var prev = text[0];

            foreach (var c in text)
            {
                if (c < prev)
                {
                    return false;
                }

                prev = c;
            }

            return true;
        }

        private static bool CheckTwoAdjacent(int pwd)
        {
            var text = pwd.ToString();

            for (var c = '0'; c <= '9'; c++)
            {
                if (text.Contains($"{c}{c}") && !text.Contains($"{c}{c}{c}"))
                {
                    return true;
                }
            }

            return false;
        }

        public static string PartTwo(string input)
        {
            var valid = new List<int>();

            for (var pwd = 138241; pwd <= 674034; pwd++)
            {
                if (CheckTwoAdjacent(pwd) && CheckIncreasingDigits(pwd))
                {
                    valid.Add(pwd);
                }
            }

            return valid.Count.ToString();
        }
    }
}