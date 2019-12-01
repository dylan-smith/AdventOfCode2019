using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day08
    {
        public static string PartOne(string input)
        {
            return GetNextNode(new Stack<int>(input.Integers().Reverse())).GetMetaSum().ToString();
        }

        private static LicenseNode GetNextNode(Stack<int> numbers)
        {
            var childCount = numbers.Pop();
            var metaCount = numbers.Pop();

            var result = new LicenseNode();

            Enumerable.Range(0, childCount).ForEach(c => result.Children.Add(GetNextNode(numbers)));
            Enumerable.Range(0, metaCount).ForEach(m => result.MetaData.Add(numbers.Pop()));

            return result;
        }

        public static string PartTwo(string input)
        {
            return GetNextNode(new Stack<int>(input.Integers().Reverse())).GetValue().ToString();
        }
    }

    public class LicenseNode
    {
        public List<int> MetaData = new List<int>();
        public List<LicenseNode> Children = new List<LicenseNode>();

        public int GetValue()
        {
            if (Children.Count == 0)
            {
                return MetaData.Sum();
            }

            return MetaData.Where(m => m > 0 && m <= Children.Count).Sum(m => Children[m - 1].GetValue());
        }

        public int GetMetaSum()
        {
            return Children.Sum(c => c.GetMetaSum()) + MetaData.Sum();
        }
    }
}