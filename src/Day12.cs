using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day12
    {
        public static string PartOne(string input)
        {
            var lastGeneration = (x: 0, state: input.Lines().First().Substring("initial state: ".Length));
            var rules = input.Lines().Skip(1).Where(x => x.EndsWith("#")).Select(x => x.Substring(0, 5)).ToList();
            var generations = 20;
            
            Enumerable.Range(0, generations).ForEach(g => lastGeneration = RunRules(lastGeneration, rules));

            return GetPotSum(lastGeneration.x, lastGeneration.state).ToString();
        }

        private static long GetPotSum(long x, string state)
        {
            return state.SelectWithIndex().Where(s => s.item == '#').Sum(s => s.index + x);
        }

        private static (int x, string state) RunRules((int x, string state) lastGeneration, List<string> rules)
        {
            var nextX = lastGeneration.x - 1;
            var nextState = new StringBuilder();

            lastGeneration.state = "..." + lastGeneration.state + "...";

            for (var i = 0; i < lastGeneration.state.Length - 4; i++)
            {
                var plantState = lastGeneration.state.Substring(i, 5);

                if (rules.Any(x => x == plantState))
                {
                    nextState.Append('#');
                }
                else
                {
                    nextState.Append(".");
                }
            }

            while (nextState[0] == '.')
            {
                nextState.Remove(0, 1);
                nextX++;
            }

            return (nextX, nextState.ToString().ShaveRight("."));
        }

        public static string PartTwo(string input)
        {
            var lastGeneration = (x: 0, state: input.Lines().First().Substring("initial state: ".Length));
            var rules = input.Lines().Skip(1).Where(x => x.EndsWith("#")).Select(x => x.Substring(0, 5)).ToList();
            var generations = 50000000000;

            for (var g = 1; g <= generations; g++)
            {
                var nextGeneration = RunRules(lastGeneration, rules);

                if (nextGeneration.state == lastGeneration.state)
                {
                    return GetPotSum((nextGeneration.x - lastGeneration.x) * (generations - g) + nextGeneration.x, nextGeneration.state).ToString();
                }

                lastGeneration = nextGeneration;
            }

            return GetPotSum(lastGeneration.x, lastGeneration.state).ToString();
        }
    }
}
