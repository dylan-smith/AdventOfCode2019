using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day25
    {
        public static string PartOne(string input)
        {
            var constellations = input.Lines().Select(x => new List<Point4D>() { new Point4D(x) }).ToList();
            IEnumerable<List<Point4D>> match = null;

            do
            {
                foreach (var combo in constellations.GetCombinations(2))
                {
                    match = null;

                    for (var i = 0; i < combo.First().Count && match == null; i++)
                    {
                        for (var j = 0; j < combo.Last().Count && match == null; j++)
                        {
                            if (combo.First()[i].GetManhattanDistance(combo.Last()[j]) <= 3)
                            {
                                match = combo;
                            }
                        }
                    }

                    if (match != null)
                    {
                        break;
                    }
                }

                if (match != null)
                {
                    var newConstellation = new List<Point4D>();
                    newConstellation.AddRange(match.First());
                    newConstellation.AddRange(match.Last());
                    constellations.Remove(match.First());
                    constellations.Remove(match.Last());
                    constellations.Add(newConstellation);
                }
            }
            while (match != null);

            return constellations.Count.ToString();
        }

        public static string PartTwo(string input)
        {
            return string.Empty;
        }
    }
}
