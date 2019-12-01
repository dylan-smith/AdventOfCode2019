using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day05
    {
        public static string PartOne(string input)
        {
            return React(input.Trim()).Length.ToString();
        }

        private static string React(string polymer)
        {
            var activations = new List<string>();

            Enumerable.Range('a', 26).ForEach(c =>
            {
                activations.Add(((char)c).ToString() + ((char)c).ToString().ToUpper());
                activations.Add(((char)c).ToString().ToUpper() + ((char)c).ToString());
            });

            var startLength = polymer.Length;

            do
            {
                startLength = polymer.Length;
                activations.ForEach(a => polymer = polymer.Replace(a, string.Empty));
            }
            while (polymer.Length < startLength);

            return polymer;
        }

        private static string ReplaceType(string polymer, char t)
        {
            return polymer.Replace(((char)t).ToString(), string.Empty).Replace(((char)t).ToString().ToUpper(), string.Empty);
        }

        public static string PartTwo(string input)
        {
            return Enumerable.Range('a', 26)
                .Min(c => React(ReplaceType(input.Trim(), (char)c)).Length)
                .ToString();
        }
    }
}