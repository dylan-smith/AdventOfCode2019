using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day04
    {
        public static string PartOne(string input)
        {
            var valid = new List<int>();

            for (var pwd = 138241; pwd <= 674034; pwd++)
            {
                if (CheckPassword(pwd))
                {
                    valid.Add(pwd);
                }
            }

            return valid.Count.ToString();
        }

        //private static bool CheckPassword(int pwd)
        //{
        //    var text = pwd.ToString();
        //    var prev = text[0];
        //    var repeat = false;

        //    for (var i = 1; i < text.Length; i++)
        //    {
        //        if (text[i] == prev)
        //        {
        //            repeat = true;
        //        }

        //        if (int.Parse(text[i].ToString()) < int.Parse(prev.ToString()))
        //        {
        //            return false;
        //        }

        //        prev = text[i];
        //    }

        //    return repeat;
        //}

        private static bool CheckPassword(int pwd)
        {
            var text = pwd.ToString();

            var prev = text[0];
            var repeat = false;

            for (var c = '0'; c <= '9'; c++)
            {
                if (text.Contains($"{c.ToString()}{c.ToString()}") && !text.Contains($"{c.ToString()}{c.ToString()}{c.ToString()}"))
                {
                    repeat = true;
                }
            }

            for (var i = 1; i < text.Length; i++)
            {
                if (int.Parse(text[i].ToString()) < int.Parse(prev.ToString()))
                {
                    return false;
                }

                prev = text[i];
            }

            return repeat;
        }

        public static string PartTwo(string input)
        {
            return "";
        }
    }
}